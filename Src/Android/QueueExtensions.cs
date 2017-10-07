using System;

namespace VulkanCore.Android
{
    /// <summary>
    /// Provides Android specific extension methods for the <see cref="Queue"/> class.
    /// </summary>
    public static unsafe class QueueExtensions
    {
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static int SignalReleaseImageAndroid(this Queue queue, Semaphore[] waitSemaphores, Image image)
        {
            int count = waitSemaphores?.Length ?? 0;
            var semaphoreHandles = stackalloc long[count];
            for (int i = 0; i < count; i++)
                semaphoreHandles[i] = waitSemaphores[i];

            int nativeFenceFd;
            Result result = vkQueueSignalReleaseImageANDROID(queue)(queue, count, semaphoreHandles, image, &nativeFenceFd);
            VulkanException.ThrowForInvalidResult(result);
            return nativeFenceFd;
        }

        private delegate Result vkQueueSignalReleaseImageANDROIDDelegate(IntPtr queue, int waitSemaphoreCount, long* waitSemaphores, long image, int* nativeFenceFd);
        private static vkQueueSignalReleaseImageANDROIDDelegate vkQueueSignalReleaseImageANDROID(Queue queue) => queue.Parent.GetProc<vkQueueSignalReleaseImageANDROIDDelegate>(nameof(vkQueueSignalReleaseImageANDROID));
    }
}
