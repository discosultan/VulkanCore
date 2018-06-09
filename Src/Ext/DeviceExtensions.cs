using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;
using static VulkanCore.Ext.DeviceExtensions;

namespace VulkanCore.Ext
{
    /// <summary>
    /// Provides extension methods for the <see cref="Device"/> class.
    /// </summary>
    public static unsafe class DeviceExtensions
    {
        /// <summary>
        /// Give a user-friendly name to an object.
        /// <para>
        /// Applications may change the name associated with an object simply by calling <see
        /// cref="DebugMarkerSetObjectNameExt"/> again with a new string. To remove a previously set
        /// name, name should be set to an empty string.
        /// </para>
        /// </summary>
        /// <param name="device">The device that created the object.</param>
        /// <param name="nameInfo">Specifies the parameters of the name to set on the object.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void DebugMarkerSetObjectNameExt(this Device device, DebugMarkerObjectNameInfoExt nameInfo)
        {
            int nameByteCount = Interop.String.GetMaxByteCount(nameInfo.ObjectName);
            byte* nameBytes = stackalloc byte[nameByteCount];
            Interop.String.ToPointer(nameInfo.ObjectName, nameBytes, nameByteCount);
            nameInfo.ToNative(out DebugMarkerObjectNameInfoExt.Native nativeNameInfo, nameBytes);

            Result result = vkDebugMarkerSetObjectNameEXT(device)(device, &nativeNameInfo);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Attach arbitrary data to an object.
        /// <para>
        /// In addition to setting a name for an object, debugging and validation layers may have
        /// uses for additional binary data on a per-object basis that has no other place in the
        /// Vulkan API. For example, a <see cref="ShaderModule"/> could have additional debugging
        /// data attached to it to aid in offline shader tracing.
        /// </para>
        /// </summary>
        /// <param name="device">The device that created the object.</param>
        /// <param name="tagInfo">Specifies the parameters of the tag to attach to the object.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void DebugMarkerSetObjectTagExt(this Device device, DebugMarkerObjectTagInfoExt tagInfo)
        {
            fixed (byte* tagPtr = tagInfo.Tag)
            {
                tagInfo.ToNative(out DebugMarkerObjectTagInfoExt.Native nativeTagInfo, tagPtr);
                vkDebugMarkerSetObjectTagEXT(device)(device, &nativeTagInfo);
            }
        }

        internal static DebugReportObjectTypeExt GetTypeForObject<T>(T obj)
        {
            string name = typeof(T).Name;
            bool success = Enum.TryParse<DebugReportObjectTypeExt>(name, out var type);
            return success ? type : DebugReportObjectTypeExt.Unknown;
        }

        /// <summary>
        /// Set the power state of a display.
        /// </summary>
        /// <param name="device">The display whose power state is modified.</param>
        /// <param name="display">A logical device associated with <paramref name="display"/>.</param>
        /// <param name="displayPowerInfo">Specifies the new power state of <paramref name="display"/>.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void DisplayPowerControlExt(this Device device, DisplayKhr display,
            DisplayPowerInfoExt displayPowerInfo)
        {
            displayPowerInfo.Prepare();
            Result result = vkDisplayPowerControlEXT(device)(device, display, &displayPowerInfo);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Signal a fence when a device event occurs.
        /// </summary>
        /// <param name="device">A logical device on which the event may occur.</param>
        /// <param name="deviceEventInfo">A structure describing the event of interest to the application.</param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting fence object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static Fence RegisterDeviceEventExt(this Device device, DeviceEventInfoExt deviceEventInfo,
            AllocationCallbacks? allocator = null)
        {
            deviceEventInfo.Prepare();

            AllocationCallbacks.Native* nativeAllocator = null;
            if (allocator.HasValue)
            {
                nativeAllocator = (AllocationCallbacks.Native*)Interop.Alloc<AllocationCallbacks.Native>();
                allocator.Value.ToNative(nativeAllocator);
            }

            long handle;
            Result result = vkRegisterDeviceEventEXT(device)(device, &deviceEventInfo, nativeAllocator, &handle);
            Interop.Free(nativeAllocator);
            VulkanException.ThrowForInvalidResult(result);
            return new Fence(device, ref allocator, handle);
        }

        /// <summary>
        /// Signal a fence when a display event occurs.
        /// </summary>
        /// <param name="device">A logical device associated with <paramref name="display"/>.</param>
        /// <param name="display">The display on which the event may occur.</param>
        /// <param name="displayEventInfo">
        /// The structure describing the event of interest to the application.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting fence object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static Fence RegisterDisplayEventExt(this Device device, DisplayKhr display, DisplayEventInfoExt displayEventInfo,
            AllocationCallbacks? allocator = null)
        {
            displayEventInfo.Prepare();

            AllocationCallbacks.Native* nativeAllocator = null;
            if (allocator.HasValue)
            {
                nativeAllocator = (AllocationCallbacks.Native*)Interop.Alloc<AllocationCallbacks.Native>();
                allocator.Value.ToNative(nativeAllocator);
            }

            long handle;
            Result result = vkRegisterDisplayEventEXT(device)(device, display, &displayEventInfo, nativeAllocator, &handle);
            Interop.Free(nativeAllocator);
            VulkanException.ThrowForInvalidResult(result);
            return new Fence(device, ref allocator, handle);
        }

        /// <summary>
        /// Function to set HDR metadata.
        /// </summary>
        /// <param name="device">The logical device where the swapchain(s) were created.</param>
        /// <param name="swapchains">The array of <see cref="SwapchainKhr"/> handles.</param>
        /// <param name="metadata">The array of <see cref="HdrMetadataExt"/> structures.</param>
        public static void SetHdrMetadataExt(this Device device, SwapchainKhr[] swapchains, HdrMetadataExt[] metadata)
        {
            int swapchainCount = swapchains?.Length ?? 0;
            var swapchainPtrs = stackalloc long[swapchainCount];
            for (int i = 0; i < swapchainCount; i++)
                swapchainPtrs[i] = swapchains[i];

            int metadataCount = metadata?.Length ?? 0;
            for (int i = 0; i < metadataCount; i++)
                metadata[i].Prepare();

            fixed (HdrMetadataExt* metadataPtr = metadata)
                vkSetHdrMetadataEXT(device)(device, swapchainCount, swapchainPtrs, metadataPtr);
        }

        /// <summary>
        /// Creates a new validation cache.
        /// </summary>
        /// <param name="device">The logical device that creates the validation cache object.</param>
        /// <param name="createInfo">The initial parameters for the validation cache object.</param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>Handle in which the resulting validation cache object is returned.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static ValidationCacheExt CreateValidationCacheExt(this Device device,
            ValidationCacheCreateInfoExt createInfo, AllocationCallbacks? allocator)
        {
            return new ValidationCacheExt(device, &createInfo, ref allocator);
        }

        /// <summary>
        /// Get properties of external memory host pointer.
        /// </summary>
        /// <param name="device">The logical device that will be importing <paramref name="hostPointer"/>.</param>
        /// <param name="handleType">The type of the handle <paramref name="hostPointer"/>.</param>
        /// <param name="hostPointer">The host pointer to import from.</param>
        /// <returns>Properties of external memory host pointer.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static MemoryHostPointerPropertiesExt GetMemoryHostPointerPropertiesExt(this Device device,
            ExternalMemoryHandleTypesKhr handleType, IntPtr hostPointer)
        {
            MemoryHostPointerPropertiesExt properties;
            Result result = vkGetMemoryHostPointerPropertiesEXT(device)(device, handleType, hostPointer, &properties);
            VulkanException.ThrowForInvalidResult(result);
            return properties;
        }

        private delegate Result vkDebugMarkerSetObjectNameEXTDelegate(IntPtr device, DebugMarkerObjectNameInfoExt.Native* nameInfo);
        private static vkDebugMarkerSetObjectNameEXTDelegate vkDebugMarkerSetObjectNameEXT(Device device) => device.GetProc<vkDebugMarkerSetObjectNameEXTDelegate>(nameof(vkDebugMarkerSetObjectNameEXT));

        private delegate Result vkDebugMarkerSetObjectTagEXTDelegate(IntPtr device, DebugMarkerObjectTagInfoExt.Native* tagInfo);
        private static vkDebugMarkerSetObjectTagEXTDelegate vkDebugMarkerSetObjectTagEXT(Device device) => device.GetProc<vkDebugMarkerSetObjectTagEXTDelegate>(nameof(vkDebugMarkerSetObjectTagEXT));

        private delegate Result vkDisplayPowerControlEXTDelegate(IntPtr device, long display, DisplayPowerInfoExt* displayPowerInfo);
        private static vkDisplayPowerControlEXTDelegate vkDisplayPowerControlEXT(Device device) => device.GetProc<vkDisplayPowerControlEXTDelegate>(nameof(vkDisplayPowerControlEXT));

        private delegate Result vkRegisterDisplayEventEXTDelegate(IntPtr device, long display, DisplayEventInfoExt* displayEventInfo, AllocationCallbacks.Native* allocator, long* fence);
        private static vkRegisterDisplayEventEXTDelegate vkRegisterDisplayEventEXT(Device device) => device.GetProc<vkRegisterDisplayEventEXTDelegate>(nameof(vkRegisterDisplayEventEXT));

        private delegate Result vkRegisterDeviceEventEXTDelegate(IntPtr device, DeviceEventInfoExt* deviceEventInfo, AllocationCallbacks.Native* allocator, long* fence);
        private static vkRegisterDeviceEventEXTDelegate vkRegisterDeviceEventEXT(Device device) => device.GetProc<vkRegisterDeviceEventEXTDelegate>(nameof(vkRegisterDeviceEventEXT));

        private delegate void vkSetHdrMetadataEXTDelegate(IntPtr device, int swapchainCount, long* swapchains, HdrMetadataExt* metadata);
        private static vkSetHdrMetadataEXTDelegate vkSetHdrMetadataEXT(Device device) => device.GetProc<vkSetHdrMetadataEXTDelegate>(nameof(vkSetHdrMetadataEXT));

        private delegate Result vkGetMemoryHostPointerPropertiesEXTDelegate(IntPtr device, ExternalMemoryHandleTypesKhr handleType, IntPtr hostPointer, MemoryHostPointerPropertiesExt* memoryHostPointerProperties);
        private static vkGetMemoryHostPointerPropertiesEXTDelegate vkGetMemoryHostPointerPropertiesEXT(Device device) => device.GetProc<vkGetMemoryHostPointerPropertiesEXTDelegate>(nameof(vkGetMemoryHostPointerPropertiesEXT));
    }

    /// <summary>
    /// Specify parameters of a name to give to an object.
    /// </summary>
    public unsafe struct DebugMarkerObjectNameInfoExt
    {
        /// <summary>
        /// Specifies specifying the type of the object to be named.
        /// </summary>
        public DebugReportObjectTypeExt ObjectType;
        /// <summary>
        /// The object to be named.
        /// </summary>
        public long Object;
        /// <summary>
        /// A unicode string specifying the name to apply to object.
        /// </summary>
        public string ObjectName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugMarkerObjectNameInfoExt"/> structure.
        /// </summary>
        /// <param name="obj">Vulkan object to be name.</param>
        /// <param name="name">Name to set.</param>
        public DebugMarkerObjectNameInfoExt(VulkanHandle<IntPtr> obj, string name)
        {
            ObjectType = GetTypeForObject(obj);
            Object = obj.Handle.ToInt64();
            ObjectName = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugMarkerObjectNameInfoExt"/> structure.
        /// </summary>
        /// <param name="obj">Vulkan object to name.</param>
        /// <param name="name">Name to set.</param>
        public DebugMarkerObjectNameInfoExt(VulkanHandle<long> obj, string name)
        {
            ObjectType = GetTypeForObject(obj);
            Object = obj.Handle;
            ObjectName = name;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public DebugReportObjectTypeExt ObjectType;
            public long Object;
            public byte* ObjectName;
        }

        internal void ToNative(out Native native, byte* objectName)
        {
            native.Type = StructureType.DebugMarkerObjectNameInfoExt;
            native.Next = IntPtr.Zero;
            native.ObjectType = ObjectType;
            native.Object = Object;
            native.ObjectName = objectName;
        }
    }

    /// <summary>
    /// Specify parameters of a tag to attach to an object.
    /// </summary>
    public unsafe struct DebugMarkerObjectTagInfoExt
    {
        /// <summary>
        /// Specifies the type of the object to be named.
        /// </summary>
        public DebugReportObjectTypeExt ObjectType;
        /// <summary>
        /// The object to be tagged.
        /// </summary>
        public long Object;
        /// <summary>
        /// A numerical identifier of the tag.
        /// </summary>
        public long TagName;
        /// <summary>
        /// Bytes containing the data to be associated with the object.
        /// </summary>
        public byte[] Tag;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugMarkerObjectTagInfoExt"/> structure.
        /// </summary>
        /// <param name="obj">Vulkan object to be tagged.</param>
        /// <param name="tagName">A numerical identifier of the tag.</param>
        /// <param name="tag">Bytes containing the data to be associated with the object.</param>
        public DebugMarkerObjectTagInfoExt(VulkanHandle<IntPtr> obj, long tagName, byte[] tag)
        {
            ObjectType = GetTypeForObject(obj);
            Object = obj.Handle.ToInt64();
            TagName = tagName;
            Tag = tag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugMarkerObjectTagInfoExt"/> structure.
        /// </summary>
        /// <param name="obj">Vulkan object to be tagged.</param>
        /// <param name="tagName">A numerical identifier of the tag.</param>
        /// <param name="tag">Bytes containing the data to be associated with the object.</param>
        public DebugMarkerObjectTagInfoExt(VulkanHandle<long> obj, long tagName, byte[] tag)
        {
            ObjectType = GetTypeForObject(obj);
            Object = obj.Handle;
            TagName = tagName;
            Tag = tag;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public DebugReportObjectTypeExt ObjectType;
            public long Object;
            public long TagName;
            public Size TagSize;
            public byte* Tag;
        }

        internal void ToNative(out Native native, byte* tag)
        {
            native.Type = StructureType.DebugMarkerObjectTagInfoExt;
            native.Next = IntPtr.Zero;
            native.ObjectType = ObjectType;
            native.Object = Object;
            native.TagName = TagName;
            native.TagSize = Tag?.Length ?? 0;
            native.Tag = tag;
        }
    }

    /// <summary>
    /// Describe the power state of a display.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayPowerInfoExt
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// The new power state of the display.
        /// </summary>
        public DisplayPowerStateExt PowerState;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayPowerInfoExt"/> structure.
        /// </summary>
        /// <param name="powerState">The new power state of the display.</param>
        public DisplayPowerInfoExt(DisplayPowerStateExt powerState)
        {
            Type = StructureType.DisplayPowerInfoExt;
            Next = IntPtr.Zero;
            PowerState = powerState;
        }

        internal void Prepare()
        {
            Type = StructureType.DisplayPowerInfoExt;
        }
    }

    /// <summary>
    /// Possible power states for a display.
    /// </summary>
    public enum DisplayPowerStateExt
    {
        /// <summary>
        /// Specifies that the display is powered down.
        /// </summary>
        Off = 0,
        /// <summary>
        /// Specifies that the display is in a low power mode, but may be able to transition back to
        /// <see cref="On"/> more quickly than if it were in <see cref="Off"/>.
        /// <para>This state may be the same as <see cref="Off"/>.</para>
        /// </summary>
        Suspend = 1,
        /// <summary>
        /// Specifies that the display is powered on.
        /// </summary>
        On = 2
    }

    /// <summary>
    /// Describe a device event to create.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceEventInfoExt
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// Specifies when the fence will be signaled.
        /// </summary>
        public DeviceEventTypeExt DeviceEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceEventInfoExt"/> structure.
        /// </summary>
        /// <param name="deviceEvent">Specifies when the fence will be signaled.</param>
        public DeviceEventInfoExt(DeviceEventTypeExt deviceEvent)
        {
            Type = StructureType.DeviceEventInfoExt;
            Next = IntPtr.Zero;
            DeviceEvent = deviceEvent;
        }

        internal void Prepare()
        {
            Type = StructureType.DeviceEventInfoExt;
        }
    }

    /// <summary>
    /// Describe a display event to create.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayEventInfoExt
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// Specifies when the fence will be signaled.
        /// </summary>
        public DisplayEventTypeExt DisplayEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayEventInfoExt"/> structure.
        /// </summary>
        /// <param name="displayEvent">Specifies when the fence will be signaled.</param>
        public DisplayEventInfoExt(DisplayEventTypeExt displayEvent)
        {
            Type = StructureType.DisplayEventInfoExt;
            Next = IntPtr.Zero;
            DisplayEvent = displayEvent;
        }

        internal void Prepare()
        {
            Type = StructureType.DisplayEventInfoExt;
        }
    }

    /// <summary>
    /// Events that can occur on a device object.
    /// </summary>
    public enum DeviceEventTypeExt
    {
        /// <summary>
        /// Specifies that the fence is signaled when a display is plugged into or unplugged from the
        /// specified device.
        /// <para>
        /// Applications can use this notification to determine when they need to re-enumerate the
        /// available displays on a device.
        /// </para>
        /// </summary>
        DisplayHotplug = 0
    }

    /// <summary>
    /// Events that can occur on a display object.
    /// </summary>
    public enum DisplayEventTypeExt
    {
        /// <summary>
        /// Specifies that the fence is signaled when the first pixel of the next display refresh
        /// cycle leaves the display engine for the display.
        /// </summary>
        FirstPixelOut = 0
    }

    /// <summary>
    /// Structure to specify HDR metadata.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HdrMetadataExt
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The mastering display's red primary in chromaticity coordinates.
        /// </summary>
        public XYColorExt DisplayPrimaryRed;
        /// <summary>
        /// The mastering display's green primary in chromaticity coordinates.
        /// </summary>
        public XYColorExt DisplayPrimaryGreen;
        /// <summary>
        /// The mastering display's blue primary in chromaticity coordinates.
        /// </summary>
        public XYColorExt DisplayPrimaryBlue;
        /// <summary>
        /// The mastering display's white-point in chromaticity coordinates.
        /// </summary>
        public XYColorExt WhitePoint;
        /// <summary>
        /// The maximum luminance of the mastering display in nits.
        /// </summary>
        public float MaxLuminance;
        /// <summary>
        /// The minimum luminance of the mastering display in nits.
        /// </summary>
        public float MinLuminance;
        /// <summary>
        /// Content's maximum luminance in nits.
        /// </summary>
        public float MaxContentLightLevel;
        /// <summary>
        /// The maximum frame average light level in nits.
        /// </summary>
        public float MaxFrameAverageLightLevel;

        internal void Prepare()
        {
            Type = StructureType.HdrMetadataExt;
        }
    }

    /// <summary>
    /// Structure to specify X,Y chromaticity coordinates.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct XYColorExt
    {
        /// <summary>
        /// The X coordinate of chromaticity limited to between 0 and 1.
        /// </summary>
        public float X;
        /// <summary>
        /// The Y coordinate of chromaticity limited to between 0 and 1.
        /// </summary>
        public float Y;
    }

    /// <summary>
    /// Specify a system wide priority.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceQueueGlobalPriorityCreateInfoExt
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The system-wide priority associated to this queue as specified by <see
        /// cref="QueueGlobalPriorityExt"/>.
        /// </summary>
        public QueueGlobalPriorityExt GlobalPriority;
    }

    /// <summary>
    /// Values specifying a system-wide queue priority.
    /// </summary>
    public enum QueueGlobalPriorityExt
    {
        /// <summary>
        /// Below the system default. Useful for non-interactive tasks.
        /// </summary>
        Low = 128,
        /// <summary>
        /// The system default priority.
        /// </summary>
        Medium = 256,
        /// <summary>
        /// Above the system default.
        /// </summary>
        High = 512,
        /// <summary>
        /// The highest priority. Useful for critical tasks.
        /// </summary>
        Realtime = 1024
    }

    /// <summary>
    /// Import memory from a host pointer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImportMemoryHostPointerInfoExt
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies the handle type.
        /// </summary>
        public ExternalMemoryHandleTypesKhr HandleType;
        /// <summary>
        /// The host pointer to import from.
        /// </summary>
        public IntPtr HostPointer;
    }

    /// <summary>
    /// Roperties of external memory host pointer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryHostPointerPropertiesExt
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bitmask containing one bit set for every memory type which the specified host pointer
        /// can be imported as.
        /// </summary>
        public int MemoryTypeBits;
    }

    /// <summary>
    /// Structure describing external memory host pointer limits that can be supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceExternalMemoryHostPropertiesExt
    {
        public StructureType Type;
        public IntPtr Next;
        public long MinImportedHostPointerAlignment;
    }
}
