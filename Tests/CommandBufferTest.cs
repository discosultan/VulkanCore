using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class CommandBufferTest : HandleTestBase
    {
        [Fact]
        public void BeginAndEnd()
        {
            CommandBuffer.Begin();
            CommandBuffer.End();
        }

        [Fact]
        public void CmdBeginAndEndQuery()
        {
            using (QueryPool queryPool = Device.CreateQueryPool(new QueryPoolCreateInfo(QueryType.Occlusion, 1)))
            {
                CommandBuffer.Begin();
                CommandBuffer.CmdBeginQuery(queryPool, 0);
                CommandBuffer.CmdEndQuery(queryPool, 0);
                CommandBuffer.End();
            }
        }

        [Fact]
        public void CmdWriteTimestamp()
        {
            using (QueryPool queryPool = Device.CreateQueryPool(new QueryPoolCreateInfo(QueryType.Timestamp, 1)))
            {
                CommandBuffer.Begin();
                CommandBuffer.CmdWriteTimestamp(PipelineStages.AllCommands, queryPool, 0);
                CommandBuffer.End();
            }
        }

        [Fact]
        public void CmdCopyQueryPoolResults()
        {
            const long bufferSize = 256L;
            using (QueryPool queryPool = Device.CreateQueryPool(new QueryPoolCreateInfo(QueryType.Timestamp, 1)))
            using (Buffer buffer = Device.CreateBuffer(new BufferCreateInfo(bufferSize, BufferUsages.TransferDst)))
            using (DeviceMemory memory = Device.AllocateMemory(new MemoryAllocateInfo(bufferSize, 0)))
            {
                // Required to keep the validation layer happy.
                // Ideally we should allocate memory based on these requirements.
                buffer.GetMemoryRequirements();

                buffer.BindMemory(memory);
                CommandBuffer.Begin();
                CommandBuffer.CmdCopyQueryPoolResults(queryPool, 0, 1, buffer, 0, bufferSize);
                CommandBuffer.End();
            }
        }

        [Fact]
        public void CmdResetQueryPool()
        {
            using (QueryPool queryPool = Device.CreateQueryPool(new QueryPoolCreateInfo(QueryType.Timestamp, 1)))
            {
                CommandBuffer.Begin();
                CommandBuffer.CmdResetQueryPool(queryPool, 0, 1);
                CommandBuffer.End();
            }
        }

        [Fact]
        public void CmdBeginEndRenderPass()
        {
            using (RenderPass renderPass = Device.CreateRenderPass(new RenderPassCreateInfo(new[] { new SubpassDescription(null) })))
            using (Framebuffer framebuffer = renderPass.CreateFramebuffer(new FramebufferCreateInfo(null, 32, 32)))
            {
                CommandBuffer.Begin();
                CommandBuffer.CmdBeginRenderPass(new RenderPassBeginInfo(framebuffer, default(Rect2D)));
                CommandBuffer.CmdEndRenderPass();
                CommandBuffer.End();
            }
        }

        [Fact]
        public void CmdSetScissors()
        {
            CommandBuffer.Begin();
            CommandBuffer.CmdSetScissor(new Rect2D(Offset2D.Zero, new Extent2D(32, 32)));
            CommandBuffer.CmdSetScissors(0, 1, new[] { new Rect2D(Offset2D.Zero, new Extent2D(32, 32)) });
            CommandBuffer.End();
        }

        [Fact]
        public void CmdSetViewports()
        {
            CommandBuffer.Begin();
            CommandBuffer.CmdSetViewport(new Viewport(0, 0, 32, 32));
            CommandBuffer.CmdSetViewports(0, 1, new[] { new Viewport(0, 0, 32, 32) });
            CommandBuffer.End();
        }

        [Fact]
        public void CmdSetLineWidth()
        {
            CommandBuffer.Begin();
            CommandBuffer.CmdSetLineWidth(1.0f);
            CommandBuffer.End();
        }

        [Fact]
        public void CmdSetDepthParameters()
        {
            CommandBuffer.Begin();
            CommandBuffer.CmdSetDepthBias(1.0f, 1.0f, 1.0f);
            CommandBuffer.CmdSetDepthBounds(0.0f, 1.0f);
            CommandBuffer.End();
        }

        [Fact]
        public void CmdSetBlendConstants()
        {
            CommandBuffer.Begin();
            CommandBuffer.CmdSetBlendConstants(new ColorF4(1.0f, 1.0f, 1.0f, 1.0f));
            CommandBuffer.End();
        }

        [Fact]
        public void CmdSetStencilParameters()
        {
            CommandBuffer.Begin();
            CommandBuffer.CmdSetStencilCompareMask(StencilFaces.Front, ~0);
            CommandBuffer.CmdSetStencilReference(StencilFaces.Front, 1);
            CommandBuffer.CmdSetStencilWriteMask(StencilFaces.Front, ~0);
            CommandBuffer.End();
        }

        [Fact]
        public void CmdBindDescriptorSet()
        {
            const int bufferSize = 256;

            var layoutCreateInfo = new DescriptorSetLayoutCreateInfo(
                new DescriptorSetLayoutBinding(0, DescriptorType.StorageBuffer, 1));
            var descriptorPoolCreateInfo = new DescriptorPoolCreateInfo(
                1,
                new[] { new DescriptorPoolSize(DescriptorType.StorageBuffer, 1) });

            using (DescriptorSetLayout descriptorSetLayout = Device.CreateDescriptorSetLayout(layoutCreateInfo))
            using (DescriptorPool descriptorPool = Device.CreateDescriptorPool(descriptorPoolCreateInfo))
            using (PipelineLayout pipelineLayout = Device.CreatePipelineLayout(new PipelineLayoutCreateInfo(new[] { descriptorSetLayout })))
            using (Buffer buffer = Device.CreateBuffer(new BufferCreateInfo(bufferSize, BufferUsages.StorageBuffer)))
            using (DeviceMemory memory = Device.AllocateMemory(new MemoryAllocateInfo(bufferSize, 0)))
            {
                // Required to satisfy the validation layer.
                buffer.GetMemoryRequirements();

                buffer.BindMemory(memory);

                DescriptorSet descriptorSet =
                    descriptorPool.AllocateSets(new DescriptorSetAllocateInfo(1, descriptorSetLayout))[0];

                var descriptorWrite = new WriteDescriptorSet(descriptorSet, 0, 0, 1, DescriptorType.StorageBuffer,
                    bufferInfo: new[] { new DescriptorBufferInfo(buffer) });

                descriptorPool.UpdateSets(new[] { descriptorWrite });

                CommandBuffer.Begin();
                CommandBuffer.CmdBindDescriptorSet(PipelineBindPoint.Graphics, pipelineLayout, descriptorSet);
                CommandBuffer.CmdBindDescriptorSets(PipelineBindPoint.Graphics, pipelineLayout, 0, new[] { descriptorSet });
                CommandBuffer.End();

                descriptorPool.Reset();
            }
        }

        [Fact]
        public void CmdBindVertexAndIndexBuffer()
        {
            const int bufferSize = 256;
            using (Buffer buffer = Device.CreateBuffer(new BufferCreateInfo(bufferSize, BufferUsages.VertexBuffer | BufferUsages.IndexBuffer)))
            {
                MemoryRequirements memReq = buffer.GetMemoryRequirements();
                int memTypeIndex = PhysicalDeviceMemoryProperties.MemoryTypes.IndexOf(
                    memReq.MemoryTypeBits, MemoryProperties.HostVisible);
                using (DeviceMemory memory = Device.AllocateMemory(new MemoryAllocateInfo(memReq.Size, memTypeIndex)))
                {
                    buffer.BindMemory(memory);

                    CommandBuffer.Begin();
                    CommandBuffer.CmdBindVertexBuffer(buffer);
                    CommandBuffer.CmdBindVertexBuffers(0, 1, new[] { buffer }, new long[] { 0 });
                    CommandBuffer.CmdBindIndexBuffer(buffer);
                    CommandBuffer.End();
                }
            }
        }

        [Fact]
        public void CmdBindPipeline()
        {
            var descriptorSetLayoutCreateInfo = new DescriptorSetLayoutCreateInfo(
                new DescriptorSetLayoutBinding(0, DescriptorType.StorageBuffer, 1, ShaderStages.Compute),
                new DescriptorSetLayoutBinding(1, DescriptorType.StorageBuffer, 1, ShaderStages.Compute));
            using (DescriptorSetLayout descriptorSetLayout = Device.CreateDescriptorSetLayout(descriptorSetLayoutCreateInfo))
            using (PipelineLayout pipelineLayout = Device.CreatePipelineLayout(new PipelineLayoutCreateInfo(new[] { descriptorSetLayout })))
            using (ShaderModule shader = Device.CreateShaderModule(new ShaderModuleCreateInfo(ReadAllBytes("Shader.comp.spv"))))
            {
                var pipelineCreateInfo = new ComputePipelineCreateInfo(
                    new PipelineShaderStageCreateInfo(ShaderStages.Compute, shader, "main"),
                    pipelineLayout);

                using (Pipeline pipeline = Device.CreateComputePipeline(pipelineCreateInfo))
                {
                    CommandBuffer.Begin();
                    CommandBuffer.CmdBindPipeline(PipelineBindPoint.Compute, pipeline);
                    CommandBuffer.End();
                }
            }
        }

        [Fact]
        public void CmdSetAndResetEvent()
        {
            using (Event evt = Device.CreateEvent())
            {
                CommandBuffer.Begin();
                CommandBuffer.CmdSetEvent(evt, PipelineStages.AllCommands);
                CommandBuffer.CmdResetEvent(evt, PipelineStages.AllCommands);
                CommandBuffer.End();
            }
        }

        [Fact]
        public void CmdWaitEvents()
        {
            using (Event evt = Device.CreateEvent())
            {
                CommandBuffer.Begin();
                CommandBuffer.CmdWaitEvent(evt, PipelineStages.AllCommands, PipelineStages.AllCommands);
                CommandBuffer.CmdWaitEvents(new[] { evt }, PipelineStages.AllCommands, PipelineStages.AllCommands);
                CommandBuffer.End();
            }
        }

        [Fact]
        public void CmdDraw()
        {
            var renderPassCreateInfo = new RenderPassCreateInfo(
                new[] { new SubpassDescription(new[] { new AttachmentReference(0, ImageLayout.ColorAttachmentOptimal) }) },
                new[]
                {
                    new AttachmentDescription
                    {
                        Format = Format.B8G8R8A8UNorm,
                        Samples = SampleCounts.Count1,
                        FinalLayout = ImageLayout.ColorAttachmentOptimal,
                        LoadOp = AttachmentLoadOp.DontCare
                    }
                });
            var imageCreateInfo = new ImageCreateInfo
            {
                Usage = ImageUsages.ColorAttachment,
                Format = Format.B8G8R8A8UNorm,
                Extent = new Extent3D(2, 2, 1),
                ImageType = ImageType.Image2D,
                MipLevels = 1,
                ArrayLayers = 1,
                Samples = SampleCounts.Count1
            };
            var imageViewCreateInfo = new ImageViewCreateInfo(
                Format.B8G8R8A8UNorm,
                new ImageSubresourceRange(ImageAspects.Color, 0, 1, 0, 1));

            using (ShaderModule vertexShader = Device.CreateShaderModule(new ShaderModuleCreateInfo(ReadAllBytes("Shader.vert.spv"))))
            using (ShaderModule fragmentShader = Device.CreateShaderModule(new ShaderModuleCreateInfo(ReadAllBytes("Shader.frag.spv"))))
            using (PipelineLayout pipelineLayout = Device.CreatePipelineLayout())
            using (RenderPass renderPass = Device.CreateRenderPass(renderPassCreateInfo))
            using (Image image = Device.CreateImage(imageCreateInfo))
            {
                MemoryRequirements imageMemReq = image.GetMemoryRequirements();
                int memTypeIndex = PhysicalDeviceMemoryProperties.MemoryTypes.IndexOf(imageMemReq.MemoryTypeBits, MemoryProperties.DeviceLocal);
                using (DeviceMemory imageMemory = Device.AllocateMemory(new MemoryAllocateInfo(imageMemReq.Size, memTypeIndex)))
                {
                    image.BindMemory(imageMemory);
                    using (ImageView imageView = image.CreateView(imageViewCreateInfo))
                    using (Framebuffer framebuffer = renderPass.CreateFramebuffer(new FramebufferCreateInfo(new[] { imageView }, 2, 2)))
                    using (Pipeline pipeline = Device.CreateGraphicsPipeline(new GraphicsPipelineCreateInfo(
                        pipelineLayout,
                        renderPass,
                        0,
                        new[]
                        {
                            new PipelineShaderStageCreateInfo(ShaderStages.Vertex, vertexShader, "main"),
                            new PipelineShaderStageCreateInfo(ShaderStages.Fragment, fragmentShader, "main")
                        },
                        new PipelineInputAssemblyStateCreateInfo(),
                        new PipelineVertexInputStateCreateInfo(),
                        new PipelineRasterizationStateCreateInfo { RasterizerDiscardEnable = true, LineWidth = 1.0f })))
                    {
                        CommandBuffer.Begin();
                        CommandBuffer.CmdBeginRenderPass(new RenderPassBeginInfo(framebuffer, new Rect2D(0, 0, 2, 2)));
                        CommandBuffer.CmdBindPipeline(PipelineBindPoint.Graphics, pipeline);
                        CommandBuffer.CmdDraw(3);
                        CommandBuffer.CmdEndRenderPass();
                        CommandBuffer.End();
                    }
                }
            }
        }

        [Fact]
        public void Reset()
        {
            CommandPool.Reset();
            CommandBuffer.Reset();
        }

        [Fact]
        public void Free()
        {
            using (CommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0]) { }
            CommandBuffer[] buffers = CommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1));
            CommandPool.FreeBuffers(buffers);
        }

        public CommandBufferTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output)
        {
            CommandPool = Device.CreateCommandPool(
                new CommandPoolCreateInfo(defaults.GraphicsQueue.FamilyIndex, CommandPoolCreateFlags.ResetCommandBuffer));
            CommandBuffer = CommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0];
        }

        public CommandPool CommandPool { get; }
        public CommandBuffer CommandBuffer { get; }

        public override void Dispose()
        {
            CommandPool.Dispose();
            base.Dispose();
        }
    }
}
