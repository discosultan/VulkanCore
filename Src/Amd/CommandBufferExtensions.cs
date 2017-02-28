using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

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

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkCmdDrawIndirectCountAMD(IntPtr commandBuffer, 
            long buffer, long offset, long countBuffer, long countBufferOffset, int maxDrawCount, int stride);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkCmdDrawIndexedIndirectCountAMD(IntPtr commandBuffer, 
            long buffer, long offset, long countBuffer, long countBufferOffset, int maxDrawCount, int stride);
    }
}
