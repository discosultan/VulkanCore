using System;
using System.Numerics;

namespace VulkanCore.Samples.Cube
{
    public class Cube : IDisposable
    {
        private readonly Device _device;
        private readonly CommandPool _cmdPool;
        private readonly PhysicalDeviceMemoryProperties _memoryProperties;

        private Buffer _vertexBuffer;
        private DeviceMemory _vertexBufferMemory;
        private Buffer _indexBuffer;
        private DeviceMemory _indexBufferMemory;

        public Cube(VulkanApp app)
        {
            _device = app.Device;
            _cmdPool = app.CommandPool;
            _memoryProperties = app.PhysicalDeviceMemoryProperties;
        }

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
            Vertex[] vertices =
            {
                new Vertex { Position = new Vector3(-1.0f, -1.0f, -1.0f) },
                new Vertex { Position = new Vector3(-1.0f, +1.0f, -1.0f) },
                new Vertex { Position = new Vector3(+1.0f, +1.0f, -1.0f) },
                new Vertex { Position = new Vector3(+1.0f, -1.0f, -1.0f) },
                new Vertex { Position = new Vector3(-1.0f, -1.0f, +1.0f) },
                new Vertex { Position = new Vector3(-1.0f, +1.0f, +1.0f) },
                new Vertex { Position = new Vector3(+1.0f, +1.0f, +1.0f) },
                new Vertex { Position = new Vector3(+1.0f, -1.0f, +1.0f) }
            };

            int[] indices =
            {
                // Front face.
                0, 1, 2,
                0, 2, 3,

                // Back face.
                4, 6, 5,
                4, 7, 6,

                // Left face.
                4, 5, 1,
                4, 1, 0,

                // Right face.
                3, 2, 6,
                3, 6, 7,

                // Top face.
                1, 5, 6,
                1, 6, 2,

                // Bottom face.
                4, 0, 3,
                4, 3, 7
            };

            // Static data like vertex and index buffer should be stored in a device local memory 
            // for optimal (and fastest) access by the GPU.
            //
            // To achieve this, we use so-called "staging buffers":
            // - Create a buffer that's visible to the host (and can be mapped)
            // - Copy the data to this buffer
            // - Create another buffer that's local on the device (VRAM) with the same size
            // - Copy the data from the host to the device using a command buffer
            // - Delete the host visible (staging) buffer
            // - Use the device local buffers for rendering

            int vertexBufferSize = vertices.Length * Interop.SizeOf<Vertex>();
            int indexBufferSize = indices.Length * sizeof(int);

            // Vertex buffer.
            Buffer vertexStagingBuffer = _device.CreateBuffer(new BufferCreateInfo(vertexBufferSize, BufferUsages.TransferSrc));
            MemoryRequirements vertexStagingReq = vertexStagingBuffer.GetMemoryRequirements();
            int vertexStagingMemoryTypeIndex = _memoryProperties.GetMemoryTypeIndex(
                vertexStagingReq.MemoryTypeBits,
                MemoryProperties.HostVisible | MemoryProperties.HostCoherent);
            DeviceMemory vertexStagingMemory = _device.AllocateMemory(new MemoryAllocateInfo(vertexStagingReq.Size, vertexStagingMemoryTypeIndex));
            IntPtr vertexPtr = vertexStagingMemory.Map(0, vertexStagingReq.Size);
            Interop.Write(vertexPtr, vertices);
            vertexStagingMemory.Unmap();
            vertexStagingBuffer.BindMemory(vertexStagingMemory);

            // Create a device local buffer where the vertex data will be copied and which will be used for rendering.
            _vertexBuffer = _device.CreateBuffer(new BufferCreateInfo(vertexBufferSize, BufferUsages.VertexBuffer | BufferUsages.TransferDst));
            MemoryRequirements vertexReq = _vertexBuffer.GetMemoryRequirements();
            int vertexMemoryTypeIndex = _memoryProperties.GetMemoryTypeIndex(
                vertexReq.MemoryTypeBits,
                MemoryProperties.DeviceLocal);
            _vertexBufferMemory = _device.AllocateMemory(new MemoryAllocateInfo(vertexReq.Size, vertexMemoryTypeIndex));
            _vertexBuffer.BindMemory(_vertexBufferMemory);

            // Index buffer.
            Buffer indexStagingBuffer = _device.CreateBuffer(new BufferCreateInfo(indexBufferSize, BufferUsages.TransferSrc));
            MemoryRequirements indexStagingReq = indexStagingBuffer.GetMemoryRequirements();
            int indexStagingMemoryTypeIndex = _memoryProperties.GetMemoryTypeIndex(
                indexStagingReq.MemoryTypeBits,
                MemoryProperties.HostVisible | MemoryProperties.HostCoherent);
            DeviceMemory indexStagingMemory = _device.AllocateMemory(new MemoryAllocateInfo(indexStagingReq.Size, indexStagingMemoryTypeIndex));
            IntPtr indexPtr = indexStagingMemory.Map(0, indexStagingReq.Size);
            Interop.Write(indexPtr, indices);
            indexStagingMemory.Unmap();
            indexStagingBuffer.BindMemory(indexStagingMemory);

            // Create a device local buffer.
            _indexBuffer = _device.CreateBuffer(new BufferCreateInfo(indexBufferSize, BufferUsages.IndexBuffer | BufferUsages.TransferDst));
            MemoryRequirements indexReq = _indexBuffer.GetMemoryRequirements();
            int indexMemoryTypeIndex = _memoryProperties.GetMemoryTypeIndex(
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

            // TODO: submit, flush and destroy staging resources.
        }
    }
}
