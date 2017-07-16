using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Khr
{
    /// <summary>
    /// Provides Khronos specific extension methods for the <see cref="CommandBuffer"/> class.
    /// </summary>
    public static unsafe class CommandBufferExtensions
    {
        /// <summary>
        /// Pushes descriptor updates into a command buffer.
        /// </summary>
        /// <param name="commandBuffer">
        /// The command buffer that the descriptors will be recorded in.
        /// </param>
        /// <param name="pipelineBindPoint">
        /// Indicates whether the descriptors will be used by graphics pipelines or compute pipelines.
        /// <para>
        /// There is a separate set of push descriptor bindings for each of graphics and compute, so
        /// binding one does not disturb the other.
        /// </para>
        /// </param>
        /// <param name="layout">The object used to program the bindings.</param>
        /// <param name="set">
        /// The set number of the descriptor set in the pipeline layout that will be updated.
        /// </param>
        /// <param name="descriptorWrites">Structures describing the descriptors to be updated.</param>
        public static void CmdPushDescriptorSetKhr(this CommandBuffer commandBuffer,
            PipelineBindPoint pipelineBindPoint, PipelineLayout layout, int set, WriteDescriptorSet[] descriptorWrites)
        {
            int count = descriptorWrites?.Length ?? 0;
            var nativeDescriptorWrites = stackalloc WriteDescriptorSet.Native[count];
            for (int i = 0; i < count; i++)
                descriptorWrites[i].ToNative(&nativeDescriptorWrites[i]);

            vkCmdPushDescriptorSetKHR(commandBuffer, pipelineBindPoint, layout, set, count, nativeDescriptorWrites);

            for (int i = 0; i < count; i++)
                nativeDescriptorWrites[i].Free();
        }

        /// <summary>
        /// Pushes descriptor updates into a command buffer using a descriptor update template.
        /// </summary>
        /// <param name="commandBuffer">
        /// The command buffer that the descriptors will be recorded in.
        /// </param>
        /// <param name="descriptorUpdateTemplate">
        /// A descriptor update template which defines how to interpret the descriptor information in
        /// <paramref name="data"/>.
        /// </param>
        /// <param name="layout">
        /// The object used to program the bindings.
        /// <para>
        /// It must be compatible with the layout used to create the <paramref
        /// name="descriptorUpdateTemplate"/> handle.
        /// </para>
        /// </param>
        /// <param name="set">
        /// The set number of the descriptor set in the pipeline layout that will be updated.
        /// <para>
        /// This must be the same number used to create the <paramref
        /// name="descriptorUpdateTemplate"/> handle.
        /// </para>
        /// </param>
        /// <param name="data">
        /// Points to memory which contains the descriptors for the templated update.
        /// </param>
        public static void CmdPushDescriptorSetWithTemplateKhr(this CommandBuffer commandBuffer,
            DescriptorUpdateTemplateKhr descriptorUpdateTemplate, PipelineLayout layout, int set, IntPtr data)
        {
            vkCmdPushDescriptorSetWithTemplateKHR(commandBuffer.Handle, descriptorUpdateTemplate, layout, set, data);
        }

        private delegate void vkCmdPushDescriptorSetKHRDelegate(IntPtr commandBuffer, PipelineBindPoint pipelineBindPoint, long layout, int set, int descriptorWriteCount, WriteDescriptorSet.Native* descriptorWrites);
        private static readonly vkCmdPushDescriptorSetKHRDelegate vkCmdPushDescriptorSetKHR = VulkanLibrary.GetStaticProc<vkCmdPushDescriptorSetKHRDelegate>(nameof(vkCmdPushDescriptorSetKHR));

        private delegate void vkCmdPushDescriptorSetWithTemplateKHRDelegate(IntPtr commandBuffer, long descriptorUpdateTemplate, long layout, int set, IntPtr data);
        private static readonly vkCmdPushDescriptorSetWithTemplateKHRDelegate vkCmdPushDescriptorSetWithTemplateKHR = VulkanLibrary.GetStaticProc<vkCmdPushDescriptorSetWithTemplateKHRDelegate>(nameof(vkCmdPushDescriptorSetWithTemplateKHR));
    }

    /// <summary>
    /// Structure specifying values for Direct3D 12 fence-backed semaphores.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct D3D12FenceSubmitInfoKhr
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
        /// The number of semaphore wait values specified in <see cref="WaitSemaphoreValues"/>.
        /// </summary>
        public int WaitSemaphoreValuesCount;
        /// <summary>
        /// Values for the corresponding semaphores in <see cref="SubmitInfo.WaitSemaphores"/> to
        /// wait for.
        /// </summary>
        public long[] WaitSemaphoreValues;
        /// <summary>
        /// The number of semaphore signal values specified in <see cref="SignalSemaphoreValues"/>.
        /// </summary>
        public int SignalSemaphoreValuesCount;
        /// <summary>
        /// Values for the corresponding semaphores in <see cref="SubmitInfo.SignalSemaphores"/> to
        /// set when signaled.
        /// </summary>
        public long[] SignalSemaphoreValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="D3D12FenceSubmitInfoKhr"/> structure.
        /// </summary>
        /// <param name="waitSemaphoreValues">
        /// Values for the corresponding semaphores in <see cref="SubmitInfo.WaitSemaphores"/> to
        /// wait for.
        /// </param>
        /// <param name="signalSemaphoreValues">
        /// Values for the corresponding semaphores in <see cref="SubmitInfo.SignalSemaphores"/> to
        /// set when signaled.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public D3D12FenceSubmitInfoKhr(long[] waitSemaphoreValues = null, long[] signalSemaphoreValues = null,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.D3D12FenceSubmitInfoKhr;
            Next = next;
            WaitSemaphoreValuesCount = waitSemaphoreValues?.Length ?? 0;
            WaitSemaphoreValues = waitSemaphoreValues;
            SignalSemaphoreValuesCount = signalSemaphoreValues?.Length ?? 0;
            SignalSemaphoreValues = signalSemaphoreValues;
        }
    }
}
