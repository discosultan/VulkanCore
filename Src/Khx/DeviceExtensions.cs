using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;

namespace VulkanCore.Khx
{
    /// <summary>
    /// Provides experimental Khronos specific extension methods for the <see cref="Device"/> class.
    /// </summary>
    public static unsafe class DeviceExtensions
    {
        /// <summary>
        /// Query supported peer memory features of a device.
        /// <para>
        /// Peer memory is memory that is allocated for a given physical device and then bound to a
        /// resource and accessed by a different physical device, in a logical device that represents
        /// multiple physical devices.
        /// </para>
        /// </summary>
        /// <param name="device">The logical device that owns the memory.</param>
        /// <param name="heapIndex">The index of the memory heap from which the memory is allocated.</param>
        /// <param name="localDeviceIndex">
        /// The device index of the physical device that performs the memory access.
        /// </param>
        /// <param name="remoteDeviceIndex">
        /// The device index of the physical device that the memory is allocated for.
        /// </param>
        /// <returns>
        /// A bitmask indicating which types of memory accesses are supported for the combination of
        /// heap, local, and remote devices.
        /// </returns>
        public static PeerMemoryFeaturesKhx GetDeviceGroupPeerMemoryFeaturesKhx(this Device device,
            int heapIndex, int localDeviceIndex, int remoteDeviceIndex)
        {
            PeerMemoryFeaturesKhx features;
            vkGetDeviceGroupPeerMemoryFeaturesKHX(device)(device, heapIndex, localDeviceIndex, remoteDeviceIndex, &features);
            return features;
        }

        /// <summary>
        /// Bind device memory to buffer objects.
        /// </summary>
        /// <param name="device">The logical device that owns the buffers and memory.</param>
        /// <param name="bindInfos">Structures describing buffers and memory to bind.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void BindBufferMemory2Khx(this Device device, params BindBufferMemoryInfoKhx[] bindInfos)
        {
            int count = bindInfos?.Length ?? 0;
            var nativeBindInfos = stackalloc BindBufferMemoryInfoKhx.Native[count];
            for (int i = 0; i < count; i++)
                bindInfos[i].ToNative(out nativeBindInfos[i]);
            Result result = vkBindBufferMemory2KHX(device)(device, count, nativeBindInfos);
            for (int i = 0; i < count; i++)
                nativeBindInfos[i].Free();
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Bind device memory to image objects.
        /// </summary>
        /// <param name="device">The logical device that owns the images and memory.</param>
        /// <param name="bindInfos">Structures describing images and memory to bind.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void BindImageMemory2Khx(this Device device, params BindImageMemoryInfoKhx[] bindInfos)
        {
            int count = bindInfos?.Length ?? 0;
            var nativeBindInfos = stackalloc BindImageMemoryInfoKhx.Native[count];
            for (int i = 0; i < count; i++)
                bindInfos[i].ToNative(out nativeBindInfos[i]);
            Result result = vkBindImageMemory2KHX(device)(device, count, nativeBindInfos);
            for (int i = 0; i < count; i++)
                nativeBindInfos[i].Free();
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Query present capabilities from other physical devices.
        /// <para>
        /// A logical device that represents multiple physical devices may support presenting from
        /// images on more than one physical device, or combining images from multiple physical devices.
        /// </para>
        /// </summary>
        /// <param name="device">The logical device.</param>
        /// <returns>Structure that is filled with the logical device's capabilities.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static DeviceGroupPresentCapabilitiesKhx GetGroupPresentCapabilitiesKhx(this Device device)
        {
            DeviceGroupPresentCapabilitiesKhx capabilities;
            Result result = vkGetDeviceGroupPresentCapabilitiesKHX(device)(device, &capabilities);
            VulkanException.ThrowForInvalidResult(result);
            return capabilities;
        }

        /// <summary>
        /// Query present capabilities for a surface.
        /// <para>Some surfaces may not be capable of using all the device group present modes.</para>
        /// </summary>
        /// <param name="device">The logical device.</param>
        /// <param name="surface">The surface.</param>
        /// <returns>
        /// A value that is filled with the supported device group present modes for the surface.
        /// </returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static DeviceGroupPresentModesKhx GetGroupSurfacePresentModesKhx(this Device device, SurfaceKhr surface)
        {
            DeviceGroupPresentModesKhx modes;
            Result result = vkGetDeviceGroupSurfacePresentModesKHX(device)(device, surface, &modes);
            VulkanException.ThrowForInvalidResult(result);
            return modes;
        }

        /// <summary>
        /// Retrieve the index of the next available presentable image.
        /// </summary>
        /// <param name="device">The device associated with <see cref="AcquireNextImageInfoKhx.Swapchain"/>.</param>
        /// <param name="acquireInfo">Structure containing parameters of the acquire.</param>
        /// <returns>The index of the next image to use.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static int AcquireNextImage2Khx(this Device device, AcquireNextImageInfoKhx acquireInfo)
        {
            int index;
            acquireInfo.Prepare();
            Result result = vkAcquireNextImage2KHX(device)(device, &acquireInfo, &index);
            VulkanException.ThrowForInvalidResult(result);
            return index;
        }

        private delegate void vkGetDeviceGroupPeerMemoryFeaturesKHXDelegate(IntPtr device, int heapIndex, int localDeviceIndex, int remoteDeviceIndex, PeerMemoryFeaturesKhx* peerMemoryFeatures);
        private static vkGetDeviceGroupPeerMemoryFeaturesKHXDelegate vkGetDeviceGroupPeerMemoryFeaturesKHX(Device device) => device.GetProc<vkGetDeviceGroupPeerMemoryFeaturesKHXDelegate>(nameof(vkGetDeviceGroupPeerMemoryFeaturesKHX));

        private delegate Result vkBindBufferMemory2KHXDelegate(IntPtr device, int bindInfoCount, BindBufferMemoryInfoKhx.Native* bindInfos);
        private static vkBindBufferMemory2KHXDelegate vkBindBufferMemory2KHX(Device device) => device.GetProc<vkBindBufferMemory2KHXDelegate>(nameof(vkBindBufferMemory2KHX));

        private delegate Result vkBindImageMemory2KHXDelegate(IntPtr device, int bindInfoCount, BindImageMemoryInfoKhx.Native* bindInfos);
        private static vkBindImageMemory2KHXDelegate vkBindImageMemory2KHX(Device device) => device.GetProc<vkBindImageMemory2KHXDelegate>(nameof(vkBindImageMemory2KHX));

        private delegate Result vkGetDeviceGroupPresentCapabilitiesKHXDelegate(IntPtr device, DeviceGroupPresentCapabilitiesKhx* deviceGroupPresentCapabilities);
        private static vkGetDeviceGroupPresentCapabilitiesKHXDelegate vkGetDeviceGroupPresentCapabilitiesKHX(Device device) => device.GetProc<vkGetDeviceGroupPresentCapabilitiesKHXDelegate>(nameof(vkGetDeviceGroupPresentCapabilitiesKHX));

        private delegate Result vkAcquireNextImage2KHXDelegate(IntPtr device, AcquireNextImageInfoKhx* acquireInfo, int* imageIndex);
        private static vkAcquireNextImage2KHXDelegate vkAcquireNextImage2KHX(Device device) => device.GetProc<vkAcquireNextImage2KHXDelegate>(nameof(vkAcquireNextImage2KHX));

        private delegate Result vkGetDeviceGroupSurfacePresentModesKHXDelegate(IntPtr device, long surface, DeviceGroupPresentModesKhx* modes);
        private static vkGetDeviceGroupSurfacePresentModesKHXDelegate vkGetDeviceGroupSurfacePresentModesKHX(Device device) => device.GetProc<vkGetDeviceGroupSurfacePresentModesKHXDelegate>(nameof(vkGetDeviceGroupSurfacePresentModesKHX));
    }

    /// <summary>
    /// Bitmask specifying supported peer memory features.
    /// </summary>
    [Flags]
    public enum PeerMemoryFeaturesKhx
    {
        /// <summary>
        /// Indicates that the memory can be accessed as the source of a
        /// <c>CommandBuffer.CmdCopy*</c> command.
        /// </summary>
        PeerMemoryFeatureCopySrcKhx = 1 << 0,
        /// <summary>
        /// Indicates that the memory can be accessed as the destination of a
        /// <c>CommandBuffer.CmdCopy*</c> command.
        /// </summary>
        PeerMemoryFeatureCopyDstKhx = 1 << 1,
        /// <summary>
        /// Indicates that the memory can be read as any other memory access type.
        /// </summary>
        PeerMemoryFeatureGenericSrcKhx = 1 << 2,
        /// <summary>
        /// Indicates that the memory can be written as any other memory access type.
        /// <para>Shader atomics are considered to be writes.</para>
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
        /// Indicates that any physical device with a presentation engine can present its own
        /// swapchain images.
        /// </summary>
        Local = 1 << 0,
        /// <summary>
        /// Indicates that any physical device with a presentation engine can present swapchain
        /// images from any physical device in its present mask.
        /// </summary>
        Remote = 1 << 1,
        /// <summary>
        /// Indicates that any physical device with a presentation engine can present the sum of
        /// swapchain images from any physical devices in its present mask.
        /// </summary>
        Sum = 1 << 2,
        /// <summary>
        /// Indicates that multiple physical devices with a presentation engine can each present
        /// their own swapchain images.
        /// </summary>
        LocalMultiDevice = 1 << 3
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

    /// <summary>
    /// Structure describing multiview features that can be supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceMultiviewFeaturesKhx
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Multiple views in a renderpass.
        /// </summary>
        public Bool Multiview;
        /// <summary>
        /// Multiple views in a renderpass w/ geometry shader.
        /// </summary>
        public Bool MultiviewGeometryShader;
        /// <summary>
        /// Multiple views in a renderpass w/ tessellation shader.
        /// </summary>
        public Bool MultiviewTessellationShader;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalDeviceMultiviewFeaturesKhx"/> structure.
        /// </summary>
        /// <param name="multiview">Multiple views in a renderpass.</param>
        /// <param name="multiviewGeometryShader">Multiple views in a renderpass w/ geometry shader.</param>
        /// <param name="multiviewTessellationShader">
        /// Multiple views in a renderpass w/ tessellation shader.
        /// </param>
        /// <param name="next">Pointer to next structure.</param>
        public PhysicalDeviceMultiviewFeaturesKhx(bool multiview, bool multiviewGeometryShader, bool multiviewTessellationShader,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.PhysicalDeviceMultiviewFeaturesKhx;
            Next = next;
            Multiview = multiview;
            MultiviewGeometryShader = multiviewGeometryShader;
            MultiviewTessellationShader = multiviewTessellationShader;
        }
    }

    /// <summary>
    /// Create a logical device from multiple physical devices.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct DeviceGroupDeviceCreateInfoKhx
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
        /// The number of elements in the <see cref="PhysicalDevices"/> array.
        /// </summary>
        public int PhysicalDeviceCount;
        /// <summary>
        /// A pointer to an array of <see cref="PhysicalDevice"/> handles belonging to the same
        /// device group.
        /// </summary>
        public IntPtr PhysicalDevices;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceGroupDeviceCreateInfoKhx"/> structure.
        /// </summary>
        /// <param name="physicalDeviceCount">
        /// The number of elements in the <paramref name="physicalDevices"/> array.
        /// </param>
        /// <param name="physicalDevices">
        /// A pointer to an array of <see cref="PhysicalDevice"/> handles belonging to the same
        /// device group.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public DeviceGroupDeviceCreateInfoKhx(int physicalDeviceCount, IntPtr physicalDevices, IntPtr next = default(IntPtr))
        {
            Type = StructureType.DeviceGroupDeviceCreateInfoKhx;
            Next = next;
            PhysicalDeviceCount = physicalDeviceCount;
            PhysicalDevices = physicalDevices;
        }
    }

    /// <summary>
    /// Structure specifying swapchain image memory to bind to.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BindImageMemorySwapchainInfoKhx
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
        /// Is 0 or a <see cref="SwapchainKhr"/> handle.
        /// </summary>
        public long Swapchain;
        /// <summary>
        /// An image index within <see cref="Swapchain"/>.
        /// </summary>
        public int ImageIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindImageMemorySwapchainInfoKhx"/> structure.
        /// </summary>
        /// <param name="swapchain">Is <c>null</c> or a <see cref="SwapchainKhr"/> handle.</param>
        /// <param name="imageIndex">An image index within <see cref="Swapchain"/>.</param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public BindImageMemorySwapchainInfoKhx(SwapchainKhr swapchain, int imageIndex, IntPtr next = default(IntPtr))
        {
            Type = StructureType.BindImageMemorySwapchainInfoKhx;
            Next = next;
            Swapchain = swapchain;
            ImageIndex = imageIndex;
        }
    }

    /// <summary>
    /// Structure specifying parameters of a newly created swapchain object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceGroupSwapchainCreateInfoKhx
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
        /// A bitfield of modes that the swapchain can be used with.
        /// </summary>
        public DeviceGroupPresentModesKhx Modes;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceGroupSwapchainCreateInfoKhx"/> structure.
        /// </summary>
        /// <param name="modes">A bitfield of modes that the swapchain can be used with.</param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public DeviceGroupSwapchainCreateInfoKhx(DeviceGroupPresentModesKhx modes, IntPtr next = default(IntPtr))
        {
            Type = StructureType.DeviceGroupSwapchainCreateInfoKhx;
            Next = next;
            Modes = modes;
        }
    }

    /// <summary>
    /// Mode and mask controlling which physical devices' images are presented.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct DeviceGroupPresentInfoKhx
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
        /// Is zero or the number of elements in <see cref="DeviceMasks"/>.
        /// </summary>
        public int SwapchainCount;
        /// <summary>
        /// An array of device masks, one for each element of <see cref="PresentInfoKhr.Swapchains"/>.
        /// </summary>
        public int* DeviceMasks;
        /// <summary>
        /// The device group present mode that will be used for this present.
        /// </summary>
        public DeviceGroupPresentModesKhx Mode;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceGroupPresentInfoKhx"/> structure.
        /// </summary>
        /// <param name="swapchainCount">Is zero or the number of elements in <see cref="DeviceMasks"/>.</param>
        /// <param name="deviceMasks">An array of device masks, one for each element of <see cref="PresentInfoKhr.Swapchains"/>.</param>
        /// <param name="mode">The device group present mode that will be used for this present.</param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public DeviceGroupPresentInfoKhx(int swapchainCount, int* deviceMasks, DeviceGroupPresentModesKhx mode,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.DeviceGroupPresentInfoKhx;
            Next = next;
            SwapchainCount = swapchainCount;
            DeviceMasks = deviceMasks;
            Mode = mode;
        }
    }
}
