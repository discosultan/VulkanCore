using System;
using System.Linq;
using Xunit;
using VulkanCore.Khx;

namespace VulkanCore.Tests.Issues
{
    // Many Vulkan extension commands were implemented as if they were statically exported.
    // That is only the case for certain WSI commands.
    // https://github.com/discosultan/VulkanCore/issues/8

    // The failure was discovered through vkEnumeratePhysicalDeviceGroupsKHX.
    // https://github.com/discosultan/VulkanCore/pull/9

    // There seems to be a bug in the validation layers as of 1.0.49. If validation layers are
    // enabled, vkEnumeratePhysicalDeviceGroupsKHX will raise an error when null is passed to
    // pPhysicalDeviceGroupProperties.

    public class OverambitiousStaticCommandImportTest
    {
        [Fact]
        public void EnumeratePhysicalDeviceGroupsKhx()
        {
            ExtensionProperties[] availableExtensions = Instance.EnumerateExtensionProperties();

            if (!availableExtensions.Contains(Constant.InstanceExtension.KhxDeviceGroupCreation)) return;
            
            var instance = new Instance(new InstanceCreateInfo(
                enabledExtensionNames: new[] { Constant.InstanceExtension.KhxDeviceGroupCreation }));
            PhysicalDeviceGroupPropertiesKhx[] groups = instance.EnumeratePhysicalDeviceGroupsKhx();

            Assert.True(0 < groups.Length && groups.Length <= Constant.MaxDeviceGroupSizeKhx);
            Assert.True(groups[0].PhysicalDevices.Length > 0);
            Assert.True(groups[0].PhysicalDevices.All(physicalDevice => physicalDevice.Handle != IntPtr.Zero));
        }
    }
}
