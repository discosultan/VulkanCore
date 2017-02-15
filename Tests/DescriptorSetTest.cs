using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class DescriptorSetTest : HandleTestBase
    {
        public DescriptorSetTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output)
        {
            DescriptorSetLayout = Device.CreateDescriptorSetLayout(new DescriptorSetLayoutCreateInfo(
                new DescriptorSetLayoutBinding(0, DescriptorType.StorageBuffer, 1, ShaderStages.All)));
            DescriptorPool = Device.CreateDescriptorPool(new DescriptorPoolCreateInfo(1, new[] { new DescriptorPoolSize(DescriptorType.StorageBuffer, 1 )}));
            DescriptorSet = DescriptorPool.AllocateSets(new DescriptorSetAllocateInfo(DescriptorSetLayout))[0];
        }

        public DescriptorPool DescriptorPool { get; }
        public DescriptorSetLayout DescriptorSetLayout { get; }
        public DescriptorSet DescriptorSet { get; }

        public override void Dispose()
        {
            DescriptorPool.Dispose();
            DescriptorSetLayout.Dispose();
            base.Dispose();
        }
    }
}
