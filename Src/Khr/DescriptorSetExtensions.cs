using System;

namespace VulkanCore.Khr
{
    // TODO: dec
    public static class DescriptorSetExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="descriptorSet"></param>
        /// <param name="descriptorUpdateTemplate"></param>
        /// <param name="data"></param>
        public static void UpdateWithTemplateKhr(this DescriptorSet descriptorSet,
            DescriptorUpdateTemplateKhr descriptorUpdateTemplate, IntPtr data)
        {
            vkUpdateDescriptorSetWithTemplateKHR(descriptorSet.Parent.Parent, descriptorSet, descriptorUpdateTemplate, data);
        }

        private delegate void vkUpdateDescriptorSetWithTemplateKHRDelegate(IntPtr device, long descriptorSet, long descriptorUpdateTemplate, IntPtr data);
        private static readonly vkUpdateDescriptorSetWithTemplateKHRDelegate vkUpdateDescriptorSetWithTemplateKHR = VulkanLibrary.GetProc<vkUpdateDescriptorSetWithTemplateKHRDelegate>(nameof(vkUpdateDescriptorSetWithTemplateKHR));
    }
}
