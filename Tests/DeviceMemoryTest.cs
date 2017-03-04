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
            using (DeviceMemory memory = AllocateMappableMemory(allocationSize))
            {
                memory.Map(0, allocationSize);
                memory.Unmap();
            }
        }

        [Fact]
        public void GetCommitment()
        {
            const int allocationSize = 32;
            using (DeviceMemory memory = AllocateMappableMemory(allocationSize))
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

        private DeviceMemory AllocateMappableMemory(int size)
        {
            PhysicalDeviceMemoryProperties memoryProperties = DefaultPhysicalDevice.GetMemoryProperties();
            int memoryTypeIndex = memoryProperties.MemoryTypes.IndexOf(~0, MemoryProperties.HostVisible | MemoryProperties.HostCoherent);

            return DefaultDevice.AllocateMemory(new MemoryAllocateInfo(size, memoryTypeIndex));
        }
    }
}
