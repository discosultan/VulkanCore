using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore.Khx
{
    // TODO: doc
    public static unsafe class SemaphoreExtensions
    {
        /// <summary>
        /// Get a Windows HANDLE for a semaphore.
        /// </summary>
        /// <param name="semaphore"></param>
        /// <param name="handleType"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static IntPtr GetWin32HandleKhx(this Semaphore semaphore, ExternalSemaphoreHandleTypesKhx handleType)
        {
            IntPtr handle;
            Result result = vkGetSemaphoreWin32HandleKHX(semaphore.Parent, semaphore, handleType, &handle);
            VulkanException.ThrowForInvalidResult(result);
            return handle;
        }

        /// <summary>
        /// Get a POSIX file descriptor handle for a semaphore.
        /// </summary>
        /// <param name="semaphore"></param>
        /// <param name="handleType"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static int GetFdKhx(this Semaphore semaphore, ExternalSemaphoreHandleTypesKhx handleType)
        {
            int fd;
            Result result = vkGetSemaphoreFdKHX(semaphore.Parent, semaphore, handleType, &fd);
            VulkanException.ThrowForInvalidResult(result);
            return fd;
        }

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetSemaphoreWin32HandleKHX(IntPtr device, 
            long semaphore, ExternalSemaphoreHandleTypesKhx handleType, IntPtr* handle);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetSemaphoreFdKHX(IntPtr device, 
            long semaphore, ExternalSemaphoreHandleTypesKhx handleType, int* fd);
    }
}
