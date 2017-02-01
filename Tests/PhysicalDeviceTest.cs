using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class PhysicalDeviceTest : HandleTestBase
    {
        [Fact]
        public void GetProperties_Succeeds()
        {
            PhysicalDeviceProperties properties = PhysicalDevice.GetProperties();
            Assert.True(properties.DeviceName.Length > 0);
        }

        [Fact]
        public void GetQueueFamilyProperties_Succeeds()
        {
            QueueFamilyProperties[] properties = PhysicalDevice.GetQueueFamilyProperties();
            Assert.True(properties.Length > 0);
        }

        [Fact]
        public void GetMemoryProperties_Succeeds()
        {
            PhysicalDeviceMemoryProperties properties = PhysicalDevice.GetMemoryProperties();
            Assert.True(properties.MemoryHeaps.Length > 0);
            Assert.True(properties.MemoryTypes.Length > 0);
        }

        [Fact]
        public void GetFeatures_Succeeds()
        {
            PhysicalDevice.GetFeatures();
        }

        [Fact]
        public void GetImageFormatFeatures_Succeeds()
        {
            PhysicalDevice.GetImageFormatProperties(Format.B8G8R8A8UNorm, ImageType.Image2D,
                ImageTiling.Optimal, ImageUsages.ColorAttachment);
        }

        [Fact]
        public void EnumerateExtensionProperties_Succeeds()
        {
            ExtensionProperties[] properties = PhysicalDevice.EnumerateExtensionProperties();
            Assert.True(properties.Length > 0);
        }

        [Fact]
        public void EnumerateLayerProperties_Succeeds()
        {
            LayerProperties[] properties = PhysicalDevice.EnumerateLayerProperties();
            Assert.True(properties.Length > 0);
        }

        [Fact]
        public void CreateDevice_Succeeds()
        {
            // Required in order to keep the validation layer happy :)
            PhysicalDevice.GetQueueFamilyProperties();

            var createInfo = new DeviceCreateInfo(new[]
            {
                new DeviceQueueCreateInfo(0, new[] { 1.0f })
            });
            using (PhysicalDevice.CreateDevice(createInfo)) { }
            using (PhysicalDevice.CreateDevice(createInfo, CustomAllocator)) { }
        }

        [Fact]
        public void GetSparseImageFormatProperties_Succeeds()
        {
            PhysicalDevice.GetSparseImageFormatProperties(
                Format.B8G8R8A8UNorm,
                ImageType.Image2D,
                SampleCounts.Count1,
                ImageUsages.ColorAttachment,
                ImageTiling.Linear);
        }

        [Fact]
        public void GetFormatProperties_Succeeds()
        {
            PhysicalDevice.GetFormatProperties(Format.B8G8R8A8UNorm);
        }

        public PhysicalDeviceTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }
    }
}
