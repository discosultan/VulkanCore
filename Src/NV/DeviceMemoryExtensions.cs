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
        /// <returns>A Windows `HANDLE`.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static IntPtr GetMemoryWin32HandleNV(this DeviceMemory deviceMemory, 
            ExternalMemoryHandleTypesNV handleType)
        {
            IntPtr handle;
            Result result = GetMemoryWin32HandleNV(deviceMemory.Parent, deviceMemory, handleType, &handle);
            VulkanException.ThrowForInvalidResult(result);
            return handle;
        }
        
        [DllImport(Constant.VulkanDll, EntryPoint = "vkGetMemoryWin32HandleNV", CallingConvention = Constant.CallConv)]
        private static extern Result GetMemoryWin32HandleNV(IntPtr device, 
            long memory, ExternalMemoryHandleTypesNV handleType, IntPtr* handle);
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
}
