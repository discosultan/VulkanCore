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
            vkCmdDrawIndirectCountAMD(commandBuffer, buffer, offset, countBuffer, countBufferOffset, maxDrawCount, stride);
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
            vkCmdDrawIndexedIndirectCountAMD(commandBuffer, buffer, offset, countBuffer, countBufferOffset, maxDrawCount, stride);
        }

        private delegate void vkCmdDrawIndirectCountAMDDelegate(IntPtr commandBuffer, long buffer, long offset, long countBuffer, long countBufferOffset, int maxDrawCount, int stride);
        private static readonly vkCmdDrawIndirectCountAMDDelegate vkCmdDrawIndirectCountAMD = VulkanLibrary.GetProc<vkCmdDrawIndirectCountAMDDelegate>(nameof(vkCmdDrawIndirectCountAMD));

        private delegate void vkCmdDrawIndexedIndirectCountAMDDelegate(IntPtr commandBuffer, long buffer, long offset, long countBuffer, long countBufferOffset, int maxDrawCount, int stride);
        private static readonly vkCmdDrawIndexedIndirectCountAMDDelegate vkCmdDrawIndexedIndirectCountAMD = VulkanLibrary.GetProc<vkCmdDrawIndexedIndirectCountAMDDelegate>(nameof(vkCmdDrawIndexedIndirectCountAMD));
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineRasterizationStateRasterizationOrderAmd
    {
        public StructureType Type;
        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Rasterization order to use for the pipeline.
        /// </summary>
        public RasterizationOrderAmd RasterizationOrder;

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="PipelineRasterizationStateRasterizationOrderAmd"/> structure.
        /// </summary>
        /// <param name="rasterizationOrder">Rasterization order to use for the pipeline.</param>
        /// <param name="next">Pointer to next structure.</param>
        public PipelineRasterizationStateRasterizationOrderAmd(RasterizationOrderAmd rasterizationOrder,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.PipelineRasterizationStateRasterizationOrderAmd;
            Next = next;
            RasterizationOrder = rasterizationOrder;
        }
    }

    public enum RasterizationOrderAmd
    {
        Strict = 0,
        Relaxed = 1
    }
}
