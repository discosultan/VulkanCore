using System;

namespace VulkanCore.Android
{
    /// <summary>
    /// Provides Android specific extension methods for the <see cref="Image"/> class.
    /// </summary>
    public static unsafe class ImageExtensions
    {
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void AcquireAndroid(this Queue queue,
            Image image, int nativeFenceFd, Semaphore semaphore, Fence fence)
        {
            Result result = vkAcquireImageANDROID(queue)(queue, image, nativeFenceFd, semaphore, fence);
            VulkanException.ThrowForInvalidResult(result);
        }

        private delegate Result vkAcquireImageANDROIDDelegate(IntPtr device, long image, int nativeFenceFd, long semaphore, long fence);
        private static vkAcquireImageANDROIDDelegate vkAcquireImageANDROID(Queue queue) => queue.Parent.GetProc<vkAcquireImageANDROIDDelegate>(nameof(vkAcquireImageANDROID));
    }
}
