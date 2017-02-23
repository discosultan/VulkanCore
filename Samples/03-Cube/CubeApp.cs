using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using VulkanCore.Khr;

namespace VulkanCore.Samples.Cube
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WVP
    {
        public Matrix4x4 World;
        public Matrix4x4 View;
        public Matrix4x4 Projection;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Vertex
    {
        public Vector3 Position;
    }

    public class CubeApp : VulkanApp
    {
        private RenderPass _renderPass;
        private ImageView[] _imageViews;
        private Framebuffer[] _framebuffers;
        private PipelineLayout _pipelineLayout;
        private Pipeline _pipeline;
        private DescriptorSetLayout _descriptorSetLayout;
        private DescriptorPool _descriptorPool;
        private DescriptorSet _descriptorSet;

        private Buffer _uniformBuffer;
        private WVP _wvp;

        public CubeApp(IntPtr hInstance, IWindow window) : base(hInstance, window)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            CreateRenderPass();
            CreateFramebuffers();
            CreateUniformBuffers();
            CreateDescriptorSetAndPipelineLayouts();
            CreateDescriptorSet();
            CreateGraphicsPipeline();
            RecordCommandBuffers();

            _wvp.View = Matrix4x4.CreateLookAt(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY);
            _wvp.Projection = Matrix4x4.CreatePerspectiveFieldOfView(
                (float)Math.PI / 4, 
                Window.Width / Window.Height, 
                1.0f, 1000.0f);
        }

        protected override void Update(Timer timer)
        {
            _wvp.World = Matrix4x4.CreateRotationX((float)Math.Sin(timer.TotalTime));
        }

        private void CreateUniformBuffers()
        {
            _uniformBuffer = Device.CreateBuffer(new BufferCreateInfo(Interop.SizeOf<WVP>(), BufferUsages.UniformBuffer));
            MemoryRequirements memoryRequirements = _uniformBuffer.GetMemoryRequirements();
            // We require host visible memory so we can map it and write to it directly.
            // We require host coherent memory so that writes are visible to the GPU right after unmapping it.
            int memoryTypeIndex = PhysicalDeviceMemoryProperties.GetMemoryTypeIndex(
                memoryRequirements.MemoryTypeBits, 
                MemoryProperties.HostVisible | MemoryProperties.HostCoherent);
            DeviceMemory memory = Device.AllocateMemory(new MemoryAllocateInfo(memoryRequirements.Size, memoryTypeIndex));
            _uniformBuffer.BindMemory(memory);
        }

        private void CreateDescriptorSet()
        {
            _descriptorPool = Device.CreateDescriptorPool(
                new DescriptorPoolCreateInfo(1, new[] { new DescriptorPoolSize(DescriptorType.UniformBuffer, 1) }));

            _descriptorSet = _descriptorPool.AllocateSets(new DescriptorSetAllocateInfo(1, _descriptorSetLayout))[0];

            // Update the descriptor set for the shader binding point.
            var writeDescriptorSet = new WriteDescriptorSet(_descriptorSet, 0, 0, 1, DescriptorType.UniformBuffer,
                bufferInfo: new[] { new DescriptorBufferInfo(_uniformBuffer) });
            _descriptorPool.UpdateSets(new[] { writeDescriptorSet });
        }

        private void CreateDescriptorSetAndPipelineLayouts()
        {
            var layoutBinding = new DescriptorSetLayoutBinding(0, DescriptorType.UniformBuffer, 1, ShaderStages.Vertex);
            _descriptorSetLayout = Device.CreateDescriptorSetLayout(new DescriptorSetLayoutCreateInfo(layoutBinding));

            var layoutCreateInfo = new PipelineLayoutCreateInfo(new[] { _descriptorSetLayout });
            _pipelineLayout = Device.CreatePipelineLayout(layoutCreateInfo);
        }

        protected override void OnResized()
        {
            base.OnResized();
            _pipeline.Dispose();
            _pipelineLayout.Dispose();
            Array.ForEach(_framebuffers, framebuffer => framebuffer.Dispose());
            Array.ForEach(_imageViews, imageView => imageView.Dispose());
            CreateFramebuffers();
            CreateGraphicsPipeline();
            RecordCommandBuffers();
        }

        protected override void Draw(Timer timer)
        {
            // Acquire drawing image.
            int imageIndex = Swapchain.AcquireNextImage(semaphore: ImageAvailableSemaphore);

            GraphicsQueue.Submit(new SubmitInfo(
                new[] { ImageAvailableSemaphore },
                new[] { PipelineStages.ColorAttachmentOutput },
                new[] { CommandBuffers[imageIndex] },
                new[] { RenderingFinishedSemaphore }
            ));

            PresentQueue.PresentKhr(new PresentInfoKhr(
                new[] { RenderingFinishedSemaphore },
                new[] { Swapchain },
                new[] { imageIndex }
            ));
        }

        public override void Dispose()
        {
            _uniformBuffer.BackingMemory.Dispose();
            _uniformBuffer.Dispose();
            _descriptorPool.Dispose();
            _descriptorSetLayout.Dispose();
            _pipeline.Dispose();
            _pipelineLayout.Dispose();
            Array.ForEach(_framebuffers, framebuffer => framebuffer.Dispose());
            Array.ForEach(_imageViews, imageView => imageView.Dispose());
            _renderPass.Dispose();
            base.Dispose();
        }

        private void CreateRenderPass()
        {
            var subpasses = new[]
            {
                new SubpassDescription(new[] { new AttachmentReference(0, ImageLayout.ColorAttachmentOptimal) })
            };
            var attachments = new[]
            {
                new AttachmentDescription
                {
                    Samples = SampleCounts.Count1,
                    Format = Swapchain.Format,
                    InitialLayout = ImageLayout.Undefined,
                    FinalLayout = ImageLayout.PresentSrcKhr,
                    LoadOp = AttachmentLoadOp.Clear,
                    StoreOp = AttachmentStoreOp.Store,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare
                }
            };

            var createInfo = new RenderPassCreateInfo(subpasses, attachments);
            _renderPass = Device.CreateRenderPass(createInfo);
        }

        private void CreateFramebuffers()
        {
            _imageViews = new ImageView[SwapchainImages.Length];
            _framebuffers = new Framebuffer[SwapchainImages.Length];

            for (int i = 0; i < SwapchainImages.Length; i++)
            {
                _imageViews[i] = SwapchainImages[i].CreateView(new ImageViewCreateInfo(
                    Swapchain.Format,
                    new ImageSubresourceRange(ImageAspects.Color)));
                _framebuffers[i] = _renderPass.CreateFramebuffer(new FramebufferCreateInfo(
                    new[] { _imageViews[i] },
                    Window.Width,
                    Window.Height));
            }
        }

        private void CreateGraphicsPipeline()
        {
            // Create shader modules. Shader modules are one of the objects required to create the
            // graphics pipeline. But after the pipeline is created, we don't need these shader
            // modules anymore, so we dispose them.
            using (ShaderModule vertexShader = Device.CreateShaderModule(
                new ShaderModuleCreateInfo(File.ReadAllBytes("Shaders\\shader.vert.spv"))))
            using (ShaderModule fragmentShader = Device.CreateShaderModule(
                new ShaderModuleCreateInfo(File.ReadAllBytes("Shaders\\shader.frag.spv"))))
            {
                var shaderStageCreateInfos = new[]
                {
                    new PipelineShaderStageCreateInfo(ShaderStages.Vertex, vertexShader, "main"),
                    new PipelineShaderStageCreateInfo(ShaderStages.Fragment, fragmentShader, "main")
                };

                var vertexInputStateCreateInfo = new PipelineVertexInputStateCreateInfo();
                var inputAssemblyStateCreateInfo = new PipelineInputAssemblyStateCreateInfo
                {
                    Topology = PrimitiveTopology.TriangleList
                };
                var viewport = new Viewport(0, 0, Window.Width, Window.Height);
                var scissor = new Rect2D(Offset2D.Zero, new Extent2D(Window.Width, Window.Height));
                var viewportStateCreateInfo = new PipelineViewportStateCreateInfo
                {
                    Viewports = new[] { viewport },
                    Scissors = new[] { scissor }
                };
                var rasterizationStateCreateInfo = new PipelineRasterizationStateCreateInfo
                {
                    PolygonMode = PolygonMode.Fill,
                    CullMode = CullModes.Back,
                    FrontFace = FrontFace.CounterClockwise,
                    LineWidth = 1.0f
                };
                var multisampleStateCreateInfo = new PipelineMultisampleStateCreateInfo
                {
                    RasterizationSamples = SampleCounts.Count1,
                    MinSampleShading = 1.0f
                };
                var colorBlendAttachmentState = new PipelineColorBlendAttachmentState
                {
                    SrcColorBlendFactor = BlendFactor.One,
                    DstColorBlendFactor = BlendFactor.Zero,
                    ColorBlendOp = BlendOp.Add,
                    SrcAlphaBlendFactor = BlendFactor.One,
                    DstAlphaBlendFactor = BlendFactor.Zero,
                    AlphaBlendOp = BlendOp.Add,
                    ColorWriteMask = ColorComponents.All
                };
                var colorBlendStateCreateInfo = new PipelineColorBlendStateCreateInfo(
                    new[] { colorBlendAttachmentState });

                var pipelineCreateInfo = new GraphicsPipelineCreateInfo(
                    _pipelineLayout, _renderPass, 0,
                    shaderStageCreateInfos,
                    inputAssemblyStateCreateInfo,
                    vertexInputStateCreateInfo,
                    rasterizationStateCreateInfo,
                    viewportState: viewportStateCreateInfo,
                    multisampleState: multisampleStateCreateInfo,
                    colorBlendState: colorBlendStateCreateInfo);
                _pipeline = Device.CreateGraphicsPipeline(pipelineCreateInfo);
            }
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

                var renderPassBeginInfo = new RenderPassBeginInfo(
                    _framebuffers[i],
                    new Rect2D(Offset2D.Zero, new Extent2D(Window.Width, Window.Height)),
                    new ClearColorValue(new ColorF4(0.39f, 0.58f, 0.93f, 1.0f)));

                cmdBuffer.CmdBeginRenderPass(renderPassBeginInfo);
                cmdBuffer.CmdBindPipeline(PipelineBindPoint.Graphics, _pipeline);
                cmdBuffer.CmdDraw(3, 1);
                cmdBuffer.CmdEndRenderPass();

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
    }
}
