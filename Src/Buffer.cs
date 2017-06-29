using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a buffer object.
    /// <para>
    /// Buffers represent linear arrays of data which are used for various purposes by binding them
    /// to a graphics or compute pipeline via descriptor sets or via certain commands, or by directly
    /// specifying them as parameters to certain commands.
    /// </para>
    /// </summary>
    public unsafe class Buffer : DisposableHandle<long>
    {
        internal Buffer(Device parent, ref BufferCreateInfo createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            fixed (int* queueFamilies = createInfo.QueueFamilyIndices)
            {
                createInfo.ToNative(out BufferCreateInfo.Native nativeCreateInfo, queueFamilies);
                long handle;
                Result result = vkCreateBuffer(
                    parent,
                    &nativeCreateInfo,
                    NativeAllocator,
                    &handle);
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Bind device memory to a buffer object.
        /// <para>Must not already be backed by a memory object.</para>
        /// <para>Must not have been created with any sparse memory binding flags.</para>
        /// </summary>
        /// <param name="memory">The object describing the device memory to attach.</param>
        /// <param name="memoryOffset">
        /// The start offset of the region of memory which is to be bound to the buffer. The number
        /// of bytes returned in the <see cref="MemoryRequirements.Size"/> member in memory, starting
        /// from <paramref name="memoryOffset"/> bytes, will be bound to the specified buffer.
        /// </param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void BindMemory(DeviceMemory memory, long memoryOffset = 0)
        {
            Result result = vkBindBufferMemory(Parent, this, memory, memoryOffset);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Create a new buffer view object.
        /// </summary>
        /// <param name="createInfo">
        /// The structure containing parameters to be used to create the buffer.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public BufferView CreateView(BufferViewCreateInfo createInfo, AllocationCallbacks? allocator = null)
        {
            return new BufferView(Parent, this, &createInfo, ref allocator);
        }

        /// <summary>
        /// Returns the memory requirements for the buffer.
        /// </summary>
        /// <returns>Memory requirements of the buffer object.</returns>
        public MemoryRequirements GetMemoryRequirements()
        {
            MemoryRequirements requirements;
            vkGetBufferMemoryRequirements(Parent, this, &requirements);
            return requirements;
        }

        /// <summary>
        /// Destroy a buffer object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyBuffer(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateBufferDelegate(IntPtr device, BufferCreateInfo.Native* createInfo, AllocationCallbacks.Native* allocator, long* buffer);
        private static readonly vkCreateBufferDelegate vkCreateBuffer = VulkanLibrary.GetStaticProc<vkCreateBufferDelegate>(nameof(vkCreateBuffer));

        private delegate void vkDestroyBufferDelegate(IntPtr device, long buffer, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyBufferDelegate vkDestroyBuffer = VulkanLibrary.GetStaticProc<vkDestroyBufferDelegate>(nameof(vkDestroyBuffer));

        private delegate Result vkBindBufferMemoryDelegate(IntPtr device, long buffer, long memory, long memoryOffset);
        private static readonly vkBindBufferMemoryDelegate vkBindBufferMemory = VulkanLibrary.GetStaticProc<vkBindBufferMemoryDelegate>(nameof(vkBindBufferMemory));

        private delegate void vkGetBufferMemoryRequirementsDelegate(IntPtr device, long buffer, MemoryRequirements* memoryRequirements);
        private static readonly vkGetBufferMemoryRequirementsDelegate vkGetBufferMemoryRequirements = VulkanLibrary.GetStaticProc<vkGetBufferMemoryRequirementsDelegate>(nameof(vkGetBufferMemoryRequirements));
    }

    /// <summary>
    /// Structure specifying the parameters of a newly created buffer object.
    /// </summary>
    public unsafe struct BufferCreateInfo
    {
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bitmask specifying additional parameters of the buffer.
        /// </summary>
        public BufferCreateFlags Flags;
        /// <summary>
        /// The size in bytes of the buffer to be created.
        /// </summary>
        public long Size;
        /// <summary>
        /// A bitmask specifying allowed usages of the buffer.
        /// </summary>
        public BufferUsages Usage;
        /// <summary>
        /// The sharing mode of the buffer when it will be accessed by multiple queue families.
        /// </summary>
        public SharingMode SharingMode;
        /// <summary>
        /// A list of queue families that will access this buffer (ignored if <see
        /// cref="SharingMode"/> is not <see cref="VulkanCore.SharingMode.Concurrent"/>).
        /// </summary>
        public int[] QueueFamilyIndices;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferCreateInfo"/> structure.
        /// </summary>
        /// <param name="size">The size in bytes of the buffer to be created.</param>
        /// <param name="usages">The bitmask specifying allowed usages of the buffer.</param>
        /// <param name="flags">A bitmask specifying additional parameters of the buffer.</param>
        /// <param name="sharingMode">
        /// The sharing mode of the buffer when it will be accessed by multiple queue families.
        /// </param>
        /// <param name="queueFamilyIndices">
        /// A list of queue families that will access this buffer (ignored if <see
        /// cref="SharingMode"/> is not <see cref="VulkanCore.SharingMode.Concurrent"/>).
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public BufferCreateInfo(
            long size,
            BufferUsages usages,
            BufferCreateFlags flags = 0,
            SharingMode sharingMode = SharingMode.Exclusive,
            int[] queueFamilyIndices = null,
            IntPtr next = default(IntPtr))
        {
            Next = next;
            Size = size;
            Usage = usages;
            Flags = flags;
            SharingMode = sharingMode;
            QueueFamilyIndices = queueFamilyIndices;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public BufferCreateFlags Flags;
            public long Size;
            public BufferUsages Usage;
            public SharingMode SharingMode;
            public int QueueFamilyIndexCount;
            public int* QueueFamilyIndices;
        }

        internal void ToNative(out Native native, int* queueFamilyIndices)
        {
            native.Type = StructureType.BufferCreateInfo;
            native.Next = Next;
            native.Flags = Flags;
            native.Size = Size;
            native.Usage = Usage;
            native.SharingMode = SharingMode;
            native.QueueFamilyIndexCount = QueueFamilyIndices?.Length ?? 0;
            native.QueueFamilyIndices = queueFamilyIndices;
        }
    }

    /// <summary>
    /// Bitmask specifying allowed usages of a buffer.
    /// </summary>
    [Flags]
    public enum BufferUsages
    {
        /// <summary>
        /// Specifies that the buffer can be used as the source of a transfer command. (see the
        /// definition of <see cref="PipelineStages.Transfer"/>).
        /// </summary>
        TransferSrc = 1 << 0,
        /// <summary>
        /// Specifies that the buffer can be used as the destination of a transfer command.
        /// </summary>
        TransferDst = 1 << 1,
        /// <summary>
        /// Specifies that the buffer can be used to create a <see cref="BufferView"/> suitable for
        /// occupying a <see cref="DescriptorSet"/> slot of type <see cref="DescriptorType.UniformTexelBuffer"/>.
        /// </summary>
        UniformTexelBuffer = 1 << 2,
        /// <summary>
        /// Specifies that the buffer can be used to create a <see cref="BufferView"/> suitable for
        /// occupying a <see cref="DescriptorSet"/> slot of type <see cref="DescriptorType.StorageTexelBuffer"/>.
        /// </summary>
        StorageTexelBuffer = 1 << 3,
        /// <summary>
        /// Specifies that the buffer can be used in a <see cref="DescriptorBufferInfo"/> suitable
        /// for occupying a <see cref="DescriptorSet"/> slot either of type <see
        /// cref="DescriptorType.UniformBuffer"/> or <see cref="DescriptorType.UniformBufferDynamic"/>.
        /// </summary>
        UniformBuffer = 1 << 4,
        /// <summary>
        /// Specifies that the buffer can be used in a <see cref="DescriptorBufferInfo"/> suitable
        /// for occupying a <see cref="DescriptorSet"/> slot either of type <see
        /// cref="DescriptorType.StorageBuffer"/> or <see cref="DescriptorType.StorageBufferDynamic"/>.
        /// </summary>
        StorageBuffer = 1 << 5,
        /// <summary>
        /// Specifies that the buffer is suitable for passing as the buffer parameter to <see cref="CommandBuffer.CmdBindIndexBuffer"/>.
        /// </summary>
        IndexBuffer = 1 << 6,
        /// <summary>
        /// Specifies that the buffer is suitable for passing as an element of the pBuffers array to
        /// <see cref="CommandBuffer.CmdBindVertexBuffers"/>.
        /// </summary>
        VertexBuffer = 1 << 7,
        /// <summary>
        /// Specifies that the buffer is suitable for passing as the buffer parameter to <see
        /// cref="CommandBuffer.CmdDrawIndirect"/>, <see
        /// cref="CommandBuffer.CmdDrawIndexedIndirect"/>, or <see
        /// cref="CommandBuffer.CmdDispatchIndirect"/>. It is also suitable for passing as the <see
        /// cref="Nvx.IndirectCommandsTokenNvx.Buffer"/> member, or <see
        /// cref="Nvx.CmdProcessCommandsInfoNvx.SequencesCountBuffer"/> or <see
        /// cref="Nvx.CmdProcessCommandsInfoNvx.SequencesIndexBuffer"/> member.
        /// </summary>
        IndirectBuffer = 1 << 8
    }

    /// <summary>
    /// Bitmask specifying additional parameters of a buffer.
    /// </summary>
    [Flags]
    public enum BufferCreateFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that the buffer will be backed using sparse memory binding.
        /// </summary>
        SparseBinding = 1 << 0,
        /// <summary>
        /// Specifies that the buffer can be partially backed using sparse memory binding. Buffers
        /// created with this flag must also be created with the <see cref="SparseBinding"/> flag.
        /// </summary>
        SparseResidency = 1 << 1,
        /// <summary>
        /// Specifies that the buffer will be backed using sparse memory binding with memory ranges
        /// that might also simultaneously be backing another buffer (or another portion of the same
        /// buffer). Buffers created with this flag must also be created with the <see
        /// cref="SparseBinding"/> flag.
        /// </summary>
        SparseAliased = 1 << 2
    }

    /// <summary>
    /// Structure specifying memory requirements.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryRequirements
    {
        /// <summary>
        /// The size, in bytes, of the memory allocation required for the resource.
        /// </summary>
        public long Size;
        /// <summary>
        /// The alignment, in bytes, of the offset within the allocation required for the resource.
        /// </summary>
        public long Alignment;
        /// <summary>
        /// A bitmask that contains one bit set for every supported memory type for the resource. Bit
        /// `i` is set if and only if the memory type `i` in the <see
        /// cref="PhysicalDeviceMemoryProperties"/> structure for the physical device is supported
        /// for the resource.
        /// </summary>
        public int MemoryTypeBits;
    }

    /// <summary>
    /// Buffer and image sharing modes.
    /// </summary>
    public enum SharingMode
    {
        /// <summary>
        /// Specifies that access to any range or image subresource of the object will be
        /// exclusive to a single queue family at a time.
        /// </summary>
        Exclusive = 0,
        /// <summary>
        /// Specifies that concurrent access to any range or image subresource of the
        /// object from multiple queue families is supported.
        /// </summary>
        Concurrent = 1
    }
}
