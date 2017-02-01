using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constants;

namespace VulkanCore.Khr
{
    /// <summary>
    /// Provides extension methods for the <see cref="Queue"/> class.
    /// </summary>
    public static unsafe class QueueExtensions
    {
        /// <summary>
        /// Queue an image for presentation.
        /// </summary>
        /// <param name="queue">
        /// The queue that is capable of presentation to the target surface’s platform on the same
        /// device as the image’s swapchain.
        /// </param>
        /// <param name="presentInfo">The structure specifying the parameters of the presentation.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void PresentKhr(this Queue queue, PresentInfoKhr presentInfo)
        {
            fixed (long* waitSemaphoresPtr = presentInfo.WaitSemaphores)
            fixed (long* swapchainsPtr = presentInfo.Swapchains)
            fixed (int* imageIndicesPtr = presentInfo.ImageIndices)
            fixed (Result* resultsPtr = presentInfo.Results)
            {
                presentInfo.ToNative(out PresentInfoKhr.Native nativePresentInfo,
                    waitSemaphoresPtr,
                    swapchainsPtr,
                    imageIndicesPtr,
                    resultsPtr);
                Result result = QueuePresentKhr(queue, &nativePresentInfo);
                VulkanException.ThrowForInvalidResult(result);
            }            
        }

        [DllImport(VulkanDll, EntryPoint = "vkQueuePresentKHR", CallingConvention = CallConv)]
        private static extern Result QueuePresentKhr(IntPtr queue, PresentInfoKhr.Native* presentInfo);
    }

    /// <summary>
    /// Structure describing parameters of a queue presentation.
    /// </summary>
    public unsafe struct PresentInfoKhr
    {
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Semaphores to wait for before presenting.
        /// </summary>
        public long[] WaitSemaphores;
        /// <summary>
        /// Valid <see cref="SwapchainKhr"/> handles.
        /// </summary>
        public long[] Swapchains;
        /// <summary>
        /// Indices into the array of each swapchain’s presentable images, with swapchain count
        /// entries. Each entry in this array identifies the image to present on the corresponding
        /// entry in the <see cref="Swapchains"/> array.
        /// </summary>
        public int[] ImageIndices;
        /// <summary>
        /// Result typed elements with swapchain count entries. Applications that do not need
        /// per-swapchain results can use <c>null</c> for <see cref="Results"/>. If not <c>null</c>,
        /// each entry in <see cref="Results"/> will be set to the result for presenting the
        /// swapchain corresponding to the same index in <see cref="Swapchains"/>.
        /// </summary>
        public Result[] Results;

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentInfoKhr"/> structure.
        /// </summary>
        /// <param name="waitSemaphores">Semaphores to wait for before presenting.</param>
        /// <param name="swapchains">Valid <see cref="SwapchainKhr"/> handles.</param>
        /// <param name="imageIndices">
        /// Indices into the array of each swapchain’s presentable images, with swapchain count
        /// entries. Each entry in this array identifies the image to present on the corresponding
        /// entry in the <see cref="Swapchains"/> array.
        /// </param>
        /// <param name="results">
        /// Result typed elements with swapchain count entries. Applications that do not need
        /// per-swapchain results can use <c>null</c> for <see cref="Results"/>. If not <c>null</c>,
        /// each entry in <see cref="Results"/> will be set to the result for presenting the
        /// swapchain corresponding to the same index in <see cref="Swapchains"/>.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public PresentInfoKhr(Semaphore[] waitSemaphores, SwapchainKhr[] swapchains, int[] imageIndices, 
            Result[] results = null, IntPtr next = default(IntPtr))
        {
            Next = next;
            WaitSemaphores = waitSemaphores?.ToHandleArray();
            Swapchains = swapchains?.ToHandleArray();
            ImageIndices = imageIndices;
            Results = results;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public int WaitSemaphoreCount;
            public long* WaitSemaphores;
            public int SwapchainCount;
            public long* Swapchains;
            public int* ImageIndices;
            public Result* Results;
        }

        internal void ToNative(out Native native, 
            long* waitSemaphores, long* swapchains, int* imageIndices, Result* results)
        {
            native.Type = StructureType.PresentInfoKhr;
            native.Next = Next;
            native.WaitSemaphoreCount = WaitSemaphores?.Length ?? 0;
            native.WaitSemaphores = waitSemaphores;
            native.SwapchainCount = Swapchains?.Length ?? 0;
            native.Swapchains = swapchains;
            native.ImageIndices = imageIndices;
            native.Results = results;
        }
    }
}
