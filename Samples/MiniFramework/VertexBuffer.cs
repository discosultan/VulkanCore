using System;

namespace VulkanCore.Samples
{
    public class VertexBuffer<TVertex> : IDisposable where TVertex : struct
    {
        public VertexBuffer(GraphicsDevice device, TVertex[] vertices)
        {
            VertexCount = vertices.Length;
            int vertexBufferSize = vertices.Length * Interop.SizeOf<TVertex>();

            // Vertex buffer.
            Buffer vertexStagingBuffer = device.Logical.CreateBuffer(new BufferCreateInfo(vertexBufferSize, BufferUsages.TransferSrc));
            MemoryRequirements vertexStagingReq = vertexStagingBuffer.GetMemoryRequirements();
            int vertexStagingMemoryTypeIndex = device.MemoryProperties.MemoryTypes.IndexOf(
                vertexStagingReq.MemoryTypeBits,
                MemoryProperties.HostVisible | MemoryProperties.HostCoherent);
            DeviceMemory vertexStagingMemory = device.Logical.AllocateMemory(new MemoryAllocateInfo(vertexStagingReq.Size, vertexStagingMemoryTypeIndex));
            IntPtr vertexPtr = vertexStagingMemory.Map(0, vertexStagingReq.Size);
            Interop.Write(vertexPtr, vertices);
            vertexStagingMemory.Unmap();
            vertexStagingBuffer.BindMemory(vertexStagingMemory);

            // Create a device local buffer where the vertex data will be copied and which will be used for rendering.
            Buffer = device.Logical.CreateBuffer(new BufferCreateInfo(vertexBufferSize, BufferUsages.VertexBuffer | BufferUsages.TransferDst));
            MemoryRequirements vertexReq = Buffer.GetMemoryRequirements();
            int vertexMemoryTypeIndex = device.MemoryProperties.MemoryTypes.IndexOf(
                vertexReq.MemoryTypeBits,
                MemoryProperties.DeviceLocal);
            Memory = device.Logical.AllocateMemory(new MemoryAllocateInfo(vertexReq.Size, vertexMemoryTypeIndex));
            Buffer.BindMemory(Memory);

            // Copy the data from staging buffers to device local buffers.
            CommandBuffer cmdBuffer = device.CommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0];
            cmdBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.OneTimeSubmit));
            cmdBuffer.CmdCopyBuffer(vertexStagingBuffer, Buffer, new BufferCopy(vertexBufferSize));
            cmdBuffer.End();

            // Submit.
            Fence fence = device.Logical.CreateFence();
            device.GraphicsQueue.Submit(new SubmitInfo(commandBuffers: new[] { cmdBuffer }), fence);
            fence.Wait();

            // Cleanup.
            fence.Dispose();
            cmdBuffer.Dispose();
            vertexStagingBuffer.Dispose();
            vertexStagingMemory.Dispose();
        }

        public Buffer Buffer { get; private set; }
        public DeviceMemory Memory { get; private set; }
        public int VertexCount { get; private set; }

        public void Dispose()
        {
            Memory.Dispose();
            Buffer.Dispose();
        }

        public static implicit operator Buffer(VertexBuffer<TVertex> value) => value.Buffer;
    }
}
