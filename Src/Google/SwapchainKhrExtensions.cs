using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;

namespace VulkanCore.Google
{
    /// <summary>
    /// Provides Google specific extension methods for the <see cref="SwapchainKhr"/> class.
    /// </summary>
    public static unsafe class SwapchainKhrExtensions
    {
        /// <summary>
        /// Obtain the RC duration of the PE's display.
        /// </summary>
        /// <param name="swapchain">The swapchain to obtain the refresh duration for.</param>
        /// <returns>An instance of the <see cref="RefreshCycleDurationGoogle"/> structure.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static RefreshCycleDurationGoogle GetRefreshCycleDurationGoogle(this SwapchainKhr swapchain)
        {
            RefreshCycleDurationGoogle properties;
            Result result = vkGetRefreshCycleDurationGOOGLE(swapchain)(swapchain.Parent, swapchain, &properties);
            VulkanException.ThrowForInvalidResult(result);
            return properties;
        }

        /// <summary>
        /// Obtain timing of a previously-presented image.
        /// <para>
        /// The implementation will maintain a limited amount of history of timing information about
        /// previous presents.
        /// </para>
        /// <para>
        /// Because of the asynchronous nature of the presentation engine, the timing information for
        /// a given <see cref="QueueExtensions.PresentKhr(Queue, Semaphore, SwapchainKhr, int)"/>
        /// command will become available some time later.
        /// </para>
        /// <para>These time values can be asynchronously queried, and will be returned if available.</para>
        /// <para>
        /// All time values are in nanoseconds, relative to a monotonically-increasing clock (e.g.
        /// `CLOCK_MONOTONIC` (see clock_gettime(2)) on Android and Linux).
        /// </para>
        /// </summary>
        /// <param name="swapchain">
        /// The swapchain to obtain presentation timing information duration for.
        /// </param>
        /// <returns>An array of <see cref="PastPresentationTimingGoogle"/> structures.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static PastPresentationTimingGoogle[] GetPastPresentationTimingGoogle(this SwapchainKhr swapchain)
        {
            int count;
            Result result = vkGetPastPresentationTimingGOOGLE(swapchain)(swapchain.Parent, swapchain, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var timings = new PastPresentationTimingGoogle[count];
            fixed (PastPresentationTimingGoogle* timingsPtr = timings)
                result = vkGetPastPresentationTimingGOOGLE(swapchain)(swapchain.Parent, swapchain, &count, timingsPtr);
            VulkanException.ThrowForInvalidResult(result);

            return timings;
        }

        private delegate Result vkGetRefreshCycleDurationGOOGLEDelegate(IntPtr device, long swapchain, RefreshCycleDurationGoogle* displayTimingProperties);
        private static vkGetRefreshCycleDurationGOOGLEDelegate vkGetRefreshCycleDurationGOOGLE(SwapchainKhr swapchain) => GetProc<vkGetRefreshCycleDurationGOOGLEDelegate>(swapchain, nameof(vkGetRefreshCycleDurationGOOGLE));

        private delegate Result vkGetPastPresentationTimingGOOGLEDelegate(IntPtr device, long swapchain, int* presentationTimingCount, PastPresentationTimingGoogle* presentationTimings);
        private static vkGetPastPresentationTimingGOOGLEDelegate vkGetPastPresentationTimingGOOGLE(SwapchainKhr swapchain) => GetProc<vkGetPastPresentationTimingGOOGLEDelegate>(swapchain, nameof(vkGetPastPresentationTimingGOOGLE));

        private static TDelegate GetProc<TDelegate>(SwapchainKhr swapchain, string name) where TDelegate : class => swapchain.Parent.GetProc<TDelegate>(name);
    }

    /// <summary>
    /// Structure containing the RC duration of a display.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RefreshCycleDurationGoogle
    {
        /// <summary>
        /// The number of nanoseconds from the start of one refresh cycle to the next.
        /// </summary>
        public long RefreshDuration;
    }

    /// <summary>
    /// Structure containing timing information about a previously-presented image.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PastPresentationTimingGoogle
    {
        /// <summary>
        /// An application-provided value that was given to a previous <see
        /// cref="QueueExtensions.PresentKhr(Queue, PresentInfoKhr)"/> command via <see
        /// cref="PresentId"/> (see below). It can be used to uniquely identify a previous present
        /// with the flink:vkQueuePresentKHR command.
        /// </summary>
        public int PresentId;
        /// <summary>
        /// An application-provided value that was given to a previous <see
        /// cref="QueueExtensions.PresentKhr(Queue, PresentInfoKhr)"/> command via <see
        /// cref="DesiredPresentTime"/>. If non-zero, it was used by the application to indicate that
        /// an image not be presented any sooner than <see cref="DesiredPresentTime"/>.
        /// </summary>
        public long DesiredPresentTime;
        /// <summary>
        /// The time when the image of the swapchain was actually displayed.
        /// </summary>
        public long ActualPresentTime;
        /// <summary>
        /// The time when the image of the swapchain could have been displayed. This may differ from
        /// <see cref="ActualPresentTime"/> if the application requested that the image be presented
        /// no sooner than <see cref="DesiredPresentTime"/>.
        /// </summary>
        public long EarliestPresentTime;
        /// <summary>
        /// An indication of how early the <see cref="QueueExtensions.PresentKhr(Queue, PresentInfoKhr)"/>
        /// command was processed compared to how soon it needed to be processed, and still be
        /// presented at <see cref="EarliestPresentTime"/>.
        /// </summary>
        public long PresentMargin;
    }

    /// <summary>
    /// The earliest time each image should be presented.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct PresentTimesInfoGoogle
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
        /// Is <c>null</c> or a pointer to an array of <see cref="PresentTimeGoogle"/> elements with
        /// <see cref="SwapchainCount"/> entries. If not <c>null</c>, each element of <see
        /// cref="Times"/> contains the earliest time to present the image corresponding to the entry
        /// in the <see cref="PresentInfoKhr.ImageIndices"/> array.
        /// </summary>
        public PresentTimeGoogle* Times;
    }

    /// <summary>
    /// The earliest time image should be presented.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PresentTimeGoogle
    {
        /// <summary>
        /// An application-provided identification value, that can be used with the results of <see
        /// cref="SwapchainKhrExtensions.GetPastPresentationTimingGoogle"/>, in order to uniquely
        /// identify this present. In order to be useful to the application, it should be unique
        /// within some period of time that is meaningful to the application.
        /// </summary>
        public int PresentId;
        /// <summary>
        /// Indicates that the image given should not be displayed to the user any earlier than this
        /// time. <see cref="DesiredPresentTime"/> is a time in nanoseconds, relative to a
        /// monotonically-increasing clock (e.g. `CLOCKMONOTONIC` (see ClockGettime(2)) on Android
        /// and Linux). A value of zero indicates that the presentation engine may display the image
        /// at any time. This is useful when the application desires to provide presentID, but
        /// doesn't need a specific <see cref="DesiredPresentTime"/>.
        /// </summary>
        public long DesiredPresentTime;
    }
}
