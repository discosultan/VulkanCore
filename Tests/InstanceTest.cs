using System;
using System.Collections.Generic;
using System.Linq;
using VulkanCore.Ext;
using VulkanCore.Khx;
using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;
using static VulkanCore.Constant;

namespace VulkanCore.Tests
{
    public unsafe class InstanceTest : HandleTestBase
    {
        [Fact]
        public void Constructor()
        {
            using (new Instance()) { }
            using (var instance = new Instance(allocator: CustomAllocator))
            {
                Assert.Equal(CustomAllocator, instance.Allocator);
            }
        }

        [Fact]
        public void ConstructorWithApplicationInfo()
        {
            var createInfo1 = new InstanceCreateInfo(new ApplicationInfo());
            var createInfo2 = new InstanceCreateInfo(new ApplicationInfo("app name", 1, "engine name", 2));
            using (new Instance(createInfo1)) { }
            using (new Instance(createInfo2)) { }
        }

        [Fact]
        public void ConstructorWithEnabledLayerAndExtension()
        {
            var createInfo = new InstanceCreateInfo(
                enabledLayerNames: new[] { InstanceLayer.LunarGStandardValidation },
                enabledExtensionNames: new[] { InstanceExtension.ExtDebugReport });

            using (new Instance(createInfo)) { }
        }

        [Fact]
        public void DisposeTwice()
        {
            var instance = new Instance();
            instance.Dispose();
            instance.Dispose();
        }

        [Fact]
        public void CreateDebugReportCallbackExt()
        {
            var createInfo = new InstanceCreateInfo(
                enabledLayerNames: new[] { InstanceLayer.LunarGStandardValidation },
                enabledExtensionNames: new[] { InstanceExtension.ExtDebugReport });

            using (var instance = new Instance(createInfo))
            {
                var callbackArgs = new List<DebugReportCallbackInfo>();
                int userData = 1;
                IntPtr userDataHandle = new IntPtr(&userData);
                var debugReportCallbackCreateInfo = new DebugReportCallbackCreateInfoExt(
                    DebugReportFlagsExt.All,
                    args =>
                    {
                        callbackArgs.Add(args);
                        return false;
                    },
                    userDataHandle);

                using (instance.CreateDebugReportCallbackExt(debugReportCallbackCreateInfo))
                {
                    instance.DebugReportMessageExt(DebugReportFlagsExt.Debug, "test");
                    Assert.NotEmpty(callbackArgs);
                    Assert.Equal(1, *(int*)callbackArgs[0].UserData);
                }
                callbackArgs.Clear();
                using (instance.CreateDebugReportCallbackExt(debugReportCallbackCreateInfo, CustomAllocator))
                {
                    instance.DebugReportMessageExt(DebugReportFlagsExt.Debug, "test");
                    Assert.NotEmpty(callbackArgs);
                    Assert.Equal(1, *(int*)callbackArgs[0].UserData);
                }
            }
        }

        [Fact]
        public void EnumeratePhysicalDevices()
        {
            PhysicalDevice[] physicalDevices = Instance.EnumeratePhysicalDevices();
            Assert.True(physicalDevices.Length > 0);
            Assert.Equal(Instance, physicalDevices[0].Parent);
        }

        [Fact]
        public void EnumeratePhysicalDeviceGroups()
        {
            using (var instance = new Instance())
            {
                PhysicalDeviceGroupProperties[] groups = instance.EnumeratePhysicalDeviceGroups();

                Assert.True(0 < groups.Length && groups.Length <= MaxDeviceGroupSize);
                Assert.True(groups[0].PhysicalDevices.Length > 0);
                Assert.True(groups[0].PhysicalDevices.All(physicalDevice => physicalDevice.Handle != IntPtr.Zero));
            }
        }

        [Fact]
        public void CreateDeviceWithDeviceGroupInfo()
        {
            using (var instance = new Instance())
            {
                PhysicalDeviceGroupProperties physicalDeviceGroup = instance.EnumeratePhysicalDeviceGroups()[0];
                // We need a pointer to the array of native handles.
                IntPtr devicePtrs = Interop.Struct.AllocToPointer(physicalDeviceGroup.PhysicalDevices.ToHandleArray());

                // Fill in the device group create info struct.
                var deviceGroupCreateInfo = new DeviceGroupDeviceCreateInfoKhx(physicalDeviceGroup.PhysicalDevices.Length, devicePtrs);
                // We also need a pointer to the create info struct itself.
                IntPtr createInfoPtr = Interop.Struct.AllocToPointer(ref deviceGroupCreateInfo);

                // Finally, pass the device group create info pointer to the `Next` chain of device create info.
                physicalDeviceGroup.PhysicalDevices[0].CreateDevice(new DeviceCreateInfo(
                    new[] { new DeviceQueueCreateInfo(0, 1, 1.0f) },
                    next: createInfoPtr));

                // Make sure to free unmanaged allocations.
                Interop.Free(createInfoPtr);
                Interop.Free(devicePtrs);
            }
        }

        [Fact]
        public void GetProcAddrForExistingCommand()
        {
            IntPtr address = Instance.GetProcAddr("vkCreateDebugReportCallbackEXT");
            Assert.NotEqual(IntPtr.Zero, address);
        }

        [Fact]
        public void GetProcAddrForMissingCommand()
        {
            IntPtr address = Instance.GetProcAddr("does not exist");
            Assert.Equal(IntPtr.Zero, address);
        }

        [Fact]
        public void GetProcAddrForNull()
        {
            Assert.Throws<ArgumentNullException>(() => Instance.GetProcAddr(null));
        }

        [Fact]
        public void GetProcForExistingCommand()
        {
            var commandDelegate = Instance.GetProc<CreateDebugReportCallbackExtDelegate>("vkCreateDebugReportCallbackEXT");
            Assert.NotNull(commandDelegate);
        }

        [Fact]
        public void GetProcForMissingCommand()
        {
            Assert.Null(Instance.GetProc<EventHandler>("does not exist"));
        }

        [Fact]
        public void GetProcForNull()
        {
            Assert.Throws<ArgumentNullException>(() => Instance.GetProc<EventHandler>(null));
        }

        [Fact]
        public void EnumerateExtensionPropertiesForAllLayers()
        {
            ExtensionProperties[] properties = Instance.EnumerateExtensionProperties();
            Assert.NotEmpty(properties);
        }

        [Fact(Skip = "Enumerating extensions for standard validation meta layer no longer returns data")]
        public void EnumerateExtensionPropertiesForSingleLayer()
        {
            ExtensionProperties[] properties = Instance.EnumerateExtensionProperties(
                InstanceLayer.LunarGStandardValidation);
            Assert.NotEmpty(properties);

            ExtensionProperties firstProperty = properties[0];
            Assert.StartsWith(firstProperty.ExtensionName, properties[0].ToString());
        }

        [Fact]
        private void EnumerateLayerProperties()
        {
            LayerProperties[] properties = Instance.EnumerateLayerProperties();
            Assert.NotEmpty(properties);

            LayerProperties firstProperty = properties[0];
            Assert.StartsWith(firstProperty.LayerName, properties[0].ToString());
        }

        [Fact]
        public void DebugReportMessageExt()
        {
            const string message = "message õäöü";
            const DebugReportObjectTypeExt objectType = DebugReportObjectTypeExt.DebugReportCallback;
            const long @object = long.MaxValue;
            var location = new IntPtr(int.MaxValue);
            const int messageCode = 1;
            const string layerPrefix = "prefix õäöü";

            bool visitedCallback = false;

            var instanceCreateInfo = new InstanceCreateInfo(
                enabledExtensionNames: new[] { InstanceExtension.ExtDebugReport });
            using (var instance = new Instance(instanceCreateInfo))
            {
                var debugReportCallbackCreateInfo = new DebugReportCallbackCreateInfoExt(
                    DebugReportFlagsExt.Error,
                    args =>
                    {
                        Assert.Equal(objectType, args.ObjectType);
                        Assert.Equal(@object, args.Object);
                        Assert.Equal(location, args.Location);
                        Assert.Equal(messageCode, args.MessageCode);
                        Assert.Equal(layerPrefix, args.LayerPrefix);
                        Assert.Equal(message, args.Message);
                        visitedCallback = true;
                        return false;
                    });
                using (instance.CreateDebugReportCallbackExt(debugReportCallbackCreateInfo))
                {
                    instance.DebugReportMessageExt(DebugReportFlagsExt.Error, message, objectType,
                        @object, location, messageCode, layerPrefix);
                }
            }

            Assert.True(visitedCallback);
        }

        [Fact]
        public void EnumerateInstanceVersion()
        {
            Instance.EnumerateInstanceVersion();
        }

        public InstanceTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }

        private delegate Result CreateDebugReportCallbackExtDelegate(IntPtr p1, IntPtr p2, IntPtr p3, IntPtr p4);
    }
}
