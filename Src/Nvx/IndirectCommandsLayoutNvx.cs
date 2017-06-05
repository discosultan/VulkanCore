using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Nvx
{
    /// <summary>
    /// Opaque handle to an indirect commands layout object.
    /// <para>
    /// The device-side command generation happens through an iterative processing of an atomic
    /// sequence comprised of command tokens, which are represented by <see cref="IndirectCommandsLayoutNvx"/>.
    /// </para>
    /// </summary>
    public unsafe class IndirectCommandsLayoutNvx : DisposableHandle<long>
    {
        internal IndirectCommandsLayoutNvx(Device parent, ref IndirectCommandsLayoutCreateInfoNvx createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            fixed (IndirectCommandsLayoutTokenNvx* tokensPtr = createInfo.Tokens)
            {
                createInfo.ToNative(out IndirectCommandsLayoutCreateInfoNvx.Native nativeCreateInfo, tokensPtr);
                long handle;
                Result result = vkCreateIndirectCommandsLayoutNVX(this)(Parent, &nativeCreateInfo, NativeAllocator, &handle);
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        /// <summary>
        /// Gets the parent of this resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Destroy an indirect commands layout object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyIndirectCommandsLayoutNVX(this)(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateIndirectCommandsLayoutNVXDelegate(IntPtr device, IndirectCommandsLayoutCreateInfoNvx.Native* createInfo, AllocationCallbacks.Native* allocator, long* indirectCommandsLayout);
        private static vkCreateIndirectCommandsLayoutNVXDelegate vkCreateIndirectCommandsLayoutNVX(IndirectCommandsLayoutNvx indirectCommandsLayout) => GetProc<vkCreateIndirectCommandsLayoutNVXDelegate>(indirectCommandsLayout, nameof(vkCreateIndirectCommandsLayoutNVX));

        private delegate void vkDestroyIndirectCommandsLayoutNVXDelegate(IntPtr device, long indirectCommandsLayout, AllocationCallbacks.Native* allocator);
        private static vkDestroyIndirectCommandsLayoutNVXDelegate vkDestroyIndirectCommandsLayoutNVX(IndirectCommandsLayoutNvx indirectCommandsLayout) => GetProc<vkDestroyIndirectCommandsLayoutNVXDelegate>(indirectCommandsLayout, nameof(vkDestroyIndirectCommandsLayoutNVX));

        private static TDelegate GetProc<TDelegate>(IndirectCommandsLayoutNvx indirectCommandsLayout, string name) where TDelegate : class => indirectCommandsLayout.Parent.GetProc<TDelegate>(name);
    }

    /// <summary>
    /// Structure specifying the parameters of a newly created indirect commands layout object.
    /// </summary>
    public unsafe struct IndirectCommandsLayoutCreateInfoNvx
    {
        /// <summary>
        /// The <see cref="VulkanCore.PipelineBindPoint"/> that this layout targets.
        /// </summary>
        public PipelineBindPoint PipelineBindPoint;
        /// <summary>
        /// A bitmask providing usage hints of this layout.
        /// </summary>
        public IndirectCommandsLayoutUsagesNvx Flags;
        /// <summary>
        /// An array describing each command token in detail. See <see
        /// cref="IndirectCommandsTokenTypeNvx"/> and <see cref="IndirectCommandsLayoutTokenNvx"/>
        /// for details.
        /// </summary>
        public IndirectCommandsLayoutTokenNvx[] Tokens;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndirectCommandsLayoutCreateInfoNvx"/> structure.
        /// </summary>
        /// <param name="pipelineBindPoint">
        /// The <see cref="VulkanCore.PipelineBindPoint"/> that this layout targets.
        /// </param>
        /// <param name="flags">A bitmask providing usage hints of this layout.</param>
        /// <param name="tokens">
        /// An array describing each command token in detail. See <see
        /// cref="IndirectCommandsTokenTypeNvx"/> and <see cref="IndirectCommandsLayoutTokenNvx"/>
        /// for details.
        /// </param>
        public IndirectCommandsLayoutCreateInfoNvx(PipelineBindPoint pipelineBindPoint, IndirectCommandsLayoutUsagesNvx flags,
            params IndirectCommandsLayoutTokenNvx[] tokens)
        {
            PipelineBindPoint = pipelineBindPoint;
            Flags = flags;
            Tokens = tokens;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PipelineBindPoint PipelineBindPoint;
            public IndirectCommandsLayoutUsagesNvx Flags;
            public int TokenCount;
            public IndirectCommandsLayoutTokenNvx* Tokens;
        }

        internal void ToNative(out Native native, IndirectCommandsLayoutTokenNvx* tokens)
        {
            native.Type = StructureType.IndirectCommandsLayoutCreateInfoNvx;
            native.Next = IntPtr.Zero;
            native.PipelineBindPoint = PipelineBindPoint;
            native.Flags = Flags;
            native.TokenCount = Tokens?.Length ?? 0;
            native.Tokens = tokens;
        }
    }

    /// <summary>
    /// Structure specifying the details of an indirect command layout token.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IndirectCommandsLayoutTokenNvx
    {
        /// <summary>
        /// Specifies the token command type.
        /// </summary>
        public IndirectCommandsTokenTypeNvx TokenType;
        /// <summary>
        /// Has a different meaning depending on the type.
        /// </summary>
        public int BindingUnit;
        /// <summary>
        /// Has a different meaning depending on the type.
        /// </summary>
        public int DynamicCount;
        /// <summary>
        /// Defines the rate at which the input data buffers are accessed.
        /// </summary>
        public int Divisor;
    }

    /// <summary>
    /// Enum specifying the token command type.
    /// </summary>
    public enum IndirectCommandsTokenTypeNvx
    {
        /// <summary>
        /// Equivalent command: <see cref="CommandBuffer.CmdBindPipeline"/>.
        /// </summary>
        Pipeline = 0,
        /// <summary>
        /// Equivalent command: <see cref="CommandBuffer.CmdBindDescriptorSets"/>.
        /// </summary>
        DescriptorSet = 1,
        /// <summary>
        /// Equivalent command: <see cref="CommandBuffer.CmdBindIndexBuffer"/>.
        /// </summary>
        IndexBuffer = 2,
        /// <summary>
        /// Equivalent command: <see cref="CommandBuffer.CmdBindVertexBuffers"/>.
        /// </summary>
        VertexBuffer = 3,
        /// <summary>
        /// Equivalent command: <see cref="CommandBuffer.CmdPushConstants"/>.
        /// </summary>
        PushConstant = 4,
        /// <summary>
        /// Equivalent command: <see cref="CommandBuffer.CmdDrawIndexedIndirect"/>.
        /// </summary>
        DrawIndexed = 5,
        /// <summary>
        /// Equivalent command: <see cref="CommandBuffer.CmdDrawIndirect"/>.
        /// </summary>
        Draw = 6,
        /// <summary>
        /// Equivalent command: <see cref="CommandBuffer.CmdDispatchIndirect"/>.
        /// </summary>
        Dispatch = 7
    }

    /// <summary>
    /// Bitmask specifying allowed usage of a indirect commands layout.
    /// </summary>
    [Flags]
    public enum IndirectCommandsLayoutUsagesNvx
    {
        /// <summary>
        /// Indicates that the processing of sequences can happen at an implementation-dependent
        /// order, which is not guaranteed to be coherent across multiple invocations.
        /// </summary>
        UnorderedSequences = 1 << 0,
        /// <summary>
        /// Indicates that there is likely a high difference between allocated number of sequences
        /// and actually used.
        /// </summary>
        SparseSequences = 1 << 1,
        /// <summary>
        /// Indicates that there are likely many draw or dispatch calls that are zero-sized (zero grid
        /// dimension, no primitives to render).
        /// </summary>
        EmptyExecutions = 1 << 2,
        /// <summary>
        /// Indicates that the input data for the sequences is not implicitly indexed from
        /// 0..sequences used but a user provided <see cref="Buffer"/> encoding the index is provided.
        /// </summary>
        IndexedSequences = 1 << 3
    }
}
