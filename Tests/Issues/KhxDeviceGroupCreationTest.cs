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

    public class KhxDeviceGroupCreationTest
    {
        [Fact]
        public void EnumeratePhysicalDeviceGroupsKhx()
        {
            // Only continue with the test if "VK_KHX_device_group_creation" extension is present.
            ExtensionProperties[] availableExtensions = Instance.EnumerateExtensionProperties();
            if (!availableExtensions.Contains(Constant.InstanceExtension.KhxDeviceGroupCreation)) return;

            var instance = new Instance(new InstanceCreateInfo(
                enabledExtensionNames: new[] { Constant.InstanceExtension.KhxDeviceGroupCreation }));

            PhysicalDeviceGroupPropertiesKhx[] groups = instance.EnumeratePhysicalDeviceGroupsKhx();

            Assert.True(0 < groups.Length && groups.Length <= Constant.MaxDeviceGroupSizeKhx);
            Assert.True(groups[0].PhysicalDevices.Length > 0);
            Assert.True(groups[0].PhysicalDevices.All(physicalDevice => physicalDevice.Handle != IntPtr.Zero));
        }

        [Fact]
        public void CreateDeviceWithDeviceGroupInfo()
        {
            ExtensionProperties[] availableExtensions = Instance.EnumerateExtensionProperties();
            if (!availableExtensions.Contains(Constant.InstanceExtension.KhxDeviceGroupCreation)) return;

            var instance = new Instance(new InstanceCreateInfo(
                enabledExtensionNames: new[] { Constant.InstanceExtension.KhxDeviceGroupCreation }));

            PhysicalDeviceGroupPropertiesKhx physicalDeviceGroup = instance.EnumeratePhysicalDeviceGroupsKhx()[0];
            // We need a pointer to the array of native handles.
            IntPtr devicePtrs = Interop.Struct.AllocToPointer(physicalDeviceGroup.PhysicalDevices.ToHandleArray());

            // Fill in the device group create info struct.
            var deviceGroupCreateInfo = new DeviceGroupDeviceCreateInfoKhx(physicalDeviceGroup.PhysicalDevices.Length, devicePtrs);
            // We also need a pointer to the create info struct itself.
            IntPtr createInfoPtr = Interop.Struct.AllocToPointer(ref deviceGroupCreateInfo);

            // Finally, pass the device group create info pointer to the `Next` chain of device create info.
            Device device = physicalDeviceGroup.PhysicalDevices[0].CreateDevice(new DeviceCreateInfo(
                new[] { new DeviceQueueCreateInfo(0, 1, 1.0f) },
                next: createInfoPtr));

            // Make sure to free unmanaged allocations.
            Interop.Free(createInfoPtr);
            Interop.Free(devicePtrs);
        }
    }
}
