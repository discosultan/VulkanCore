using System;

namespace VulkanCore.Samples
{
    public class UniformBuffer : IDisposable
    {
        public UniformBuffer(GraphicsDevice device, long size)
        {
            Buffer = device.Logical.CreateBuffer(new BufferCreateInfo(size, BufferUsages.UniformBuffer));
            MemoryRequirements memoryRequirements = Buffer.GetMemoryRequirements();
            // We require host visible memory so we can map it and write to it directly.
            // We require host coherent memory so that writes are visible to the GPU right after unmapping it.
            int memoryTypeIndex = device.MemoryProperties.MemoryTypes.IndexOf(
                memoryRequirements.MemoryTypeBits,
                MemoryProperties.HostVisible | MemoryProperties.HostCoherent);
            Memory = device.Logical.AllocateMemory(new MemoryAllocateInfo(memoryRequirements.Size, memoryTypeIndex));
            Buffer.BindMemory(Memory);
        }

        public Buffer Buffer { get; }
        public DeviceMemory Memory { get; }

        public void Dispose()
        {
            Memory.Dispose();
            Buffer.Dispose();
        }

        public static implicit operator Buffer(UniformBuffer value) => value.Buffer;
    }
}
