using System;
using System.Runtime.InteropServices;

namespace VulkanCore.NV
{
    /// <summary>
    /// Provides NVIDIA specific extension methods for the <see cref="CommandBuffer"/> class.
    /// </summary>
    public static unsafe class CommandBufferExtensions
    {
        /// <summary>
        /// Set the viewport W scaling on a command buffer.
        /// </summary>
        /// <param name="commandBuffer">The command buffer into which the command will be recorded.</param>
        /// <param name="firstViewport">
        /// The index of the first viewport whose parameters are updated by the command.
        /// </param>
        /// <param name="viewportWScalings">Structures specifying viewport parameters.</param>
        public static void CmdSetViewportWScalingNV(this CommandBuffer commandBuffer,
            int firstViewport, ViewportWScalingNV[] viewportWScalings)
        {
            fixed (ViewportWScalingNV* viewportWScalingsPtr = viewportWScalings)
            {
                vkCmdSetViewportWScalingNV(commandBuffer)
                    (commandBuffer, firstViewport, viewportWScalings?.Length ?? 0, viewportWScalingsPtr);
            }
        }

        private delegate void vkCmdSetViewportWScalingNVDelegate(IntPtr commandBuffer, int firstViewport, int viewportCount, ViewportWScalingNV* viewportWScalings);
        private static vkCmdSetViewportWScalingNVDelegate vkCmdSetViewportWScalingNV(CommandBuffer commandBuffer) => commandBuffer.Parent.Parent.GetProc<vkCmdSetViewportWScalingNVDelegate>(nameof(vkCmdSetViewportWScalingNV));
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewportWScalingNV"/> structure.
        /// </summary>
        /// <param name="xCoeff">The viewport's W scaling factor for x.</param>
        /// <param name="yCoeff">The viewport's W scaling factor for y.</param>
        public ViewportWScalingNV(float xCoeff, float yCoeff)
        {
            XCoeff = xCoeff;
            YCoeff = yCoeff;
        }
    }

    /// <summary>
    /// Structure specifying parameters of a newly created pipeline viewport W scaling state.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineViewportWScalingStateCreateInfoNV
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
        /// Controls whether viewport W scaling is enabled.
        /// </summary>
        public Bool ViewportWScalingEnable;
        /// <summary>
        /// The number of viewports used by W scaling and must match the number of viewports in the
        /// pipeline if viewport W scaling is enabled.
        /// </summary>
        public int ViewportCount;
        /// <summary>
        /// Structures, which define the W scaling parameters for the corresponding viewport. If the
        /// viewport W scaling state is dynamic, this member is ignored.
        /// </summary>
        public ViewportWScalingNV[] ViewportWScalings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineViewportWScalingStateCreateInfoNV"/> structure.
        /// </summary>
        /// <param name="viewportWScalingEnable">The enable for viewport W scaling.</param>
        /// <param name="viewportWScalings">
        /// Structures which define the W scaling parameters for the corresponding viewport. If the
        /// viewport W scaling state is dynamic, this member is ignored.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public PipelineViewportWScalingStateCreateInfoNV(bool viewportWScalingEnable, ViewportWScalingNV[] viewportWScalings,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.PipelineViewportWScalingStateCreateInfoNV;
            Next = next;
            ViewportWScalingEnable = viewportWScalingEnable;
            ViewportCount = viewportWScalings?.Length ?? 0;
            ViewportWScalings = viewportWScalings;
        }
    }

    /// <summary>
    /// Use Windows keyex mutex mechanism to synchronize work.
    /// <para>
    /// When submitting work that operates on memory imported from a Direct3D 11 resource to a queue,
    /// the keyed mutex mechanism may be used in addition to Vulkan semaphores to synchronize the work.
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Win32KeyedMutexAcquireReleaseInfoNV
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
        /// the number of entries in the <see cref="AcquireSyncs"/>, <see cref="AcquireKeys"/>, and
        /// <see cref="AcquireTimeoutMilliseconds"/> arrays.
        /// </summary>
        public int AcquireCount;
        /// <summary>
        /// An array of <see cref="DeviceMemory"/> objects which were imported from Direct3D 11 resources.
        /// </summary>
        public long[] AcquireSyncs;
        /// <summary>
        /// Mutex key values to wait for prior to beginning the submitted work.
        /// <para>
        /// Entries refer to the keyed mutex associated with the corresponding entries in <see cref="AcquireSyncs"/>.
        /// </para>
        /// </summary>
        public long[] AcquireKeys;
        /// <summary>
        /// Timeout values, in millisecond units, for each acquire specified in <see cref="AcquireKeys"/>.
        /// </summary>
        public int[] AcquireTimeoutMilliseconds;
        /// <summary>
        /// The number of entries in the <see cref="ReleaseSyncs"/> and <see cref="ReleaseKeys"/> arrays.
        /// </summary>
        public int ReleaseCount;
        /// <summary>
        /// An array of <see cref="DeviceMemory"/> objects which were imported from Direct3D 11 resources.
        /// </summary>
        public long[] ReleaseSyncs;
        /// <summary>
        /// Mutex key values to set when the submitted work has completed.
        /// <para>
        /// Entries refer to the keyed mutex associated with the corresponding entries in <see cref="ReleaseSyncs"/>.
        /// </para>
        /// </summary>
        public long[] ReleaseKeys;

        /// <summary>
        /// Initializes a new instance of the <see cref="Win32KeyedMutexAcquireReleaseInfoNV"/> structure.
        /// </summary>
        /// <param name="acquireSyncs">
        /// An array of <see cref="DeviceMemory"/> objects which were imported from Direct3D 11 resources.
        /// </param>
        /// <param name="acquireKeys">
        /// Mutex key values to wait for prior to beginning the submitted work.
        /// </param>
        /// <param name="acquireTimeoutMilliseconds">
        /// Timeout values, in millisecond units, for each acquire specified in <see cref="AcquireKeys"/>.
        /// </param>
        /// <param name="releaseSyncs">
        /// An array of <see cref="DeviceMemory"/> objects which were imported from Direct3D 11 resources.
        /// </param>
        /// <param name="releaseKeys">
        /// An array of <see cref="DeviceMemory"/> objects which were imported from Direct3D 11 resources.
        /// </param>
        /// <param name="next"></param>
        public Win32KeyedMutexAcquireReleaseInfoNV(DeviceMemory[] acquireSyncs, long[] acquireKeys,
            int[] acquireTimeoutMilliseconds, DeviceMemory[] releaseSyncs, long[] releaseKeys,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.Win32KeyedMutexAcquireReleaseInfoNV;
            Next = next;
            AcquireCount = acquireSyncs?.Length ?? 0;
            AcquireSyncs = acquireSyncs?.ToHandleArray();
            AcquireKeys = acquireKeys;
            AcquireTimeoutMilliseconds = acquireTimeoutMilliseconds;
            ReleaseCount = releaseSyncs?.Length ?? 0;
            ReleaseSyncs = releaseSyncs?.ToHandleArray();
            ReleaseKeys = releaseKeys;
        }
    }
}
