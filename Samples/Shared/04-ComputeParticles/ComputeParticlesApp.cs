namespace VulkanCore.Samples.ComputeParticles
{
    public class ComputeParticlesApp : VulkanApp
    {
        private Pipeline _graphicsPipeline;
        private Pipeline _computePipeline;

        protected override void InitializeFrame()
        {
            _graphicsPipeline = ToDispose(CreateGraphicsPipeline());
            _computePipeline = ToDispose(CreateComputePipeline());
        }

        protected override void RecordCommandBuffer(CommandBuffer cmdBuffer, int imageIndex)
        {
            throw new System.NotImplementedException();
        }

        private Pipeline CreateGraphicsPipeline()
        {
            var inputAssemblyState = new PipelineInputAssemblyStateCreateInfo(PrimitiveTopology.PointList);
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

            ShaderModule vertexShader = Content.Load<ShaderModule>("shader.vert.spv");
            ShaderModule fragmentShader = Content.Load<ShaderModule>("shader.frag.spv");

            var pipelineCreateInfo = new GraphicsPipelineCreateInfo();

            return Device.Logical.CreateGraphicsPipeline(pipelineCreateInfo);
        }

        private Pipeline CreateComputePipeline()
        {
            return null;
        }
    }
}
