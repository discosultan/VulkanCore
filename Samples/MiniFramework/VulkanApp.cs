using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VulkanCore.Ext;
using VulkanCore.Khr;
using VulkanCore.Mvk;

namespace VulkanCore.Samples
{
    public enum Platform
    {
        Android, Win32, MacOS
    }

    public interface IVulkanAppHost : IDisposable
    {
        IntPtr WindowHandle { get; }
        IntPtr InstanceHandle { get; }
        int Width { get; }
        int Height { get; }
        Platform Platform { get; }

        Stream Open(string path);
    }

    public abstract class VulkanApp : IDisposable
    {
        private readonly Stack<IDisposable> _toDisposePermanent = new Stack<IDisposable>();
        private readonly Stack<IDisposable> _toDisposeFrame = new Stack<IDisposable>();
        private bool _initializingPermanent;

        public IVulkanAppHost Host { get; private set; }

        public Instance Instance { get; private set; }
        protected DebugReportCallbackExt DebugReportCallback { get; private set; }
        public VulkanContext Context { get; private set; }
        public ContentManager Content { get; private set; }

        protected SurfaceKhr Surface { get; private set; }
        protected SwapchainKhr Swapchain { get; private set; }
        protected Image[] SwapchainImages { get; private set; }
        protected CommandBuffer[] CommandBuffers { get; private set; }
        protected Fence[] SubmitFences { get; private set; }

        protected Semaphore ImageAvailableSemaphore { get; private set; }
        protected Semaphore RenderingFinishedSemaphore { get; private set; }

        public void Initialize(IVulkanAppHost host)
        {
            Host = host;
#if DEBUG
            const bool debug = true;
#else
            const bool debug = false;
#endif
            _initializingPermanent = true;
            // Calling ToDispose here registers the resource to be automatically disposed on exit.
            Instance                   = ToDispose(CreateInstance(debug));
            DebugReportCallback        = ToDispose(CreateDebugReportCallback(debug));
            Surface                    = ToDispose(CreateSurface());
            Context                    = ToDispose(new VulkanContext(Instance, Surface, Host.Platform));
            Content                    = ToDispose(new ContentManager(Host, Context, "Content"));
            ImageAvailableSemaphore    = ToDispose(Context.Device.CreateSemaphore());
            RenderingFinishedSemaphore = ToDispose(Context.Device.CreateSemaphore());

            if(host.Platform == Platform.MacOS)
            {
                //Setup MoltenVK specific device configuration.
                MVKDeviceConfiguration deviceConfig = Context.Device.GetMVKDeviceConfiguration();
                deviceConfig.DebugMode = debug;
                deviceConfig.PerformanceTracking = debug;
                deviceConfig.PerformanceLoggingFrameCount = debug ? 300 : 0;
                Context.Device.SetMVKDeviceConfiguration(deviceConfig);
            }

            _initializingPermanent = false;
            // Calling ToDispose here registers the resource to be automatically disposed on events
            // such as window resize.
            Swapchain = ToDispose(CreateSwapchain());
            // Acquire underlying images of the freshly created swapchain.
            SwapchainImages = Swapchain.GetImages();
            // Create a command buffer for each swapchain image.
            CommandBuffers = Context.GraphicsCommandPool.AllocateBuffers(
                new CommandBufferAllocateInfo(CommandBufferLevel.Primary, SwapchainImages.Length));
            // Create a fence for each commandbuffer so that we can wait before using it again
            _initializingPermanent = true; //We need our fences to be there permanently
            SubmitFences = new Fence[SwapchainImages.Length];
            for (int i = 0; i < SubmitFences.Length; i++)
                ToDispose(SubmitFences[i] = Context.Device.CreateFence(new FenceCreateInfo(FenceCreateFlags.Signaled))); 

            // Allow concrete samples to initialize their resources.
            InitializePermanent();
            _initializingPermanent = false;
            InitializeFrame();

            // Record commands for execution by Vulkan.
            RecordCommandBuffers();
        }

        /// <summary>
        /// Allows derived classes to initializes resources the will stay alive for the duration of
        /// the application.
        /// </summary>
        protected virtual void InitializePermanent() { }
        
        /// <summary>
        /// Allows derived classes to initializes resources that need to be recreated on events such
        /// as window resize.
        /// </summary>
        protected virtual void InitializeFrame() { }

        public void Resize()
        {
            Context.Device.WaitIdle();

            // Dispose all frame dependent resources.
            while (_toDisposeFrame.Count > 0)
                _toDisposeFrame.Pop().Dispose();

            // Reset all the command buffers allocated from the pools.
            Context.GraphicsCommandPool.Reset();
            Context.ComputeCommandPool.Reset();

            // Reinitialize frame dependent resources.
            Swapchain = ToDispose(CreateSwapchain());
            SwapchainImages = Swapchain.GetImages();
            InitializeFrame();

            // Re-record command buffers.
            RecordCommandBuffers();
        }

        public void Tick(Timer timer)
        {
            Update(timer);
            Draw(timer);
        }

        protected virtual void Update(Timer timer) { }

        protected virtual void Draw(Timer timer)
        {
            // Acquire an index of drawing image for this frame.
            int imageIndex = Swapchain.AcquireNextImage(semaphore: ImageAvailableSemaphore);

            // Use a fence to wait until the command buffer has finished execution before using it again
            SubmitFences[imageIndex].Wait();
            SubmitFences[imageIndex].Reset();

            // Submit recorded commands to graphics queue for execution.
            Context.GraphicsQueue.Submit(
                ImageAvailableSemaphore,
                PipelineStages.ColorAttachmentOutput,
                CommandBuffers[imageIndex],
                RenderingFinishedSemaphore,
                SubmitFences[imageIndex]
            );

            // Present the color output to screen.
            Context.PresentQueue.PresentKhr(RenderingFinishedSemaphore, Swapchain, imageIndex);
        }

        public virtual void Dispose()
        {
            Context.Device.WaitIdle();
            while (_toDisposeFrame.Count > 0)
                _toDisposeFrame.Pop().Dispose();
            while (_toDisposePermanent.Count > 0)
                _toDisposePermanent.Pop().Dispose();
        }

        private Instance CreateInstance(bool debug)
        {
            // Specify standard validation layers.
            string surfaceExtension;
            switch (Host.Platform)
            {
                case Platform.Android:
                    surfaceExtension = Constant.InstanceExtension.KhrAndroidSurface;
                    break;
                case Platform.Win32:
                    surfaceExtension = Constant.InstanceExtension.KhrWin32Surface;
                    break;
                case Platform.MacOS:
                    surfaceExtension = Constant.InstanceExtension.MvkMacOSSurface;
                    break;
                default:
                    throw new NotImplementedException();
            }

            var createInfo = new InstanceCreateInfo();

            //Currently MoltenVK (used for MacOS) doesn't support the debug layer.
            if (debug && Host.Platform != Platform.MacOS)
            {
                var availableLayers = Instance.EnumerateLayerProperties();
                createInfo.EnabledLayerNames = new[] { Constant.InstanceLayer.LunarGStandardValidation }
                    .Where(availableLayers.Contains)
                    .ToArray();
                createInfo.EnabledExtensionNames = new[]
                {
                    Constant.InstanceExtension.KhrSurface,
                    surfaceExtension,
                    Constant.InstanceExtension.ExtDebugReport
                };
            }
            else
            {
                createInfo.EnabledExtensionNames = new[]
                {
                    Constant.InstanceExtension.KhrSurface,
                    surfaceExtension,
                };
            }
            return new Instance(createInfo);
        }

        private DebugReportCallbackExt CreateDebugReportCallback(bool debug)
        {
            //Currently MoltenVK (used for MacOS) doesn't support the debug layer.
            if (!debug || Host.Platform == Platform.MacOS) return null;

            // Attach debug callback.
            var debugReportCreateInfo = new DebugReportCallbackCreateInfoExt(
                DebugReportFlagsExt.All,
                args =>
                {
                    Debug.WriteLine($"[{args.Flags}][{args.LayerPrefix}] {args.Message}");
                    return args.Flags.HasFlag(DebugReportFlagsExt.Error);
                }
            );
            return Instance.CreateDebugReportCallbackExt(debugReportCreateInfo);
        }

        private SurfaceKhr CreateSurface()
        {
            // Create surface.
            switch (Host.Platform)
            {
                case Platform.Android:
                    return Instance.CreateAndroidSurfaceKhr(new AndroidSurfaceCreateInfoKhr(Host.WindowHandle));
                case Platform.Win32:
                    return Instance.CreateWin32SurfaceKhr(new Win32SurfaceCreateInfoKhr(Host.InstanceHandle, Host.WindowHandle));
                case Platform.MacOS:
                    return Instance.CreateMacOSSurfaceMvk(new MacOSSurfaceCreateInfoMvk(Host.WindowHandle));
                default:
                    throw new NotImplementedException();
            }
        }

        private SwapchainKhr CreateSwapchain()
        {
            SurfaceCapabilitiesKhr capabilities = Context.PhysicalDevice.GetSurfaceCapabilitiesKhr(Surface);
            SurfaceFormatKhr[] formats = Context.PhysicalDevice.GetSurfaceFormatsKhr(Surface);
            PresentModeKhr[] presentModes = Context.PhysicalDevice.GetSurfacePresentModesKhr(Surface);
            Format format = formats.Length == 1 && formats[0].Format == Format.Undefined
                ? Format.B8G8R8A8UNorm
                : formats[0].Format;
            PresentModeKhr presentMode =
                presentModes.Contains(PresentModeKhr.Mailbox) ? PresentModeKhr.Mailbox :
                presentModes.Contains(PresentModeKhr.FifoRelaxed) ? PresentModeKhr.FifoRelaxed :
                presentModes.Contains(PresentModeKhr.Fifo) ? PresentModeKhr.Fifo :
                PresentModeKhr.Immediate;

            return Context.Device.CreateSwapchainKhr(new SwapchainCreateInfoKhr(
                surface: Surface,
                imageFormat: format,
                imageExtent: capabilities.CurrentExtent,
                preTransform: capabilities.CurrentTransform,
                presentMode: presentMode));
        }

        private void RecordCommandBuffers()
        {
            var subresourceRange = new ImageSubresourceRange(ImageAspects.Color, 0, 1, 0, 1);
            for (int i = 0; i < CommandBuffers.Length; i++)
            {
                CommandBuffer cmdBuffer = CommandBuffers[i];
                cmdBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.SimultaneousUse));

                if (Context.PresentQueue != Context.GraphicsQueue)
                {
                    var barrierFromPresentToDraw = new ImageMemoryBarrier(
                        SwapchainImages[i], subresourceRange,
                        Accesses.MemoryRead, Accesses.ColorAttachmentWrite,
                        ImageLayout.Undefined, ImageLayout.PresentSrcKhr,
                        Context.PresentQueue.FamilyIndex, Context.GraphicsQueue.FamilyIndex);

                    cmdBuffer.CmdPipelineBarrier(
                        PipelineStages.ColorAttachmentOutput,
                        PipelineStages.ColorAttachmentOutput,
                        imageMemoryBarriers: new[] { barrierFromPresentToDraw });
                }

                RecordCommandBuffer(cmdBuffer, i);

                if (Context.PresentQueue != Context.GraphicsQueue)
                {
                    var barrierFromDrawToPresent = new ImageMemoryBarrier(
                        SwapchainImages[i], subresourceRange,
                        Accesses.ColorAttachmentWrite, Accesses.MemoryRead,
                        ImageLayout.PresentSrcKhr, ImageLayout.PresentSrcKhr,
                        Context.GraphicsQueue.FamilyIndex, Context.PresentQueue.FamilyIndex);

                    cmdBuffer.CmdPipelineBarrier(
                        PipelineStages.ColorAttachmentOutput,
                        PipelineStages.BottomOfPipe,
                        imageMemoryBarriers: new[] { barrierFromDrawToPresent });
                }

                cmdBuffer.End();
            } 
        }
        
        protected abstract void RecordCommandBuffer(CommandBuffer cmdBuffer, int imageIndex);

        protected T ToDispose<T>(T disposable)
        {
            var toDispose = _initializingPermanent ? _toDisposePermanent : _toDisposeFrame;
            switch (disposable)
            {
                case IEnumerable<IDisposable> sequence:
                    foreach (var element in sequence)
                        toDispose.Push(element);
                    break;
                case IDisposable element:
                    toDispose.Push(element);
                    break;
            }
            return disposable;
        }
    }
}
