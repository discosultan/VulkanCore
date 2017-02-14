using System;
using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public unsafe class QueryTest : HandleTestBase
    {
        [Fact]
        public void GetResults_Succeeds()
        {
            using (var cmdPool = Device.CreateCommandPool(new CommandPoolCreateInfo(GraphicsQueue.FamilyIndex)))
            using (var cmdBuffer = cmdPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0])
            using (var queryPool = Device.CreateQueryPool(new QueryPoolCreateInfo(QueryType.Occlusion, 1)))
            {
                cmdBuffer.Begin();
                cmdBuffer.CmdBeginQuery(queryPool, 0);
                cmdBuffer.CmdEndQuery(queryPool, 0);
                cmdBuffer.End();

                GraphicsQueue.Submit(new SubmitInfo(commandBuffers: new[] { cmdBuffer }));
                Device.WaitIdle();

                const int size = 1024;
                byte* pointer = stackalloc byte[size];
                queryPool.GetResults(0, 1, size, new IntPtr(pointer), size);
            }
        }

        public QueryTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }
    }
}
