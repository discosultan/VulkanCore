using System;
using System.IO;

namespace VulkanCore.Samples.Triangle
{
    public class TriangleApp : VulkanApp
    {
        private RenderPass _renderPass;
        private ImageView[] _imageViews;
        private Framebuffer[] _framebuffers;
        private PipelineLayout _pipelineLayout;
        private Pipeline _pipeline;

        public TriangleApp(IntPtr hInstance, IWindow window) : base(hInstance, window)
        {
        }

        protected override void OnInitialized()
        {
            CreateRenderPass();
            CreateFramebuffers();
            CreatePipelineLayout();
            CreateGraphicsPipeline();
        }

        protected override void OnResized()
        {
            _pipeline.Dispose();
            Array.ForEach(_framebuffers, framebuffer => framebuffer.Dispose());
            Array.ForEach(_imageViews, imageView => imageView.Dispose());
            CreateFramebuffers();
            CreateGraphicsPipeline();
        }

        public override void Dispose()
        {
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

        private void CreatePipelineLayout()
        {
            var layoutCreateInfo = new PipelineLayoutCreateInfo();
            _pipelineLayout = Device.CreatePipelineLayout(layoutCreateInfo);
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
                var viewport = new Viewport(0, 0, Window.Width, Window.Height) ;
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

        protected override void RecordCommandBuffer(CommandBuffer cmdBuffer, int imageIndex)
        {
            var renderPassBeginInfo = new RenderPassBeginInfo(
                _framebuffers[imageIndex],
                new Rect2D(Offset2D.Zero, new Extent2D(Window.Width, Window.Height)),
                new ClearColorValue(new ColorF4(0.39f, 0.58f, 0.93f, 1.0f)));

            cmdBuffer.CmdBeginRenderPass(renderPassBeginInfo);
            cmdBuffer.CmdBindPipeline(PipelineBindPoint.Graphics, _pipeline);
            cmdBuffer.CmdDraw(3);
            cmdBuffer.CmdEndRenderPass();
        }
    }
}
