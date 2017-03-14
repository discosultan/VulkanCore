using System;
using System.Runtime.InteropServices;

namespace VulkanCore.NV
{
    /// <summary>
    /// Provides NVIDIA specific extension methods for the <see cref="DeviceMemory"/> class.
    /// </summary>
    public static unsafe class DeviceMemoryExtensions
    {
        /// <summary>
        /// Retrieve Win32 handle to a device memory object.
        /// </summary>
        /// <param name="deviceMemory">Opaque handle to a device memory object.</param>
        /// <param name="handleType">
        /// A bitmask containing a single bit specifying the type of handle requested.
        /// </param>
        /// <returns>A Windows HANDLE.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static IntPtr GetWin32HandleNV(this DeviceMemory deviceMemory,
            ExternalMemoryHandleTypesNV handleType)
        {
            IntPtr handle;
            Result result = vkGetMemoryWin32HandleNV(deviceMemory.Parent, deviceMemory, handleType, &handle);
            VulkanException.ThrowForInvalidResult(result);
            return handle;
        }

        private delegate Result vkGetMemoryWin32HandleNVDelegate(IntPtr device, long memory, ExternalMemoryHandleTypesNV handleType, IntPtr* handle);
        private static readonly vkGetMemoryWin32HandleNVDelegate vkGetMemoryWin32HandleNV = VulkanLibrary.GetProc<vkGetMemoryWin32HandleNVDelegate>(nameof(vkGetMemoryWin32HandleNV));
    }

    /// <summary>
    /// Bitmask specifying memory handle types.
    /// </summary>
    [Flags]
    public enum ExternalMemoryHandleTypesNV
    {
        OpaqueWin32 = 1 << 0,
        OpaqueWin32Kmt = 1 << 1,
        D3D11Image = 1 << 2,
        D3D11ImageKmt = 1 << 3
    }

    /// <summary>
    /// Specify a dedicated memory allocation resource.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DedicatedAllocationMemoryAllocateInfoNV
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
        /// Is 0 or a handle of an <see cref="VulkanCore.Image"/> which this memory will be bound to.
        /// </summary>
        public long Image;
        /// <summary>
        /// Is 0 or a handle of a <see cref="VulkanCore.Buffer"/> which this memory will be bound to.
        /// </summary>
        public long Buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DedicatedAllocationMemoryAllocateInfoNV"/> structure.
        /// </summary>
        /// <param name="image">An image which this memory will be bound to.</param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public DedicatedAllocationMemoryAllocateInfoNV(Image image, IntPtr next = default(IntPtr))
        {
            Type = StructureType.DedicatedAllocationMemoryAllocateInfoNV;
            Next = next;
            Image = image;
            Buffer = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DedicatedAllocationMemoryAllocateInfoNV"/> structure.
        /// </summary>
        /// <param name="buffer">A buffer which this memory will be bound to.</param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public DedicatedAllocationMemoryAllocateInfoNV(Buffer buffer, IntPtr next = default(IntPtr))
        {
            Type = StructureType.DedicatedAllocationMemoryAllocateInfoNV;
            Next = next;
            Image = 0;
            Buffer = buffer;
        }
    }

    /// <summary>
    /// Specify that a buffer is bound to a dedicated memory resource.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DedicatedAllocationBufferCreateInfoNV
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
        /// Indicates whether the buffer will have a dedicated allocation bound to it.
        /// </summary>
        public Bool DedicatedAllocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="DedicatedAllocationBufferCreateInfoNV"/> structure.
        /// </summary>
        /// <param name="dedicatedAllocation">
        /// Indicates whether the buffer will have a dedicated allocation bound to it.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public DedicatedAllocationBufferCreateInfoNV(bool dedicatedAllocation, IntPtr next = default(IntPtr))
        {
            Type = StructureType.DedicatedAllocationBufferCreateInfoNV;
            Next = next;
            DedicatedAllocation = dedicatedAllocation;
        }
    }

    /// <summary>
    /// Specify that an image is bound to a dedicated memory resource.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DedicatedAllocationImageCreateInfoNV
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
        /// Indicates whether the image will have a dedicated allocation bound to it.
        /// </summary>
        public Bool DedicatedAllocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="DedicatedAllocationBufferCreateInfoNV"/> structure.
        /// </summary>
        /// <param name="dedicatedAllocation">
        /// Indicates whether the image will have a dedicated allocation bound to it.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public DedicatedAllocationImageCreateInfoNV(bool dedicatedAllocation, IntPtr next = default(IntPtr))
        {
            Type = StructureType.DedicatedAllocationImageCreateInfoNV;
            Next = next;
            DedicatedAllocation = dedicatedAllocation;
        }
    }
}
