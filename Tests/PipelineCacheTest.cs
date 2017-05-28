using System.Linq;
using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class PipelineCacheTest : HandleTestBase
    {
        [Fact]
        public void GetData()
        {
            var descriptorSetLayoutCreateInfo = new DescriptorSetLayoutCreateInfo(
                new DescriptorSetLayoutBinding(0, DescriptorType.StorageBuffer, 1, ShaderStages.Compute),
                new DescriptorSetLayoutBinding(1, DescriptorType.StorageBuffer, 1, ShaderStages.Compute));
            using (DescriptorSetLayout descriptorSetLayout = Device.CreateDescriptorSetLayout(descriptorSetLayoutCreateInfo))
            using (PipelineLayout pipelineLayout = Device.CreatePipelineLayout(new PipelineLayoutCreateInfo(new[] { descriptorSetLayout })))
            using (ShaderModule shader = Device.CreateShaderModule(new ShaderModuleCreateInfo(ReadAllBytes("Shader.comp.spv"))))
            {
                var pipelineCreateInfo = new ComputePipelineCreateInfo(
                    new PipelineShaderStageCreateInfo(ShaderStages.Compute, shader, "main"),
                    pipelineLayout);

                byte[] cacheBytes;

                // Populate cache.
                using (PipelineCache cache = Device.CreatePipelineCache())
                {
                    using (Device.CreateComputePipeline(pipelineCreateInfo, cache)) { }
                    cacheBytes = cache.GetData();
                }

                Assert.False(cacheBytes.All(x => x == 0));

                // Recreate pipeline from cache.
                using (PipelineCache cache = Device.CreatePipelineCache(new PipelineCacheCreateInfo(cacheBytes)))
                {
                    using (Device.CreateComputePipeline(pipelineCreateInfo, cache)) { }
                }
            }
        }

        [Fact]
        public void MergePipelines()
        {
            using (PipelineCache dstCache = Device.CreatePipelineCache())
            using (PipelineCache srcCache = Device.CreatePipelineCache())
            {
                dstCache.MergeCache(srcCache);
                dstCache.MergeCaches(srcCache);
            }
        }

        public PipelineCacheTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }
    }
}
