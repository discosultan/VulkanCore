using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using VulkanCore.Khr;

namespace VulkanCore.Samples.Cube
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WorldViewProjection
    {
        public Matrix4x4 World;
        public Matrix4x4 View;
        public Matrix4x4 Projection;
    }

    public class CubeApp : VulkanApp
    {
        private RenderPass _renderPass;
        private ImageView[] _imageViews;
        private Framebuffer[] _framebuffers;
        private PipelineLayout _pipelineLayout;
        private Pipeline _pipeline;
        private DescriptorSetLayout _descriptorSetLayout;
        private DescriptorPool _descriptorPool;
        private DescriptorSet _descriptorSet;

        private Buffer _uniformBuffer;
        private DeviceMemory _uniformBufferMemory;

        private Image _depthStencil;
        private ImageView _depthStencilView;
        private DeviceMemory _depthStencilMemory;

        private Image _texture;
        private ImageView _textureView;
        private DeviceMemory _textureMemory;

        private Sampler _sampler;

        private Cube _cube;
        private WorldViewProjection _wvp;

        public CubeApp(IntPtr hInstance, IWindow window) : base(hInstance, window)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            _cube = new Cube(this);
            _cube.Initialize();
            CreateDepthStencil();
            CreateSampler();
            LoadTexture("Textures\\IndustryForgedDark512.ktx", Format.R5G5B5A1UNormPack16);
            CreateRenderPass();
            CreateFramebuffers();
            CreateUniformBuffers();
            CreateDescriptorSetAndPipelineLayouts();
            CreateDescriptorSet();
            CreateGraphicsPipeline();
            RecordCommandBuffers();
            SetViewProjection();
        }

        private void CreateSampler()
        {
            _sampler = Device.CreateSampler(new SamplerCreateInfo
            {
                MagFilter = Filter.Linear,
                MinFilter = Filter.Linear,
                MipmapMode = SamplerMipmapMode.Linear
            });
        }

        private void LoadTexture(string path, Format format)
        {
            FormatProperties formatProps = PhysicalDevice.GetFormatProperties(format);

            KtxTextureData tex2D = KtxLoader.Load(path);

            Buffer stagingBuffer = Device.CreateBuffer(
                new BufferCreateInfo(tex2D.Mipmaps[0].Size, BufferUsages.TransferSrc));
            MemoryRequirements stagingMemReq = stagingBuffer.GetMemoryRequirements();
            int heapIndex = PhysicalDeviceMemoryProperties.GetMemoryTypeIndex(
                stagingMemReq.MemoryTypeBits, MemoryProperties.HostVisible);
            DeviceMemory stagingMemory = Device.AllocateMemory(
                new MemoryAllocateInfo(stagingMemReq.Size, heapIndex));
            stagingBuffer.BindMemory(stagingMemory);

            IntPtr ptr = stagingMemory.Map(0, stagingMemReq.Size);
            Interop.Write(ptr, tex2D.Mipmaps[0].Data);
            stagingMemory.Unmap();

            // Setup buffer copy regions for each mip level.
            var bufferCopyRegions = new BufferImageCopy[tex2D.Mipmaps.Length];
            int offset = 0;
            for (int i = 0; i < bufferCopyRegions.Length; i++)
            {
                bufferCopyRegions = new[] 
                {
                    new BufferImageCopy
                    {
                        ImageSubresource = new ImageSubresourceLayers(ImageAspects.Color, i, 0, 1),
                        ImageExtent = tex2D.Mipmaps[0].Extent,
                        BufferOffset = offset
                    }
                };
                offset += tex2D.Mipmaps[i].Size;
            }

            // Create optimal tiled target image.
            _texture = Device.CreateImage(new ImageCreateInfo
            {
                ImageType = ImageType.Image2D,
                Format = format,
                MipLevels = tex2D.Mipmaps.Length,
                ArrayLayers = 1,
                Samples = SampleCounts.Count1,
                Tiling = ImageTiling.Optimal,
                SharingMode = SharingMode.Exclusive,
                InitialLayout = ImageLayout.Undefined,
                Extent = tex2D.Mipmaps[0].Extent,
                Usage = ImageUsages.Sampled | ImageUsages.TransferDst
            });
            MemoryRequirements imageMemReq = _texture.GetMemoryRequirements();
            int imageHeapIndex = PhysicalDeviceMemoryProperties.GetMemoryTypeIndex(
                imageMemReq.MemoryTypeBits, MemoryProperties.DeviceLocal);
            _textureMemory = Device.AllocateMemory(new MemoryAllocateInfo(imageMemReq.Size, imageHeapIndex));
            _texture.BindMemory(_textureMemory);

            var subresourceRange = new ImageSubresourceRange(ImageAspects.Color, levelCount: tex2D.Mipmaps.Length);

            // Copy the data from staging buffers to device local buffers.
            CommandBuffer cmdBuffer = CommandPool.AllocateBuffers(new CommandBufferAllocateInfo(CommandBufferLevel.Primary, 1))[0];
            cmdBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.OneTimeSubmit));
            cmdBuffer.CmdPipelineBarrier(PipelineStages.TopOfPipe, PipelineStages.TopOfPipe,
                imageMemoryBarriers: new[]
                {
                    new ImageMemoryBarrier(
                        _texture, subresourceRange,
                        0, Accesses.TransferWrite, 
                        ImageLayout.Undefined, ImageLayout.TransferDstOptimal)
                });
            cmdBuffer.CmdCopyBufferToImage(stagingBuffer, _texture, ImageLayout.TransferDstOptimal, bufferCopyRegions);
            cmdBuffer.CmdPipelineBarrier(PipelineStages.TopOfPipe, PipelineStages.TopOfPipe,
                imageMemoryBarriers: new[]
                {
                    new ImageMemoryBarrier(
                        _texture, subresourceRange,
                        Accesses.TransferWrite, Accesses.ShaderRead,
                        ImageLayout.TransferDstOptimal, ImageLayout.ShaderReadOnlyOptimal)
                });
            cmdBuffer.End();

            // Submit.
            Fence fence = Device.CreateFence();
            GraphicsQueue.Submit(new SubmitInfo(commandBuffers: new[] { cmdBuffer }), fence);
            fence.Wait();

            // Cleanup staging resources.
            stagingMemory.Dispose();
            stagingBuffer.Dispose();

            // Create image view.
            _textureView = _texture.CreateView(new ImageViewCreateInfo(format, subresourceRange));
        }

        protected override void OnResized()
        {
            base.OnResized();

            _depthStencilView.Dispose();
            _depthStencilMemory.Dispose();
            _depthStencil.Dispose();
            _pipeline.Dispose();
            Array.ForEach(_framebuffers, framebuffer => framebuffer.Dispose());
            Array.ForEach(_imageViews, imageView => imageView.Dispose());

            CreateDepthStencil();
            CreateFramebuffers();
            CreateGraphicsPipeline();
            RecordCommandBuffers();
            SetViewProjection();
        }

        public override void Dispose()
        {
            _uniformBufferMemory.Dispose();
            _uniformBuffer.Dispose();
            _descriptorPool.Dispose();
            _descriptorSetLayout.Dispose();
            _pipeline.Dispose();
            _pipelineLayout.Dispose();
            Array.ForEach(_framebuffers, framebuffer => framebuffer.Dispose());
            Array.ForEach(_imageViews, imageView => imageView.Dispose());
            _renderPass.Dispose();
            _depthStencilView.Dispose();
            _depthStencilMemory.Dispose();
            _depthStencil.Dispose();
            _cube.Dispose();
            base.Dispose();
        }

        protected override void Update(Timer timer)
        {
            const float twoPi = (float)Math.PI * 2.0f;
            const float yawSpeed   = 1.0f;
            const float pitchSpeed = 0.0f;
            const float rollSpeed  = 1.0f;

            _wvp.World = Matrix4x4.CreateFromYawPitchRoll(
                (timer.TotalTime * yawSpeed) % twoPi,
                (timer.TotalTime * pitchSpeed) % twoPi,
                (timer.TotalTime * rollSpeed) % twoPi);

            UpdateUniformBuffers();
        }

        protected override void Draw(Timer timer)
        {
            // Acquire drawing image.
            int imageIndex = Swapchain.AcquireNextImage(semaphore: ImageAvailableSemaphore);

            GraphicsQueue.Submit(new SubmitInfo(
                new[] { ImageAvailableSemaphore },
                new[] { PipelineStages.ColorAttachmentOutput },
                new[] { CommandBuffers[imageIndex] },
                new[] { RenderingFinishedSemaphore }
            ));

            PresentQueue.PresentKhr(new PresentInfoKhr(
                new[] { RenderingFinishedSemaphore },
                new[] { Swapchain },
                new[] { imageIndex }
            ));
        }

        private void SetViewProjection()
        {
            const float cameraDistance = 2.5f;
            _wvp.View = Matrix4x4.CreateLookAt(Vector3.UnitZ * cameraDistance, Vector3.Zero, Vector3.UnitY);
            _wvp.Projection = Matrix4x4.CreatePerspectiveFieldOfView(
                (float)Math.PI / 4,
                (float)Window.Width / Window.Height,
                1.0f, 1000.0f);
        }

        private void CreateDepthStencil()
        {
            _depthStencil = Device.CreateImage(new ImageCreateInfo
            {
                ImageType = ImageType.Image2D,
                Format = Format.D24UNormS8UInt,
                Extent = new Extent3D(Window.Width, Window.Height, 1),
                MipLevels = 1,
                ArrayLayers = 1,
                Samples = SampleCounts.Count1,
                Tiling = ImageTiling.Optimal,
                Usage = ImageUsages.DepthStencilAttachment | ImageUsages.TransferSrc
            });
            MemoryRequirements memReq = _depthStencil.GetMemoryRequirements();
            int heapIndex = PhysicalDeviceMemoryProperties.GetMemoryTypeIndex(
                memReq.MemoryTypeBits, MemoryProperties.DeviceLocal);
            _depthStencilMemory = Device.AllocateMemory(new MemoryAllocateInfo(memReq.Size, heapIndex));
            _depthStencil.BindMemory(_depthStencilMemory);
            _depthStencilView = _depthStencil.CreateView(new ImageViewCreateInfo(Format.D24UNormS8UInt,
                new ImageSubresourceRange(ImageAspects.Depth | ImageAspects.Stencil)));
        }

        private void CreateUniformBuffers()
        {
            _uniformBuffer = Device.CreateBuffer(new BufferCreateInfo(Interop.SizeOf<WorldViewProjection>(), BufferUsages.UniformBuffer));
            MemoryRequirements memoryRequirements = _uniformBuffer.GetMemoryRequirements();
            // We require host visible memory so we can map it and write to it directly.
            // We require host coherent memory so that writes are visible to the GPU right after unmapping it.
            int memoryTypeIndex = PhysicalDeviceMemoryProperties.GetMemoryTypeIndex(
                memoryRequirements.MemoryTypeBits,
                MemoryProperties.HostVisible | MemoryProperties.HostCoherent);
            _uniformBufferMemory = Device.AllocateMemory(new MemoryAllocateInfo(memoryRequirements.Size, memoryTypeIndex));
            _uniformBuffer.BindMemory(_uniformBufferMemory);
        }

        private void UpdateUniformBuffers()
        {
            IntPtr ptr = _uniformBufferMemory.Map(0, Interop.SizeOf<WorldViewProjection>());
            Interop.Write(ptr, ref _wvp);
            _uniformBufferMemory.Unmap();
        }

        private void CreateDescriptorSet()
        {
            var descriptorPoolSizes = new[]
            {
                new DescriptorPoolSize(DescriptorType.UniformBuffer, 1),
                new DescriptorPoolSize(DescriptorType.CombinedImageSampler, 1)
            };
            _descriptorPool = Device.CreateDescriptorPool(
                new DescriptorPoolCreateInfo(descriptorPoolSizes.Length, descriptorPoolSizes));

            _descriptorSet = _descriptorPool.AllocateSets(new DescriptorSetAllocateInfo(1, _descriptorSetLayout))[0];

            // Update the descriptor set for the shader binding point.
            var writeDescriptorSets = new[]
            {
                new WriteDescriptorSet(_descriptorSet, 0, 0, 1, DescriptorType.UniformBuffer,
                    bufferInfo: new[] { new DescriptorBufferInfo(_uniformBuffer) }),
                new WriteDescriptorSet(_descriptorSet, 1, 0, 1, DescriptorType.CombinedImageSampler,
                    imageInfo: new[] { new DescriptorImageInfo(_sampler, _textureView, ImageLayout.General) })
            };
            _descriptorPool.UpdateSets(writeDescriptorSets);
        }

        private void CreateDescriptorSetAndPipelineLayouts()
        {
            _descriptorSetLayout = Device.CreateDescriptorSetLayout(new DescriptorSetLayoutCreateInfo(
                new DescriptorSetLayoutBinding(0, DescriptorType.UniformBuffer, 1, ShaderStages.Vertex),
                new DescriptorSetLayoutBinding(1, DescriptorType.CombinedImageSampler, 1, ShaderStages.Fragment)));

            var layoutCreateInfo = new PipelineLayoutCreateInfo(new[] { _descriptorSetLayout });
            _pipelineLayout = Device.CreatePipelineLayout(layoutCreateInfo);
        }

        private void CreateRenderPass()
        {
            var attachments = new[]
            {
                // Color attachment.
                new AttachmentDescription
                {
                    Format = Swapchain.Format,
                    Samples = SampleCounts.Count1,
                    LoadOp = AttachmentLoadOp.Clear,
                    StoreOp = AttachmentStoreOp.Store,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare,
                    InitialLayout = ImageLayout.Undefined,
                    FinalLayout = ImageLayout.PresentSrcKhr
                },
                // Depth attachment.
                new AttachmentDescription
                {
                    Format = Format.D24UNormS8UInt,
                    Samples = SampleCounts.Count1,
                    LoadOp = AttachmentLoadOp.Clear,
                    StoreOp = AttachmentStoreOp.DontCare,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare,
                    InitialLayout = ImageLayout.Undefined,
                    FinalLayout = ImageLayout.DepthStencilAttachmentOptimal
                }
            };
            var subpasses = new[]
            {
                new SubpassDescription(
                    colorAttachments: new[] { new AttachmentReference(0, ImageLayout.ColorAttachmentOptimal) },
                    depthStencilAttachment: new AttachmentReference(1, ImageLayout.DepthStencilAttachmentOptimal))
            };
            var dependencies = new[]
            {
                new SubpassDependency
                {
                    SrcSubpass = Constant.SubpassExternal,
                    DstSubpass = 0,
                    SrcStageMask = PipelineStages.BottomOfPipe,
                    DstStageMask = PipelineStages.ColorAttachmentOutput,
                    SrcAccessMask = Accesses.MemoryRead,
                    DstAccessMask = Accesses.ColorAttachmentRead | Accesses.ColorAttachmentWrite,
                    DependencyFlags = Dependencies.ByRegion
                },
                new SubpassDependency
                {
                    SrcSubpass = 0,
                    DstSubpass = Constant.SubpassExternal,
                    SrcStageMask = PipelineStages.ColorAttachmentOutput,
                    DstStageMask = PipelineStages.BottomOfPipe,
                    SrcAccessMask = Accesses.ColorAttachmentRead | Accesses.ColorAttachmentWrite,
                    DstAccessMask = Accesses.MemoryRead,
                    DependencyFlags = Dependencies.ByRegion
                }
            };

            var createInfo = new RenderPassCreateInfo(subpasses, attachments, dependencies);
            _renderPass = Device.CreateRenderPass(createInfo);
        }

        private void CreateFramebuffers()
        {
            _imageViews = new ImageView[SwapchainImages.Length];
            _framebuffers = new Framebuffer[SwapchainImages.Length];

            for (int i = 0; i < SwapchainImages.Length; i++)
            {
                _imageViews[i] = SwapchainImages[i].CreateView(new ImageViewCreateInfo(
                    Swapchain.Format,
                    new ImageSubresourceRange(ImageAspects.Color)));
                _framebuffers[i] = _renderPass.CreateFramebuffer(new FramebufferCreateInfo(
                    new[] { _imageViews[i], _depthStencilView },
                    Window.Width,
                    Window.Height));
            }
        }

        private void CreateGraphicsPipeline()
        {
            // Create shader modules. Shader modules are one of the objects required to create the
            // graphics pipeline. But after the pipeline is created, we don't need these shader
            // modules anymore, so we dispose them.
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

                var vertexInputStateCreateInfo = new PipelineVertexInputStateCreateInfo
                {
                    VertexBindingDescriptions = new[]
                    {
                        new VertexInputBindingDescription(0, Interop.SizeOf<Vertex>(), VertexInputRate.Vertex)
                    },
                    VertexAttributeDescriptions = new[]
                    {
                        new VertexInputAttributeDescription(0, 0, Format.R32G32B32SFloat, 0),  // Position.
                        new VertexInputAttributeDescription(1, 0, Format.R32G32B32SFloat, 12), // Normal.
                        new VertexInputAttributeDescription(2, 0, Format.R32G32SFloat, 24)     // TexCoord.
                    }
                };
                var inputAssemblyStateCreateInfo = new PipelineInputAssemblyStateCreateInfo
                {
                    Topology = PrimitiveTopology.TriangleList
                };
                var viewport = new Viewport(0, 0, Window.Width, Window.Height);
                var scissor = new Rect2D(Offset2D.Zero, new Extent2D(Window.Width, Window.Height));
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
                var multisampleStateCreateInfo = new PipelineMultisampleStateCreateInfo
                {
                    RasterizationSamples = SampleCounts.Count1,
                    MinSampleShading = 1.0f
                };
                var depthStencilCreateInfo = new PipelineDepthStencilStateCreateInfo
                {
                    DepthTestEnable = true,
                    DepthWriteEnable = true,
                    DepthCompareOp = CompareOp.LessOrEqual,
                    Back = new StencilOpState
                    {
                        FailOp = StencilOp.Keep,
                        PassOp = StencilOp.Keep,
                        CompareOp = CompareOp.Always
                    },
                    Front = new StencilOpState
                    {
                        FailOp = StencilOp.Keep,
                        PassOp = StencilOp.Keep,
                        CompareOp = CompareOp.Always
                    }
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
                var colorBlendStateCreateInfo = new PipelineColorBlendStateCreateInfo(
                    new[] { colorBlendAttachmentState });

                var pipelineCreateInfo = new GraphicsPipelineCreateInfo(
                    _pipelineLayout, _renderPass, 0,
                    shaderStageCreateInfos,
                    inputAssemblyStateCreateInfo,
                    vertexInputStateCreateInfo,
                    rasterizationStateCreateInfo,
                    viewportState: viewportStateCreateInfo,
                    multisampleState: multisampleStateCreateInfo,
                    depthStencilState: depthStencilCreateInfo,
                    colorBlendState: colorBlendStateCreateInfo);
                _pipeline = Device.CreateGraphicsPipeline(pipelineCreateInfo);
            }
        }

        private void RecordCommandBuffers()
        {
            var subresourceRange = new ImageSubresourceRange(ImageAspects.Color);
            for (int i = 0; i < CommandBuffers.Length; i++)
            {
                CommandBuffer cmdBuffer = CommandBuffers[i];
                cmdBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.SimultaneousUse));

                if (PresentQueue != GraphicsQueue)
                {
                    var barrierFromPresentToDraw = new ImageMemoryBarrier(
                        SwapchainImages[i], subresourceRange,
                        Accesses.MemoryRead, Accesses.ColorAttachmentWrite,
                        ImageLayout.Undefined, ImageLayout.PresentSrcKhr,
                        PresentQueue.FamilyIndex, GraphicsQueue.FamilyIndex);

                    cmdBuffer.CmdPipelineBarrier(
                        PipelineStages.ColorAttachmentOutput,
                        PipelineStages.ColorAttachmentOutput,
                        imageMemoryBarriers: new[] { barrierFromPresentToDraw });
                }

                var renderPassBeginInfo = new RenderPassBeginInfo(
                    _framebuffers[i],
                    new Rect2D(Offset2D.Zero, new Extent2D(Window.Width, Window.Height)),
                    new ClearColorValue(new ColorF4(0.39f, 0.58f, 0.93f, 1.0f)),
                    new ClearDepthStencilValue(1.0f, 0));

                cmdBuffer.CmdBeginRenderPass(renderPassBeginInfo);
                cmdBuffer.CmdBindDescriptorSet(PipelineBindPoint.Graphics, _pipelineLayout, _descriptorSet);
                cmdBuffer.CmdBindPipeline(PipelineBindPoint.Graphics, _pipeline);
                cmdBuffer.CmdBindVertexBuffer(_cube.VertexBuffer);
                cmdBuffer.CmdBindIndexBuffer(_cube.IndexBuffer);
                cmdBuffer.CmdDrawIndexed(_cube.IndexCount, 1, 0, 0, 0);
                cmdBuffer.CmdEndRenderPass();

                if (PresentQueue != GraphicsQueue)
                {
                    var barrierFromDrawToPresent = new ImageMemoryBarrier(
                        SwapchainImages[i], subresourceRange,
                        Accesses.ColorAttachmentWrite, Accesses.MemoryRead,
                        ImageLayout.PresentSrcKhr, ImageLayout.PresentSrcKhr,
                        GraphicsQueue.FamilyIndex, PresentQueue.FamilyIndex);

                    cmdBuffer.CmdPipelineBarrier(
                        PipelineStages.ColorAttachmentOutput,
                        PipelineStages.BottomOfPipe,
                         imageMemoryBarriers: new[] { barrierFromDrawToPresent });
                }

                cmdBuffer.End();
            }
        }
    }
}
