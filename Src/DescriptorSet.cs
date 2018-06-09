using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a descriptor set object.
    /// <para>Descriptor sets are allocated from descriptor pool objects.</para>
    /// </summary>
    public unsafe class DescriptorSet : DisposableHandle<long>
    {
        internal DescriptorSet(DescriptorPool parent, long handle)
        {
            Parent = parent;
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public DescriptorPool Parent { get; }

        /// <summary>
        /// Free descriptor set.
        /// </summary>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public override void Dispose()
        {
            if (!Disposed)
            {
                long handle = this;
                Result result = vkFreeDescriptorSets(Parent.Parent, Parent, 1, &handle);
                VulkanException.ThrowForInvalidResult(result);
            }
            base.Dispose();
        }

        internal static void Free(DescriptorPool parent, DescriptorSet[] descriptorSets)
        {
            int count = descriptorSets?.Length ?? 0;
            var descriptorSetsPtr = stackalloc long[count];
            for (int i = 0; i < count; i++)
                descriptorSetsPtr[i] = descriptorSets[i];

            Result result = vkFreeDescriptorSets(parent.Parent, parent, count, descriptorSetsPtr);
            VulkanException.ThrowForInvalidResult(result);
        }

        internal static DescriptorSet[] Allocate(DescriptorPool parent, ref DescriptorSetAllocateInfo createInfo)
        {
            fixed (long* setLayoutsPtr = createInfo.SetLayouts)
            {
                createInfo.ToNative(out DescriptorSetAllocateInfo.Native nativeCreateInfo, parent, setLayoutsPtr);

                int count = createInfo.SetLayouts?.Length ?? 0;

                var descriptorSetsPtr = stackalloc long[count];
                Result result = vkAllocateDescriptorSets(parent.Parent, &nativeCreateInfo, descriptorSetsPtr);
                VulkanException.ThrowForInvalidResult(result);

                var descriptorSets = new DescriptorSet[count];
                for (int i = 0; i < count; i++)
                    descriptorSets[i] = new DescriptorSet(parent, descriptorSetsPtr[i]);
                return descriptorSets;
            }
        }

        internal static void Update(DescriptorPool parent,
            WriteDescriptorSet[] descriptorWrites, CopyDescriptorSet[] descriptorCopies)
        {
            int descriptorWriteCount = descriptorWrites?.Length ?? 0;
            var nativeDescriptorWritesPtr = stackalloc WriteDescriptorSet.Native[descriptorWriteCount];
            for (int i = 0; i < descriptorWriteCount; i++)
                descriptorWrites[i].ToNative(&nativeDescriptorWritesPtr[i]);

            int descriptorCopyCount = descriptorCopies?.Length ?? 0;
            for (int i = 0; i < descriptorCopyCount; i++)
                descriptorCopies[i].Prepare();

            fixed (CopyDescriptorSet* descriptorCopiesPtr = descriptorCopies)
            {
                vkUpdateDescriptorSets(
                    parent.Parent,
                    descriptorWriteCount,
                    nativeDescriptorWritesPtr,
                    descriptorCopyCount,
                    descriptorCopiesPtr);
            }

            for (int i = 0; i < descriptorWriteCount; i++)
                nativeDescriptorWritesPtr[i].Free();
        }

        private delegate Result vkAllocateDescriptorSetsDelegate(IntPtr device, DescriptorSetAllocateInfo.Native* allocateInfo, long* descriptorSets);
        private static readonly vkAllocateDescriptorSetsDelegate vkAllocateDescriptorSets = VulkanLibrary.GetStaticProc<vkAllocateDescriptorSetsDelegate>(nameof(vkAllocateDescriptorSets));

        private delegate Result vkFreeDescriptorSetsDelegate(IntPtr device, long descriptorPool, int descriptorSetCount, long* descriptorSets);
        private static readonly vkFreeDescriptorSetsDelegate vkFreeDescriptorSets = VulkanLibrary.GetStaticProc<vkFreeDescriptorSetsDelegate>(nameof(vkFreeDescriptorSets));

        private delegate void vkUpdateDescriptorSetsDelegate(IntPtr device, int descriptorWriteCount, WriteDescriptorSet.Native* descriptorWrites, int descriptorCopyCount, CopyDescriptorSet* descriptorCopies);
        private static readonly vkUpdateDescriptorSetsDelegate vkUpdateDescriptorSets = VulkanLibrary.GetStaticProc<vkUpdateDescriptorSetsDelegate>(nameof(vkUpdateDescriptorSets));
    }

    /// <summary>
    /// Structure specifying the allocation parameters for descriptor sets.
    /// </summary>
    public unsafe struct DescriptorSetAllocateInfo
    {
        /// <summary>
        /// Determines the number of descriptor sets to be allocated from the pool.
        /// </summary>
        public int DescriptorSetCount;
        /// <summary>
        /// An array of <see cref="DescriptorSetLayout"/>, with each member specifying how the
        /// corresponding descriptor set is allocated.
        /// <para>Array length must be greater than 0.</para>
        /// </summary>
        public long[] SetLayouts;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptorSetAllocateInfo"/> structure.
        /// </summary>
        /// <param name="descriptorSetCount">
        /// Determines the number of descriptor sets to be allocated from the pool.
        /// </param>
        /// <param name="setLayouts">
        /// An array of descriptor set layouts, with each member specifying how the corresponding
        /// descriptor set is allocated.
        /// </param>
        public DescriptorSetAllocateInfo(int descriptorSetCount, params DescriptorSetLayout[] setLayouts)
        {
            DescriptorSetCount = descriptorSetCount;
            SetLayouts = setLayouts?.ToHandleArray();
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public long DescriptorPool;
            public int DescriptorSetCount;
            public long* SetLayouts;
        }

        internal void ToNative(out Native native, DescriptorPool pool, long* setLayouts)
        {
            native.Type = StructureType.DescriptorSetAllocateInfo;
            native.Next = IntPtr.Zero;
            native.DescriptorPool = pool;
            native.DescriptorSetCount = DescriptorSetCount;
            native.SetLayouts = setLayouts;
        }
    }

    /// <summary>
    /// Structure specifying the parameters of a descriptor set write operation.
    /// </summary>
    public unsafe struct WriteDescriptorSet
    {
        /// <summary>
        /// The destination descriptor set to update.
        /// </summary>
        public long DstSet;
        /// <summary>
        /// The descriptor binding within that set.
        /// </summary>
        public int DstBinding;
        /// <summary>
        /// The starting element in that array.
        /// </summary>
        public int DstArrayElement;
        /// <summary>
        /// The number of descriptors to update (the number of elements in <see cref="ImageInfo"/>,
        /// <see cref="BufferInfo"/>, or <see cref="TexelBufferView"/>).
        /// </summary>
        public int DescriptorCount;
        /// <summary>
        /// Specifies the type of each descriptor in <see cref="ImageInfo"/>, <see
        /// cref="BufferInfo"/>, or <see cref="TexelBufferView"/>, as described below. It must be the
        /// same type as that specified in <see cref="DescriptorSetLayoutBinding"/> for <see
        /// cref="DstSet"/> at <see cref="DstBinding"/>. The type of the descriptor also controls
        /// which array the descriptors are taken from.
        /// </summary>
        public DescriptorType DescriptorType;
        /// <summary>
        /// An array of <see cref="DescriptorImageInfo"/> structures or is ignored.
        /// </summary>
        public DescriptorImageInfo[] ImageInfo;
        /// <summary>
        /// An array of <see cref="DescriptorBufferInfo"/> structures or is ignored.
        /// </summary>
        public DescriptorBufferInfo[] BufferInfo;
        /// <summary>
        /// An array of <see cref="BufferView"/> handles or is ignored.
        /// </summary>
        public long[] TexelBufferView;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteDescriptorSet"/> structure.
        /// </summary>
        /// <param name="dstSet">The destination descriptor set to update.</param>
        /// <param name="dstBinding">The descriptor binding within that set.</param>
        /// <param name="dstArrayElement">The starting element in that array.</param>
        /// <param name="descriptorCount">
        /// The number of descriptors to update (the number of elements in <see cref="ImageInfo"/>,
        /// <see cref="BufferInfo"/>, or <see cref="TexelBufferView"/>).
        /// </param>
        /// <param name="descriptorType">
        /// Specifies the type of each descriptor in <see cref="ImageInfo"/>, <see
        /// cref="BufferInfo"/>, or <see cref="TexelBufferView"/>, as described below. It must be the
        /// same type as that specified in <see cref="DescriptorSetLayoutBinding"/> for <see
        /// cref="DstSet"/> at <see cref="DstBinding"/>. The type of the descriptor also controls
        /// which array the descriptors are taken from.
        /// </param>
        /// <param name="imageInfo">
        /// An array of <see cref="DescriptorImageInfo"/> structures or is ignored.
        /// </param>
        /// <param name="bufferInfo">
        /// An array of <see cref="DescriptorBufferInfo"/> structures or is ignored.
        /// </param>
        /// <param name="texelBufferView">An array of <see cref="BufferView"/> handles or is ignored.</param>
        public WriteDescriptorSet(DescriptorSet dstSet, int dstBinding, int dstArrayElement, int descriptorCount,
            DescriptorType descriptorType, DescriptorImageInfo[] imageInfo = null, DescriptorBufferInfo[] bufferInfo = null,
            BufferView[] texelBufferView = null)
        {
            DstSet = dstSet;
            DstBinding = dstBinding;
            DstArrayElement = dstArrayElement;
            DescriptorCount = descriptorCount;
            DescriptorType = descriptorType;
            ImageInfo = imageInfo;
            BufferInfo = bufferInfo;
            TexelBufferView = texelBufferView?.ToHandleArray();
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public long DstSet;
            public int DstBinding;
            public int DstArrayElement;
            public int DescriptorCount;
            public DescriptorType DescriptorType;
            public IntPtr ImageInfo;
            public IntPtr BufferInfo;
            public IntPtr TexelBufferView;

            public void Free()
            {
                Interop.Free(ImageInfo);
                Interop.Free(BufferInfo);
                Interop.Free(TexelBufferView);
            }
        }

        internal void ToNative(Native* native)
        {
            native->Type = StructureType.WriteDescriptorSet;
            native->DstSet = DstSet;
            native->DstBinding = DstBinding;
            native->DescriptorCount = DescriptorCount;
            native->DescriptorType = DescriptorType;
            native->ImageInfo = Interop.Struct.AllocToPointer(ImageInfo);
            native->BufferInfo = Interop.Struct.AllocToPointer(BufferInfo);
            native->TexelBufferView = Interop.Struct.AllocToPointer(TexelBufferView);
        }
    }

    /// <summary>
    /// Specifies the type of a descriptor in a descriptor set.
    /// </summary>
    public enum DescriptorType
    {
        /// <summary>
        /// Specifies a sampler descriptor.
        /// </summary>
        Sampler = 0,
        /// <summary>
        /// Specifies a combined image sampler descriptor.
        /// </summary>
        CombinedImageSampler = 1,
        /// <summary>
        /// Specifies a storage image descriptor.
        /// </summary>
        SampledImage = 2,
        /// <summary>
        /// Specifies a sampled image descriptor.
        /// </summary>
        StorageImage = 3,
        /// <summary>
        /// Specifies a uniform texel buffer descriptor.
        /// </summary>
        UniformTexelBuffer = 4,
        /// <summary>
        /// Specifies a storage texel buffer descriptor.
        /// </summary>
        StorageTexelBuffer = 5,
        /// <summary>
        /// Specifies a uniform buffer descriptor.
        /// </summary>
        UniformBuffer = 6,
        /// <summary>
        /// Specifies a storage buffer descriptor.
        /// </summary>
        StorageBuffer = 7,
        /// <summary>
        /// Specifies a dynamic uniform buffer descriptor.
        /// </summary>
        UniformBufferDynamic = 8,
        /// <summary>
        /// Specifies a dynamic storage buffer descriptor.
        /// </summary>
        StorageBufferDynamic = 9,
        /// <summary>
        /// Specifies a input attachment descriptor.
        /// </summary>
        InputAttachment = 10
    }

    /// <summary>
    /// Structure specifying descriptor image info.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DescriptorImageInfo
    {
        /// <summary>
        /// A sampler handle, and is used in descriptor updates for types <see
        /// cref="DescriptorType.Sampler"/> and <see cref="DescriptorType.CombinedImageSampler"/> if
        /// the binding being updated does not use immutable samplers.
        /// </summary>
        public long Sampler;
        /// <summary>
        /// An <see cref="ImageView"/> handle, and is used in descriptor updates for types <see
        /// cref="DescriptorType.SampledImage"/>, <see cref="DescriptorType.StorageImage"/>, <see
        /// cref="DescriptorType.CombinedImageSampler"/>, and <see cref="DescriptorType.InputAttachment"/>.
        /// </summary>
        public long ImageView;
        /// <summary>
        /// The layout that the image subresources accessible from <see cref="ImageView"/> will be in
        /// at the time this descriptor is accessed. Is used in descriptor updates for types <see
        /// cref="DescriptorType.SampledImage"/>, <see cref="DescriptorType.StorageImage"/>, <see
        /// cref="DescriptorType.CombinedImageSampler"/>, and <see cref="DescriptorType.InputAttachment"/>.
        /// </summary>
        public ImageLayout ImageLayout;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptorImageInfo"/> structure.
        /// </summary>
        /// <param name="sampler">
        /// A sampler handle, and is used in descriptor updates for types <see
        /// cref="DescriptorType.Sampler"/> and <see cref="DescriptorType.CombinedImageSampler"/> if
        /// the binding being updated does not use immutable samplers.
        /// </param>
        /// <param name="imageView">
        /// An image view handle, and is used in descriptor updates for types <see
        /// cref="DescriptorType.SampledImage"/>, <see cref="DescriptorType.StorageImage"/>, <see
        /// cref="DescriptorType.CombinedImageSampler"/>, and <see cref="DescriptorType.InputAttachment"/>.
        /// </param>
        /// <param name="imageLayout">
        /// The layout that the image will be in at the time this descriptor is accessed. Is used in
        /// descriptor updates for types <see cref="DescriptorType.SampledImage"/>, <see
        /// cref="DescriptorType.StorageImage"/>, <see cref="DescriptorType.CombinedImageSampler"/>,
        /// and <see cref="DescriptorType.InputAttachment"/>.
        /// </param>
        public DescriptorImageInfo(Sampler sampler, ImageView imageView, ImageLayout imageLayout)
        {
            Sampler = sampler;
            ImageView = imageView;
            ImageLayout = imageLayout;
        }
    }

    /// <summary>
    /// Structure specifying descriptor buffer info.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DescriptorBufferInfo
    {
        /// <summary>
        /// The <see cref="Buffer"/> resource.
        /// </summary>
        public long Buffer;
        /// <summary>
        /// The offset in bytes from the start of buffer. Access to buffer memory via this descriptor
        /// uses addressing that is relative to this starting offset.
        /// </summary>
        public long Offset;
        /// <summary>
        /// The size in bytes that is used for this descriptor update, or <see cref="WholeSize"/> to
        /// use the range from <see cref="Offset"/> to the end of the buffer.
        /// </summary>
        public long Range;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptorBufferInfo"/> structure.
        /// </summary>
        /// <param name="buffer">The buffer resource.</param>
        /// <param name="offset">
        /// The offset in bytes from the start of buffer. Access to buffer memory via this descriptor
        /// uses addressing that is relative to this starting offset.
        /// </param>
        /// <param name="range">
        /// The size in bytes that is used for this descriptor update, or <see cref="WholeSize"/> to
        /// use the range from <see cref="Offset"/> to the end of the buffer.
        /// </param>
        public DescriptorBufferInfo(Buffer buffer, long offset = 0, long range = WholeSize)
        {
            Buffer = buffer;
            Offset = offset;
            Range = range;
        }
    }

    /// <summary>
    /// Structure specifying a copy descriptor set operation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CopyDescriptorSet
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// Source <see cref="DescriptorSet"/>.
        /// </summary>
        public long SrcSet;
        /// <summary>
        /// Source binding.
        /// <para>
        /// Must be a valid binding within <see cref="SrcSet"/>. The sum of <see
        /// cref="SrcArrayElement"/> and <see cref="DescriptorCount"/> must be less than or equal to
        /// the number of array elements in the descriptor set binding specified by <see
        /// cref="SrcBinding"/>, and all applicable consecutive bindings.
        /// </para>
        /// </summary>
        public int SrcBinding;
        /// <summary>
        /// Array element within the source binding to copy from.
        /// </summary>
        public int SrcArrayElement;
        /// <summary>
        /// Destination <see cref="DescriptorSet"/>.
        /// </summary>
        public long DstSet;
        /// <summary>
        /// Destination binding.
        /// <para>
        /// Must be a valid binding within <see cref="DstSet"/>. The sum of <see
        /// cref="DstArrayElement"/> and <see cref="DescriptorCount"/> must be less than or equal to
        /// the number of array elements in the descriptor set binding specified by <see
        /// cref="DstBinding"/>, and all applicable consecutive bindings. If <see cref="SrcSet"/> is
        /// equal to <see cref="DstSet"/>, then the source and destination ranges of descriptors must
        /// not overlap, where the ranges may include array elements from consecutive bindings.
        /// </para>
        /// </summary>
        public int DstBinding;
        /// <summary>
        /// Array element within the destination binding to copy to.
        /// </summary>
        public int DstArrayElement;
        /// <summary>
        /// The number of descriptors to copy from the source to destination.
        /// <para>
        /// If <see cref="DescriptorCount"/> is greater than the number of remaining array elements
        /// in the source or destination binding, those affect consecutive bindings in a manner
        /// similar to <see cref="WriteDescriptorSet"/>.
        /// </para>
        /// </summary>
        public int DescriptorCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyDescriptorSet"/> structure.
        /// </summary>
        /// <param name="srcSet">Source descriptor set.</param>
        /// <param name="srcBinding">Source binding.</param>
        /// <param name="srcArrayElement">Array element within the source binding to copy from.</param>
        /// <param name="dstSet">Destination descriptor set.</param>
        /// <param name="dstBinding">Destination binding.</param>
        /// <param name="dstArrayElement">
        /// Array element within the destination binding to copy to.
        /// </param>
        /// <param name="descriptorCount">
        /// The number of descriptors to copy from the source to destination.
        /// </param>
        public CopyDescriptorSet(
            DescriptorSet srcSet, int srcBinding, int srcArrayElement,
            DescriptorSet dstSet, int dstBinding, int dstArrayElement,
            int descriptorCount)
        {
            Type = StructureType.CopyDescriptorSet;
            Next = IntPtr.Zero;
            SrcSet = srcSet;
            SrcBinding = srcBinding;
            SrcArrayElement = srcArrayElement;
            DstSet = dstSet;
            DstBinding = dstBinding;
            DstArrayElement = dstArrayElement;
            DescriptorCount = descriptorCount;
        }

        internal void Prepare()
        {
            Type = StructureType.CopyDescriptorSet;
        }
    }
}
