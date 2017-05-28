using System;

namespace VulkanCore.Khr
{
    /// <summary>
    /// Provides Khronos specific extension methods for the <see cref="DescriptorSet"/> class.
    /// </summary>
    public static class DescriptorSetExtensions
    {
        /// <summary>
        /// Update the contents of a descriptor set object using an update template.
        /// </summary>
        /// <param name="descriptorSet">The descriptor set to update.</param>
        /// <param name="descriptorUpdateTemplate">
        /// Specifies the update mapping between the application pointer and the descriptor set to update.
        /// </param>
        /// <param name="data">
        /// A pointer to memory which contains one or more structures of <see
        /// cref="DescriptorImageInfo"/>, <see cref="DescriptorBufferInfo"/>, or <see
        /// cref="BufferView"/> used to write the descriptors.
        /// </param>
        public static void UpdateWithTemplateKhr(this DescriptorSet descriptorSet,
            DescriptorUpdateTemplateKhr descriptorUpdateTemplate, IntPtr data)
        {
            vkUpdateDescriptorSetWithTemplateKHR(descriptorSet.Parent.Parent, descriptorSet, descriptorUpdateTemplate, data);
        }

        private delegate void vkUpdateDescriptorSetWithTemplateKHRDelegate(IntPtr device, long descriptorSet, long descriptorUpdateTemplate, IntPtr data);
        private static readonly vkUpdateDescriptorSetWithTemplateKHRDelegate vkUpdateDescriptorSetWithTemplateKHR = VulkanLibrary.GetStaticProc<vkUpdateDescriptorSetWithTemplateKHRDelegate>(nameof(vkUpdateDescriptorSetWithTemplateKHR));
    }
}
