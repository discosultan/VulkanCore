using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore.NV
{
    // TODO: doc
    /// <summary>
    /// Provides NVIDIA specific extension methods for the <see cref="CommandBuffer"/> class.
    /// </summary>
    public static unsafe class CommandBufferExtensions
    {
        /// <summary>
        /// Set the viewport W scaling on a command buffer.
        /// </summary>
        /// <param name="commandBuffer"></param>
        /// <param name="firstViewport"></param>
        /// <param name="viewportWScalings"></param>
        public static void CmdSetViewportWScalingNV(this CommandBuffer commandBuffer,
            int firstViewport, ViewportWScalingNV[] viewportWScalings)
        {
            fixed (ViewportWScalingNV* viewportWScalingsPtr = viewportWScalings)
                vkCmdSetViewportWScalingNV(commandBuffer, firstViewport, viewportWScalings?.Length ?? 0, viewportWScalingsPtr);
        }

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkCmdSetViewportWScalingNV(IntPtr commandBuffer,
            int firstViewport, int viewportCount, ViewportWScalingNV* viewportWScalings);
    }

    /// <summary>
    /// Structure specifying a viewport.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ViewportWScalingNV
    {
        /// <summary>
        /// The viewport's W scaling factor for x.
        /// </summary>
        public float XCoeff;
        /// <summary>
        /// The viewport's W scaling factor for y.
        /// </summary>
        public float YCoeff;
    }
}
