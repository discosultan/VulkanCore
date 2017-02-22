using System;
using System.Linq;
using VulkanCore.Ext;
using Xunit;

namespace VulkanCore.Tests.Utilities
{
    public class DefaultHandles : IDisposable
    {
        private DebugReportCallbackExt _debugReportCallback;

        public DefaultHandles()
        {
            CreateInstance();
            PickPhysicalDevice();
            CreateDevice();
        }

        public Instance Instance { get; private set; }
        public PhysicalDevice PhysicalDevice { get; private set; }
        public PhysicalDeviceMemoryProperties PhysicalDeviceMemoryProperties { get; private set; }
        public Device Device { get; private set; }

        public Queue ComputeQueue { get; private set; }
        public Queue GraphicsQueue { get; private set; }
        public Queue SparseBindingQueue { get; private set; }
        public Queue TransferQueue { get; private set; }

        public virtual void Dispose()
        {
            Device.WaitIdle();
            Device.Dispose();
            _debugReportCallback.Dispose();
            Instance.Dispose();
        }

        private void CreateInstance()
        {
            // Specify standard validation layers.
            var createInfo = new InstanceCreateInfo
            {
                EnabledLayerNames = new[] { Constant.InstanceLayer.LunarGStandardValidation },
                EnabledExtensionNames = new[] { Constant.InstanceExtension.ExtDebugReport }
            };
            Instance = new Instance(createInfo);

            // Attach debug callback and fail any test on any error or warning from validation layers.
            var debugReportCreateInfo = new DebugReportCallbackCreateInfoExt(
                DebugReportFlagsExt.Error |
                DebugReportFlagsExt.Warning |
                DebugReportFlagsExt.PerformanceWarning,
                args =>
                {
                    if ((args.Flags & (DebugReportFlagsExt.Error |
                                       DebugReportFlagsExt.PerformanceWarning |
                                       DebugReportFlagsExt.Warning)) > 0)
                    {
                        Assert.True(false, $"Validation layer error/warning:\n\n[{args.Flags}] {args.Message}");
                    }
                    return false;
                }
            );
            _debugReportCallback = Instance.CreateDebugReportCallbackExt(debugReportCreateInfo);
        }

        private void PickPhysicalDevice()
        {
            // Currently simply picks the first physical device. We might want a
            // smarter choice here.
            PhysicalDevice = Instance.EnumeratePhysicalDevices()[0];
        }

        private void CreateDevice()
        {
            // TODO: Separation between graphics and present queue (for testing presentation).

            QueueFamilyProperties[] queueProps = PhysicalDevice.GetQueueFamilyProperties();
            int computeFamilyIndex = -1;
            int graphicsFamilyIndex = -1;
            int sparseBindingFamilyIndex = -1;
            int transferFamilyIndex = -1;
            for (var i = 0; i < queueProps.Length; i++)
            {
                QueueFamilyProperties props = queueProps[i];

                if (computeFamilyIndex == -1 && props.QueueFlags.HasFlag(Queues.Compute))
                    computeFamilyIndex = i;

                if (graphicsFamilyIndex == -1 && props.QueueFlags.HasFlag(Queues.Graphics))
                    graphicsFamilyIndex = i;

                if (sparseBindingFamilyIndex == -1 && props.QueueFlags.HasFlag(Queues.SparseBinding))
                    sparseBindingFamilyIndex = i;

                if (transferFamilyIndex == -1 && props.QueueFlags.HasFlag(Queues.Transfer))
                    transferFamilyIndex = i;
            }

            var queueInfos = new[]
            {
                computeFamilyIndex,
                graphicsFamilyIndex,
                sparseBindingFamilyIndex,
                transferFamilyIndex
            }.Distinct().Select(i => new DeviceQueueCreateInfo(i, 1, 1.0f)).ToArray();

            var createInfo = new DeviceCreateInfo(queueInfos
            //, new[] { Extensions.ExtDebugMarker } // TODO: why is the extension not present?
            );
            Device = PhysicalDevice.CreateDevice(createInfo);

            ComputeQueue = Device.GetQueue(computeFamilyIndex);
            GraphicsQueue = Device.GetQueue(graphicsFamilyIndex);
            SparseBindingQueue = Device.GetQueue(sparseBindingFamilyIndex);
            TransferQueue = Device.GetQueue(transferFamilyIndex);
        }
    }
}
