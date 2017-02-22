using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class BufferTest : HandleTestBase
    {
        private const int DefaultBufferSize = 32;

        [Fact]
        public void GetMemoryRequirements_Succeeds()
        {
            using (Buffer buffer = CreateBuffer())
            {
                buffer.GetMemoryRequirements();
            }
        }

        [Fact]
        public void BindMemoryAndCreateBufferView_Succeeds()
        {
            using (Buffer buffer = CreateBuffer())
            {
                PhysicalDeviceMemoryProperties deviceMemProps = PhysicalDevice.GetMemoryProperties();
                MemoryRequirements memReq = buffer.GetMemoryRequirements();
                var memoryAllocateInfo = new MemoryAllocateInfo(
                    memReq.Size,
                    deviceMemProps.GetMemoryTypeIndex(memReq.MemoryTypeBits, 0));

                using (DeviceMemory memory = Device.AllocateMemory(memoryAllocateInfo))
                {
                    buffer.BindMemory(memory);
                    Assert.Equal(memory, buffer.BackedMemory);

                    var bufferViewCreateInfo = new BufferViewCreateInfo(Format.R32UInt);
                    using (buffer.CreateView(bufferViewCreateInfo)) { }
                    using (buffer.CreateView(bufferViewCreateInfo, CustomAllocator)) { }
                }
            }
        }

        public BufferTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }

        private Buffer CreateBuffer()
        {
            var bufferCreateInfo = new BufferCreateInfo(DefaultBufferSize, BufferUsages.UniformTexelBuffer);
            return Device.CreateBuffer(bufferCreateInfo);

        }
    }
}
