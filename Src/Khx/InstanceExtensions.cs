using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore.Khx
{
    // TODO: doc
    public static unsafe class InstanceExtensions
    {
        /// <summary>
        /// Enumerates groups of physical devices that can be used to create a single logical device.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="handleType"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static PhysicalDeviceGroupPropertiesKhx[] EnumeratePhysicalDeviceGroupsKhx(this Instance instance,
            ExternalSemaphoreHandleTypesKhx handleType)
        {
            int count;
            Result result = vkEnumeratePhysicalDeviceGroupsKHX(instance, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var nativeProperties = new PhysicalDeviceGroupPropertiesKhx.Native[count];
            result = vkEnumeratePhysicalDeviceGroupsKHX(instance, &count, nativeProperties);
            VulkanException.ThrowForInvalidResult(result);

            var groupProperties = new PhysicalDeviceGroupPropertiesKhx[count];
            for (int i = 0; i < count; i++)
            {
                ref PhysicalDeviceGroupPropertiesKhx.Native nativeProps = ref nativeProperties[i];
                var devices = new PhysicalDevice[nativeProps.PhysicalDeviceCount];
                for (int j = 0; j < nativeProps.PhysicalDeviceCount; j++)
                    devices[j] = new PhysicalDevice(nativeProps.PhysicalDevices[j], instance);
                PhysicalDeviceGroupPropertiesKhx.FromNative(ref nativeProps, devices, out groupProperties[i]);
            }

            return groupProperties;
        }
        
        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkEnumeratePhysicalDeviceGroupsKHX(IntPtr instance,
            int* physicalDeviceGroupCount, PhysicalDeviceGroupPropertiesKhx.Native[] physicalDeviceGroupProperties);
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
        /// An array of physical device handles representing all physical devices in the
        /// group.
        /// </summary>
        public PhysicalDevice[] PhysicalDevices;
        /// <summary>
        /// Indicates whether logical devices created from the group support allocating
        /// device memory on a subset of devices, via the <c>deviceMask</c> member of the 
        /// <c>VkMemoryAllocateFlagsInfoKHX</c>. If this is <c>false</c>, then all device memory
        /// allocations are made across all physical devices in the group. If
        /// physicalDeviceCount is 1, then <see cref="SubsetAllocation"/> must be <c>false</c>.
        /// </summary>
        public Bool SubsetAllocation;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public int PhysicalDeviceCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public IntPtr[] PhysicalDevices;
            public Bool SubsetAllocation;
        }

        internal static void FromNative(ref Native native, PhysicalDevice[] physicalDevices, out PhysicalDeviceGroupPropertiesKhx managed)
        {
            managed.Next = native.Next;
            managed.PhysicalDevices = physicalDevices;
            managed.SubsetAllocation = native.SubsetAllocation;
        }
    }
}
