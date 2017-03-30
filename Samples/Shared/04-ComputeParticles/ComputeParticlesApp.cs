namespace VulkanCore.Samples.ComputeParticles
{
    public class ComputeParticlesApp : VulkanApp
    {
        private RenderPass _renderPass;
        private ImageView[] _imageViews;
        private Framebuffer[] _framebuffers;
        private DescriptorPool _descriptorPool;
        private DescriptorSet _descriptorSet;

        private DepthStencilBuffer _depthStencilBuffer;

        private DescriptorSetLayout _graphicsDescriptorSetLayout;
        private PipelineLayout _graphicsPipelineLayout;
        private Pipeline _graphicsPipeline;

        private DescriptorSetLayout _computeDescriptorSetLayout;
        private PipelineLayout _computePipelineLayout;
        private Pipeline _computePipeline;

        protected override void InitializePermanent()
        {
            _graphicsDescriptorSetLayout = ToDispose(CreateGraphicsDescriptorSetLayout());
            _graphicsPipelineLayout = ToDispose(CreateGraphicsPipelineLayout());

            _computeDescriptorSetLayout = ToDispose(CreateComputeDescriptorSetLayout());
            _computePipelineLayout = ToDispose(CreateComputePipelineLayout());
        }

        protected override void InitializeFrame()
        {
            _depthStencilBuffer = ToDispose(new DepthStencilBuffer(Device, Host.Width, Host.Height));
            _renderPass = ToDispose(CreateRenderPass());
            _imageViews = ToDispose(CreateImageViews());
            _framebuffers = ToDispose(CreateFramebuffers());

            _graphicsPipeline = ToDispose(CreateGraphicsPipeline());

            _computePipeline = ToDispose(CreateComputePipeline());
        }

        protected override void RecordCommandBuffer(CommandBuffer cmdBuffer, int imageIndex)
        {
            throw new System.NotImplementedException();
        }

        private RenderPass CreateRenderPass()
        {
            var attachments = new[]
            {
                // Color attachment.
                new AttachmentDescription
                {
                    Format = Swapchain.Format,
                    Samples = SampleCounts.Count1,
                    LoadOp = AttachmentLoadOp.Clear,
                    StoreOp = AttachmentStoreOp.Store,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare,
                    InitialLayout = ImageLayout.Undefined,
                    FinalLayout = ImageLayout.PresentSrcKhr
                },
                // Depth attachment.
                new AttachmentDescription
                {
                    Format = _depthStencilBuffer.Format,
                    Samples = SampleCounts.Count1,
                    LoadOp = AttachmentLoadOp.Clear,
                    StoreOp = AttachmentStoreOp.DontCare,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare,
                    InitialLayout = ImageLayout.Undefined,
                    FinalLayout = ImageLayout.DepthStencilAttachmentOptimal
                }
            };
            var subpasses = new[]
            {
                new SubpassDescription(
                    new[] { new AttachmentReference(0, ImageLayout.ColorAttachmentOptimal) },
                    new AttachmentReference(1, ImageLayout.DepthStencilAttachmentOptimal))
            };
            var dependencies = new[]
            {
                new SubpassDependency
                {
                    SrcSubpass = Constant.SubpassExternal,
                    DstSubpass = 0,
                    SrcStageMask = PipelineStages.BottomOfPipe,
                    DstStageMask = PipelineStages.ColorAttachmentOutput,
                    SrcAccessMask = Accesses.MemoryRead,
                    DstAccessMask = Accesses.ColorAttachmentRead | Accesses.ColorAttachmentWrite,
                    DependencyFlags = Dependencies.ByRegion
                },
                new SubpassDependency
                {
                    SrcSubpass = 0,
                    DstSubpass = Constant.SubpassExternal,
                    SrcStageMask = PipelineStages.ColorAttachmentOutput,
                    DstStageMask = PipelineStages.BottomOfPipe,
                    SrcAccessMask = Accesses.ColorAttachmentRead | Accesses.ColorAttachmentWrite,
                    DstAccessMask = Accesses.MemoryRead,
                    DependencyFlags = Dependencies.ByRegion
                }
            };

            var createInfo = new RenderPassCreateInfo(subpasses, attachments, dependencies);
            return Device.Logical.CreateRenderPass(createInfo);
        }

        private ImageView[] CreateImageViews()
        {
            var imageViews = new ImageView[SwapchainImages.Length];
            for (int i = 0; i < SwapchainImages.Length; i++)
            {
                imageViews[i] = SwapchainImages[i].CreateView(new ImageViewCreateInfo(
                    Swapchain.Format,
                    new ImageSubresourceRange(ImageAspects.Color, 0, 1, 0, 1)));
            }
            return imageViews;
        }

        private Framebuffer[] CreateFramebuffers()
        {
            var framebuffers = new Framebuffer[SwapchainImages.Length];
            for (int i = 0; i < SwapchainImages.Length; i++)
            {
                framebuffers[i] = _renderPass.CreateFramebuffer(new FramebufferCreateInfo(
                    new[] { _imageViews[i], _depthStencilBuffer.View },
                    Host.Width,
                    Host.Height));
            }
            return framebuffers;
        }

        private DescriptorSetLayout CreateGraphicsDescriptorSetLayout()
        {
            return Device.Logical.CreateDescriptorSetLayout(new DescriptorSetLayoutCreateInfo(
                new DescriptorSetLayoutBinding(0, DescriptorType.CombinedImageSampler, 1, ShaderStages.Fragment),
                new DescriptorSetLayoutBinding(1, DescriptorType.CombinedImageSampler, 1, ShaderStages.Fragment)));
        }

        private PipelineLayout CreateGraphicsPipelineLayout()
        {
            return Device.Logical.CreatePipelineLayout(new PipelineLayoutCreateInfo(new[] { _graphicsDescriptorSetLayout }));
        }

        private Pipeline CreateGraphicsPipeline()
        {
            var inputAssemblyState = new PipelineInputAssemblyStateCreateInfo(PrimitiveTopology.PointList);
            var vertexInputState = new PipelineVertexInputStateCreateInfo(
                new[] { new VertexInputBindingDescription(0, Interop.SizeOf<VertexParticle>(), VertexInputRate.Vertex) },
                new[]
                {
                    new VertexInputAttributeDescription(0, 0, Format.R32G32SFloat, 0),
                    new VertexInputAttributeDescription(1, 0, Format.R32G32B32A32SFloat, 8)
                });
            var rasterizationState = new PipelineRasterizationStateCreateInfo
            {
                PolygonMode = PolygonMode.Fill,
                CullMode = CullModes.None,
                FrontFace = FrontFace.CounterClockwise
            };
            // Additive blending.
            var blendAttachmentState = new PipelineColorBlendAttachmentState
            {
                BlendEnable = true,
                ColorWriteMask = ColorComponents.All,
                ColorBlendOp = BlendOp.Add,
                SrcColorBlendFactor = BlendFactor.One,
                DstColorBlendFactor = BlendFactor.One,
                AlphaBlendOp = BlendOp.Add,
                SrcAlphaBlendFactor = BlendFactor.SrcAlpha,
                DstAlphaBlendFactor = BlendFactor.DstAlpha
            };
            var colorBlendState = new PipelineColorBlendStateCreateInfo(new[] { blendAttachmentState });
            var depthStencilState = new PipelineDepthStencilStateCreateInfo();
            var viewportState = new PipelineViewportStateCreateInfo(
                new Viewport(0, 0, Host.Width, Host.Height),
                new Rect2D(0, 0, Host.Width, Host.Height));
            var multisampleState = new PipelineMultisampleStateCreateInfo { RasterizationSamples = SampleCounts.Count1 };

            var pipelineShaderStages = new[]
            {
                new PipelineShaderStageCreateInfo(ShaderStages.Vertex, Content.Load<ShaderModule>("shader.vert.spv"), "main"),
                new PipelineShaderStageCreateInfo(ShaderStages.Fragment, Content.Load<ShaderModule>("shader.frag.spv"), "main"),
            };

            var pipelineCreateInfo = new GraphicsPipelineCreateInfo(_graphicsPipelineLayout, _renderPass, 0,
                pipelineShaderStages, 
                inputAssemblyState,
                vertexInputState,
                rasterizationState,
                viewportState: viewportState,
                multisampleState: multisampleState,
                depthStencilState: depthStencilState,
                colorBlendState: colorBlendState);

            return Device.Logical.CreateGraphicsPipeline(pipelineCreateInfo);
        }

        private DescriptorSetLayout CreateComputeDescriptorSetLayout()
        {
            return Device.Logical.CreateDescriptorSetLayout(new DescriptorSetLayoutCreateInfo(
                new DescriptorSetLayoutBinding(0, DescriptorType.StorageBuffer, 1, ShaderStages.Compute),
                new DescriptorSetLayoutBinding(1, DescriptorType.UniformBuffer, 1, ShaderStages.Compute)));
        }

        private PipelineLayout CreateComputePipelineLayout()
        {
            return Device.Logical.CreatePipelineLayout( new PipelineLayoutCreateInfo(new[] { _computeDescriptorSetLayout }));
        }

        private Pipeline CreateComputePipeline()
        {
            var pipelineCreateInfo = new ComputePipelineCreateInfo(
                new PipelineShaderStageCreateInfo(ShaderStages.Compute, Content.Load<ShaderModule>("shader.comp.spv"), "main"),
                _computePipelineLayout);
            return Device.Logical.CreateComputePipeline(pipelineCreateInfo);
        }
    }

    public struct VertexParticle
    {
        
    }
}
