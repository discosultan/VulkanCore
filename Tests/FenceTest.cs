using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class FenceTest : HandleTestBase
    {
        [Fact]
        public void WaitFence()
        {
            using (Fence fence = Device.CreateFence(new FenceCreateInfo(FenceCreateFlags.Signaled)))
                fence.Wait();
            using (Fence fence = Device.CreateFence(new FenceCreateInfo(FenceCreateFlags.Signaled)))
                Device.WaitFences(new[] { fence }, true);
        }

        [Fact]
        public void ResetFence()
        {
            using (Fence fence = Device.CreateFence(new FenceCreateInfo(FenceCreateFlags.Signaled)))
                fence.Reset();
            using (Fence fence = Device.CreateFence(new FenceCreateInfo(FenceCreateFlags.Signaled)))
                Device.ResetFences(fence);
        }

        [Fact]
        public void GetStatus()
        {
            using (Fence fence = Device.CreateFence(new FenceCreateInfo(FenceCreateFlags.Signaled)))
            {
                Result status = fence.GetStatus();
                Assert.Equal(Result.Success, status);
            }
        }

        public FenceTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }
    }
}
