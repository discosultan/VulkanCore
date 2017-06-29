using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;
using static VulkanCore.Constant;

namespace VulkanCore.Khx
{
    /// <summary>
    /// Provides experimental Khronos specific extension methods for the <see cref="PhysicalDevice"/> class.
    /// </summary>
    public static unsafe class PhysicalDeviceExtensions
    {
        /// <summary>
        /// Query the external handle types supported by buffers.
        /// </summary>
        /// <param name="physicalDevice">The physical device from which to query the buffer capabilities.</param>
        /// <param name="externalBufferInfo">
        /// Structure, describing the parameters that would be consumed by <see cref="Device.CreateBuffer"/>.
        /// </param>
        /// <returns>A structure in which capabilities are returned.</returns>
        public static ExternalBufferPropertiesKhx GetExternalBufferPropertiesKhx(this PhysicalDevice physicalDevice,
            PhysicalDeviceExternalBufferInfoKhx externalBufferInfo)
        {
            ExternalBufferPropertiesKhx properties;
            vkGetPhysicalDeviceExternalBufferPropertiesKHX(physicalDevice)(physicalDevice, &externalBufferInfo, &properties);
            return properties;
        }

        /// <summary>
        /// Function for querying external semaphore handle capabilities.
        /// <para>Semaphores may support import and export of external semaphore handles.</para>
        /// </summary>
        /// <param name="physicalDevice">
        /// The physical device from which to query the semaphore capabilities.
        /// </param>
        /// <param name="externalSemaphoreInfo">
        /// Describes the parameters that would be consumed by <see cref="Device.CreateSemaphore"/>.
        /// </param>
        /// <returns>Structure in which capabilities are returned.</returns>
        public static ExternalSemaphorePropertiesKhx GetExternalSemaphorePropertiesKhx(this PhysicalDevice physicalDevice,
            PhysicalDeviceExternalSemaphoreInfoKhx externalSemaphoreInfo)
        {
            ExternalSemaphorePropertiesKhx properties;
            vkGetPhysicalDeviceExternalSemaphorePropertiesKHX(physicalDevice)(physicalDevice, &externalSemaphoreInfo, &properties);
            return properties;
        }

        /// <summary>
        /// Query present rectangles for a surface on a physical device.
        /// <para>
        /// When using <see
        /// cref="DeviceGroupPresentModesKhx.DeviceGroupPresentModeLocalMultiDeviceKhx"/>, the
        /// application may need to know which regions of the surface are used when presenting
        /// locally on each physical device.
        /// </para>
        /// <para>
        /// Presentation of swapchain images to this surface need only have valid contents in the
        /// regions returned by this command.
        /// </para>
        /// </summary>
        /// <param name="physicalDevice">The physical device.</param>
        /// <param name="surface">The surface.</param>
        /// <returns>An array of <see cref="Rect2D"/> structures.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static Rect2D[] GetPresentRectanglesKhx(this PhysicalDevice physicalDevice, SurfaceKhr surface)
        {
            int count;
            Result result = vkGetPhysicalDevicePresentRectanglesKHX(physicalDevice)(physicalDevice, surface, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var rectangles = new Rect2D[count];
            fixed (Rect2D* rectanglesPtr = rectangles)
            {
                result = vkGetPhysicalDevicePresentRectanglesKHX(physicalDevice)(physicalDevice, surface, &count, rectanglesPtr);
                VulkanException.ThrowForInvalidResult(result);
                return rectangles;
            }
        }

        private delegate void vkGetPhysicalDeviceExternalBufferPropertiesKHXDelegate(IntPtr physicalDevice, PhysicalDeviceExternalBufferInfoKhx* externalBufferInfo, ExternalBufferPropertiesKhx* externalBufferProperties);
        private static vkGetPhysicalDeviceExternalBufferPropertiesKHXDelegate vkGetPhysicalDeviceExternalBufferPropertiesKHX(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDeviceExternalBufferPropertiesKHXDelegate>(physicalDevice, nameof(vkGetPhysicalDeviceExternalBufferPropertiesKHX));

        private delegate void vkGetPhysicalDeviceExternalSemaphorePropertiesKHXDelegate(IntPtr physicalDevice, PhysicalDeviceExternalSemaphoreInfoKhx* externalSemaphoreInfo, ExternalSemaphorePropertiesKhx* externalSemaphoreProperties);
        private static vkGetPhysicalDeviceExternalSemaphorePropertiesKHXDelegate vkGetPhysicalDeviceExternalSemaphorePropertiesKHX(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDeviceExternalSemaphorePropertiesKHXDelegate>(physicalDevice, nameof(vkGetPhysicalDeviceExternalSemaphorePropertiesKHX));

        private delegate Result vkGetPhysicalDevicePresentRectanglesKHXDelegate(IntPtr physicalDevice, long surface, int* rectCount, Rect2D* rects);
        private static vkGetPhysicalDevicePresentRectanglesKHXDelegate vkGetPhysicalDevicePresentRectanglesKHX(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDevicePresentRectanglesKHXDelegate>(physicalDevice, nameof(vkGetPhysicalDevicePresentRectanglesKHX));

        private static TDelegate GetProc<TDelegate>(PhysicalDevice physicalDevice, string name) where TDelegate : class => physicalDevice.Parent.GetProc<TDelegate>(name);
    }

    /// <summary>
    /// Structure specifying buffer creation parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceExternalBufferInfoKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bitmask describing additional parameters of the buffer, corresponding to <see cref="BufferCreateInfo.Flags"/>.
        /// </summary>
        public BufferCreateFlags Flags;
        /// <summary>
        /// A bitmask describing the intended usage of the buffer, corresponding to <see cref="BufferCreateInfo.Usage"/>.
        /// </summary>
        public BufferUsages Usage;
        /// <summary>
        /// A bit indicating a memory handle type that will be used with the memory associated with
        /// the buffer. See <see cref="ExternalMemoryHandleTypesKhx"/> for details.
        /// </summary>
        public ExternalMemoryHandleTypesKhx HandleType;
    }

    /// <summary>
    /// Structure specifying supported external handle capabilities.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalBufferPropertiesKHX
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies various capabilities of the external handle type when used with the specified
        /// buffer creation parameters.
        /// </summary>
        public ExternalMemoryPropertiesKhx ExternalMemoryProperties;
    }

    /// <summary>
    /// Structure specifying external memory handle type capabilities.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalMemoryPropertiesKhx
    {
        /// <summary>
        /// A bitmask describing the features of handle type. See <see
        /// cref="ExternalMemoryFeaturesKhx"/> below for a description of the possible bits.
        /// </summary>
        public ExternalMemoryFeaturesKhx ExternalMemoryFeatures;
        /// <summary>
        /// A bitmask specifying handle types that can be used to import objects from which handle
        /// type can be exported.
        /// </summary>
        public ExternalMemoryHandleTypesKhx ExportFromImportedHandleTypes;
        /// <summary>
        /// A bitmask specifying handle types which can be specified at the same time as handle type
        /// when creating an image compatible with external memory.
        /// </summary>
        public ExternalMemoryHandleTypesKhx CompatibleHandleTypes;
    }

    /// <summary>
    /// Bitmask specifying features of an external memory handle type.
    /// </summary>
    [Flags]
    public enum ExternalMemoryFeaturesKhx
    {
        /// <summary>
        /// Specifies that images or buffers created with the specified parameters and handle type
        /// must use the mechanisms defined in the "VK_NV_dedicated_allocation" extension to to
        /// create (or import) a dedicated allocation for the image or buffer.
        /// </summary>
        DedicatedOnly = 1 << 0,
        /// <summary>
        /// Specifies that handles of this type can be exported from Vulkan memory objects.
        /// </summary>
        Exportable = 1 << 1,
        /// <summary>
        /// Specifies that handles of this type can be imported as Vulkan memory objects.
        /// </summary>
        Importable = 1 << 2
    }

    /// <summary>
    /// Structure specifying supported external handle capabilities.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalBufferPropertiesKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies various capabilities of the external handle type when used with the specified
        /// buffer creation parameters.
        /// </summary>
        public ExternalMemoryPropertiesKhx ExternalMemoryProperties;
    }

    /// <summary>
    /// Structure specifying semaphore creation parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceExternalSemaphoreInfoKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bit indicating an external semaphore handle type for which capabilities will be returned.
        /// </summary>
        public ExternalSemaphoreHandleTypesKhx HandleType;
    }

    /// <summary>
    /// Structure describing supported external semaphore handle features.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalSemaphorePropertiesKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bitmask specifying handle types that can be used to import objects from which
        /// handleType can be exported.
        /// </summary>
        public ExternalSemaphoreHandleTypesKhx ExportFromImportedHandleTypes;
        /// <summary>
        /// A bitmask specifying handle types which can be specified at the same time as handleType
        /// when creating a semaphore.
        /// </summary>
        public ExternalSemaphoreHandleTypesKhx CompatibleHandleTypes;
        /// <summary>
        /// A bitmask describing the features of handle type.
        /// </summary>
        public ExternalSemaphoreFeaturesKhx ExternalSemaphoreFeatures;
    }

    /// <summary>
    /// Bitfield describing features of an external semaphore handle type.
    /// </summary>
    [Flags]
    public enum ExternalSemaphoreFeaturesKhx
    {
        /// <summary>
        /// Specifies that handles of this type can be exported from Vulkan semaphore objects.
        /// </summary>
        Exportable = 1 << 0,
        /// <summary>
        /// Specifies that handles of this type can be imported as Vulkan semaphore objects.
        /// </summary>
        Importable = 1 << 1
    }

    /// <summary>
    /// Structure specifying IDs related to the physical device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct PhysicalDeviceIdPropertiesKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// An array of size <see cref="UuidSize"/>, containing 8-bit values that represent a
        /// universally unique identifier for the device.
        /// </summary>
        public fixed byte DeviceUuid[UuidSize];
        /// <summary>
        /// An array of size <see cref="UuidSize"/>, containing 8-bit values that represent a
        /// universally unique identifier for the driver build in use by the device.
        /// </summary>
        public fixed byte DriverUuid[UuidSize];
        /// <summary>
        /// A array of size <see cref="LuidSizeKhx"/>, containing 8-bit values that represent a
        /// locally unique identifier for the device.
        /// </summary>
        public fixed byte DeviceLuid[LuidSizeKhx];
        /// <summary>
        /// A boolean value that will be <c>true</c> if <see cref="DeviceLuid"/> contains a valid
        /// LUID, and <c>false</c> if it does not.
        /// </summary>
        public Bool DeviceLuidValid;
    }

    /// <summary>
    /// Structure describing multiview limits that can be supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceMultiviewPropertiesKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Max number of views in a subpass.
        /// </summary>
        public int MaxMultiviewViewCount;
        /// <summary>
        /// Max instance index for a draw in a multiview subpass.
        /// </summary>
        public int MaxMultiviewInstanceIndex;
    }

    /// <summary>
    /// Structure specifying supported external handle properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalImageFormatPropertiesKHX
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies various capabilities of the external handle type when used with the specified
        /// image creation parameters.
        /// </summary>
        public ExternalMemoryPropertiesKhx ExternalMemoryProperties;
    }

    /// <summary>
    /// Structure specifying external image creation parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceExternalImageFormatInfoKhx
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bit indicating a memory handle type that will be used with the memory associated with
        /// the image.
        /// </summary>
        public ExternalMemoryHandleTypesKhx HandleType;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalDeviceExternalImageFormatInfoKhx"/> structure.
        /// </summary>
        /// <param name="handleType">
        /// A bit indicating a memory handle type that will be used with the memory associated with
        /// the image.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public PhysicalDeviceExternalImageFormatInfoKhx(ExternalMemoryHandleTypesKhx handleType,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.PhysicalDeviceExternalImageFormatInfoKhx;
            Next = next;
            HandleType = handleType;
        }
    }
}
