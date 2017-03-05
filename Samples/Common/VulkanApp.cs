using System;
using System.Diagnostics;
using System.Linq;
using VulkanCore.Ext;
using VulkanCore.Khr;

namespace VulkanCore.Samples
{
    public abstract class VulkanApp : IDisposable
    {
        private readonly IntPtr _hInstance;

        private DebugReportCallbackExt _debugCallback;

        protected VulkanApp(IntPtr hInstance, IWindow window)
        {
            _hInstance = hInstance;
            Window = window;
        }

        protected IWindow Window { get; }

        public Instance Instance { get; private set; }
        public PhysicalDevice PhysicalDevice { get; private set; }
        public PhysicalDeviceMemoryProperties PhysicalDeviceMemoryProperties { get; private set; }
        public Device Device { get; private set; }

        protected SurfaceKhr Surface { get; private set; }
        protected SwapchainKhr Swapchain { get; private set; }
        protected Image[] SwapchainImages { get; private set; }

        public Queue GraphicsQueue { get; private set; }
        public Queue PresentQueue { get; private set; }
        public CommandPool CommandPool { get; private set; }
        protected CommandBuffer[] CommandBuffers { get; private set; }
        protected Semaphore ImageAvailableSemaphore { get; private set; }
        protected Semaphore RenderingFinishedSemaphore { get; private set; }

        public void Initialize()
        {
            Window.Initialize(Rezize);

            CreateInstanceAndSurface();
            CreateDeviceAndGetQueues();
            CreateSwapchain();
            CreateSemaphoresAndCommandBuffers();

            OnInitialized();

            RecordCommandBuffers();
        }

        protected virtual void OnInitialized()
        {
        }

        private void Rezize()
        {
            Device.WaitIdle();
            Swapchain.Dispose();
            CreateSwapchain();

            OnResized();

            CommandPool.Reset(); // Resets all the command buffers allocated from the pool.
            RecordCommandBuffers();
        }

        protected virtual void OnResized()
        {
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
            GraphicsQueue.Submit(
                ImageAvailableSemaphore,
                PipelineStages.ColorAttachmentOutput,
                CommandBuffers[imageIndex],
                RenderingFinishedSemaphore
            );

            // Present the color output to screen.
            PresentQueue.PresentKhr(RenderingFinishedSemaphore, Swapchain, imageIndex);
        }

        public virtual void Dispose()
        {
            Device.WaitIdle();
            ImageAvailableSemaphore.Dispose();
            RenderingFinishedSemaphore.Dispose();
            CommandPool.Dispose();
            Swapchain.Dispose();
            Device.Dispose();
            Surface.Dispose();
            _debugCallback?.Dispose();
            Instance.Dispose();
        }

        private void CreateInstanceAndSurface()
        {
            // Specify standard validation layers.
            Instance = new Instance(new InstanceCreateInfo(
#if DEBUG
                enabledLayerNames: new[] { Constant.InstanceLayer.LunarGStandardValidation },
#endif
                enabledExtensionNames: new[] 
                {
                    Constant.InstanceExtension.KhrSurface,
                    Constant.InstanceExtension.KhrWin32Surface,
#if DEBUG
                    Constant.InstanceExtension.ExtDebugReport
#endif
                }
            ));

#if DEBUG
            // Attach debug callback.
            var debugReportCreateInfo = new DebugReportCallbackCreateInfoExt(
                DebugReportFlagsExt.All,
                args =>
                {
                    Trace.WriteLine($"[{args.Flags}][{args.LayerPrefix}] {args.Message}");
                    return args.Flags.HasFlag(DebugReportFlagsExt.Error);
                }
            );
            _debugCallback = Instance.CreateDebugReportCallbackExt(debugReportCreateInfo);
#endif

            // Create surface.
            Surface = Instance.CreateWin32SurfaceKhr( // TODO: x-plat
                new Win32SurfaceCreateInfoKhr(_hInstance, Window.Handle));
        }

        private void CreateDeviceAndGetQueues()
        {
            // Get physical device.
            int graphicsQueueFamilyIndex = -1;
            int presentQueueFamilyIndex = -1;
            foreach (PhysicalDevice physicalDevice in Instance.EnumeratePhysicalDevices())
            {
                QueueFamilyProperties[] queueFamilyProperties = physicalDevice.GetQueueFamilyProperties();
                for (int i = 0; i < queueFamilyProperties.Length; i++)
                {
                    if (queueFamilyProperties[i].QueueFlags.HasFlag(Queues.Graphics))
                    {
                        if (graphicsQueueFamilyIndex == -1) graphicsQueueFamilyIndex = i;

                        if (physicalDevice.GetSurfaceSupportKhr(i, Surface) &&
                            physicalDevice.GetWin32PresentationSupportKhr(i)) // TODO: x-plat
                        {
                            presentQueueFamilyIndex = i;
                            PhysicalDevice = physicalDevice;
                            break;
                        }
                    }
                }
                if (PhysicalDevice != null) break;
            }

            if (PhysicalDevice == null)
                throw new ApplicationException("No suitable physical device found.");

            // Store memory properties of the physical device.
            PhysicalDeviceMemoryProperties = PhysicalDevice.GetMemoryProperties();

            // Create device.
            bool sameGraphicsAndPresent = graphicsQueueFamilyIndex == presentQueueFamilyIndex;
            var queueCreateInfos = new DeviceQueueCreateInfo[sameGraphicsAndPresent ? 1 : 2];
            queueCreateInfos[0] = new DeviceQueueCreateInfo(graphicsQueueFamilyIndex, 1, 1.0f);
            if (!sameGraphicsAndPresent)
                queueCreateInfos[1] = new DeviceQueueCreateInfo(presentQueueFamilyIndex, 1, 1.0f);

            var deviceCreateInfo = new DeviceCreateInfo(
                queueCreateInfos,
                new[] { Constant.DeviceExtension.KhrSwapchain });
            Device = PhysicalDevice.CreateDevice(deviceCreateInfo);

            // Get queue.
            GraphicsQueue = Device.GetQueue(graphicsQueueFamilyIndex);
            PresentQueue = presentQueueFamilyIndex == graphicsQueueFamilyIndex 
                ? GraphicsQueue 
                : Device.GetQueue(presentQueueFamilyIndex);
        }

        private void CreateSwapchain()
        {
            SurfaceCapabilitiesKhr capabilities = PhysicalDevice.GetSurfaceCapabilitiesKhr(Surface);
            SurfaceFormatKhr[] formats = PhysicalDevice.GetSurfaceFormatsKhr(Surface);
            PresentModeKhr[] presentModes = PhysicalDevice.GetSurfacePresentModesKhr(Surface);
            Format format = formats.Length == 1 && formats[0].Format == Format.Undefined
                ? Format.B8G8R8A8UNorm
                : formats[0].Format;
            PresentModeKhr presentMode =
                presentModes.Contains(PresentModeKhr.Mailbox) ? PresentModeKhr.Mailbox :
                presentModes.Contains(PresentModeKhr.FifoRelaxed) ? PresentModeKhr.FifoRelaxed :
                presentModes.Contains(PresentModeKhr.Fifo) ? PresentModeKhr.Fifo :
                PresentModeKhr.Immediate;

            Swapchain = Device.CreateSwapchainKhr(new SwapchainCreateInfoKhr(
                Surface,
                format,
                capabilities.CurrentExtent,
                capabilities.CurrentTransform,
                presentMode));
            SwapchainImages = Swapchain.GetImages();
        }

        private void CreateSemaphoresAndCommandBuffers()
        {
            // Create semaphores.
            ImageAvailableSemaphore = Device.CreateSemaphore();
            RenderingFinishedSemaphore = Device.CreateSemaphore();

            // Create command buffers.
            CommandPool = Device.CreateCommandPool(new CommandPoolCreateInfo(GraphicsQueue.FamilyIndex));
            CommandBuffers = CommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, SwapchainImages.Length));
        }

        private void RecordCommandBuffers()
        {
            var subresourceRange = new ImageSubresourceRange(ImageAspects.Color);
            for (int i = 0; i < CommandBuffers.Length; i++)
            {
                CommandBuffer cmdBuffer = CommandBuffers[i];
                cmdBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.SimultaneousUse));

                if (PresentQueue != GraphicsQueue)
                {
                    var barrierFromPresentToDraw = new ImageMemoryBarrier(
                        SwapchainImages[i], subresourceRange,
                        Accesses.MemoryRead, Accesses.ColorAttachmentWrite,
                        ImageLayout.Undefined, ImageLayout.PresentSrcKhr,
                        PresentQueue.FamilyIndex, GraphicsQueue.FamilyIndex);

                    cmdBuffer.CmdPipelineBarrier(
                        PipelineStages.ColorAttachmentOutput,
                        PipelineStages.ColorAttachmentOutput,
                        imageMemoryBarriers: new[] { barrierFromPresentToDraw });
                }

                RecordCommandBuffer(cmdBuffer, i);

                if (PresentQueue != GraphicsQueue)
                {
                    var barrierFromDrawToPresent = new ImageMemoryBarrier(
                        SwapchainImages[i], subresourceRange,
                        Accesses.ColorAttachmentWrite, Accesses.MemoryRead,
                        ImageLayout.PresentSrcKhr, ImageLayout.PresentSrcKhr,
                        GraphicsQueue.FamilyIndex, PresentQueue.FamilyIndex);

                    cmdBuffer.CmdPipelineBarrier(
                        PipelineStages.ColorAttachmentOutput,
                        PipelineStages.BottomOfPipe,
                        imageMemoryBarriers: new[] { barrierFromDrawToPresent });
                }

                cmdBuffer.End();
            }
        }
        
        protected abstract void RecordCommandBuffer(CommandBuffer cmdBuffer, int imageIndex);
    }
}
