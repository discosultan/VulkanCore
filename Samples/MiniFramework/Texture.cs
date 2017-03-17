using System;

namespace VulkanCore.Samples
{
    internal class TextureData
    {
        public Mipmap[] Mipmaps { get; set; }
        public Format Format { get; set; }

        public class Mipmap
        {
            public byte[] Data { get; set; }
            public Extent3D Extent { get; set; }
            public int Size { get; set; }
        }
    }

    public class Texture : IDisposable
    {
        private readonly Image _texture;
        private readonly DeviceMemory _memory;

        public ImageView View { get; }

        internal Texture(GraphicsDevice device, TextureData tex2D)
        {
            FormatProperties formatProps = device.Physical.GetFormatProperties(tex2D.Format);

            Buffer stagingBuffer = device.Logical.CreateBuffer(
                new BufferCreateInfo(tex2D.Mipmaps[0].Size, BufferUsages.TransferSrc));
            MemoryRequirements stagingMemReq = stagingBuffer.GetMemoryRequirements();
            int heapIndex = device.MemoryProperties.MemoryTypes.IndexOf(
                stagingMemReq.MemoryTypeBits, MemoryProperties.HostVisible);
            DeviceMemory stagingMemory = device.Logical.AllocateMemory(
                new MemoryAllocateInfo(stagingMemReq.Size, heapIndex));
            stagingBuffer.BindMemory(stagingMemory);

            IntPtr ptr = stagingMemory.Map(0, stagingMemReq.Size);
            Interop.Write(ptr, tex2D.Mipmaps[0].Data);
            stagingMemory.Unmap();

            // Setup buffer copy regions for each mip level.
            var bufferCopyRegions = new BufferImageCopy[tex2D.Mipmaps.Length];
            int offset = 0;
            for (int i = 0; i < bufferCopyRegions.Length; i++)
            {
                bufferCopyRegions = new[]
                {
                    new BufferImageCopy
                    {
                        ImageSubresource = new ImageSubresourceLayers(ImageAspects.Color, i, 0, 1),
                        ImageExtent = tex2D.Mipmaps[0].Extent,
                        BufferOffset = offset
                    }
                };
                offset += tex2D.Mipmaps[i].Size;
            }

            // Create optimal tiled target image.
            _texture = device.Logical.CreateImage(new ImageCreateInfo
            {
                ImageType = ImageType.Image2D,
                Format = tex2D.Format,
                MipLevels = tex2D.Mipmaps.Length,
                ArrayLayers = 1,
                Samples = SampleCounts.Count1,
                Tiling = ImageTiling.Optimal,
                SharingMode = SharingMode.Exclusive,
                InitialLayout = ImageLayout.Undefined,
                Extent = tex2D.Mipmaps[0].Extent,
                Usage = ImageUsages.Sampled | ImageUsages.TransferDst
            });
            MemoryRequirements imageMemReq = _texture.GetMemoryRequirements();
            int imageHeapIndex = device.MemoryProperties.MemoryTypes.IndexOf(
                imageMemReq.MemoryTypeBits, MemoryProperties.DeviceLocal);
            _memory = device.Logical.AllocateMemory(new MemoryAllocateInfo(imageMemReq.Size, imageHeapIndex));
            _texture.BindMemory(_memory);

            var subresourceRange = new ImageSubresourceRange(ImageAspects.Color, 0, tex2D.Mipmaps.Length, 0, 1);

            // Copy the data from staging buffers to device local buffers.
            CommandBuffer cmdBuffer = device.CommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0];
            cmdBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.OneTimeSubmit));
            cmdBuffer.CmdPipelineBarrier(PipelineStages.TopOfPipe, PipelineStages.TopOfPipe,
                imageMemoryBarriers: new[]
                {
                    new ImageMemoryBarrier(
                        _texture, subresourceRange,
                        0, Accesses.TransferWrite,
                        ImageLayout.Undefined, ImageLayout.TransferDstOptimal)
                });
            cmdBuffer.CmdCopyBufferToImage(stagingBuffer, _texture, ImageLayout.TransferDstOptimal, bufferCopyRegions);
            cmdBuffer.CmdPipelineBarrier(PipelineStages.TopOfPipe, PipelineStages.TopOfPipe,
                imageMemoryBarriers: new[]
                {
                    new ImageMemoryBarrier(
                        _texture, subresourceRange,
                        Accesses.TransferWrite, Accesses.ShaderRead,
                        ImageLayout.TransferDstOptimal, ImageLayout.ShaderReadOnlyOptimal)
                });
            cmdBuffer.End();

            // Submit.
            Fence fence = device.Logical.CreateFence();
            device.GraphicsQueue.Submit(new SubmitInfo(commandBuffers: new[] { cmdBuffer }), fence);
            fence.Wait();

            // Cleanup staging resources.
            fence.Dispose();
            stagingMemory.Dispose();
            stagingBuffer.Dispose();

            // Create image view.
            View = _texture.CreateView(new ImageViewCreateInfo(tex2D.Format, subresourceRange));
        }

        public void Dispose()
        {
            View.Dispose();
            _memory.Dispose();
            _texture.Dispose();
        }
    }
}
