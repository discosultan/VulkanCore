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
            Result result = vkGetMemoryWin32HandleKHX(memory.Parent, memory, handleType, &handle);
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
            Result result = vkGetMemoryFdKHX(memory.Parent, memory, handleType, &fd);
            VulkanException.ThrowForInvalidResult(result);
            return fd;
        }

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetMemoryWin32HandleKHX(IntPtr device, long memory, ExternalMemoryHandleTypesKhx handleType, IntPtr* handle);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetMemoryFdKHX(IntPtr device, long memory, ExternalMemoryHandleTypesKhx handleType, int* fd);
    }
}
