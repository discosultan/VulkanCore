using System;

namespace VulkanCore.Khx
{
    /// <summary>
    /// Provides experimental Khronos specific extension methods for the <see cref="Semaphore"/> class.
    /// </summary>
    public static unsafe class SemaphoreExtensions
    {
        /// <summary>
        /// Get a Windows HANDLE for a semaphore.
        /// </summary>
        /// <param name="semaphore">The semaphore from which state will be exported.</param>
        /// <param name="handleType">The type of handle requested.</param>
        /// <returns>The Windows handle representing the semaphore state.</returns>
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
        /// <param name="semaphore">The semaphore from which state will be exported.</param>
        /// <param name="handleType">The type of handle requested.</param>
        /// <returns>The file descriptor representing the semaphore state.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static int GetFdKhx(this Semaphore semaphore, ExternalSemaphoreHandleTypesKhx handleType)
        {
            int fd;
            Result result = vkGetSemaphoreFdKHX(semaphore.Parent, semaphore, handleType, &fd);
            VulkanException.ThrowForInvalidResult(result);
            return fd;
        }

        private delegate Result vkGetSemaphoreWin32HandleKHXDelegate(IntPtr device, long semaphore, ExternalSemaphoreHandleTypesKhx handleType, IntPtr* handle);
        private static readonly vkGetSemaphoreWin32HandleKHXDelegate vkGetSemaphoreWin32HandleKHX = VulkanLibrary.GetProc<vkGetSemaphoreWin32HandleKHXDelegate>(nameof(vkGetSemaphoreWin32HandleKHX));

        private delegate Result vkGetSemaphoreFdKHXDelegate(IntPtr device, long semaphore, ExternalSemaphoreHandleTypesKhx handleType, int* fd);
        private static readonly vkGetSemaphoreFdKHXDelegate vkGetSemaphoreFdKHX = VulkanLibrary.GetProc<vkGetSemaphoreFdKHXDelegate>(nameof(vkGetSemaphoreFdKHX));
    }
}
