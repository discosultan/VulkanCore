using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;
using static VulkanCore.Constant;

namespace VulkanCore.Khx
{
    // TODO: doc
    public static unsafe class DeviceExtensions
    {
        /// <summary>
        /// Get properties of external memory Win32 handles.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="handleType"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static MemoryWin32HandlePropertiesKhx GetMemoryWin32HandlePropertiesKhx(this Device device,
            ExternalMemoryHandleTypesKhx handleType, IntPtr handle)
        {
            MemoryWin32HandlePropertiesKhx properties;
            Result result = GetMemoryWin32HandlePropertiesKhx(device, handleType, handle, &properties);
            VulkanException.ThrowForInvalidResult(result);
            return properties;
        }

        /// <summary>
        /// Get properties of external memory file descriptors.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="handleType"></param>
        /// <param name="fd"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static MemoryFdPropertiesKHX GetMemoryFdPropertiesKhx(this Device device,
            ExternalMemoryHandleTypesKhx handleType, int fd)
        {
            MemoryFdPropertiesKHX properties;
            Result result = GetMemoryFdPropertiesKhx(device, handleType, fd, &properties);
            VulkanException.ThrowForInvalidResult(result);
            return properties;
        }

        /// <summary>
        /// Import a semaphore from a Windows HANDLE.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static ImportSemaphoreWin32HandleInfoKhx ImportSemaphoreWin32HandleKhx(this Device device)
        {
            ImportSemaphoreWin32HandleInfoKhx.Native nativeInfo;
            Result result = ImportSemaphoreWin32HandleKhx(device, &nativeInfo);
            VulkanException.ThrowForInvalidResult(result);
            var semaphore = new Semaphore(nativeInfo.Semaphore, device);
            ImportSemaphoreWin32HandleInfoKhx.FromNative(ref nativeInfo, semaphore, out var info);
            return info;
        }

        /// <summary>
        /// Import a semaphore from a POSIX file descriptor.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static ImportSemaphoreFdInfoKhx ImportSemaphoreFdKhx(this Device device)
        {
            ImportSemaphoreFdInfoKhx.Native nativeInfo;
            Result result = ImportSemaphoreFdKhx(device, &nativeInfo);
            VulkanException.ThrowForInvalidResult(result);
            var semaphore = new Semaphore(nativeInfo.Semaphore, device);
            ImportSemaphoreFdInfoKhx.FromNative(ref nativeInfo, semaphore, out var info);
            return info;
        }

        /// <summary>
        /// Query supported peer memory features of a device.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="heapIndex"></param>
        /// <param name="localDeviceIndex"></param>
        /// <param name="remoteDeviceIndex"></param>
        /// <returns></returns>
        public static PeerMemoryFeaturesKhx GetDeviceGroupPeerMemoryFeaturesKhx(this Device device,
            int heapIndex, int localDeviceIndex, int remoteDeviceIndex)
        {
            PeerMemoryFeaturesKhx features;
            GetDeviceGroupPeerMemoryFeaturesKhx(device, heapIndex, localDeviceIndex, remoteDeviceIndex, &features);
            return features;
        }

        /// <summary>
        /// Bind device memory to buffer objects.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="bindInfos"></param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void BindBufferMemory2Khx(this Device device, params BindBufferMemoryInfoKhx[] bindInfos)
        {
            int count = bindInfos?.Length ?? 0;
            var nativeBindInfos = stackalloc BindBufferMemoryInfoKhx.Native[count];
            for (int i = 0; i < count; i++)
                bindInfos[i].ToNative(out nativeBindInfos[i]);
            Result result = BindBufferMemory2Khx(device, count, nativeBindInfos);
            for (int i = 0; i < count; i++)
                nativeBindInfos[i].Free();
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Bind device memory to image objects.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="bindInfos"></param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void BindImageMemory2Khx(this Device device, params BindImageMemoryInfoKhx[] bindInfos)
        {
            int count = bindInfos?.Length ?? 0;
            var nativeBindInfos = stackalloc BindImageMemoryInfoKhx.Native[count];
            for (int i = 0; i < count; i++)
                bindInfos[i].ToNative(out nativeBindInfos[i]);
            Result result = BindImageMemory2Khx(device, count, nativeBindInfos);
            for (int i = 0; i < count; i++)
                nativeBindInfos[i].Free();
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Query present capabilities from other physical devices.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static DeviceGroupPresentCapabilitiesKhx GetGroupPresentCapabilitiesKhx(this Device device)
        {
            DeviceGroupPresentCapabilitiesKhx capabilities;
            Result result = GetDeviceGroupPresentCapabilitiesKhx(device, &capabilities);
            VulkanException.ThrowForInvalidResult(result);
            return capabilities;
        }

        /// <summary>
        /// Query present capabilities for a surface.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="surface"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static DeviceGroupPresentModesKhx GetGroupSurfacePresentModesKhx(this Device device, SurfaceKhr surface)
        {
            DeviceGroupPresentModesKhx modes;
            Result result = GetDeviceGroupSurfacePresentModesKhx(device, surface, &modes);
            VulkanException.ThrowForInvalidResult(result);
            return modes;
        }

        /// <summary>
        /// Retrieve the index of the next available presentable image.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="acquireInfo"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static int AcquireNextImage2Khx(this Device device, AcquireNextImageInfoKhx acquireInfo)
        {
            int index;
            acquireInfo.Prepare();
            Result result = AcquireNextImage2Khx(device, &acquireInfo, &index);
            VulkanException.ThrowForInvalidResult(result);
            return index;
        }

        [DllImport(VulkanDll, EntryPoint = "vkGetMemoryWin32HandlePropertiesKHX", CallingConvention = CallConv)]
        private static extern Result GetMemoryWin32HandlePropertiesKhx(IntPtr device, 
            ExternalMemoryHandleTypesKhx handleType, IntPtr handle, MemoryWin32HandlePropertiesKhx* memoryWin32HandleProperties);

        [DllImport(VulkanDll, EntryPoint = "vkGetMemoryFdPropertiesKHX", CallingConvention = CallConv)]
        private static extern Result GetMemoryFdPropertiesKhx(IntPtr device,
            ExternalMemoryHandleTypesKhx handleType, int fd, MemoryFdPropertiesKHX* memoryFdProperties);

        [DllImport(VulkanDll, EntryPoint = "vkImportSemaphoreWin32HandleKHX", CallingConvention = CallConv)]
        private static extern Result ImportSemaphoreWin32HandleKhx(IntPtr device,
            ImportSemaphoreWin32HandleInfoKhx.Native* importSemaphoreWin32HandleInfo);

        [DllImport(VulkanDll, EntryPoint = "vkImportSemaphoreFdKHX", CallingConvention = CallConv)]
        private static extern Result ImportSemaphoreFdKhx(IntPtr device, 
            ImportSemaphoreFdInfoKhx.Native* importSemaphoreFdInfo);

        [DllImport(VulkanDll, EntryPoint = "vkGetDeviceGroupPeerMemoryFeaturesKHX", CallingConvention = CallConv)]
        private static extern void GetDeviceGroupPeerMemoryFeaturesKhx(IntPtr device, int heapIndex, 
            int localDeviceIndex, int remoteDeviceIndex, PeerMemoryFeaturesKhx* peerMemoryFeatures);

        [DllImport(VulkanDll, EntryPoint = "vkBindBufferMemory2KHX", CallingConvention = CallConv)]
        private static extern Result BindBufferMemory2Khx(IntPtr device, int bindInfoCount, BindBufferMemoryInfoKhx.Native* bindInfos);

        [DllImport(VulkanDll, EntryPoint = "vkBindImageMemory2KHX", CallingConvention = CallConv)]
        private static extern Result BindImageMemory2Khx(IntPtr device, int bindInfoCount, BindImageMemoryInfoKhx.Native* bindInfos);

        [DllImport(VulkanDll, EntryPoint = "vkGetDeviceGroupPresentCapabilitiesKHX", CallingConvention = CallConv)]
        private static extern Result GetDeviceGroupPresentCapabilitiesKhx(IntPtr device,
            DeviceGroupPresentCapabilitiesKhx* deviceGroupPresentCapabilities);

        [DllImport(VulkanDll, EntryPoint = "vkGetDeviceGroupSurfacePresentModesKHX", CallingConvention = CallConv)]
        private static extern Result GetDeviceGroupSurfacePresentModesKhx(IntPtr device,
            SurfaceKhr surface, DeviceGroupPresentModesKhx* modes);

        [DllImport(VulkanDll, EntryPoint = "vkAcquireNextImage2KHX", CallingConvention = CallConv)]
        private static extern Result AcquireNextImage2Khx(IntPtr device, AcquireNextImageInfoKhx* acquireInfo, int* imageIndex);
    }

    /// <summary>
    /// Bitmask of valid external memory handle types.
    /// </summary>
    [Flags]
    public enum ExternalMemoryHandleTypesKhx
    {
        OpaqueFd = 1 << 0,
        OpaqueWin32 = 1 << 1,
        OpaqueWin32Kmt = 1 << 2,
        D3D11Texture = 1 << 3,
        D3D11TextureKmt = 1 << 4,
        D3D12Heap = 1 << 5,
        D3D12Resource = 1 << 6
    }

    /// <summary>
    /// Properties of external memory windows handles.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryWin32HandlePropertiesKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bitmask containing one bit set for every memory type which the specified windows handle
        /// can be imported as.
        /// </summary>
        public int MemoryTypeBits;
    }

    /// <summary>
    /// Properties of external memory file descriptors.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryFdPropertiesKHX
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bitmask containing one bit set for every memory type which the specified file
        /// descriptor can be imported as.
        /// </summary>
        public int MemoryTypeBits;
    }

    public struct ImportSemaphoreWin32HandleInfoKhx
    {
        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        public Semaphore Semaphore;
        public ExternalSemaphoreHandleTypesKhx HandleType;
        public IntPtr Handle;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public long Semaphore;
            public ExternalSemaphoreHandleTypesKhx HandleType;
            public IntPtr Handle;
        }

        internal static void FromNative(ref Native native, Semaphore semaphore,
            out ImportSemaphoreWin32HandleInfoKhx managed)
        {
            managed.Next = native.Next;
            managed.Semaphore = semaphore;
            managed.HandleType = native.HandleType;
            managed.Handle = native.Handle;
        }
    }

    public struct ImportSemaphoreFdInfoKhx
    {
        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        public Semaphore Semaphore;
        public ExternalSemaphoreHandleTypesKhx HandleType;
        public int Fd;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public long Semaphore;
            public ExternalSemaphoreHandleTypesKhx HandleType;
            public int Fd;
        }

        internal static void FromNative(ref Native native, Semaphore semaphore,
            out ImportSemaphoreFdInfoKhx managed)
        {
            managed.Next = native.Next;
            managed.Semaphore = semaphore;
            managed.HandleType = native.HandleType;
            managed.Fd = native.Fd;
        }
    }

    /// <summary>
    /// Bitmask of valid external semaphore handle types.
    /// </summary>
    [Flags]
    public enum ExternalSemaphoreHandleTypesKhx
    {
        OpaqueFd = 1 << 0,
        OpaqueWin32 = 1 << 1,
        OpaqueWin32Kmt = 1 << 2,
        D3D12Fence = 1 << 3,
        FenceFd = 1 << 4
    }

    /// <summary>
    /// Bitmask specifying supported peer memory features.
    /// </summary>
    [Flags]
    public enum PeerMemoryFeaturesKhx
    {
        /// <summary>
        /// Can read with vkCmdCopy commands.
        /// </summary>
        PeerMemoryFeatureCopySrcKhx = 1 << 0,
        /// <summary>
        /// Can write with vkCmdCopy commands.
        /// </summary>
        PeerMemoryFeatureCopyDstKhx = 1 << 1,
        /// <summary>
        /// Can read with any access type/command.
        /// </summary>
        PeerMemoryFeatureGenericSrcKhx = 1 << 2,
        /// <summary>
        /// Can write with and access type/command.
        /// </summary>
        PeerMemoryFeatureGenericDstKhx = 1 << 3
    }

    /// <summary>
    /// Structure specifying how to bind a buffer to memory.
    /// </summary>
    public struct BindBufferMemoryInfoKhx
    {
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The <see cref="VulkanCore.Buffer"/> to be attached to memory.
        /// </summary>
        public long Buffer;
        /// <summary>
        /// A <see cref="DeviceMemory"/> object describing the device memory to attach.
        /// </summary>
        public long Memory;
        /// <summary>
        /// The start offset of the region of memory which is to be bound to the buffer. The number
        /// of bytes returned in the <see cref="MemoryRequirements.Size"/> member in memory, starting
        /// from <see cref="MemoryOffset"/> bytes, will be bound to the specified buffer.
        /// </summary>
        public long MemoryOffset;
        /// <summary>
        /// An array of device indices.
        /// </summary>
        public int[] DeviceIndices;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindBufferMemoryInfoKhx"/> structure.
        /// </summary>
        /// <param name="buffer">The <see cref="VulkanCore.Buffer"/> to be attached to memory.</param>
        /// <param name="memory">
        /// A <see cref="DeviceMemory"/> object describing the device memory to attach.
        /// </param>
        /// <param name="memoryOffset">
        /// The start offset of the region of memory which is to be bound to the buffer. The number
        /// of bytes returned in the <see cref="MemoryRequirements.Size"/> member in memory, starting
        /// from <see cref="MemoryOffset"/> bytes, will be bound to the specified buffer.
        /// </param>
        /// <param name="deviceIndices">An array of device indices.</param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public BindBufferMemoryInfoKhx(Buffer buffer, DeviceMemory memory, long memoryOffset,
            int[] deviceIndices, IntPtr next = default(IntPtr))
        {
            Next = next;
            Buffer = buffer;
            Memory = memory;
            MemoryOffset = memoryOffset;
            DeviceIndices = deviceIndices;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public long Buffer;
            public long Memory;
            public long MemoryOffset;
            public int DeviceIndexCount;
            public IntPtr DeviceIndices;

            public void Free()
            {
                Interop.Free(DeviceIndices);
            }
        }

        internal void ToNative(out Native native)
        {
            native.Type = StructureType.BindBufferMemoryInfoKhx;
            native.Next = Next;
            native.Buffer = Buffer;
            native.Memory = Memory;
            native.MemoryOffset = MemoryOffset;
            native.DeviceIndexCount = DeviceIndices?.Length ?? 0;
            native.DeviceIndices = Interop.Struct.AllocToPointer(DeviceIndices);
        }
    }

    /// <summary>
    /// Structure specifying how to bind an image to memory.
    /// </summary>
    public struct BindImageMemoryInfoKhx
    {
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The <see cref="VulkanCore.Image"/> to be attached to memory.
        /// </summary>
        public long Image;
        /// <summary>
        /// A <see cref="DeviceMemory"/> object describing the device memory to attach.
        /// </summary>
        public long Memory;
        /// <summary>
        /// The start offset of the region of memory which is to be bound to the image. If the length
        /// of <see cref="SFRRects"/> is zero, the number of bytes returned in the <see
        /// cref="MemoryRequirements.Size"/> member in memory, starting from <see
        /// cref="MemoryOffset"/> bytes, will be bound to the specified image.
        /// </summary>
        public long MemoryOffset;
        /// <summary>
        /// An array of device indices.
        /// </summary>
        public int[] DeviceIndices;
        /// <summary>
        /// An array of rectangles describing which regions of the image are attached to each
        /// instance of memory.
        /// </summary>
        public Rect2D[] SFRRects;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindImageMemoryInfoKhx"/> structure.
        /// </summary>
        /// <param name="image">The <see cref="VulkanCore.Image"/> to be attached to memory.</param>
        /// <param name="memory">
        /// A <see cref="DeviceMemory"/> object describing the device memory to attach.
        /// </param>
        /// <param name="memoryOffset">
        /// The start offset of the region of memory which is to be bound to the image. If the length
        /// of <see cref="SFRRects"/> is zero, the number of bytes returned in the <see
        /// cref="MemoryRequirements.Size"/> member in memory, starting from <see
        /// cref="MemoryOffset"/> bytes, will be bound to the specified image.
        /// </param>
        /// <param name="deviceIndices">An array of device indices.</param>
        /// <param name="sfrRects">
        /// An array of rectangles describing which regions of the image are attached to each
        /// instance of memory.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public BindImageMemoryInfoKhx(Image image, DeviceMemory memory, long memoryOffset,
            int[] deviceIndices, Rect2D[] sfrRects = null, IntPtr next = default(IntPtr))
        {
            Next = next;
            Image = image;
            Memory = memory;
            MemoryOffset = memoryOffset;
            DeviceIndices = deviceIndices;
            SFRRects = sfrRects;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public long Image;
            public long Memory;
            public long MemoryOffset;
            public int DeviceIndexCount;
            public IntPtr DeviceIndices;
            public int SFRRectCount;
            public IntPtr SFRRects;

            public void Free()
            {
                Interop.Free(DeviceIndices);
                Interop.Free(SFRRects);
            }
        }

        internal void ToNative(out Native native)
        {
            native.Type = StructureType.BindBufferMemoryInfoKhx;
            native.Next = Next;
            native.Image = Image;
            native.Memory = Memory;
            native.MemoryOffset = MemoryOffset;
            native.DeviceIndexCount = DeviceIndices?.Length ?? 0;
            native.DeviceIndices = Interop.Struct.AllocToPointer(DeviceIndices);
            native.SFRRectCount = SFRRects?.Length ?? 0;
            native.SFRRects = Interop.Struct.AllocToPointer(SFRRects);
        }
    }

    /// <summary>
    /// Present capabilities from other physical devices.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct DeviceGroupPresentCapabilitiesKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// An array of masks, where the mask at element [i] is non-zero if physical device [i] has a
        /// presentation engine, and where bit [j] is set in element [i] if physical device [i] can
        /// present swapchain images from physical device [j]. If element [i] is non-zero, then bit
        /// [i] must be set.
        /// </summary>
        public fixed int PresentMask[32];
        /// <summary>
        /// A bitmask indicating which device group presentation modes are supported.
        /// </summary>
        public DeviceGroupPresentModesKhx Modes;
    }

    /// <summary>
    /// Bitmask specifying supported device group present modes.
    /// </summary>
    [Flags]
    public enum DeviceGroupPresentModesKhx
    {
        /// <summary>
        /// Present from local memory.
        /// </summary>
        DeviceGroupPresentModeLocalKhx = 1 << 0,
        /// <summary>
        /// Present from remote memory.
        /// </summary>
        DeviceGroupPresentModeRemoteKhx = 1 << 1,
        /// <summary>
        /// Present sum of local and/or remote memory.
        /// </summary>
        DeviceGroupPresentModeSumKhx = 1 << 2,
        /// <summary>
        /// Each physical device presents from local memory.
        /// </summary>
        DeviceGroupPresentModeLocalMultiDeviceKhx = 1 << 3
    }

    /// <summary>
    /// Structure specifying parameters of the acquire.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AcquireNextImageInfoKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The <see cref="SwapchainKhr"/> from which an image is being acquired.
        /// </summary>
        public long Swapchain;
        /// <summary>
        /// Indicates how long the function waits, in nanoseconds, if no image is available.
        /// </summary>
        public long Timeout;
        /// <summary>
        /// Is <c>0</c> or a <see cref="Semaphore"/> to signal.
        /// </summary>
        public long Semaphore;
        /// <summary>
        /// Is <c>0</c> or a <see cref="Fence"/> to signal.
        /// </summary>
        public long Fence;
        /// <summary>
        /// A mask of physical devices for which the swapchain image will be ready to use when the
        /// semaphore or fence is signaled.
        /// </summary>
        public int DeviceMask;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcquireNextImageInfoKhx"/> structure.
        /// </summary>
        /// <param name="swapchain">
        /// The <see cref="SwapchainKhr"/> from which an image is being acquired.
        /// </param>
        /// <param name="timeout">
        /// Indicates how long the function waits, in nanoseconds, if no image is available.
        /// </param>
        /// <param name="semaphore">A <see cref="Semaphore"/> to signal.</param>
        /// <param name="fence">A <see cref="Fence"/> to signal.</param>
        /// <param name="deviceMask">
        /// A mask of physical devices for which the swapchain image will be ready to use when the
        /// semaphore or fence is signaled.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public AcquireNextImageInfoKhx(SwapchainKhr swapchain, long timeout,
            Semaphore semaphore = null, Fence fence = null, int deviceMask = 0, IntPtr next = default(IntPtr))
        {
            Type = StructureType.AcquireNextImageInfoKhx;
            Swapchain = swapchain;
            Timeout = timeout;
            Semaphore = semaphore;
            Fence = fence;
            DeviceMask = deviceMask;
            Next = next;
        }

        internal void Prepare()
        {
            Type = StructureType.AcquireNextImageInfoKhx;
        }
    }
}
