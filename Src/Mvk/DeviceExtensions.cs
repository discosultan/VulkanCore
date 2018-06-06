using System;

namespace VulkanCore.Mvk
{
    /// <summary>
    /// Provides Brenwill Workshop specific extension methods for the <see cref="Device"/> class.
    /// </summary>
    public static unsafe class DeviceExtensions
    {
        /// <summary>
        /// Get the current <see cref="MVKDeviceConfiguration"/> structure for this device
        /// </summary>
        /// <param name="device">The device to get configuration from.</param>
        /// <returns>The configuration structure</returns>
        public static MVKDeviceConfiguration GetMVKDeviceConfiguration(this Device device)
        {
            MVKDeviceConfiguration configuration;
            vkGetMoltenVKDeviceConfigurationMVK(device)(device, &configuration);
            return configuration;
        }

        /// <summary>
        /// Sets the current <see cref="MVKDeviceConfiguration"/> structure for this device
        /// </summary>
        /// <param name="device">The device to set the configuration to.</param>
        /// <param name="configuration"> Structure containing the configuration parameters.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void SetMVKDeviceConfiguration(this Device device, MVKDeviceConfiguration configuration)
        {
            Result result = vkSetMoltenVKDeviceConfigurationMVK(device)(device, &configuration);
            VulkanException.ThrowForInvalidResult(result);
        }

        private delegate void vkGetMoltenVKDeviceConfigurationMVKDelegate(IntPtr device, MVKDeviceConfiguration* configuration);
        private static vkGetMoltenVKDeviceConfigurationMVKDelegate vkGetMoltenVKDeviceConfigurationMVK(Device device) => device.GetProc<vkGetMoltenVKDeviceConfigurationMVKDelegate>(nameof(vkGetMoltenVKDeviceConfigurationMVK));

        private delegate Result vkSetMoltenVKDeviceConfigurationMVKDelegate(IntPtr device, MVKDeviceConfiguration* configuration);
        private static vkSetMoltenVKDeviceConfigurationMVKDelegate vkSetMoltenVKDeviceConfigurationMVK(Device device) => device.GetProc<vkSetMoltenVKDeviceConfigurationMVKDelegate>(nameof(vkSetMoltenVKDeviceConfigurationMVK));
    }
}
