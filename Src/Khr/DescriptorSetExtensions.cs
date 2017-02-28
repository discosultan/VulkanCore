using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

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
            UpdateDescriptorSetWithTemplateKhr(descriptorSet.Parent.Parent, descriptorSet, descriptorUpdateTemplate, data);
        }

        [DllImport(VulkanDll, EntryPoint = "vkUpdateDescriptorSetWithTemplateKHR", CallingConvention = CallConv)]
        private static extern void UpdateDescriptorSetWithTemplateKhr(IntPtr device, long descriptorSet, long descriptorUpdateTemplate, IntPtr data);
    }
}
