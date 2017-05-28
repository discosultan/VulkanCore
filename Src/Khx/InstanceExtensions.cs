using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Khx
{
    /// <summary>
    /// Provides experimental Khronos specific extension methods for the <see cref="Instance"/> class.
    /// </summary>
    public static unsafe class InstanceExtensions
    {
        /// <summary>
        /// Enumerates groups of physical devices that can be used to create a single logical device.
        /// </summary>
        /// <param name="instance">A handle to a previously created Vulkan instance.</param>
        /// <returns>An array of <see cref="PhysicalDeviceGroupPropertiesKhx"/> structures.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static PhysicalDeviceGroupPropertiesKhx[] EnumeratePhysicalDeviceGroupsKhx(this Instance instance)
        {
            int count;
            Result result = vkEnumeratePhysicalDeviceGroupsKHX(instance)(instance, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var nativeProperties = new PhysicalDeviceGroupPropertiesKhx.Native[count];
            result = vkEnumeratePhysicalDeviceGroupsKHX(instance)(instance, &count, nativeProperties);
            VulkanException.ThrowForInvalidResult(result);

            var groupProperties = new PhysicalDeviceGroupPropertiesKhx[count];
            for (int i = 0; i < count; i++)
                PhysicalDeviceGroupPropertiesKhx.FromNative(ref nativeProperties[i], instance, out groupProperties[i]);
            return groupProperties;
        }

        private delegate Result vkEnumeratePhysicalDeviceGroupsKHXDelegate(IntPtr instance, int* physicalDeviceGroupCount, [Out]PhysicalDeviceGroupPropertiesKhx.Native[] physicalDeviceGroupProperties);
        private static vkEnumeratePhysicalDeviceGroupsKHXDelegate vkEnumeratePhysicalDeviceGroupsKHX(Instance instance) => instance.GetProc<vkEnumeratePhysicalDeviceGroupsKHXDelegate>(nameof(vkEnumeratePhysicalDeviceGroupsKHX));
    }

    /// <summary>
    /// Structure specifying physical device group properties.
    /// </summary>
    public struct PhysicalDeviceGroupPropertiesKhx
    {
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// An array of physical device handles representing all physical devices in the group.
        /// </summary>
        public PhysicalDevice[] PhysicalDevices;
        /// <summary>
        /// Indicates whether logical devices created from the group support allocating device memory
        /// on a subset of devices, via the <see cref="MemoryAllocateFlagsInfoKhx.DeviceMask"/>
        /// member. If this is <c>false</c>, then all device memory allocations are made across all
        /// physical devices in the group. If <see cref="PhysicalDevices"/> length is 1, then <see
        /// cref="SubsetAllocation"/> must be <c>false</c>.
        /// </summary>
        public Bool SubsetAllocation;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public int PhysicalDeviceCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constant.MaxDeviceGroupSizeKhx)]
            public IntPtr[] PhysicalDevices;
            public Bool SubsetAllocation;
        }

        internal static void FromNative(ref Native native, Instance instance, out PhysicalDeviceGroupPropertiesKhx managed)
        {
            managed.Next = native.Next;
            managed.PhysicalDevices = new PhysicalDevice[native.PhysicalDeviceCount];
            for (int i = 0; i < native.PhysicalDeviceCount; i++)
                managed.PhysicalDevices[i] = new PhysicalDevice(native.PhysicalDevices[i], instance);
            managed.SubsetAllocation = native.SubsetAllocation;
        }
    }
}
