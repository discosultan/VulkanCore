using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a descriptor pool object.
    /// </summary>
    public unsafe class DescriptorPool : DisposableHandle<long>
    {
        internal DescriptorPool(Device parent, ref DescriptorPoolCreateInfo createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            fixed (DescriptorPoolSize* poolSizesPtr = createInfo.PoolSizes)
            {
                createInfo.ToNative(out DescriptorPoolCreateInfo.Native nativeCreateInfo, poolSizesPtr);
                long handle;
                Result result = vkCreateDescriptorPool(Parent, &nativeCreateInfo, NativeAllocator, &handle);
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Resets a descriptor pool object.
        /// <para>
        /// Resetting a descriptor pool recycles all of the resources from all of the descriptor sets
        /// allocated from the descriptor pool back to the descriptor pool, and the descriptor sets
        /// are implicitly freed.
        /// </para>
        /// </summary>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void Reset()
        {
            Result result = vkResetDescriptorPool(Parent, this, 0);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Allocate one or more descriptor sets.
        /// <para>
        /// The pool must have enough free descriptor capacity remaining to allocate the descriptor
        /// sets of the specified layouts.
        /// </para>
        /// <para>
        /// When a descriptor set is allocated, the initial state is largely uninitialized and all
        /// descriptors are undefined. However, the descriptor set can be bound in a command buffer
        /// without causing errors or exceptions. All entries that are statically used by a pipeline
        /// in a drawing or dispatching command must have been populated before the descriptor set is
        /// bound for use by that command. Entries that are not statically used by a pipeline can
        /// have uninitialized descriptors or descriptors of resources that have been destroyed, and
        /// executing a draw or dispatch with such a descriptor set bound does not cause undefined
        /// behavior. This means applications need not populate unused entries with dummy descriptors.
        /// </para>
        /// <para>
        /// If an allocation fails due to fragmentation, an indeterminate error is returned with an
        /// unspecified error code. Any returned error other than <see
        /// cref="Result.ErrorFragmentedPool"/> does not imply its usual meaning: applications should
        /// assume that the allocation failed due to fragmentation, and create a new descriptor pool.
        /// </para>
        /// </summary>
        /// <param name="allocateInfo">The structure describing parameters of the allocation.</param>
        public DescriptorSet[] AllocateSets(DescriptorSetAllocateInfo allocateInfo)
        {
            return DescriptorSet.Allocate(this, ref allocateInfo);
        }

        /// <summary>
        /// Update the contents of a descriptor set object.
        /// <para>
        /// The operations described by <paramref name="descriptorWrites"/> are performed first,
        /// followed by the operations described by <paramref name="descriptorCopies"/>. Within each
        /// array, the operations are performed in the order they appear in the array.
        /// </para>
        /// <para>
        /// Each element in the <paramref name="descriptorWrites"/> array describes an operation
        /// updating the descriptor set using descriptors for resources specified in the structure.
        /// </para>
        /// <para>
        /// Each element in the <paramref name="descriptorCopies"/> array is a structure describing an
        /// operation copying descriptors between sets.
        /// </para>
        /// </summary>
        /// <param name="descriptorWrites">The structures describing the descriptor sets to write to.</param>
        /// <param name="descriptorCopies">The structures describing the descriptor sets to copy between.</param>
        public void UpdateSets(WriteDescriptorSet[] descriptorWrites = null, CopyDescriptorSet[] descriptorCopies = null)
        {
            DescriptorSet.Update(this, descriptorWrites, descriptorCopies);
        }

        /// <summary>
        /// Free one or more descriptor sets.
        /// </summary>
        /// <param name="descriptorSets">An array of handles to <see cref="DescriptorSet"/> objects.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void FreeSets(params DescriptorSet[] descriptorSets)
        {
            DescriptorSet.Free(this, descriptorSets);
        }

        /// <summary>
        /// Destroy a descriptor pool object.
        /// <para>
        /// When a pool is destroyed, all descriptor sets allocated from the pool are implicitly
        /// freed and become invalid. Descriptor sets allocated from a given pool do not need to be
        /// freed before destroying that descriptor pool.
        /// </para>
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyDescriptorPool(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateDescriptorPoolDelegate(IntPtr device, DescriptorPoolCreateInfo.Native* createInfo, AllocationCallbacks.Native* allocator, long* descriptorPool);
        private static readonly vkCreateDescriptorPoolDelegate vkCreateDescriptorPool = VulkanLibrary.GetStaticProc<vkCreateDescriptorPoolDelegate>(nameof(vkCreateDescriptorPool));

        private delegate void vkDestroyDescriptorPoolDelegate(IntPtr device, long descriptorPool, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyDescriptorPoolDelegate vkDestroyDescriptorPool = VulkanLibrary.GetStaticProc<vkDestroyDescriptorPoolDelegate>(nameof(vkDestroyDescriptorPool));

        private delegate Result vkResetDescriptorPoolDelegate(IntPtr device, long descriptorPool, VkDescriptorPoolResetFlags flags);
        private static readonly vkResetDescriptorPoolDelegate vkResetDescriptorPool = VulkanLibrary.GetStaticProc<vkResetDescriptorPoolDelegate>(nameof(vkResetDescriptorPool));
    }

    // Is reserved for future use.
    [Flags]
    internal enum VkDescriptorPoolResetFlags
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying parameters of a newly created descriptor pool.
    /// </summary>
    public unsafe struct DescriptorPoolCreateInfo
    {
        /// <summary>
        /// A bitmask specifying certain supported operations on the pool.
        /// </summary>
        public DescriptorPoolCreateFlags Flags;
        /// <summary>
        /// The maximum number of descriptor sets that can be allocated from the pool.
        /// </summary>
        public int MaxSets;
        /// <summary>
        /// Structures, each containing a descriptor type and number of descriptors of that type to
        /// be allocated in the pool.
        /// </summary>
        public DescriptorPoolSize[] PoolSizes;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptorPoolCreateInfo"/> structure.
        /// </summary>
        /// <param name="maxSets">
        /// The maximum number of descriptor sets that can be allocated from the pool.
        /// </param>
        /// <param name="poolSizes">
        /// Structures, each containing a descriptor type and number of descriptors of that type to
        /// be allocated in the pool.
        /// </param>
        /// <param name="flags">A bitmask specifying certain supported operations on the pool.</param>
        public DescriptorPoolCreateInfo(
            int maxSets,
            DescriptorPoolSize[] poolSizes,
            DescriptorPoolCreateFlags flags = 0)
        {
            MaxSets = maxSets;
            PoolSizes = poolSizes;
            Flags = flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public DescriptorPoolCreateFlags Flags;
            public int MaxSets;
            public int PoolSizeCount;
            public DescriptorPoolSize* PoolSizes;
        }

        internal void ToNative(out Native native, DescriptorPoolSize* poolSizes)
        {
            native.Type = StructureType.DescriptorPoolCreateInfo;
            native.Next = IntPtr.Zero;
            native.Flags = Flags;
            native.MaxSets = MaxSets;
            native.PoolSizeCount = PoolSizes?.Length ?? 0;
            native.PoolSizes = poolSizes;
        }
    }

    /// <summary>
    /// Bitmask specifying certain supported operations on a descriptor pool.
    /// </summary>
    [Flags]
    public enum DescriptorPoolCreateFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that descriptor sets can return their individual allocations to the pool, i.e.
        /// all of <see cref="DescriptorPool.AllocateSets"/>, <see cref="DescriptorPool.FreeSets"/>,
        /// and <see cref="DescriptorPool.Reset"/> are allowed.
        /// <para>
        /// Otherwise, descriptor sets allocated from the pool must not be individually freed back to
        /// the pool, i.e. only <see cref="DescriptorPool.AllocateSets"/> and <see
        /// cref="DescriptorPool.Reset"/> are allowed.
        /// </para>
        /// </summary>
        FreeDescriptorSet = 1 << 0
    }

    /// <summary>
    /// Structure specifying descriptor pool size.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DescriptorPoolSize
    {
        /// <summary>
        /// The type of descriptor.
        /// </summary>
        public DescriptorType Type;
        /// <summary>
        /// The number of descriptors of that type to allocate.
        /// <para>Must be greater than 0.</para>
        /// </summary>
        public int DescriptorCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptorPoolSize"/> structure.
        /// </summary>
        /// <param name="type">The type of descriptor.</param>
        /// <param name="descriptorCount">The number of descriptors of that type to allocate.</param>
        public DescriptorPoolSize(DescriptorType type, int descriptorCount)
        {
            Type = type;
            DescriptorCount = descriptorCount;
        }
    }
}
