using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a descriptor set layout object.
    /// <para>
    /// A descriptor set layout object is defined by an array of zero or more descriptor bindings.
    /// Each individual descriptor binding is specified by a descriptor type, a count (array size) of
    /// the number of descriptors in the binding, a set of shader stages that can access the binding,
    /// and (if using immutable samplers) an array of sampler descriptors.
    /// </para>
    /// </summary>
    public unsafe class DescriptorSetLayout : DisposableHandle<long>
    {
        internal DescriptorSetLayout(Device parent,
            ref DescriptorSetLayoutCreateInfo createInfo,
            ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            createInfo.ToNative(out DescriptorSetLayoutCreateInfo.Native nativeCreateInfo);
            long handle;
            Result result = vkCreateDescriptorSetLayout(Parent, &nativeCreateInfo, NativeAllocator, &handle);
            nativeCreateInfo.Free();
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Destroy a descriptor set layout object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyDescriptorSetLayout(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateDescriptorSetLayoutDelegate(IntPtr device, DescriptorSetLayoutCreateInfo.Native* createInfo, AllocationCallbacks.Native* allocator, long* setLayout);
        private static readonly vkCreateDescriptorSetLayoutDelegate vkCreateDescriptorSetLayout = VulkanLibrary.GetStaticProc<vkCreateDescriptorSetLayoutDelegate>(nameof(vkCreateDescriptorSetLayout));

        private delegate void vkDestroyDescriptorSetLayoutDelegate(IntPtr device, long descriptorSetLayout, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyDescriptorSetLayoutDelegate vkDestroyDescriptorSetLayout = VulkanLibrary.GetStaticProc<vkDestroyDescriptorSetLayoutDelegate>(nameof(vkDestroyDescriptorSetLayout));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created descriptor set layout.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct DescriptorSetLayoutCreateInfo
    {
        /// <summary>
        /// A bitmask specifying options for descriptor set layout creation.
        /// </summary>
        public DescriptorSetLayoutCreateFlags Flags;
        /// <summary>
        /// An array of <see cref="DescriptorSetLayoutBinding"/> structures.
        /// </summary>
        public DescriptorSetLayoutBinding[] Bindings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptorSetLayoutCreateInfo"/> structure.
        /// </summary>
        /// <param name="bindings">An array of <see cref="DescriptorSetLayoutBinding"/> structures.</param>
        public DescriptorSetLayoutCreateInfo(params DescriptorSetLayoutBinding[] bindings)
        {
            Flags = 0;
            Bindings = bindings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptorSetLayoutCreateInfo"/> structure.
        /// </summary>
        /// <param name="bindings">An array of <see cref="DescriptorSetLayoutBinding"/> structures.</param>
        /// <param name="flags">A bitmask specifying options for descriptor set layout creation.</param>
        public DescriptorSetLayoutCreateInfo(DescriptorSetLayoutBinding[] bindings, DescriptorSetLayoutCreateFlags flags = 0)
        {
            Flags = flags;
            Bindings = bindings;
        }

        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public DescriptorSetLayoutCreateFlags Flags;
            public int BindingCount;
            public DescriptorSetLayoutBinding.Native* Bindings;

            public void Free()
            {
                for (int i = 0; i < BindingCount; i++)
                    Bindings[i].Free();
                Interop.Free(Bindings);
            }
        }

        internal void ToNative(out Native native)
        {
            int bindingCount = Bindings?.Length ?? 0;
            var bindings = (DescriptorSetLayoutBinding.Native*)Interop.Alloc<DescriptorSetLayoutBinding.Native>(bindingCount);
            for (int i = 0; i < bindingCount; i++)
                Bindings[i].ToNative(&bindings[i]);

            native.Type = StructureType.DescriptorSetLayoutCreateInfo;
            native.Next = IntPtr.Zero;
            native.Flags = Flags;
            native.BindingCount = bindingCount;
            native.Bindings = bindings;
        }
    }

    /// <summary>
    /// Bitmask specifying descriptor set layout properties.
    /// </summary>
    [Flags]
    public enum DescriptorSetLayoutCreateFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that descriptor sets must not be allocated using this layout, and descriptors
        /// are instead pushed by <see cref="Khr.CommandBufferExtensions.CmdPushDescriptorSetKhr"/>.
        /// </summary>
        PushDescriptorKhr = 1 << 0
    }

    /// <summary>
    /// Structure specifying a descriptor set layout binding.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct DescriptorSetLayoutBinding
    {
        /// <summary>
        /// The binding number of this entry and corresponds to a resource of the same binding number
        /// in the shader stages.
        /// </summary>
        public int Binding;
        /// <summary>
        /// Specifies which type of resource descriptors are used for this binding.
        /// </summary>
        public DescriptorType DescriptorType;
        /// <summary>
        /// The number of descriptors contained in the binding, accessed in a shader as an array. If
        /// <see cref="DescriptorCount"/> is zero this binding entry is reserved and the resource
        /// must not be accessed from any stage via this binding within any pipeline using the set layout.
        /// </summary>
        public int DescriptorCount;
        /// <summary>
        /// Specifies which pipeline shader stages can access a resource for this binding. <see
        /// cref="ShaderStages.All"/> is a shorthand specifying that all defined shader stages,
        /// including any additional stages defined by extensions, can access the resource.
        /// <para>
        /// If a shader stage is not included in <see cref="StageFlags"/>, then a resource must not
        /// be accessed from that stage via this binding within any pipeline using the set layout.
        /// There are no limitations on what combinations of stages can be used by a descriptor
        /// binding, and in particular a binding can be used by both graphics stages and the compute stage.
        /// </para>
        /// </summary>
        public ShaderStages StageFlags;
        /// <summary>
        /// Affects initialization of samplers. If <see cref="DescriptorType"/> specifies a <see
        /// cref="VulkanCore.DescriptorType.Sampler"/> or <see
        /// cref="VulkanCore.DescriptorType.CombinedImageSampler"/> type descriptor, then <see
        /// cref="ImmutableSamplers"/> can be used to initialize a set of immutable samplers.
        /// Immutable samplers are permanently bound into the set layout; later binding a sampler
        /// into an immutable sampler slot in a descriptor set is not allowed. If <see
        /// cref="ImmutableSamplers"/> is not <c>null</c>, then it is considered to be an array of
        /// sampler handles that will be consumed by the set layout and used for the corresponding
        /// binding. If <see cref="ImmutableSamplers"/> is <c>null</c>, then the sampler slots are
        /// dynamic and sampler handles must be bound into descriptor sets using this layout. If <see
        /// cref="DescriptorType"/> is not one of these descriptor types, then <see
        /// cref="ImmutableSamplers"/> is ignored.
        /// </summary>
        public long[] ImmutableSamplers;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptorSetLayoutBinding"/> structure.
        /// </summary>
        /// <param name="binding">
        /// The binding number of this entry and corresponds to a resource of the same binding number
        /// in the shader stages.
        /// </param>
        /// <param name="descriptorType">
        /// Specifies which type of resource descriptors are used for this binding.
        /// </param>
        /// <param name="descriptorCount">
        /// The number of descriptors contained in the binding, accessed in a shader as an array. If
        /// <see cref="DescriptorCount"/> is zero this binding entry is reserved and the resource
        /// must not be accessed from any stage via this binding within any pipeline using the set layout.
        /// </param>
        /// <param name="stageFlags">
        /// Specifies which pipeline shader stages can access a resource for this binding. <see
        /// cref="ShaderStages.All"/> is a shorthand specifying that all defined shader stages,
        /// including any additional stages defined by extensions, can access the resource.
        /// </param>
        /// <param name="immutableSamplers">
        /// Affects initialization of samplers. If <see cref="DescriptorType"/> specifies a <see
        /// cref="VulkanCore.DescriptorType.Sampler"/> or <see
        /// cref="VulkanCore.DescriptorType.CombinedImageSampler"/> type descriptor, then <see
        /// cref="ImmutableSamplers"/> can be used to initialize a set of immutable samplers.
        /// Immutable samplers are permanently bound into the set layout; later binding a sampler
        /// into an immutable sampler slot in a descriptor set is not allowed. If <see
        /// cref="ImmutableSamplers"/> is not <c>null</c>, then it is considered to be an array of
        /// sampler handles that will be consumed by the set layout and used for the corresponding
        /// binding. If <see cref="ImmutableSamplers"/> is <c>null</c>, then the sampler slots are
        /// dynamic and sampler handles must be bound into descriptor sets using this layout. If <see
        /// cref="DescriptorType"/> is not one of these descriptor types, then <see
        /// cref="ImmutableSamplers"/> is ignored.
        /// </param>
        public DescriptorSetLayoutBinding(int binding, DescriptorType descriptorType, int descriptorCount,
            ShaderStages stageFlags = ShaderStages.All, Sampler[] immutableSamplers = null)
        {
            Binding = binding;
            DescriptorType = descriptorType;
            DescriptorCount = descriptorCount;
            StageFlags = stageFlags;
            ImmutableSamplers = immutableSamplers?.ToHandleArray();
        }

        internal struct Native
        {
            public int Binding;
            public DescriptorType DescriptorType;
            public int DescriptorCount;
            public ShaderStages StageFlags;
            public IntPtr ImmutableSamplers;

            public void Free()
            {
                Interop.Free(ImmutableSamplers);
            }
        }

        internal void ToNative(Native* native)
        {
            native->Binding = Binding;
            native->DescriptorType = DescriptorType;
            native->DescriptorCount = DescriptorCount;
            native->StageFlags = StageFlags;
            native->ImmutableSamplers = Interop.Struct.AllocToPointer(ImmutableSamplers);
        }
    }
}
