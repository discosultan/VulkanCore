using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a device memory object.
    /// <para>
    /// A Vulkan device operates on data in device memory via memory objects that are represented in
    /// the API by a <see cref="DeviceMemory"/> handle.
    /// </para>
    /// </summary>
    public unsafe class DeviceMemory : DisposableHandle<long>
    {
        internal DeviceMemory(Device parent, MemoryAllocateInfo* allocateInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            long handle;
            allocateInfo->Prepare();
            Result result = AllocateMemory(parent, allocateInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the owner of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Map a memory object into application address space.
        /// <para>
        /// It is an application error to call <see cref="Map"/> on a memory object that is already mapped.
        /// </para>
        /// <para>
        /// Will fail if the implementation is unable to allocate an appropriately sized contiguous
        /// virtual address range, e.g. due to virtual address space fragmentation or platform
        /// limits. In such cases, <see cref="Map"/> must return <see
        /// cref="Result.ErrorMemoryMapFailed"/>. The application can improve the likelihood of
        /// success by reducing the size of the mapped range and/or removing unneeded mappings using
        /// <see cref="Unmap"/>.
        /// </para>
        /// <para>
        /// Does not check whether the device memory is currently in use before returning the
        /// host-accessible pointer. The application must guarantee that any previously submitted
        /// command that writes to this range has completed before the host reads from or writes to
        /// that range, and that any previously submitted command that reads from that range has
        /// completed before the host writes to that region (see here for details on fulfilling such
        /// a guarantee). If the device memory was allocated without the <see
        /// cref="MemoryProperties.HostCoherent"/> set, these guarantees must be made for an extended
        /// range: the application must round down the start of the range to the nearest multiple of
        ///        <see cref="PhysicalDeviceLimits.NonCoherentAtomSize"/>, and round the end of the
        /// range up to the nearest multiple of <see cref="PhysicalDeviceLimits.NonCoherentAtomSize"/>.
        /// </para>
        /// <para>
        /// While a range of device memory is mapped for host access, the application is responsible
        /// for synchronizing both device and host access to that memory range.
        /// </para>
        /// </summary>
        /// <param name="offset">A zero-based byte offset from the beginning of the memory object.</param>
        /// <param name="size">
        /// The size of the memory range to map, or <see cref="WholeSize"/> to map from offset to the
        /// end of the allocation.
        /// </param>
        /// <returns>
        /// A pointer in which is returned a host-accessible pointer to the beginning of the mapped
        /// range. This pointer minus offset must be aligned to at least <see cref="PhysicalDeviceLimits.MinMemoryMapAlignment"/>.
        /// </returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public IntPtr Map(long offset, long size)
        {
            IntPtr ptr;
            Result result = MapMemory(Parent, this, offset, size, MemoryMapFlags.None, &ptr);
            VulkanException.ThrowForInvalidResult(result);
            return ptr;
        }

        /// <summary>
        /// Unmap a previously mapped memory object.
        /// </summary>
        public void Unmap()
        {
            UnmapMemory(Parent, this);
        }

        /// <summary>
        /// Query the current commitment for a <see cref="DeviceMemory"/>.
        /// <para>
        /// The implementation may update the commitment at any time, and the value returned by this
        /// query may be out of date.
        /// </para>
        /// <para>
        /// The implementation guarantees to allocate any committed memory from the heapIndex
        /// indicated by the memory type that the memory object was created with.
        /// </para>
        /// </summary>
        /// <returns>The number of bytes currently committed.</returns>
        public long GetCommitment()
        {
            long commitment;
            GetDeviceMemoryCommitment(Parent, this, &commitment);
            return commitment;
        }

        protected override void DisposeManaged()
        {
            FreeMemory(Parent, this, NativeAllocator);
            base.DisposeManaged();
        }

        [DllImport(VulkanDll, EntryPoint = "vkAllocateMemory", CallingConvention = CallConv)]
        private static extern Result AllocateMemory(IntPtr device, 
            MemoryAllocateInfo* allocateInfo, AllocationCallbacks.Native* allocator, long* memory);
        
        [DllImport(VulkanDll, EntryPoint = "vkFreeMemory", CallingConvention = CallConv)]
        private static extern void FreeMemory(IntPtr device, long memory, AllocationCallbacks.Native* Allocator);
                
        [DllImport(VulkanDll, EntryPoint = "vkMapMemory", CallingConvention = CallConv)]
        private static extern Result MapMemory(IntPtr device, long memory, long offset, long size, MemoryMapFlags flags, IntPtr* data);

        [DllImport(VulkanDll, EntryPoint = "vkUnmapMemory", CallingConvention = CallConv)]
        private static extern void UnmapMemory(IntPtr device, long memory);

        [DllImport(VulkanDll, EntryPoint = "vkGetDeviceMemoryCommitment", CallingConvention = CallConv)]
        private static extern void GetDeviceMemoryCommitment(IntPtr device, long memory, long* committedMemoryInBytes);
    }

    /// <summary>
    /// Structure containing parameters of a memory allocation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryAllocateInfo
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The size of the allocation in bytes. Must be greater than 0.
        /// </summary>
        public long AllocationSize;
        /// <summary>
        /// The memory type index, which selects the properties of the memory to be allocated, as
        /// well as the heap the memory will come from.
        /// </summary>
        public int MemoryTypeIndex;

        /// <summary>
        /// Initializes a new instance of <see cref="MemoryAllocateInfo"/> structure.
        /// </summary>
        /// <param name="allocationSize">
        /// The size of the allocation in bytes. Must be greater than 0.
        /// </param>
        /// <param name="memoryTypeIndex">
        /// The memory type index, which selects the properties of the memory to be allocated, as
        /// well as the heap the memory will come from.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public MemoryAllocateInfo(long allocationSize, int memoryTypeIndex, IntPtr next = default(IntPtr))
        {
            Type = StructureType.MemoryAllocateInfo;
            Next = next;
            AllocationSize = allocationSize;
            MemoryTypeIndex = memoryTypeIndex;
        }

        internal void Prepare()
        {
            Type = StructureType.MemoryAllocateInfo;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum MemoryMapFlags
    {
        None = 0,
    }
}