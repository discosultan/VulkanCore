using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class ImageTest : HandleTestBase
    {
        private const int DefaultWidth = 32;
        private const int DefaultHeight = 32;
        private const int DefaultBytesPerPx = 4;

        [Fact]
        public void GetSubresourceLayout_Succeeds()
        {
            using (Image image = CreateImage())
            {
                SubresourceLayout layout =
                    image.GetSubresourceLayout(new ImageSubresource(ImageAspects.Color, 0, 0));
                Assert.Equal(DefaultWidth * DefaultHeight * DefaultBytesPerPx, layout.Size);
            }
        }

        [Fact]
        public void BindMemoryAndCreateView_Succeeds()
        {
            using (Image image = CreateImage())
            {
                PhysicalDeviceMemoryProperties deviceMemProps = PhysicalDevice.GetMemoryProperties();
                MemoryRequirements memReq = image.GetMemoryRequirements();

                using (DeviceMemory memory = Device.AllocateMemory(new MemoryAllocateInfo(
                    memReq.Size, 
                    deviceMemProps.GetMemoryTypeIndex(memReq.MemoryTypeBits, 0))))
                {
                    image.BindMemory(memory);
                    Assert.Equal(memory, image.BackingMemory);

                    var createInfo = new ImageViewCreateInfo
                    {
                        Format = Format.R32UInt,
                        ViewType = ImageViewType.Image2D,
                        SubresourceRange = new ImageSubresourceRange
                        {
                            AspectMask = ImageAspects.Color,
                            LayerCount = 1,
                            LevelCount = 1
                        }
                    };
                    using (image.CreateView(createInfo)) { }
                    using (image.CreateView(createInfo, CustomAllocator)) { }
                }
            }
        }

        [Fact]
        public void GetSparseMemoryRequirements_Succeeds()
        {
            using (Image image = CreateImage(ImageCreateFlags.SparseBinding | ImageCreateFlags.SparseResidency))
            {
                SparseImageMemoryRequirements[] requirements = image.GetSparseMemoryRequirements();
                Assert.True(requirements.Length > 0);
            }
        }


        public ImageTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }

        private Image CreateImage(ImageCreateFlags flags = 0)
        {
            var createInfo = new ImageCreateInfo
            {
                ArrayLayers = 1,
                Extent = new Extent3D(DefaultWidth, DefaultHeight, 1),
                Format = Format.R32UInt,
                ImageType = ImageType.Image2D,
                Usage = ImageUsages.TransferSrc | ImageUsages.Sampled,
                MipLevels = 1,
                Samples = SampleCounts.Count1,
                Flags = flags
            };
            return Device.CreateImage(createInfo);
        }
    }
}