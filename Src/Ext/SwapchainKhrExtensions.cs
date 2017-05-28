using System;
using VulkanCore.Khr;

namespace VulkanCore.Ext
{
    /// <summary>
    /// Provides extension methods for the <see cref="SwapchainKhr"/> class.
    /// </summary>
    public static unsafe class SwapchainKhrExtensions
    {
        /// <summary>
        /// Query the current value of a surface counter.
        /// <para>
        /// The requested counters become active when the first presentation command for the
        /// associated swapchain is processed by the presentation engine.
        /// </para>
        /// </summary>
        /// <param name="swapchain">The swapchain from which to query the counter value.</param>
        /// <param name="counter">The counter to query.</param>
        /// <returns>The current value of the counter.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static long GetCounterExt(this SwapchainKhr swapchain, SurfaceCountersExt counter)
        {
            long counterValue;
            Result result = vkGetSwapchainCounterEXT(swapchain)(swapchain.Parent, swapchain, counter, &counterValue);
            VulkanException.ThrowForInvalidResult(result);
            return counterValue;
        }

        private delegate Result vkGetSwapchainCounterEXTDelegate(IntPtr device, long swapchain, SurfaceCountersExt counter, long* counterValue);
        private static vkGetSwapchainCounterEXTDelegate vkGetSwapchainCounterEXT(SwapchainKhr swapchain) => swapchain.Parent.GetProc<vkGetSwapchainCounterEXTDelegate>(nameof(vkGetSwapchainCounterEXT));
    }
}
