using System;
using System.Numerics;

namespace VulkanCore.Samples.Cube
{
    public class Cube : IDisposable
    {
        private readonly Device _device;
        private readonly PhysicalDeviceMemoryProperties _memoryProperties;

        private Buffer _vertexBuffer;
        private Buffer _indexBuffer;

        public Cube(VulkanApp app)
        {
            _device = app.Device;
            _memoryProperties = app.PhysicalDeviceMemoryProperties;
        }

        public void Initialize()
        {
            CreateVertexAndIndexBuffers();
        }

        public void Dispose()
        {
            _indexBuffer.BackedMemory.Dispose();
            _indexBuffer.Dispose();
            _vertexBuffer.BackedMemory.Dispose();
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
            using (Buffer vertexStagingBuffer = _device.CreateBuffer(new BufferCreateInfo(vertexBufferSize, BufferUsages.TransferSrc)))
            {
                MemoryRequirements memReq = vertexStagingBuffer.GetMemoryRequirements();
                int memoryTypeIndex = _memoryProperties.GetMemoryTypeIndex(
                    memReq.MemoryTypeBits,
                    MemoryProperties.HostVisible | MemoryProperties.HostCoherent);
                using (DeviceMemory stagingMemory = _device.AllocateMemory(new MemoryAllocateInfo(memReq.Size, memoryTypeIndex)))
                {
                    IntPtr ptr = stagingMemory.Map(0, memReq.Size);
                    Interop.Write(ptr, vertices);
                    stagingMemory.Unmap();
                    vertexStagingBuffer.BindMemory(stagingMemory);

                    // Create a device local buffer where the vertex data will be copied and which will be used for rendering.
                    _vertexBuffer = _device.CreateBuffer(new BufferCreateInfo(vertexBufferSize, BufferUsages.VertexBuffer | BufferUsages.TransferDst));
                    memReq = _vertexBuffer.GetMemoryRequirements();
                    memoryTypeIndex = _memoryProperties.GetMemoryTypeIndex(
                        memReq.MemoryTypeBits,
                        MemoryProperties.DeviceLocal);
                    _vertexBuffer.BindMemory(_device.AllocateMemory(new MemoryAllocateInfo(memReq.Size, memoryTypeIndex)));
                }
            }

            // Index buffer.
        }
    }
}
