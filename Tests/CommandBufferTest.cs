using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class CommandBufferTest : HandleTestBase
    {
        [Fact]
        public void BeginEnd_Succeeds()
        {
            CommandBuffer.Begin();
            CommandBuffer.End();
        }

        [Fact]
        public void BeginEndQuery_Succeeds()
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
        public void WriteTimestamp_Succeeds()
        {
            using (QueryPool queryPool = Device.CreateQueryPool(new QueryPoolCreateInfo(QueryType.Timestamp, 1)))
            {
                CommandBuffer.Begin();
                CommandBuffer.CmdWriteTimestamp(PipelineStages.AllCommands, queryPool, 0);
                CommandBuffer.End();
            }
        }

        [Fact]
        public void CopyQueryPoolResults_Succeeds()
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
        public void ResetQueryPool_Succeeds()
        {
            using (QueryPool queryPool = Device.CreateQueryPool(new QueryPoolCreateInfo(QueryType.Timestamp, 1)))
            {
                CommandBuffer.Begin();
                CommandBuffer.CmdResetQueryPool(queryPool, 0, 1);
                CommandBuffer.End();
            }
        }

        [Fact]
        public void BeginEndRenderPass_Succeeds()
        {
            using (RenderPass renderPass = Device.CreateRenderPass(new RenderPassCreateInfo(new[] { new SubpassDescription() })))
            using (Framebuffer framebuffer = renderPass.CreateFramebuffer(new FramebufferCreateInfo()))
            {
                CommandBuffer.Begin();
                CommandBuffer.CmdBeginRenderPass(new RenderPassBeginInfo(framebuffer, default(Rect2D)));
                CommandBuffer.CmdEndRenderPass();
                CommandBuffer.End();
            }
        }

        [Fact]
        public void SetScissor_Succeeds()
        {
            CommandBuffer.Begin();
            CommandBuffer.CmdSetScissor(new Rect2D(Offset2D.Zero, new Extent2D(32, 32)));
            CommandBuffer.CmdSetScissors(0, 1, new[] { new Rect2D(Offset2D.Zero, new Extent2D(32, 32)) });
            CommandBuffer.End();
        }

        [Fact]
        public void SetViewport_Succeeds()
        {
            CommandBuffer.Begin();
            CommandBuffer.CmdSetViewport(new Viewport(0, 0, 32, 32));
            CommandBuffer.CmdSetViewports(0, 1, new[] { new Viewport(0, 0, 32, 32) });
            CommandBuffer.End();
        }

        [Fact]
        public void Reset_Succeeds()
        {
            CommandPool.Reset();
            CommandBuffer.Reset();
        }

        [Fact]
        public void Free_Succeeds()
        {
            using (CommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0]) { }
            CommandBuffer[] buffers = CommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1));
            CommandPool.FreeBuffers(buffers);
        }

        public CommandBufferTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output)
        {
            CommandPool = defaults.Device.CreateCommandPool(
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
