using System;
using VulkanCore.Khr;

namespace VulkanCore.Samples
{
    /// <summary>
    /// Encapsulates Vulkan <see cref="VulkanCore.PhysicalDevice"/> and <see cref="VulkanCore.Device"/> and exposes queues
    /// and a command pool for rendering tasks.
    /// </summary>
    public class VulkanContext : IDisposable
    {
        public VulkanContext(Instance instance, SurfaceKhr surface, Platform platform)
        {
            // Find graphics and presentation capable physical device(s) that support
            // the provided surface for platform.
            int graphicsQueueFamilyIndex = -1;
            int computeQueueFamilyIndex = -1;
            int presentQueueFamilyIndex = -1;
            foreach (PhysicalDevice physicalDevice in instance.EnumeratePhysicalDevices())
            {
                QueueFamilyProperties[] queueFamilyProperties = physicalDevice.GetQueueFamilyProperties();
                for (int i = 0; i < queueFamilyProperties.Length; i++)
                {
                    if (queueFamilyProperties[i].QueueFlags.HasFlag(Queues.Graphics))
                    {
                        if (graphicsQueueFamilyIndex == -1) graphicsQueueFamilyIndex = i;
                        if (computeQueueFamilyIndex == -1) computeQueueFamilyIndex = i;

                        if (physicalDevice.GetSurfaceSupportKhr(i, surface) &&
                            GetPresentationSupport(physicalDevice, i))
                        {
                            presentQueueFamilyIndex = i;
                        }

                        if (graphicsQueueFamilyIndex != -1 &&
                            computeQueueFamilyIndex != -1 &&
                            presentQueueFamilyIndex != -1)
                        {
                            PhysicalDevice = physicalDevice;
                            break;
                        }
                    }
                }
                if (PhysicalDevice != null) break;
            }

            bool GetPresentationSupport(PhysicalDevice physicalDevice, int queueFamilyIndex)
            {
                switch (platform)
                {
                    case Platform.Android:
                        return true;
                    case Platform.Win32:
                        return physicalDevice.GetWin32PresentationSupportKhr(queueFamilyIndex);
                    default:
                        throw new NotImplementedException();
                }
            }

            if (PhysicalDevice == null)
                throw new InvalidOperationException("No suitable physical device found.");

            // Store memory properties of the physical device.
            MemoryProperties = PhysicalDevice.GetMemoryProperties();
            Features = PhysicalDevice.GetFeatures();
            Properties = PhysicalDevice.GetProperties();

            // Create a logical device.
            bool sameGraphicsAndPresent = graphicsQueueFamilyIndex == presentQueueFamilyIndex;
            var queueCreateInfos = new DeviceQueueCreateInfo[sameGraphicsAndPresent ? 1 : 2];
            queueCreateInfos[0] = new DeviceQueueCreateInfo(graphicsQueueFamilyIndex, 1, 1.0f);
            if (!sameGraphicsAndPresent)
                queueCreateInfos[1] = new DeviceQueueCreateInfo(presentQueueFamilyIndex, 1, 1.0f);

            var deviceCreateInfo = new DeviceCreateInfo(
                queueCreateInfos,
                new[] { Constant.DeviceExtension.KhrSwapchain },
                Features);
            Device = PhysicalDevice.CreateDevice(deviceCreateInfo);

            // Get queue(s).
            GraphicsQueue = Device.GetQueue(graphicsQueueFamilyIndex);
            ComputeQueue = computeQueueFamilyIndex == graphicsQueueFamilyIndex
                ? GraphicsQueue
                : Device.GetQueue(computeQueueFamilyIndex);
            PresentQueue = presentQueueFamilyIndex == graphicsQueueFamilyIndex
                ? GraphicsQueue
                : Device.GetQueue(presentQueueFamilyIndex);

            // Create command pool(s).
            GraphicsCommandPool = Device.CreateCommandPool(new CommandPoolCreateInfo(graphicsQueueFamilyIndex));
            ComputeCommandPool = Device.CreateCommandPool(new CommandPoolCreateInfo(computeQueueFamilyIndex));
        }

        public PhysicalDevice PhysicalDevice { get; }
        public Device Device { get; }
        public PhysicalDeviceMemoryProperties MemoryProperties { get; }
        public PhysicalDeviceFeatures Features { get; }
        public PhysicalDeviceProperties Properties { get; }
        public Queue GraphicsQueue { get; }
        public Queue ComputeQueue { get; }
        public Queue PresentQueue { get; }
        public CommandPool GraphicsCommandPool { get; }
        public CommandPool ComputeCommandPool { get; }

        public void Dispose()
        {
            ComputeCommandPool.Dispose();
            GraphicsCommandPool.Dispose();
            Device.Dispose();
        }
    }
}
