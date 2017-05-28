using System;
using Xunit;

namespace VulkanCore.Tests
{
    public class UtilityTest
    {
        [Fact]
        public void IndexOfMemoryTypes()
        {
            MemoryType[] memTypes =
            {
                new MemoryType { HeapIndex = 0, PropertyFlags = MemoryProperties.HostVisible },
                new MemoryType { HeapIndex = 1, PropertyFlags = MemoryProperties.HostVisible | MemoryProperties.HostCoherent },
                new MemoryType { HeapIndex = 2, PropertyFlags = MemoryProperties.HostVisible | MemoryProperties.HostCoherent }
            };
            const int availableMemoryTypeBits = 0b0101;

            int index = memTypes.IndexOf(availableMemoryTypeBits, MemoryProperties.HostVisible | MemoryProperties.HostCoherent);

            Assert.Equal(2, index);
        }

        [Fact]
        public void IndexOfMemoryTypesForNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((MemoryType[])null).IndexOf(~0, 0));
        }

        [Fact]
        public void LayerPropertiesContains()
        {
            LayerProperties[] layerProperties =
            {
                new LayerProperties { LayerName = "name1" },
                new LayerProperties { LayerName = "name2" }
            };

            Assert.True(layerProperties.Contains("name2"));
            Assert.False(layerProperties.Contains("name3"));
        }

        [Fact]
        public void NullLayerPropertiesContains()
        {
            Assert.Throws<ArgumentNullException>(() => ((LayerProperties[])null).Contains("name"));
        }

        [Fact]
        public void ExtensionPropertiesContains()
        {
            ExtensionProperties[] layerProperties =
            {
                new ExtensionProperties { ExtensionName = "name1" },
                new ExtensionProperties { ExtensionName = "name2" }
            };

            Assert.True(layerProperties.Contains("name2"));
            Assert.False(layerProperties.Contains("name3"));
        }

        [Fact]
        public void NullLExtensionPropertiesContains()
        {
            Assert.Throws<ArgumentNullException>(() => ((ExtensionProperties[])null).Contains("name"));
        }
    }
}
