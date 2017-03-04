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
        public static int IndexOf(this MemoryType[] memoryTypes, int memoryTypeBits, MemoryProperties properties)
        {
            int count = memoryTypes?.Length ?? 0;
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
    }
}
