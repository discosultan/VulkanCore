using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class SynchronizationPrimitivesTest : HandleTestBase
    {
        [Fact]
        public void WaitFence_Succeeds()
        {
            using (Fence fence = Device.CreateFence(new FenceCreateInfo(FenceCreateFlags.Signaled)))
                fence.Wait();
            using (Fence fence = Device.CreateFence(new FenceCreateInfo(FenceCreateFlags.Signaled)))
                Device.WaitFences(new[] { fence }, true);
        }

        [Fact]
        public void ResetFence_Succeeds()
        {
            using (Fence fence = Device.CreateFence(new FenceCreateInfo(FenceCreateFlags.Signaled)))
                fence.Reset();
            using (Fence fence = Device.CreateFence(new FenceCreateInfo(FenceCreateFlags.Signaled)))
                Device.ResetFences(fence);
        }

        [Fact]
        public void SetEvent_Succeeds()
        {
            using (Event evt = Device.CreateEvent())
                evt.Set();            
        }

        [Fact]
        public void ResetEvent_Succeeds()
        {
            using (Event evt = Device.CreateEvent())
                evt.Reset();
        }

        public SynchronizationPrimitivesTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }
    }
}
