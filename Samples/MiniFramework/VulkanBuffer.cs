using System;

namespace VulkanCore.Samples
{
    public class VulkanBuffer : IDisposable
    {
        private VulkanBuffer(Buffer buffer, DeviceMemory memory, int count)
        {
            Buffer = buffer;
            Memory = memory;
            Count = count;
        }

        public Buffer Buffer { get; }
        public DeviceMemory Memory { get; }
        public int Count { get; }

        public void Dispose()
        {
            Memory.Dispose();
            Buffer.Dispose();
        }

        public static implicit operator Buffer(VulkanBuffer value) => value.Buffer;

        public static VulkanBuffer Uniform<T>(VulkanContext ctx, int count) where T : struct
        {
            long size = Interop.SizeOf<T>() * count;

            Buffer buffer = ctx.Device.CreateBuffer(new BufferCreateInfo(size, BufferUsages.UniformBuffer));
            MemoryRequirements memoryRequirements = buffer.GetMemoryRequirements();
            // We require host visible memory so we can map it and write to it directly.
            // We require host coherent memory so that writes are visible to the GPU right after unmapping it.
            int memoryTypeIndex = ctx.MemoryProperties.MemoryTypes.IndexOf(
                memoryRequirements.MemoryTypeBits,
                MemoryProperties.HostVisible | MemoryProperties.HostCoherent);
            DeviceMemory memory = ctx.Device.AllocateMemory(new MemoryAllocateInfo(memoryRequirements.Size, memoryTypeIndex));
            buffer.BindMemory(memory);

            return new VulkanBuffer(buffer, memory, count);
        }

        public static VulkanBuffer Index(VulkanContext ctx, int[] indices)
        {
            long size = indices.Length * sizeof(int);

            // Create staging buffer.
            Buffer indexStagingBuffer = ctx.Device.CreateBuffer(new BufferCreateInfo(size, BufferUsages.TransferSrc));
            MemoryRequirements indexStagingReq = indexStagingBuffer.GetMemoryRequirements();
            int indexStagingMemoryTypeIndex = ctx.MemoryProperties.MemoryTypes.IndexOf(
                indexStagingReq.MemoryTypeBits,
                MemoryProperties.HostVisible | MemoryProperties.HostCoherent);
            DeviceMemory indexStagingMemory = ctx.Device.AllocateMemory(new MemoryAllocateInfo(indexStagingReq.Size, indexStagingMemoryTypeIndex));
            IntPtr indexPtr = indexStagingMemory.Map(0, indexStagingReq.Size);
            Interop.Write(indexPtr, indices);
            indexStagingMemory.Unmap();
            indexStagingBuffer.BindMemory(indexStagingMemory);

            // Create a device local buffer.
            Buffer buffer = ctx.Device.CreateBuffer(new BufferCreateInfo(size, BufferUsages.IndexBuffer | BufferUsages.TransferDst));
            MemoryRequirements indexReq = buffer.GetMemoryRequirements();
            int indexMemoryTypeIndex = ctx.MemoryProperties.MemoryTypes.IndexOf(
                indexReq.MemoryTypeBits,
                MemoryProperties.DeviceLocal);
            DeviceMemory memory = ctx.Device.AllocateMemory(new MemoryAllocateInfo(indexReq.Size, indexMemoryTypeIndex));
            buffer.BindMemory(memory);

            // Copy the data from staging buffer to device local buffer.
            CommandBuffer cmdBuffer = ctx.CommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0];
            cmdBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.OneTimeSubmit));
            cmdBuffer.CmdCopyBuffer(indexStagingBuffer, buffer, new BufferCopy(size));
            cmdBuffer.End();

            // Submit.
            Fence fence = ctx.Device.CreateFence();
            ctx.GraphicsQueue.Submit(new SubmitInfo(commandBuffers: new[] { cmdBuffer }), fence);
            fence.Wait();

            // Cleanup.
            fence.Dispose();
            cmdBuffer.Dispose();
            indexStagingBuffer.Dispose();
            indexStagingMemory.Dispose();

            return new VulkanBuffer(buffer, memory, indices.Length);
        }

        public static VulkanBuffer Vertex<T>(VulkanContext ctx, T[] vertices) where T : struct
        {
            int vertexBufferSize = vertices.Length * Interop.SizeOf<T>();

            // Vertex buffer.
            Buffer vertexStagingBuffer = ctx.Device.CreateBuffer(new BufferCreateInfo(vertexBufferSize, BufferUsages.TransferSrc));
            MemoryRequirements vertexStagingReq = vertexStagingBuffer.GetMemoryRequirements();
            int vertexStagingMemoryTypeIndex = ctx.MemoryProperties.MemoryTypes.IndexOf(
                vertexStagingReq.MemoryTypeBits,
                MemoryProperties.HostVisible | MemoryProperties.HostCoherent);
            DeviceMemory vertexStagingMemory = ctx.Device.AllocateMemory(new MemoryAllocateInfo(vertexStagingReq.Size, vertexStagingMemoryTypeIndex));
            IntPtr vertexPtr = vertexStagingMemory.Map(0, vertexStagingReq.Size);
            Interop.Write(vertexPtr, vertices);
            vertexStagingMemory.Unmap();
            vertexStagingBuffer.BindMemory(vertexStagingMemory);

            // Create a device local buffer where the vertex data will be copied and which will be used for rendering.
            Buffer buffer = ctx.Device.CreateBuffer(new BufferCreateInfo(vertexBufferSize, BufferUsages.VertexBuffer | BufferUsages.TransferDst));
            MemoryRequirements vertexReq = buffer.GetMemoryRequirements();
            int vertexMemoryTypeIndex = ctx.MemoryProperties.MemoryTypes.IndexOf(
                vertexReq.MemoryTypeBits,
                MemoryProperties.DeviceLocal);
            DeviceMemory memory = ctx.Device.AllocateMemory(new MemoryAllocateInfo(vertexReq.Size, vertexMemoryTypeIndex));
            buffer.BindMemory(memory);

            // Copy the data from staging buffers to device local buffers.
            CommandBuffer cmdBuffer = ctx.CommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0];
            cmdBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.OneTimeSubmit));
            cmdBuffer.CmdCopyBuffer(vertexStagingBuffer, buffer, new BufferCopy(vertexBufferSize));
            cmdBuffer.End();

            // Submit.
            Fence fence = ctx.Device.CreateFence();
            ctx.GraphicsQueue.Submit(new SubmitInfo(commandBuffers: new[] { cmdBuffer }), fence);
            fence.Wait();

            // Cleanup.
            fence.Dispose();
            cmdBuffer.Dispose();
            vertexStagingBuffer.Dispose();
            vertexStagingMemory.Dispose();

            return new VulkanBuffer(buffer, memory, vertices.Length);
        }
    }
}
