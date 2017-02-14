using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore.Nvx
{
    /// <summary>
    /// Opaque handle to an object table.
    /// <para>The device-side bindings are registered inside a table.</para>
    /// </summary>
    public unsafe class ObjectTableNvx : DisposableHandle<long>
    {
        internal ObjectTableNvx(Device parent, ref ObjectTableCreateInfoNvx createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            fixed (ObjectEntryTypeNvx* objEntryTypesPtr = createInfo.ObjectEntryTypes)
            fixed (int* objEntryCountsPtr = createInfo.ObjectEntryCounts)
            fixed (ObjectEntryUsagesNvx* objEntryUsagesPtr = createInfo.ObjectEntryUsageFlags)
            {
                createInfo.ToNative(out ObjectTableCreateInfoNvx.Native nativeCreateInfo, objEntryTypesPtr,
                    objEntryCountsPtr, objEntryUsagesPtr);
                long handle;
                Result result = CreateObjectTableNvx(Parent, &nativeCreateInfo, NativeAllocator, &handle);
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        /// <summary>
        /// Gets the parent of this resource.
        /// </summary>
        public Device Parent { get; }

        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void RegisterObjects(ObjectTableEntryNvx[,] objectTableEntries, int[] objectIndices) // TODO: validate mem layout
        {
            int x = objectTableEntries?.GetLength(0) ?? 0;
            int y = objectTableEntries?.GetLength(1) ?? 0;

            fixed (ObjectTableEntryNvx* objTableEntriesPtrsPtr = objectTableEntries)
            fixed (int* objIndicesPtr = objectIndices)
            {
                var ptrs = stackalloc ObjectTableEntryNvx*[x];
                for (int i = 0; i < x; i++)
                    ptrs[i] = &objTableEntriesPtrsPtr[i * y];
                Result result = RegisterObjectsNvx(Parent, this, objectIndices?.Length ?? 0, ptrs, objIndicesPtr);
                VulkanException.ThrowForInvalidResult(result);
            }
        }

        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void UnregisterObjects(ObjectEntryTypeNvx[] objectEntryTypes, int[] objectIndices)
        {
            fixed (ObjectEntryTypeNvx* objEntryTypesPtr = objectEntryTypes)
            fixed (int* objIndicesPtr = objectIndices)
            {
                Result result = UnregisterObjectsNvx(Parent, this, objectEntryTypes?.Length ?? 0, objEntryTypesPtr,
                    objIndicesPtr);
                VulkanException.ThrowForInvalidResult(result);
            }
        }

        /// <summary>
        /// Destroy an object table.
        /// </summary>
        public override void Dispose()
        {
            DestroyObjectTableNvx(Parent, this, NativeAllocator);
            base.Dispose();
        }

        [DllImport(VulkanDll, EntryPoint = "vkCreateObjectTableNVX", CallingConvention = CallConv)]
        private static extern Result CreateObjectTableNvx(IntPtr device, 
            ObjectTableCreateInfoNvx.Native* createInfo, AllocationCallbacks.Native* allocator, long* objectTable);

        [DllImport(VulkanDll, EntryPoint = "vkDestroyObjectTableNVX", CallingConvention = CallConv)]
        private static extern void DestroyObjectTableNvx(IntPtr device, long objectTable, AllocationCallbacks.Native* allocator);

        [DllImport(VulkanDll, EntryPoint = "vkRegisterObjectsNVX", CallingConvention = CallConv)]
        private static extern Result RegisterObjectsNvx(IntPtr device, long objectTable, 
            int objectCount, ObjectTableEntryNvx** objectTableEntries, int* objectIndices);

        [DllImport(VulkanDll, EntryPoint = "vkUnregisterObjectsNVX", CallingConvention = CallConv)]
        private static extern Result UnregisterObjectsNvx(IntPtr device, long objectTable, 
            int objectCount, ObjectEntryTypeNvx* objectEntryTypes, int* objectIndices);
    }

    /// <summary>
    /// Structure specifying the parameters of a newly created object table.
    /// </summary>
    public unsafe struct ObjectTableCreateInfoNvx
    {
        /// <summary>
        /// An array providing the entry type of a given configuration.
        /// </summary>
        public ObjectEntryTypeNvx[] ObjectEntryTypes;
        /// <summary>
        /// An array of how many objects can be registered in the table.
        /// </summary>
        public int[] ObjectEntryCounts;
        /// <summary>
        /// An array of bitmasks describing the binding usage of the entry.
        /// </summary>
        public ObjectEntryUsagesNvx[] ObjectEntryUsageFlags;
        /// <summary>
        /// The maximum number of <see cref="DescriptorType.UniformBuffer"/> or <see
        /// cref="DescriptorType.UniformBufferDynamic"/> used by any single registered <see
        /// cref="DescriptorSet"/> in this table.
        /// </summary>
        public int MaxUniformBuffersPerDescriptor;
        /// <summary>
        /// The maximum number of <see cref="DescriptorType.StorageBuffer"/> or <see
        /// cref="DescriptorType.StorageBufferDynamic"/> used by any single registered <see
        /// cref="DescriptorSet"/> in this table.
        /// </summary>
        public int MaxStorageBuffersPerDescriptor;
        /// <summary>
        /// The maximum number of <see cref="DescriptorType.StorageImage"/> or <see
        /// cref="DescriptorType.StorageTexelBuffer"/> used by any single registered <see
        /// cref="DescriptorSet"/> in this table.
        /// </summary>
        public int MaxStorageImagesPerDescriptor;
        /// <summary>
        /// The maximum number of <see cref="DescriptorType.Sampler"/>, <see
        /// cref="DescriptorType.CombinedImageSampler"/>, <see
        /// cref="DescriptorType.UniformTexelBuffer"/> or <see
        /// cref="DescriptorType.InputAttachment"/> used by any single registered <see
        /// cref="DescriptorSet"/> in this table.
        /// </summary>
        public int MaxSampledImagesPerDescriptor;
        /// <summary>
        /// The maximum number of unique <see cref="PipelineLayout"/> used by any registered <see
        /// cref="DescriptorSet"/> or <see cref="Pipeline"/> in this table.
        /// </summary>
        public int MaxPipelineLayouts;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public int ObjectCount;
            public ObjectEntryTypeNvx* ObjectEntryTypes;
            public int* ObjectEntryCounts;
            public ObjectEntryUsagesNvx* ObjectEntryUsageFlags;
            public int MaxUniformBuffersPerDescriptor;
            public int MaxStorageBuffersPerDescriptor;
            public int MaxStorageImagesPerDescriptor;
            public int MaxSampledImagesPerDescriptor;
            public int MaxPipelineLayouts;
        }

        internal void ToNative(out Native native, ObjectEntryTypeNvx* objectEntryTypes,
            int* objectEntryCounts, ObjectEntryUsagesNvx* objectEntryUsageFlags)
        {
            native.Type = StructureType.ObjectTableCreateInfoNvx;
            native.Next = IntPtr.Zero;
            native.ObjectCount = ObjectEntryTypes?.Length ?? 0;
            native.ObjectEntryTypes = objectEntryTypes;
            native.ObjectEntryCounts = objectEntryCounts;
            native.ObjectEntryUsageFlags = objectEntryUsageFlags;
            native.MaxUniformBuffersPerDescriptor = MaxUniformBuffersPerDescriptor;
            native.MaxStorageBuffersPerDescriptor = MaxStorageBuffersPerDescriptor;
            native.MaxStorageImagesPerDescriptor = MaxStorageImagesPerDescriptor;
            native.MaxSampledImagesPerDescriptor = MaxSampledImagesPerDescriptor;
            native.MaxPipelineLayouts = MaxPipelineLayouts;
        }
    }

    /// <summary>
    /// Bitmask specifying allowed usage of an object entry.
    /// </summary>
    [Flags]
    public enum ObjectEntryUsagesNvx
    {
        /// <summary>
        /// Indicates that the resource is bound to <see cref="PipelineBindPoint.Graphics"/>.
        /// </summary>
        Graphics = 1 << 0,
        /// <summary>
        /// Indicates that the resource is bound to <see cref="PipelineBindPoint.Compute"/>.
        /// </summary>
        Compute = 1 << 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ObjectTableEntryNvx
    {
        public ObjectEntryTypeNvx Type;
        public ObjectEntryUsagesNvx Flags;
    }

    /// <summary>
    /// Enum specifying object table entry type.
    /// </summary>
    public enum ObjectEntryTypeNvx
    {
        ObjectEntryDescriptorSet = 0,
        ObjectEntryPipeline = 1,
        ObjectEntryIndexBuffer = 2,
        ObjectEntryVertexBuffer = 3,
        ObjectEntryPushConstant = 4
    }
}
