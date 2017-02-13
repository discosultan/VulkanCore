using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;
using static VulkanCore.Constant;

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
            Result result = GetSwapchainCounterExt(swapchain.Parent, swapchain, counter, &counterValue);
            VulkanException.ThrowForInvalidResult(result);
            return counterValue;
        }

        [DllImport(VulkanDll, EntryPoint = "vkGetSwapchainCounterEXT", CallingConvention = CallConv)]
        private static extern Result GetSwapchainCounterExt(IntPtr device, long swapchain, SurfaceCountersExt counter, long* counterValue);
    }
}
