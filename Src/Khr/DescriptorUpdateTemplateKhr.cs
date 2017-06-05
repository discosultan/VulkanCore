using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Khr
{
    /// <summary>
    /// Opaque handle to a descriptor update template.
    /// <para>
    /// A descriptor update template specifies a mapping from descriptor update information in host
    /// memory to descriptors in a descriptor set. It is designed to avoid passing redundant
    /// information to the driver when frequently updating the same set of descriptors in descriptor sets.
    /// </para>
    /// </summary>
    public unsafe class DescriptorUpdateTemplateKhr : DisposableHandle<long>
    {
        internal DescriptorUpdateTemplateKhr(Device parent,
            ref DescriptorUpdateTemplateCreateInfoKhr createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            fixed (DescriptorUpdateTemplateEntryKhr* nativeDescriptorUpdateEntries = createInfo.DescriptorUpdateEntries)
            {
                createInfo.ToNative(out var nativeCreateInfo, nativeDescriptorUpdateEntries);
                long handle;
                Result result = vkCreateDescriptorUpdateTemplateKHR(parent, &nativeCreateInfo, NativeAllocator, &handle);
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Destroy a descriptor update template object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyDescriptorUpdateTemplateKHR(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateDescriptorUpdateTemplateKHRDelegate(IntPtr device, DescriptorUpdateTemplateCreateInfoKhr.Native* createInfo, AllocationCallbacks.Native* allocator, long* descriptorUpdateTemplate);
        private static readonly vkCreateDescriptorUpdateTemplateKHRDelegate vkCreateDescriptorUpdateTemplateKHR = VulkanLibrary.GetStaticProc<vkCreateDescriptorUpdateTemplateKHRDelegate>(nameof(vkCreateDescriptorUpdateTemplateKHR));

        private delegate void vkDestroyDescriptorUpdateTemplateKHRDelegate(IntPtr device, long descriptorUpdateTemplate, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyDescriptorUpdateTemplateKHRDelegate vkDestroyDescriptorUpdateTemplateKHR = VulkanLibrary.GetStaticProc<vkDestroyDescriptorUpdateTemplateKHRDelegate>(nameof(vkDestroyDescriptorUpdateTemplateKHR));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created descriptor update template.
    /// </summary>
    public unsafe struct DescriptorUpdateTemplateCreateInfoKhr
    {
        /// <summary>
        /// Structures describing the descriptors to be updated by the descriptor update template.
        /// </summary>
        public DescriptorUpdateTemplateEntryKhr[] DescriptorUpdateEntries;
        /// <summary>
        /// Specifies the type of the descriptor update template. If set to <see
        /// cref="DescriptorUpdateTemplateTypeKhr.DescriptorSet"/> it can only be used to update
        /// descriptor sets with a fixed <see cref="DescriptorSetLayout"/>. If set to <see
        /// cref="DescriptorUpdateTemplateTypeKhr.PushDescriptors"/> it can only be used to push
        /// descriptor sets using the provided <see cref="PipelineBindPoint"/>, <see
        /// cref="PipelineLayout"/>, and <see cref="Set"/> number.
        /// </summary>
        public DescriptorUpdateTemplateTypeKhr TemplateType;
        /// <summary>
        /// The <see cref="VulkanCore.DescriptorSetLayout"/> the parameter update template will be
        /// used with. All descriptor sets which are going to be updated through the newly created
        /// descriptor update template must be created with this layout. <see
        /// cref="DescriptorSetLayout"/> is the descriptor set layout used to build the descriptor
        /// update template. All descriptor sets which are going to be updated through the newly
        /// created descriptor update template must be created with a layout that matches (is the
        /// same as, or defined identically to) this layout. This parameter is ignored if <see
        /// cref="TemplateType"/> is not <see cref="DescriptorUpdateTemplateTypeKhr.DescriptorSet"/>.
        /// </summary>
        public long DescriptorSetLayout;
        /// <summary>
        /// Indicates whether the descriptors will be used by graphics pipelines or compute pipelines.
        /// </summary>
        public PipelineBindPoint PipelineBindPoint;
        /// <summary>
        /// The <see cref="PipelineLayout"/> object used to program the bindings.
        /// </summary>
        public long PipelineLayout;
        /// <summary>
        /// The set number of the descriptor set in the pipeline layout that will be updated.
        /// </summary>
        public int Set;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public DescriptorUpdateTemplateCreateFlagsKhr Flags;
            public int DescriptorUpdateEntryCount;
            public DescriptorUpdateTemplateEntryKhr* DescriptorUpdateEntries;
            public DescriptorUpdateTemplateTypeKhr TemplateType;
            public long DescriptorSetLayout;
            public PipelineBindPoint PipelineBindPoint;
            public long PipelineLayout;
            public int Set;
        }

        internal void ToNative(out Native native,
            DescriptorUpdateTemplateEntryKhr* nativeDescriptorUpdateEntries)
        {
            native.Type = StructureType.DescriptorUpdateTemplateCreateInfoKhr;
            native.Next = IntPtr.Zero;
            native.Flags = 0;
            native.DescriptorUpdateEntryCount = DescriptorUpdateEntries?.Length ?? 0;
            native.DescriptorUpdateEntries = nativeDescriptorUpdateEntries;
            native.TemplateType = TemplateType;
            native.DescriptorSetLayout = DescriptorSetLayout;
            native.PipelineBindPoint = PipelineBindPoint;
            native.PipelineLayout = PipelineLayout;
            native.Set = Set;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum DescriptorUpdateTemplateCreateFlagsKhr
    {
        None = 0
    }

    /// <summary>
    /// Describes a single descriptor update of the descriptor update template.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DescriptorUpdateTemplateEntryKhr
    {
        /// <summary>
        /// Must be a valid binding in the descriptor set layout implicitly specified when using a
        /// descriptor update template to update descriptors.
        /// </summary>
        public int DstBinding;
        /// <summary>
        /// And descriptorCount must be less than or equal to the number of array elements in the
        /// descriptor set binding implicitly specified when using a descriptor update template to
        /// update descriptors, and all applicable consecutive bindings.
        /// </summary>
        public int DstArrayElement;
        /// <summary>
        /// The number of descriptors to update. If descriptorCount is greater than the number of
        /// remaining array elements in the destination binding, those affect consecutive bindings in
        /// a manner similar to <see cref="WriteDescriptorSet"/> above.
        /// </summary>
        public int DescriptorCount;
        /// <summary>
        /// Specifies the type of the descriptor.
        /// </summary>
        public DescriptorType DescriptorType;
        /// <summary>
        /// The offset in bytes of the first binding in the raw data structure.
        /// </summary>
        public int Offset;
        /// <summary>
        /// The stride in bytes between two consecutive array elements of the descriptor update
        /// informations in the raw data structure. The stride is useful in case the bindings are
        /// stored in structs along with other data.
        /// </summary>
        public int Stride;
    }

    /// <summary>
    /// Indicates the valid usage of the descriptor update template.
    /// </summary>
    public enum DescriptorUpdateTemplateTypeKhr
    {
        /// <summary>
        /// Specifies that the descriptor update template will be used for descriptor set updates only.
        /// </summary>
        DescriptorSet = 0,
        /// <summary>
        /// Specifies that the descriptor update template will be used for push descriptor updates only.
        /// </summary>
        PushDescriptors = 1
    }
}
