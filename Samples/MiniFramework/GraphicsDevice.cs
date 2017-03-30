using System;
using VulkanCore.Khr;

namespace VulkanCore.Samples
{
    /// <summary>
    /// Encapsulates Vulkan <see cref="PhysicalDevice"/> and <see cref="Device"/> and exposes queues
    /// and a command pool for rendering tasks.
    /// </summary>
    public class GraphicsDevice : IDisposable
    {
        public GraphicsDevice(Instance instance, SurfaceKhr surface, Platform platform)
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
                            Physical = physicalDevice;
                            break;
                        }
                    }
                }
                if (Physical != null) break;
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

            if (Physical == null)
                throw new InvalidOperationException("No suitable physical device found.");

            // Store memory properties of the physical device.
            MemoryProperties = Physical.GetMemoryProperties();

            // Create a logical device.
            bool sameGraphicsAndPresent = graphicsQueueFamilyIndex == presentQueueFamilyIndex;
            var queueCreateInfos = new DeviceQueueCreateInfo[sameGraphicsAndPresent ? 1 : 2];
            queueCreateInfos[0] = new DeviceQueueCreateInfo(graphicsQueueFamilyIndex, 1, 1.0f);
            if (!sameGraphicsAndPresent)
                queueCreateInfos[1] = new DeviceQueueCreateInfo(presentQueueFamilyIndex, 1, 1.0f);

            var deviceCreateInfo = new DeviceCreateInfo(
                queueCreateInfos,
                new[] { Constant.DeviceExtension.KhrSwapchain });
            Logical = Physical.CreateDevice(deviceCreateInfo);

            // Get queue(s).
            GraphicsQueue = Logical.GetQueue(graphicsQueueFamilyIndex);
            ComputeQueue = computeQueueFamilyIndex == graphicsQueueFamilyIndex
                ? GraphicsQueue
                : Logical.GetQueue(computeQueueFamilyIndex);
            PresentQueue = presentQueueFamilyIndex == graphicsQueueFamilyIndex
                ? GraphicsQueue
                : Logical.GetQueue(presentQueueFamilyIndex);

            // Create a command pool.
            CommandPool = Logical.CreateCommandPool(new CommandPoolCreateInfo(graphicsQueueFamilyIndex));
        }

        public PhysicalDevice Physical { get; private set; }
        public Device Logical { get; private set; }
        public PhysicalDeviceMemoryProperties MemoryProperties { get; private set; }
        public Queue GraphicsQueue { get; private set; }
        public Queue ComputeQueue { get; }
        public Queue PresentQueue { get; private set; }
        public CommandPool CommandPool { get; private set; }

        public void Dispose()
        {
            CommandPool.Dispose();
            Logical.Dispose();
        }
    }
}
