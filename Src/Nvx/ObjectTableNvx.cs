using System;
using System.Runtime.InteropServices;

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
                Result result = vkCreateObjectTableNVX(this)(Parent, &nativeCreateInfo, NativeAllocator, &handle);
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        /// <summary>
        /// Gets the parent of this resource.
        /// </summary>
        public Device Parent { get; }

        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void RegisterObjects(ObjectTableEntryNvx[,] objectTableEntries, int[] objectIndices)
        {
            int x = objectTableEntries?.GetLength(0) ?? 0;
            int y = objectTableEntries?.GetLength(1) ?? 0;

            fixed (ObjectTableEntryNvx* objTableEntriesPtrsPtr = objectTableEntries)
            fixed (int* objIndicesPtr = objectIndices)
            {
                var ptrs = stackalloc ObjectTableEntryNvx*[x];
                for (int i = 0; i < x; i++)
                    ptrs[i] = &objTableEntriesPtrsPtr[i * y];
                Result result = vkRegisterObjectsNVX(this)(Parent, this, objectIndices?.Length ?? 0, ptrs, objIndicesPtr);
                VulkanException.ThrowForInvalidResult(result);
            }
        }

        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void UnregisterObjects(ObjectEntryTypeNvx[] objectEntryTypes, int[] objectIndices)
        {
            fixed (ObjectEntryTypeNvx* objEntryTypesPtr = objectEntryTypes)
            fixed (int* objIndicesPtr = objectIndices)
            {
                Result result = vkUnregisterObjectsNVX(this)(Parent, this, objectEntryTypes?.Length ?? 0, objEntryTypesPtr,
                    objIndicesPtr);
                VulkanException.ThrowForInvalidResult(result);
            }
        }

        /// <summary>
        /// Destroy an object table.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyObjectTableNVX(this)(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateObjectTableNVXDelegate(IntPtr device, ObjectTableCreateInfoNvx.Native* createInfo, AllocationCallbacks.Native* allocator, long* objectTable);
        private static vkCreateObjectTableNVXDelegate vkCreateObjectTableNVX(ObjectTableNvx objectTable) => GetProc<vkCreateObjectTableNVXDelegate>(objectTable, nameof(vkCreateObjectTableNVX));

        private delegate void vkDestroyObjectTableNVXDelegate(IntPtr device, long objectTable, AllocationCallbacks.Native* allocator);
        private static vkDestroyObjectTableNVXDelegate vkDestroyObjectTableNVX(ObjectTableNvx objectTable) => GetProc<vkDestroyObjectTableNVXDelegate>(objectTable, nameof(vkDestroyObjectTableNVX));

        private delegate Result vkRegisterObjectsNVXDelegate(IntPtr device, long objectTable, int objectCount, ObjectTableEntryNvx** ppObjectTableEntries, int* objectIndices);
        private static vkRegisterObjectsNVXDelegate vkRegisterObjectsNVX(ObjectTableNvx objectTable) => GetProc<vkRegisterObjectsNVXDelegate>(objectTable, nameof(vkRegisterObjectsNVX));

        private delegate Result vkUnregisterObjectsNVXDelegate(IntPtr device, long objectTable, int objectCount, ObjectEntryTypeNvx* objectEntryTypes, int* objectIndices);
        private static vkUnregisterObjectsNVXDelegate vkUnregisterObjectsNVX(ObjectTableNvx objectTable) => GetProc<vkUnregisterObjectsNVXDelegate>(objectTable, nameof(vkUnregisterObjectsNVX));

        private static TDelegate GetProc<TDelegate>(ObjectTableNvx objectTable, string name) where TDelegate : class => objectTable.Parent.GetProc<TDelegate>(name);
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

    /// <summary>
    /// Common parameters of an object table resource entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ObjectTableEntryNvx
    {
        /// <summary>
        /// Defines the entry type.
        /// </summary>
        public ObjectEntryTypeNvx Type;
        /// <summary>
        /// Defines which <see cref="PipelineBindPoint"/> the resource can be used with.
        /// <para>Some entry types allow only a single flag to be set.</para>
        /// </summary>
        public ObjectEntryUsagesNvx Flags;
    }

    /// <summary>
    /// Enum specifying object table entry type.
    /// </summary>
    public enum ObjectEntryTypeNvx
    {
        /// <summary>
        /// Indicates a <see cref="DescriptorSet"/> resource entry that is registered via <see cref="ObjectTableDescriptorSetEntryNvx"/>.
        /// </summary>
        DescriptorSet = 0,
        /// <summary>
        /// Indicates a <see cref="Pipeline"/> resource entry that is registered via <see cref="ObjectTablePipelineEntryNvx"/>.
        /// </summary>
        Pipeline = 1,
        /// <summary>
        /// Indicates a <see cref="Buffer"/> resource entry that is registered via <see cref="ObjectTableIndexBufferEntryNvx"/>.
        /// </summary>
        IndexBuffer = 2,
        /// <summary>
        /// Indicates a <see cref="Buffer"/> resource entry that is registered via <see cref="ObjectTableVertexBufferEntryNvx"/>.
        /// </summary>
        VertexBuffer = 3,
        /// <summary>
        /// Indicates the resource entry is registered via <see cref="ObjectTablePushConstantEntryNvx"/>.
        /// </summary>
        PushConstant = 4
    }

    /// <summary>
    /// Parameters of an object table pipeline entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ObjectTablePipelineEntryNvx
    {
        /// <summary>
        /// Defines the entry type.
        /// </summary>
        public ObjectEntryTypeNvx Type;
        /// <summary>
        /// Defines which <see cref="PipelineBindPoint"/> the resource can be used with.
        /// <para>Some entry types allow only a single flag to be set.</para>
        /// </summary>
        public ObjectEntryUsagesNvx Flags;
        /// <summary>
        /// Specifies the <see cref="VulkanCore.Pipeline"/> that this resource entry references.
        /// </summary>
        public long Pipeline;
    }

    /// <summary>
    /// Parameters of an object table descriptor set entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ObjectTableDescriptorSetEntryNvx
    {
        /// <summary>
        /// Defines the entry type.
        /// </summary>
        public ObjectEntryTypeNvx Type;
        /// <summary>
        /// Defines which <see cref="PipelineBindPoint"/> the resource can be used with.
        /// <para>Some entry types allow only a single flag to be set.</para>
        /// </summary>
        public ObjectEntryUsagesNvx Flags;
        /// <summary>
        /// Specifies the <see cref="VulkanCore.PipelineLayout"/> that the <see
        /// cref="DescriptorSet"/> is used with.
        /// </summary>
        public long PipelineLayout;
        /// <summary>
        /// Specifies the <see cref="VulkanCore.DescriptorSet"/> that can be bound with this entry.
        /// </summary>
        public long DescriptorSet;
    }

    /// <summary>
    /// Parameters of an object table vertex buffer entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ObjectTableVertexBufferEntryNvx
    {
        /// <summary>
        /// Defines the entry type.
        /// </summary>
        public ObjectEntryTypeNvx Type;
        /// <summary>
        /// Defines which <see cref="PipelineBindPoint"/> the resource can be used with.
        /// <para>Some entry types allow only a single flag to be set.</para>
        /// </summary>
        public ObjectEntryUsagesNvx Flags;
        /// <summary>
        /// Specifies the <see cref="VulkanCore.Buffer"/> that can be bound as vertex buffer.
        /// </summary>
        public long Buffer;
    }

    /// <summary>
    /// Parameters of an object table index buffer entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ObjectTableIndexBufferEntryNvx
    {
        /// <summary>
        /// Defines the entry type.
        /// </summary>
        public ObjectEntryTypeNvx Type;
        /// <summary>
        /// Defines which <see cref="PipelineBindPoint"/> the resource can be used with.
        /// <para>Some entry types allow only a single flag to be set.</para>
        /// </summary>
        public ObjectEntryUsagesNvx Flags;
        /// <summary>
        /// Specifies the <see cref="VulkanCore.Buffer"/> that can be bound as index buffer.
        /// </summary>
        public long Buffer;
        /// <summary>
        /// Specifies the <see cref="VulkanCore.IndexType"/> used with this index buffer.
        /// </summary>
        public IndexType IndexType;
    }

    /// <summary>
    /// Parameters of an object table push constant entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ObjectTablePushConstantEntryNvx
    {
        /// <summary>
        /// Defines the entry type.
        /// </summary>
        public ObjectEntryTypeNvx Type;
        /// <summary>
        /// Defines which <see cref="PipelineBindPoint"/> the resource can be used with.
        /// <para>Some entry types allow only a single flag to be set.</para>
        /// </summary>
        public ObjectEntryUsagesNvx Flags;
        /// <summary>
        /// Specifies the <see cref="VulkanCore.PipelineLayout"/> that push constants using this object index are used with.
        /// </summary>
        public long PipelineLayout;
        /// <summary>
        /// The <see cref="ShaderStages"/> that push constants using this object index are used with.
        /// </summary>
        public ShaderStages StageFlags;
    }
}
