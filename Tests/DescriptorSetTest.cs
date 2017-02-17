using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class DescriptorSetTest : HandleTestBase
    {
        [Fact]
        public void AllocateSetsAndFreeSets_Succeeds()
        {
            var layoutCreateInfo = new DescriptorSetLayoutCreateInfo(
                new DescriptorSetLayoutBinding(0, DescriptorType.StorageBuffer, 1));
            var poolCreateInfo = new DescriptorPoolCreateInfo(
                1, 
                new[] { new DescriptorPoolSize(DescriptorType.StorageBuffer, 1) }, 
                DescriptorPoolCreateFlags.FreeDescriptorSet);
            using (DescriptorSetLayout layout = Device.CreateDescriptorSetLayout(layoutCreateInfo))
            using (DescriptorPool pool = Device.CreateDescriptorPool(poolCreateInfo))
            {
                using (pool.AllocateSets(new DescriptorSetAllocateInfo(1, layout))[0]) { }
                DescriptorSet set = pool.AllocateSets(new DescriptorSetAllocateInfo(1, layout))[0];
                pool.FreeSets(set);
            }
        }

        [Fact]
        public void UpdateSetsDescriptorWrite_Succeeds()
        {
            const int bufferSize = 256;

            var layoutCreateInfo = new DescriptorSetLayoutCreateInfo(
                new DescriptorSetLayoutBinding(0, DescriptorType.StorageBuffer, 1));
            var poolCreateInfo = new DescriptorPoolCreateInfo(
                1,
                new[] { new DescriptorPoolSize(DescriptorType.StorageBuffer, 1) },
                DescriptorPoolCreateFlags.FreeDescriptorSet);
            using (Buffer buffer = Device.CreateBuffer(new BufferCreateInfo(bufferSize, BufferUsages.StorageBuffer)))
            using (DeviceMemory memory = Device.AllocateMemory(new MemoryAllocateInfo(bufferSize, 0)))
            using (DescriptorSetLayout layout = Device.CreateDescriptorSetLayout(layoutCreateInfo))
            using (DescriptorPool pool = Device.CreateDescriptorPool(poolCreateInfo))
            using (DescriptorSet set = pool.AllocateSets(new DescriptorSetAllocateInfo(1, layout))[0])
            {
                // Required to satisfy the validation layer.
                buffer.GetMemoryRequirements();

                buffer.BindMemory(memory);

                var descriptorWrite = new WriteDescriptorSet(set, 0, 0, 1, DescriptorType.StorageBuffer,
                    bufferInfo: new[] { new DescriptorBufferInfo(buffer) });
                pool.UpdateSets(new[] { descriptorWrite });
            }
        }

        [Fact(Skip = "Resolve valid usage")]
        public void UpdateSetsDescriptorCopy_Succeeds()
        {
            var layoutCreateInfo = new DescriptorSetLayoutCreateInfo(
                new DescriptorSetLayoutBinding(0, DescriptorType.StorageBuffer, 2));
            var poolCreateInfo = new DescriptorPoolCreateInfo(
                1,
                new[] { new DescriptorPoolSize(DescriptorType.StorageBuffer, 2) },
                DescriptorPoolCreateFlags.FreeDescriptorSet);
            using (DescriptorSetLayout layout = Device.CreateDescriptorSetLayout(layoutCreateInfo))
            using (DescriptorPool pool = Device.CreateDescriptorPool(poolCreateInfo))
            using (DescriptorSet set = pool.AllocateSets(new DescriptorSetAllocateInfo(1, layout))[0])
            {
                // It is valid to copy from self to self (without overlapping memory boundaries).
                var descriptorCopy = new CopyDescriptorSet(set, 0, 0, set, 0, 1, 1);
                pool.UpdateSets(descriptorCopies: new[] { descriptorCopy });
            }
        }

        [Fact]
        public void ResetPool_Succeeds()
        {
            var layoutCreateInfo = new DescriptorSetLayoutCreateInfo(
                new DescriptorSetLayoutBinding(0, DescriptorType.StorageBuffer, 1));
            var poolCreateInfo = new DescriptorPoolCreateInfo(
                1,
                new[] { new DescriptorPoolSize(DescriptorType.StorageBuffer, 1) });
            using (DescriptorSetLayout layout = Device.CreateDescriptorSetLayout(layoutCreateInfo))
            using (DescriptorPool pool = Device.CreateDescriptorPool(poolCreateInfo))
            {
                pool.AllocateSets(new DescriptorSetAllocateInfo(1, layout));
                pool.Reset();
            }
        }

        public DescriptorSetTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }
    }
}
