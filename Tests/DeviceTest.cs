using System;
using System.IO;
using System.Linq;
using VulkanCore.Ext;
using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class DeviceTest : HandleTestBase
    {
        [Fact]
        public void GetProcAddr_ThrowsArgumentNullExForNullName()
        {
            Assert.Throws<ArgumentNullException>(() => Device.GetProcAddr(null));
        }

        [Fact]
        public void GetProcAddr_ReturnsZeroPtrForUnknownProcedure()
        {
            IntPtr ptr = Device.GetProcAddr("does not exist");
            Assert.Equal(IntPtr.Zero, ptr);
        }

        [Fact]
        public void GetProcAddr_ReturnsValidHandleForExistingCommand()
        {
            IntPtr address = Device.GetProcAddr("vkDebugMarkerSetObjectNameEXT");
            Assert.NotEqual(IntPtr.Zero, address);
        }

        [Fact]
        public void GetProc_ThrowsArgumentNullForNull()
        {
            Assert.Throws<ArgumentNullException>(() => Device.GetProc<EventHandler>(null));
        }

        [Fact]
        public void GetProc_ReturnsNullForMissingCommand()
        {
            Assert.Null(Device.GetProc<EventHandler>("does not exist"));
        }
        
        [Fact]
        public void GetProc_ReturnsValidDelegate()
        {
            var commandDelegate = Device.GetProc<DebugMarkerSetObjectNameExt>("vkDebugMarkerSetObjectNameEXT");
            Assert.NotNull(commandDelegate);
        }

        [Fact]
        public void GetProcTwice_DelegatesEqual()
        {
            const string commandName = "vkDebugMarkerSetObjectNameEXT";
            var commandDelegate1 = Device.GetProc<DebugMarkerSetObjectNameExt>(commandName);
            var commandDelegate2 = Device.GetProc<DebugMarkerSetObjectNameExt>(commandName);
            Assert.Equal(commandDelegate1, commandDelegate2);
        }

        [Fact]
        public void GetQueue_Succeeds()
        {
            Device.GetQueue(0);
        }

        [Fact]
        public void CreateBuffer_Succeeds()
        {
            var createInfo = new BufferCreateInfo(32, BufferUsages.VertexBuffer);
            using (Device.CreateBuffer(createInfo)) { }
            using (Device.CreateBuffer(createInfo, CustomAllocator)) { }
        }

        [Fact]
        public void CreateImage_Succeeds()
        {
            var createInfo = new ImageCreateInfo
            {
                ArrayLayers = 1,
                Extent = new Extent3D(32, 32, 1),
                Format = Format.R32UInt,
                ImageType = ImageType.Image2D,
                Usage = ImageUsages.TransferSrc,
                MipLevels = 1,
                Samples = SampleCounts.Count1
            };
            using (Device.CreateImage(createInfo)) { }
            using (Device.CreateImage(createInfo, CustomAllocator)) { }
        }

        [Fact]
        public void AllocateMemory_Succeeds()
        {
            var allocateInfo = new MemoryAllocateInfo(32, 0);
            using (Device.AllocateMemory(allocateInfo)) { }
            using (Device.AllocateMemory(allocateInfo, CustomAllocator)) { }
        }

        [Fact]
        public void CreatePipelineCache_Succeeds()
        {
            var createInfo = new PipelineCacheCreateInfo();
            using (Device.CreatePipelineCache(createInfo)) { }
            using (Device.CreatePipelineCache(createInfo, CustomAllocator)) { }
        }

        [Fact]
        public void CreatePipelineLayout_Succeeds()
        {
            var createInfo = new PipelineLayoutCreateInfo();
            using (Device.CreatePipelineLayout(createInfo)) { }
            using (Device.CreatePipelineLayout(createInfo, CustomAllocator)) { }
        }

        [Fact]
        public void CreateSampler_Succeeds()
        {
            var createInfo = new SamplerCreateInfo();
            using (Device.CreateSampler(createInfo)) { }
            using (Device.CreateSampler(createInfo, CustomAllocator)) { }
        }

        [Fact]
        public void CreateDescriptorPool_Succeeds()
        {
            var createInfo = new DescriptorPoolCreateInfo(
                1,
                new[] { new DescriptorPoolSize(DescriptorType.StorageBuffer, 2) });
            using (Device.CreateDescriptorPool(createInfo)) { }
            using (Device.CreateDescriptorPool(createInfo, CustomAllocator)) { }
        }

        [Fact]
        public void CreateEvent_Succeeds()
        {
            using (Device.CreateEvent()) { }
            using (Device.CreateEvent(CustomAllocator)) { }
        }

        [Fact]
        public void CreateFence_Succeeds()
        {
            using (Device.CreateFence()) { }
            using (Device.CreateFence(allocator: CustomAllocator)) { }
        }

        [Fact]
        public void CreateSemaphore_Succeeds()
        {
            using (Device.CreateSemaphore()) { }
            using (Device.CreateSemaphore(CustomAllocator)) { }
        }

        [Fact]
        public void CreateRenderPass_Succeeds()
        {
            var createInfo = new RenderPassCreateInfo(
                new[] { new SubpassDescription() }
            );
            using (Device.CreateRenderPass(createInfo)) { }
            using (Device.CreateRenderPass(createInfo, CustomAllocator)) { }
        }

        [Fact]
        public void DebugMarkerSetObjectNameExt_Succeeds()
        {
            if (!AvailableDeviceExtensions.Contains(Constant.DeviceExtension.ExtDebugMarker)) return;

            Device.DebugMarkerSetObjectNameExt(new DebugMarkerObjectNameInfoExt(Device, "my device"));
        }

        [Fact]
        public void DebugMarkerSetObjectTagExt_Succeeds()
        {
            if (!AvailableDeviceExtensions.Contains(Constant.DeviceExtension.ExtDebugMarker)) return;

            Device.DebugMarkerSetObjectTagExt(new DebugMarkerObjectTagInfoExt(Device, 1, new byte[] { 0xFF }));
            Device.DebugMarkerSetObjectTagExt(new DebugMarkerObjectTagInfoExt(GraphicsQueue, 2, new byte[] { 0xFF }));
        }

        [Fact]
        public void CreateCommandPool_Succeeds()
        {
            var createInfo = new CommandPoolCreateInfo(0);
            using (Device.CreateCommandPool(createInfo)) { }
            using (Device.CreateCommandPool(createInfo, CustomAllocator)) { }
        }

        [Fact]
        public void CreateQueryPool_Succeeds()
        {
            var createInfo = new QueryPoolCreateInfo(QueryType.Timestamp, 1);
            using (Device.CreateQueryPool(createInfo)) { }
            using (Device.CreateQueryPool(createInfo, CustomAllocator)) { }
        }

        [Fact]
        public void CreateShaderModule_Succeeds()
        {
            var createInfo = new ShaderModuleCreateInfo(File.ReadAllBytes("Shaders\\shader.vert.spv"));
            using (Device.CreateShaderModule(createInfo)) { }
            using (Device.CreateShaderModule(createInfo, CustomAllocator)) { }
        }

        [Fact]
        public void CreateGraphicsPipeline_Succeeds()
        {
            var attachment = new AttachmentDescription
            {
                Samples = SampleCounts.Count1,
                Format = Format.B8G8R8A8UNorm,
                InitialLayout = ImageLayout.Undefined,
                FinalLayout = ImageLayout.PresentSrcKhr,
                LoadOp = AttachmentLoadOp.Clear,
                StoreOp = AttachmentStoreOp.Store,
                StencilLoadOp = AttachmentLoadOp.DontCare,
                StencilStoreOp = AttachmentStoreOp.DontCare
            };
            var subpass = new SubpassDescription(new[] { new AttachmentReference(0, ImageLayout.ColorAttachmentOptimal) });
            var createInfo = new RenderPassCreateInfo(new[] { subpass }, new[] { attachment });

            using (PipelineCache cache = Device.CreatePipelineCache())
            using (RenderPass renderPass = Device.CreateRenderPass(createInfo))
            using (ShaderModule vertexShader = Device.CreateShaderModule(
                new ShaderModuleCreateInfo(File.ReadAllBytes("Shaders\\shader.vert.spv"))))
            using (ShaderModule fragmentShader = Device.CreateShaderModule(
                new ShaderModuleCreateInfo(File.ReadAllBytes("Shaders\\shader.frag.spv"))))
            {
                var shaderStageCreateInfos = new[]
                {
                    new PipelineShaderStageCreateInfo(ShaderStages.Vertex, vertexShader, "main"),
                    new PipelineShaderStageCreateInfo(ShaderStages.Fragment, fragmentShader, "main")
                };
                
                var vertexInputStateCreateInfo = new PipelineVertexInputStateCreateInfo();
                var inputAssemblyStateCreateInfo = new PipelineInputAssemblyStateCreateInfo
                {
                    Topology = PrimitiveTopology.TriangleList
                };
                var viewport = new Viewport(0, 0, 32, 32);
                var scissor = new Rect2D(Offset2D.Zero, new Extent2D(32, 32));
                var viewportStateCreateInfo = new PipelineViewportStateCreateInfo
                {
                    Viewports = new[] { viewport },
                    Scissors = new[] { scissor }
                };
                var rasterizationStateCreateInfo = new PipelineRasterizationStateCreateInfo
                {
                    PolygonMode = PolygonMode.Fill,
                    CullMode = CullModes.Back,
                    FrontFace = FrontFace.CounterClockwise,
                    LineWidth = 1.0f
                };
                var tessellationStateCreateInfo = new PipelineTessellationStateCreateInfo(4);
                var multisampleStateCreateInfo = new PipelineMultisampleStateCreateInfo
                {
                    RasterizationSamples = SampleCounts.Count1,
                    MinSampleShading = 1.0f
                };
                var colorBlendAttachmentState = new PipelineColorBlendAttachmentState
                {
                    SrcColorBlendFactor = BlendFactor.One,
                    DstColorBlendFactor = BlendFactor.Zero,
                    ColorBlendOp = BlendOp.Add,
                    SrcAlphaBlendFactor = BlendFactor.One,
                    DstAlphaBlendFactor = BlendFactor.Zero,
                    AlphaBlendOp = BlendOp.Add,
                    ColorWriteMask = ColorComponents.All
                };
                var depthStencilStateCreateInfo = new PipelineDepthStencilStateCreateInfo();                
                var colorBlendStateCreateInfo = new PipelineColorBlendStateCreateInfo(
                    new[] { colorBlendAttachmentState });
                var dynamicStateCreateInfo = new PipelineDynamicStateCreateInfo(DynamicState.LineWidth);

                using (PipelineLayout layout = Device.CreatePipelineLayout())
                {
                    var pipelineCreateInfo = new GraphicsPipelineCreateInfo(
                        layout, renderPass, 0,
                        shaderStageCreateInfos,
                        inputAssemblyStateCreateInfo,
                        vertexInputStateCreateInfo,
                        rasterizationStateCreateInfo,
                        tessellationStateCreateInfo,
                        viewportStateCreateInfo,
                        multisampleStateCreateInfo,
                        depthStencilStateCreateInfo,
                        colorBlendStateCreateInfo,
                        dynamicStateCreateInfo);
                    using (Device.CreateGraphicsPipelines(new[] { pipelineCreateInfo })[0]) { }
                    using (Device.CreateGraphicsPipelines(new[] { pipelineCreateInfo }, cache)[0]) { }
                    using (Device.CreateGraphicsPipelines(new[] { pipelineCreateInfo }, allocator: CustomAllocator)[0]) { }
                    using (Device.CreateGraphicsPipelines(new[] { pipelineCreateInfo }, cache, CustomAllocator)[0]) { }
                    using (Device.CreateGraphicsPipeline(pipelineCreateInfo)) { }
                    using (Device.CreateGraphicsPipeline(pipelineCreateInfo, allocator: CustomAllocator)) { }
                    using (Device.CreateGraphicsPipeline(pipelineCreateInfo, cache)) { }
                    using (Device.CreateGraphicsPipeline(pipelineCreateInfo, cache, CustomAllocator)) { }
                }
            }
        }

        [Fact]
        public void CreateComputePipeline_Succeeds()
        {
            var descriptorSetLayoutCreateInfo = new DescriptorSetLayoutCreateInfo(
                new DescriptorSetLayoutBinding(0, DescriptorType.StorageBuffer, 1, ShaderStages.Compute),
                new DescriptorSetLayoutBinding(1, DescriptorType.StorageBuffer, 1, ShaderStages.Compute));
            using (DescriptorSetLayout descriptorSetLayout = Device.CreateDescriptorSetLayout(descriptorSetLayoutCreateInfo))
            using (PipelineLayout pipelineLayout = Device.CreatePipelineLayout(new PipelineLayoutCreateInfo(new[] { descriptorSetLayout })))
            using (ShaderModule shader = Device.CreateShaderModule(new ShaderModuleCreateInfo(File.ReadAllBytes("Shaders\\shader.comp.spv"))))
            using (PipelineCache cache = Device.CreatePipelineCache())
            {
                var pipelineCreateInfo = new ComputePipelineCreateInfo(
                    new PipelineShaderStageCreateInfo(ShaderStages.Compute, shader, "main"),
                    pipelineLayout);

                using (Device.CreateComputePipelines(new[] { pipelineCreateInfo })[0]) { }
                using (Device.CreateComputePipelines(new[] { pipelineCreateInfo }, cache)[0]) { }
                using (Device.CreateComputePipelines(new[] { pipelineCreateInfo }, allocator: CustomAllocator)[0]) { }
                using (Device.CreateComputePipelines(new[] { pipelineCreateInfo }, cache, CustomAllocator)[0]) { }
                using (Device.CreateComputePipeline(pipelineCreateInfo)) { }
                using (Device.CreateComputePipeline(pipelineCreateInfo, allocator: CustomAllocator)) { }
                using (Device.CreateComputePipeline(pipelineCreateInfo, cache)) { }
                using (Device.CreateComputePipeline(pipelineCreateInfo, cache, CustomAllocator)) { }
            }
        }

        [Fact]
        public void FlushMappedMemoryRange_Succeeds()
        {
            PhysicalDeviceMemoryProperties memoryProperties = PhysicalDevice.GetMemoryProperties();
            int memoryTypeIndex = memoryProperties.MemoryTypes.Select((mem, i) => (mem, i))
                .First(x => x.Item1.PropertyFlags.HasFlag(MemoryProperties.HostVisible)).Item2;

            const int size = 1024;
            using (var memory = Device.AllocateMemory(new MemoryAllocateInfo(size, memoryTypeIndex)))
            {
                memory.Map(0, size);
                Device.FlushMappedMemoryRange(new MappedMemoryRange(memory, 0, size));
                Device.FlushMappedMemoryRanges(new MappedMemoryRange(memory, 0, size));
                memory.Unmap();
            }
        }

        [Fact]
        public void InvalidateMappedMemoryRange_Succeeds()
        {
            PhysicalDeviceMemoryProperties memoryProperties = PhysicalDevice.GetMemoryProperties();
            int memoryTypeIndex = memoryProperties.MemoryTypes.Select((mem, i) => (mem, i))
                .First(x => x.Item1.PropertyFlags.HasFlag(MemoryProperties.HostVisible)).Item2;

            const int size = 1024;
            using (var memory = Device.AllocateMemory(new MemoryAllocateInfo(size, memoryTypeIndex)))
            {
                memory.Map(0, size);
                Device.InvalidateMappedMemoryRange(new MappedMemoryRange(memory, 0, size));
                Device.InvalidateMappedMemoryRanges(new MappedMemoryRange(memory, 0, size));
                memory.Unmap();
            }
        }

        public DeviceTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }

        private delegate Result DebugMarkerSetObjectNameExt(IntPtr p1, IntPtr p2);
    }
}
