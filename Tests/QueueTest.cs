using System.Linq;
using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public unsafe class QueueTest : HandleTestBase
    {
        [Fact]
        public void PropertiesSet()
        {
            QueueFamilyProperties[] props = PhysicalDevice.GetQueueFamilyProperties();
            (QueueFamilyProperties prop, int index) = props.Select((x, i) => (x, i)).Last();
            var deviceCreateInfo = new DeviceCreateInfo(new[]
            {
                new DeviceQueueCreateInfo(index, 1, new[] { 1.0f })
            });
            using (Device device = PhysicalDevice.CreateDevice(deviceCreateInfo))
            {
                Queue queue = device.GetQueue(index, 0);

                Assert.Equal(device, queue.Parent);
                Assert.Equal(index, queue.FamilyIndex);
                Assert.Equal(0, queue.Index);
            }
        }

        [Fact]
        public void WaitIdle_Succeeds()
        {
            GraphicsQueue.WaitIdle();
        }

        [Fact]
        public void Submit_Succeeds()
        {
            GraphicsQueue.Submit(new SubmitInfo());
            GraphicsQueue.Submit(new[] { new SubmitInfo() });
        }

        [Fact]
        public void BindSparse_Succeeds()
        {
            var bindSparseInfo = new BindSparseInfo(null, null, null, null, null);
            GraphicsQueue.BindSparse(bindSparseInfo);
            GraphicsQueue.BindSparse(new[] { bindSparseInfo });
        }

        public QueueTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }
    }
}
