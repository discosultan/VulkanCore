using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constants;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a pipeline layout object.
    /// <para>
    /// Access to descriptor sets from a pipeline is accomplished through a pipeline layout. Zero or
    /// more descriptor set layouts and zero or more push constant ranges are combined to form a
    /// pipeline layout object which describes the complete set of resources that can be accessed by
    /// a pipeline. The pipeline layout represents a sequence of descriptor sets with each having a
    /// specific layout. This sequence of layouts is used to determine the interface between shader
    /// stages and shader resources. Each pipeline is created using a pipeline layout.
    /// </para>
    /// </summary>
    public unsafe class PipelineLayout : DisposableHandle<long>
    {
        internal PipelineLayout(Device parent, ref PipelineLayoutCreateInfo createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            int setLayoutCount = createInfo.SetLayouts?.Length ?? 0;
            var layoutsPtr = stackalloc long[setLayoutCount];
            for (int i = 0; i < setLayoutCount; i++)
                layoutsPtr[i] = createInfo.SetLayouts[i];

            fixed (PushConstantRange* pushConstantRangesPtr = createInfo.PushConstantRanges)
            {                
                createInfo.ToNative(out PipelineLayoutCreateInfo.Native nativeCreateInfo, 
                    layoutsPtr,
                    pushConstantRangesPtr);

                long handle;
                Result result = CreatePipelineLayout(Parent, &nativeCreateInfo, NativeAllocator, &handle);
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        protected override void DisposeManaged()
        {
            DestroyPipelineLayout(Parent, this, NativeAllocator);
            base.DisposeManaged();
        }
        
        [DllImport(VulkanDll, EntryPoint = "vkCreatePipelineLayout", CallingConvention = CallConv)]
        private static extern Result CreatePipelineLayout(IntPtr device, 
            PipelineLayoutCreateInfo.Native* createInfo, AllocationCallbacks.Native* allocator, long* pipelineLayout);

        [DllImport(VulkanDll, EntryPoint = "vkDestroyPipelineLayout", CallingConvention = CallConv)]
        private static extern void DestroyPipelineLayout(IntPtr device, long pipelineLayout, AllocationCallbacks.Native* allocator);
    }

    /// <summary>
    /// Structure specifying the parameters of a newly created pipeline layout object.
    /// </summary>
    public unsafe struct PipelineLayoutCreateInfo
    {
        /// <summary>
        /// An array of <see cref="DescriptorSetLayout"/> objects.
        /// </summary>
        public long[] SetLayouts;
        /// <summary>
        /// Structures defining a set of push constant ranges for use in a single pipeline layout. In
        /// addition to descriptor set layouts, a pipeline layout also describes how many push
        /// constants can be accessed by each stage of the pipeline. Push constants represent a high
        /// speed path to modify constant data in pipelines that is expected to outperform
        /// memory-backed resource updates.
        /// </summary>
        public PushConstantRange[] PushConstantRanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineLayoutCreateInfo"/> structure.
        /// </summary>
        /// <param name="setLayouts">An array of <see cref="DescriptorSetLayout"/> objects.</param>
        /// <param name="pushConstantRanges">
        /// Structures defining a set of push constant ranges for use in a single pipeline layout. In
        /// addition to descriptor set layouts, a pipeline layout also describes how many push
        /// constants can be accessed by each stage of the pipeline. Push constants represent a high
        /// speed path to modify constant data in pipelines that is expected to outperform
        /// memory-backed resource updates.
        /// </param>
        public PipelineLayoutCreateInfo(
            DescriptorSetLayout[] setLayouts = null, 
            PushConstantRange[] pushConstantRanges = null)
        {
            SetLayouts = setLayouts?.ToHandleArray();
            PushConstantRanges = pushConstantRanges;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PipelineLayoutCreateFlags Flags;
            public int SetLayoutCount;
            public long* SetLayouts;
            public int PushConstantRangeCount;
            public PushConstantRange* PushConstantRanges;
        }

        internal void ToNative(out Native native, long* setLayouts, PushConstantRange* pushConstantRanges)
        {
            native.Type = StructureType.PipelineLayoutCreateInfo;
            native.Next = IntPtr.Zero;
            native.Flags = 0;
            native.SetLayoutCount = SetLayouts?.Length ?? 0;
            native.SetLayouts = setLayouts;
            native.PushConstantRangeCount = PushConstantRanges?.Length ?? 0;
            native.PushConstantRanges = pushConstantRanges;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum PipelineLayoutCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying a push constant range.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PushConstantRange
    {
        /// <summary>
        /// A set of stage flags describing the shader stages that will access a range of push
        /// constants. If a particular stage is not included in the range, then accessing members of
        /// that range of push constants from the corresponding shader stage will result in undefined
        /// data being read.
        /// </summary>
        public ShaderStages StageFlags;
        /// <summary>
        /// The start offset, consumed by the range. Offset is in units of bytes and must be a
        /// multiple of 4. The layout of the push constant variables is specified in the shader.
        /// </summary>
        public int Offset;
        /// <summary>
        /// The size, consumed by the range. Size is in units of bytes and must be a multiple of 4.
        /// The layout of the push constant variables is specified in the shader.
        /// <para>
        /// Must be less than or equal to <see cref="PhysicalDeviceLimits.MaxPushConstantsSize"/>
        /// minus offset.
        /// </para>
        /// </summary>
        public int Size;
    }
}
