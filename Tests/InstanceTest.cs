using System;
using System.Collections.Generic;
using VulkanCore.Ext;
using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public unsafe class InstanceTest : HandleTestBase
    {
        [Fact]
        public void Constructor_Succeeds()
        {
            using (new Instance()) { }
            using (var instance = new Instance(default(InstanceCreateInfo), CustomAllocator))
            {
                Assert.Equal(CustomAllocator, instance.Allocator);
            }
        }

        [Fact]
        public void Constructor_ApplicationInfo_Succeeds()
        {
            var createInfo = new InstanceCreateInfo
            {
                ApplicationInfo = new ApplicationInfo
                {
                    ApplicationName = "app name",
                    EngineName = "engine name"
                }
            };
            using (new Instance(createInfo)) { }
        }

        [Fact]
        public void Constructor_EnabledLayerAndExtension_Succeeds()
        {
            var createInfo = new InstanceCreateInfo
            {
                EnabledLayerNames = new[] { Layers.LunarGStandardValidation },
                EnabledExtensionNames = new[] { Extensions.ExtDebugReport }
            };
            using (new Instance(createInfo)) { }
        }

        [Fact]
        public void DisposeTwice_Succeeds()
        {
            var instance = new Instance();
            instance.Dispose();
            instance.Dispose();
        }

        [Fact]
        public void CreateDebugReportCallbackExt_Succeeds()
        {
            var createInfo = new InstanceCreateInfo
            {
                EnabledLayerNames = new[] { Layers.LunarGStandardValidation },
                EnabledExtensionNames = new[] { Extensions.ExtDebugReport }
            };            
            using (Instance instance = new Instance(createInfo))
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

                // Registering the callback should generate DEBUG messages.
                using (instance.CreateDebugReportCallbackExt(debugReportCallbackCreateInfo)) { }
                using (instance.CreateDebugReportCallbackExt(debugReportCallbackCreateInfo, CustomAllocator)) { }

                Assert.True(callbackArgs.Count > 0);
                Assert.Equal(1, *(int*)callbackArgs[0].UserData);
            }
        }

        [Fact]
        public void EnumeratePhysicalDevices_ReturnsAtLeastOneDevice()
        {
            PhysicalDevice[] physicalDevices = Instance.EnumeratePhysicalDevices();
            Assert.True(physicalDevices.Length > 0);
            Assert.Equal(Instance, physicalDevices[0].Parent);
        }

        [Fact]
        public void EnumeratePhysicalDevicesTwice_PhysicalDevicesEqual()
        {
            PhysicalDevice[] physicalDevices1 = Instance.EnumeratePhysicalDevices();
            PhysicalDevice[] physicalDevices2 = Instance.EnumeratePhysicalDevices();
            Assert.Equal(physicalDevices1[0], physicalDevices2[0]);
        }

        [Fact]
        public void GetProcAddr_ReturnsValidHandleForExistingCommand()
        {
            IntPtr address = Instance.GetProcAddr("vkCreateDebugReportCallbackEXT");
            Assert.NotEqual(IntPtr.Zero, address);
        }

        [Fact]
        public void GetProcAddr_ReturnsNullHandleForMissingCommand()
        {
            IntPtr address = Instance.GetProcAddr("does not exist");
            Assert.Equal(IntPtr.Zero, address);
        }

        [Fact]
        public void GetProcAddr_ThrowsArgumentNullForNull()
        {
            Assert.Throws<ArgumentNullException>(() => Instance.GetProcAddr(null));
        }

        [Fact]
        public void GetProc_ThrowsArgumentNullForNull()
        {
            Assert.Throws<ArgumentNullException>(() => Instance.GetProc<EventHandler>(null));
        }

        [Fact]
        public void GetProc_ReturnsNullForMissingCommand()
        {
            Assert.Null(Instance.GetProc<EventHandler>("does not exist"));
        }

        private delegate Result vkCreateDebugReportCallbackEXT(IntPtr p1, IntPtr p2, IntPtr p3, IntPtr p4);
        [Fact]
        public void GetProc_ReturnsValidDelegate()
        {
            var commandDelegate = Instance.GetProc<vkCreateDebugReportCallbackEXT>("vkCreateDebugReportCallbackEXT");
            Assert.NotNull(commandDelegate);
        }

        [Fact]
        public void EnumerateExtensionProperties_SucceedsWithoutLayerName()
        {
            ExtensionProperties[] properties = Instance.EnumerateExtensionProperties();
            Assert.True(properties.Length > 0);
        }

        [Fact]
        public void EnumerateExtensionProperties_SucceedsForLayerName()
        {
            ExtensionProperties[] properties = Instance.EnumerateExtensionProperties(Layers.LunarGStandardValidation);
            Assert.True(properties.Length > 0);
        }

        [Fact]
        private void EnumerateLayerProperties_Succeeds()
        {
            LayerProperties[] properties = Instance.EnumerateLayerProperties();
            Assert.True(properties.Length > 0);
        }

        [Fact]
        public void DebugReportMessageExt_Succeeds()
        {
            const string message = "hello õäöü";

            using (var instance = new Instance(new InstanceCreateInfo(
                enabledExtensionNames: new[] { Extensions.ExtDebugReport })))
            {
                var createInfo = new DebugReportCallbackCreateInfoExt(
                    DebugReportFlagsExt.Error,
                    args =>
                    {
                        Assert.Equal(message, args.Message);
                        return false;
                    });
                using (instance.CreateDebugReportCallbackExt(createInfo))
                {
                    instance.DebugReportMessageExt(
                        DebugReportFlagsExt.Error,
                        DebugReportObjectTypeExt.Unknown,
                        0, 0, 0, null, message);
                }
            }                
        }

        public InstanceTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }
    }
}
