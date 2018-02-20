using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Amd
{
    /// <summary>
    /// Provides AMD specific extension methods for the <see cref="CommandBuffer"/> class.
    /// </summary>
    public static class CommandBufferExtensions
    {
        /// <summary>
        /// Perform an indirect draw with the draw count sourced from a buffer.
        /// <para>
        /// Behaves similar to <see cref="CommandBuffer.CmdDrawIndirect"/> except that the draw count
        /// is read by the device from a buffer during execution.
        /// </para>
        /// <para>
        /// The command will read an unsigned 32-bit integer from <paramref name="countBuffer"/>
        /// located at <paramref name="countBufferOffset"/> and use this as the draw count.
        /// </para>
        /// </summary>
        /// <param name="commandBuffer">The command buffer into which the command is recorded.</param>
        /// <param name="buffer">The buffer containing draw parameters.</param>
        /// <param name="offset">
        /// The byte offset into <paramref name="buffer"/> where parameters begin.
        /// </param>
        /// <param name="countBuffer">The buffer containing the draw count.</param>
        /// <param name="countBufferOffset">
        /// The byte offset into <paramref name="countBuffer"/> where the draw count begins.
        /// </param>
        /// <param name="maxDrawCount">
        /// Specifies the maximum number of draws that will be executed.
        /// <para>
        /// The actual number of executed draw calls is the minimum of the count specified in
        /// <paramref name="countBuffer"/> and <paramref name="maxDrawCount"/>.
        /// </para>
        /// </param>
        /// <param name="stride">The byte stride between successive sets of draw parameters.</param>
        public static void CmdDrawIndirectCountAmd(this CommandBuffer commandBuffer, Buffer buffer, long offset,
            Buffer countBuffer, long countBufferOffset, int maxDrawCount, int stride)
        {
            vkCmdDrawIndirectCountAMD(commandBuffer)(commandBuffer, buffer, offset, countBuffer, countBufferOffset, maxDrawCount, stride);
        }

        /// <summary>
        /// Perform an indexed indirect draw with the draw count sourced from a buffer.
        /// <para>
        /// Behaves similar to <see cref="CmdDrawIndirectCountAmd"/> except that the draw count is
        /// read by the device from a buffer during execution.
        /// </para>
        /// <para>
        /// The command will read an unsigned 32-bit integer from <paramref name="countBuffer"/>
        /// located at <paramref name="countBufferOffset"/> and use this as the draw count.
        /// </para>
        /// </summary>
        /// <param name="commandBuffer">The command buffer into which the command is recorded.</param>
        /// <param name="buffer">The buffer containing draw parameters.</param>
        /// <param name="offset">
        /// The byte offset into <paramref name="buffer"/> where parameters begin.
        /// </param>
        /// <param name="countBuffer">The buffer containing the draw count.</param>
        /// <param name="countBufferOffset">
        /// The byte offset into <paramref name="countBuffer"/> where the draw count begins.
        /// </param>
        /// <param name="maxDrawCount">
        /// Specifies the maximum number of draws that will be executed.
        /// <para>
        /// The actual number of executed draw calls is the minimum of the count specified in
        /// <paramref name="countBuffer"/> and <paramref name="maxDrawCount"/>.
        /// </para>
        /// </param>
        /// <param name="stride">The byte stride between successive sets of draw parameters.</param>
        public static void CmdDrawIndexedIndirectCountAmd(this CommandBuffer commandBuffer,
            Buffer buffer, long offset, Buffer countBuffer, long countBufferOffset, int maxDrawCount, int stride)
        {
            vkCmdDrawIndexedIndirectCountAMD(commandBuffer)(commandBuffer, buffer, offset, countBuffer, countBufferOffset, maxDrawCount, stride);
        }

        /// <summary>
        /// Execute a pipelined write of a marker value into a buffer.
        /// </summary>
        /// <param name="commandBuffer">The command buffer into which the command will be recorded.</param>
        /// <param name="pipelineStage">Specifies the pipeline stage whose completion triggers the marker write.</param>
        /// <param name="dstBuffer">The buffer where the marker will be written to.</param>
        /// <param name="dstOffset">The byte offset into the buffer where the marker will be written to.</param>
        /// <param name="marker">The 32-bit value of the marker.</param>
        public static void CmdWriteBufferMarkerAmd(this CommandBuffer commandBuffer,
            PipelineStages pipelineStage, Buffer dstBuffer, long dstOffset, int marker)
        {
            vkCmdWriteBufferMarkerAMD(commandBuffer)(commandBuffer, pipelineStage, dstBuffer, dstOffset, marker);
        }

        private delegate void vkCmdDrawIndirectCountAMDDelegate(IntPtr commandBuffer, long buffer, long offset, long countBuffer, long countBufferOffset, int maxDrawCount, int stride);
        private static vkCmdDrawIndirectCountAMDDelegate vkCmdDrawIndirectCountAMD(CommandBuffer commandBuffer) => GetProc<vkCmdDrawIndirectCountAMDDelegate>(commandBuffer, nameof(vkCmdDrawIndirectCountAMD));

        private delegate void vkCmdDrawIndexedIndirectCountAMDDelegate(IntPtr commandBuffer, long buffer, long offset, long countBuffer, long countBufferOffset, int maxDrawCount, int stride);
        private static vkCmdDrawIndexedIndirectCountAMDDelegate vkCmdDrawIndexedIndirectCountAMD(CommandBuffer commandBuffer) => GetProc<vkCmdDrawIndexedIndirectCountAMDDelegate>(commandBuffer, nameof(vkCmdDrawIndexedIndirectCountAMD));

        private delegate void vkCmdWriteBufferMarkerAMDDelegate(IntPtr commandBuffer, PipelineStages pipelineStage, long dstBuffer, long dstOffset, int marker);
        private static vkCmdWriteBufferMarkerAMDDelegate vkCmdWriteBufferMarkerAMD(CommandBuffer commandBuffer) => GetProc<vkCmdWriteBufferMarkerAMDDelegate>(commandBuffer, nameof(vkCmdWriteBufferMarkerAMD));

        private static TDelegate GetProc<TDelegate>(CommandBuffer commandBuffer, string name) where TDelegate : class => commandBuffer.Parent.Parent.GetProc<TDelegate>(name);
    }

    /// <summary>
    /// Structure defining rasterization order for a graphics pipeline.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineRasterizationStateRasterizationOrderAmd
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies the primitive rasterization order to use.
        /// </summary>
        public RasterizationOrderAmd RasterizationOrder;

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="PipelineRasterizationStateRasterizationOrderAmd"/> structure.
        /// </summary>
        /// <param name="rasterizationOrder">Specifies the primitive rasterization order to use.</param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public PipelineRasterizationStateRasterizationOrderAmd(RasterizationOrderAmd rasterizationOrder,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.PipelineRasterizationStateRasterizationOrderAmd;
            Next = next;
            RasterizationOrder = rasterizationOrder;
        }
    }

    /// <summary>
    /// Specify rasterization order for a graphics pipeline.
    /// </summary>
    public enum RasterizationOrderAmd
    {
        /// <summary>
        /// Specifies that the order of these operations for each primitive in a subpass must occur
        /// in primitive order.
        /// </summary>
        Strict = 0,
        /// <summary>
        /// Specifies that the order of these operations for each primitive in a subpass may not
        /// occur in primitive order.
        /// </summary>
        Relaxed = 1
    }

    /// <summary>
    /// Structure informing whether or not texture gather bias/LOD functionality is
    /// supported for a given image format and a given physical device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TextureLodGatherFormatPropertiesAmd
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/>.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Tells if the image format can be used with texture gather bias/LOD functions, as
        /// introduced by the "VK_AMD_texture_gather_bias_lod" extension. This field is set by the
        /// implementation. User-specified value is ignored.
        /// </summary>
        public Bool SupportsTextureGatherLodBiasAmd;
    }
}
