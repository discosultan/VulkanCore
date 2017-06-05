using System;
using System.Runtime.InteropServices;

namespace VulkanCore.NV
{
    /// <summary>
    /// Provides NVIDIA specific extension methods for the <see cref="PhysicalDevice"/> class.
    /// </summary>
    public static unsafe class PhysicalDeviceExtensions
    {
        /// <summary>
        /// Determine image capabilities compatible with external memory handle types.
        /// </summary>
        /// <param name="physicalDevice">The physical device from which to query the image capabilities.</param>
        /// <param name="format">The image format, corresponding to <see cref="ImageCreateInfo.Format"/>.</param>
        /// <param name="type">The image type, corresponding to <see cref="ImageCreateInfo.ImageType"/>.</param>
        /// <param name="tiling">The image tiling, corresponding to <see cref="ImageCreateInfo.Tiling"/>.</param>
        /// <param name="usage">The intended usage of the image, corresponding to <see cref="ImageCreateInfo.Usage"/>.</param>
        /// <param name="flags">
        /// A bitmask describing additional parameters of the image, corresponding to <see cref="ImageCreateInfo.Flags"/>.
        /// </param>
        /// <param name="externalHandleType">
        /// Either one of the bits from <see cref="ExternalMemoryHandleTypesNV"/>, or <c>0</c>.
        /// </param>
        /// <returns>The structure in which capabilities are returned.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static ExternalImageFormatPropertiesNV GetExternalImageFormatPropertiesNV(this PhysicalDevice physicalDevice,
            Format format, ImageType type, ImageTiling tiling, ImageUsages usage, ImageCreateFlags flags,
            ExternalMemoryHandleTypesNV externalHandleType)
        {
            ExternalImageFormatPropertiesNV properties;
            Result result = vkGetPhysicalDeviceExternalImageFormatPropertiesNV(physicalDevice)
                (physicalDevice, format, type, tiling, usage, flags, externalHandleType, &properties);
            VulkanException.ThrowForInvalidResult(result);
            return properties;
        }

        private delegate Result vkGetPhysicalDeviceExternalImageFormatPropertiesNVDelegate(IntPtr physicalDevice, Format format, ImageType type, ImageTiling tiling, ImageUsages usage, ImageCreateFlags flags, ExternalMemoryHandleTypesNV externalHandleType, ExternalImageFormatPropertiesNV* externalImageFormatProperties);
        private static vkGetPhysicalDeviceExternalImageFormatPropertiesNVDelegate vkGetPhysicalDeviceExternalImageFormatPropertiesNV(PhysicalDevice physicalDevice) => physicalDevice.Parent.GetProc<vkGetPhysicalDeviceExternalImageFormatPropertiesNVDelegate>(nameof(vkGetPhysicalDeviceExternalImageFormatPropertiesNV));
    }

    /// <summary>
    /// Structure specifying external image format properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalImageFormatPropertiesNV
    {
        /// <summary>
        /// Will be filled in as when calling <see
        /// cref="PhysicalDeviceExtensions.vkGetPhysicalDeviceExternalImageFormatPropertiesNV"/>, but
        /// the values returned may vary depending on the external handle type requested.
        /// </summary>
        public ImageFormatProperties ImageFormatProperties;
        /// <summary>
        /// A bitmask of <see cref="ExternalMemoryFeaturesNV"/> indicating properties of the external
        /// memory handle type being queried, or <c>0</c> if the external memory handle type is <c>0</c>.
        /// </summary>
        public ExternalMemoryFeaturesNV ExternalMemoryFeatures;
        /// <summary>
        /// A bitmask of <see cref="ExternalMemoryHandleTypesNV"/> containing a bit set for every
        /// external handle type that may be used to create memory from which the handles of the type
        /// can be exported, or <c>0</c> if the external memory handle type is <c>0</c>.
        /// </summary>
        public ExternalMemoryHandleTypesNV ExportFromImportedHandleTypes;
        /// <summary>
        /// A bitmask of <see cref="ExternalMemoryHandleTypesNV"/> containing a bit set for every
        /// external handle type that may be specified simultaneously with the handle type when
        /// calling <see cref="Device.AllocateMemory"/>, or <c>0</c> if the external memory handle
        /// type is <c>0</c>.
        /// </summary>
        public ExternalMemoryHandleTypesNV CompatibleHandleTypes;
    }

    /// <summary>
    /// Bitmask specifying external memory features.
    /// </summary>
    [Flags]
    public enum ExternalMemoryFeaturesNV
    {
        /// <summary>
        /// Indicates that external memory of the specified type must be created as a dedicated
        /// allocation when used in the manner specified.
        /// </summary>
        DedicatedOnly = 1 << 0,
        /// <summary>
        /// Indicates that the implementation supports exporting handles of the specified type.
        /// </summary>
        Exportable = 1 << 1,
        /// <summary>
        /// Indicates that the implementation supports importing handles of the specified type.
        /// </summary>
        Importable = 1 << 2
    }
}
