using System;
using System.Linq;

namespace VulkanCore.Samples
{
    public class DepthStencilBuffer : IDisposable
    {
        public Format Format { get; }
        public Image Buffer { get; }
        public ImageView View { get; }
        public DeviceMemory Memory { get; }

        public DepthStencilBuffer(GraphicsDevice device, int width, int height)
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
                format =>
                {
                    FormatProperties formatProps = device.Physical.GetFormatProperties(format);
                    return (formatProps.OptimalTilingFeatures & FormatFeatures.DepthStencilAttachment) > 0;
                });

            if (!potentialFormat.HasValue)
                throw new InvalidOperationException("Required depth stencil format not supported.");

            Format = potentialFormat.Value;

            Buffer = device.Logical.CreateImage(new ImageCreateInfo
            {
                ImageType = ImageType.Image2D,
                Format = Format,
                Extent = new Extent3D(width, height, 1),
                MipLevels = 1,
                ArrayLayers = 1,
                Samples = SampleCounts.Count1,
                Tiling = ImageTiling.Optimal,
                Usage = ImageUsages.DepthStencilAttachment | ImageUsages.TransferSrc
            });
            MemoryRequirements memReq = Buffer.GetMemoryRequirements();
            int heapIndex = device.MemoryProperties.MemoryTypes.IndexOf(
                memReq.MemoryTypeBits, MemoryProperties.DeviceLocal);
            Memory = device.Logical.AllocateMemory(new MemoryAllocateInfo(memReq.Size, heapIndex));
            Buffer.BindMemory(Memory);
            View = Buffer.CreateView(new ImageViewCreateInfo(Format,
                new ImageSubresourceRange(ImageAspects.Depth | ImageAspects.Stencil, 0, 1, 0, 1)));
        }

        public void Dispose()
        {
            View.Dispose();
            Memory.Dispose();
            Buffer.Dispose();
        }

        public static implicit operator Image(DepthStencilBuffer value) => value.Buffer;
    }
}
