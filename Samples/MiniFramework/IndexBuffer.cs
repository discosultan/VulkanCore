using System;

namespace VulkanCore.Samples
{
    public class IndexBuffer : IDisposable
    {
        public IndexBuffer(GraphicsDevice device, int[] indices)
        {
            IndexCount = indices.Length;
            int indexBufferSize = indices.Length * sizeof(int);

            // Create staging buffer.
            Buffer indexStagingBuffer = device.Logical.CreateBuffer(new BufferCreateInfo(indexBufferSize, BufferUsages.TransferSrc));
            MemoryRequirements indexStagingReq = indexStagingBuffer.GetMemoryRequirements();
            int indexStagingMemoryTypeIndex = device.MemoryProperties.MemoryTypes.IndexOf(
                indexStagingReq.MemoryTypeBits,
                MemoryProperties.HostVisible | MemoryProperties.HostCoherent);
            DeviceMemory indexStagingMemory = device.Logical.AllocateMemory(new MemoryAllocateInfo(indexStagingReq.Size, indexStagingMemoryTypeIndex));
            IntPtr indexPtr = indexStagingMemory.Map(0, indexStagingReq.Size);
            Interop.Write(indexPtr, indices);
            indexStagingMemory.Unmap();
            indexStagingBuffer.BindMemory(indexStagingMemory);

            // Create a device local buffer.
            Buffer = device.Logical.CreateBuffer(new BufferCreateInfo(indexBufferSize, BufferUsages.IndexBuffer | BufferUsages.TransferDst));
            MemoryRequirements indexReq = Buffer.GetMemoryRequirements();
            int indexMemoryTypeIndex = device.MemoryProperties.MemoryTypes.IndexOf(
                indexReq.MemoryTypeBits,
                MemoryProperties.DeviceLocal);
            Memory = device.Logical.AllocateMemory(new MemoryAllocateInfo(indexReq.Size, indexMemoryTypeIndex));
            Buffer.BindMemory(Memory);

            // Copy the data from staging buffer to device local buffer.
            CommandBuffer cmdBuffer = device.CommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0];
            cmdBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.OneTimeSubmit));
            cmdBuffer.CmdCopyBuffer(indexStagingBuffer, Buffer, new BufferCopy(indexBufferSize));
            cmdBuffer.End();

            // Submit.
            Fence fence = device.Logical.CreateFence();
            device.GraphicsQueue.Submit(new SubmitInfo(commandBuffers: new[] { cmdBuffer }), fence);
            fence.Wait();

            // Cleanup.
            fence.Dispose();
            cmdBuffer.Dispose();
            indexStagingBuffer.Dispose();
            indexStagingMemory.Dispose();
        }

        public Buffer Buffer { get; private set; }
        public DeviceMemory Memory { get; private set; }
        public int IndexCount { get; private set; }

        public void Dispose()
        {
            Memory.Dispose();
            Buffer.Dispose();
        }

        public static implicit operator Buffer(IndexBuffer value) => value.Buffer;
    }
}
