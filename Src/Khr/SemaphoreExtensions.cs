using System;

namespace VulkanCore.Khr
{
    /// <summary>
    /// Provides Khronos specific extension methods for the <see cref="Semaphore"/> class.
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
        public static IntPtr GetWin32HandleKhr(this Semaphore semaphore, ExternalSemaphoreHandleTypesKhr handleType)
        {
            IntPtr handle;
            Result result = vkGetSemaphoreWin32HandleKHR(semaphore)(semaphore.Parent, semaphore, handleType, &handle);
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
        public static int GetFdKhr(this Semaphore semaphore, ExternalSemaphoreHandleTypesKhr handleType)
        {
            int fd;
            Result result = vkGetSemaphoreFdKHR(semaphore)(semaphore.Parent, semaphore, handleType, &fd);
            VulkanException.ThrowForInvalidResult(result);
            return fd;
        }

        private delegate Result vkGetSemaphoreWin32HandleKHRDelegate(IntPtr device, long semaphore, ExternalSemaphoreHandleTypesKhr handleType, IntPtr* handle);
        private static vkGetSemaphoreWin32HandleKHRDelegate vkGetSemaphoreWin32HandleKHR(Semaphore semaphore) => GetProc<vkGetSemaphoreWin32HandleKHRDelegate>(semaphore, nameof(vkGetSemaphoreWin32HandleKHR));

        private delegate Result vkGetSemaphoreFdKHXDelegate(IntPtr device, long semaphore, ExternalSemaphoreHandleTypesKhr handleType, int* fd);
        private static vkGetSemaphoreFdKHXDelegate vkGetSemaphoreFdKHR(Semaphore semaphore) => GetProc<vkGetSemaphoreFdKHXDelegate>(semaphore, nameof(vkGetSemaphoreFdKHR));

        private static TDelegate GetProc<TDelegate>(Semaphore semaphore, string name) where TDelegate : class => semaphore.Parent.GetProc<TDelegate>(name);
    }
}
