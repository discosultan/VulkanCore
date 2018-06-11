using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;

namespace VulkanCore.Khx
{
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
            Type = StructureType.ImageSwapchainCreateInfoKhr;
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
            Type = StructureType.DeviceGroupBindSparseInfo;
            Next = next;
            ResourceDeviceIndex = resourceDeviceIndex;
            MemoryDeviceIndex = memoryDeviceIndex;
        }
    }
}
