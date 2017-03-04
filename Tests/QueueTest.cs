using System.Linq;
using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class QueueTest : HandleTestBase
    {
        [Fact]
        public void PropertiesSet()
        {
            QueueFamilyProperties[] props = PhysicalDevice.GetQueueFamilyProperties();
            int queueFamilyIndex = props.Length - 1;
            int queueCount = props[props.Length - 1].QueueCount;
            int queueIndex = queueCount - 1;
            var deviceCreateInfo = new DeviceCreateInfo(new[]
            {
                new DeviceQueueCreateInfo(queueFamilyIndex, queueCount, Enumerable.Range(0, queueCount).Select(_ => 1.0f).ToArray())
            });
            using (Device device = PhysicalDevice.CreateDevice(deviceCreateInfo))
            {
                Queue queue = device.GetQueue(queueFamilyIndex);

                Assert.Equal(device, queue.Parent);
                Assert.Equal(queueFamilyIndex, queue.FamilyIndex);
                Assert.Equal(queueIndex, queue.Index);
            }
        }

        [Fact]
        public void WaitIdle()
        {
            GraphicsQueue.WaitIdle();
        }

        [Fact]
        public void Submit()
        {
            GraphicsQueue.Submit(null, 0, null, null);
            GraphicsQueue.Submit(new SubmitInfo());
            GraphicsQueue.Submit(new[] { new SubmitInfo() });
        }

        [Fact]
        public void BindSparse()
        {
            var bindSparseInfo = new BindSparseInfo(null, null, null, null, null);
            GraphicsQueue.BindSparse(bindSparseInfo);
            GraphicsQueue.BindSparse(new[] { bindSparseInfo });
        }

        public QueueTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }
    }
}
