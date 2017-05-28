using System;
using System.Collections.Generic;
using System.Linq;

namespace VulkanCore
{
    /// <summary>
    /// Provides helper methods for various types.
    /// </summary>
    public static class UtilityExtensions
    {
        /// <summary>
        /// Gets the index of the <see cref="MemoryType"/> that has all the requested <paramref
        /// name="properties"/> set or <c>-1</c> if not found.
        /// </summary>
        /// <param name="memoryTypes">
        /// Structures describing the memory types that can be used to access memory allocated from
        /// the heaps specified by <see cref="PhysicalDeviceMemoryProperties.MemoryHeaps"/>.
        /// </param>
        /// <param name="memoryTypeBits">
        /// A bitmask of <see cref="MemoryRequirements.MemoryTypeBits"/> that contains one bit set
        /// for every memory type supported by the resource.
        /// </param>
        /// <param name="properties">A bitmask of properties to request.</param>
        /// <returns>Index of the requested <see cref="MemoryType"/> or <c>-1</c> if not found.</returns>
        /// ///
        /// <exception cref="ArgumentNullException"><paramref name="memoryTypes"/> is <c>null</c>.</exception>
        public static int IndexOf(this IList<MemoryType> memoryTypes, int memoryTypeBits, MemoryProperties properties)
        {
            if (memoryTypes == null)
                throw new ArgumentNullException(nameof(memoryTypes));

            int count = memoryTypes?.Count ?? 0;
            for (int i = 0; i < count; i++)
            {
                if ((memoryTypeBits & 1) == 1 &&
                    (memoryTypes[i].PropertyFlags & properties) == properties)
                {
                    return i;
                }
                memoryTypeBits >>= 1;
            }
            return -1;
        }

        /// <summary>
        /// Determines whether a sequence of <see cref="LayerProperties"/> contains a layer with
        /// specified <paramref name="name"/>.
        /// </summary>
        /// <param name="layers">A sequence in which to locate a layer name.</param>
        /// <param name="name">The layer name to locate in the sequence.</param>
        /// <returns>
        /// <c>true</c> if the source sequence contains an element that has the specified value;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="layers"/> is <c>null</c>.</exception>
        public static bool Contains(this IEnumerable<LayerProperties> layers, string name)
        {
            return layers
                .Select(layer => layer.LayerName)
                .Contains(name, StringComparer.Ordinal);
        }

        /// <summary>
        /// Determines whether a sequence of <see cref="ExtensionProperties"/> contains a layer with
        /// specified <paramref name="name"/>.
        /// </summary>
        /// <param name="extensions">A sequence in which to locate an extension name.</param>
        /// <param name="name">The layer name to locate in the sequence.</param>
        /// <returns>
        /// <c>true</c> if the source sequence contains an element that has the specified value;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="extensions"/> is <c>null</c>.</exception>
        public static bool Contains(this IEnumerable<ExtensionProperties> extensions, string name)
        {
            return extensions
                .Select(extension => extension.ExtensionName)
                .Contains(name, StringComparer.Ordinal);
        }
    }
}
