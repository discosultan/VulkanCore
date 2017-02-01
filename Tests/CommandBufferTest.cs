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
            DefaultCommandBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.OneTimeSubmit));
            DefaultCommandBuffer.End();
        }

        [Fact]
        public void SetScissor_Succeeds()
        {
            DefaultCommandBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.OneTimeSubmit));
            DefaultCommandBuffer.CmdSetScissor(new Rect2D(Offset2D.Zero, new Extent2D(32, 32)));
            DefaultCommandBuffer.CmdSetScissors(0, 1, new[] { new Rect2D(Offset2D.Zero, new Extent2D(32, 32)) });
            DefaultCommandBuffer.End();
        }

        [Fact]
        public void SetViewport_Succeeds()
        {
            DefaultCommandBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.OneTimeSubmit));
            DefaultCommandBuffer.CmdSetViewport(new Viewport(0, 0, 32, 32));
            DefaultCommandBuffer.CmdSetViewports(0, 1, new[] { new Viewport(0, 0, 32, 32) });
            DefaultCommandBuffer.End();
        }

        [Fact]
        public void Reset_Succeeds()
        {
            DefaultCommandPool.Reset();
            DefaultCommandBuffer.Reset();
        }

        [Fact]
        public void Free_Succeeds()
        {
            using (DefaultCommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0]) { }
            CommandBuffer[] buffers = DefaultCommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1));
            DefaultCommandPool.FreeBuffers(buffers);
        }

        public CommandBufferTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output)
        {
            DefaultCommandPool = defaults.Device.CreateCommandPool(
                new CommandPoolCreateInfo(defaults.GraphicsQueue.FamilyIndex, CommandPoolCreateFlags.ResetCommandBuffer));
            DefaultCommandBuffer = DefaultCommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0];
        }

        public CommandPool DefaultCommandPool { get; }
        public CommandBuffer DefaultCommandBuffer { get; }

        public override void Dispose()
        {
            DefaultCommandPool.Dispose();
            base.Dispose();
        }
    }
}
