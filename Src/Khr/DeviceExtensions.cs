namespace VulkanCore.Khr
{
    /// <summary>
    /// Provides Khronos specific extension methods for the <see cref="Device"/> class.
    /// </summary>
    public static class DeviceExtensions
    {
        /// <summary>
        /// Create a swapchain.
        /// </summary>
        /// <param name="device">The device to create the swapchain for.</param>
        /// <param name="createInfo">The structure specifying the parameters of the created swapchain.</param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the swapchain object when there is no
        /// more specific allocator available.
        /// </param>
        /// <returns>Created swapchain object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SwapchainKhr CreateSwapchainKhr(this Device device, SwapchainCreateInfoKhr createInfo,
            AllocationCallbacks? allocator = null)
        {
            return new SwapchainKhr(device, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Create multiple swapchains that share presentable images.
        /// <para>
        /// Is similar to <see cref="CreateSwapchainKhr"/>, except that it takes an array of <see
        /// cref="SwapchainCreateInfoKhr"/> structures, and returns an array of swapchain objects.
        /// </para>
        /// <para>
        /// The swapchain creation parameters that affect the properties and number of presentable
        /// images must match between all the swapchains.If the displays used by any of the
        /// swapchains do not use the same presentable image layout or are incompatible in a way that
        /// prevents sharing images, swapchain creation will fail with the result code <see
        /// cref="Result.ErrorIncompatibleDisplayKhr"/>. If any error occurs, no swapchains will be
        /// created. Images presented to multiple swapchains must be re-acquired from all of them
        /// before transitioning away from <see cref="ImageLayout.PresentSrcKhr"/>. After destroying
        /// one or more of the swapchains, the remaining swapchains and the presentable images can
        /// continue to be used.
        /// </para>
        /// </summary>
        /// <param name="device">The device to create the swapchains for.</param>
        /// <param name="createInfos">Structures specifying the parameters of the created swapchains.</param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the swapchain objects when there is no
        /// more specific allocator available.
        /// </param>
        /// <returns>The created swapchain objects.</returns>
        public static SwapchainKhr[] CreateSharedSwapchainsKhr(this Device device,
            SwapchainCreateInfoKhr[] createInfos, AllocationCallbacks? allocator = null)
        {
            return SwapchainKhr.CreateSharedKhr(device, createInfos, ref allocator);
        }

        // TODO: doc
        /// <summary>
        /// Create a new descriptor update template.
        /// </summary>
        /// <param name="device">The logical device that creates the descriptor update template.</param>
        /// <param name="createInfo">
        /// Specifies the set of descriptors to update with a single call to flink:vkUpdateDescriptorSetWithTemplateKHR
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting descriptor update template object.</returns>
        public static DescriptorUpdateTemplateKhr CreateDescriptorUpdateTemplateKhr(this Device device,
            DescriptorUpdateTemplateCreateInfoKhr createInfo, AllocationCallbacks? allocator = null)
        {
            return new DescriptorUpdateTemplateKhr(device, ref createInfo, ref allocator);
        }
    }
}
