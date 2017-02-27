using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore.Khr
{
    /// <summary>
    /// Provides Khronos specific extension methods for the <see cref="CommandPool"/> class.
    /// </summary>
    public static class CommandPoolExtensions
    {
        /// <summary>
        /// Trim a command pool.
        /// <para>
        /// Trimming a command pool recycles unused memory from the command pool back to the system.
        /// </para>
        /// <para>Command buffers allocated from the pool are not affected by the command.</para>
        /// </summary>
        /// <param name="commandPool">The command pool to trim.</param>
        public static void TrimKhr(this CommandPool commandPool)
        {
            TrimCommandPoolKhr(commandPool.Parent, commandPool, CommandPoolTrimFlags.None);
        }

        [DllImport(VulkanDll, EntryPoint = "vkTrimCommandPoolKHR", CallingConvention = CallConv)]
        private static extern void TrimCommandPoolKhr(IntPtr device, long commandPool, CommandPoolTrimFlags flags);
    }

    // Is reserved for future use.
    [Flags]
    internal enum CommandPoolTrimFlags
    {
        None = 0
    }
}
