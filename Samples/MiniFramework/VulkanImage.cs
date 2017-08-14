using System;
using System.Linq;

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

    public class VulkanImage : IDisposable
    {
        private VulkanImage(Image image, DeviceMemory memory, ImageView view, Format format)
        {
            Image = image;
            Memory = memory;
            View = view;
            Format = format;
        }

        public Format Format { get; }
        public Image Image { get; }
        public ImageView View { get; }
        public DeviceMemory Memory { get; }

        public void Dispose()
        {
            View.Dispose();
            Memory.Dispose();
            Image.Dispose();
        }

        public static implicit operator Image(VulkanImage value) => value.Image;

        public static VulkanImage DepthStencil(VulkanContext device, int width, int height)
        {
            Format[] validFormats =
            {
                Format.D32SFloatS8UInt,
                Format.D32SFloat,
                Format.D24UNormS8UInt,
                Format.D16UNormS8UInt,
                Format.D16UNorm
            };

            Format? potentialFormat = validFormats.FirstOrDefault(
                validFormat =>
                {
                    FormatProperties formatProps = device.PhysicalDevice.GetFormatProperties(validFormat);
                    return (formatProps.OptimalTilingFeatures & FormatFeatures.DepthStencilAttachment) > 0;
                });

            if (!potentialFormat.HasValue)
                throw new InvalidOperationException("Required depth stencil format not supported.");

            Format format = potentialFormat.Value;

            Image image = device.Device.CreateImage(new ImageCreateInfo
            {
                ImageType = ImageType.Image2D,
                Format = format,
                Extent = new Extent3D(width, height, 1),
                MipLevels = 1,
                ArrayLayers = 1,
                Samples = SampleCounts.Count1,
                Tiling = ImageTiling.Optimal,
                Usage = ImageUsages.DepthStencilAttachment | ImageUsages.TransferSrc
            });
            MemoryRequirements memReq = image.GetMemoryRequirements();
            int heapIndex = device.MemoryProperties.MemoryTypes.IndexOf(
                memReq.MemoryTypeBits, MemoryProperties.DeviceLocal);
            DeviceMemory memory = device.Device.AllocateMemory(new MemoryAllocateInfo(memReq.Size, heapIndex));
            image.BindMemory(memory);
            ImageView view = image.CreateView(new ImageViewCreateInfo(format,
                new ImageSubresourceRange(ImageAspects.Depth | ImageAspects.Stencil, 0, 1, 0, 1)));

            return new VulkanImage(image, memory, view, format);
        }

        internal static VulkanImage Texture2D(VulkanContext ctx, TextureData tex2D)
        {
            Buffer stagingBuffer = ctx.Device.CreateBuffer(
                new BufferCreateInfo(tex2D.Mipmaps[0].Size, BufferUsages.TransferSrc));
            MemoryRequirements stagingMemReq = stagingBuffer.GetMemoryRequirements();
            int heapIndex = ctx.MemoryProperties.MemoryTypes.IndexOf(
                stagingMemReq.MemoryTypeBits, MemoryProperties.HostVisible);
            DeviceMemory stagingMemory = ctx.Device.AllocateMemory(
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
            Image image = ctx.Device.CreateImage(new ImageCreateInfo
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
            MemoryRequirements imageMemReq = image.GetMemoryRequirements();
            int imageHeapIndex = ctx.MemoryProperties.MemoryTypes.IndexOf(
                imageMemReq.MemoryTypeBits, MemoryProperties.DeviceLocal);
            DeviceMemory memory = ctx.Device.AllocateMemory(new MemoryAllocateInfo(imageMemReq.Size, imageHeapIndex));
            image.BindMemory(memory);

            var subresourceRange = new ImageSubresourceRange(ImageAspects.Color, 0, tex2D.Mipmaps.Length, 0, 1);

            // Copy the data from staging buffers to device local buffers.
            CommandBuffer cmdBuffer = ctx.GraphicsCommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0];
            cmdBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.OneTimeSubmit));
            cmdBuffer.CmdPipelineBarrier(PipelineStages.TopOfPipe, PipelineStages.Transfer,
                imageMemoryBarriers: new[]
                {
                    new ImageMemoryBarrier(
                        image, subresourceRange,
                        0, Accesses.TransferWrite,
                        ImageLayout.Undefined, ImageLayout.TransferDstOptimal)
                });
            cmdBuffer.CmdCopyBufferToImage(stagingBuffer, image, ImageLayout.TransferDstOptimal, bufferCopyRegions);
            cmdBuffer.CmdPipelineBarrier(PipelineStages.Transfer, PipelineStages.FragmentShader,
                imageMemoryBarriers: new[]
                {
                    new ImageMemoryBarrier(
                        image, subresourceRange,
                        Accesses.TransferWrite, Accesses.ShaderRead,
                        ImageLayout.TransferDstOptimal, ImageLayout.ShaderReadOnlyOptimal)
                });
            cmdBuffer.End();

            // Submit.
            Fence fence = ctx.Device.CreateFence();
            ctx.GraphicsQueue.Submit(new SubmitInfo(commandBuffers: new[] { cmdBuffer }), fence);
            fence.Wait();

            // Cleanup staging resources.
            fence.Dispose();
            stagingMemory.Dispose();
            stagingBuffer.Dispose();

            // Create image view.
            ImageView view = image.CreateView(new ImageViewCreateInfo(tex2D.Format, subresourceRange));

            return new VulkanImage(image, memory, view, tex2D.Format);
        }
    }
}
