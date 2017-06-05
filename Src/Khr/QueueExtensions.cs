using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Khr
{
    /// <summary>
    /// Provides Khronos specific extension methods for the <see cref="Queue"/> class.
    /// </summary>
    public static unsafe class QueueExtensions
    {
        /// <summary>
        /// Queue an image for presentation.
        /// </summary>
        /// <param name="queue">
        /// The queue that is capable of presentation to the target surface's platform on the same
        /// device as the image's swapchain.
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
                Result result = vkQueuePresentKHR(queue, &nativePresentInfo);
                VulkanException.ThrowForInvalidResult(result);
            }
        }

        /// <summary>
        /// Queue an image for presentation.
        /// </summary>
        /// <param name="queue">
        /// The queue that is capable of presentation to the target surface's platform on the same
        /// device as the image's swapchain.
        /// </param>
        /// <param name="waitSemaphore">Semaphore to wait for before presenting.</param>
        /// <param name="swapchain">Valid swapchain handle.</param>
        /// <param name="imageIndex">Index into the array of swapchain's presentable images.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void PresentKhr(this Queue queue, Semaphore waitSemaphore, SwapchainKhr swapchain, int imageIndex)
        {
            long waitSemaphoreHandle = waitSemaphore;
            long swapchainHandle = swapchain;
            var nativePresentInfo = new PresentInfoKhr.Native
            {
                Type = StructureType.PresentInfoKhr,
                WaitSemaphoreCount = waitSemaphoreHandle == 0 ? 0 : 1,
                WaitSemaphores = &waitSemaphoreHandle,
                SwapchainCount = swapchainHandle == 0 ? 0 : 1,
                Swapchains = &swapchainHandle,
                ImageIndices = &imageIndex
            };

            Result result = vkQueuePresentKHR(queue, &nativePresentInfo);
            VulkanException.ThrowForInvalidResult(result);
        }

        private delegate Result vkQueuePresentKHRDelegate(IntPtr queue, PresentInfoKhr.Native* presentInfo);
        private static readonly vkQueuePresentKHRDelegate vkQueuePresentKHR = VulkanLibrary.GetStaticProc<vkQueuePresentKHRDelegate>(nameof(vkQueuePresentKHR));
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
        /// Indices into the array of each swapchain's presentable images, with swapchain count entries.
        /// <para>
        /// Each entry in this array identifies the image to present on the corresponding entry in
        /// the <see cref="Swapchains"/> array.
        /// </para>
        /// </summary>
        public int[] ImageIndices;
        /// <summary>
        /// <see cref="Result"/> typed elements with swapchain count entries.
        /// <para>
        /// Applications that do not need per-swapchain results can use <c>null</c> for <see cref="Results"/>.
        /// </para>
        /// <para>
        /// If not <c>null</c>, each entry in <see cref="Results"/> will be set to the <see
        /// cref="Result"/> for presenting the swapchain corresponding to the same index in <see cref="Swapchains"/>.
        /// </para>
        /// </summary>
        public Result[] Results;

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentInfoKhr"/> structure.
        /// </summary>
        /// <param name="waitSemaphores">Semaphores to wait for before presenting.</param>
        /// <param name="swapchains">Valid <see cref="SwapchainKhr"/> handles.</param>
        /// <param name="imageIndices">
        /// Indices into the array of each swapchain’s presentable images, with swapchain count entries.
        /// <para>
        /// Each entry in this array identifies the image to present on the corresponding entry in
        /// the <see cref="Swapchains"/> array.
        /// </para>
        /// </param>
        /// <param name="results">
        /// <see cref="Result"/> typed elements with swapchain count entries.
        /// <para>
        /// Applications that do not need per-swapchain results can use <c>null</c> for <see cref="Results"/>.
        /// </para>
        /// <para>
        /// If not <c>null</c>, each entry in <see cref="Results"/> will be set to the <see
        /// cref="Result"/> for presenting the swapchain corresponding to the same index in <see cref="Swapchains"/>.
        /// </para>
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

    /// <summary>
    /// Structure describing parameters of a queue presentation to a swapchain.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayPresentInfoKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A rectangular region of pixels to present.
        /// <para>It must be a subset of the image being presented.</para>
        /// <para>
        /// If <see cref="DisplayPresentInfoKhr"/> is not specified, this region will be assumed to
        /// be the entire presentable image.
        /// </para>
        /// </summary>
        public Rect2D SrcRect;
        /// <summary>
        /// A rectangular region within the visible region of the swapchain's display mode.
        /// <para>
        /// If <see cref="DisplayPresentInfoKhr"/> is not specified, this region will be assumed to
        /// be the entire visible region of the visible region of the swapchain's mode.
        /// </para>
        /// <para>
        /// If the specified rectangle is a subset of the display mode's visible region, content from
        /// display planes below the swapchain's plane will be visible outside the rectangle.
        /// </para>
        /// <para>
        /// If there are no planes below the swapchain's, the area outside the specified rectangle
        /// will be black.
        /// </para>
        /// <para>
        /// If portions of the specified rectangle are outside of the display's visible region,
        /// pixels mapping only to those portions of the rectangle will be discarded.
        /// </para>
        /// </summary>
        public Rect2D DstRect;
        /// <summary>
        /// If this is <c>true</c>, the display engine will enable buffered mode on displays that
        /// support it. This allows the display engine to stop sending content to the display until a
        /// new image is presented. The display will instead maintain a copy of the last presented
        /// image. This allows less power to be used, but may increase presentation latency.
        /// <para>
        /// If <see cref="DisplayPresentInfoKhr"/> is not specified, persistent mode will not be used.
        /// </para>
        /// </summary>
        public Bool Persistent;
    }

    /// <summary>
    /// Structure hint of rectangular regions changed by <see cref="QueueExtensions.PresentKhr(Queue, PresentInfoKhr)"/>.
    /// <para>
    /// When the "VK_KHR_incremental_present" extension is enabled, additional fields can be
    /// specified that allow an application to specify that only certain rectangular regions of the
    /// presentable images of a swapchain are changed.
    /// </para>
    /// <para>
    /// This is an optimization hint that a presentation engine may use to only update the region of
    /// a surface that is actually changing. The application still must ensure that all pixels of a
    /// presented image contain the desired values, in case the presentation engine ignores this hint.
    /// </para>
    /// <para>
    /// An application can provide this hint by including the <see cref="PresentRegionsKhr"/>
    /// structure in the <see cref="PresentInfoKhr.Next"/> chain.
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct PresentRegionsKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The number of swapchains being presented to by this command.
        /// </summary>
        public int SwapchainCount;
        /// <summary>
        /// Is <c>null</c> or a pointer to an array of <see cref="PresentRegionKhr"/> elements with
        /// <see cref="SwapchainCount"/> entries. If not <c>null</c>, each element of <see
        /// cref="Regions"/> contains the region that has changed since the last present to the
        /// swapchain in the corresponding entry in the <see cref="PresentInfoKhr.Swapchains"/> array.
        /// </summary>
        public PresentRegionKhr* Regions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentRegionsKhr"/> structure.
        /// </summary>
        /// <param name="swapchainCount">The number of swapchains being presented to by this command.</param>
        /// <param name="regions">
        /// Is <c>null</c> or a pointer to an array of <see cref="PresentRegionKhr"/> elements with
        /// <see cref="SwapchainCount"/> entries. If not <c>null</c>, each element of <see
        /// cref="Regions"/> contains the region that has changed since the last present to the
        /// swapchain in the corresponding entry in the <see cref="PresentInfoKhr.Swapchains"/> array.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public PresentRegionsKhr(int swapchainCount, PresentRegionKhr* regions, IntPtr next = default(IntPtr))
        {
            Type = StructureType.PresentRegionsKhr;
            Next = next;
            SwapchainCount = swapchainCount;
            Regions = regions;
        }
    }

    /// <summary>
    /// Structure containing rectangular region changed by <see
    /// cref="QueueExtensions.PresentKhr(Queue, PresentInfoKhr)"/> for a given <see cref="Image"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct PresentRegionKhr
    {
        /// <summary>
        /// The number of rectangles in <see cref="Rectangles"/>, or zero if the entire image has
        /// changed and should be presented.
        /// </summary>
        public int RectangleCount;
        /// <summary>
        /// Is either <c>null</c> or a pointer to an array of <see cref="RectLayerKhr"/> structures.
        /// <para>
        /// The <see cref="RectLayerKhr"/> structure is the framebuffer coordinates, plus layer, of a
        /// portion of a presentable image that has changed and must be presented. If non-
        /// <c>null</c>, each entry in <see cref="Rectangles"/> is a rectangle of the given image
        /// that has changed since the last image was presented to the given swapchain.
        /// </para>
        /// </summary>
        public RectLayerKhr* Rectangles;

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentRegionKhr"/> structure.
        /// </summary>
        /// <param name="rectangleCount">
        /// The number of rectangles in <see cref="Rectangles"/>, or zero if the entire image has
        /// changed and should be presented.
        /// </param>
        /// <param name="rectangles">
        /// Is either <c>null</c> or a pointer to an array of <see cref="RectLayerKhr"/> structures.
        /// <para>
        /// The <see cref="RectLayerKhr"/> structure is the framebuffer coordinates, plus layer, of a
        /// portion of a presentable image that has changed and must be presented. If non-
        /// <c>null</c>, each entry in <see cref="Rectangles"/> is a rectangle of the given image
        /// that has changed since the last image was presented to the given swapchain.
        /// </para>
        /// </param>
        public PresentRegionKhr(int rectangleCount, RectLayerKhr* rectangles)
        {
            RectangleCount = rectangleCount;
            Rectangles = rectangles;
        }
    }

    /// <summary>
    /// Structure containing a rectangle, including layer, changed by <see
    /// cref="QueueExtensions.PresentKhr(Queue, PresentInfoKhr)"/> for a given <see cref="Image"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RectLayerKhr
    {
        /// <summary>
        /// The origin of the rectangle, in pixels.
        /// </summary>
        public Offset2D Offset;
        /// <summary>
        /// The size of the rectangle, in pixels.
        /// </summary>
        public Extent2D Extent;
        /// <summary>
        /// The layer of the image.
        /// <para>For images with only one layer, the value of <see cref="Layer"/> must be 0.</para>
        /// </summary>
        public int Layer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RectLayerKhr"/> structure.
        /// </summary>
        /// <param name="offset">The origin of the rectangle, in pixels.</param>
        /// <param name="extent">The size of the rectangle, in pixels.</param>
        /// <param name="layer">
        /// The layer of the image.
        /// <para>For images with only one layer, the value of <see cref="Layer"/> must be 0.</para>
        /// </param>
        public RectLayerKhr(Offset2D offset, Extent2D extent, int layer)
        {
            Offset = offset;
            Extent = extent;
            Layer = layer;
        }
    }
}
