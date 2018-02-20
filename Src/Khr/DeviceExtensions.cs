using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Khr
{
    /// <summary>
    /// Provides Khronos specific extension methods for the <see cref="Device"/> class.
    /// </summary>
    public unsafe static class DeviceExtensions
    {
        /// <summary>
        /// Create a swapchain.
        /// </summary>
        /// <param name="device">The device to create the swapchain for.</param>
        /// <param name="createInfo">The structure specifying the parameters of the created swapchain.</param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the swapchain object when there is no
        /// more specific allocator available.
        /// </param>
        /// <returns>Created swapchain object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SwapchainKhr CreateSwapchainKhr(this Device device, SwapchainCreateInfoKhr createInfo,
            AllocationCallbacks? allocator = null)
        {
            return new SwapchainKhr(device, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Create multiple swapchains that share presentable images.
        /// <para>
        /// Is similar to <see cref="CreateSwapchainKhr"/>, except that it takes an array of <see
        /// cref="SwapchainCreateInfoKhr"/> structures, and returns an array of swapchain objects.
        /// </para>
        /// <para>
        /// The swapchain creation parameters that affect the properties and number of presentable
        /// images must match between all the swapchains.If the displays used by any of the
        /// swapchains do not use the same presentable image layout or are incompatible in a way that
        /// prevents sharing images, swapchain creation will fail with the result code <see
        /// cref="Result.ErrorIncompatibleDisplayKhr"/>. If any error occurs, no swapchains will be
        /// created. Images presented to multiple swapchains must be re-acquired from all of them
        /// before transitioning away from <see cref="ImageLayout.PresentSrcKhr"/>. After destroying
        /// one or more of the swapchains, the remaining swapchains and the presentable images can
        /// continue to be used.
        /// </para>
        /// </summary>
        /// <param name="device">The device to create the swapchains for.</param>
        /// <param name="createInfos">Structures specifying the parameters of the created swapchains.</param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the swapchain objects when there is no
        /// more specific allocator available.
        /// </param>
        /// <returns>The created swapchain objects.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SwapchainKhr[] CreateSharedSwapchainsKhr(this Device device,
            SwapchainCreateInfoKhr[] createInfos, AllocationCallbacks? allocator = null)
        {
            return SwapchainKhr.CreateSharedKhr(device, createInfos, ref allocator);
        }

        /// <summary>
        /// Create a new descriptor update template.
        /// </summary>
        /// <param name="device">The logical device that creates the descriptor update template.</param>
        /// <param name="createInfo">
        /// Specifies the set of descriptors to update with a single call to <see cref="DescriptorSetExtensions.UpdateWithTemplateKhr"/>.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting descriptor update template object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static DescriptorUpdateTemplateKhr CreateDescriptorUpdateTemplateKhr(this Device device,
            DescriptorUpdateTemplateCreateInfoKhr createInfo, AllocationCallbacks? allocator = null)
        {
            return new DescriptorUpdateTemplateKhr(device, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Get properties of external memory Win32 handles.
        /// <para>
        /// Windows memory handles compatible with Vulkan may also be created by non-Vulkan APIs
        /// using methods beyond the scope of this specification.
        /// </para>
        /// </summary>
        /// <param name="device">The logical device that will be importing <paramref name="handle"/>.</param>
        /// <param name="handleType">The type of the handle <paramref name="handle"/>.</param>
        /// <param name="handle">the handle which will be imported.</param>
        /// <returns>Properties of <paramref name="handle"/>.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static MemoryWin32HandlePropertiesKhr GetMemoryWin32HandlePropertiesKhr(this Device device,
            ExternalMemoryHandleTypesKhr handleType, IntPtr handle)
        {
            MemoryWin32HandlePropertiesKhr properties;
            Result result = vkGetMemoryWin32HandlePropertiesKHR(device)(device, handleType, handle, &properties);
            VulkanException.ThrowForInvalidResult(result);
            return properties;
        }

        /// <summary>
        /// Get properties of external memory file descriptors.
        /// <para>
        /// POSIX file descriptor memory handles compatible with Vulkan may also be created by
        /// non-Vulkan APIs using methods beyond the scope of this specification.
        /// </para>
        /// </summary>
        /// <param name="device">The logical device that will be importing <paramref name="fd"/>.</param>
        /// <param name="handleType">The type of the handle <paramref name="fd"/>.</param>
        /// <param name="fd">The handle which will be imported.</param>
        /// <returns>Properties of the handle <paramref name="fd"/>.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static MemoryFdPropertiesKhr GetMemoryFdPropertiesKhr(this Device device,
            ExternalMemoryHandleTypesKhr handleType, int fd)
        {
            MemoryFdPropertiesKhr properties;
            Result result = vkGetMemoryFdPropertiesKHR(device)(device, handleType, fd, &properties);
            VulkanException.ThrowForInvalidResult(result);
            return properties;
        }

        /// <summary>
        /// Import a semaphore from a Windows HANDLE.
        /// </summary>
        /// <param name="device">The logical device that created the semaphore.</param>
        /// <returns>Structure specifying the semaphore and import parameters.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static ImportSemaphoreWin32HandleInfoKhr ImportSemaphoreWin32HandleKhr(this Device device)
        {
            ImportSemaphoreWin32HandleInfoKhr.Native nativeInfo;
            Result result = vkImportSemaphoreWin32HandleKHR(device)(device, &nativeInfo);
            VulkanException.ThrowForInvalidResult(result);
            var semaphore = new Semaphore(nativeInfo.Semaphore, device);
            ImportSemaphoreWin32HandleInfoKhr.FromNative(ref nativeInfo, semaphore, out var info);
            return info;
        }

        /// <summary>
        /// Import a semaphore from a POSIX file descriptor.
        /// </summary>
        /// <param name="device">The logical device that created the semaphore.</param>
        /// <returns>Structure specifying the semaphore and import parameters.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static ImportSemaphoreFdInfoKhr ImportSemaphoreFdKhr(this Device device)
        {
            ImportSemaphoreFdInfoKhr.Native nativeInfo;
            Result result = vkImportSemaphoreFdKHR(device)(device, &nativeInfo);
            VulkanException.ThrowForInvalidResult(result);
            var semaphore = new Semaphore(nativeInfo.Semaphore, device);
            ImportSemaphoreFdInfoKhr.FromNative(ref nativeInfo, semaphore, out var info);
            return info;
        }

        /// <summary>
        /// Returns the memory requirements for specified Vulkan object.
        /// </summary>
        /// <param name="device">The logical device that owns the buffer.</param>
        /// <param name="info">
        /// Structure containing parameters required for the memory requirements query.
        /// </param>
        /// <returns>Structure in which the memory requirements of the buffer object are returned.</returns>
        public static MemoryRequirements2Khr GetBufferMemoryRequirements2Khr(this Device device, BufferMemoryRequirementsInfo2Khr info)
        {
            MemoryRequirements2Khr memoryRequirements;
            vkGetBufferMemoryRequirements2KHR(device)(device, &info, &memoryRequirements);
            return memoryRequirements;
        }

        /// <summary>
        /// Returns the memory requirements for specified Vulkan object.
        /// </summary>
        /// <param name="device">The logical device that owns the image.</param>
        /// <param name="info">
        /// Structure containing parameters required for the memory requirements query.
        /// </param>
        /// <returns>Structure in which the memory requirements of the image object are returned.</returns>
        public static MemoryRequirements2Khr GetImageMemoryRequirements2Khr(this Device device, ImageMemoryRequirementsInfo2Khr info)
        {
            MemoryRequirements2Khr memoryRequirements;
            vkGetImageMemoryRequirements2KHR(device)(device, &info, &memoryRequirements);
            return memoryRequirements;
        }

        public static SparseImageMemoryRequirements2Khr[] GetImageSparseMemoryRequirements2Khr(this Device device,
            ImageSparseMemoryRequirementsInfo2Khr info)
        {
            int count;
            vkGetImageSparseMemoryRequirements2KHR(device)(device, &info, &count, null);

            var memoryRequirements = new SparseImageMemoryRequirements2Khr[count];
            fixed (SparseImageMemoryRequirements2Khr* memoryRequirementsPtr = memoryRequirements)
                vkGetImageSparseMemoryRequirements2KHR(device)(device, &info, &count, memoryRequirementsPtr);
            return memoryRequirements;
        }


        /// <summary>
        /// Bind device memory to buffer objects.
        /// </summary>
        /// <param name="device">The logical device that owns the buffers and memory.</param>
        /// <param name="bindInfos">Structures describing buffers and memory to bind.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void BindBufferMemory2Khr(this Device device, params BindBufferMemoryInfoKhr[] bindInfos)
        {
            int count = bindInfos?.Length ?? 0;
            var nativeBindInfos = stackalloc BindBufferMemoryInfoKhr.Native[count];
            for (int i = 0; i < count; i++)
                bindInfos[i].ToNative(out nativeBindInfos[i]);
            Result result = vkBindBufferMemory2KHR(device)(device, count, nativeBindInfos);
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
        public static void BindImageMemory2Khr(this Device device, params BindImageMemoryInfoKhr[] bindInfos)
        {
            int count = bindInfos?.Length ?? 0;
            var nativeBindInfos = stackalloc BindImageMemoryInfoKhr.Native[count];
            for (int i = 0; i < count; i++)
                bindInfos[i].ToNative(out nativeBindInfos[i]);
            Result result = vkBindImageMemory2KHR(device)(device, count, nativeBindInfos);
            for (int i = 0; i < count; i++)
                nativeBindInfos[i].Free();
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Create a new Ycbcr conversion.
        /// </summary>
        /// <param name="device">The logical device that creates the sampler Y'C~B~C~R~ conversion.</param>
        /// <param name="createInfo">
        /// Specifies the requested sampler Y'C~B~C~R~ conversion.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting sampler Y'C~B~C~R~ conversion.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SamplerYcbcrConversionKhr CreateSamplerYcbcrConversionKhr(this Device device,
            SamplerYcbcrConversionCreateInfoKhr createInfo, AllocationCallbacks? allocator = null)
        {
            return new SamplerYcbcrConversionKhr(device, &createInfo, ref allocator);
        }

        private delegate Result vkGetMemoryWin32HandlePropertiesKHRDelegate(IntPtr device, ExternalMemoryHandleTypesKhr handleType, IntPtr handle, MemoryWin32HandlePropertiesKhr* memoryWin32HandleProperties);
        private static vkGetMemoryWin32HandlePropertiesKHRDelegate vkGetMemoryWin32HandlePropertiesKHR(Device device) => device.GetProc<vkGetMemoryWin32HandlePropertiesKHRDelegate>(nameof(vkGetMemoryWin32HandlePropertiesKHR));

        private delegate Result vkGetMemoryFdPropertiesKHRDelegate(IntPtr device, ExternalMemoryHandleTypesKhr handleType, int fd, MemoryFdPropertiesKhr* memoryFdProperties);
        private static vkGetMemoryFdPropertiesKHRDelegate vkGetMemoryFdPropertiesKHR(Device device) => device.GetProc<vkGetMemoryFdPropertiesKHRDelegate>(nameof(vkGetMemoryFdPropertiesKHR));

        private delegate Result vkImportSemaphoreWin32HandleKHRDelegate(IntPtr device, ImportSemaphoreWin32HandleInfoKhr.Native* importSemaphoreWin32HandleInfo);
        private static vkImportSemaphoreWin32HandleKHRDelegate vkImportSemaphoreWin32HandleKHR(Device device) => device.GetProc<vkImportSemaphoreWin32HandleKHRDelegate>(nameof(vkImportSemaphoreWin32HandleKHR));

        private delegate Result vkImportSemaphoreFdKHRDelegate(IntPtr device, ImportSemaphoreFdInfoKhr.Native* importSemaphoreFdInfo);
        private static vkImportSemaphoreFdKHRDelegate vkImportSemaphoreFdKHR(Device device) => device.GetProc<vkImportSemaphoreFdKHRDelegate>(nameof(vkImportSemaphoreFdKHR));

        private delegate void vkGetBufferMemoryRequirements2KHRDelegate(IntPtr device, BufferMemoryRequirementsInfo2Khr* info, MemoryRequirements2Khr* memoryRequirements);
        private static vkGetBufferMemoryRequirements2KHRDelegate vkGetBufferMemoryRequirements2KHR(Device device) => device.GetProc<vkGetBufferMemoryRequirements2KHRDelegate>(nameof(vkGetBufferMemoryRequirements2KHR));

        private delegate void vkGetImageMemoryRequirements2KHRDelegate(IntPtr device, ImageMemoryRequirementsInfo2Khr* info, MemoryRequirements2Khr* memoryRequirements);
        private static vkGetImageMemoryRequirements2KHRDelegate vkGetImageMemoryRequirements2KHR(Device device) => device.GetProc<vkGetImageMemoryRequirements2KHRDelegate>(nameof(vkGetImageMemoryRequirements2KHR));

        private delegate void vkGetImageSparseMemoryRequirements2KHRDelegate(IntPtr device, ImageSparseMemoryRequirementsInfo2Khr* info, int* sparseMemoryRequirementCount, SparseImageMemoryRequirements2Khr* sparseMemoryRequirements);
        private static vkGetImageSparseMemoryRequirements2KHRDelegate vkGetImageSparseMemoryRequirements2KHR(Device device) => device.GetProc<vkGetImageSparseMemoryRequirements2KHRDelegate>(nameof(vkGetImageSparseMemoryRequirements2KHR));

        private delegate Result vkBindBufferMemory2KHRDelegate(IntPtr device, int bindInfoCount, BindBufferMemoryInfoKhr.Native* bindInfos);
        private static vkBindBufferMemory2KHRDelegate vkBindBufferMemory2KHR(Device device) => device.GetProc<vkBindBufferMemory2KHRDelegate>(nameof(vkBindBufferMemory2KHR));

        private delegate Result vkBindImageMemory2KHRDelegate(IntPtr device, int bindInfoCount, BindImageMemoryInfoKhr.Native* bindInfos);
        private static vkBindImageMemory2KHRDelegate vkBindImageMemory2KHR(Device device) => device.GetProc<vkBindImageMemory2KHRDelegate>(nameof(vkBindImageMemory2KHR));
    }

    /// <summary>
    /// Structure specifying Windows handle to import to a semaphore.
    /// </summary>
    public struct ImportSemaphoreWin32HandleInfoKhr
    {
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The semaphore into which the state will be imported.
        /// </summary>
        public Semaphore Semaphore;
        /// <summary>
        /// Specifies additional parameters for the semaphore payload import operation.
        /// </summary>
        public SemaphoreImportFlagsKhr Flags;
        /// <summary>
        /// Specifies the type of <see cref="Handle"/>.
        /// </summary>
        public ExternalSemaphoreHandleTypesKhr HandleType;
        /// <summary>
        /// The external handle to import, or <c>null</c>.
        /// </summary>
        public IntPtr Handle;
        /// <summary>
        /// A NULL-terminated UTF-16 string naming the underlying synchronization primitive to
        /// import, or <c>null</c>.
        /// </summary>
        public IntPtr Name;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public long Semaphore;
            public SemaphoreImportFlagsKhr Flags;
            public ExternalSemaphoreHandleTypesKhr HandleType;
            public IntPtr Handle;
            public IntPtr Name;
        }

        internal static void FromNative(ref Native native, Semaphore semaphore,
            out ImportSemaphoreWin32HandleInfoKhr managed)
        {
            managed.Next = native.Next;
            managed.Semaphore = semaphore;
            managed.Flags = native.Flags;
            managed.HandleType = native.HandleType;
            managed.Handle = native.Handle;
            managed.Name = native.Name;
        }
    }

    /// <summary>
    /// Structure specifying POSIX file descriptor to import to a semaphore.
    /// </summary>
    public struct ImportSemaphoreFdInfoKhr
    {
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The semaphore into which the payload will be imported.
        /// </summary>
        public Semaphore Semaphore;
        /// <summary>
        /// Specifies additional parameters for the semaphore payload import operation.
        /// </summary>
        public SemaphoreImportFlagsKhr Flags;
        /// <summary>
        /// Specifies the type of <see cref="Fd"/>.
        /// </summary>
        public ExternalSemaphoreHandleTypesKhr HandleType;
        /// <summary>
        /// The external handle to import.
        /// </summary>
        public int Fd;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public long Semaphore;
            public SemaphoreImportFlagsKhr Flags;
            public ExternalSemaphoreHandleTypesKhr HandleType;
            public int Fd;
        }

        internal static void FromNative(ref Native native, Semaphore semaphore,
            out ImportSemaphoreFdInfoKhr managed)
        {
            managed.Next = native.Next;
            managed.Semaphore = semaphore;
            managed.Flags = native.Flags;
            managed.HandleType = native.HandleType;
            managed.Fd = native.Fd;
        }
    }

    /// <summary>
    /// Bitmask of valid external semaphore handle types.
    /// </summary>
    [Flags]
    public enum ExternalSemaphoreHandleTypesKhr
    {
        /// <summary>
        /// Specifies a POSIX file descriptor handle that has only limited valid usage outside of
        /// Vulkan and other compatible APIs.
        /// <para>
        /// It must be compatible with the POSIX system calls <c>dup</c>, <c>dup2</c>, <c>close</c>,
        /// and the non-standard system call <c>dup3</c>. Additionally, it must be transportable over
        /// a socket using an <c>SCM_RIGHTS</c> control message.
        /// </para>
        /// <para>
        /// It owns a reference to the underlying synchronization primitive represented by its Vulkan
        /// semaphore object.
        /// </para>
        /// </summary>
        OpaqueFd = 1 << 0,
        /// <summary>
        /// Specifies an NT handle that has only limited valid usage outside of Vulkan and other
        /// compatible APIs.
        /// <para>
        /// It must be compatible with the functions <c>DuplicateHandle</c>, <c>CloseHandle</c>,
        /// <c>CompareObjectHandles</c>, <c>GetHandleInformation</c>, and <c>SetHandleInformation</c>.
        /// </para>
        /// <para>
        /// It owns a reference to the underlying synchronization primitive represented by its Vulkan
        /// semaphore object.
        /// </para>
        /// </summary>
        OpaqueWin32 = 1 << 1,
        /// <summary>
        /// Specifies a global share handle that has only limited valid usage outside of Vulkan and
        /// other compatible APIs.
        /// <para>It is not compatible with any native APIs.</para>
        /// <para>
        /// It does not own own a reference to the underlying synchronization primitive represented
        /// its Vulkan semaphore object, and will therefore become invalid when all Vulkan semaphore
        /// objects associated with it are destroyed.
        /// </para>
        /// </summary>
        OpaqueWin32Kmt = 1 << 2,
        /// <summary>
        /// Specifies an NT handle returned by <c>ID3D12Device::CreateSharedHandle</c> referring to a
        /// Direct3D 12 fence.
        /// <para>
        /// It owns a reference to the underlying synchronization primitive associated with the
        /// Direct3D fence.
        /// </para>
        /// </summary>
        D3D12Fence = 1 << 3,
        /// <summary>
        /// Specifies a POSIX file descriptor handle to a Linux or Android Fence object.
        /// <para>
        /// It can be used with any native API accepting a valid fence object file descriptor as input.
        /// </para>
        /// <para>
        /// It owns a reference to the underlying synchronization primitive associated with the file descriptor.
        /// </para>
        /// <para>
        /// Implementations which support importing this handle type must accept any type of fence FD
        /// supported by the native system they are running on.
        /// </para>
        /// </summary>
        SyncFd = 1 << 4
    }

    /// <summary>
    /// Properties of external memory windows handles.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryWin32HandlePropertiesKhr
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
    public struct MemoryFdPropertiesKhr
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

    [StructLayout(LayoutKind.Sequential)]
    public struct BufferMemoryRequirementsInfo2Khr
    {
        /// <summary>
        /// Type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The <see cref="VulkanCore.Buffer"/> to query.
        /// </summary>
        public long Buffer;
    }

    /// <summary>
    /// Structure specifying memory requirements.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryRequirements2Khr
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
        /// Describes the memory requirements of the resource.
        /// </summary>
        public MemoryRequirements MemoryRequirements;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageMemoryRequirementsInfo2Khr
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
        /// The <see cref="VulkanCore.Image"/> to query.
        /// </summary>
        public long Image;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageSparseMemoryRequirementsInfo2Khr
    {
        public StructureType Type;
        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        public long Image;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SparseImageMemoryRequirements2Khr
    {
        public StructureType Type;
        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        public SparseImageMemoryRequirements MemoryRequirements;
    }


    /// <summary>
    /// Structure specifying how to bind a buffer to memory.
    /// </summary>
    public struct BindBufferMemoryInfoKhr
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
        /// Initializes a new instance of the <see cref="BindBufferMemoryInfoKhr"/> structure.
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
        public BindBufferMemoryInfoKhr(Buffer buffer, DeviceMemory memory, long memoryOffset,
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
            native.Type = StructureType.BindBufferMemoryInfoKhr;
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
    public struct BindImageMemoryInfoKhr
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
        /// The start offset of the region of memory which is to be bound to the image. The number of
        /// bytes returned in the <see cref="MemoryRequirements.Size"/> member in memory, starting
        /// from <see cref="MemoryOffset"/> bytes, will be bound to the specified image.
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
        /// Initializes a new instance of the <see cref="BindImageMemoryInfoKhr"/> structure.
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
        public BindImageMemoryInfoKhr(Image image, DeviceMemory memory, long memoryOffset,
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
            native.Type = StructureType.BindBufferMemoryInfoKhr;
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
    /// Enum specifying the point clipping behaviour.
    /// </summary>
    public enum PointClippingBehaviorKhr
    {
        AllClipPlanes = 0,
        UserClipPlanesOnly = 1
    }

    /// <summary>
    /// Enum describing tessellation domain origin.
    /// </summary>
    public enum TessellationDomainOriginKhr
    {
        /// <summary>
        /// Indicates that the origin of the domain space is in the upper left corner.
        /// </summary>
        UpperLeft = 0,
        /// <summary>
        /// Indicates that the origin of the domain space is in the lower left corner.
        /// </summary>
        LowerLeft = 1
    }

    /// <summary>
    /// Structure specifying a subpass/input attachment pair and an aspect mask that can be read.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct InputAttachmentAspectReferenceKhr
    {
        /// <summary>
        /// An index into the parent <see cref="RenderPassCreateInfo.Subpasses"/>.
        /// </summary>
        public int Subpass;
        public int InputAttachmentIndex;
        /// <summary>
        /// A mask of which aspect(s) can be accessed within the specified subpass.
        /// </summary>
        public int AspectMask;
    }

    /// <summary>
    /// Structure specifying, for a given subpass/input attachment pair, which aspect can be read.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RenderPassInputAttachmentAspectCreateInfoKhr
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
        /// The number of elements in the <see cref="AspectReferences"/> array.
        /// </summary>
        public int AspectReferenceCount;
        /// <summary>
        /// Points to an array of <see cref="AspectReferenceCount"/> number of <see
        /// cref="InputAttachmentAspectReferenceKhr"/> structures describing which aspect(s) can be
        /// accessed for a given input attachment within a given subpass.
        /// </summary>
        public IntPtr AspectReferences;
    }

    /// <summary>
    /// Specify the intended usage of an image view.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageViewUsageCreateInfoKhr
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
        /// A bitmask describing the allowed usages of the image view.
        /// </summary>
        public ImageUsages Usage;
    }

    /// <summary>
    /// Structure specifying the orientation of the tessellation domain.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineTessellationDomainOriginStateCreateInfoKhr
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
        /// Controls the origin of the tessellation domain space.
        /// </summary>
        public TessellationDomainOriginKhr DomainOrigin;
    }

    /// <summary>
    /// Structure specifying how to bind an image plane to memory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BindImagePlaneMemoryInfoKhr
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
        /// The aspect of the disjoint image plane to bind.
        /// </summary>
        public ImageAspects PlaneAspect;
    }

    /// <summary>
    /// Structure specifying image plane for memory requirements.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImagePlaneMemoryRequirementsInfoKhr
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
        /// The aspect corresponding to the image plane to query.
        /// </summary>
        public ImageAspects PlaneAspect;
    }

    /// <summary>
    /// Specify that an image can be used with a particular set of formats.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageFormatListCreateInfoKhr
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
        /// The number of entries in the <see cref="ViewFormats"/> array.
        /// </summary>
        public int ViewFormatCount;
        /// <summary>
        /// An array which lists of all formats which can be used when creating views of this image.
        /// </summary>
        public IntPtr ViewFormats;
    }
}
