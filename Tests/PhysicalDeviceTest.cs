using System.Linq;
using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class PhysicalDeviceTest : HandleTestBase
    {
        [Fact]
        public void GetProperties()
        {
            PhysicalDeviceProperties properties = PhysicalDevice.GetProperties();
            Assert.True(properties.DeviceName.Length > 0);
        }

        [Fact]
        public void GetQueueFamilyProperties()
        {
            QueueFamilyProperties[] properties = PhysicalDevice.GetQueueFamilyProperties();
            Assert.True(properties.Length > 0);
        }

        [Fact]
        public void GetMemoryProperties()
        {
            PhysicalDeviceMemoryProperties properties = PhysicalDevice.GetMemoryProperties();
            Assert.True(properties.MemoryHeaps.Length > 0);
            Assert.True(properties.MemoryHeaps.All(x => x.Size > 0));
            Assert.True(properties.MemoryTypes.Length > 0);
            Assert.True(properties.MemoryTypes.All(x => 
                x.HeapIndex >= 0 && 
                x.HeapIndex < properties.MemoryHeaps.Length));
        }

        [Fact]
        public void GetFeatures()
        {
            PhysicalDevice.GetFeatures();
        }

        [Fact]
        public void GetImageFormatFeatures()
        {
            PhysicalDevice.GetImageFormatProperties(Format.B8G8R8A8UNorm, ImageType.Image2D,
                ImageTiling.Optimal, ImageUsages.ColorAttachment);
        }

        [Fact]
        public void EnumerateExtensionProperties()
        {
            ExtensionProperties[] properties = PhysicalDevice.EnumerateExtensionProperties();
            Assert.True(properties.Length > 0);
        }

        [Fact]
        public void EnumerateLayerProperties()
        {
            LayerProperties[] properties = PhysicalDevice.EnumerateLayerProperties();
            Assert.True(properties.Length > 0);
        }

        [Fact]
        public void CreateDevice()
        {
            // Required in order to keep the validation layer happy :)
            PhysicalDevice.GetQueueFamilyProperties();

            var createInfo = new DeviceCreateInfo(new[]
            {
                new DeviceQueueCreateInfo(0, 1, 1.0f)
            });
            using (PhysicalDevice.CreateDevice(createInfo)) { }
            using (PhysicalDevice.CreateDevice(createInfo, CustomAllocator)) { }
        }

        [Fact]
        public void GetSparseImageFormatProperties()
        {
            PhysicalDevice.GetSparseImageFormatProperties(
                Format.B8G8R8A8UNorm,
                ImageType.Image2D,
                SampleCounts.Count1,
                ImageUsages.ColorAttachment,
                ImageTiling.Linear);
        }

        [Fact]
        public void GetFormatProperties()
        {
            PhysicalDevice.GetFormatProperties(Format.B8G8R8A8UNorm);
        }

        public PhysicalDeviceTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }
    }
}
