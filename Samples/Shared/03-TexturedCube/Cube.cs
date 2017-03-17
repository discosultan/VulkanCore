using System;

namespace VulkanCore.Samples.TexturedCube
{
    public class Cube : IDisposable
    {
        private readonly Device _device;
        private readonly CommandPool _cmdPool;
        private readonly Queue _queue;
        private readonly PhysicalDeviceMemoryProperties _memoryProperties;

        private Buffer _vertexBuffer;
        private DeviceMemory _vertexBufferMemory;
        private Buffer _indexBuffer;
        private DeviceMemory _indexBufferMemory;

        public Cube(GraphicsDevice device)
        {
            _device = device.Logical;
            _cmdPool = device.CommandPool;
            _queue = device.GraphicsQueue;
            _memoryProperties = device.MemoryProperties;
        }

        public Buffer VertexBuffer => _vertexBuffer;
        public Buffer IndexBuffer => _indexBuffer;
        public int IndexCount { get; private set; }
        public int VertexCount { get; private set; }

        public void Initialize()
        {
            CreateVertexAndIndexBuffers();
        }

        public void Dispose()
        {
            _indexBufferMemory.Dispose();
            _indexBuffer.Dispose();
            _vertexBufferMemory.Dispose();
            _vertexBuffer.Dispose();
        }

        private void CreateVertexAndIndexBuffers()
        {
            var cube = GeometricPrimitive.Box(1.0f, 1.0f, 1.0f);

            IndexCount = cube.Indices.Length;

            int vertexBufferSize = cube.Vertices.Length * Interop.SizeOf<Vertex>();
            int indexBufferSize = cube.Indices.Length * sizeof(int);

            // Vertex buffer.
            Buffer vertexStagingBuffer = _device.CreateBuffer(new BufferCreateInfo(vertexBufferSize, BufferUsages.TransferSrc));
            MemoryRequirements vertexStagingReq = vertexStagingBuffer.GetMemoryRequirements();
            int vertexStagingMemoryTypeIndex = _memoryProperties.MemoryTypes.IndexOf(
                vertexStagingReq.MemoryTypeBits,
                MemoryProperties.HostVisible | MemoryProperties.HostCoherent);
            DeviceMemory vertexStagingMemory = _device.AllocateMemory(new MemoryAllocateInfo(vertexStagingReq.Size, vertexStagingMemoryTypeIndex));
            IntPtr vertexPtr = vertexStagingMemory.Map(0, vertexStagingReq.Size);
            Interop.Write(vertexPtr, cube.Vertices);
            vertexStagingMemory.Unmap();
            vertexStagingBuffer.BindMemory(vertexStagingMemory);

            // Create a device local buffer where the vertex data will be copied and which will be used for rendering.
            _vertexBuffer = _device.CreateBuffer(new BufferCreateInfo(vertexBufferSize, BufferUsages.VertexBuffer | BufferUsages.TransferDst));
            MemoryRequirements vertexReq = _vertexBuffer.GetMemoryRequirements();
            int vertexMemoryTypeIndex = _memoryProperties.MemoryTypes.IndexOf(
                vertexReq.MemoryTypeBits,
                MemoryProperties.DeviceLocal);
            _vertexBufferMemory = _device.AllocateMemory(new MemoryAllocateInfo(vertexReq.Size, vertexMemoryTypeIndex));
            _vertexBuffer.BindMemory(_vertexBufferMemory);

            // Index buffer.
            Buffer indexStagingBuffer = _device.CreateBuffer(new BufferCreateInfo(indexBufferSize, BufferUsages.TransferSrc));
            MemoryRequirements indexStagingReq = indexStagingBuffer.GetMemoryRequirements();
            int indexStagingMemoryTypeIndex = _memoryProperties.MemoryTypes.IndexOf(
                indexStagingReq.MemoryTypeBits,
                MemoryProperties.HostVisible | MemoryProperties.HostCoherent);
            DeviceMemory indexStagingMemory = _device.AllocateMemory(new MemoryAllocateInfo(indexStagingReq.Size, indexStagingMemoryTypeIndex));
            IntPtr indexPtr = indexStagingMemory.Map(0, indexStagingReq.Size);
            Interop.Write(indexPtr, cube.Indices);
            indexStagingMemory.Unmap();
            indexStagingBuffer.BindMemory(indexStagingMemory);

            // Create a device local buffer.
            _indexBuffer = _device.CreateBuffer(new BufferCreateInfo(indexBufferSize, BufferUsages.IndexBuffer | BufferUsages.TransferDst));
            MemoryRequirements indexReq = _indexBuffer.GetMemoryRequirements();
            int indexMemoryTypeIndex = _memoryProperties.MemoryTypes.IndexOf(
                indexReq.MemoryTypeBits,
                MemoryProperties.DeviceLocal);
            _indexBufferMemory = _device.AllocateMemory(new MemoryAllocateInfo(indexReq.Size, indexMemoryTypeIndex));
            _indexBuffer.BindMemory(_indexBufferMemory);

            // Copy the data from staging buffers to device local buffers.
            CommandBuffer cmdBuffer = _cmdPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0];
            cmdBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.OneTimeSubmit));
            cmdBuffer.CmdCopyBuffer(vertexStagingBuffer, _vertexBuffer, new BufferCopy(vertexBufferSize));
            cmdBuffer.CmdCopyBuffer(indexStagingBuffer, _indexBuffer, new BufferCopy(indexBufferSize));
            cmdBuffer.End();

            // Submit.
            Fence fence = _device.CreateFence();
            _queue.Submit(new SubmitInfo(commandBuffers: new[] { cmdBuffer }), fence);
            fence.Wait();

            // Cleanup.
            fence.Dispose();
            cmdBuffer.Dispose();
            indexStagingBuffer.Dispose();
            indexStagingMemory.Dispose();
            vertexStagingBuffer.Dispose();
            vertexStagingMemory.Dispose();
        }
    }
}
