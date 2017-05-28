using System;

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
            vkTrimCommandPoolKHR(commandPool.Parent, commandPool, 0);
        }

        private delegate void vkTrimCommandPoolKHRDelegate(IntPtr device, long commandPool, CommandPoolTrimFlags flags);
        private static readonly vkTrimCommandPoolKHRDelegate vkTrimCommandPoolKHR = VulkanLibrary.GetStaticProc<vkTrimCommandPoolKHRDelegate>(nameof(vkTrimCommandPoolKHR));
    }

    // Is reserved for future use.
    [Flags]
    internal enum CommandPoolTrimFlags
    {
        None = 0
    }
}
