using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;

namespace VulkanCore.Khx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryAllocateFlagsInfoKhx
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
        /// A bitmask of flags controlling the allocation.
        /// </summary>
        public MemoryAllocateFlagsKhx Flags;
        /// <summary>
        /// A mask of physical devices in the logical device, indicating that memory must be
        /// allocated on each device in the mask, if <see cref="MemoryAllocateFlagsKhx.DeviceMask"/>
        /// is set.
        /// </summary>
        public int DeviceMask;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryAllocateFlagsInfoKhx"/> structure.
        /// </summary>
        /// <param name="flags">A bitmask of flags controlling the allocation.</param>
        /// <param name="deviceMask">A mask of physical devices in the logical device, indicating that memory must be
        /// allocated on each device in the mask, if <see cref="MemoryAllocateFlagsKhx.DeviceMask"/>
        /// is set.</param>
        /// <param name="next">Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.</param>
        public MemoryAllocateFlagsInfoKhx(MemoryAllocateFlagsKhx flags, int deviceMask, IntPtr next = default(IntPtr))
        {
            Type = StructureType.MemoryAllocateFlagsInfoKhx;
            Next = next;
            Flags = flags;
            DeviceMask = deviceMask;
        }
    }

    /// <summary>
    /// Bitmask specifying flags for a device memory allocation.
    /// </summary>
    [Flags]
    public enum MemoryAllocateFlagsKhx
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that memory will be allocated for the devices in <see cref="MemoryAllocateFlagsInfoKhx.DeviceMask"/>.
        /// </summary>
        DeviceMask = 1 << 0
    }

    /// <summary>
    /// Specify that an image will be bound to swapchain memory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageSwapchainCreateInfoKhx
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
        /// Is 0 or a handle of an <see cref="SwapchainKhr"/> that the image will be bound to.
        /// </summary>
        public long Swapchain;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSwapchainCreateInfoKhx"/> structure.
        /// </summary>
        /// <param name="swapchain">
        /// Is <c>null</c> or a handle of an <see cref="SwapchainKhr"/> that the image will be bound to.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public ImageSwapchainCreateInfoKhx(SwapchainKhr swapchain, IntPtr next = default(IntPtr))
        {
            Type = StructureType.ImageSwapchainCreateInfoKhx;
            Next = next;
            Swapchain = swapchain;
        }
    }

    /// <summary>
    /// Structure indicating which instances are bound.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceGroupBindSparseInfoKhx
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
        /// A device index indicating which instance of the resource is bound.
        /// </summary>
        public int ResourceDeviceIndex;
        /// <summary>
        /// A device index indicating which instance of the memory the resource instance is bound to.
        /// </summary>
        public int MemoryDeviceIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceGroupBindSparseInfoKhx"/> structure.
        /// </summary>
        /// <param name="resourceDeviceIndex">
        /// A device index indicating which instance of the resource is bound.
        /// </param>
        /// <param name="memoryDeviceIndex">
        /// A device index indicating which instance of the memory the resource instance is bound to.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public DeviceGroupBindSparseInfoKhx(int resourceDeviceIndex, int memoryDeviceIndex,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.DeviceGroupBindSparseInfoKhx;
            Next = next;
            ResourceDeviceIndex = resourceDeviceIndex;
            MemoryDeviceIndex = memoryDeviceIndex;
        }
    }
}
