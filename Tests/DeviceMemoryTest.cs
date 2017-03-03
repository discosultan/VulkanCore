using System.Linq;
using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class DeviceMemoryTest : HandleTestBase
    {
        [Fact]
        public void MapAndUnmapMemory()
        {
            const int allocationSize = 32;
            using (DeviceMemory memory = AllocateHostVisibleMemory(allocationSize))
            {
                memory.Map(0, allocationSize);
                memory.Unmap();
            }
        }

        [Fact]
        public void GetCommitment()
        {
            const int allocationSize = 32;
            using (DeviceMemory memory = AllocateHostVisibleMemory(allocationSize))
            {
                long commitment = memory.GetCommitment();
                Assert.True(commitment > 0);
            }            
        }

        public DeviceMemoryTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output)
        {
            DefaultPhysicalDevice = defaults.PhysicalDevice;
            DefaultDevice = defaults.Device;
        }

        private PhysicalDevice DefaultPhysicalDevice { get; }
        private Device DefaultDevice { get; }

        private DeviceMemory AllocateHostVisibleMemory(int size)
        {
            PhysicalDeviceMemoryProperties memoryProperties = DefaultPhysicalDevice.GetMemoryProperties();
            (MemoryType type, int index) = memoryProperties.MemoryTypes
                .Select((t, i) => (t, i))
                .First(t => t.Item1.PropertyFlags.HasFlag(MemoryProperties.HostVisible));

            return DefaultDevice.AllocateMemory(new MemoryAllocateInfo(size, index));
        }
    }
}
