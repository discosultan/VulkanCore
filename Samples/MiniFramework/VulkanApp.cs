using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VulkanCore.Ext;
using VulkanCore.Khr;

namespace VulkanCore.Samples
{
    public abstract class VulkanApp : IDisposable
    {
        private readonly Stack<IDisposable> _toDisposePermanent = new Stack<IDisposable>();
        private readonly Stack<IDisposable> _toDisposeFrame = new Stack<IDisposable>();
        private readonly IntPtr _hInstance;
        private bool _initializingPermanent;

        protected VulkanApp(IntPtr hInstance, IWindow window)
        {
            _hInstance = hInstance;
            Window = window;
        }

        public IWindow Window { get; }

        public Instance Instance { get; private set; }
        protected DebugReportCallbackExt DebugReportCallback { get; private set; }
        // Encapsulates physical and logical device.
        public GraphicsDevice Device { get; private set; }
        public ContentManager Content { get; private set; }

        protected SurfaceKhr Surface { get; private set; }
        protected SwapchainKhr Swapchain { get; private set; }
        protected Image[] SwapchainImages { get; private set; }
        protected CommandBuffer[] CommandBuffers { get; private set; }

        protected Semaphore ImageAvailableSemaphore { get; private set; }
        protected Semaphore RenderingFinishedSemaphore { get; private set; }

        public void Initialize()
        {
            // Initialize the host window.
            Window.Initialize(Rezize);

            // Initialize necessary Vulkan resources for application.
            InitializePermanent();
            InitializeFrame();

            // Record commands for execution by Vulkan.
            RecordCommandBuffers();
        }

        /// <summary>
        /// Initializes resources the will stay alive for the duration of the application.
        /// </summary>
        protected virtual void InitializePermanent()
        {
#if DEBUG
            bool debug = true;
#else
            bool debug = false;
#endif
            _initializingPermanent = true;

            // Calling ToDispose in this method registers the resource to be automatically disposed on exit.
            Instance =                   ToDispose(CreateInstance(debug));
            DebugReportCallback =        ToDispose(CreateDebugReportCallback(debug));
            Surface =                    ToDispose(CreateSurface());
            Device =                     ToDispose(new GraphicsDevice(Instance, Surface, Window.Platform));
            Content =                    ToDispose(new ContentManager("Content"));
            ImageAvailableSemaphore =    ToDispose(Device.Logical.CreateSemaphore());
            RenderingFinishedSemaphore = ToDispose(Device.Logical.CreateSemaphore());
        }

        /// <summary>
        /// Initializes resources that need to be recreated on events such as window resize.
        /// </summary>
        protected virtual void InitializeFrame()
        {
            _initializingPermanent = false;

            // Calling ToDispose in this method registers the resource to be automatically disposed
            // on events such as window resize.
            Swapchain = ToDispose(CreateSwapchain());

            // Acquire underlying images of the freshly created swapchain.
            SwapchainImages = Swapchain.GetImages();

            // Create a command buffer for each swapchain image.
            CommandBuffers = Device.CommandPool.AllocateBuffers(
                new CommandBufferAllocateInfo(CommandBufferLevel.Primary, SwapchainImages.Length));
        }

        private void Rezize()
        {
            Device.Logical.WaitIdle();

            // Dispose all frame dependent resources.
            while (_toDisposeFrame.Count > 0)
                _toDisposeFrame.Pop().Dispose();

            // Reinitialize frame dependent resources.
            InitializeFrame();

            // Reset all the command buffers allocated from the pool and re-record them.
            Device.CommandPool.Reset();
            RecordCommandBuffers();
        }

        public virtual void Run() => Window.Run(Tick);

        private void Tick(Timer timer)
        {
            Update(timer);
            Draw(timer);
        }

        protected virtual void Update(Timer timer) { }

        protected virtual void Draw(Timer timer)
        {
            // Acquire an index of drawing image for this frame.
            int imageIndex = Swapchain.AcquireNextImage(semaphore: ImageAvailableSemaphore);

            // Submit recorded commands to graphics queue for execution.
            Device.GraphicsQueue.Submit(
                ImageAvailableSemaphore,
                PipelineStages.ColorAttachmentOutput,
                CommandBuffers[imageIndex],
                RenderingFinishedSemaphore
            );

            // Present the color output to screen.
            Device.PresentQueue.PresentKhr(RenderingFinishedSemaphore, Swapchain, imageIndex);
        }

        public virtual void Dispose()
        {
            Device.Logical.WaitIdle();
            while (_toDisposeFrame.Count > 0)
                _toDisposeFrame.Pop().Dispose();
            while (_toDisposePermanent.Count > 0)
                _toDisposePermanent.Pop().Dispose();
        }

        private Instance CreateInstance(bool debug)
        {
            // Specify standard validation layers.
            string surfaceExtension;
            switch (Window.Platform)
            {
                case Platform.Android:
                    surfaceExtension = Constant.InstanceExtension.KhrAndroidSurface;
                    break;
                case Platform.Win32:
                    surfaceExtension = Constant.InstanceExtension.KhrWin32Surface;
                    break;
                default:
                    throw new NotImplementedException();
            }

            var createInfo = new InstanceCreateInfo();
            if (debug)
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
            if (!debug) return null;

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
            switch (Window.Platform)
            {
                case Platform.Android:
                    return Instance.CreateAndroidSurfaceKhr(new AndroidSurfaceCreateInfoKhr(Window.Handle));
                case Platform.Win32:
                    return Instance.CreateWin32SurfaceKhr(new Win32SurfaceCreateInfoKhr(_hInstance, Window.Handle));
                default:
                    throw new NotImplementedException();
            }
        }

        private SwapchainKhr CreateSwapchain()
        {
            SurfaceCapabilitiesKhr capabilities = Device.Physical.GetSurfaceCapabilitiesKhr(Surface);
            SurfaceFormatKhr[] formats = Device.Physical.GetSurfaceFormatsKhr(Surface);
            PresentModeKhr[] presentModes = Device.Physical.GetSurfacePresentModesKhr(Surface);
            Format format = formats.Length == 1 && formats[0].Format == Format.Undefined
                ? Format.B8G8R8A8UNorm
                : formats[0].Format;
            PresentModeKhr presentMode =
                presentModes.Contains(PresentModeKhr.Mailbox) ? PresentModeKhr.Mailbox :
                presentModes.Contains(PresentModeKhr.FifoRelaxed) ? PresentModeKhr.FifoRelaxed :
                presentModes.Contains(PresentModeKhr.Fifo) ? PresentModeKhr.Fifo :
                PresentModeKhr.Immediate;

            return Device.Logical.CreateSwapchainKhr(new SwapchainCreateInfoKhr(
                Surface,
                format,
                capabilities.CurrentExtent,
                capabilities.CurrentTransform,
                presentMode));
        }

        private void RecordCommandBuffers()
        {
            var subresourceRange = new ImageSubresourceRange(ImageAspects.Color, 0, 1, 0, 1);
            for (int i = 0; i < CommandBuffers.Length; i++)
            {
                CommandBuffer cmdBuffer = CommandBuffers[i];
                cmdBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.SimultaneousUse));

                if (Device.PresentQueue != Device.GraphicsQueue)
                {
                    var barrierFromPresentToDraw = new ImageMemoryBarrier(
                        SwapchainImages[i], subresourceRange,
                        Accesses.MemoryRead, Accesses.ColorAttachmentWrite,
                        ImageLayout.Undefined, ImageLayout.PresentSrcKhr,
                        Device.PresentQueue.FamilyIndex, Device.GraphicsQueue.FamilyIndex);

                    cmdBuffer.CmdPipelineBarrier(
                        PipelineStages.ColorAttachmentOutput,
                        PipelineStages.ColorAttachmentOutput,
                        imageMemoryBarriers: new[] { barrierFromPresentToDraw });
                }

                RecordCommandBuffer(cmdBuffer, i);

                if (Device.PresentQueue != Device.GraphicsQueue)
                {
                    var barrierFromDrawToPresent = new ImageMemoryBarrier(
                        SwapchainImages[i], subresourceRange,
                        Accesses.ColorAttachmentWrite, Accesses.MemoryRead,
                        ImageLayout.PresentSrcKhr, ImageLayout.PresentSrcKhr,
                        Device.GraphicsQueue.FamilyIndex, Device.PresentQueue.FamilyIndex);

                    cmdBuffer.CmdPipelineBarrier(
                        PipelineStages.ColorAttachmentOutput,
                        PipelineStages.BottomOfPipe,
                        imageMemoryBarriers: new[] { barrierFromDrawToPresent });
                }

                cmdBuffer.End();
            } 
        }
        
        protected abstract void RecordCommandBuffer(CommandBuffer cmdBuffer, int imageIndex);

        protected T ToDispose<T>(T disposable) where T : IDisposable
        {
            var toDispose = _initializingPermanent ? _toDisposePermanent : _toDisposeFrame;
            toDispose.Push(disposable);
            return disposable;
        }

        protected T[] ToDispose<T>(T[] disposables) where T : IDisposable
        {
            var toDispose = _initializingPermanent ? _toDisposePermanent : _toDisposeFrame;
            foreach (T disposable in disposables)
                toDispose.Push(disposable);
            return disposables;
        }
    }
}
