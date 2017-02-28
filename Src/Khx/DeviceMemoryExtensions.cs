using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore.Khx
{
    // TODO: doc
    public static unsafe class DeviceMemoryExtensions
    {
        /// <summary>
        /// Get a Windows HANDLE for a memory object.
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="handleType"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static IntPtr GetWin32HandleKhx(this DeviceMemory memory, ExternalMemoryHandleTypesKhx handleType)
        {
            IntPtr handle;
            Result result = GetMemoryWin32HandleKhx(memory.Parent, memory, handleType, &handle);
            VulkanException.ThrowForInvalidResult(result);
            return handle;
        }

        /// <summary>
        /// Get a POSIX file descriptor for a memory object.
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="handleType"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static int GetFdKhx(this DeviceMemory memory, ExternalMemoryHandleTypesKhx handleType)
        {
            int fd;
            Result result = GetMemoryFdKhx(memory.Parent, memory, handleType, &fd);
            VulkanException.ThrowForInvalidResult(result);
            return fd;
        }

        [DllImport(VulkanDll, EntryPoint = "vkGetMemoryWin32HandleKHX", CallingConvention = CallConv)]
        private static extern Result GetMemoryWin32HandleKhx(IntPtr device, long memory, ExternalMemoryHandleTypesKhx handleType, IntPtr* handle);

        [DllImport(VulkanDll, EntryPoint = "vkGetMemoryFdKHX", CallingConvention = CallConv)]
        private static extern Result GetMemoryFdKhx(IntPtr device, long memory, ExternalMemoryHandleTypesKhx handleType, int* fd);
    }
}
