using System;

namespace VulkanCore.Khr
{
    /// <summary>
    /// Provides Khronos specific extension methods for the <see cref="DeviceMemory"/> class.
    /// </summary>
    public static unsafe class DeviceMemoryExtensions
    {
        /// <summary>
        /// Get a Windows HANDLE for a memory object.
        /// </summary>
        /// <param name="memory">The memory object from which the handle will be exported.</param>
        /// <param name="handleType">The type of handle requested.</param>
        /// <returns>The Windows handle representing the underlying resources of the device memory object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static IntPtr GetWin32HandleKhr(this DeviceMemory memory, ExternalMemoryHandleTypesKhr handleType)
        {
            IntPtr handle;
            Result result = vkGetMemoryWin32HandleKHR(memory)(memory.Parent, memory, handleType, &handle);
            VulkanException.ThrowForInvalidResult(result);
            return handle;
        }

        /// <summary>
        /// Get a POSIX file descriptor for a memory object.
        /// </summary>
        /// <param name="memory">The memory object from which the handle will be exported.</param>
        /// <param name="handleType">The type of handle requested.</param>
        /// <returns>A file descriptor representing the underlying resources of the device memory object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static int GetFdKhr(this DeviceMemory memory, ExternalMemoryHandleTypesKhr handleType)
        {
            int fd;
            Result result = vkGetMemoryFdKHR(memory)(memory.Parent, memory, handleType, &fd);
            VulkanException.ThrowForInvalidResult(result);
            return fd;
        }

        private delegate Result vkGetMemoryWin32HandleKHRDelegate(IntPtr device, long memory, ExternalMemoryHandleTypesKhr handleType, IntPtr* handle);
        private static vkGetMemoryWin32HandleKHRDelegate vkGetMemoryWin32HandleKHR(DeviceMemory memory) => GetProc<vkGetMemoryWin32HandleKHRDelegate>(memory, nameof(vkGetMemoryWin32HandleKHR));

        private delegate Result vkGetMemoryFdKHRDelegate(IntPtr device, long memory, ExternalMemoryHandleTypesKhr handleType, int* fd);
        private static vkGetMemoryFdKHRDelegate vkGetMemoryFdKHR(DeviceMemory memory) => GetProc<vkGetMemoryFdKHRDelegate>(memory, nameof(vkGetMemoryFdKHR));

        private static TDelegate GetProc<TDelegate>(DeviceMemory memory, string name) where TDelegate : class => memory.Parent.GetProc<TDelegate>(name);
    }
}
