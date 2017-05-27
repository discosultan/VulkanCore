using System;
using System.Collections.Generic;
using System.Linq;
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
            
	        var nativePropertiesPtr = Interop.Alloc<PhysicalDeviceGroupPropertiesKhx.Native>(count);
            result = vkEnumeratePhysicalDeviceGroupsKHX(instance, &count, nativePropertiesPtr);
            VulkanException.ThrowForInvalidResult(result);
            var nativeProperties = (PhysicalDeviceGroupPropertiesKhx.Native*) nativePropertiesPtr;

            var groupProperties = new PhysicalDeviceGroupPropertiesKhx[count];
            for (int i = 0; i < count; i++)
            {
                groupProperties[i] = new PhysicalDeviceGroupPropertiesKhx(instance,i+nativeProperties);
            }

            return groupProperties;
        }

        private delegate Result vkEnumeratePhysicalDeviceGroupsKHXDelegate(IntPtr instance, int* physicalDeviceGroupCount, IntPtr physicalDeviceGroupProperties);
        private static vkEnumeratePhysicalDeviceGroupsKHXDelegate vkEnumeratePhysicalDeviceGroupsKHX = VulkanLibrary.GetProc<vkEnumeratePhysicalDeviceGroupsKHXDelegate>(nameof(vkEnumeratePhysicalDeviceGroupsKHX));
    }

    /// <summary>
    /// Structure specifying physical device group properties.
    /// </summary>
    public unsafe class PhysicalDeviceGroupPropertiesKhx : IDisposable
    {
        internal PhysicalDeviceGroupPropertiesKhx(Instance instance, Native* pNative)
        {
            Parent = instance;
            _pNative = pNative;

            PhysicalDevices = new VirtualList<PhysicalDevice>(
                () => PhysicalDeviceCount,
                index => new PhysicalDevice(*(index+&PNative->PhysicalDevice0),Parent),
                (index, item) =>
                {
                    if (index > PhysicalDeviceCount || index >= Constant.MaxDeviceGroupSizeKhx)
                        return false;
                    *(index + &PNative->PhysicalDevice0) = item.Handle;
                    if (index == PhysicalDeviceCount)
                        ++PhysicalDeviceCount;
                    return true;
                },
                index =>
                {
                    if (index >= PhysicalDeviceCount)
                        return false;
                    *(index + &PNative->PhysicalDevice0) = default(IntPtr);
                    --PhysicalDeviceCount;
                    return true;
                }
            );

        }

        internal PhysicalDeviceGroupPropertiesKhx(Instance instance)
            : this(instance,(Native*)Interop.Alloc<Native>())
        {

        }

        public Instance Parent;

        /// <summary>
        /// The unmanaged structure that we're wrapping.
        /// </summary>
        private Native* _pNative;

        /// <summary>
        /// Safe accessor for the pointer to the unmanaged structure.
        /// </summary>
        internal Native* PNative
            => _pNative == null
                ? throw new ObjectDisposedException(nameof(PhysicalDeviceGroupPropertiesKhx))
                : _pNative;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next
        {
            get => PNative->Next;
            set => PNative->Next = value;
        }

        private int PhysicalDeviceCount
        {
            get => PNative->PhysicalDeviceCount;
            set => PNative->PhysicalDeviceCount = value;
        }

        /// <summary>
        /// An array of physical device handles representing all physical devices in the group.
        /// </summary>
        public IList<PhysicalDevice> PhysicalDevices;

        /// <summary>
        /// Indicates whether logical devices created from the group support allocating
        /// device memory on a subset of devices, via the <c>deviceMask</c> member of the
        /// <c>VkMemoryAllocateFlagsInfoKHX</c>. If this is <c>false</c>, then all device memory
        /// allocations are made across all physical devices in the group. If
        /// physicalDeviceCount is 1, then <see cref="SubsetAllocation"/> must be <c>false</c>.
        /// </summary>
        public Bool SubsetAllocation
        {
            get => PNative->SubsetAllocation;
            set => PNative->SubsetAllocation = value;
        }

        public void Dispose()
        {
            Interop.Free(_pNative);
            _pNative = null;
            Parent = null;
        }
		
	    [StructLayout(LayoutKind.Sequential)]
	    internal struct Native
        {
            public StructureType Type; // 4
            public IntPtr Next; // 8
            public int PhysicalDeviceCount; // 12

            /// <see cref="Constant.MaxDeviceGroupSizeKhx"/>
            public IntPtr PhysicalDevice0; // 16
            public IntPtr PhysicalDevice1;
            public IntPtr PhysicalDevice2;
            public IntPtr PhysicalDevice3;
            public IntPtr PhysicalDevice4; // 32
            public IntPtr PhysicalDevice5;
            public IntPtr PhysicalDevice6;
            public IntPtr PhysicalDevice7;
            public IntPtr PhysicalDevice8; // 48
            public IntPtr PhysicalDevice9;
            public IntPtr PhysicalDevice10;
            public IntPtr PhysicalDevice11;
            public IntPtr PhysicalDevice12; // 64
            public IntPtr PhysicalDevice13;
            public IntPtr PhysicalDevice14;
            public IntPtr PhysicalDevice15;
            public IntPtr PhysicalDevice16; // 80
            public IntPtr PhysicalDevice17;
            public IntPtr PhysicalDevice18;
            public IntPtr PhysicalDevice19;
            public IntPtr PhysicalDevice20; // 96
            public IntPtr PhysicalDevice21;
            public IntPtr PhysicalDevice22;
            public IntPtr PhysicalDevice23;
            public IntPtr PhysicalDevice24; // 112
            public IntPtr PhysicalDevice25;
            public IntPtr PhysicalDevice26;
            public IntPtr PhysicalDevice27;
            public IntPtr PhysicalDevice28; // 128
            public IntPtr PhysicalDevice29;
            public IntPtr PhysicalDevice30;
            public IntPtr PhysicalDevice31;

            public Bool SubsetAllocation; // 144
        }
        
    }
}
