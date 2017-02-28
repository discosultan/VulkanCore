using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore.Khx
{
    // TODO: doc
    public static class CommandBufferExtensions
    {
        /// <summary>
        /// Modify device mask of a command buffer.
        /// </summary>
        /// <param name="commandBuffer"></param>
        /// <param name="deviceMask"></param>
        /// <returns></returns>
        public static void CmdSetDeviceMaskKhx(this CommandBuffer commandBuffer, int deviceMask)
        {
            vkCmdSetDeviceMaskKHX(commandBuffer.Handle, deviceMask);
        }

        /// <summary>
        /// Dispatch compute work items.
        /// </summary>
        /// <param name="commandBuffer"></param>
        /// <param name="baseGroupX"></param>
        /// <param name="baseGroupY"></param>
        /// <param name="baseGroupZ"></param>
        /// <param name="groupCountX"></param>
        /// <param name="groupCountY"></param>
        /// <param name="groupCountZ"></param>
        public static void CmdDispatchBaseKhx(this CommandBuffer commandBuffer,
            int baseGroupX, int baseGroupY, int baseGroupZ,
            int groupCountX, int groupCountY, int groupCountZ)
        {
            vkCmdDispatchBaseKHX(commandBuffer.Handle,
                baseGroupX, baseGroupY, baseGroupZ,
                groupCountX, groupCountY, groupCountZ);
        }

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkCmdSetDeviceMaskKHX(IntPtr commandBuffer, int deviceMask);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkCmdDispatchBaseKHX(IntPtr commandBuffer,
            int baseGroupX, int baseGroupY, int baseGroupZ,
            int groupCountX, int groupCountY, int groupCountZ);
    }
}
