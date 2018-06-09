using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a command buffer object.
    /// <para>
    /// Command buffers are objects used to record commands which can be subsequently submitted to a
    /// device queue for execution. There are two levels of command buffers - primary command
    /// buffers, which can execute secondary command buffers, and which are submitted to queues, and
    /// secondary command buffers, which can be executed by primary command buffers, and which are
    /// not directly submitted to queues.
    /// </para>
    /// </summary>
    public unsafe class CommandBuffer : DisposableHandle<IntPtr>
    {
        internal CommandBuffer(CommandPool parent, IntPtr handle)
        {
            Parent = parent;
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public CommandPool Parent { get; }

        /// <summary>
        /// Start recording the command buffer.
        /// </summary>
        /// <param name="beginInfo">
        /// Defines additional information about how the command buffer begins recording.
        /// </param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void Begin(CommandBufferBeginInfo beginInfo = default(CommandBufferBeginInfo))
        {
            beginInfo.ToNative(out CommandBufferBeginInfo.Native nativeBeginInfo);
            Result result = vkBeginCommandBuffer(this, &nativeBeginInfo);
            nativeBeginInfo.Free();
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Finish recording the command buffer.
        /// </summary>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void End()
        {
            Result result = vkEndCommandBuffer(this);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Reset a command buffer to the initial state.
        /// </summary>
        /// <param name="flags">
        /// A bitmask controlling the reset operation.
        /// <para>
        /// If flags includes <see cref="CommandBufferResetFlags.ReleaseResources"/>, then most or
        /// all memory resources currently owned by the command buffer should be returned to the
        /// parent command pool. If this flag is not set, then the command buffer may hold onto
        /// memory resources and reuse them when recording commands.
        /// </para>
        /// </param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void Reset(CommandBufferResetFlags flags = 0)
        {
            Result result = vkResetCommandBuffer(this, flags);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Bind a pipeline object to a command buffer.
        /// </summary>
        /// <param name="pipelineBindPoint">
        /// Specifies whether pipeline will be bound as a compute or graphics pipeline. There are
        /// separate bind points for each of graphics and compute, so binding one does not disturb
        /// the other.
        /// </param>
        /// <param name="pipeline">The pipeline to be bound.</param>
        public void CmdBindPipeline(PipelineBindPoint pipelineBindPoint, Pipeline pipeline)
        {
            vkCmdBindPipeline(this, pipelineBindPoint, pipeline);
        }

        /// <summary>
        /// Set the viewport on a command buffer.
        /// <para>
        /// If the bound pipeline state object was not created with the <see
        /// cref="DynamicState.Viewport"/> dynamic state enabled, viewport transformation parameters
        /// are specified using the <see cref="PipelineViewportStateCreateInfo.Viewports"/> member in
        /// the pipeline state object. If the pipeline state object was created with the <see
        /// cref="DynamicState.Viewport"/> dynamic state enabled, the viewport transformation
        /// parameters are dynamically set and changed with this command.
        /// </para>
        /// </summary>
        /// <param name="viewport">Specifies viewport parameters.</param>
        public void CmdSetViewport(Viewport viewport)
        {
            vkCmdSetViewport(this, 0, 1, &viewport);
        }

        /// <summary>
        /// Set the viewport on a command buffer.
        /// <para>
        /// If the bound pipeline state object was not created with the <see
        /// cref="DynamicState.Viewport"/> dynamic state enabled, viewport transformation parameters
        /// are specified using the <see cref="PipelineViewportStateCreateInfo.Viewports"/> member in
        /// the pipeline state object. If the pipeline state object was created with the <see
        /// cref="DynamicState.Viewport"/> dynamic state enabled, the viewport transformation
        /// parameters are dynamically set and changed with this command.
        /// </para>
        /// </summary>
        /// <param name="firstViewport">
        /// The index of the first viewport whose parameters are updated by the command.
        /// </param>
        /// <param name="viewportCount">
        /// The index of the first viewport whose parameters are updated by the command.
        /// </param>
        /// <param name="viewports">Structures specifying viewport parameters.</param>
        public void CmdSetViewports(int firstViewport, int viewportCount, params Viewport[] viewports)
        {
            fixed (Viewport* viewportsPtr = viewports)
                vkCmdSetViewport(this, firstViewport, viewportCount, viewportsPtr);
        }

        /// <summary>
        /// Set the dynamic scissor rectangles on a command buffer.
        /// <para>
        /// The scissor test determines if a fragment’s framebuffer coordinates (xf,yf) lie within
        /// the scissor rectangle corresponding to the viewport index used by the primitive that
        /// generated the fragment. If the pipeline state object is created without <see
        /// cref="DynamicState.Scissor"/> enabled then the scissor rectangles are set by the <see
        /// cref="PipelineViewportStateCreateInfo.Scissors"/> state of the pipeline state object.
        /// Otherwise, to dynamically set the scissor rectangles call this command.
        /// </para>
        /// </summary>
        /// <param name="scissor">Defines scissor rectangle.</param>
        public void CmdSetScissor(Rect2D scissor)
        {
            vkCmdSetScissor(this, 0, 1, &scissor);
        }

        /// <summary>
        /// Set the dynamic scissor rectangles on a command buffer.
        /// <para>
        /// The scissor test determines if a fragment’s framebuffer coordinates (xf,yf) lie within
        /// the scissor rectangle corresponding to the viewport index used by the primitive that
        /// generated the fragment. If the pipeline state object is created without <see
        /// cref="DynamicState.Scissor"/> enabled then the scissor rectangles are set by the <see
        /// cref="PipelineViewportStateCreateInfo.Scissors"/> state of the pipeline state object.
        /// Otherwise, to dynamically set the scissor rectangles call this command.
        /// </para>
        /// </summary>
        /// <param name="firstScissor">
        /// The index of the first scissor whose state is updated by the command.
        /// </param>
        /// <param name="scissorCount">
        /// The number of scissors whose rectangles are updated by the command.
        /// </param>
        /// <param name="scissors">Structures defining scissor rectangles.</param>
        public void CmdSetScissors(int firstScissor, int scissorCount, params Rect2D[] scissors)
        {
            fixed (Rect2D* scissorsPtr = scissors)
                vkCmdSetScissor(this, firstScissor, scissorCount, scissorsPtr);
        }

        /// <summary>
        /// Set the dynamic line width state.
        /// <para>
        /// The line width is set by the <see cref="PipelineRasterizationStateCreateInfo.LineWidth"/>
        /// property in the currently active pipeline if the pipeline was not created with <see
        /// cref="DynamicState.LineWidth"/> enabled. Otherwise, the line width is set by calling this command.
        /// </para>
        /// </summary>
        /// <param name="lineWidth">The width of rasterized line segments.</param>
        public void CmdSetLineWidth(float lineWidth)
        {
            vkCmdSetLineWidth(this, lineWidth);
        }

        /// <summary>
        /// Set the depth bias dynamic state.
        /// <para>
        /// The depth values of all fragments generated by the rasterization of a polygon can be
        /// offset by a single value that is computed for that polygon. This behavior is controlled
        /// by the <see cref="PipelineRasterizationStateCreateInfo.DepthBiasEnable"/>, <see
        /// cref="PipelineRasterizationStateCreateInfo.DepthBiasConstantFactor"/><see
        /// cref="PipelineRasterizationStateCreateInfo.DepthBiasClamp"/><see
        /// cref="PipelineRasterizationStateCreateInfo.DepthBiasSlopeFactor"/> members, or by the
        /// corresponding parameters to the this command if depth bias state is dynamic.
        /// </para>
        /// </summary>
        /// <param name="depthBiasConstantFactor">
        /// A scalar factor controlling the constant depth value added to each fragment.
        /// </param>
        /// <param name="depthBiasClamp">The maximum (or minimum) depth bias of a fragment.</param>
        /// <param name="depthBiasSlopeFactor">
        /// A scalar factor applied to a fragment’s slope in depth bias calculations.
        /// </param>
        public void CmdSetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
        {
            vkCmdSetDepthBias(this, depthBiasConstantFactor, depthBiasClamp, depthBiasSlopeFactor);
        }

        /// <summary>
        /// Set the values of blend constants.
        /// </summary>
        /// <param name="blendConstants">
        /// The R, G, B, and A components of the blend constant color used in blending, depending on
        /// the blend factor.
        /// </param>
        public void CmdSetBlendConstants(ColorF4 blendConstants)
        {
            vkCmdSetBlendConstants(this, blendConstants);
        }

        /// <summary>
        /// Set the depth bounds test values for a command buffer.
        /// <para>
        /// The depth bounds test conditionally disables coverage of a sample based on the outcome of
        /// a comparison between the value za in the depth attachment at location (xf,yf) (for the
        /// appropriate sample) and a range of values. The test is enabled or disabled by the <see
        /// cref="PipelineDepthStencilStateCreateInfo.DepthBoundsTestEnable"/> member. If the
        /// pipeline state object is created without the <see cref="DynamicState.DepthBounds"/>
        /// dynamic state enabled then the range of values used in the depth bounds test are defined
        /// by the <see cref="PipelineDepthStencilStateCreateInfo.MinDepthBounds"/> and <see
        /// cref="PipelineDepthStencilStateCreateInfo.MaxDepthBounds"/> members. Otherwise, to
        /// dynamically set the depth bounds range values call this command.
        /// </para>
        /// </summary>
        /// <param name="minDepthBounds">
        /// The lower bound of the range of depth values used in the depth bounds test.
        /// </param>
        /// <param name="maxDepthBounds">The upper bound of the range.</param>
        public void CmdSetDepthBounds(float minDepthBounds, float maxDepthBounds)
        {
            vkCmdSetDepthBounds(this, minDepthBounds, maxDepthBounds);
        }

        /// <summary>
        /// Set the stencil compare mask dynamic state.
        /// <para>
        /// If the pipeline state object is created with the <see
        /// cref="DynamicState.StencilCompareMask"/> dynamic state enabled, then to dynamically set
        /// the stencil compare mask call this command.
        /// </para>
        /// </summary>
        /// <param name="faceMask">
        /// A bitmask specifying the set of stencil state for which to update the compare mask.
        /// </param>
        /// <param name="compareMask">The new value to use as the stencil compare mask.</param>
        public void CmdSetStencilCompareMask(StencilFaces faceMask, int compareMask)
        {
            vkCmdSetStencilCompareMask(this, faceMask, compareMask);
        }

        /// <summary>
        /// Set the stencil write mask dynamic state.
        /// <para>
        /// If the pipeline state object is created with the <see
        /// cref="DynamicState.StencilWriteMask"/> dynamic state enabled, then to dynamically set the
        /// stencil write mask call this command.
        /// </para>
        /// </summary>
        /// <param name="faceMask">
        /// Specifies the set of stencil state for which to update the write mask.
        /// </param>
        /// <param name="writeMask">The new value to use as the stencil write mask.</param>
        public void CmdSetStencilWriteMask(StencilFaces faceMask, int writeMask)
        {
            vkCmdSetStencilWriteMask(this, faceMask, writeMask);
        }

        /// <summary>
        /// Set the stencil reference dynamic state.
        /// <para>
        /// If the pipeline state object is created with the <see
        /// cref="DynamicState.StencilReference"/> dynamic state enabled, then to dynamically set the
        /// stencil reference value call this command.
        /// </para>
        /// </summary>
        /// <param name="faceMask">
        /// Specifies the set of stencil state for which to update the reference value.
        /// </param>
        /// <param name="reference">the new value to use as the stencil reference value.</param>
        public void CmdSetStencilReference(StencilFaces faceMask, int reference)
        {
            vkCmdSetStencilReference(this, faceMask, reference);
        }

        /// <summary>
        /// Binds descriptor set to a command buffer.
        /// </summary>
        /// <param name="pipelineBindPoint">
        /// Indicates whether the descriptors will be used by graphics pipelines or compute
        /// pipelines. There is a separate set of bind points for each of graphics and compute, so
        /// binding one does not disturb the other.
        /// </param>
        /// <param name="layout">A <see cref="PipelineLayout"/> object used to program the bindings.</param>
        /// <param name="descriptorSet">
        /// Handle to a <see cref="DescriptorSet"/> object describing the descriptor set to write to.
        /// </param>
        /// <param name="dynamicOffset">Value specifying dynamic offset.</param>
        public void CmdBindDescriptorSet(PipelineBindPoint pipelineBindPoint, PipelineLayout layout,
            DescriptorSet descriptorSet, int? dynamicOffset = null)
        {
            long descriptorSetHandle = descriptorSet;
            int dynamicOffsetValue = dynamicOffset ?? 0;
            vkCmdBindDescriptorSets(this, pipelineBindPoint, layout, 0, 1,
                &descriptorSetHandle, dynamicOffset.HasValue ? 1 : 0,
                dynamicOffset.HasValue ? &dynamicOffsetValue : null);
        }

        /// <summary>
        /// Binds descriptor sets to a command buffer.
        /// </summary>
        /// <param name="pipelineBindPoint">
        /// Indicates whether the descriptors will be used by graphics pipelines or compute
        /// pipelines. There is a separate set of bind points for each of graphics and compute, so
        /// binding one does not disturb the other.
        /// </param>
        /// <param name="layout">A <see cref="PipelineLayout"/> object used to program the bindings.</param>
        /// <param name="firstSet">The set number of the first descriptor set to be bound.</param>
        /// <param name="descriptorSets">
        /// Handles to <see cref="DescriptorSet"/> objects describing the descriptor sets to write to.
        /// </param>
        /// <param name="dynamicOffsets">Values specifying dynamic offsets.</param>
        public void CmdBindDescriptorSets(PipelineBindPoint pipelineBindPoint, PipelineLayout layout, int firstSet,
            DescriptorSet[] descriptorSets, int[] dynamicOffsets = null)
        {
            int count = descriptorSets?.Length ?? 0;
            var descriptorSetsPtr = stackalloc long[count];
            for (int i = 0; i < count; i++)
                descriptorSetsPtr[i] = descriptorSets[i];

            fixed (int* dynamicOffsetsPtr = dynamicOffsets)
            {
                vkCmdBindDescriptorSets(this, pipelineBindPoint, layout, firstSet, count,
                    descriptorSetsPtr, dynamicOffsets?.Length ?? 0, dynamicOffsetsPtr);
            }
        }

        /// <summary>
        /// Bind an index buffer to a command buffer.
        /// </summary>
        /// <param name="buffer">The buffer being bound.</param>
        /// <param name="offset">
        /// The starting offset in bytes within buffer used in index buffer address calculations.
        /// </param>
        /// <param name="indexType">Selects whether indices are treated as 16 bits or 32 bits.</param>
        public void CmdBindIndexBuffer(Buffer buffer, long offset = 0, IndexType indexType = IndexType.UInt32)
        {
            vkCmdBindIndexBuffer(this, buffer, offset, indexType);
        }

        /// <summary>
        /// Bind vertex buffer to a command buffer.
        /// </summary>
        /// <param name="buffer">The <see cref="Buffer"/> handle.</param>
        /// <param name="offset">The <see cref="Buffer"/> offsets.</param>
        public void CmdBindVertexBuffer(Buffer buffer, long offset = 0)
        {
            long handle = buffer.Handle;
            vkCmdBindVertexBuffers(this, 0, 1, &handle, &offset);
        }

        /// <summary>
        /// Bind vertex buffers to a command buffer.
        /// </summary>
        /// <param name="firstBinding">
        /// The index of the first vertex input binding whose state is updated by the command.
        /// </param>
        /// <param name="bindingCount">
        /// The number of vertex input bindings whose state is updated by the command.
        /// </param>
        /// <param name="buffers">An array of <see cref="Buffer"/> handles.</param>
        /// <param name="offsets">An array of <see cref="Buffer"/> offsets.</param>
        public void CmdBindVertexBuffers(int firstBinding, int bindingCount, Buffer[] buffers, long[] offsets)
        {
            int bufferCount = buffers?.Length ?? 0;
            long* buffersPtr = stackalloc long[bufferCount];
            for (int i = 0; i < bufferCount; i++)
                buffersPtr[i] = buffers[i];

            fixed (long* offsetsPtr = offsets)
                vkCmdBindVertexBuffers(this, firstBinding, bindingCount, buffersPtr, offsetsPtr);
        }

        /// <summary>
        /// Draw primitives.
        /// <para>
        /// When the command is executed, primitives are assembled using the current primitive
        /// topology and <paramref name="vertexCount"/> consecutive vertex indices with the first
        /// vertex index value equal to <paramref name="firstVertex"/>. The primitives are drawn
        /// <paramref name="instanceCount"/> times with instance index starting with <paramref
        /// name="firstInstance"/> and increasing sequentially for each instance. The assembled
        /// primitives execute the currently bound graphics pipeline.
        /// </para>
        /// </summary>
        /// <param name="vertexCount">The number of vertices to draw.</param>
        /// <param name="instanceCount">The number of instances to draw.</param>
        /// <param name="firstVertex">The index of the first vertex to draw.</param>
        /// <param name="firstInstance">The instance id of the first instance to draw.</param>
        public void CmdDraw(int vertexCount, int instanceCount = 1, int firstVertex = 0, int firstInstance = 0)
        {
            vkCmdDraw(this, vertexCount, instanceCount, firstVertex, firstInstance);
        }

        /// <summary>
        /// Issue an indexed draw into a command buffer.
        /// <para>
        /// When the command is executed, primitives are assembled using the current primitive
        /// topology and <paramref name="indexCount"/> vertices whose indices are retrieved from the
        /// index buffer. The index buffer is treated as an array of tightly packed unsigned integers
        /// of size defined by the <see cref="CmdBindIndexBuffer"/> index type parameter with which
        /// the buffer was bound.
        /// </para>
        /// </summary>
        /// <param name="indexCount">The number of vertices to draw.</param>
        /// <param name="instanceCount">The number of instances to draw.</param>
        /// <param name="firstIndex">The base index within the index buffer.</param>
        /// <param name="vertexOffset">
        /// The value added to the vertex index before indexing into the vertex buffer.
        /// </param>
        /// <param name="firstInstance">The instance id of the first instance to draw.</param>
        public void CmdDrawIndexed(int indexCount, int instanceCount = 1, int firstIndex = 0, int vertexOffset = 0, int firstInstance = 0)
        {
            vkCmdDrawIndexed(this, indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
        }

        /// <summary>
        /// Issue an indirect draw into a command buffer.
        /// <para>
        /// Behaves similarly to <see cref="CmdDraw"/> except that the parameters are read by the
        /// device from a buffer during execution. <paramref name="drawCount"/> draws are executed by
        /// the command, with parameters taken from buffer starting at <paramref name="offset"/> and
        /// increasing by <paramref name="stride"/> bytes for each successive draw. If <paramref
        /// name="drawCount"/> is less than or equal to one, <paramref name="stride"/> is ignored.
        /// </para>
        /// </summary>
        /// <param name="buffer">The buffer containing draw parameters.</param>
        /// <param name="offset">The byte offset into buffer where parameters begin.</param>
        /// <param name="drawCount">The number of draws to execute, and can be zero.</param>
        /// <param name="stride">The byte stride between successive sets of draw parameters.</param>
        public void CmdDrawIndirect(Buffer buffer, long offset, int drawCount, int stride)
        {
            vkCmdDrawIndirect(this, buffer, offset, drawCount, stride);
        }

        /// <summary>
        /// Perform an indexed indirect draw.
        /// <para>
        /// Behaves similarly to <see cref="CmdDrawIndexed"/> except that the parameters are read by
        /// the device from a buffer during execution. <paramref name="drawCount"/> draws are
        /// executed by the command, with parameters taken from buffer starting at <paramref
        /// name="offset"/> and increasing by <paramref name="stride"/> bytes for each successive
        /// draw. If <paramref name="drawCount"/> is less than or equal to one, <paramref
        /// name="stride"/> is ignored.
        /// </para>
        /// </summary>
        /// <param name="buffer">The buffer containing draw parameters.</param>
        /// <param name="offset">The byte offset into buffer where parameters begin.</param>
        /// <param name="drawCount">The number of draws to execute, and can be zero.</param>
        /// <param name="stride">The byte stride between successive sets of draw parameters.</param>
        public void CmdDrawIndexedIndirect(Buffer buffer, long offset, int drawCount, int stride)
        {
            vkCmdDrawIndexedIndirect(this, buffer, offset, drawCount, stride);
        }

        /// <summary>
        /// Dispatch compute work items.
        /// <para>
        /// When the command is executed, a global workgroup consisting of x × y × z local workgroups
        /// is assembled.
        /// </para>
        /// </summary>
        /// <param name="groupCountX">The number of local workgroups to dispatch in the X dimension.</param>
        /// <param name="groupCountY">The number of local workgroups to dispatch in the Y dimension.</param>
        /// <param name="groupCountZ">The number of local workgroups to dispatch in the Z dimension.</param>
        public void CmdDispatch(int groupCountX, int groupCountY, int groupCountZ)
        {
            vkCmdDispatch(this, groupCountX, groupCountY, groupCountZ);
        }

        /// <summary>
        /// Dispatch compute work items using indirect parameters.
        /// <para>
        /// Behaves similarly to <see cref="CmdDispatch"/> except that the parameters are read by the
        /// device from a buffer during execution.
        /// </para>
        /// </summary>
        /// <param name="buffer">The buffer containing dispatch parameters.</param>
        /// <param name="offset">The byte offset into buffer where parameters begin.</param>
        public void CmdDispatchIndirect(Buffer buffer, long offset)
        {
            vkCmdDispatchIndirect(this, buffer, offset);
        }

        /// <summary>
        /// Copy data between buffer regions.
        /// <para>
        /// Each region in <paramref name="regions"/> is copied from the source buffer to the same
        /// region of the destination buffer. <paramref name="srcBuffer"/> and <paramref
        /// name="dstBuffer"/> can be the same buffer or alias the same memory, but the result is
        /// undefined if the copy regions overlap in memory.
        /// </para>
        /// </summary>
        /// <param name="srcBuffer">The source buffer.</param>
        /// <param name="dstBuffer">The destination buffer.</param>
        /// <param name="regions">Structures specifying the regions to copy.</param>
        public void CmdCopyBuffer(Buffer srcBuffer, Buffer dstBuffer, params BufferCopy[] regions)
        {
            fixed (BufferCopy* regionsPtr = regions)
                vkCmdCopyBuffer(this, srcBuffer, dstBuffer, regions?.Length ?? 0, regionsPtr);
        }

        /// <summary>
        /// Copy data between images.
        /// </summary>
        /// <param name="srcImage">The source image.</param>
        /// <param name="srcImageLayout">The current layout of the source image subresource.</param>
        /// <param name="dstImage">The destination image.</param>
        /// <param name="dstImageLayout">The current layout of the destination image subresource.</param>
        /// <param name="regions">Structures specifying the regions to copy.</param>
        public void CmdCopyImage(Image srcImage, ImageLayout srcImageLayout, Image dstImage, ImageLayout dstImageLayout,
            params ImageCopy[] regions)
        {
            fixed (ImageCopy* regionsPtr = regions)
                vkCmdCopyImage(this, srcImage, srcImageLayout, dstImage, dstImageLayout, regions?.Length ?? 0, regionsPtr);
        }

        /// <summary>
        /// Copy regions of an image, potentially performing format conversion, arbitrary scaling,
        /// and filtering.
        /// <para>
        /// Must not be used for multisampled source or destination images. Use <see
        /// cref="CmdResolveImage"/> for this purpose.
        /// </para>
        /// </summary>
        /// <param name="srcImage">The source image.</param>
        /// <param name="srcImageLayout">The layout of the source image subresources for the blit.</param>
        /// <param name="dstImage">The source image.</param>
        /// <param name="dstImageLayout">
        /// The layout of the destination image subresources for the blit.
        /// </param>
        /// <param name="regions">Structures specifying the regions to blit.</param>
        /// <param name="filter">Specifies the filter to apply if the blits require scaling.</param>
        public void CmdBlitImage(Image srcImage, ImageLayout srcImageLayout, long dstImage,
            ImageLayout dstImageLayout, ImageBlit[] regions, Filter filter)
        {
            fixed (ImageBlit* regionsPtr = regions)
                vkCmdBlitImage(this, srcImage, srcImageLayout, dstImage, dstImageLayout, regions?.Length ?? 0, regionsPtr, filter);
        }

        /// <summary>
        /// Copy data from a buffer into an image.
        /// <para>
        /// Each region in <paramref name="regions"/> is copied from the specified region of the
        /// source buffer to the specified region of the destination image.
        /// </para>
        /// </summary>
        /// <param name="srcBuffer">The source buffer.</param>
        /// <param name="dstImage">The destination image.</param>
        /// <param name="dstImageLayout">
        /// The layout of the destination image subresources for the copy.
        /// </param>
        /// <param name="regions">Structures specifying the regions to copy.</param>
        public void CmdCopyBufferToImage(Buffer srcBuffer, Image dstImage, ImageLayout dstImageLayout,
            params BufferImageCopy[] regions)
        {
            fixed (BufferImageCopy* regionsPtr = regions)
                vkCmdCopyBufferToImage(this, srcBuffer, dstImage, dstImageLayout, regions?.Length ?? 0, regionsPtr);
        }

        /// <summary>
        /// Copy image data into a buffer.
        /// <para>
        /// Each region in <paramref name="regions"/> is copied from the specified region of the
        /// source image to the specified region of the destination buffer.
        /// </para>
        /// </summary>
        /// <param name="srcImage">The source image.</param>
        /// <param name="srcImageLayout">The layout of the source image subresources for the copy.</param>
        /// <param name="dstBuffer">The destination buffer.</param>
        /// <param name="regions">Structures specifying the regions to copy.</param>
        public void CmdCopyImageToBuffer(Image srcImage, ImageLayout srcImageLayout, Buffer dstBuffer,
            params BufferImageCopy[] regions)
        {
            fixed (BufferImageCopy* regionsPtr = regions)
                vkCmdCopyImageToBuffer(this, srcImage, srcImageLayout, dstBuffer, regions?.Length ?? 0, regionsPtr);
        }

        /// <summary>
        /// Update a buffer's contents from host memory.
        /// <para>
        /// <paramref name="dataSize"/> must be less than or equal to 65536 bytes. For larger
        /// updates, applications can use buffer to buffer copies.
        /// </para>
        /// <para>
        /// Is only allowed outside of a render pass. This command is treated as "transfer"
        /// operation, for the purposes of synchronization barriers. The <see
        /// cref="BufferUsages.TransferDst"/> must be specified in usage of <see
        /// cref="BufferViewCreateInfo"/> in order for the buffer to be compatible with <see cref="CmdUpdateBuffer"/>.
        /// </para>
        /// </summary>
        /// <param name="dstBuffer">A handle to the buffer to be updated.</param>
        /// <param name="dstOffset">
        /// The byte offset into the buffer to start updating, and must be a multiple of 4.
        /// </param>
        /// <param name="dataSize">The number of bytes to update, and must be a multiple of 4.</param>
        /// <param name="data">
        /// A pointer to the source data for the buffer update, and must be at least <paramref
        /// name="dataSize"/> bytes in size.
        /// </param>
        public void CmdUpdateBuffer(Buffer dstBuffer, long dstOffset, long dataSize, IntPtr data)
        {
            vkCmdUpdateBuffer(this, dstBuffer, dstOffset, dataSize, data);
        }

        /// <summary>
        /// Fill a region of a buffer with a fixed value.
        /// <para>
        /// Is treated as "transfer" operation for the purposes of synchronization barriers. The <see
        /// cref="BufferUsages.TransferDst"/> must be specified in usage of <see
        /// cref="BufferCreateInfo"/> in order for the buffer to be compatible with <see cref="CmdFillBuffer"/>.
        /// </para>
        /// </summary>
        /// <param name="dstBuffer">The buffer to be filled.</param>
        /// <param name="dstOffset">
        /// The byte offset into the buffer at which to start filling, and must be a multiple of 4.
        /// </param>
        /// <param name="size">
        /// the number of bytes to fill, and must be either a multiple of 4, or <see
        /// cref="WholeSize"/> to fill the range from offset to the end of the buffer. If <see
        /// cref="WholeSize"/> is used and the remaining size of the buffer is not a multiple of 4,
        /// then the nearest smaller multiple is used.
        /// </param>
        /// <param name="data">
        /// The 4-byte word written repeatedly to the buffer to fill size bytes of data. The data
        /// word is written to memory according to the host endianness.
        /// </param>
        public void CmdFillBuffer(Buffer dstBuffer, long dstOffset, long size, int data)
        {
            vkCmdFillBuffer(this, dstBuffer, dstOffset, size, data);
        }

        /// <summary>
        /// Clear regions of a color image.
        /// <para>
        /// Each specified range in <paramref name="ranges"/> is cleared to the value specified by
        /// <paramref name="color"/>.
        /// </para>
        /// </summary>
        /// <param name="image">The image to be cleared.</param>
        /// <param name="imageLayout">
        /// Specifies the current layout of the image subresource ranges to be cleared, and must be
        /// <see cref="ImageLayout.General"/> or <see cref="ImageLayout.TransferDstOptimal"/>.
        /// </param>
        /// <param name="color">
        /// Contains the values the image subresource ranges will be cleared to.
        /// </param>
        /// <param name="ranges">
        /// Structures that describe a range of mipmap levels, array layers, and aspects to be
        /// cleared. The aspect mask of all image subresource ranges must only include <see cref="ImageAspects.Color"/>.
        /// </param>
        public void CmdClearColorImage(Image image, ImageLayout imageLayout,
            ClearColorValue color, params ImageSubresourceRange[] ranges)
        {
            fixed (ImageSubresourceRange* rangesPtr = ranges)
                vkCmdClearColorImage(this, image, imageLayout, &color, ranges?.Length ?? 0, rangesPtr);
        }

        /// <summary>
        /// Fill regions of a combined depth/stencil image.
        /// </summary>
        /// <param name="image">The image to be cleared.</param>
        /// <param name="imageLayout">
        /// Specifies the current layout of the image subresource ranges to be cleared, and must be
        /// <see cref="ImageLayout.General"/> or <see cref="ImageLayout.TransferDstOptimal"/>.
        /// </param>
        /// <param name="depthStencil">
        /// Structure that contains the values the depth and stencil image subresource ranges will be
        /// cleared to.
        /// </param>
        /// <param name="ranges">
        /// Structures that describe a range of mipmap levels, array layers, and aspects to be
        /// cleared. The aspect mask of each image subresource range in <paramref name="ranges"/> can
        /// include <see cref="ImageAspects.Depth"/> if the image format has a depth component,
        /// and <see cref="ImageAspects.Stencil"/> if the image format has a stencil component.
        /// </param>
        public void CmdClearDepthStencilImage(Image image, ImageLayout imageLayout,
            ClearDepthStencilValue depthStencil, params ImageSubresourceRange[] ranges)
        {
            fixed (ImageSubresourceRange* rangesPtr = ranges)
                vkCmdClearDepthStencilImage(this, image, imageLayout, &depthStencil, ranges?.Length ?? 0, rangesPtr);
        }

        /// <summary>
        /// Clear regions within currently bound framebuffer attachments.
        /// <para>
        /// Can clear multiple regions of each attachment used in the current subpass of a render
        /// pass instance. This command must be called only inside a render pass instance, and
        /// implicitly selects the images to clear based on the current framebuffer attachments and
        /// the command parameters.
        /// </para>
        /// </summary>
        /// <param name="attachments">
        /// Structures defining the attachments to clear and the clear values to use.
        /// </param>
        /// <param name="rects">
        /// Structures defining regions within each selected attachment to clear.
        /// </param>
        public void CmdClearAttachments(ClearAttachment[] attachments, ClearRect[] rects)
        {
            fixed (ClearAttachment* attachmentsPtr = attachments)
            fixed (ClearRect* rectsPtr = rects)
            {
                vkCmdClearAttachments(
                    this,
                    attachments?.Length ?? 0, attachmentsPtr,
                    rects?.Length ?? 0, rectsPtr);
            }
        }

        /// <summary>
        /// Resolve regions of an image.
        /// <para>
        /// During the resolve the samples corresponding to each pixel location in the source are
        /// converted to a single sample before being written to the destination. If the source
        /// formats are floating-point or normalized types, the sample values for each pixel are
        /// resolved in an implementation-dependent manner. If the source formats are integer types,
        /// a single sample’s value is selected for each pixel.
        /// </para>
        /// </summary>
        /// <param name="srcImage">The source image.</param>
        /// <param name="srcImageLayout">The layout of the source image subresources for the resolve.</param>
        /// <param name="dstImage">The destination image.</param>
        /// <param name="dstImageLayout">
        /// The layout of the destination image subresources for the resolve.
        /// </param>
        /// <param name="regions">Structures specifying the regions to resolve.</param>
        public void CmdResolveImage(Image srcImage, ImageLayout srcImageLayout, Image dstImage,
            ImageLayout dstImageLayout, params ImageResolve[] regions)
        {
            fixed (ImageResolve* regionsPtr = regions)
            {
                vkCmdResolveImage(
                    this,
                    srcImage, srcImageLayout,
                    dstImage, dstImageLayout,
                    regions?.Length ?? 0, regionsPtr);
            }
        }

        /// <summary>
        /// Set an event object to signaled state.
        /// <para>
        /// When <see cref="CmdSetEvent"/> is submitted to a queue, it defines an execution
        /// dependency on commands that were submitted before it, and defines an event signal
        /// operation which sets the event to the signaled state.
        /// </para>
        /// </summary>
        /// <param name="event">The event that will be signaled.</param>
        /// <param name="stageMask">
        /// Specifies the source stage mask used to determine when the <paramref name="event"/> is signaled.
        /// </param>
        public void CmdSetEvent(Event @event, PipelineStages stageMask)
        {
            vkCmdSetEvent(this, @event, stageMask);
        }

        /// <summary>
        /// Reset an event object to non-signaled state.
        /// <para>
        /// When <see cref="CmdResetEvent"/> is submitted to a queue, it defines an execution
        /// dependency on commands that were submitted before it, and defines an event unsignal
        /// operation which resets the event to the unsignaled state.
        /// </para>
        /// </summary>
        /// <param name="event">The event that will be unsignaled.</param>
        /// <param name="stageMask">
        /// Specifies the source stage mask used to determine when the <paramref name="event"/> is unsignaled.
        /// </param>
        public void CmdResetEvent(Event @event, PipelineStages stageMask)
        {
            vkCmdResetEvent(this, @event, stageMask);
        }

        /// <summary>
        /// Wait for one or more events and insert a set of memory.
        /// <para>
        /// When <see cref="CmdWaitEvents"/> is submitted to a queue, it defines a memory dependency
        /// between prior event signal operations, and subsequent commands.
        /// </para>
        /// </summary>
        /// <param name="events">Event object handles to wait on.</param>
        /// <param name="srcStageMask">The source stage mask.</param>
        /// <param name="dstStageMask">The destination stage mask.</param>
        /// <param name="memoryBarriers">An array of <see cref="MemoryBarrier"/> structures.</param>
        /// <param name="bufferMemoryBarriers">An array of <see cref="BufferMemoryBarrier"/> structures.</param>
        /// <param name="imageMemoryBarriers">An array of <see cref="ImageMemoryBarrier"/> structures.</param>
        public void CmdWaitEvents(Event[] events, PipelineStages srcStageMask, PipelineStages dstStageMask,
            MemoryBarrier[] memoryBarriers = null,
            BufferMemoryBarrier[] bufferMemoryBarriers = null,
            ImageMemoryBarrier[] imageMemoryBarriers = null)
        {
            PrepareBarriers(memoryBarriers, bufferMemoryBarriers, imageMemoryBarriers);

            int eventCount = events?.Length ?? 0;
            long* eventsHandles = stackalloc long[eventCount];
            for (int i = 0; i < eventCount; i++)
                eventsHandles[i] = events[i];

            fixed (MemoryBarrier* memoryBarriersPtr = memoryBarriers)
            fixed (BufferMemoryBarrier* bufferMemoryBarriersPtr = bufferMemoryBarriers)
            fixed (ImageMemoryBarrier* imageMemoryBarriersPtr = imageMemoryBarriers)
            {
                vkCmdWaitEvents(this, eventCount, eventsHandles, srcStageMask, dstStageMask, memoryBarriers?.Length ?? 0,
                    memoryBarriersPtr, bufferMemoryBarriers?.Length ?? 0, bufferMemoryBarriersPtr,
                    imageMemoryBarriers?.Length ?? 0, imageMemoryBarriersPtr);
            }
        }

        /// <summary>
        /// Wait for an event and insert a set of memory.
        /// <para>
        /// When <see cref="CmdWaitEvent"/> is submitted to a queue, it defines a memory dependency
        /// between prior event signal operations, and subsequent commands.
        /// </para>
        /// </summary>
        /// <param name="event">Event object handle to wait on.</param>
        /// <param name="srcStageMask">The source stage mask.</param>
        /// <param name="dstStageMask">The destination stage mask.</param>
        /// <param name="memoryBarriers">An array of <see cref="MemoryBarrier"/> structures.</param>
        /// <param name="bufferMemoryBarriers">An array of <see cref="BufferMemoryBarrier"/> structures.</param>
        /// <param name="imageMemoryBarriers">An array of <see cref="ImageMemoryBarrier"/> structures.</param>
        public void CmdWaitEvent(Event @event, PipelineStages srcStageMask, PipelineStages dstStageMask,
            MemoryBarrier[] memoryBarriers = null,
            BufferMemoryBarrier[] bufferMemoryBarriers = null,
            ImageMemoryBarrier[] imageMemoryBarriers = null)
        {
            PrepareBarriers(memoryBarriers, bufferMemoryBarriers, imageMemoryBarriers);

            long eventHandle = @event;

            fixed (MemoryBarrier* memoryBarriersPtr = memoryBarriers)
            fixed (BufferMemoryBarrier* bufferMemoryBarriersPtr = bufferMemoryBarriers)
            fixed (ImageMemoryBarrier* imageMemoryBarriersPtr = imageMemoryBarriers)
            {
                vkCmdWaitEvents(this, 1, &eventHandle, srcStageMask, dstStageMask, memoryBarriers?.Length ?? 0,
                    memoryBarriersPtr, bufferMemoryBarriers?.Length ?? 0, bufferMemoryBarriersPtr,
                    imageMemoryBarriers?.Length ?? 0, imageMemoryBarriersPtr);
            }
        }

        /// <summary>
        /// Insert a memory dependency.
        /// <para>
        /// When the command is submitted to a queue, it defines a memory dependency between commands
        /// that were submitted before it, and those submitted after it.
        /// </para>
        /// </summary>
        /// <param name="srcStageMask">Defines a source stage mask.</param>
        /// <param name="dstStageMask">Defines a destination stage mask.</param>
        /// <param name="dependencyFlags">a bitmask of <see cref="Dependencies"/>.</param>
        /// <param name="memoryBarriers">An array of <see cref="MemoryBarrier"/> structures.</param>
        /// <param name="bufferMemoryBarriers">An array of <see cref="BufferMemoryBarrier"/> structures.</param>
        /// <param name="imageMemoryBarriers">An array of <see cref="ImageMemoryBarrier"/> structures.</param>
        public void CmdPipelineBarrier(PipelineStages srcStageMask, PipelineStages dstStageMask, Dependencies dependencyFlags = 0,
            MemoryBarrier[] memoryBarriers = null,
            BufferMemoryBarrier[] bufferMemoryBarriers = null,
            ImageMemoryBarrier[] imageMemoryBarriers = null)
        {
            PrepareBarriers(memoryBarriers, bufferMemoryBarriers, imageMemoryBarriers);

            fixed (MemoryBarrier* memoryBarriersPtr = memoryBarriers)
            fixed (BufferMemoryBarrier* bufferMemoryBarriersPtr = bufferMemoryBarriers)
            fixed (ImageMemoryBarrier* imageMemoryBarriersPtr = imageMemoryBarriers)
            {
                vkCmdPipelineBarrier(this, srcStageMask, dstStageMask, dependencyFlags, memoryBarriers?.Length ?? 0,
                    memoryBarriersPtr, bufferMemoryBarriers?.Length ?? 0, bufferMemoryBarriersPtr,
                    imageMemoryBarriers?.Length ?? 0, imageMemoryBarriersPtr);
            }
        }

        /// <summary>
        /// Begin a query.
        /// </summary>
        /// <param name="queryPool">The query pool that will manage the results of the query.</param>
        /// <param name="query">The query index within the query pool that will contain the results.</param>
        /// <param name="flags">
        /// A bitmask indicating constraints on the types of queries that can be performed.
        /// </param>
        public void CmdBeginQuery(QueryPool queryPool, int query, QueryControlFlags flags = 0)
        {
            vkCmdBeginQuery(this, queryPool, query, flags);
        }

        /// <summary>
        /// Ends a query.
        /// <para>
        /// As queries operate asynchronously, ending a query does not immediately set the query’s
        /// status to available. A query is considered finished when the final results of the query
        /// are ready to be retrieved by <see cref="QueryPool.vkGetQueryPoolResults"/> and <see
        /// cref="CmdCopyQueryPoolResults"/>, and this is when the query’s status is set to available.
        /// </para>
        /// <para>
        /// Once a query is ended the query must finish in finite time, unless the state of the query
        /// is changed using other commands, e.g. by issuing a reset of the query.
        /// </para>
        /// </summary>
        /// <param name="queryPool">The query pool that is managing the results of the query.</param>
        /// <param name="query">The query index within the query pool where the result is stored.</param>
        public void CmdEndQuery(QueryPool queryPool, int query)
        {
            vkCmdEndQuery(this, queryPool, query);
        }

        /// <summary>
        /// Reset queries in a query pool.
        /// <para>
        /// When executed on a queue, this command sets the status of query indices [firstQuery,
        /// firstQuery + queryCount - 1] to unavailable.
        /// </para>
        /// </summary>
        /// <param name="queryPool">The handle of the query pool managing the queries being reset.</param>
        /// <param name="firstQuery">The initial query index to reset.</param>
        /// <param name="queryCount">The number of queries to reset.</param>
        public void CmdResetQueryPool(QueryPool queryPool, int firstQuery, int queryCount)
        {
            vkCmdResetQueryPool(this, queryPool, firstQuery, queryCount);
        }

        /// <summary>
        /// Write a device timestamp into a query object.
        /// <para>
        /// Latches the value of the timer when all previous commands have completed executing as far
        /// as the specified pipeline stage, and writes the timestamp value to memory. When the
        /// timestamp value is written, the availability status of the query is set to available.
        /// </para>
        /// </summary>
        /// <param name="pipelineStage">Specifies a stage of the pipeline.</param>
        /// <param name="queryPool">The query pool that will manage the timestamp.</param>
        /// <param name="query">The query within the query pool that will contain the timestamp.</param>
        public void CmdWriteTimestamp(PipelineStages pipelineStage, QueryPool queryPool, int query)
        {
            vkCmdWriteTimestamp(this, pipelineStage, queryPool, query);
        }

        /// <summary>
        /// Copy the results of queries in a query pool to a buffer object.
        /// <para>
        /// Is guaranteed to see the effect of previous uses of <see cref="CmdResetQueryPool"/> in
        /// the same queue, without any additional synchronization. Thus, the results will always
        /// reflect the most recent use of the query.
        /// </para>
        /// </summary>
        /// <param name="queryPool">
        /// The query pool managing the queries containing the desired results.
        /// </param>
        /// <param name="firstQuery">The initial query index.</param>
        /// <param name="queryCount">
        /// The number of queries. <paramref name="firstQuery"/> and <paramref name="queryCount"/>
        /// together define a range of queries.
        /// </param>
        /// <param name="dstBuffer">Buffer object that will receive the results of the copy command.</param>
        /// <param name="dstOffset">An offset into <paramref name="dstBuffer"/>.</param>
        /// <param name="stride">
        /// The stride in bytes between results for individual queries within <paramref
        /// name="dstBuffer"/>. The required size of the backing memory for <paramref
        /// name="dstBuffer"/> is determined as described above for <see cref="QueryPool.GetResults"/>.
        /// </param>
        /// <param name="flags">A bitmask specifying how and when results are returned.</param>
        public void CmdCopyQueryPoolResults(QueryPool queryPool, int firstQuery, int queryCount, Buffer dstBuffer,
            long dstOffset, long stride, QueryResults flags = 0)
        {
            vkCmdCopyQueryPoolResults(this, queryPool, firstQuery, queryCount, dstBuffer, dstOffset, stride, flags);
        }

        /// <summary>
        /// Update the values of push constants.
        /// </summary>
        /// <param name="layout">The pipeline layout used to program the push constant updates.</param>
        /// <param name="stageFlags">
        /// A bitmask specifying the shader stages that will use the push constants in the updated range.
        /// <para>
        /// Must match exactly the shader stages used in <paramref name="layout"/> for the range
        /// specified by <paramref name="offset"/> and <paramref name="size"/>.
        /// </para>
        /// </param>
        /// <param name="offset">
        /// The start offset of the push constant range to update, in units of bytes.
        /// <para>Must be a multiple of 4.</para>
        /// <para>Must be less than <see cref="PhysicalDeviceLimits.MaxPushConstantsSize"/>.</para>
        /// </param>
        /// <param name="size">
        /// The size of the push constant range to update, in units of bytes.
        /// <para>Must be a multiple of 4.</para>
        /// <para>
        /// Must be less than or equal to <see cref="PhysicalDeviceLimits.MaxPushConstantsSize"/>
        /// minus <paramref name="offset"/>.
        /// </para>
        /// </param>
        /// <param name="values">
        /// An array of <paramref name="size"/> bytes containing the new push constant values.
        /// </param>
        public void CmdPushConstants(PipelineLayout layout, ShaderStages stageFlags, int offset, int size,
            IntPtr values)
        {
            vkCmdPushConstants(this, layout, stageFlags, offset, size, values);
        }

        /// <summary>
        /// Begin a new render pass.
        /// <para>
        /// After beginning a render pass instance, the command buffer is ready to record the
        /// commands for the first subpass of that render pass.
        /// </para>
        /// </summary>
        /// <param name="beginInfo">
        /// Indicates the render pass to begin an instance of, and the framebuffer the instance uses.
        /// </param>
        /// <param name="contents">Specifies how the commands in the first subpass will be provided.</param>
        public void CmdBeginRenderPass(RenderPassBeginInfo beginInfo, SubpassContents contents = SubpassContents.Inline)
        {
            fixed (ClearValue* clearValuesPtr = beginInfo.ClearValues)
            {
                beginInfo.ToNative(out RenderPassBeginInfo.Native nativeBeginInfo, clearValuesPtr);
                vkCmdBeginRenderPass(this, &nativeBeginInfo, contents);
            }
        }

        /// <summary>
        /// Transition to the next subpass of a render pass.
        /// <para>
        /// The subpass index for a render pass begins at zero when <see cref="CmdBeginRenderPass"/>
        /// is recorded, and increments each time <see cref="CmdNextSubpass"/> is recorded.
        /// </para>
        /// <para>
        /// Moving to the next subpass automatically performs any multisample resolve operations in
        /// the subpass being ended. End-of-subpass multisample resolves are treated as color
        /// attachment writes for the purposes of synchronization. That is, they are considered to
        /// execute in the <see cref="PipelineStages.ColorAttachmentOutput"/> pipeline stage and
        /// their writes are synchronized with <see cref="Accesses.ColorAttachmentWrite"/>.
        /// Synchronization between rendering within a subpass and any resolve operations at the end
        /// of the subpass occurs automatically, without need for explicit dependencies or pipeline
        /// barriers. However, if the resolve attachment is also used in a different subpass, an
        /// explicit dependency is needed.
        /// </para>
        /// <para>
        /// After transitioning to the next subpass, the application can record the commands for that subpass.
        /// </para>
        /// </summary>
        /// <param name="contents">
        /// Specifies how the commands in the next subpass will be provided, in the same fashion as
        /// the corresponding parameter of <see cref="CmdBeginRenderPass"/>.
        /// </param>
        public void CmdNextSubpass(SubpassContents contents)
        {
            vkCmdNextSubpass(this, contents);
        }

        /// <summary>
        /// End the current render pass.
        /// <para>
        /// Ending a render pass instance performs any multisample resolve operations on the final subpass.
        /// </para>
        /// </summary>
        public void CmdEndRenderPass()
        {
            vkCmdEndRenderPass(this);
        }

        /// <summary>
        /// Execute a secondary command buffer from a primary command buffer.
        /// <para>
        /// A secondary command buffer must not be directly submitted to a queue. Instead, secondary
        /// command buffers are recorded to execute as part of a primary command buffer with this command.
        /// </para>
        /// </summary>
        /// <param name="commandBuffer">
        /// Secondary command buffer handle, which is recorded to execute in the primary command buffer.
        /// </param>
        public void CmdExecuteCommand(CommandBuffer commandBuffer)
        {
            IntPtr handle = commandBuffer;
            vkCmdExecuteCommands(this, 1, &handle);
        }

        /// <summary>
        /// Execute a secondary command buffer from a primary command buffer.
        /// <para>
        /// A secondary command buffer must not be directly submitted to a queue. Instead, secondary
        /// command buffers are recorded to execute as part of a primary command buffer with this command.
        /// </para>
        /// </summary>
        /// <param name="commandBuffers">
        /// Secondary command buffer handles, which are recorded to execute in the primary command
        /// buffer in the order they are listed in the array.
        /// </param>
        public void CmdExecuteCommands(params CommandBuffer[] commandBuffers)
        {
            int count = commandBuffers?.Length ?? 0;
            var commandBuffersPtr = stackalloc IntPtr[count];
            for (int i = 0; i < count; i++)
                commandBuffersPtr[i] = commandBuffers[i];

            vkCmdExecuteCommands(this, count, commandBuffersPtr);
        }

        /// <summary>
        /// Free command buffer.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed)
            {
                IntPtr handle = this;
                vkFreeCommandBuffers(Parent.Parent, Parent, 1, &handle);
            }
            base.Dispose();
        }

        internal static CommandBuffer[] Allocate(CommandPool parent, CommandBufferAllocateInfo* allocateInfo)
        {
            int count = allocateInfo->CommandBufferCount;
            var commandBuffersPtr = stackalloc IntPtr[count];
            allocateInfo->Prepare(parent);

            Result result = vkAllocateCommandBuffers(parent.Parent, allocateInfo, commandBuffersPtr);
            VulkanException.ThrowForInvalidResult(result);

            var commandBuffers = new CommandBuffer[count];
            for (int i = 0; i < count; i++)
                commandBuffers[i] = new CommandBuffer(parent, commandBuffersPtr[i]);
            return commandBuffers;
        }

        internal static void Free(CommandPool parent, CommandBuffer[] commandBuffers)
        {
            int count = commandBuffers?.Length ?? 0;
            var commandBuffersPtr = stackalloc IntPtr[count];
            for (int i = 0; i < count; i++)
                commandBuffersPtr[i] = commandBuffers[i];

            vkFreeCommandBuffers(
                parent.Parent,
                parent,
                count,
                commandBuffersPtr);
        }

        private static void PrepareBarriers(
            MemoryBarrier[] memoryBarriers,
            BufferMemoryBarrier[] bufferMemoryBarriers,
            ImageMemoryBarrier[] imageMemoryBarriers)
        {
            int memoryBarrierCount = memoryBarriers?.Length ?? 0;
            int bufferMemoryBarrierCount = bufferMemoryBarriers?.Length ?? 0;
            int imageMemoryBarrierCount = imageMemoryBarriers?.Length ?? 0;
            for (int i = 0; i < memoryBarrierCount; i++)
                memoryBarriers[i].Prepare();
            for (int i = 0; i < bufferMemoryBarrierCount; i++)
                bufferMemoryBarriers[i].Prepare();
            for (int i = 0; i < imageMemoryBarrierCount; i++)
                imageMemoryBarriers[i].Prepare();
        }

        private delegate Result vkAllocateCommandBuffersDelegate(IntPtr device, CommandBufferAllocateInfo* allocateInfo, IntPtr* commandBuffers);
        private static readonly vkAllocateCommandBuffersDelegate vkAllocateCommandBuffers = VulkanLibrary.GetStaticProc<vkAllocateCommandBuffersDelegate>(nameof(vkAllocateCommandBuffers));

        private delegate void vkFreeCommandBuffersDelegate(IntPtr device, long commandPool, int commandBufferCount, IntPtr* commandBuffers);
        private static readonly vkFreeCommandBuffersDelegate vkFreeCommandBuffers = VulkanLibrary.GetStaticProc<vkFreeCommandBuffersDelegate>(nameof(vkFreeCommandBuffers));

        private delegate Result vkBeginCommandBufferDelegate(IntPtr commandBuffer, CommandBufferBeginInfo.Native* beginInfo);
        private static readonly vkBeginCommandBufferDelegate vkBeginCommandBuffer = VulkanLibrary.GetStaticProc<vkBeginCommandBufferDelegate>(nameof(vkBeginCommandBuffer));

        private delegate Result vkEndCommandBufferDelegate(IntPtr commandBuffer);
        private static readonly vkEndCommandBufferDelegate vkEndCommandBuffer = VulkanLibrary.GetStaticProc<vkEndCommandBufferDelegate>(nameof(vkEndCommandBuffer));

        private delegate Result vkResetCommandBufferDelegate(IntPtr commandBuffer, CommandBufferResetFlags flags);
        private static readonly vkResetCommandBufferDelegate vkResetCommandBuffer = VulkanLibrary.GetStaticProc<vkResetCommandBufferDelegate>(nameof(vkResetCommandBuffer));

        private delegate void vkCmdBindPipelineDelegate(IntPtr commandBuffer, PipelineBindPoint pipelineBindPoint, long pipeline);
        private static readonly vkCmdBindPipelineDelegate vkCmdBindPipeline = VulkanLibrary.GetStaticProc<vkCmdBindPipelineDelegate>(nameof(vkCmdBindPipeline));

        private delegate void vkCmdSetViewportDelegate(IntPtr commandBuffer, int firstViewport, int viewportCount, Viewport* viewports);
        private static readonly vkCmdSetViewportDelegate vkCmdSetViewport = VulkanLibrary.GetStaticProc<vkCmdSetViewportDelegate>(nameof(vkCmdSetViewport));

        private delegate void vkCmdSetScissorDelegate(IntPtr commandBuffer, int firstScissor, int scissorCount, Rect2D* scissors);
        private static readonly vkCmdSetScissorDelegate vkCmdSetScissor = VulkanLibrary.GetStaticProc<vkCmdSetScissorDelegate>(nameof(vkCmdSetScissor));

        private delegate void vkCmdSetLineWidthDelegate(IntPtr commandBuffer, float lineWidth);
        private static readonly vkCmdSetLineWidthDelegate vkCmdSetLineWidth = VulkanLibrary.GetStaticProc<vkCmdSetLineWidthDelegate>(nameof(vkCmdSetLineWidth));

        private delegate void vkCmdSetDepthBiasDelegate(IntPtr commandBuffer, float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor);
        private static readonly vkCmdSetDepthBiasDelegate vkCmdSetDepthBias = VulkanLibrary.GetStaticProc<vkCmdSetDepthBiasDelegate>(nameof(vkCmdSetDepthBias));

        private delegate void vkCmdSetBlendConstantsDelegate(IntPtr commandBuffer, ColorF4 blendConstants);
        private static readonly vkCmdSetBlendConstantsDelegate vkCmdSetBlendConstants = VulkanLibrary.GetStaticProc<vkCmdSetBlendConstantsDelegate>(nameof(vkCmdSetBlendConstants));

        private delegate void vkCmdSetDepthBoundsDelegate(IntPtr commandBuffer, float minDepthBounds, float maxDepthBounds);
        private static readonly vkCmdSetDepthBoundsDelegate vkCmdSetDepthBounds = VulkanLibrary.GetStaticProc<vkCmdSetDepthBoundsDelegate>(nameof(vkCmdSetDepthBounds));

        private delegate void vkCmdSetStencilCompareMaskDelegate(IntPtr commandBuffer, StencilFaces faceMask, int compareMask);
        private static readonly vkCmdSetStencilCompareMaskDelegate vkCmdSetStencilCompareMask = VulkanLibrary.GetStaticProc<vkCmdSetStencilCompareMaskDelegate>(nameof(vkCmdSetStencilCompareMask));

        private delegate void vkCmdSetStencilWriteMaskDelegate(IntPtr commandBuffer, StencilFaces faceMask, int writeMask);
        private static readonly vkCmdSetStencilWriteMaskDelegate vkCmdSetStencilWriteMask = VulkanLibrary.GetStaticProc<vkCmdSetStencilWriteMaskDelegate>(nameof(vkCmdSetStencilWriteMask));

        private delegate void vkCmdSetStencilReferenceDelegate(IntPtr commandBuffer, StencilFaces faceMask, int reference);
        private static readonly vkCmdSetStencilReferenceDelegate vkCmdSetStencilReference = VulkanLibrary.GetStaticProc<vkCmdSetStencilReferenceDelegate>(nameof(vkCmdSetStencilReference));

        private delegate void vkCmdBindDescriptorSetsDelegate(IntPtr commandBuffer, PipelineBindPoint pipelineBindPoint, long layout, int firstSet, int descriptorSetCount, long* descriptorSets, int dynamicOffsetCount, int* dynamicOffsets);
        private static readonly vkCmdBindDescriptorSetsDelegate vkCmdBindDescriptorSets = VulkanLibrary.GetStaticProc<vkCmdBindDescriptorSetsDelegate>(nameof(vkCmdBindDescriptorSets));

        private delegate void vkCmdBindVertexBuffersDelegate(IntPtr commandBuffer, int firstBinding, int bindingCount, long* buffers, long* offsets);
        private static readonly vkCmdBindVertexBuffersDelegate vkCmdBindVertexBuffers = VulkanLibrary.GetStaticProc<vkCmdBindVertexBuffersDelegate>(nameof(vkCmdBindVertexBuffers));

        private delegate void vkCmdDrawDelegate(IntPtr commandBuffer, int vertexCount, int instanceCount, int firstVertex, int firstInstance);
        private static readonly vkCmdDrawDelegate vkCmdDraw = VulkanLibrary.GetStaticProc<vkCmdDrawDelegate>(nameof(vkCmdDraw));

        private delegate void vkCmdDrawIndexedDelegate(IntPtr commandBuffer, int indexCount, int instanceCount, int firstIndex, int vertexOffset, int firstInstance);
        private static readonly vkCmdDrawIndexedDelegate vkCmdDrawIndexed = VulkanLibrary.GetStaticProc<vkCmdDrawIndexedDelegate>(nameof(vkCmdDrawIndexed));

        private delegate void vkCmdDispatchDelegate(IntPtr commandBuffer, int groupCountX, int groupCountY, int groupCountZ);
        private static readonly vkCmdDispatchDelegate vkCmdDispatch = VulkanLibrary.GetStaticProc<vkCmdDispatchDelegate>(nameof(vkCmdDispatch));

        private delegate void vkCmdClearAttachmentsDelegate(IntPtr commandBuffer, int attachmentCount, ClearAttachment* attachments, int rectCount, ClearRect* rects);
        private static readonly vkCmdClearAttachmentsDelegate vkCmdClearAttachments = VulkanLibrary.GetStaticProc<vkCmdClearAttachmentsDelegate>(nameof(vkCmdClearAttachments));

        private delegate void vkCmdWaitEventsDelegate(IntPtr commandBuffer, int eventCount, long* events, PipelineStages srcStageMask, PipelineStages dstStageMask, int memoryBarrierCount, MemoryBarrier* memoryBarriers, int bufferMemoryBarrierCount, BufferMemoryBarrier* bufferMemoryBarriers, int imageMemoryBarrierCount, ImageMemoryBarrier* imageMemoryBarriers);
        private static readonly vkCmdWaitEventsDelegate vkCmdWaitEvents = VulkanLibrary.GetStaticProc<vkCmdWaitEventsDelegate>(nameof(vkCmdWaitEvents));

        private delegate void vkCmdPipelineBarrierDelegate(IntPtr commandBuffer, PipelineStages srcStageMask, PipelineStages dstStageMask, Dependencies dependencyFlags, int memoryBarrierCount, MemoryBarrier* memoryBarriers, int bufferMemoryBarrierCount, BufferMemoryBarrier* bufferMemoryBarriers, int imageMemoryBarrierCount, ImageMemoryBarrier* imageMemoryBarriers);
        private static readonly vkCmdPipelineBarrierDelegate vkCmdPipelineBarrier = VulkanLibrary.GetStaticProc<vkCmdPipelineBarrierDelegate>(nameof(vkCmdPipelineBarrier));

        private delegate void vkCmdWriteTimestampDelegate(IntPtr commandBuffer, PipelineStages pipelineStage, long queryPool, int query);
        private static readonly vkCmdWriteTimestampDelegate vkCmdWriteTimestamp = VulkanLibrary.GetStaticProc<vkCmdWriteTimestampDelegate>(nameof(vkCmdWriteTimestamp));

        private delegate void vkCmdBeginRenderPassDelegate(IntPtr commandBuffer, RenderPassBeginInfo.Native* renderPassBegin, SubpassContents contents);
        private static readonly vkCmdBeginRenderPassDelegate vkCmdBeginRenderPass = VulkanLibrary.GetStaticProc<vkCmdBeginRenderPassDelegate>(nameof(vkCmdBeginRenderPass));

        private delegate void vkCmdNextSubpassDelegate(IntPtr commandBuffer, SubpassContents contents);
        private static readonly vkCmdNextSubpassDelegate vkCmdNextSubpass = VulkanLibrary.GetStaticProc<vkCmdNextSubpassDelegate>(nameof(vkCmdNextSubpass));

        private delegate void vkCmdEndRenderPassDelegate(IntPtr commandBuffer);
        private static readonly vkCmdEndRenderPassDelegate vkCmdEndRenderPass = VulkanLibrary.GetStaticProc<vkCmdEndRenderPassDelegate>(nameof(vkCmdEndRenderPass));

        private delegate void vkCmdExecuteCommandsDelegate(IntPtr commandBuffer, int commandBufferCount, IntPtr* commandBuffers);
        private static readonly vkCmdExecuteCommandsDelegate vkCmdExecuteCommands = VulkanLibrary.GetStaticProc<vkCmdExecuteCommandsDelegate>(nameof(vkCmdExecuteCommands));

        private delegate void vkCmdBindIndexBufferDelegate(IntPtr commandBuffer, long buffer, long offset, IndexType indexType);
        private static readonly vkCmdBindIndexBufferDelegate vkCmdBindIndexBuffer = VulkanLibrary.GetStaticProc<vkCmdBindIndexBufferDelegate>(nameof(vkCmdBindIndexBuffer));

        private delegate void vkCmdDrawIndirectDelegate(IntPtr commandBuffer, long buffer, long offset, int drawCount, int stride);
        private static readonly vkCmdDrawIndirectDelegate vkCmdDrawIndirect = VulkanLibrary.GetStaticProc<vkCmdDrawIndirectDelegate>(nameof(vkCmdDrawIndirect));

        private delegate void vkCmdDrawIndexedIndirectDelegate(IntPtr commandBuffer, long buffer, long offset, int drawCount, int stride);
        private static readonly vkCmdDrawIndexedIndirectDelegate vkCmdDrawIndexedIndirect = VulkanLibrary.GetStaticProc<vkCmdDrawIndexedIndirectDelegate>(nameof(vkCmdDrawIndexedIndirect));

        private delegate void vkCmdDispatchIndirectDelegate(IntPtr commandBuffer, long buffer, long offset);
        private static readonly vkCmdDispatchIndirectDelegate vkCmdDispatchIndirect = VulkanLibrary.GetStaticProc<vkCmdDispatchIndirectDelegate>(nameof(vkCmdDispatchIndirect));

        private delegate void vkCmdCopyBufferDelegate(IntPtr commandBuffer, long srcBuffer, long dstBuffer, int regionCount, BufferCopy* regions);
        private static readonly vkCmdCopyBufferDelegate vkCmdCopyBuffer = VulkanLibrary.GetStaticProc<vkCmdCopyBufferDelegate>(nameof(vkCmdCopyBuffer));

        private delegate void vkCmdUpdateBufferDelegate(IntPtr commandBuffer, long dstBuffer, long dstOffset, long dataSize, IntPtr data);
        private static readonly vkCmdUpdateBufferDelegate vkCmdUpdateBuffer = VulkanLibrary.GetStaticProc<vkCmdUpdateBufferDelegate>(nameof(vkCmdUpdateBuffer));

        private delegate void vkCmdFillBufferDelegate(IntPtr commandBuffer, long dstBuffer, long dstOffset, long size, int data);
        private static readonly vkCmdFillBufferDelegate vkCmdFillBuffer = VulkanLibrary.GetStaticProc<vkCmdFillBufferDelegate>(nameof(vkCmdFillBuffer));

        private delegate void vkCmdCopyImageDelegate(IntPtr commandBuffer, long srcImage, ImageLayout srcImageLayout, long dstImage, ImageLayout dstImageLayout, int regionCount, ImageCopy* regions);
        private static readonly vkCmdCopyImageDelegate vkCmdCopyImage = VulkanLibrary.GetStaticProc<vkCmdCopyImageDelegate>(nameof(vkCmdCopyImage));

        private delegate void vkCmdBlitImageDelegate(IntPtr commandBuffer, long srcImage, ImageLayout srcImageLayout, long dstImage, ImageLayout dstImageLayout, int regionCount, ImageBlit* regions, Filter filter);
        private static readonly vkCmdBlitImageDelegate vkCmdBlitImage = VulkanLibrary.GetStaticProc<vkCmdBlitImageDelegate>(nameof(vkCmdBlitImage));

        private delegate void vkCmdCopyImageToBufferDelegate(IntPtr commandBuffer, long srcImage, ImageLayout srcImageLayout, long dstBuffer, int regionCount, BufferImageCopy* regions);
        private static readonly vkCmdCopyImageToBufferDelegate vkCmdCopyImageToBuffer = VulkanLibrary.GetStaticProc<vkCmdCopyImageToBufferDelegate>(nameof(vkCmdCopyImageToBuffer));

        private delegate void vkCmdResolveImageDelegate(IntPtr commandBuffer, long srcImage, ImageLayout srcImageLayout, long dstImage, ImageLayout dstImageLayout, int regionCount, ImageResolve* regions);
        private static readonly vkCmdResolveImageDelegate vkCmdResolveImage = VulkanLibrary.GetStaticProc<vkCmdResolveImageDelegate>(nameof(vkCmdResolveImage));

        private delegate void vkCmdCopyBufferToImageDelegate(IntPtr commandBuffer, long srcBuffer, long dstImage, ImageLayout dstImageLayout, int regionCount, BufferImageCopy* regions);
        private static readonly vkCmdCopyBufferToImageDelegate vkCmdCopyBufferToImage = VulkanLibrary.GetStaticProc<vkCmdCopyBufferToImageDelegate>(nameof(vkCmdCopyBufferToImage));

        private delegate void vkCmdPushConstantsDelegate(IntPtr commandBuffer, long layout, ShaderStages stageFlags, int offset, int size, IntPtr values);
        private static readonly vkCmdPushConstantsDelegate vkCmdPushConstants = VulkanLibrary.GetStaticProc<vkCmdPushConstantsDelegate>(nameof(vkCmdPushConstants));

        private delegate void vkCmdClearColorImageDelegate(IntPtr commandBuffer, long image, ImageLayout imageLayout, ClearColorValue* color, int rangeCount, ImageSubresourceRange* ranges);
        private static readonly vkCmdClearColorImageDelegate vkCmdClearColorImage = VulkanLibrary.GetStaticProc<vkCmdClearColorImageDelegate>(nameof(vkCmdClearColorImage));

        private delegate void vkCmdClearDepthStencilImageDelegate(IntPtr commandBuffer, long image, ImageLayout imageLayout, ClearDepthStencilValue* depthStencil, int rangeCount, ImageSubresourceRange* ranges);
        private static readonly vkCmdClearDepthStencilImageDelegate vkCmdClearDepthStencilImage = VulkanLibrary.GetStaticProc<vkCmdClearDepthStencilImageDelegate>(nameof(vkCmdClearDepthStencilImage));

        private delegate void vkCmdSetEventDelegate(IntPtr commandBuffer, long @event, PipelineStages stageMask);
        private static readonly vkCmdSetEventDelegate vkCmdSetEvent = VulkanLibrary.GetStaticProc<vkCmdSetEventDelegate>(nameof(vkCmdSetEvent));

        private delegate void vkCmdResetEventDelegate(IntPtr commandBuffer, long @event, PipelineStages stageMask);
        private static readonly vkCmdResetEventDelegate vkCmdResetEvent = VulkanLibrary.GetStaticProc<vkCmdResetEventDelegate>(nameof(vkCmdResetEvent));

        private delegate void vkCmdBeginQueryDelegate(IntPtr commandBuffer, long queryPool, int query, QueryControlFlags flags);
        private static readonly vkCmdBeginQueryDelegate vkCmdBeginQuery = VulkanLibrary.GetStaticProc<vkCmdBeginQueryDelegate>(nameof(vkCmdBeginQuery));

        private delegate void vkCmdEndQueryDelegate(IntPtr commandBuffer, long queryPool, int query);
        private static readonly vkCmdEndQueryDelegate vkCmdEndQuery = VulkanLibrary.GetStaticProc<vkCmdEndQueryDelegate>(nameof(vkCmdEndQuery));

        private delegate void vkCmdResetQueryPoolDelegate(IntPtr commandBuffer, long queryPool, int firstQuery, int queryCount);
        private static readonly vkCmdResetQueryPoolDelegate vkCmdResetQueryPool = VulkanLibrary.GetStaticProc<vkCmdResetQueryPoolDelegate>(nameof(vkCmdResetQueryPool));

        private delegate void vkCmdCopyQueryPoolResultsDelegate(IntPtr commandBuffer, long queryPool, int firstQuery, int queryCount, long dstBuffer, long dstOffset, long stride, QueryResults flags);
        private static readonly vkCmdCopyQueryPoolResultsDelegate vkCmdCopyQueryPoolResults = VulkanLibrary.GetStaticProc<vkCmdCopyQueryPoolResultsDelegate>(nameof(vkCmdCopyQueryPoolResults));
    }

    /// <summary>
    /// Structure specifying the allocation parameters for command buffer object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CommandBufferAllocateInfo
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal long CommandPool;

        /// <summary>
        /// Specifies the command buffer level.
        /// </summary>
        public CommandBufferLevel Level;
        /// <summary>
        /// The number of command buffers to allocate from the pool.
        /// </summary>
        public int CommandBufferCount;

        /// <summary>
        /// Inititializes a new instance of the <see cref="CommandBuffer"/> structure.
        /// </summary>
        /// <param name="level">Specifies the command buffer level.</param>
        /// <param name="count">The number of command buffers to allocate from the pool.</param>
        public CommandBufferAllocateInfo(CommandBufferLevel level, int count)
        {
            Type = StructureType.CommandBufferAllocateInfo;
            Next = IntPtr.Zero;
            CommandPool = 0;
            Level = level;
            CommandBufferCount = count;
        }

        internal void Prepare(CommandPool pool)
        {
            Type = StructureType.CommandBufferAllocateInfo;
            CommandPool = pool;
        }
    }

    /// <summary>
    /// Enumerant specifying a command buffer level.
    /// </summary>
    public enum CommandBufferLevel
    {
        /// <summary>
        /// Specifies a primary command buffer.
        /// </summary>
        Primary = 0,
        /// <summary>
        /// Specifies a secondary command buffer.
        /// </summary>
        Secondary = 1
    }

    /// <summary>
    /// Structure specifying a command buffer begin operation.
    /// </summary>
    public unsafe struct CommandBufferBeginInfo
    {
        /// <summary>
        /// A bitmask specifying usage behavior for the command buffer.
        /// </summary>
        public CommandBufferUsages Flags;
        /// <summary>
        /// Structure, which is used if command buffer is a secondary command buffer.
        /// <para>If it is a primary command buffer, then this value is ignored.</para>
        /// </summary>
        public CommandBufferInheritanceInfo? InheritanceInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBufferBeginInfo"/> structure.
        /// </summary>
        /// <param name="flags">A bitmask specifying usage behavior for the command buffer.</param>
        /// <param name="inheritanceInfo">
        /// Structure, which is used if command buffer is a secondary command buffer.
        /// <para>If it is a primary command buffer, then this value is ignored.</para>
        /// </param>
        public CommandBufferBeginInfo(CommandBufferUsages flags = 0,
            CommandBufferInheritanceInfo? inheritanceInfo = null)
        {
            Flags = flags;
            InheritanceInfo = inheritanceInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public CommandBufferUsages Flags;
            public CommandBufferInheritanceInfo* InheritanceInfo;

            public void Free()
            {
                Interop.Free(InheritanceInfo);
            }
        }

        internal void ToNative(out Native native)
        {
            var inheritanceInfo = (CommandBufferInheritanceInfo*)Interop.Struct.AllocToPointer(ref InheritanceInfo);
            if (InheritanceInfo.HasValue)
                inheritanceInfo->Prepare();

            native.Type = StructureType.CommandBufferBeginInfo;
            native.Next = IntPtr.Zero;
            native.Flags = Flags;
            native.InheritanceInfo = inheritanceInfo;
        }
    }

    /// <summary>
    /// Bitmask specifying usage behavior for command buffer.
    /// </summary>
    [Flags]
    public enum CommandBufferUsages
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that each recording of the command buffer will only be submitted once, and the
        /// command buffer will be reset and recorded again between each submission.
        /// </summary>
        OneTimeSubmit = 1 << 0,
        /// <summary>
        /// Specifies that a secondary command buffer is considered to be entirely inside a render pass.
        /// <para>If this is a primary command buffer, then this bit is ignored.</para>
        /// </summary>
        RenderPassContinue = 1 << 1,
        /// <summary>
        /// Specifies that a command buffer can be resubmitted to a queue while it is in the pending
        /// State, and recorded into multiple primary command buffers.
        /// </summary>
        SimultaneousUse = 1 << 2
    }

    /// <summary>
    /// Structure specifying command buffer inheritance info.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CommandBufferInheritanceInfo
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// A <see cref="VulkanCore.RenderPass"/> object defining which render passes the <see
        /// cref="CommandBuffer"/> will be compatible with and can be executed within. If the <see
        /// cref="CommandBuffer"/> will not be executed within a render pass instance, <see
        /// cref="RenderPass"/> is ignored.
        /// </summary>
        public long RenderPass;
        /// <summary>
        /// The index of the subpass within the render pass instance that the <see
        /// cref="CommandBuffer"/> will be executed within. If the <see cref="CommandBuffer"/> will
        /// not be executed within a render pass instance, subpass is ignored.
        /// </summary>
        public int Subpass;
        /// <summary>
        /// Optionally refers to the <see cref="VulkanCore.Framebuffer"/> object that the <see
        /// cref="CommandBuffer"/> will be rendering to if it is executed within a render pass
        /// instance. It can be 0 if the framebuffer is not known, or if the <see
        /// cref="CommandBuffer"/> will not be executed within a render pass instance.
        /// <para>
        /// Specifying the exact framebuffer that the secondary command buffer will be executed with
        /// may result in better performance at command buffer execution time.
        /// </para>
        /// </summary>
        public long Framebuffer;
        /// <summary>
        /// Indicates whether the command buffer can be executed while an occlusion query is active
        /// in the primary command buffer. If this is <c>true</c>, then this command buffer can be
        /// executed whether the primary command buffer has an occlusion query active or not. If this
        /// is <c>false</c>, then the primary command buffer must not have an occlusion query active.
        /// </summary>
        public Bool OcclusionQueryEnable;
        /// <summary>
        /// Indicates the query flags that can be used by an active occlusion query in the primary
        /// command buffer when this secondary command buffer is executed. If this value includes the
        /// <see cref="QueryControlFlags.Precise"/> bit, then the active query can return boolean
        /// results or actual sample counts. If this bit is not set, then the active query must not
        /// use the <see cref="QueryControlFlags.Precise"/> bit.
        /// </summary>
        public QueryControlFlags QueryFlags;
        /// <summary>
        /// Specifies the set of pipeline statistics that can be counted by an active query in the
        /// primary command buffer when this secondary command buffer is executed. If this value
        /// includes a given bit, then this command buffer can be executed whether the primary
        /// command buffer has a pipeline statistics query active that includes this bit or not. If
        /// this value excludes a given bit, then the active pipeline statistics query must not be
        /// from a query pool that counts that statistic.
        /// </summary>
        public QueryPipelineStatistics PipelineStatistics;

        internal void Prepare()
        {
            Type = StructureType.CommandBufferInheritanceInfo;
        }
    }

    /// <summary>
    /// Bitmask specifying constraints on a query.
    /// </summary>
    [Flags]
    public enum QueryControlFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies the precision of occlusion queries.
        /// </summary>
        Precise = 1 << 0
    }

    /// <summary>
    /// Bitmask specifying queried pipeline statistics.
    /// </summary>
    [Flags]
    public enum QueryPipelineStatistics
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that queries managed by the pool will count the number of vertices processed by
        /// the input assembly stage.
        /// <para>Vertices corresponding to incomplete primitives may contribute to the count.</para>
        /// </summary>
        InputAssemblyVertices = 1 << 0,
        /// <summary>
        /// Specifies that queries managed by the pool will count the number of primitives processed
        /// by the input assembly stage.
        /// <para>
        /// If primitive restart is enabled, restarting the primitive topology has no effect on the count.
        /// </para>
        /// <para>Incomplete primitives may be counted.</para>
        /// </summary>
        InputAssemblyPrimitives = 1 << 1,
        /// <summary>
        /// Specifies that queries managed by the pool will count the number of vertex shader invocations.
        /// <para>This counter's value is incremented each time a vertex shader is invoked.</para>
        /// </summary>
        VertexShaderInvocations = 1 << 2,
        /// <summary>
        /// Specifies that queries managed by the pool will count the number of geometry shader invocations.
        /// <para>This counter's value is incremented each time a geometry shader is invoked.</para>
        /// <para>
        /// In the case of instanced geometry shaders, the geometry shader invocations count is
        /// incremented for each separate instanced invocation.
        /// </para>
        /// </summary>
        GeometryShaderInvocations = 1 << 3,
        /// <summary>
        /// Specifies that queries managed by the pool will count the number of primitives generated
        /// by geometry shader invocations.
        /// <para>The counter's value is incremented each time the geometry shader emits a primitive.</para>
        /// <para>
        /// Restarting primitive topology using the SPIR-V instructions <c>OpEndPrimitive</c> or
        /// <c>OpEndStreamPrimitive</c> has no effect on the geometry shader output primitives count.
        /// </para>
        /// </summary>
        GeometryShaderPrimitives = 1 << 4,
        /// <summary>
        /// Specifies that queries managed by the pool will count the number of primitives processed
        /// by the primitive clipping stage of the pipeline.
        /// <para>
        /// The counter's value is incremented each time a primitive reaches the primitive clipping stage.
        /// </para>
        /// </summary>
        ClippingInvocations = 1 << 5,
        /// <summary>
        /// Specifies that queries managed by the pool will count the number of primitives output by
        /// the primitive clipping stage of the pipeline.
        /// <para>
        /// The counter's value is incremented each time a primitive passes the primitive clipping stage.
        /// </para>
        /// <para>
        /// The actual number of primitives output by the primitive clipping stage for a particular
        /// input primitive is implementation-dependent but must satisfy the following conditions:
        /// </para>
        /// <para>
        /// ** If at least one vertex of the input primitive lies inside the clipping volume, the
        /// counter is incremented by one or more.
        /// </para>
        /// <para>** Otherwise, the counter is incremented by zero or more.</para>
        /// </summary>
        ClippingPrimitives = 1 << 6,
        /// <summary>
        /// Specifies that queries managed by the pool will count the number of fragment shader invocations.
        /// <para>The counter's value is incremented each time the fragment shader is invoked.</para>
        /// </summary>
        FragmentShaderInvocations = 1 << 7,
        /// <summary>
        /// Specifies that queries managed by the pool will count the number of patches processed by
        /// the tessellation control shader.
        /// <para>
        /// The counter's value is incremented once for each patch for which a tessellation control
        /// shader is invoked.
        /// </para>
        /// </summary>
        TessellationControlShaderPatches = 1 << 8,
        /// <summary>
        /// Specifies that queries managed by the pool will count the number of invocations of the
        /// tessellation evaluation shader.
        /// <para>
        /// The counter's value is incremented each time the tessellation evaluation shader is invoked.
        /// </para>
        /// </summary>
        TessellationEvaluationShaderInvocations = 1 << 9,
        /// <summary>
        /// Specifies that queries managed by the pool will count the number of compute shader invocations.
        /// <para>The counter's value is incremented every time the compute shader is invoked.</para>
        /// <para>
        /// Implementations may skip the execution of certain compute shader invocations or execute
        /// additional compute shader invocations for implementation-dependent reasons as long as the
        /// results of rendering otherwise remain unchanged.
        /// </para>
        /// </summary>
        ComputeShaderInvocations = 1 << 10
    }

    /// <summary>
    /// Bitmask controlling behavior of a command buffer reset.
    /// </summary>
    [Flags]
    public enum CommandBufferResetFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that most or all memory resources currently owned by the command buffer should
        /// be returned to the parent command pool.
        /// <para>
        /// If this flag is not set, then the command buffer may hold onto memory resources and reuse
        /// them when recording commands.
        /// </para>
        /// <para>Command buffer is moved to the initial state.</para>
        /// </summary>
        ReleaseResources = 1 << 0
    }

    /// <summary>
    /// Type of index buffer indices.
    /// </summary>
    public enum IndexType
    {
        /// <summary>
        /// Specifies that indices are 16-bit unsigned integer values.
        /// </summary>
        UInt16 = 0,
        /// <summary>
        /// Specifies that indices are 32-bit unsigned integer values.
        /// </summary>
        UInt32 = 1
    }

    /// <summary>
    /// Bitmask specifying sets of stencil state for which to update the compare mask.
    /// </summary>
    [Flags]
    public enum StencilFaces
    {
        /// <summary>
        /// Specifies that only the front set of stencil state is updated.
        /// </summary>
        Front = 1 << 0,
        /// <summary>
        /// Specifies that only the back set of stencil state is updated.
        /// </summary>
        Back = 1 << 1,
        /// <summary>
        /// Is the combination of <see cref="Front"/> and <see cref="Back"/>, and specifies that both
        /// sets of stencil state are updated.
        /// </summary>
        StencilFrontAndBack = 0x00000003
    }

    /// <summary>
    /// Structure specifying a buffer copy operation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BufferCopy
    {
        /// <summary>
        /// The starting offset in bytes from the start of source buffer.
        /// </summary>
        public long SrcOffset;
        /// <summary>
        /// The starting offset in bytes from the start of destination buffer.
        /// </summary>
        public long DstOffset;
        /// <summary>
        /// The number of bytes to copy.
        /// </summary>
        public long Size;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferCopy"/> structure.
        /// </summary>
        /// <param name="srcOffset">The starting offset in bytes from the start of source buffer.</param>
        /// <param name="dstOffset">The starting offset in bytes from the start of destination buffer.</param>
        /// <param name="size">The number of bytes to copy.</param>
        public BufferCopy(long size, long srcOffset = 0, long dstOffset = 0)
        {
            SrcOffset = srcOffset;
            DstOffset = dstOffset;
            Size = size;
        }
    }

    /// <summary>
    /// Structure specifying an image copy operation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageCopy
    {
        /// <summary>
        /// Specifies the image subresource of the image used for the source image data.
        /// </summary>
        public ImageSubresourceLayers SrcSubresource;
        /// <summary>
        /// Selects the initial <c>X</c>, <c>Y</c>, and <c>Z</c> offsets in texels of the sub-region
        /// of the source image data.
        /// </summary>
        public Offset3D SrcOffset;
        /// <summary>
        /// Specifies the image subresource of the image used for the destination image data.
        /// </summary>
        public ImageSubresourceLayers DstSubresource;
        /// <summary>
        /// Selects the initial <c>X</c>, <c>Y</c>, and <c>Z</c> offsets in texels of the sub-region
        /// of the source image data.
        /// </summary>
        public Offset3D DstOffset;
        /// <summary>
        /// The size in texels of the image to copy in <c>Width</c>, <c>Height</c> and <c>Depth</c>.
        /// </summary>
        public Extent3D Extent;
    }

    /// <summary>
    /// Structure specifying a image subresource layers.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageSubresourceLayers
    {
        /// <summary>
        /// A combination of <see cref="ImageAspects"/> selecting the color, depth and/or stencil
        /// aspects to be copied.
        /// <para>Must not contain <see cref="ImageAspects.Metadata"/>.</para>
        /// </summary>
        public ImageAspects AspectMask;
        /// <summary>
        /// The mipmap level to copy from.
        /// <para>
        /// Must be less than the specified <see cref="ImageCreateInfo.MipLevels"/> when the image
        /// was created.
        /// </para>
        /// </summary>
        public int MipLevel;
        /// <summary>
        /// The starting layer.
        /// <para>
        /// Must be less than or equal to the arrayLayers specified in <see cref="ImageCreateInfo"/>
        /// when the image was created.
        /// </para>
        /// </summary>
        public int BaseArrayLayer;
        /// <summary>
        /// The number of layers to copy.
        /// </summary>
        public int LayerCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSubresourceLayers"/> structure.
        /// </summary>
        /// <param name="aspectMask">
        /// A combination of <see cref="ImageAspects"/> selecting the color, depth and/or stencil
        /// aspects to be copied.
        /// </param>
        /// <param name="mipLevel">The mipmap level to copy from.</param>
        /// <param name="baseArrayLayer">The starting layer.</param>
        /// <param name="layerCount">The number of layers to copy.</param>
        public ImageSubresourceLayers(ImageAspects aspectMask, int mipLevel, int baseArrayLayer, int layerCount)
        {
            AspectMask = aspectMask;
            MipLevel = mipLevel;
            BaseArrayLayer = baseArrayLayer;
            LayerCount = layerCount;
        }
    }

    /// <summary>
    /// Structure specifying an image blit operation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageBlit
    {
        /// <summary>
        /// The subresource to blit from.
        /// </summary>
        public ImageSubresourceLayers SrcSubresource;
        /// <summary>
        /// Specifies the bounds of the first source region within <see cref="SrcSubresource"/>.
        /// </summary>
        public Offset3D SrcOffset1;
        /// <summary>
        /// Specifies the bounds of the second source region within <see cref="SrcSubresource"/>.
        /// </summary>
        public Offset3D SrcOffset2;
        /// <summary>
        /// The subresource to blit into.
        /// </summary>
        public ImageSubresourceLayers DstSubresource;
        /// <summary>
        /// Specifies the bounds of the first destination region within <see cref="DstSubresource"/>.
        /// </summary>
        public Offset3D DstOffset1;
        /// <summary>
        /// Specifies the bounds of the second destination region within <see cref="DstSubresource"/>.
        /// </summary>
        public Offset3D DstOffset2;
    }

    /// <summary>
    /// Structure specifying a buffer image copy operation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BufferImageCopy
    {
        /// <summary>
        /// the offset in bytes from the start of the buffer object where the image data is copied
        /// from or to.
        /// <para>Must be a multiple of 4.</para>
        /// </summary>
        public long BufferOffset;
        /// <summary>
        /// Specifies the data in buffer memory as a subregion of a larger two- or three-dimensional
        /// image, and control the addressing calculations of data in buffer memory. If this value is
        /// zero, that aspect of the buffer memory is considered to be tightly packed according to
        /// the <see cref="ImageExtent"/>.
        /// <para>Must be 0, or greater than or equal to the width member.</para>
        /// of imageExtent.
        /// </summary>
        public int BufferRowLength;
        /// <summary>
        /// Specifies the data in buffer memory as a subregion of a larger two- or three-dimensional
        /// image, and control the addressing calculations of data in buffer memory. If this value is
        /// zero, that aspect of the buffer memory is considered to be tightly packed according to
        /// the <see cref="ImageExtent"/>.
        /// <para>Must be 0, or greater than or equal to the height member of <see cref="ImageExtent"/>.</para>
        /// </summary>
        public int BufferImageHeight;
        /// <summary>
        /// Used to specify the specific image subresources of the image used for the source or
        /// destination image data.
        /// </summary>
        public ImageSubresourceLayers ImageSubresource;
        /// <summary>
        /// Selects the initial <c>X</c>, <c>Y</c><c>Z</c> offsets in texels of the sub-region of the
        /// source or destination image data.
        /// </summary>
        public Offset3D ImageOffset;
        /// <summary>
        /// The size in texels of the image to copy in width, height and depth.
        /// </summary>
        public Extent3D ImageExtent;
    }

    /// <summary>
    /// Structure specifying a clear color value.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct ClearColorValue
    {
        /// <summary>
        /// Are the color clear values when the format of the image or attachment is one of the
        /// formats other than signed integer or unsigned integer. Floating point values are
        /// automatically converted to the format of the image, with the clear value being treated as
        /// linear if the image is sRGB.
        /// </summary>
        [FieldOffset(0)] public ColorF4 Float4;
        /// <summary>
        /// Are the color clear values when the format of the image or attachment is signed integer.
        /// Signed integer values are converted to the format of the image by casting to the smaller
        /// type (with negative 32-bit values mapping to negative values in the smaller type). If the
        /// integer clear value is not representable in the target type (e.g. would overflow in
        /// conversion to that type), the clear value is undefined.
        /// </summary>
        [FieldOffset(0)] public ColorI4 Int4;
        /// <summary>
        /// Are the color clear values when the format of the image or attachment is unsigned
        /// integer. Unsigned integer values are converted to the format of the image by casting to
        /// the integer type with fewer bits.
        /// </summary>
        [FieldOffset(0)] public ColorU4 UInt4;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearColorValue"/> structure.
        /// </summary>
        /// <param name="value">
        /// Are the color clear values when the format of the image or attachment is one of the
        /// formats other than signed integer or unsigned integer. Floating point values are
        /// automatically converted to the format of the image, with the clear value being treated as
        /// linear if the image is sRGB.
        /// </param>
        public ClearColorValue(ColorF4 value) : this()
        {
            Float4 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearColorValue"/> structure.
        /// </summary>
        /// <param name="value">
        /// Are the color clear values when the format of the image or attachment is signed integer.
        /// Signed integer values are converted to the format of the image by casting to the smaller
        /// type (with negative 32-bit values mapping to negative values in the smaller type). If the
        /// integer clear value is not representable in the target type (e.g. would overflow in
        /// conversion to that type), the clear value is undefined.
        /// </param>
        public ClearColorValue(ColorI4 value) : this()
        {
            Int4 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearColorValue"/> structure.
        /// </summary>
        /// <param name="value">
        /// Are the color clear values when the format of the image or attachment is unsigned
        /// integer. Unsigned integer values are converted to the format of the image by casting to
        /// the integer type with fewer bits.
        /// </param>
        public ClearColorValue(ColorU4 value) : this()
        {
            UInt4 = value;
        }
    }

    /// <summary>
    /// Structure specifying a clear depth stencil value.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ClearDepthStencilValue
    {
        /// <summary>
        /// The clear value for the depth aspect of the depth/stencil attachment. It is a
        /// floating-point value which is automatically converted to the attachment’s format.
        /// <para>Must be between 0.0 and 1.0, inclusive.</para>
        /// </summary>
        public float Depth;
        /// <summary>
        /// The clear value for the stencil aspect of the depth/stencil attachment. It is a 32-bit
        /// integer value which is converted to the attachment's format by taking the appropriate
        /// number of LSBs.
        /// </summary>
        public int Stencil;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearDepthStencilValue"/> structure.
        /// </summary>
        /// <param name="depth">
        /// The clear value for the depth aspect of the depth/stencil attachment. It is a
        /// floating-point value which is automatically converted to the attachment’s format.
        /// </param>
        /// <param name="stencil">
        /// The clear value for the stencil aspect of the depth/stencil attachment. It is a 32-bit
        /// integer value which is converted to the attachment's format by taking the appropriate
        /// number of LSBs.
        /// </param>
        public ClearDepthStencilValue(float depth, int stencil)
        {
            Depth = depth;
            Stencil = stencil;
        }
    }

    /// <summary>
    /// Structure specifying a clear value.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct ClearValue
    {
        /// <summary>
        /// Specifies the color image clear values to use when clearing a color image or attachment.
        /// </summary>
        [FieldOffset(0)] public ClearColorValue Color;
        /// <summary>
        /// Specifies the depth and stencil clear values to use when clearing a depth/stencil image
        /// or attachment.
        /// </summary>
        [FieldOffset(0)] public ClearDepthStencilValue DepthStencil;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearValue"/> structure.
        /// </summary>
        /// <param name="color">
        /// Specifies the color image clear values to use when clearing a color image or attachment.
        /// </param>
        public ClearValue(ClearColorValue color) : this()
        {
            Color = color;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearValue"/> structure.
        /// </summary>
        /// <param name="depthStencil">
        /// Specifies the depth and stencil clear values to use when clearing a depth/stencil image
        /// or attachment.
        /// </param>
        public ClearValue(ClearDepthStencilValue depthStencil) : this()
        {
            DepthStencil = depthStencil;
        }

        /// <summary>
        /// Implicitly converts an instance of <see cref="ClearColorValue"/> to an instance of <see cref="ClearValue"/>.
        /// </summary>
        /// <param name="value">Instance to convert.</param>
        public static implicit operator ClearValue(ClearColorValue value) => new ClearValue(value);

        /// <summary>
        /// Implicitly converts an instance of <see cref="ClearDepthStencilValue"/> to an instance of
        /// <see cref="ClearValue"/>.
        /// </summary>
        /// <param name="value">Instance to convert.</param>
        public static implicit operator ClearValue(ClearDepthStencilValue value) => new ClearValue(value);
    }

    /// <summary>
    /// Structure specifying a clear attachment.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ClearAttachment
    {
        /// <summary>
        /// A mask selecting the color, depth and/or stencil aspects of the attachment to be cleared.
        /// <see cref="AspectMask"/> can include <see cref="ImageAspects.Color"/> for color
        /// attachments, <see cref="ImageAspects.Depth"/> for depth/stencil attachments with a
        /// depth component, and <see cref="ImageAspects.Stencil"/> for depth/stencil attachments
        /// with a stencil component. If the subpass's depth/stencil attachment is <see
        /// cref="AttachmentUnused"/>, then the clear has no effect.
        /// <para>Must not include <see cref="ImageAspects.Metadata"/>.</para>
        /// </summary>
        public ImageAspects AspectMask;
        /// <summary>
        /// Is only meaningful if <see cref="ImageAspects.Color"/> is set in <see
        /// cref="AspectMask"/>, in which case it is an index to the <see
        /// cref="SubpassDescription.ColorAttachments"/> array in the of the current subpass which
        /// selects the color attachment to clear. If <see cref="ColorAttachment"/> is <see
        /// cref="AttachmentUnused"/> then the clear has no effect.
        /// </summary>
        public int ColorAttachment;
        /// <summary>
        /// The color or depth/stencil value to clear the attachment to.
        /// </summary>
        public ClearValue ClearValue;
    }

    /// <summary>
    /// Structure specifying an image resolve operation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageResolve
    {
        /// <summary>
        /// Specifies the image subresource of the source image data. Resolve of depth/stencil image
        /// is not supported.
        /// </summary>
        public ImageSubresourceLayers SrcSubresource;
        /// <summary>
        /// Selects the initial <c>X</c>, <c>Y</c>, and <c>Z</c> offsets in texels of the sub-region
        /// of the source image data.
        /// </summary>
        public Offset3D SrcOffset;
        /// <summary>
        /// Specifies the image subresource of the destination image data. Resolve of depth/stencil
        /// image is not supported.
        /// </summary>
        public ImageSubresourceLayers DstSubresource;
        /// <summary>
        /// Selects the initial <c>X</c>, <c>Y</c>, and <c>Z</c> offsets in texels of the sub-region
        /// of the destination image data.
        /// </summary>
        public Offset3D DstOffset;
        /// <summary>
        /// The size in texels of the source image to resolve in width, height and depth.
        /// </summary>
        public Extent3D Extent;
    }

    /// <summary>
    /// Structure specifying a global memory barrier.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryBarrier
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// Specifies a source access mask.
        /// </summary>
        public Accesses SrcAccessMask;
        /// <summary>
        /// Specifies a destination access mask.
        /// </summary>
        public Accesses DstAccessMask;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryBarrier"/> structure.
        /// </summary>
        /// <param name="srcAccessMask">Specifies a source access mask.</param>
        /// <param name="dstAccessMask">Specifies a destination access mask.</param>
        public MemoryBarrier(Accesses srcAccessMask, Accesses dstAccessMask)
        {
            Type = StructureType.MemoryBarrier;
            Next = IntPtr.Zero;
            SrcAccessMask = srcAccessMask;
            DstAccessMask = dstAccessMask;
        }

        internal void Prepare()
        {
            Type = StructureType.MemoryBarrier;
        }
    }

    /// <summary>
    /// Structure specifying a buffer memory barrier.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BufferMemoryBarrier
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// Specifies a source access mask.
        /// </summary>
        public Accesses SrcAccessMask;
        /// <summary>
        /// Specifies a destination access mask.
        /// </summary>
        public Accesses DstAccessMask;
        /// <summary>
        /// The source queue family for a queue family ownership transfer.
        /// </summary>
        public int SrcQueueFamilyIndex;
        /// <summary>
        /// The destination queue family for a queue family ownership transfer.
        /// </summary>
        public int DstQueueFamilyIndex;
        /// <summary>
        /// A <see cref="VulkanCore.Buffer"/> handle to the buffer whose backing memory is affected by the barrier.
        /// </summary>
        public long Buffer;
        /// <summary>
        /// An offset in bytes into the backing memory for buffer; this is relative to the base
        /// offset as bound to the buffer.
        /// </summary>
        public long Offset;
        /// <summary>
        /// A size in bytes of the affected area of backing memory for buffer, or <see
        /// cref="WholeSize"/> to use the range from offset to the end of the buffer.
        /// </summary>
        public long Size;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferMemoryBarrier"/> structure.
        /// </summary>
        /// <param name="buffer">
        /// A <see cref="VulkanCore.Buffer"/> handle to the buffer whose backing memory is affected
        /// by the barrier.
        /// </param>
        /// <param name="srcAccessMask">Specifies a source access mask.</param>
        /// <param name="dstAccessMask">Specifies a destination access mask.</param>
        /// <param name="offset">
        /// An offset in bytes into the backing memory for buffer; this is relative to the base
        /// offset as bound to the buffer.
        /// </param>
        /// <param name="size">
        /// A size in bytes of the affected area of backing memory for buffer, or <see
        /// cref="WholeSize"/> to use the range from offset to the end of the buffer.
        /// </param>
        public BufferMemoryBarrier(Buffer buffer, Accesses srcAccessMask, Accesses dstAccessMask, long offset = 0, long size = WholeSize)
            : this(buffer, srcAccessMask, dstAccessMask, QueueFamilyIgnored, QueueFamilyIgnored, offset, size)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferMemoryBarrier"/> structure.
        /// </summary>
        /// <param name="buffer">
        /// A <see cref="VulkanCore.Buffer"/> handle to the buffer whose backing memory is affected
        /// by the barrier.
        /// </param>
        /// <param name="srcAccessMask">Specifies a source access mask.</param>
        /// <param name="dstAccessMask">Specifies a destination access mask.</param>
        /// <param name="srcQueueFamilyIndex">
        /// The source queue family for a queue family ownership transfer.
        /// </param>
        /// <param name="dstQueueFamilyIndex">
        /// The destination queue family for a queue family ownership transfer.
        /// </param>
        /// <param name="offset">
        /// An offset in bytes into the backing memory for buffer; this is relative to the base
        /// offset as bound to the buffer.
        /// </param>
        /// <param name="size">
        /// A size in bytes of the affected area of backing memory for buffer, or <see
        /// cref="WholeSize"/> to use the range from offset to the end of the buffer.
        /// </param>
        public BufferMemoryBarrier(Buffer buffer, Accesses srcAccessMask, Accesses dstAccessMask,
            int srcQueueFamilyIndex, int dstQueueFamilyIndex, long offset = 0, long size = WholeSize)
        {
            Type = StructureType.BufferMemoryBarrier;
            Next = IntPtr.Zero;
            Buffer = buffer;
            Offset = offset;
            Size = size;
            SrcAccessMask = srcAccessMask;
            DstAccessMask = dstAccessMask;
            SrcQueueFamilyIndex = srcQueueFamilyIndex;
            DstQueueFamilyIndex = dstQueueFamilyIndex;
        }

        internal void Prepare()
        {
            Type = StructureType.BufferMemoryBarrier;
        }
    }

    /// <summary>
    /// Structure specifying the parameters of an image memory barrier.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageMemoryBarrier
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// Specifies a source access mask.
        /// </summary>
        public Accesses SrcAccessMask;
        /// <summary>
        /// Specifies a destination access mask.
        /// </summary>
        public Accesses DstAccessMask;
        /// <summary>
        /// The old layout in an image layout transition.
        /// <para>
        /// Must be <see cref="ImageLayout.Undefined"/> or the current layout of the image
        /// subresources affected by the barrier.
        /// </para>
        /// </summary>
        public ImageLayout OldLayout;
        /// <summary>
        /// The new layout in an image layout transition.
        /// <para>Must not be <see cref="ImageLayout.Undefined"/> or <see cref="ImageLayout.Preinitialized"/></para>
        /// </summary>
        public ImageLayout NewLayout;
        /// <summary>
        /// The source queue family for a queue family ownership transfer.
        /// </summary>
        public int SrcQueueFamilyIndex;
        /// <summary>
        /// The destination queue family for a queue family ownership transfer.
        /// </summary>
        public int DstQueueFamilyIndex;
        /// <summary>
        /// A handle to the <see cref="VulkanCore.Image"/> affected by this barrier.
        /// </summary>
        public long Image;
        /// <summary>
        /// Describes an area of the backing memory for image, as well as the set of image
        /// subresources whose image layouts are modified.
        /// </summary>
        public ImageSubresourceRange SubresourceRange;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageMemoryBarrier"/> structure.
        /// </summary>
        /// <param name="image">
        /// A handle to the <see cref="VulkanCore.Image"/> affected by this barrier.
        /// </param>
        /// <param name="subresourceRange">
        /// Describes an area of the backing memory for image, as well as the set of image
        /// subresources whose image layouts are modified.
        /// </param>
        /// <param name="srcAccessMask">Specifies a source access mask.</param>
        /// <param name="dstAccessMask">Specifies a destination access mask.</param>
        /// <param name="oldLayout">The old layout in an image layout transition.</param>
        /// <param name="newLayout">The new layout in an image layout transition.</param>
        /// <param name="srcQueueFamilyIndex">
        /// The source queue family for a queue family ownership transfer.
        /// </param>
        /// <param name="dstQueueFamilyIndex">
        /// The destination queue family for a queue family ownership transfer.
        /// </param>
        public ImageMemoryBarrier(Image image, ImageSubresourceRange subresourceRange,
            Accesses srcAccessMask, Accesses dstAccessMask, ImageLayout oldLayout, ImageLayout newLayout,
            int srcQueueFamilyIndex = QueueFamilyIgnored, int dstQueueFamilyIndex = QueueFamilyIgnored)
        {
            Type = StructureType.ImageMemoryBarrier;
            Next = IntPtr.Zero;
            SrcAccessMask = srcAccessMask;
            DstAccessMask = dstAccessMask;
            OldLayout = oldLayout;
            NewLayout = newLayout;
            SrcQueueFamilyIndex = srcQueueFamilyIndex;
            DstQueueFamilyIndex = dstQueueFamilyIndex;
            Image = image;
            SubresourceRange = subresourceRange;
        }

        internal void Prepare()
        {
            Type = StructureType.ImageMemoryBarrier;
        }
    }

    /// <summary>
    /// Bitmask specifying memory access types that will participate in a memory dependency.
    /// </summary>
    [Flags]
    public enum Accesses
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies read access to an indirect command structure read as part of an indirect
        /// drawing or dispatch command.
        /// </summary>
        IndirectCommandRead = 1 << 0,
        /// <summary>
        /// Specifies read access to an index buffer as part of an indexed drawing command, bound by
        /// <see cref="CommandBuffer.CmdBindIndexBuffer"/>.
        /// </summary>
        IndexRead = 1 << 1,
        /// <summary>
        /// Specifies read access to a vertex buffer as part of a drawing command, bound by <see cref="CommandBuffer.CmdBindVertexBuffers"/>.
        /// </summary>
        VertexAttributeRead = 1 << 2,
        /// <summary>
        /// Specifies read access to a uniform buffer.
        /// </summary>
        UniformRead = 1 << 3,
        /// <summary>
        /// Specifies read access to an input attachment within a render pass during fragment shading.
        /// </summary>
        InputAttachmentRead = 1 << 4,
        /// <summary>
        /// Specifies read access to a storage buffer, uniform texel buffer, storage texel buffer,
        /// sampled image or storage image.
        /// </summary>
        ShaderRead = 1 << 5,
        /// <summary>
        /// Specifies write access to a storage buffer, storage texel buffer or storage image.
        /// </summary>
        ShaderWrite = 1 << 6,
        /// <summary>
        /// Specifies read access to a color attachment, such as via blending, logic operations or
        /// via certain subpass load operations.
        /// </summary>
        ColorAttachmentRead = 1 << 7,
        /// <summary>
        /// Specifies write access to a color or resolve attachment during a render pass or via
        /// certain subpass load and store operations.
        /// </summary>
        ColorAttachmentWrite = 1 << 8,
        /// <summary>
        /// Specifies read access to a depth/stencil attachment via depth or stencil operations or
        /// via certain subpass load operations.
        /// </summary>
        DepthStencilAttachmentRead = 1 << 9,
        /// <summary>
        /// Specifies write access to a depth/stencil attachment via depth or stencil operations or
        /// via certain subpass load and store operations.
        /// </summary>
        DepthStencilAttachmentWrite = 1 << 10,
        /// <summary>
        /// Specifies read access to an image or buffer in a copy operation.
        /// </summary>
        TransferRead = 1 << 11,
        /// <summary>
        /// Specifies write access to an image or buffer in a clear or copy operation.
        /// </summary>
        TransferWrite = 1 << 12,
        /// <summary>
        /// Specifies read access by a host operation. Accesses of this type are not performed
        /// through a resource, but directly on memory.
        /// </summary>
        HostRead = 1 << 13,
        /// <summary>
        /// Specifies write access by a host operation. Accesses of this type are not performed
        /// through a resource, but directly on memory.
        /// </summary>
        HostWrite = 1 << 14,
        /// <summary>
        /// Specifies read access via non-specific entities. These entities include the Vulkan device
        /// and host, but may also include entities external to the Vulkan device or otherwise not
        /// part of the core Vulkan pipeline. When included in a destination access mask, makes all
        /// available writes visible to all future read accesses on entities known to the Vulkan device.
        /// </summary>
        MemoryRead = 1 << 15,
        /// <summary>
        /// Specifies write access via non-specific entities. These entities include the Vulkan
        /// device and host, but may also include entities external to the Vulkan device or otherwise
        /// not part of the core Vulkan pipeline. When included in a source access mask, all writes
        /// that are performed by entities known to the Vulkan device are made available. When
        /// included in a destination access mask, makes all available writes visible to all future
        /// write accesses on entities known to the Vulkan device.
        /// </summary>
        MemoryWrite = 1 << 16,
        /// <summary>
        /// Specifies reads from <see cref="Buffer"/> inputs to <see cref="Nvx.CommandBufferExtensions.CmdProcessCommandsNvx"/>.
        /// </summary>
        CommandProcessReadNvx = 1 << 17,
        /// <summary>
        /// Specifies writes to the target command buffer in <see cref="Nvx.CommandBufferExtensions.CmdProcessCommandsNvx"/>.
        /// </summary>
        CommandProcessWriteNvx = 1 << 18,
        /// <summary>
        /// Is similar to <see cref="ColorAttachmentRead"/>, but also includes advanced blend operations.
        /// </summary>
        ColorAttachmentReadNoncoherentExt = 1 << 19
    }

    /// <summary>
    /// Structure specifying render pass begin info.
    /// </summary>
    public unsafe struct RenderPassBeginInfo
    {
        /// <summary>
        /// The <see cref="VulkanCore.RenderPass"/> to begin an instance of.
        /// </summary>
        public long RenderPass;
        /// <summary>
        /// The <see cref="VulkanCore.Framebuffer"/> containing the attachments that are used with
        /// the render pass.
        /// </summary>
        public long Framebuffer;
        /// <summary>
        /// The render area that is affected by the render pass instance.
        /// <para>
        /// The effects of attachment load, store and multisample resolve operations are restricted
        /// to the pixels whose x and y coordinates fall within the render area on all attachments.
        /// The render area extends to all layers of framebuffer. The application must ensure (using
        /// scissor if necessary) that all rendering is contained within the render area, otherwise
        /// the pixels outside of the render area become undefined and shader side effects may occur
        /// for fragments outside the render area. The render area must be contained within the
        /// framebuffer dimensions.
        /// </para>
        /// </summary>
        public Rect2D RenderArea;

        /// <summary>
        /// An array of <see cref="ClearValue"/> structures that contains clear values for each
        /// attachment, if the attachment uses a <see cref="AttachmentDescription.LoadOp"/> value of
        /// <see cref="AttachmentLoadOp.Clear"/> or if the attachment has a depth/stencil format and
        /// uses a <see cref="AttachmentDescription.StencilLoadOp"/> value of <see
        /// cref="AttachmentLoadOp.Clear"/>. The array is indexed by attachment number. Only elements
        /// corresponding to cleared attachments are used. Other elements of <see
        /// cref="ClearValues"/> are ignored.
        /// </summary>
        public ClearValue[] ClearValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderPassBeginInfo"/> structure.
        /// </summary>
        /// <param name="framebuffer">
        /// The <see cref="VulkanCore.Framebuffer"/> containing the attachments that are used with
        /// the render pass.
        /// </param>
        /// <param name="renderArea">The render area that is affected by the render pass instance.</param>
        /// <param name="clearValues">
        /// An array of <see cref="ClearValue"/> structures that contains clear values for each
        /// attachment, if the attachment uses a <see cref="AttachmentDescription.LoadOp"/> value of
        /// <see cref="AttachmentLoadOp.Clear"/> or if the attachment has a depth/stencil format and
        /// uses a <see cref="AttachmentDescription.StencilLoadOp"/> value of <see
        /// cref="AttachmentLoadOp.Clear"/>. The array is indexed by attachment number. Only elements
        /// corresponding to cleared attachments are used. Other elements of <see
        /// cref="ClearValues"/> are ignored.
        /// </param>
        public RenderPassBeginInfo(Framebuffer framebuffer, Rect2D renderArea, params ClearValue[] clearValues)
        {
            RenderPass = framebuffer.RenderPass;
            Framebuffer = framebuffer;
            RenderArea = renderArea;
            ClearValues = clearValues;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderPassBeginInfo"/> structure.
        /// </summary>
        /// <param name="framebuffer">
        /// The <see cref="VulkanCore.Framebuffer"/> containing the attachments that are used with
        /// the render pass.
        /// </param>
        /// <param name="renderPass">The <see cref="VulkanCore.RenderPass"/> to use.</param>
        /// <param name="renderArea">The render area that is affected by the render pass instance.</param>
        /// <param name="clearValues">
        /// An array of <see cref="ClearValue"/> structures that contains clear values for each
        /// attachment, if the attachment uses a <see cref="AttachmentDescription.LoadOp"/> value of
        /// <see cref="AttachmentLoadOp.Clear"/> or if the attachment has a depth/stencil format and
        /// uses a <see cref="AttachmentDescription.StencilLoadOp"/> value of <see
        /// cref="AttachmentLoadOp.Clear"/>. The array is indexed by attachment number. Only elements
        /// corresponding to cleared attachments are used. Other elements of <see
        /// cref="ClearValues"/> are ignored.
        /// </param>
        public RenderPassBeginInfo(Framebuffer framebuffer, RenderPass renderPass, Rect2D renderArea, params ClearValue[] clearValues)
        {
            RenderPass = renderPass;
            Framebuffer = framebuffer;
            RenderArea = renderArea;
            ClearValues = clearValues;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public long RenderPass;
            public long Framebuffer;
            public Rect2D RenderArea;
            public int ClearValueCount;
            public ClearValue* ClearValues;
        }

        internal void ToNative(out Native native, ClearValue* clearValues)
        {
            native.Type = StructureType.RenderPassBeginInfo;
            native.Next = IntPtr.Zero;
            native.RenderPass = RenderPass;
            native.Framebuffer = Framebuffer;
            native.RenderArea = RenderArea;
            native.ClearValueCount = ClearValues?.Length ?? 0;
            native.ClearValues = clearValues;
        }
    }

    /// <summary>
    /// Specify how commands in the first subpass of a render pass are provided.
    /// </summary>
    public enum SubpassContents
    {
        /// <summary>
        /// Specifies that the contents of the subpass will be recorded inline in the primary command
        /// buffer, and secondary command buffers must not be executed within the subpass.
        /// </summary>
        Inline = 0,
        /// <summary>
        /// Specifies that the contents are recorded in secondary command buffers that will be called
        /// from the primary command buffer, and <see cref="CommandBuffer.CmdExecuteCommands"/> is
        /// the only valid command on the command buffer until <see
        /// cref="CommandBuffer.vkCmdNextSubpass"/> or <see cref="CommandBuffer.CmdEndRenderPass"/>.
        /// </summary>
        SecondaryCommandBuffers = 1
    }

    /// <summary>
    /// Bitmask specifying pipeline stages.
    /// </summary>
    [Flags]
    public enum PipelineStages
    {
        /// <summary>
        /// Specifies the stage of the pipeline where any commands are initially received by the queue.
        /// </summary>
        TopOfPipe = 1 << 0,
        /// <summary>
        /// Specifies the stage of the pipeline where Draw/DispatchIndirect data structures are consumed.
        /// <para>This stage also includes reading commands written by <see cref="Nvx.CommandBufferExtensions.CmdProcessCommandsNvx"/>.</para>
        /// </summary>
        DrawIndirect = 1 << 1,
        /// <summary>
        /// Specifies the stage of the pipeline where vertex and index buffers are consumed.
        /// </summary>
        VertexInput = 1 << 2,
        /// <summary>
        /// Specifies the vertex shader stage.
        /// </summary>
        VertexShader = 1 << 3,
        /// <summary>
        /// Specifies the tessellation control shader stage.
        /// </summary>
        TessellationControlShader = 1 << 4,
        /// <summary>
        /// Specifies the tessellation evaluation shader stage.
        /// </summary>
        TessellationEvaluationShader = 1 << 5,
        /// <summary>
        /// Specifies the geometry shader stage.
        /// </summary>
        GeometryShader = 1 << 6,
        /// <summary>
        /// Specifies the fragment shader stage.
        /// </summary>
        FragmentShader = 1 << 7,
        /// <summary>
        /// Specifies the stage of the pipeline where early fragment tests (depth and stencil tests
        /// before fragment
        /// shading) are performed. This stage also includes subpass load operations for framebuffer
        /// attachments with a depth/stencil format.
        /// </summary>
        EarlyFragmentTests = 1 << 8,
        /// <summary>
        /// Specifies that the stage of the pipeline where late fragment tests (depth and stencil
        /// tests after fragment shading) are performed.
        /// <para>
        /// This stage also includes subpass store operations for framebuffer attachments with a
        /// depth/stencil format.
        /// </para>
        /// </summary>
        LateFragmentTests = 1 << 9,
        /// <summary>
        /// Specifies that the stage of the pipeline after blending where the final color values are
        /// output from the pipeline.
        /// <para>
        /// This stage also includes subpass load and store operations and multisample resolve
        /// operations for framebuffer attachments with a color format.
        /// </para>
        /// </summary>
        ColorAttachmentOutput = 1 << 10,
        /// <summary>
        /// Specifies the execution of a compute shader.
        /// </summary>
        ComputeShader = 1 << 11,
        /// <summary>
        /// Transfer/copy operations.
        /// </summary>
        Transfer = 1 << 12,
        /// <summary>
        /// Specifies the final stage in the pipeline where operations generated by all commands
        /// complete execution.
        /// </summary>
        BottomOfPipe = 1 << 13,
        /// <summary>
        /// Specifies a pseudo-stage indicating execution on the host of reads/writes of device
        /// memory.
        /// <para>This stage is not invoked by any commands recorded in a command buffer.</para>
        /// </summary>
        Host = 1 << 14,
        /// <summary>
        /// Specifies the execution of all graphics pipeline stages, and is equivalent to the logical
        /// OR of:
        /// <para>
        /// <see cref="TopOfPipe"/>, <see cref="DrawIndirect"/>, <see cref="VertexInput"/>, <see
        /// cref="VertexShader"/>, <see cref="TessellationControlShader"/>, <see
        /// cref="TessellationEvaluationShader"/>, <see cref="GeometryShader"/>, <see
        /// cref="FragmentShader"/>, <see cref="EarlyFragmentTests"/>, <see
        /// cref="LateFragmentTests"/>, <see cref="ColorAttachmentOutput"/>, <see cref="BottomOfPipe"/>.
        /// </para>
        /// </summary>
        AllGraphics = 1 << 15,
        /// <summary>
        /// Equivalent to the logical OR of every other pipeline stage flag that is supported on the
        /// queue it is used with.
        /// </summary>
        AllCommands = 1 << 16,
        /// <summary>
        /// Specifies the stage of the pipeline where device-side generation of commands via <see
        /// cref="Nvx.CommandBufferExtensions.CmdProcessCommandsNvx"/> is handled.
        /// </summary>
        CommandProcessNvx = 1 << 17
    }

    /// <summary>
    /// Structure specifying a clear rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ClearRect
    {
        /// <summary>
        /// The two-dimensional region to be cleared.
        /// </summary>
        public Rect2D Rect;
        /// <summary>
        /// The first layer to be cleared.
        /// </summary>
        public int BaseArrayLayer;
        /// <summary>
        /// The number of layers to clear.
        /// </summary>
        public int LayerCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearRect"/> structure.
        /// </summary>
        /// <param name="rect">The two-dimensional region to be cleared.</param>
        /// <param name="baseArrayLayer">The first layer to be cleared.</param>
        /// <param name="layerCount">The number of layers to clear.</param>
        public ClearRect(Rect2D rect, int baseArrayLayer, int layerCount)
        {
            Rect = rect;
            BaseArrayLayer = baseArrayLayer;
            LayerCount = layerCount;
        }
    }
}
