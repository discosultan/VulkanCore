using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Khx
{
    /// <summary>
    /// Provides experimental Khronos specific extension methods for the <see cref="CommandBuffer"/> class.
    /// </summary>
    public static class CommandBufferExtensions
    {
        /// <summary>
        /// Modify device mask of a command buffer.
        /// </summary>
        /// <param name="commandBuffer">Command buffer whose current device mask is modified.</param>
        /// <param name="deviceMask">The new value of the current device mask.</param>
        public static void CmdSetDeviceMaskKhx(this CommandBuffer commandBuffer, int deviceMask)
        {
            vkCmdSetDeviceMaskKHX(commandBuffer)(commandBuffer.Handle, deviceMask);
        }

        /// <summary>
        /// Dispatch compute work items.
        /// </summary>
        /// <param name="commandBuffer">The command buffer into which the command will be recorded.</param>
        /// <param name="baseGroupX">The start value for the X component of <c>WorkgroupId</c>.</param>
        /// <param name="baseGroupY">The start value for the Y component of <c>WorkgroupId</c>.</param>
        /// <param name="baseGroupZ">The start value for the Z component of <c>WorkgroupId</c>.</param>
        /// <param name="groupCountX">The number of local workgroups to dispatch in the X dimension.</param>
        /// <param name="groupCountY">The number of local workgroups to dispatch in the Y dimension.</param>
        /// <param name="groupCountZ">The number of local workgroups to dispatch in the Z dimension.</param>
        public static void CmdDispatchBaseKhx(this CommandBuffer commandBuffer,
            int baseGroupX, int baseGroupY, int baseGroupZ,
            int groupCountX, int groupCountY, int groupCountZ)
        {
            vkCmdDispatchBaseKHX(commandBuffer)(commandBuffer.Handle,
                baseGroupX, baseGroupY, baseGroupZ,
                groupCountX, groupCountY, groupCountZ);
        }

        private delegate void vkCmdSetDeviceMaskKHXDelegate(IntPtr commandBuffer, int deviceMask);
        private static vkCmdSetDeviceMaskKHXDelegate vkCmdSetDeviceMaskKHX(CommandBuffer commandBuffer) => GetProc<vkCmdSetDeviceMaskKHXDelegate>(commandBuffer, nameof(vkCmdSetDeviceMaskKHX));

        private delegate void vkCmdDispatchBaseKHXDelegate(IntPtr commandBuffer, int baseGroupX, int baseGroupY, int baseGroupZ, int groupCountX, int groupCountY, int groupCountZ);
        private static vkCmdDispatchBaseKHXDelegate vkCmdDispatchBaseKHX(CommandBuffer commandBuffer) => GetProc<vkCmdDispatchBaseKHXDelegate>(commandBuffer, nameof(vkCmdDispatchBaseKHX));

        private static TDelegate GetProc<TDelegate>(CommandBuffer commandBuffer, string name) where TDelegate : class => commandBuffer.Parent.Parent.GetProc<TDelegate>(name);
    }

    /// <summary>
    /// Set the initial device mask for a command buffer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceGroupCommandBufferBeginInfoKhx
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
        /// The initial value of the command buffer's device mask.
        /// </summary>
        public int DeviceMask;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceGroupCommandBufferBeginInfoKhx"/> structure.
        /// </summary>
        /// <param name="deviceMask">The initial value of the command buffer's device mask.</param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public DeviceGroupCommandBufferBeginInfoKhx(int deviceMask, IntPtr next = default(IntPtr))
        {
            Type = StructureType.DeviceGroupCommandBufferBeginInfoKhx;
            Next = next;
            DeviceMask = deviceMask;
        }
    }

    /// <summary>
    /// Set the initial device mask and render areas for a render pass instance.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceGroupRenderPassBeginInfoKhx
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
        /// The device mask for the render pass instance.
        /// </summary>
        public int DeviceMask;
        /// <summary>
        /// The number of elements in the <see cref="DeviceRenderAreas"/> array.
        /// </summary>
        public int DeviceRenderAreaCount;
        /// <summary>
        /// Structures defining the render area for each physical device.
        /// </summary>
        public Rect2D[] DeviceRenderAreas;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceGroupRenderPassBeginInfoKhx"/> structure.
        /// </summary>
        /// <param name="deviceMask">The device mask for the render pass instance.</param>
        /// <param name="deviceRenderAreas">
        /// Structures defining the render area for each physical device.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public DeviceGroupRenderPassBeginInfoKhx(int deviceMask, Rect2D[] deviceRenderAreas,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.DeviceGroupRenderPassBeginInfoKhx;
            Next = next;
            DeviceMask = deviceMask;
            DeviceRenderAreaCount = deviceRenderAreas?.Length ?? 0;
            DeviceRenderAreas = deviceRenderAreas;
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
    public struct Win32KeyedMutexAcquireReleaseInfoKhx
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Pointer to next structure.
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
        /// Initializes a new instance of the <see cref="Win32KeyedMutexAcquireReleaseInfoKhx"/> structure.
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
        public Win32KeyedMutexAcquireReleaseInfoKhx(DeviceMemory[] acquireSyncs, long[] acquireKeys,
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

    /// <summary>
    /// Structure indicating which physical devices execute semaphore operations and
    /// command buffers.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceGroupSubmitInfoKhx
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
        /// The number of elements in the <see cref="WaitSemaphoreDeviceIndices"/> array.
        /// </summary>
        public int WaitSemaphoreCount;
        /// <summary>
        /// Device indices indicating which physical device executes the semaphore wait operation in
        /// the corresponding element of <see cref="SubmitInfo.WaitSemaphores"/>.
        /// </summary>
        public int[] WaitSemaphoreDeviceIndices;
        /// <summary>
        /// The number of elements in the <see cref="CommandBufferDeviceMasks"/> array.
        /// </summary>
        public int CommandBufferCount;
        /// <summary>
        /// Device masks indicating which physical devices execute the command buffer in the
        /// corresponding element of <see cref="SubmitInfo.CommandBuffers"/>. A physical device
        /// executes the command buffer if the corresponding bit is set in the mask.
        /// </summary>
        public int[] CommandBufferDeviceMasks;
        /// <summary>
        /// The number of elements in the <see cref="SignalSemaphoreDeviceIndices"/> array.
        /// </summary>
        public int SignalSemaphoreCount;
        /// <summary>
        /// Device indices indicating which physical device executes the semaphore signal operation
        /// in the corresponding element of <see cref="SubmitInfo.SignalSemaphores"/>.
        /// </summary>
        public int[] SignalSemaphoreDeviceIndices;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceGroupSubmitInfoKhx"/> structure.
        /// </summary>
        /// <param name="waitSemaphoreDeviceIndices">
        /// Device indices indicating which physical device executes the semaphore wait operation in
        /// the corresponding element of <see cref="SubmitInfo.WaitSemaphores"/>.
        /// </param>
        /// <param name="commandBufferDeviceMasks">
        /// Device masks indicating which physical devices execute the command buffer in the
        /// corresponding element of <see cref="SubmitInfo.CommandBuffers"/>. A physical device
        /// executes the command buffer if the corresponding bit is set in the mask.
        /// </param>
        /// <param name="signalSemaphoreDeviceIndices">
        /// Device indices indicating which physical device executes the semaphore signal operation
        /// in the corresponding element of <see cref="SubmitInfo.SignalSemaphores"/>.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public DeviceGroupSubmitInfoKhx(
            int[] waitSemaphoreDeviceIndices = null,
            int[] commandBufferDeviceMasks = null,
            int[] signalSemaphoreDeviceIndices = null,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.DeviceGroupSubmitInfoKhx;
            Next = next;
            WaitSemaphoreCount = waitSemaphoreDeviceIndices?.Length ?? 0;
            WaitSemaphoreDeviceIndices = waitSemaphoreDeviceIndices;
            CommandBufferCount = commandBufferDeviceMasks?.Length ?? 0;
            CommandBufferDeviceMasks = commandBufferDeviceMasks;
            SignalSemaphoreCount = signalSemaphoreDeviceIndices?.Length ?? 0;
            SignalSemaphoreDeviceIndices = signalSemaphoreDeviceIndices;
        }
    }
}
