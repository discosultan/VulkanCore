using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a pipeline object.
    /// </summary>
    public unsafe class Pipeline : DisposableHandle<long>
    {
        internal Pipeline(Device parent, PipelineCache cache, ref GraphicsPipelineCreateInfo createInfo,
            ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Cache = cache;
            Allocator = allocator;

            GraphicsPipelineCreateInfo.Native nativeCreateInfo;
            createInfo.ToNative(&nativeCreateInfo);
            long handle;
            Result result = vkCreateGraphicsPipelines(Parent, Cache, 1, &nativeCreateInfo, NativeAllocator, &handle);
            nativeCreateInfo.Free();
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        internal Pipeline(Device parent, PipelineCache cache, ref ComputePipelineCreateInfo createInfo,
            ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Cache = cache;
            Allocator = allocator;

            ComputePipelineCreateInfo.Native nativeCreateInfo;
            createInfo.ToNative(&nativeCreateInfo);
            long handle;
            Result result = vkCreateComputePipelines(Parent, Cache, 1, &nativeCreateInfo, NativeAllocator, &handle);
            nativeCreateInfo.Free();
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        internal Pipeline(Device parent, PipelineCache cache, long handle, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Cache = cache;
            Handle = handle;
            Allocator = allocator;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Gets the cache of the resource.
        /// </summary>
        public PipelineCache Cache { get; }

        /// <summary>
        /// Destroy a pipeline object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyPipeline(Parent, this, NativeAllocator);
            base.Dispose();
        }

        internal static Pipeline[] CreateGraphicsPipelines(Device parent, PipelineCache cache,
            GraphicsPipelineCreateInfo[] createInfos, ref AllocationCallbacks? allocator)
        {
            int pipelineCount = createInfos?.Length ?? 0;
            var nativeCreateInfos = stackalloc GraphicsPipelineCreateInfo.Native[pipelineCount];
            for (int i = 0; i < pipelineCount; i++)
                createInfos[i].ToNative(&nativeCreateInfos[i]);

            AllocationCallbacks.Native nativeAllocator;
            allocator?.ToNative(&nativeAllocator);

            var pipelineHandles = stackalloc long[pipelineCount];
            Result result = vkCreateGraphicsPipelines(
                parent,
                cache,
                pipelineCount,
                nativeCreateInfos,
                allocator.HasValue ? &nativeAllocator : null,
                pipelineHandles);
            for (int i = 0; i < pipelineCount; i++)
                nativeCreateInfos[i].Free();

            VulkanException.ThrowForInvalidResult(result);

            var pipelines = new Pipeline[pipelineCount];
            for (int i = 0; i < pipelineCount; i++)
                pipelines[i] = new Pipeline(parent, cache, pipelineHandles[i], ref allocator);
            return pipelines;
        }

        internal static Pipeline[] CreateComputePipelines(Device parent, PipelineCache cache,
            ComputePipelineCreateInfo[] createInfos, ref AllocationCallbacks? allocator)
        {
            int pipelineCount = createInfos?.Length ?? 0;
            var nativeCreateInfos = stackalloc ComputePipelineCreateInfo.Native[pipelineCount];
            for (int i = 0; i < pipelineCount; i++)
                createInfos[i].ToNative(&nativeCreateInfos[i]);

            AllocationCallbacks.Native nativeAllocator;
            allocator?.ToNative(&nativeAllocator);

            var pipelineHandles = stackalloc long[pipelineCount];
            Result result = vkCreateComputePipelines(
                parent,
                cache,
                pipelineCount,
                nativeCreateInfos,
                allocator.HasValue ? &nativeAllocator : null,
                pipelineHandles);
            for (int i = 0; i < pipelineCount; i++)
                nativeCreateInfos[i].Free();

            VulkanException.ThrowForInvalidResult(result);

            var pipelines = new Pipeline[pipelineCount];
            for (int i = 0; i < pipelineCount; i++)
                pipelines[i] = new Pipeline(parent, cache, pipelineHandles[i], ref allocator);
            return pipelines;
        }

        private delegate Result vkCreateGraphicsPipelinesDelegate(IntPtr device, long pipelineCache, int createInfoCount, GraphicsPipelineCreateInfo.Native* createInfos, AllocationCallbacks.Native* allocator, long* pipelines);
        private static readonly vkCreateGraphicsPipelinesDelegate vkCreateGraphicsPipelines = VulkanLibrary.GetStaticProc<vkCreateGraphicsPipelinesDelegate>(nameof(vkCreateGraphicsPipelines));

        private delegate Result vkCreateComputePipelinesDelegate(IntPtr device, long pipelineCache, int createInfoCount, ComputePipelineCreateInfo.Native* createInfos, AllocationCallbacks.Native* allocator, long* pipelines);
        private static readonly vkCreateComputePipelinesDelegate vkCreateComputePipelines = VulkanLibrary.GetStaticProc<vkCreateComputePipelinesDelegate>(nameof(vkCreateComputePipelines));

        private delegate void vkDestroyPipelineDelegate(IntPtr device, long pipeline, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyPipelineDelegate vkDestroyPipeline = VulkanLibrary.GetStaticProc<vkDestroyPipelineDelegate>(nameof(vkDestroyPipeline));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created graphics pipeline.
    /// </summary>
    public unsafe struct GraphicsPipelineCreateInfo
    {
        /// <summary>
        /// A bitmask specifying how the pipeline will be generated.
        /// </summary>
        public PipelineCreateFlags Flags;
        /// <summary>
        /// Structures describing the set of the shader stages to be included in the graphics pipeline.
        /// </summary>
        public PipelineShaderStageCreateInfo[] Stages;
        /// <summary>
        /// An instance of the <see cref="PipelineVertexInputStateCreateInfo"/> structure.
        /// </summary>
        public PipelineVertexInputStateCreateInfo VertexInputState;
        /// <summary>
        /// The structure which determines input assembly behavior.
        /// </summary>
        public PipelineInputAssemblyStateCreateInfo InputAssemblyState;
        /// <summary>
        /// An instance of the <see cref="PipelineTessellationStateCreateInfo"/> structure, and
        /// is ignored if the pipeline does not include a tessellation control shader stage and
        /// tessellation evaluation shader stage.
        /// </summary>
        public PipelineTessellationStateCreateInfo? TessellationState;
        /// <summary>
        /// An instance of the <see cref="PipelineViewportStateCreateInfo"/> structure, and
        /// is ignored if the pipeline has rasterization disabled.
        /// </summary>
        public PipelineViewportStateCreateInfo? ViewportState;
        /// <summary>
        /// An instance of the <see cref="PipelineRasterizationStateCreateInfo"/> structure.
        /// </summary>
        public PipelineRasterizationStateCreateInfo RasterizationState;
        /// <summary>
        /// An instance of the <see cref="PipelineMultisampleStateCreateInfo"/>, and is ignored if
        /// the pipeline has rasterization disabled.
        /// </summary>
        public PipelineMultisampleStateCreateInfo? MultisampleState;
        /// <summary>
        /// An instance of the <see cref="PipelineDepthStencilStateCreateInfo"/> structure, and
        /// is ignored if the pipeline has rasterization disabled or if the subpass of the render
        /// pass the pipeline is created against does not use a depth/stencil attachment.
        /// </summary>
        public PipelineDepthStencilStateCreateInfo? DepthStencilState;
        /// <summary>
        /// An instance of the <see cref="PipelineColorBlendStateCreateInfo"/> structure, and
        /// is ignored if the pipeline has rasterization disabled or if the subpass of the render
        /// pass the pipeline is created against does not use any color attachments.
        /// </summary>
        public PipelineColorBlendStateCreateInfo? ColorBlendState;
        /// <summary>
        /// Is used to indicate which properties of the pipeline state object are dynamic and can be
        /// changed independently of the pipeline state. This can be <c>null</c>, which means no
        /// state in the pipeline is considered dynamic.
        /// </summary>
        public PipelineDynamicStateCreateInfo? DynamicState;
        /// <summary>
        /// The description of binding locations used by both the pipeline and descriptor sets used
        /// with the pipeline.
        /// </summary>
        public long Layout;
        /// <summary>
        /// A <see cref="RenderPass"/> object describing the environment in which the pipeline will
        /// be used; the pipeline must only be used with an instance of any render pass compatible
        /// with the one provided.
        /// </summary>
        public long RenderPass;
        /// <summary>
        /// The index of the subpass in the render pass where this pipeline will be used.
        /// </summary>
        public int Subpass;
        /// <summary>
        /// A pipeline to derive from.
        /// </summary>
        public Pipeline BasePipelineHandle;
        /// <summary>
        /// An index into the <see cref="Pipeline.CreateGraphicsPipelines"/> create infos parameter
        /// to use as a pipeline to derive from.
        /// </summary>
        public int BasePipelineIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPipelineCreateInfo"/> structure.
        /// </summary>
        /// <param name="layout">
        /// The description of binding locations used by both the pipeline and descriptor sets used
        /// with the pipeline.
        /// </param>
        /// <param name="renderPass">
        /// A <see cref="RenderPass"/> object describing the environment in which the pipeline will
        /// be used; the pipeline must only be used with an instance of any render pass compatible
        /// with the one provided.
        /// </param>
        /// <param name="subpass">
        /// The index of the subpass in the render pass where this pipeline will be used.
        /// </param>
        /// <param name="stages">
        /// Structures describing the set of the shader stages to be included in the graphics pipeline.
        /// </param>
        /// <param name="inputAssemblyState">The structure which determines input assembly behavior.</param>
        /// <param name="vertexInputState">
        /// An instance of the <see cref="PipelineVertexInputStateCreateInfo"/> structure.
        /// </param>
        /// <param name="rasterizationState">
        /// An instance of the <see cref="PipelineRasterizationStateCreateInfo"/> structure.
        /// </param>
        /// <param name="tessellationState">
        /// An instance of the <see cref="PipelineTessellationStateCreateInfo"/> structure, or
        /// <c>null</c> if the pipeline does not include a tessellation control shader stage and
        /// tessellation evaluation shader stage.
        /// </param>
        /// <param name="viewportState">
        /// An instance of the <see cref="PipelineViewportStateCreateInfo"/> structure, or
        /// <c>null</c> if the pipeline has rasterization disabled.
        /// </param>
        /// <param name="multisampleState">
        /// An instance of the <see cref="PipelineMultisampleStateCreateInfo"/>, or <c>null</c> if
        /// the pipeline has rasterization disabled.
        /// </param>
        /// <param name="depthStencilState">
        /// An instance of the <see cref="PipelineDepthStencilStateCreateInfo"/> structure, or
        /// <c>null</c> if the pipeline has rasterization disabled or if the subpass of the render
        /// pass the pipeline is created against does not use a depth/stencil attachment.
        /// </param>
        /// <param name="colorBlendState">
        /// An instance of the <see cref="PipelineColorBlendStateCreateInfo"/> structure, or
        /// <c>null</c> if the pipeline has rasterization disabled or if the subpass of the render
        /// pass the pipeline is created against does not use any color attachments.
        /// </param>
        /// <param name="dynamicState">
        /// Is used to indicate which properties of the pipeline state object are dynamic and can be
        /// changed independently of the pipeline state. This can be <c>null</c>, which means no
        /// state in the pipeline is considered dynamic.
        /// </param>
        /// <param name="flags">
        /// A bitmask of <see cref="PipelineCreateFlags"/> controlling how the pipeline will be generated.
        /// </param>
        /// <param name="basePipelineHandle">A pipeline to derive from.</param>
        /// <param name="basePipelineIndex">
        /// An index into the <see cref="Pipeline.CreateGraphicsPipelines"/> create infos parameter
        /// to use as a pipeline to derive from.
        /// </param>
        public GraphicsPipelineCreateInfo(PipelineLayout layout, RenderPass renderPass, int subpass,
            PipelineShaderStageCreateInfo[] stages, PipelineInputAssemblyStateCreateInfo inputAssemblyState,
            PipelineVertexInputStateCreateInfo vertexInputState, PipelineRasterizationStateCreateInfo rasterizationState,
            PipelineTessellationStateCreateInfo? tessellationState = null, PipelineViewportStateCreateInfo? viewportState = null,
            PipelineMultisampleStateCreateInfo? multisampleState = null, PipelineDepthStencilStateCreateInfo? depthStencilState = null,
            PipelineColorBlendStateCreateInfo? colorBlendState = null, PipelineDynamicStateCreateInfo? dynamicState = null,
            PipelineCreateFlags flags = 0, Pipeline basePipelineHandle = null, int basePipelineIndex = -1)
        {
            Flags = flags;
            Stages = stages;
            VertexInputState = vertexInputState;
            InputAssemblyState = inputAssemblyState;
            TessellationState = tessellationState;
            ViewportState = viewportState;
            RasterizationState = rasterizationState;
            MultisampleState = multisampleState;
            DepthStencilState = depthStencilState;
            ColorBlendState = colorBlendState;
            DynamicState = dynamicState;
            Layout = layout;
            RenderPass = renderPass;
            Subpass = subpass;
            BasePipelineHandle = basePipelineHandle;
            BasePipelineIndex = basePipelineIndex;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PipelineCreateFlags Flags;
            public int StageCount;
            public PipelineShaderStageCreateInfo.Native* Stages;
            public PipelineVertexInputStateCreateInfo.Native* VertexInputState;
            public IntPtr InputAssemblyState;
            public PipelineTessellationStateCreateInfo* TessellationState;
            public PipelineViewportStateCreateInfo.Native* ViewportState;
            public IntPtr RasterizationState;
            public PipelineMultisampleStateCreateInfo.Native* MultisampleState;
            public PipelineDepthStencilStateCreateInfo* DepthStencilState;
            public PipelineColorBlendStateCreateInfo.Native* ColorBlendState;
            public PipelineDynamicStateCreateInfo.Native* DynamicState;
            public long Layout;
            public long RenderPass;
            public int Subpass;
            public long BasePipelineHandle;
            public int BasePipelineIndex;

            public void Free()
            {
                for (int i = 0; i < StageCount; i++)
                    Stages[i].Free();
                Interop.Free(Stages);
                VertexInputState->Free();
                Interop.Free(VertexInputState);
                Interop.Free(InputAssemblyState);
                Interop.Free(TessellationState);
                if (ViewportState != null)
                {
                    ViewportState->Free();
                    Interop.Free(ViewportState);
                }
                Interop.Free(RasterizationState);
                if (MultisampleState != null)
                {
                    MultisampleState->Free();
                    Interop.Free(MultisampleState);
                }
                Interop.Free(DepthStencilState);
                if (ColorBlendState != null)
                {
                    ColorBlendState->Free();
                    Interop.Free(ColorBlendState);
                }
                if (DynamicState != null)
                {
                    DynamicState->Free();
                    Interop.Free(DynamicState);
                }
            }
        }

        internal void ToNative(Native* native)
        {
            InputAssemblyState.Prepare();
            RasterizationState.Prepare();

            int stageCount = Stages?.Length ?? 0;
            var stages = (PipelineShaderStageCreateInfo.Native*)Interop.Alloc<PipelineShaderStageCreateInfo.Native>(stageCount);
            for (int i = 0; i < stageCount; i++)
                Stages[i].ToNative(&stages[i]);
            var vertexInputState = (PipelineVertexInputStateCreateInfo.Native*)Interop.Alloc<PipelineVertexInputStateCreateInfo.Native>();
            VertexInputState.ToNative(vertexInputState);
            var tessellationState = (PipelineTessellationStateCreateInfo*)Interop.Struct.AllocToPointer(ref TessellationState);
            if (tessellationState != null) tessellationState->Prepare();
            PipelineViewportStateCreateInfo.Native* viewportState = null;
            if (ViewportState.HasValue)
            {
                viewportState = (PipelineViewportStateCreateInfo.Native*)Interop.Alloc<PipelineViewportStateCreateInfo.Native>();
                ViewportState.Value.ToNative(viewportState);
            }
            PipelineMultisampleStateCreateInfo.Native* multisampleState = null;
            if (MultisampleState.HasValue)
            {
                multisampleState = (PipelineMultisampleStateCreateInfo.Native*)Interop.Alloc<PipelineMultisampleStateCreateInfo.Native>();
                MultisampleState.Value.ToNative(multisampleState);
            }
            var depthStencilState = (PipelineDepthStencilStateCreateInfo*)Interop.Struct.AllocToPointer(ref DepthStencilState);
            if (depthStencilState != null) depthStencilState->Prepare();
            PipelineColorBlendStateCreateInfo.Native* colorBlendState = null;
            if (ColorBlendState.HasValue)
            {
                colorBlendState = (PipelineColorBlendStateCreateInfo.Native*)Interop.Alloc<PipelineColorBlendStateCreateInfo.Native>();
                ColorBlendState.Value.ToNative(colorBlendState);
            }
            PipelineDynamicStateCreateInfo.Native* dynamicState = null;
            if (DynamicState.HasValue)
            {
                dynamicState = (PipelineDynamicStateCreateInfo.Native*)Interop.Alloc<PipelineDynamicStateCreateInfo.Native>();
                DynamicState.Value.ToNative(dynamicState);
            }

            native->Type = StructureType.GraphicsPipelineCreateInfo;
            native->Next = IntPtr.Zero;
            native->Flags = Flags;
            native->StageCount = Stages?.Length ?? 0;
            native->Stages = stages;
            native->VertexInputState = vertexInputState;
            native->InputAssemblyState = Interop.Struct.AllocToPointer(ref InputAssemblyState);
            native->TessellationState = tessellationState;
            native->ViewportState = viewportState;
            native->RasterizationState = Interop.Struct.AllocToPointer(ref RasterizationState);
            native->MultisampleState = multisampleState;
            native->DepthStencilState = depthStencilState;
            native->ColorBlendState = colorBlendState;
            native->DynamicState = dynamicState;
            native->Layout = Layout;
            native->RenderPass = RenderPass;
            native->Subpass = Subpass;
            native->BasePipelineHandle = BasePipelineHandle;
            native->BasePipelineIndex = BasePipelineIndex;
        }
    }

    /// <summary>
    /// Bitmask controlling how a pipeline is created.
    /// </summary>
    [Flags]
    public enum PipelineCreateFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that the created pipeline will not be optimized. Using this flag may reduce the
        /// time taken to create the pipeline.
        /// </summary>
        DisableOptimization = 1 << 0,
        /// <summary>
        /// Specifies that the pipeline to be created is allowed to be the parent of a pipeline that
        /// will be created in a subsequent call to <see cref="Device.CreateGraphicsPipelines"/> or
        /// <see cref="Device.CreateComputePipelines"/>.
        /// </summary>
        AllowDerivatives = 1 << 1,
        /// <summary>
        /// Specifies that the pipeline to be created will be a child of a previously created parent pipeline.
        /// </summary>
        Derivative = 1 << 2,
        /// <summary>
        /// Specifies that any shader input variables decorated as device index will be
        /// assigned values as if they were decorated as view index.
        /// </summary>
        ViewIndexFromDeviceIndexKhx = 1 << 3,
        /// <summary>
        /// Specifies that a compute pipeline can be used with <see
        /// cref="Khx.CommandBufferExtensions.CmdDispatchBaseKhx"/> with a non-zero base workgroup.
        /// </summary>
        DispatchBaseKhx = 1 << 4
    }

    /// <summary>
    /// Structure specifying parameters of a newly created compute pipeline.
    /// </summary>
    public unsafe struct ComputePipelineCreateInfo
    {
        /// <summary>
        /// A bitmask specifying options for pipeline creation.
        /// </summary>
        public PipelineCreateFlags Flags;
        /// <summary>
        /// Describes the compute shader.
        /// </summary>
        public PipelineShaderStageCreateInfo Stage;
        /// <summary>
        /// The description of binding locations used by both the pipeline and descriptor sets used
        /// with the pipeline.
        /// <para>Must be consistent with the layout of the compute shader specified in stage.</para>
        /// </summary>
        public long Layout;
        /// <summary>
        /// A pipeline to derive from.
        /// </summary>
        public long BasePipelineHandle;
        /// <summary>
        /// An index into the <see cref="Pipeline.CreateComputePipelines"/> create infos parameter to
        /// use as a pipeline to derive from.
        /// </summary>
        public int BasePipelineIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputePipelineCreateInfo"/> structure.
        /// </summary>
        /// <param name="stage">Describes the compute shader.</param>
        /// <param name="layout">
        /// The description of binding locations used by both the pipeline and descriptor sets used
        /// with the pipeline.
        /// </param>
        /// <param name="flags">A bitmask specifying options for pipeline creation.</param>
        /// <param name="basePipelineHandle">A pipeline to derive from.</param>
        /// <param name="basePipelineIndex">
        /// An index into the <see cref="Pipeline.CreateComputePipelines"/> create infos parameter to
        /// use as a pipeline to derive from.
        /// </param>
        public ComputePipelineCreateInfo(PipelineShaderStageCreateInfo stage, PipelineLayout layout,
            PipelineCreateFlags flags = 0, Pipeline basePipelineHandle = null, int basePipelineIndex = -1)
        {
            Flags = flags;
            Stage = stage;
            Layout = layout;
            BasePipelineHandle = basePipelineHandle;
            BasePipelineIndex = basePipelineIndex;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PipelineCreateFlags Flags;
            public PipelineShaderStageCreateInfo.Native Stage;
            public long Layout;
            public long BasePipelineHandle;
            public int BasePipelineIndex;

            public void Free()
            {
                Stage.Free();
            }
        }

        internal void ToNative(Native* native)
        {
            native->Type = StructureType.ComputePipelineCreateInfo;
            native->Next = IntPtr.Zero;
            native->Flags = Flags;
            Stage.ToNative(&native->Stage);
            native->Layout = Layout;
            native->BasePipelineHandle = BasePipelineHandle;
            native->BasePipelineIndex = BasePipelineIndex;
        }
    }

    /// <summary>
    /// Structure specifying parameters of a newly created pipeline shader stage.
    /// </summary>
    public unsafe struct PipelineShaderStageCreateInfo
    {
        /// <summary>
        /// Specifies a single pipeline stage.
        /// </summary>
        public ShaderStages Stage;
        /// <summary>
        /// A <see cref="ShaderModule"/> object that contains the shader for this stage.
        /// </summary>
        public long Module;
        /// <summary>
        /// Unicode string specifying the entry point name of the shader for this stage.
        /// </summary>
        public string Name;
        /// <summary>
        /// Is <c>null</c> or a structure specifying specialization info.
        /// </summary>
        public SpecializationInfo? SpecializationInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineShaderStageCreateInfo"/> structure.
        /// </summary>
        /// <param name="stage">Specifies a single pipeline stage.</param>
        /// <param name="module">A <see cref="ShaderModule"/> object that contains the shader for this stage.</param>
        /// <param name="name">Unicode string specifying the entry point name of the shader for this stage.</param>
        /// <param name="specializationInfo">
        /// Is <c>null</c> or a structure specifying specialization info.
        /// </param>
        public PipelineShaderStageCreateInfo(ShaderStages stage, ShaderModule module, string name,
            SpecializationInfo? specializationInfo = null)
        {
            Stage = stage;
            Module = module;
            Name = name;
            SpecializationInfo = specializationInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PipelineShaderStageCreateFlags Flags;
            public ShaderStages Stage;
            public long Module;
            public IntPtr Name;
            public SpecializationInfo.Native* SpecializationInfo;

            public void Free()
            {
                Interop.Free(Name);
                if (SpecializationInfo != null)
                {
                    SpecializationInfo->Free();
                    Interop.Free(SpecializationInfo);
                }
            }
        }

        internal void ToNative(Native* native)
        {
            SpecializationInfo.Native* specializationInfo = null;
            if (SpecializationInfo.HasValue)
            {
                specializationInfo = (SpecializationInfo.Native*)Interop.Alloc<SpecializationInfo.Native>();
                SpecializationInfo.Value.ToNative(specializationInfo);
            }

            native->Type = StructureType.PipelineShaderStageCreateInfo;
            native->Next = IntPtr.Zero;
            native->Flags = 0;
            native->Stage = Stage;
            native->Module = Module;
            native->Name = Interop.String.AllocToPointer(Name);
            native->SpecializationInfo = specializationInfo;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum PipelineShaderStageCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Bitmask specifying a pipeline stage.
    /// </summary>
    [Flags]
    public enum ShaderStages
    {
        /// <summary>
        /// Specifies the vertex stage.
        /// </summary>
        Vertex = 1 << 0,
        /// <summary>
        /// Specifies the tessellation control stage.
        /// </summary>
        TessellationControl = 1 << 1,
        /// <summary>
        /// Specifies the tessellation evaluation stage.
        /// </summary>
        TessellationEvaluation = 1 << 2,
        /// <summary>
        /// Specifies the geometry stage.
        /// </summary>
        Geometry = 1 << 3,
        /// <summary>
        /// Specifies the fragment stage.
        /// </summary>
        Fragment = 1 << 4,
        /// <summary>
        /// Specifies the compute stage.
        /// </summary>
        Compute = 1 << 5,
        /// <summary>
        /// Is a combination of bits used as shorthand to specify all graphics stages defined above
        /// (excluding the compute stage).
        /// </summary>
        AllGraphics = 0x0000001F,
        /// <summary>
        /// Is a combination of bits used as shorthand to specify all shader stages supported by the
        /// device, including all additional stages which are introduced by extensions.
        /// </summary>
        All = 0x7FFFFFFF
    }

    /// <summary>
    /// Structure specifying specialization info.
    /// </summary>
    public unsafe struct SpecializationInfo
    {
        /// <summary>
        /// An array of <see cref="SpecializationMapEntry"/> which maps constant ids to offsets in
        /// <see cref="Data"/>.
        /// </summary>
        public SpecializationMapEntry[] MapEntries;
        /// <summary>
        /// The byte size of the <see cref="Data"/> buffer.
        /// </summary>
        public Size DataSize;
        /// <summary>
        /// Contains the actual constant values to specialize with.
        /// </summary>
        public IntPtr Data;

        /// <summary>
        /// Initializes a new instasnce of the <see cref="SpecializationInfo"/> structure.
        /// </summary>
        /// <param name="mapEntries">
        /// An array of <see cref="SpecializationMapEntry"/> which maps constant ids to offsets in
        /// <see cref="Data"/>.
        /// </param>
        /// <param name="dataSize">The byte size of the <see cref="Data"/> buffer.</param>
        /// <param name="data">Contains the actual constant values to specialize with.</param>
        public SpecializationInfo(SpecializationMapEntry[] mapEntries, Size dataSize, IntPtr data)
        {
            MapEntries = mapEntries;
            DataSize = dataSize;
            Data = data;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public int MapEntryCount;
            public IntPtr MapEntries;
            public Size DataSize;
            public IntPtr Data;

            public void Free()
            {
                Interop.Free(MapEntries);
            }
        }

        internal void ToNative(Native* native)
        {
            native->MapEntryCount = MapEntries?.Length ?? 0;
            native->MapEntries = Interop.Struct.AllocToPointer(MapEntries);
            native->DataSize = DataSize;
            native->Data = Data;
        }
    }

    /// <summary>
    /// Structure specifying a specialization map entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SpecializationMapEntry
    {
        /// <summary>
        /// The id of the specialization constant in SPIR-V.
        /// </summary>
        public int ConstantId;
        /// <summary>
        /// The byte offset of the specialization constant value within the supplied data buffer.
        /// </summary>
        public int Offset;
        /// <summary>
        /// The byte size of the specialization constant value within the supplied data buffer.
        /// </summary>
        public int Size;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecializationMapEntry"/> structure.
        /// </summary>
        /// <param name="constantId">The id of the specialization constant in SPIR-V.</param>
        /// <param name="offset">
        /// The byte offset of the specialization constant value within the supplied data buffer.
        /// </param>
        /// <param name="size">
        /// The byte size of the specialization constant value within the supplied data buffer.
        /// </param>
        public SpecializationMapEntry(int constantId, int offset, int size)
        {
            ConstantId = constantId;
            Offset = offset;
            Size = size;
        }
    }

    /// <summary>
    /// Structure specifying parameters of a newly created pipeline vertex input state.
    /// </summary>
    public unsafe struct PipelineVertexInputStateCreateInfo
    {
        /// <summary>
        /// An array of <see cref="VertexInputBindingDescription"/> structures.
        /// </summary>
        public VertexInputBindingDescription[] VertexBindingDescriptions;
        /// <summary>
        /// An array of <see cref="VertexInputAttributeDescription"/> structures.
        /// </summary>
        public VertexInputAttributeDescription[] VertexAttributeDescriptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineVertexInputStateCreateInfo"/> structure.
        /// </summary>
        /// <param name="vertexBindingDescriptions">
        /// An array of <see cref="VertexInputBindingDescription"/> structures.
        /// </param>
        /// <param name="vertexAttributeDescriptions">
        /// An array of <see cref="VertexInputAttributeDescription"/> structures.
        /// </param>
        public PipelineVertexInputStateCreateInfo(
            VertexInputBindingDescription[] vertexBindingDescriptions,
            VertexInputAttributeDescription[] vertexAttributeDescriptions)
        {
            VertexBindingDescriptions = vertexBindingDescriptions;
            VertexAttributeDescriptions = vertexAttributeDescriptions;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PipelineVertexInputStateCreateFlags Flags;
            public int VertexBindingDescriptionCount;
            public IntPtr VertexBindingDescriptions;
            public int VertexAttributeDescriptionCount;
            public IntPtr VertexAttributeDescriptions;

            public void Free()
            {
                Interop.Free(VertexBindingDescriptions);
                Interop.Free(VertexAttributeDescriptions);
            }
        }

        internal void ToNative(Native* native)
        {
            native->Type = StructureType.PipelineVertexInputStateCreateInfo;
            native->Next = IntPtr.Zero;
            native->Flags = 0;
            native->VertexBindingDescriptionCount = VertexBindingDescriptions?.Length ?? 0;
            native->VertexBindingDescriptions = Interop.Struct.AllocToPointer(VertexBindingDescriptions);
            native->VertexAttributeDescriptionCount = VertexAttributeDescriptions?.Length ?? 0;
            native->VertexAttributeDescriptions = Interop.Struct.AllocToPointer(VertexAttributeDescriptions);
        }
    }

    [Flags]
    internal enum PipelineVertexInputStateCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying vertex input binding description.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexInputBindingDescription
    {
        /// <summary>
        /// The binding number that this structure describes.
        /// </summary>
        public int Binding;
        /// <summary>
        /// The distance in bytes between two consecutive elements within the buffer.
        /// </summary>
        public int Stride;
        /// <summary>
        /// Specifies whether vertex attribute addressing is a function of the vertex index or of the
        /// instance index.
        /// </summary>
        public VertexInputRate InputRate;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexInputBindingDescription"/> structure.
        /// </summary>
        /// <param name="binding">The binding number that this structure describes.</param>
        /// <param name="stride">
        /// The distance in bytes between two consecutive elements within the buffer.
        /// </param>
        /// <param name="inputRate">
        /// Specifies whether vertex attribute addressing is a function of the vertex index or of the
        /// instance index.
        /// </param>
        public VertexInputBindingDescription(int binding, int stride, VertexInputRate inputRate)
        {
            Binding = binding;
            Stride = stride;
            InputRate = inputRate;
        }
    }

    /// <summary>
    /// Specify rate at which vertex attributes are pulled from buffers.
    /// </summary>
    public enum VertexInputRate
    {
        /// <summary>
        /// Specifies that vertex attribute addressing is a function of the vertex index.
        /// </summary>
        Vertex = 0,
        /// <summary>
        /// Specifies that vertex attribute addressing is a function of the instance index.
        /// </summary>
        Instance = 1
    }

    /// <summary>
    /// Structure specifying vertex input attribute description.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexInputAttributeDescription
    {
        /// <summary>
        /// The shader binding location number for this attribute.
        /// <para>Must be less than <see cref="PhysicalDeviceLimits.MaxVertexInputAttributes"/>.</para>
        /// </summary>
        public int Location;
        /// <summary>
        /// The binding number which this attribute takes its data from.
        /// <para>Must be less than <see cref="PhysicalDeviceLimits.MaxVertexInputBindings"/>.</para>
        /// </summary>
        public int Binding;
        /// <summary>
        /// The size and type of the vertex attribute data.
        /// <para>
        /// Must be allowed as a vertex buffer format, as specified by the <see
        /// cref="FormatFeatures.VertexBuffer"/> flag in <see
        /// cref="FormatProperties.BufferFeatures"/> returned by <see cref="PhysicalDevice.GetFormatProperties"/>.
        /// </para>
        /// </summary>
        public Format Format;
        /// <summary>
        /// A byte offset of this attribute relative to the start of an element in the vertex input binding.
        /// <para>Must be less than or equal to <see cref="PhysicalDeviceLimits.MaxVertexInputAttributeOffset"/>.</para>
        /// </summary>
        public int Offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexInputAttributeDescription"/> attribute.
        /// </summary>
        /// <param name="location">The shader binding location number for this attribute.</param>
        /// <param name="binding">The binding number which this attribute takes its data from.</param>
        /// <param name="format">The size and type of the vertex attribute data.</param>
        /// <param name="offset">
        /// A byte offset of this attribute relative to the start of an element in the vertex input binding.
        /// </param>
        public VertexInputAttributeDescription(int location, int binding, Format format, int offset)
        {
            Location = location;
            Binding = binding;
            Format = format;
            Offset = offset;
        }
    }

    /// <summary>
    /// Structure specifying parameters of a newly created pipeline input assembly state.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineInputAssemblyStateCreateInfo
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal PipelineInputAssemblyStateCreateFlags Flags;

        /// <summary>
        /// Defines the primitive topology.
        /// </summary>
        public PrimitiveTopology Topology;
        /// <summary>
        /// Controls whether a special vertex index value is treated as restarting the assembly of
        /// primitives. This enable only applies to indexed draws (<see
        /// cref="CommandBuffer.CmdDrawIndexed"/> and <see
        /// cref="CommandBuffer.CmdDrawIndexedIndirect"/>), and the special index value is either
        /// 0xFFFFFFFF when the index type parameter of <see
        /// cref="CommandBuffer.CmdBindIndexBuffer"/> is equal to <see cref="IndexType.UInt32"/>, or
        /// 0xFFFF when index type is equal to <see cref="IndexType.UInt16"/>. Primitive restart is
        /// not allowed for "list" topologies.
        /// </summary>
        public Bool PrimitiveRestartEnable;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInputAssemblyStateCreateInfo"/> structure.
        /// </summary>
        /// <param name="topology">Defines the primitive topology.</param>
        /// <param name="primitiveRestartEnable">
        /// Controls whether a special vertex index value is treated as restarting the assembly of
        /// primitives. This enable only applies to indexed draws ( <see
        /// cref="CommandBuffer.CmdDrawIndexed"/> and <see
        /// cref="CommandBuffer.CmdDrawIndexedIndirect"/>), and the special index value is either
        /// 0xFFFFFFFF when the index type parameter of <see
        /// cref="CommandBuffer.CmdBindIndexBuffer"/> is equal to <see cref="IndexType.UInt32"/>, or
        /// 0xFFFF when index type is equal to <see cref="IndexType.UInt16"/>. Primitive restart is
        /// not allowed for "list" topologies.
        /// </param>
        public PipelineInputAssemblyStateCreateInfo(PrimitiveTopology topology, bool primitiveRestartEnable = false)
        {
            Type = StructureType.PipelineInputAssemblyStateCreateInfo;
            Next = IntPtr.Zero;
            Flags = 0;
            Topology = topology;
            PrimitiveRestartEnable = primitiveRestartEnable;
        }

        internal void Prepare()
        {
            Type = StructureType.PipelineInputAssemblyStateCreateInfo;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum PipelineInputAssemblyStateCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Supported primitive topologies.
    /// </summary>
    public enum PrimitiveTopology
    {
        PointList = 0,
        LineList = 1,
        LineStrip = 2,
        TriangleList = 3,
        TriangleStrip = 4,
        TriangleFan = 5,
        LineListWithAdjacency = 6,
        LineStripWithAdjacency = 7,
        TriangleListWithAdjacency = 8,
        TriangleStripWithAdjacency = 9,
        PatchList = 10
    }

    /// <summary>
    /// Structure specifying parameters of a newly created pipeline tessellation state.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineTessellationStateCreateInfo
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal PipelineTessellationStateCreateFlags Flags;

        /// <summary>
        /// Number of control points per patch.
        /// <para>Must be greater than zero and less than or equal to <see cref="PhysicalDeviceLimits.MaxTessellationPatchSize"/>.</para>
        /// </summary>
        public int PatchControlPoints;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineTessellationStateCreateInfo"/> structure.
        /// </summary>
        /// <param name="patchControlPoints">Number of control points per patch.</param>
        public PipelineTessellationStateCreateInfo(int patchControlPoints)
        {
            Type = StructureType.PipelineTessellationStateCreateInfo;
            Next = IntPtr.Zero;
            Flags = 0;
            PatchControlPoints = patchControlPoints;
        }

        internal void Prepare()
        {
            Type = StructureType.PipelineTessellationStateCreateInfo;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum PipelineTessellationStateCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying parameters of a newly created pipeline viewport state.
    /// </summary>
    public unsafe struct PipelineViewportStateCreateInfo
    {
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// An array of <see cref="Viewport"/> structures, defining the viewport transforms. If the
        /// viewport state is dynamic, this member is ignored.
        /// </summary>
        public Viewport[] Viewports;
        /// <summary>
        /// An array of <see cref="Rect2D"/> structures which define the rectangular bounds of the
        /// scissor for the corresponding viewport. If the scissor state is dynamic, this member is ignored.
        /// </summary>
        public Rect2D[] Scissors;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineViewportStateCreateInfo"/> structure.
        /// </summary>
        /// <param name="viewports">
        /// An array of <see cref="Viewport"/> structures, defining the viewport transforms. If the
        /// viewport state is dynamic, this member is ignored.
        /// </param>
        /// <param name="scissors">
        /// An array of <see cref="Rect2D"/> structures which define the rectangular bounds of the
        /// scissor for the corresponding viewport. If the scissor state is dynamic, this member is ignored.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public PipelineViewportStateCreateInfo(Viewport[] viewports, Rect2D[] scissors, IntPtr next = default(IntPtr))
        {
            Next = next;
            Viewports = viewports;
            Scissors = scissors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineViewportStateCreateInfo"/> structure.
        /// </summary>
        /// <param name="viewport">
        /// Defines the viewport transforms. If the viewport state is dynamic, this member is ignored.
        /// </param>
        /// <param name="scissor">
        /// Defines the rectangular bounds of the scissor for the viewport. If the scissor state is
        /// dynamic, this member is ignored.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public PipelineViewportStateCreateInfo(Viewport viewport, Rect2D scissor, IntPtr next = default(IntPtr))
        {
            Next = next;
            Viewports = new[] { viewport };
            Scissors = new[] { scissor };
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PipelineViewportStateCreateFlags Flags;
            public int ViewportCount;
            public IntPtr Viewports;
            public int ScissorCount;
            public IntPtr Scissors;

            public void Free()
            {
                Interop.Free(Viewports);
                Interop.Free(Scissors);
            }
        }

        internal void ToNative(Native* native)
        {
            native->Type = StructureType.PipelineViewportStateCreateInfo;
            native->Next = Next;
            native->Flags = 0;
            native->ViewportCount = Viewports?.Length ?? 0;
            native->Viewports = Interop.Struct.AllocToPointer(Viewports);
            native->ScissorCount = Scissors?.Length ?? 0;
            native->Scissors = Interop.Struct.AllocToPointer(Scissors);
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum PipelineViewportStateCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying a viewport.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Viewport
    {
        /// <summary>
        /// The x coordinate for the viewport's upper left corner (x,y).
        /// </summary>
        public float X;
        /// <summary>
        /// The y coordinate for the viewport's upper left corner (x,y).
        /// </summary>
        public float Y;
        /// <summary>
        /// Viewport's width.
        /// </summary>
        public float Width;
        /// <summary>
        /// Viewport's height.
        /// </summary>
        public float Height;
        /// <summary>
        /// Minimum depth range for the viewport. It is valid for <see cref="MinDepth"/> to be
        /// greater than or equal to <see cref="MaxDepth"/>.
        /// <para>Must be between 0.0 and 1.0, inclusive.</para>
        /// </summary>
        public float MinDepth;
        /// <summary>
        /// Maximum depth range for the viewport. It is valid for <see cref="MinDepth"/> to be
        /// greater than or equal to <see cref="MaxDepth"/>.
        /// <para>Must be between 0.0 and 1.0, inclusive.</para>
        /// </summary>
        public float MaxDepth;

        /// <summary>
        /// Initializes a new instance of the <see cref="Viewport"/> structure.
        /// </summary>
        /// <param name="x">The x coordinate for the viewport's upper left corner (x,y).</param>
        /// <param name="y">The y coordinate for the viewport's upper left corner (x,y).</param>
        /// <param name="width">Viewport's width.</param>
        /// <param name="height">Viewport's height.</param>
        /// <param name="minDepth">
        /// Minimum depth range for the viewport. It is valid for <see cref="MinDepth"/> to be
        /// greater than or equal to <see cref="MaxDepth"/>.
        /// </param>
        /// <param name="maxDepth">
        /// Maximum depth range for the viewport. It is valid for <see cref="MinDepth"/> to be
        /// greater than or equal to <see cref="MaxDepth"/>.
        /// </param>
        public Viewport(float x, float y, float width, float height, float minDepth = 0.0f, float maxDepth = 1.0f)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            MinDepth = minDepth;
            MaxDepth = maxDepth;
        }
    }

    /// <summary>
    /// Structure specifying parameters of a newly created pipeline rasterization state.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineRasterizationStateCreateInfo
    {
        internal StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        internal PipelineRasterizationStateCreateFlags Flags;
        /// <summary>
        /// Controls whether to clamp the fragment's depth values instead of clipping primitives to
        /// the z planes of the frustum.
        /// </summary>
        public Bool DepthClampEnable;
        /// <summary>
        /// Controls whether primitives are discarded immediately before the rasterization stage.
        /// </summary>
        public Bool RasterizerDiscardEnable;
        /// <summary>
        /// The triangle rendering mode. See <see cref="VulkanCore.PolygonMode"/>.
        /// </summary>
        public PolygonMode PolygonMode;
        /// <summary>
        /// The triangle facing direction used for primitive culling. See <see cref="CullModes"/>.
        /// </summary>
        public CullModes CullMode;
        /// <summary>
        /// Specifies the front-facing triangle orientation to be used for culling.
        /// </summary>
        public FrontFace FrontFace;
        /// <summary>
        /// Controls whether to bias fragment depth values.
        /// </summary>
        public Bool DepthBiasEnable;
        /// <summary>
        /// A scalar factor controlling the constant depth value added to each fragment.
        /// </summary>
        public float DepthBiasConstantFactor;
        /// <summary>
        /// The maximum (or minimum) depth bias of a fragment.
        /// </summary>
        public float DepthBiasClamp;
        /// <summary>
        /// A scalar factor applied to a fragment's slope in depth bias calculations.
        /// </summary>
        public float DepthBiasSlopeFactor;
        /// <summary>
        /// The width of rasterized line segments.
        /// </summary>
        public float LineWidth;

        internal void Prepare()
        {
            Type = StructureType.PipelineRasterizationStateCreateInfo;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum PipelineRasterizationStateCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Bitmask controlling triangle culling.
    /// </summary>
    [Flags]
    public enum CullModes
    {
        /// <summary>
        /// Specifies that no triangles are discarded.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that front-facing triangles are discarded.
        /// </summary>
        Front = 1 << 0,
        /// <summary>
        /// Specifies that back-facing triangles are discarded.
        /// </summary>
        Back = 1 << 1,
        /// <summary>
        /// Specifies that all triangles are discarded.
        /// </summary>
        FrontAndBack = 0x00000003
    }

    /// <summary>
    /// Control polygon rasterization mode.
    /// </summary>
    public enum PolygonMode
    {
        /// <summary>
        /// Specifies that polygons are rendered using the polygon rasterization rules in this section.
        /// </summary>
        Fill = 0,
        /// <summary>
        /// Specifies that polygon edges are drawn as line segments.
        /// </summary>
        Line = 1,
        /// <summary>
        /// Specifies that polygon vertices are drawn as points.
        /// </summary>
        Point = 2,
        /// <summary>
        /// Specifies that polygons are rendered using polygon rasterization rules, modified to
        /// consider a sample within the primitive if the sample location is inside the axis-aligned
        /// bounding box of the triangle after projection. Note that the barycentric weights used in
        /// attribute interpolation can extend outside the range [0,1] when these primitives are
        /// shaded. Special treatment is given to a sample position on the boundary edge of the
        /// bounding box.
        /// <para>
        /// In such a case, if two rectangles lie on either side of a common edge (with identical
        /// endpoints) on which a sample position lies, then exactly one of the triangles must
        /// produce a fragment that covers that sample during rasterization.
        /// </para>
        /// <para>
        /// Polygons rendered in <see cref="FillRectangleNV"/> mode may be clipped by the frustum or
        /// by user clip planes.
        /// </para>
        /// <para>If clipping is applied, the triangle is culled rather than clipped.</para>
        /// <para>
        /// Area calculation and facingness are determined for <see cref="FillRectangleNV"/> mode
        /// using the triangle's vertices.
        /// </para>
        /// </summary>
        FillRectangleNV = 1000153000
    }

    /// <summary>
    /// Interpret polygon front-facing orientation.
    /// </summary>
    public enum FrontFace
    {
        /// <summary>
        /// Specifies that a triangle with positive area is considered front-facing.
        /// </summary>
        CounterClockwise = 0,
        /// <summary>
		/// Specifies that a triangle with negative area is considered front-facing.
		/// </summary>
        Clockwise = 1
    }

    /// <summary>
    /// Structure specifying parameters of a newly created pipeline multisample state.
    /// </summary>
    public unsafe struct PipelineMultisampleStateCreateInfo
    {
        /// <summary>
        /// Specifies the number of samples per pixel used in rasterization.
        /// </summary>
        public SampleCounts RasterizationSamples;
        /// <summary>
        /// Specifies that fragment shading executes per-sample if <c>true</c>, or per-fragment if <c>false</c>.
        /// </summary>
        public Bool SampleShadingEnable;
        /// <summary>
        /// The minimum fraction of sample shading.
        /// </summary>
        public float MinSampleShading;
        /// <summary>
        /// Bitmasks of static coverage information that is ANDed with the coverage information
        /// generated during rasterization.
        /// <para>
        /// If not <c>null</c>, must be a an array of <c>RasterizationSamples / 32</c>
        /// sample mask values.
        /// </para>
        /// </summary>
        public int[] SampleMask;
        /// <summary>
        /// Controls whether a temporary coverage value is generated based on the alpha component of
        /// the fragment's first color output.
        /// </summary>
        public Bool AlphaToCoverageEnable;
        /// <summary>
        /// Controls whether the alpha component of the fragment's first color output is replaced
        /// with one.
        /// </summary>
        public Bool AlphaToOneEnable;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PipelineMultisampleStateCreateFlags Flags;
            public SampleCounts RasterizationSamples;
            public Bool SampleShadingEnable;
            public float MinSampleShading;
            public int* SampleMask;
            public Bool AlphaToCoverageEnable;
            public Bool AlphaToOneEnable;

            public void Free()
            {
                Interop.Free(SampleMask);
            }
        }

        internal void ToNative(Native* native)
        {
            int* sampleMask = null;
            if (SampleMask != null && SampleMask.Length > 0)
            {
                int sampleMaskCount = (int)Math.Ceiling((int)RasterizationSamples / 32.0d);
                sampleMask = (int*)Interop.Alloc(sampleMaskCount * sizeof(int));
                for (int i = 0; i < sampleMaskCount || i < SampleMask.Length; i++)
                    sampleMask[i] = SampleMask[i];
            }

            native->Type = StructureType.PipelineMultisampleStateCreateInfo;
            native->Next = IntPtr.Zero;
            native->Flags = 0;
            native->RasterizationSamples = RasterizationSamples;
            native->SampleShadingEnable = SampleShadingEnable;
            native->MinSampleShading = MinSampleShading;
            native->SampleMask = sampleMask;
            native->AlphaToCoverageEnable = AlphaToCoverageEnable;
            native->AlphaToOneEnable = AlphaToOneEnable;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum PipelineMultisampleStateCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying a pipeline color blend attachment state.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineColorBlendAttachmentState
    {
        /// <summary>
        /// Controls whether blending is enabled for the corresponding color attachment. If blending
        /// is not enabled, the source fragment's color for that attachment is passed through unmodified.
        /// </summary>
        public Bool BlendEnable;
        /// <summary>
        /// Selects which blend factor is used to determine the source factors.
        /// </summary>
        public BlendFactor SrcColorBlendFactor;
        /// <summary>
        /// Selects which blend factor is used to determine the destination factors.
        /// </summary>
        public BlendFactor DstColorBlendFactor;
        /// <summary>
        /// Selects which blend operation is used to calculate the RGB values to write to the color attachment.
        /// </summary>
        public BlendOp ColorBlendOp;
        /// <summary>
        /// Selects which blend factor is used to determine the source factor.
        /// </summary>
        public BlendFactor SrcAlphaBlendFactor;
        /// <summary>
        /// Selects which blend factor is used to determine the destination factor.
        /// </summary>
        public BlendFactor DstAlphaBlendFactor;
        /// <summary>
        /// Selects which blend operation is use to calculate the alpha values to write to the color attachment.
        /// </summary>
        public BlendOp AlphaBlendOp;
        /// <summary>
        /// A bitmask specifying which of the R, G, B, and/or A components are enabled for writing.
        /// </summary>
        public ColorComponents ColorWriteMask;
    }

    /// <summary>
    /// Bitmask controlling which components are written to the framebuffer.
    /// </summary>
    [Flags]
    public enum ColorComponents
    {
        /// <summary>
        /// Specifies that the R value is written to color attachment for the appropriate sample.
        /// Otherwise, the value in memory is unmodified.
        /// </summary>
        R = 1 << 0,
        /// <summary>
        /// Specifies that the G value is written to color attachment for the appropriate sample.
        /// Otherwise, the value in memory is unmodified.
        /// </summary>
        G = 1 << 1,
        /// <summary>
        /// Specifies that the B value is written to color attachment for the appropriate sample.
        /// Otherwise, the value in memory is unmodified.
        /// </summary>
        B = 1 << 2,
        /// <summary>
        /// Specifies that the A value is written to color attachment for the appropriate sample.
        /// Otherwise, the value in memory is unmodified.
        /// </summary>
        A = 1 << 3,
        /// <summary>
        /// Specifies that all the values are written to color attachment for the appropriate sample.
        /// Otherwise, the value in memory is unmodified.
        /// </summary>
        All = R | G | B | A
    }

    /// <summary>
    /// Structure specifying parameters of a newly created pipeline color blend state.
    /// </summary>
    public unsafe struct PipelineColorBlendStateCreateInfo
    {
        /// <summary>
        /// Controls whether to apply logical operations.
        /// </summary>
        public Bool LogicOpEnable;
        /// <summary>
        /// Selects which logical operation to apply.
        /// </summary>
        public LogicOp LogicOp;
        /// <summary>
        /// Per target attachment states.
        /// </summary>
        public PipelineColorBlendAttachmentState[] Attachments;
        /// <summary>
        /// R, G, B, and A components of the blend constant that are used in blending, depending on
        /// the blend factor.
        /// </summary>
        public ColorF4 BlendConstants;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineColorBlendStateCreateInfo"/> structure.
        /// </summary>
        /// <param name="attachments">Per target attachment states.</param>
        /// <param name="logicOpEnable">Controls whether to apply logical operations.</param>
        /// <param name="logicOp">Selects which logical operation to apply.</param>
        /// <param name="blendConstants">
        /// R, G, B, and A components of the blend constant that are used in blending, depending on
        /// the blend factor.
        /// </param>
        public PipelineColorBlendStateCreateInfo(
            PipelineColorBlendAttachmentState[] attachments,
            bool logicOpEnable = false,
            LogicOp logicOp = LogicOp.NoOp,
            ColorF4 blendConstants = default(ColorF4))
        {
            Attachments = attachments;
            LogicOpEnable = logicOpEnable;
            LogicOp = logicOp;
            BlendConstants = blendConstants;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PipelineColorBlendStateCreateFlags Flags;
            public Bool LogicOpEnable;
            public LogicOp LogicOp;
            public int AttachmentCount;
            public IntPtr Attachments;
            public ColorF4 BlendConstants;

            public void Free()
            {
                Interop.Free(Attachments);
            }
        }

        internal void ToNative(Native* native)
        {
            native->Type = StructureType.PipelineColorBlendStateCreateInfo;
            native->Next = IntPtr.Zero;
            native->Flags = 0;
            native->LogicOpEnable = LogicOpEnable;
            native->LogicOp = LogicOp;
            native->AttachmentCount = Attachments?.Length ?? 0;
            native->Attachments = Interop.Struct.AllocToPointer(Attachments);
            native->BlendConstants = BlendConstants;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum PipelineColorBlendStateCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Framebuffer blending operations.
    /// </summary>
    public enum BlendOp
    {
        Add = 0,
        Subtract = 1,
        ReverseSubtract = 2,
        Min = 3,
        Max = 4,
        ZeroExt = 1000148000,
        SrcExt = 1000148001,
        DstExt = 1000148002,
        SrcOverExt = 1000148003,
        DstOverExt = 1000148004,
        SrcInExt = 1000148005,
        DstInExt = 1000148006,
        SrcOutExt = 1000148007,
        DstOutExt = 1000148008,
        SrcAtopExt = 1000148009,
        DstAtopExt = 1000148010,
        XorExt = 1000148011,
        MultiplyExt = 1000148012,
        ScreenExt = 1000148013,
        OverlayExt = 1000148014,
        DarkenExt = 1000148015,
        LightenExt = 1000148016,
        ColordodgeExt = 1000148017,
        ColorburnExt = 1000148018,
        HardlightExt = 1000148019,
        SoftlightExt = 1000148020,
        DifferenceExt = 1000148021,
        ExclusionExt = 1000148022,
        InvertExt = 1000148023,
        InvertRgbExt = 1000148024,
        LineardodgeExt = 1000148025,
        LinearburnExt = 1000148026,
        VividlightExt = 1000148027,
        LinearlightExt = 1000148028,
        PinlightExt = 1000148029,
        HardmixExt = 1000148030,
        HslHueExt = 1000148031,
        HslSaturationExt = 1000148032,
        HslColorExt = 1000148033,
        HslLuminosityExt = 1000148034,
        PlusExt = 1000148035,
        PlusClampedExt = 1000148036,
        PlusClampedAlphaExt = 1000148037,
        PlusDarkerExt = 1000148038,
        MinusExt = 1000148039,
        MinusClampedExt = 1000148040,
        ContrastExt = 1000148041,
        InvertOvgExt = 1000148042,
        RedExt = 1000148043,
        GreenExt = 1000148044,
        BlueExt = 1000148045
    }

    /// <summary>
    /// Stencil comparison function.
    /// </summary>
    public enum StencilOp
    {
        /// <summary>
        /// Keeps the current value.
        /// </summary>
        Keep = 0,
        /// <summary>
        /// Sets the value to 0.
        /// </summary>
        Zero = 1,
        /// <summary>
        /// Sets the value to reference.
        /// </summary>
        Replace = 2,
        /// <summary>
        /// Increments the current value and clamps to the maximum representable unsigned value.
        /// </summary>
        IncrementAndClamp = 3,
        /// <summary>
        /// Decrements the current value and clamps to 0.
        /// </summary>
        DecrementAndClamp = 4,
        /// <summary>
        /// Bitwise-inverts the current value.
        /// </summary>
        Invert = 5,
        /// <summary>
        /// Increments the current value and wraps to 0 when the maximum value would have been exceeded.
        /// </summary>
        IncrementAndWrap = 6,
        /// <summary>
        /// Decrements the current value and wraps to the maximum possible value when the value would
        /// go below 0.
        /// </summary>
        DecrementAndWrap = 7
    }

    /// <summary>
    /// Framebuffer logical operations.
    /// </summary>
    public enum LogicOp
    {
        Clear = 0,
        And = 1,
        AndReverse = 2,
        Copy = 3,
        AndInverted = 4,
        NoOp = 5,
        Xor = 6,
        Or = 7,
        Nor = 8,
        Equivalent = 9,
        Invert = 10,
        OrReverse = 11,
        CopyInverted = 12,
        OrInverted = 13,
        Nand = 14,
        Set = 15
    }

    /// <summary>
    /// Framebuffer blending factors.
    /// </summary>
    public enum BlendFactor
    {
        Zero = 0,
        One = 1,
        SrcColor = 2,
        OneMinusSrcColor = 3,
        DstColor = 4,
        OneMinusDstColor = 5,
        SrcAlpha = 6,
        OneMinusSrcAlpha = 7,
        DstAlpha = 8,
        OneMinusDstAlpha = 9,
        ConstantColor = 10,
        OneMinusConstantColor = 11,
        ConstantAlpha = 12,
        OneMinusConstantAlpha = 13,
        SrcAlphaSaturate = 14,
        Src1Color = 15,
        OneMinusSrc1Color = 16,
        Src1Alpha = 17,
        OneMinusSrc1Alpha = 18
    }

    /// <summary>
    /// Structure specifying parameters of a newly created pipeline depth stencil state.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineDepthStencilStateCreateInfo
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal PipelineDepthStencilStateCreateFlags Flags;

        /// <summary>
        /// Controls whether depth testing is enabled.
        /// </summary>
        public Bool DepthTestEnable;
        /// <summary>
        /// Controls whether depth writes are enabled when <see cref="DepthTestEnable"/> is
        /// <c>true</c>. Depth writes are always disabled when <see cref="DepthTestEnable"/> is <c>false</c>.
        /// </summary>
        public Bool DepthWriteEnable;
        /// <summary>
        /// The comparison operator used in the depth test.
        /// </summary>
        public CompareOp DepthCompareOp;
        /// <summary>
        /// Controls whether depth bounds testing is enabled.
        /// </summary>
        public Bool DepthBoundsTestEnable;
        /// <summary>
        /// Controls whether stencil testing is enabled.
        /// </summary>
        public Bool StencilTestEnable;
        /// <summary>
        /// Controls the parameters of the stencil test.
        /// </summary>
        public StencilOpState Front;
        /// <summary>
        /// Controls the parameters of the stencil test.
        /// </summary>
        public StencilOpState Back;
        /// <summary>
        /// Defines the range of values used in the depth bounds test.
        /// </summary>
        public float MinDepthBounds;
        /// <summary>
        /// Defines the range of values used in the depth bounds test.
        /// </summary>
        public float MaxDepthBounds;

        internal void Prepare()
        {
            Type = StructureType.PipelineDepthStencilStateCreateInfo;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum PipelineDepthStencilStateCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying stencil operation state.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct StencilOpState
    {
        /// <summary>
        /// Specifies the action performed on samples that fail the stencil test.
        /// </summary>
        public StencilOp FailOp;
        /// <summary>
        /// Specifies the action performed on samples that pass both the depth and stencil tests.
        /// </summary>
        public StencilOp PassOp;
        /// <summary>
        /// Specifies the action performed on samples that pass the stencil test and fail the depth test.
        /// </summary>
        public StencilOp DepthFailOp;
        /// <summary>
        /// Specifies the comparison operator used in the stencil test.
        /// </summary>
        public CompareOp CompareOp;
        /// <summary>
        /// Selects the bits of the unsigned integer stencil values participating in the stencil test.
        /// </summary>
        public int CompareMask;
        /// <summary>
        /// Selects the bits of the unsigned integer stencil values updated by the stencil test in
        /// the stencil framebuffer attachment.
        /// </summary>
        public int WriteMask;
        /// <summary>
        /// An integer reference value that is used in the unsigned stencil comparison.
        /// </summary>
        public int Reference;
    }

    /// <summary>
    /// Stencil comparison function.
    /// </summary>
    public enum CompareOp
    {
        /// <summary>
        /// Specifies that the test never passes.
        /// </summary>
        Never = 0,
        /// <summary>
        /// Specifies that the test passes when R &lt; S.
        /// </summary>
        Less = 1,
        /// <summary>
        /// Specifies that the test passes when R = S.
        /// </summary>
        Equal = 2,
        /// <summary>
        /// Specifies that the test passes when R &lt;= S.
        /// </summary>
        LessOrEqual = 3,
        /// <summary>
        /// Specifies that the test passes when R &gt; S.
        /// </summary>
        Greater = 4,
        /// <summary>
        /// Specifies that the test passes when R != S.
        /// </summary>
        NotEqual = 5,
        /// <summary>
        /// Specifies that the test passes when R &gt;= S.
        /// </summary>
        GreaterOrEqual = 6,
        /// <summary>
        /// Specifies that the test always passes.
        /// </summary>
        Always = 7
    }

    /// <summary>
    /// Structure specifying parameters of a newly created pipeline dynamic state.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct PipelineDynamicStateCreateInfo
    {
        /// <summary>
        /// Values specifying which pieces of pipeline state will use the values from dynamic state
        /// commands rather than from the pipeline state creation info.
        /// </summary>
        public DynamicState[] DynamicStates;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDynamicStateCreateInfo"/> structure.
        /// </summary>
        /// <param name="dynamicStates">
        /// Values specifying which pieces of pipeline state will use the values from dynamic state
        /// commands rather than from the pipeline state creation info.
        /// </param>
        public PipelineDynamicStateCreateInfo(params DynamicState[] dynamicStates)
        {
            DynamicStates = dynamicStates;
        }

        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PipelineDynamicStateCreateFlags Flags;
            public int DynamicStateCount;
            public IntPtr DynamicStates;

            public void Free()
            {
                Interop.Free(DynamicStates);
            }
        }

        internal void ToNative(Native* native)
        {
            native->Type = StructureType.PipelineDynamicStateCreateInfo;
            native->Next = IntPtr.Zero;
            native->Flags = 0;
            native->DynamicStateCount = DynamicStates?.Length ?? 0;
            native->DynamicStates = Interop.Struct.AllocToPointer(DynamicStates);
        }
    }

    // Is reserved for future use.
    internal enum PipelineDynamicStateCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Indicate which dynamic state is taken from dynamic state commands.
    /// </summary>
    public enum DynamicState
    {
        /// <summary>
        /// Specifies that the <see cref="PipelineViewportStateCreateInfo.Viewports"/> state will be
        /// ignored and must be set dynamically with <see cref="CommandBuffer.CmdSetViewport"/>
        /// before any draw commands. The number of viewports used by a pipeline is still specified
        /// by the length of <see cref="PipelineViewportStateCreateInfo.Viewports"/>.
        /// </summary>
        Viewport = 0,
        /// <summary>
        /// Specifies that the <see cref="PipelineViewportStateCreateInfo.Scissors"/> state will be
        /// ignored and must be set dynamically with <see cref="CommandBuffer.CmdSetScissor"/> before
        /// any draw commands. The number of scissor rectangles used by a pipeline is still specified
        /// by the length of <see cref="PipelineViewportStateCreateInfo.Scissors"/>.
        /// </summary>
        Scissor = 1,
        /// <summary>
        /// Specifies that the <see cref="PipelineRasterizationStateCreateInfo.LineWidth"/> state
        /// will be ignored and must be set dynamically with <see
        /// cref="CommandBuffer.CmdSetLineWidth"/> before any draw commands that generate line
        /// primitives for the rasterizer.
        /// </summary>
        LineWidth = 2,
        /// <summary>
        /// Specifies that the <see
        /// cref="PipelineRasterizationStateCreateInfo.DepthBiasConstantFactor"/>, <see
        /// cref="PipelineRasterizationStateCreateInfo.DepthBiasClamp"/> and <see
        /// cref="PipelineRasterizationStateCreateInfo.DepthBiasSlopeFactor"/> states will be ignored
        /// and must be set dynamically with <see cref="CommandBuffer.CmdSetDepthBias"/> before any
        /// draws are performed with <see
        /// cref="PipelineRasterizationStateCreateInfo.DepthBiasEnable"/> set to <c>true</c>.
        /// </summary>
        DepthBias = 3,
        /// <summary>
        /// Specifies that the <see cref="PipelineColorBlendStateCreateInfo.BlendConstants"/> state
        /// will be ignored and must be set dynamically with <see
        /// cref="CommandBuffer.CmdSetBlendConstants"/> before any draws are performed with a
        /// pipeline state with <see cref="PipelineColorBlendAttachmentState.BlendEnable"/> member
        /// set to <c>true</c> and any of the blend functions using a constant blend color.
        /// </summary>
        BlendConstants = 4,
        /// <summary>
        /// Specifies that the <see cref="PipelineDepthStencilStateCreateInfo.MinDepthBounds"/> and
        /// <see cref="PipelineDepthStencilStateCreateInfo.MaxDepthBounds"/> states will be ignored
        /// and must be set dynamically with <see cref="CommandBuffer.CmdSetDepthBounds"/> before any
        /// draws are performed with a pipeline state with <see
        /// cref="PipelineDepthStencilStateCreateInfo.DepthBoundsTestEnable"/> member set to <c>true</c>.
        /// </summary>
        DepthBounds = 5,
        /// <summary>
        /// Specifies that the compare mask state in both <see
        /// cref="PipelineDepthStencilStateCreateInfo.Front"/> and <see
        /// cref="PipelineDepthStencilStateCreateInfo.Back"/> will be ignored and must be set
        /// dynamically with <see cref="CommandBuffer.CmdSetStencilCompareMask"/> before any draws
        /// are performed with a pipeline state with <see
        /// cref="PipelineDepthStencilStateCreateInfo.StencilTestEnable"/> member set to <c>true</c>.
        /// </summary>
        StencilCompareMask = 6,
        /// <summary>
        /// Specifies that the write mask state in both <see
        /// cref="PipelineDepthStencilStateCreateInfo.Front"/> and <see
        /// cref="PipelineDepthStencilStateCreateInfo.Back"/> will be ignored and must be set
        /// dynamically with <see cref="CommandBuffer.CmdSetStencilWriteMask"/> before any draws are
        /// performed with a pipeline state with <see
        /// cref="PipelineDepthStencilStateCreateInfo.StencilTestEnable"/> member set to <c>true</c>.
        /// </summary>
        StencilWriteMask = 7,
        /// <summary>
        /// Specifies that the reference state in both <see
        /// cref="PipelineDepthStencilStateCreateInfo.Front"/> and <see
        /// cref="PipelineDepthStencilStateCreateInfo.Back"/> will be ignored and must be set
        /// dynamically with <see cref="CommandBuffer.CmdSetStencilReference"/> before any draws are
        /// performed with a pipeline state with <see
        /// cref="PipelineDepthStencilStateCreateInfo.StencilTestEnable"/> member set to <c>true</c>.
        /// </summary>
        StencilReference = 8,
        /// <summary>
        /// Specifies that the <see
        /// cref="NV.PipelineViewportWScalingStateCreateInfoNV.ViewportWScalings"/> state will be
        /// ignored and must be set dynamically with <see
        /// cref="NV.CommandBufferExtensions.CmdSetViewportWScalingNV"/> before any draws are
        /// performed with a pipeline state with <see
        /// cref="NV.PipelineViewportWScalingStateCreateInfoNV.ViewportWScalingEnable"/> set to <c>true</c>.
        /// </summary>
        ViewportWScalingNV = 1000087000,
        DiscardRectangleExt = 1000099000,
        /// <summary>
        /// Specifies that the <see
        /// cref="Ext.PipelineSampleLocationsStateCreateInfoExt.SampleLocationsInfo"/> state will be
        /// ignored and must be set dynamically with <see
        /// cref="Ext.CommandBufferExtensions.CmdSetSampleLocationsExt"/> before any draw or clear
        /// commands. Enabling custom sample locations is still indicated by the <see
        /// cref="Ext.PipelineSampleLocationsStateCreateInfoExt.SampleLocationsEnable"/> member.
        /// </summary>
        SampleLocationsExt = 1000143000
    }
}
