﻿using System;
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

	        if (vkEnumeratePhysicalDeviceGroupsKHX == null)
			{
		        vkEnumeratePhysicalDeviceGroupsKHX = instance
			        .GetProc<vkEnumeratePhysicalDeviceGroupsKHXDelegate>
						(nameof(vkEnumeratePhysicalDeviceGroupsKHX));
	        }
			
	        Result result = vkEnumeratePhysicalDeviceGroupsKHX(instance, &count, default(IntPtr));
            VulkanException.ThrowForInvalidResult(result);

	        var structSize = Interop.SizeOf<PhysicalDeviceGroupPropertiesKhx.Native>();
	        var nativePropertiesPtr = stackalloc byte[structSize * count];
            result = vkEnumeratePhysicalDeviceGroupsKHX(instance, &count, (IntPtr)nativePropertiesPtr);
            VulkanException.ThrowForInvalidResult(result);

            var groupProperties = new PhysicalDeviceGroupPropertiesKhx[count];
            for (int i = 0; i < count; i++)
            {
	            var nativeProperty = Marshal.PtrToStructure<PhysicalDeviceGroupPropertiesKhx.Native>
					((IntPtr)nativePropertiesPtr + structSize*i);
                var devices = new PhysicalDevice[nativeProperty.PhysicalDeviceCount];
                for (int j = 0; j < nativeProperty.PhysicalDeviceCount; j++)
                    devices[j] = new PhysicalDevice(nativeProperty.PhysicalDevices[j], instance);
                PhysicalDeviceGroupPropertiesKhx.FromNative(ref nativeProperty, devices, out groupProperties[i]);
            }
			

            return groupProperties;
        }

        private delegate Result vkEnumeratePhysicalDeviceGroupsKHXDelegate(IntPtr instance, int* physicalDeviceGroupCount, IntPtr physicalDeviceGroupProperties);
        private static vkEnumeratePhysicalDeviceGroupsKHXDelegate vkEnumeratePhysicalDeviceGroupsKHX = VulkanLibrary.GetProc<vkEnumeratePhysicalDeviceGroupsKHXDelegate>(nameof(vkEnumeratePhysicalDeviceGroupsKHX));
    }

    /// <summary>
    /// Structure specifying physical device group properties.
    /// </summary>
    public unsafe struct PhysicalDeviceGroupPropertiesKhx
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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constant.MaxDeviceGroupSizeKhx)]
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
