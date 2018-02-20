using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a physical device object.
    /// <para>
    /// Vulkan separates the concept of physical and logical devices. A physical device usually
    /// represents a single device in a system (perhaps made up of several individual hardware
    /// devices working together), of which there are a finite number. A logical device represents an
    /// application's view of the device.
    /// </para>
    /// </summary>
    public unsafe class PhysicalDevice : VulkanHandle<IntPtr>
    {
        internal PhysicalDevice(IntPtr handle, Instance parent)
        {
            Handle = handle;
            Parent = parent;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Instance Parent { get; }

        /// <summary>
        /// Returns properties of a physical device.
        /// </summary>
        /// <returns>Properties of a physical device.</returns>
        public PhysicalDeviceProperties GetProperties()
        {
            PhysicalDeviceProperties.Native native;
            vkGetPhysicalDeviceProperties(this, &native);
            PhysicalDeviceProperties.FromNative(ref native, out var properties);
            return properties;
        }

        /// <summary>
        /// Reports properties of the queues of the physical device.
        /// </summary>
        /// <returns>Properties of the queues of the physical device.</returns>
        public QueueFamilyProperties[] GetQueueFamilyProperties()
        {
            int count;
            vkGetPhysicalDeviceQueueFamilyProperties(Handle, &count, null);

            var queueFamilyProperties = new QueueFamilyProperties[count];
            fixed (QueueFamilyProperties* queueFamilyPropertiesPtr = queueFamilyProperties)
                vkGetPhysicalDeviceQueueFamilyProperties(this, &count, queueFamilyPropertiesPtr);
            return queueFamilyProperties;
        }

        /// <summary>
        /// Reports memory information for physical device.
        /// </summary>
        /// <returns>Structure in which the properties are returned.</returns>
        public PhysicalDeviceMemoryProperties GetMemoryProperties()
        {
            var nativeProperties = new PhysicalDeviceMemoryProperties.Native();
            vkGetPhysicalDeviceMemoryProperties(this, ref nativeProperties);
            PhysicalDeviceMemoryProperties.FromNative(ref nativeProperties, out var properties);
            return properties;
        }

        /// <summary>
        /// Reports capabilities of a physical device.
        /// </summary>
        /// <returns>Capabilities of a physical device.</returns>
        public PhysicalDeviceFeatures GetFeatures()
        {
            PhysicalDeviceFeatures features;
            vkGetPhysicalDeviceFeatures(this, &features);
            return features;
        }

        /// <summary>
        /// Lists physical device's format capabilities.
        /// </summary>
        /// <param name="format">The format whose properties are queried.</param>
        /// <returns>Format capabilities of a physical device.</returns>
        public FormatProperties GetFormatProperties(Format format)
        {
            FormatProperties properties;
            vkGetPhysicalDeviceFormatProperties(this, format, &properties);
            return properties;
        }

        /// <summary>
        /// Lists physical device's image format capabilities.
        /// </summary>
        /// <param name="format">The image format, corresponding to <see cref="ImageCreateInfo.Format"/>.</param>
        /// <param name="type">The image type, corresponding to <see cref="ImageCreateInfo.ImageType"/>.</param>
        /// <param name="tiling">The image tiling, corresponding to <see cref="ImageCreateInfo.Tiling"/>.</param>
        /// <param name="usages">The intended usage of the image, corresponding to <see cref="ImageCreateInfo.Usage"/>.</param>
        /// <param name="flags">
        /// A bitmask describing additional parameters of the image, corresponding to <see cref="ImageCreateInfo.Flags"/>.
        /// </param>
        /// <returns>Image format capabilities of a physical device</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public ImageFormatProperties GetImageFormatProperties(Format format, ImageType type,
            ImageTiling tiling, ImageUsages usages, ImageCreateFlags flags = 0)
        {
            ImageFormatProperties properties;
            Result result = vkGetPhysicalDeviceImageFormatProperties(
                this, format, type, tiling, usages, flags, &properties);
            VulkanException.ThrowForInvalidResult(result);
            return properties;
        }

        /// <summary>
        /// Returns properties of available physical device extensions.
        /// </summary>
        /// <param name="layerName">
        /// Is either <c>null</c> or a unicode string naming the layer to retrieve extensions from.
        /// When parameter is <c>null</c>, only extensions provided by the Vulkan implementation or
        /// by implicitly enabled layers are returned.
        /// </param>
        /// <returns>Properties of available extensions for layer.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public ExtensionProperties[] EnumerateExtensionProperties(string layerName = null)
        {
            int dstLayerNameByteCount = Interop.String.GetMaxByteCount(layerName);
            var dstLayerNamePtr = stackalloc byte[dstLayerNameByteCount];
            Interop.String.ToPointer(layerName, dstLayerNamePtr, dstLayerNameByteCount);

            int count;
            Result result = vkEnumerateDeviceExtensionProperties(this, dstLayerNamePtr, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var propertiesPtr = stackalloc ExtensionProperties.Native[count];
            result = vkEnumerateDeviceExtensionProperties(this, dstLayerNamePtr, &count, propertiesPtr);
            VulkanException.ThrowForInvalidResult(result);

            var properties = new ExtensionProperties[count];
            for (int i = 0; i < count; i++)
                ExtensionProperties.FromNative(ref propertiesPtr[i], out properties[i]);
            return properties;
        }

        /// <summary>
        /// Returns properties of available physical device layers.
        /// </summary>
        /// <returns>Properties of available layers.</returns>
        /// <exception cref="VulkanException"></exception>
        public LayerProperties[] EnumerateLayerProperties()
        {
            int count;
            Result result = vkEnumerateDeviceLayerProperties(this, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var nativePropertiesPtr = stackalloc LayerProperties.Native[count];
            result = vkEnumerateDeviceLayerProperties(this, &count, nativePropertiesPtr);
            VulkanException.ThrowForInvalidResult(result);

            var properties = new LayerProperties[count];
            for (int i = 0; i < count; i++)
                properties[i] = LayerProperties.FromNative(ref nativePropertiesPtr[i]);
            return properties;
        }

        /// <summary>
        /// Create a new device instance. A logical device is created as a connection to a physical device.
        /// </summary>
        /// <param name="createInfo">
        /// The structure containing information about how to create the device.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>Device instance.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Device CreateDevice(DeviceCreateInfo createInfo, AllocationCallbacks? allocator = null)
        {
            return new Device(this, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Retrieve properties of an image format applied to sparse images.
        /// </summary>
        /// <param name="format">The image format.</param>
        /// <param name="type">The dimensionality of image.</param>
        /// <param name="samples">The number of samples per pixel.</param>
        /// <param name="usage">The intended usage of the image.</param>
        /// <param name="tiling">The tiling arrangement of the data elements in memory.</param>
        /// <returns>Properties of an image format applied to sparse images.</returns>
        public SparseImageFormatProperties[] GetSparseImageFormatProperties(
            Format format, ImageType type, SampleCounts samples, ImageUsages usage, ImageTiling tiling)
        {
            int count;
            vkGetPhysicalDeviceSparseImageFormatProperties(this, format, type, samples, usage, tiling, &count, null);

            var properties = new SparseImageFormatProperties[count];
            fixed (SparseImageFormatProperties* propertiesPtr = properties)
                vkGetPhysicalDeviceSparseImageFormatProperties(this, format, type, samples, usage, tiling, &count, propertiesPtr);
            return properties;
        }

        private delegate void vkGetPhysicalDevicePropertiesDelegate(IntPtr physicalDevice, PhysicalDeviceProperties.Native* properties);
        private static readonly vkGetPhysicalDevicePropertiesDelegate vkGetPhysicalDeviceProperties = VulkanLibrary.GetStaticProc<vkGetPhysicalDevicePropertiesDelegate>(nameof(vkGetPhysicalDeviceProperties));

        private delegate void vkGetPhysicalDeviceQueueFamilyPropertiesDelegate(IntPtr physicalDevice, int* queueFamilyPropertyCount, QueueFamilyProperties* queueFamilyProperties);
        private static readonly vkGetPhysicalDeviceQueueFamilyPropertiesDelegate vkGetPhysicalDeviceQueueFamilyProperties = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceQueueFamilyPropertiesDelegate>(nameof(vkGetPhysicalDeviceQueueFamilyProperties));

        private delegate void vkGetPhysicalDeviceMemoryPropertiesDelegate(IntPtr physicalDevice, ref PhysicalDeviceMemoryProperties.Native memoryProperties);
        private static readonly vkGetPhysicalDeviceMemoryPropertiesDelegate vkGetPhysicalDeviceMemoryProperties = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceMemoryPropertiesDelegate>(nameof(vkGetPhysicalDeviceMemoryProperties));

        private delegate void vkGetPhysicalDeviceFeaturesDelegate(IntPtr physicalDevice, PhysicalDeviceFeatures* features);
        private static readonly vkGetPhysicalDeviceFeaturesDelegate vkGetPhysicalDeviceFeatures = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceFeaturesDelegate>(nameof(vkGetPhysicalDeviceFeatures));

        private delegate void vkGetPhysicalDeviceFormatPropertiesDelegate(IntPtr physicalDevice, Format format, FormatProperties* formatProperties);
        private static readonly vkGetPhysicalDeviceFormatPropertiesDelegate vkGetPhysicalDeviceFormatProperties = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceFormatPropertiesDelegate>(nameof(vkGetPhysicalDeviceFormatProperties));

        private delegate Result vkGetPhysicalDeviceImageFormatPropertiesDelegate(IntPtr physicalDevice, Format format, ImageType type, ImageTiling tiling, ImageUsages usage, ImageCreateFlags flags, ImageFormatProperties* imageFormatProperties);
        private static readonly vkGetPhysicalDeviceImageFormatPropertiesDelegate vkGetPhysicalDeviceImageFormatProperties = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceImageFormatPropertiesDelegate>(nameof(vkGetPhysicalDeviceImageFormatProperties));

        private delegate Result vkEnumerateDeviceLayerPropertiesDelegate(IntPtr physicalDevice, int* propertyCount, LayerProperties.Native* properties);
        private static readonly vkEnumerateDeviceLayerPropertiesDelegate vkEnumerateDeviceLayerProperties = VulkanLibrary.GetStaticProc<vkEnumerateDeviceLayerPropertiesDelegate>(nameof(vkEnumerateDeviceLayerProperties));

        private delegate Result vkEnumerateDeviceExtensionPropertiesDelegate(IntPtr physicalDevice, byte* layerName, int* propertyCount, ExtensionProperties.Native* properties);
        private static readonly vkEnumerateDeviceExtensionPropertiesDelegate vkEnumerateDeviceExtensionProperties = VulkanLibrary.GetStaticProc<vkEnumerateDeviceExtensionPropertiesDelegate>(nameof(vkEnumerateDeviceExtensionProperties));

        private delegate void vkGetPhysicalDeviceSparseImageFormatPropertiesDelegate(IntPtr physicalDevice, Format format, ImageType type, SampleCounts samples, ImageUsages usage, ImageTiling tiling, int* propertyCount, SparseImageFormatProperties* properties);
        private static readonly vkGetPhysicalDeviceSparseImageFormatPropertiesDelegate vkGetPhysicalDeviceSparseImageFormatProperties = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceSparseImageFormatPropertiesDelegate>(nameof(vkGetPhysicalDeviceSparseImageFormatProperties));
    }

    /// <summary>
    /// Structure describing the fine-grained features that can be supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceFeatures
    {
        /// <summary>
        /// Indicates that accesses to buffers are bounds-checked against the range of the buffer
        /// descriptor (as determined by <see cref="DescriptorBufferInfo.Range"/>, <see
        /// cref="BufferViewCreateInfo.Range"/>, or the size of the buffer).
        /// </summary>
        public Bool RobustBufferAccess;
        /// <summary>
        /// Full 32-bit range of indices for indexed draw calls.
        /// </summary>
        public Bool FullDrawIndexUint32;
        /// <summary>
        /// Image views which are arrays of cube maps.
        /// </summary>
        public Bool ImageCubeArray;
        /// <summary>
        /// Blending operations are controlled per-attachment.
        /// </summary>
        public Bool IndependentBlend;
        /// <summary>
        /// Geometry stage.
        /// </summary>
        public Bool GeometryShader;
        /// <summary>
        /// Tessellation control and evaluation stage.
        /// </summary>
        public Bool TessellationShader;
        /// <summary>
        /// Per-sample shading and interpolation.
        /// </summary>
        public Bool SampleRateShading;
        /// <summary>
        /// Blend operations which take two sources.
        /// </summary>
        public Bool DualSrcBlend;
        /// <summary>
        /// Logic operations.
        /// </summary>
        public Bool LogicOp;
        /// <summary>
        /// Multi draw indirect.
        /// </summary>
        public Bool MultiDrawIndirect;
        /// <summary>
        /// Indirect draws can use non-zero firstInstance.
        /// </summary>
        public Bool DrawIndirectFirstInstance;
        /// <summary>
        /// Depth clamping.
        /// </summary>
        public Bool DepthClamp;
        /// <summary>
        /// Depth bias clamping.
        /// </summary>
        public Bool DepthBiasClamp;
        /// <summary>
        /// Point and wireframe fill modes.
        /// </summary>
        public Bool FillModeNonSolid;
        /// <summary>
        /// Depth bounds test.
        /// </summary>
        public Bool DepthBounds;
        /// <summary>
        /// Lines with width greater than 1.
        /// </summary>
        public Bool WideLines;
        /// <summary>
        /// Points with size greater than 1.
        /// </summary>
        public Bool LargePoints;
        /// <summary>
        /// The fragment alpha component can be forced to maximum representable alpha value.
        /// </summary>
        public Bool AlphaToOne;
        /// <summary>
        /// Viewport arrays.
        /// </summary>
        public Bool MultiViewport;
        /// <summary>
        /// Anisotropic sampler filtering.
        /// </summary>
        public Bool SamplerAnisotropy;
        /// <summary>
        /// ETC texture compression formats.
        /// </summary>
        public Bool TextureCompressionETC2;
        /// <summary>
        /// ASTC LDR texture compression formats.
        /// </summary>
        public Bool TextureCompressionASTC_LDR;
        /// <summary>
        /// BC1-7 texture compressed formats.
        /// </summary>
        public Bool TextureCompressionBC;
        /// <summary>
        /// Precise occlusion queries returning actual sample counts.
        /// </summary>
        public Bool OcclusionQueryPrecise;
        /// <summary>
        /// Pipeline statistics query.
        /// </summary>
        public Bool PipelineStatisticsQuery;
        /// <summary>
        /// Stores and atomic ops on storage buffers and images are supported in vertex,
        /// tessellation, and geometry stages.
        /// </summary>
        public Bool VertexPipelineStoresAndAtomics;
        /// <summary>
        /// Stores and atomic ops on storage buffers and images are supported in the fragment stage.
        /// </summary>
        public Bool FragmentStoresAndAtomics;
        /// <summary>
        /// Tessellation and geometry stages can export point size.
        /// </summary>
        public Bool ShaderTessellationAndGeometryPointSize;
        /// <summary>
        /// Image gather with run-time values and independent offsets.
        /// </summary>
        public Bool ShaderImageGatherExtended;
        /// <summary>
        /// The extended set of formats can be used for storage images.
        /// </summary>
        public Bool ShaderStorageImageExtendedFormats;
        /// <summary>
        /// Multisample images can be used for storage images.
        /// </summary>
        public Bool ShaderStorageImageMultisample;
        /// <summary>
        /// Read from storage image does not require format qualifier.
        /// </summary>
        public Bool ShaderStorageImageReadWithoutFormat;
        /// <summary>
        /// Write to storage image does not require format qualifier.
        /// </summary>
        public Bool ShaderStorageImageWriteWithoutFormat;
        /// <summary>
        /// Arrays of uniform buffers can be accessed with dynamically uniform indices.
        /// </summary>
        public Bool ShaderUniformBufferArrayDynamicIndexing;
        /// <summary>
        /// Arrays of sampled images can be accessed with dynamically uniform indices.
        /// </summary>
        public Bool ShaderSampledImageArrayDynamicIndexing;
        /// <summary>
        /// Arrays of storage buffers can be accessed with dynamically uniform indices.
        /// </summary>
        public Bool ShaderStorageBufferArrayDynamicIndexing;
        /// <summary>
        /// Arrays of storage images can be accessed with dynamically uniform indices.
        /// </summary>
        public Bool ShaderStorageImageArrayDynamicIndexing;
        /// <summary>
        /// Clip distance in shaders.
        /// </summary>
        public Bool ShaderClipDistance;
        /// <summary>
        /// Cull distance in shaders.
        /// </summary>
        public Bool ShaderCullDistance;
        /// <summary>
        /// 64-bit floats (doubles) in shaders.
        /// </summary>
        public Bool ShaderFloat64;
        /// <summary>
        /// 64-bit integers in shaders.
        /// </summary>
        public Bool ShaderInt64;
        /// <summary>
        /// 16-bit integers in shaders.
        /// </summary>
        public Bool ShaderInt16;
        /// <summary>
        /// Shader can use texture operations that return resource residency information (requires
        /// sparseNonResident support).
        /// </summary>
        public Bool ShaderResourceResidency;
        /// <summary>
        /// Shader can use texture operations that specify minimum resource level of detail.
        /// </summary>
        public Bool ShaderResourceMinLod;
        /// <summary>
        /// Sparse resources support: resource memory can be managed at opaque page level rather than
        /// object level.
        /// </summary>
        public Bool SparseBinding;
        /// <summary>
        /// Sparse resources support: GPU can access partially resident buffers.
        /// </summary>
        public Bool SparseResidencyBuffer;
        /// <summary>
        /// Sparse resources support: GPU can access partially resident 2D (non-MSAA
        /// non-depth/stencil) images.
        /// </summary>
        public Bool SparseResidencyImage2D;
        /// <summary>
        /// Sparse resources support: GPU can access partially resident 3D images.
        /// </summary>
        public Bool SparseResidencyImage3D;
        /// <summary>
        /// Sparse resources support: GPU can access partially resident MSAA 2D images with 2 samples.
        /// </summary>
        public Bool SparseResidency2Samples;
        /// <summary>
        /// Sparse resources support: GPU can access partially resident MSAA 2D images with 4 samples.
        /// </summary>
        public Bool SparseResidency4Samples;
        /// <summary>
        /// Sparse resources support: GPU can access partially resident MSAA 2D images with 8 samples.
        /// </summary>
        public Bool SparseResidency8Samples;
        /// <summary>
        /// Sparse resources support: GPU can access partially resident MSAA 2D images with 16 samples.
        /// </summary>
        public Bool SparseResidency16Samples;
        /// <summary>
        /// Sparse resources support: GPU can correctly access data aliased into multiple locations (opt-in).
        /// </summary>
        public Bool SparseResidencyAliased;
        /// <summary>
        /// Multisample rate must be the same for all pipelines in a subpass.
        /// </summary>
        public Bool VariableMultisampleRate;
        /// <summary>
        /// Queries may be inherited from primary to secondary command buffers.
        /// </summary>
        public Bool InheritedQueries;
    }

    /// <summary>
    /// Structure reporting implementation-dependent physical device limits.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceLimits
    {
        /// <summary>
        /// The maximum dimension (width) of an image created with an <see cref="ImageType.Image1D"/>.
        /// </summary>
        public int MaxImageDimension1D;
        /// <summary>
        /// The maximum dimension (width or height) of an image created with an <see
        /// cref="ImageType.Image2D"/> and without <see cref="ImageCreateFlags.CubeCompatible"/> set
        /// in flags.
        /// </summary>
        public int MaxImageDimension2D;
        /// <summary>
        /// The maximum dimension (width, height, or depth) of an image created with an <see cref="ImageType.Image3D"/>.
        /// </summary>
        public int MaxImageDimension3D;
        /// <summary>
        /// The maximum dimension (width or height) of an image created with an <see
        /// cref="ImageType.Image2D"/> and with <see cref="ImageCreateFlags.CubeCompatible"/> set in flags.
        /// </summary>
        public int MaxImageDimensionCube;
        /// <summary>
        /// The maximum number of layers (array layers) for an image.
        /// </summary>
        public int MaxImageArrayLayers;
        /// <summary>
        /// The maximum number of addressable texels for a buffer view created on a buffer which was
        /// created with the <see cref="BufferUsages.UniformTexelBuffer"/> or <see
        /// cref="BufferUsages.StorageTexelBuffer"/> set in the <see cref="BufferCreateInfo.Usage"/> member.
        /// </summary>
        public int MaxTexelBufferElements;
        /// <summary>
        /// The maximum value that can be specified in the range member of any <see
        /// cref="DescriptorBufferInfo"/> structures passed to a call to <see
        /// cref="DescriptorPool.UpdateSets"/> for descriptors of type <see
        /// cref="DescriptorType.UniformBuffer"/> or <see cref="DescriptorType.UniformBufferDynamic"/>.
        /// </summary>
        public int MaxUniformBufferRange;
        /// <summary>
        /// The maximum value that can be specified in the range member of any <see cref="DescriptorBufferInfo"/>
        /// structures passed to a call to <see cref="DescriptorPool.UpdateSets"/> for descriptors of type
        /// <see cref="DescriptorType.StorageBuffer"/> or <see cref="DescriptorType.StorageBufferDynamic"/>.
        /// </summary>
        public int MaxStorageBufferRange;
        /// <summary>
        /// The maximum size, in bytes, of the pool of push constant memory. For each of the push
        /// constant ranges indicated by the <see
        /// cref="PipelineLayoutCreateInfo.PushConstantRanges"/> member, <see
        /// cref="PushConstantRange.Offset"/> + <see cref="PushConstantRange.Size"/> must be less
        /// than or equal to this limit.
        /// </summary>
        public int MaxPushConstantsSize;
        /// <summary>
        /// The maximum number of device memory allocations, as created by <see
        /// cref="Device.AllocateMemory"/>, which can simultaneously exist.
        /// </summary>
        public int MaxMemoryAllocationCount;
        /// <summary>
        /// The maximum number of sampler objects, as created by <see cref="Device.CreateSampler"/>,
        /// which can simultaneously exist on a device.
        /// </summary>
        public int MaxSamplerAllocationCount;
        /// <summary>
        /// The granularity, in bytes, at which buffer or linear image resources, and optimal image
        /// resources can be bound to adjacent offsets in the same <see cref="DeviceMemory"/> object
        /// without aliasing.
        /// </summary>
        public long BufferImageGranularity;
        /// <summary>
        /// The total amount of address space available, in bytes, for sparse memory resources. This
        /// is an upper bound on the sum of the size of all sparse resources, regardless of whether
        /// any memory is bound to them.
        /// </summary>
        public long SparseAddressSpaceSize;
        /// <summary>
        /// The maximum number of descriptor sets that can be simultaneously used by a pipeline. All
        /// <see cref="DescriptorSet"/> decorations in shader modules must have a value less than
        /// <see cref="MaxBoundDescriptorSets"/>.
        /// </summary>
        public int MaxBoundDescriptorSets;
        /// <summary>
        /// The maximum number of samplers that can be accessible to a single shader stage in a
        /// pipeline layout. Descriptors with a type of <see cref="DescriptorType.Sampler"/> or <see
        /// cref="DescriptorType.CombinedImageSampler"/> count against this limit. A descriptor is
        /// accessible to a shader stage when the <see cref="DescriptorSetLayoutBinding.StageFlags"/>
        /// member has the bit for that shader stage set.
        /// </summary>
        public int MaxPerStageDescriptorSamplers;
        /// <summary>
        /// The maximum number of uniform buffers that can be accessible to a single shader stage in
        /// a pipeline layout. Descriptors with a type of <see cref="DescriptorType.UniformBuffer"/>
        /// or <see cref="DescriptorType.UniformBufferDynamic"/> count against this limit. A
        /// descriptor is accessible to a shader stage when the <see
        /// cref="DescriptorSetLayoutBinding.StageFlags"/> member has the bit for that shader stage set.
        /// </summary>
        public int MaxPerStageDescriptorUniformBuffers;
        /// <summary>
        /// The maximum number of storage buffers that can be accessible to a single shader stage in
        /// a pipeline layout. Descriptors with a type of <see cref="DescriptorType.StorageBuffer"/>
        /// or <see cref="DescriptorType.StorageBufferDynamic"/> count against this limit. A
        /// descriptor is accessible to a pipeline shader stage when the <see
        /// cref="DescriptorSetLayoutBinding.StageFlags"/> member has the bit for that shader stage set.
        /// </summary>
        public int MaxPerStageDescriptorStorageBuffers;
        /// <summary>
        /// The maximum number of sampled images that can be accessible to a single shader stage in a
        /// pipeline layout. Descriptors with a type of <see
        /// cref="DescriptorType.CombinedImageSampler"/>, <see cref="DescriptorType.SampledImage"/>,
        /// or <see cref="DescriptorType.UniformTexelBuffer"/> count against this limit. A descriptor
        /// is accessible to a pipeline shader stage when the <see
        /// cref="DescriptorSetLayoutBinding.StageFlags"/> member has the bit for that shader stage set.
        /// </summary>
        public int MaxPerStageDescriptorSampledImages;
        /// <summary>
        /// The maximum number of storage images that can be accessible to a single shader stage in a
        /// pipeline layout. Descriptors with a type of <see cref="DescriptorType.StorageImage"/>, or
        /// <see cref="DescriptorType.StorageTexelBuffer"/> count against this limit. A descriptor is
        /// accessible to a pipeline shader stage when the <see
        /// cref="DescriptorSetLayoutBinding.StageFlags"/> member has the bit for that shader stage set.
        /// </summary>
        public int MaxPerStageDescriptorStorageImages;
        /// <summary>
        /// The maximum number of input attachments that can be accessible to a single shader stage
        /// in a pipeline layout. Descriptors with a type of <see
        /// cref="DescriptorType.InputAttachment"/> count against this limit. A descriptor is
        /// accessible to a pipeline shader stage when the <see
        /// cref="DescriptorSetLayoutBinding.StageFlags"/> member has the bit for that shader stage
        /// set. These are only supported for the fragment stage.
        /// </summary>
        public int MaxPerStageDescriptorInputAttachments;
        /// <summary>
        /// The maximum number of resources that can be accessible to a single shader stage in a
        /// pipeline layout. Descriptors with a type of <see cref="DescriptorType.CombinedImageSampler"/>,
        /// <see cref="DescriptorType.SampledImage"/>, <see cref="DescriptorType.StorageImage"/>,
        /// <see cref="DescriptorType.UniformTexelBuffer"/>, <see cref="DescriptorType.StorageTexelBuffer"/>,
        /// <see cref="DescriptorType.UniformBuffer"/>, <see cref="DescriptorType.StorageBuffer"/>,
        /// <see cref="DescriptorType.UniformBufferDynamic"/>, <see cref="DescriptorType.StorageBufferDynamic"/>, or
        /// <see cref="DescriptorType.InputAttachment"/> count against this limit. For the fragment shader
        /// stage the framebuffer color attachments also count against this limit.
        /// </summary>
        public int MaxPerStageResources;
        /// <summary>
        /// The maximum number of samplers that can be included in descriptor bindings in a pipeline
        /// layout across all pipeline shader stages and descriptor set numbers. Descriptors with a
        /// type of <see cref="DescriptorType.Sampler"/> or <see cref="DescriptorType.CombinedImageSampler"/> count
        /// against this limit.
        /// </summary>
        public int MaxDescriptorSetSamplers;
        /// <summary>
        /// The maximum number of uniform buffers that can be included in descriptor bindings in a
        /// pipeline layout across all pipeline shader stages and descriptor set numbers. Descriptors
        /// with a type of <see cref="DescriptorType.UniformBuffer"/> or
        /// <see cref="DescriptorType.UniformBufferDynamic"/> count against this limit.
        /// </summary>
        public int MaxDescriptorSetUniformBuffers;
        /// <summary>
        /// The maximum number of dynamic uniform buffers that can be included in descriptor bindings
        /// in a pipeline layout across all pipeline shader stages and descriptor set numbers.
        /// Descriptors with a type of <see cref="DescriptorType.UniformBufferDynamic"/> count against this limit.
        /// </summary>
        public int MaxDescriptorSetUniformBuffersDynamic;
        /// <summary>
        /// The maximum number of storage buffers that can be included in descriptor bindings in a
        /// pipeline layout across all pipeline shader stages and descriptor set numbers. Descriptors
        /// with a type of <see cref="DescriptorType.StorageBuffer"/> or
        /// <see cref="DescriptorType.StorageBufferDynamic"/> count against this limit.
        /// </summary>
        public int MaxDescriptorSetStorageBuffers;
        /// <summary>
        /// The maximum number of dynamic storage buffers that can be included in descriptor bindings
        /// in a pipeline layout across all pipeline shader stages and descriptor set numbers.
        /// Descriptors with a type of <see cref="DescriptorType.StorageBufferDynamic"/> count against this limit.
        /// </summary>
        public int MaxDescriptorSetStorageBuffersDynamic;
        /// <summary>
        /// The maximum number of sampled images that can be included in descriptor bindings in a
        /// pipeline layout across all pipeline shader stages and descriptor set numbers. Descriptors
        /// with a type of <see cref="DescriptorType.CombinedImageSampler"/>,
        /// <see cref="DescriptorType.SampledImage"/>, or <see cref="DescriptorType.UniformTexelBuffer"/> count
        /// against this limit.
        /// </summary>
        public int MaxDescriptorSetSampledImages;
        /// <summary>
        /// The maximum number of storage images that can be included in descriptor bindings in a
        /// pipeline layout across all pipeline shader stages and descriptor set numbers. Descriptors
        /// with a type of <see cref="DescriptorType.StorageImage"/>, or
        /// <see cref="DescriptorType.StorageTexelBuffer"/> count against this limit.
        /// </summary>
        public int MaxDescriptorSetStorageImages;
        /// <summary>
        /// The maximum number of input attachments that can be included in descriptor bindings in a
        /// pipeline layout across all pipeline shader stages and descriptor set numbers. Descriptors
        /// with a type of <see cref="DescriptorType.InputAttachment"/> count against this limit.
        /// </summary>
        public int MaxDescriptorSetInputAttachments;
        /// <summary>
        /// The maximum number of vertex input attributes that can be specified for a graphics
        /// pipeline. These are described in the array of <see
        /// cref="VertexInputAttributeDescription"/> structures that are provided at graphics
        /// pipeline creation time via the <see
        /// cref="PipelineVertexInputStateCreateInfo.VertexAttributeDescriptions"/> member.
        /// </summary>
        public int MaxVertexInputAttributes;
        /// <summary>
        /// The maximum number of vertex buffers that can be specified for providing vertex
        /// attributes to a graphics pipeline. These are described in the array of <see
        /// cref="VertexInputBindingDescription"/> structures that are provided at graphics pipeline
        /// creation time via the <see
        /// cref="PipelineVertexInputStateCreateInfo.VertexBindingDescriptions"/> member. The <see
        /// cref="VertexInputBindingDescription.Binding"/> member must be less than this limit.
        /// </summary>
        public int MaxVertexInputBindings;
        /// <summary>
        /// The maximum vertex input attribute offset that can be added to the vertex input binding
        /// stride. The <see cref="VertexInputAttributeDescription.Offset"/> member structure must be
        /// less than or equal to this limit.
        /// </summary>
        public int MaxVertexInputAttributeOffset;
        /// <summary>
        /// The maximum vertex input binding stride that can be specified in a vertex input binding.
        /// The <see cref="VertexInputBindingDescription.Stride"/> member must be less than or equal
        /// to this limit.
        /// </summary>
        public int MaxVertexInputBindingStride;
        /// <summary>
        /// The maximum number of components of output variables which can be output by a vertex shader.
        /// </summary>
        public int MaxVertexOutputComponents;
        /// <summary>
        /// The maximum tessellation generation level supported by the fixed-function tessellation
        /// primitive generator.
        /// </summary>
        public int MaxTessellationGenerationLevel;
        /// <summary>
        /// The maximum patch size, in vertices, of patches that can be processed by the tessellation
        /// control shader and tessellation primitive generator. The <see
        /// cref="PipelineTessellationStateCreateInfo.PatchControlPoints"/> member specified at
        /// pipeline creation time and the value provided in the OutputVertices execution mode of
        /// shader modules must be less than or equal to this limit.
        /// </summary>
        public int MaxTessellationPatchSize;
        /// <summary>
        /// The maximum number of components of input variables which can be provided as per-vertex
        /// inputs to the tessellation control shader stage.
        /// </summary>
        public int MaxTessellationControlPerVertexInputComponents;
        /// <summary>
        /// The maximum number of components of per-vertex output variables which can be output from
        /// the tessellation control shader stage.
        /// </summary>
        public int MaxTessellationControlPerVertexOutputComponents;
        /// <summary>
        /// The maximum number of components of per-patch output variables which can be output from
        /// the tessellation control shader stage.
        /// </summary>
        public int MaxTessellationControlPerPatchOutputComponents;
        /// <summary>
        /// The maximum total number of components of per-vertex and per-patch output variables which
        /// can be output from the tessellation control shader stage.
        /// </summary>
        public int MaxTessellationControlTotalOutputComponents;
        /// <summary>
        /// The maximum number of components of input variables which can be provided as per-vertex
        /// inputs to the tessellation evaluation shader stage.
        /// </summary>
        public int MaxTessellationEvaluationInputComponents;
        /// <summary>
        /// The maximum number of components of per-vertex output variables which can be output from
        /// the tessellation evaluation shader stage.
        /// </summary>
        public int MaxTessellationEvaluationOutputComponents;
        /// <summary>
        /// The maximum invocation count supported for instanced geometry shaders. The value provided
        /// in the`Invocations execution mode of shader modules must be less than or equal to this limit.
        /// </summary>
        public int MaxGeometryShaderInvocations;
        /// <summary>
        /// The maximum number of components of input variables which can be provided as inputs to
        /// the geometry shader stage.
        /// </summary>
        public int MaxGeometryInputComponents;
        /// <summary>
        /// The maximum number of components of output variables which can be output from the
        /// geometry shader stage.
        /// </summary>
        public int MaxGeometryOutputComponents;
        /// <summary>
        /// The maximum number of vertices which can be emitted by any geometry shader.
        /// </summary>
        public int MaxGeometryOutputVertices;
        /// <summary>
        /// The maximum total number of components of output, across all emitted vertices, which can
        /// be output from the geometry shader stage.
        /// </summary>
        public int MaxGeometryTotalOutputComponents;
        /// <summary>
        /// The maximum number of components of input variables which can be provided as inputs to
        /// the fragment shader stage.
        /// </summary>
        public int MaxFragmentInputComponents;
        /// <summary>
        /// The maximum number of output attachments which can be written to by the fragment shader stage.
        /// </summary>
        public int MaxFragmentOutputAttachments;
        /// <summary>
        /// The maximum number of output attachments which can be written to by the fragment shader
        /// stage when blending is enabled and one of the dual source blend modes is in use.
        /// </summary>
        public int MaxFragmentDualSrcAttachments;
        /// <summary>
        /// The total number of storage buffers, storage images, and output buffers which can be used
        /// in the fragment shader stage.
        /// </summary>
        public int MaxFragmentCombinedOutputResources;
        /// <summary>
        /// The maximum total storage size, in bytes, of all variables declared with the
        /// WorkgroupLocal storage class in shader modules (or with the shared storage qualifier in
        /// GLSL) in the compute shader stage.
        /// </summary>
        public int MaxComputeSharedMemorySize;
        /// <summary>
        /// The maximum number of local workgroups that can be dispatched by a single dispatch
        /// command. This value represents the maximum number of local workgroups for the X
        /// dimension. The x parameter to the <see cref="CommandBuffer.CmdDispatch"/> command must be
        /// less than or equal to the limit.
        /// </summary>
        public int MaxComputeXWorkGroupCount;
        /// <summary>
        /// The maximum number of local workgroups that can be dispatched by a single dispatch
        /// command. This value represents the maximum number of local workgroups for the Y
        /// dimension. The y parameter to the <see cref="CommandBuffer.CmdDispatch"/> command must be
        /// less than or equal to the limit.
        /// </summary>
        public int MaxComputeYWorkGroupCount;
        /// <summary>
        /// The maximum number of local workgroups that can be dispatched by a single dispatch
        /// command. This value represents the maximum number of local workgroups for the Z
        /// dimension. The z parameter to the <see cref="CommandBuffer.CmdDispatch"/> command must be
        /// less than or equal to the limit.
        /// </summary>
        public int MaxComputeZWorkGroupCount;
        /// <summary>
        /// The maximum total number of compute shader invocations in a single local workgroup. The
        /// product of the X, Y, and Z sizes as specified by the LocalSize execution mode in shader
        /// modules and by the object decorated by the WorkgroupSize decoration must be less than or
        /// equal to this limit.
        /// </summary>
        public int MaxComputeWorkGroupInvocations;
        /// <summary>
        /// The maximum size of a local compute workgroup, per dimension. This value represents the
        /// maximum local workgroup size in the X dimension. The x size specified by the LocalSize
        /// execution mode and by the object decorated by the WorkgroupSize decoration in shader
        /// modules must be less than or equal to the limit.
        /// </summary>
        public int MaxComputeXWorkGroupSize;
        /// <summary>
        /// The maximum size of a local compute workgroup, per dimension. This value represents the
        /// maximum local workgroup size in the Y dimension. The y size specified by the LocalSize
        /// execution mode and by the object decorated by the WorkgroupSize decoration in shader
        /// modules must be less than or equal to the limit.
        /// </summary>
        public int MaxComputeYWorkGroupSize;
        /// <summary>
        /// The maximum size of a local compute workgroup, per dimension. This value represents the
        /// maximum local workgroup size in the Z dimension. The z size specified by the LocalSize
        /// execution mode and by the object decorated by the WorkgroupSize decoration in shader
        /// modules must be less than or equal to the limit.
        /// </summary>
        public int MaxComputeZWorkGroupSize;
        /// <summary>
        /// The number of bits of subpixel precision in framebuffer coordinates.
        /// </summary>
        public int SubPixelPrecisionBits;
        /// <summary>
        /// The number of bits of precision in the division along an axis of an image used for
        /// minification and magnification filters. 2**<see cref="SubTexelPrecisionBits"/> is the
        /// actual number of divisions along each axis of the image represented. The filtering
        /// hardware will snap to these locations when computing the filtered results.
        /// </summary>
        public int SubTexelPrecisionBits;
        /// <summary>
        /// The number of bits of division that the LOD calculation for mipmap fetching get snapped
        /// to when determining the contribution from each mip level to the mip filtered results.
        /// 2**<see cref="MipmapPrecisionBits"/> is the actual number of divisions.
        /// <para>
        /// For example, if this value is 2 bits then when linearly filtering between two levels,
        /// each level could contribute: 0%, 33%, 66%, or 100% (this is just an example and the
        /// amount of contribution should be covered by different equations in the spec).
        /// </para>
        /// </summary>
        public int MipmapPrecisionBits;
        /// <summary>
        /// The maximum index value that can be used for indexed draw calls when using 32-bit
        /// indices. This excludes the primitive restart index value of 0xFFFFFFFF.
        /// </summary>
        public int MaxDrawIndexedIndexValue;
        /// <summary>
        /// The maximum draw count that is supported for indirect draw calls.
        /// </summary>
        public int MaxDrawIndirectCount;
        /// <summary>
        /// The maximum absolute sampler level of detail bias. The sum of the <see
        /// cref="SamplerCreateInfo.MipLodBias"/> member and the Bias operand of image sampling
        /// operations in shader modules (or 0 if no Bias operand is provided to an image sampling
        /// operation) are clamped to the range [-<see cref="MaxSamplerLodBias"/>,+<see cref="MaxSamplerLodBias"/>].
        /// </summary>
        public float MaxSamplerLodBias;
        /// <summary>
        /// The maximum degree of sampler anisotropy. The maximum degree of anisotropic filtering
        /// used for an image sampling operation is the minimum of the <see
        /// cref="SamplerCreateInfo.MaxAnisotropy"/> member and this limit.
        /// </summary>
        public float MaxSamplerAnisotropy;
        /// <summary>
        /// The maximum number of active viewports. The <see
        /// cref="PipelineViewportStateCreateInfo.Viewports"/> member length that is provided at
        /// pipeline creation must be less than or equal to this limit.
        /// </summary>
        public int MaxViewports;
        /// <summary>
        /// The maximum viewport width in the X dimension. The maximum viewport dimension must be
        /// greater than or equal to the largest image which can be created and used as a framebuffer attachment.
        /// </summary>
        public int MaxViewportXDimension;
        /// <summary>
        /// The maximum viewport height in Y dimension. The maximum viewport dimension must be
        /// greater than or equal to the largest image which can be created and used as a framebuffer attachment.
        /// </summary>
        public int MaxViewportYDimension;
        /// <summary>
        /// The minimum of the range that the corners of a viewport must be contained in. This range
        /// must be at least [-2 × size, 2 × size - 1], where size = max( <see
        /// cref="MaxViewportXDimension"/>, <see cref="MaxViewportYDimension"/>).
        /// <para>
        /// The intent of the limit is to allow a maximum sized viewport to be arbitrarily shifted
        /// relative to the output target as long as at least some portion intersects. This would
        /// give a bounds limit of [-size + 1, 2 × size - 1] which would allow all possible
        /// non-empty-set intersections of the output target and the viewport. Since these numbers
        /// are typically powers of two, picking the signed number range using the smallest possible
        /// number of bits ends up with the specified range.
        /// </para>
        /// </summary>
        public float MinViewportBounds;
        /// <summary>
        /// The maximum of the range that the corners of a viewport must be contained in. This range
        /// must be at least [-2 × size, 2 × size - 1], where size = max( <see
        /// cref="MaxViewportXDimension"/>, <see cref="MaxViewportYDimension"/>).
        /// <para>
        /// The intent of the limit is to allow a maximum sized viewport to be arbitrarily shifted
        /// relative to the output target as long as at least some portion intersects. This would
        /// give a bounds limit of [-size + 1, 2 × size - 1] which would allow all possible
        /// non-empty-set intersections of the output target and the viewport. Since these numbers
        /// are typically powers of two, picking the signed number range using the smallest possible
        /// number of bits ends up with the specified range.
        /// </para>
        /// </summary>
        public float MaxViewportBounds;
        /// <summary>
        /// The number of bits of subpixel precision for viewport bounds. The subpixel precision that
        /// floating-point viewport bounds are interpreted at is given by this limit.
        /// </summary>
        public int ViewportSubPixelBits;
        /// <summary>
        /// The minimum required alignment, in bytes, of host visible memory allocations within the
        /// host address space. When mapping a memory allocation with <see cref="DeviceMemory.Map"/>,
        /// subtracting offset bytes from the returned pointer will always produce an integer
        /// multiple of this limit.
        /// </summary>
        public Size MinMemoryMapAlignment;
        /// <summary>
        /// The minimum required alignment, in bytes, for the <see
        /// cref="BufferViewCreateInfo.Offset"/> member for texel buffers. When a buffer view is
        /// created for a buffer which was created with <see cref="BufferUsages.UniformTexelBuffer"/>
        /// or <see cref="BufferUsages.StorageTexelBuffer"/> set in the <see
        /// cref="BufferCreateInfo.Usage"/> member, the offset must be an integer multiple of this limit.
        /// </summary>
        public long MinTexelBufferOffsetAlignment;
        /// <summary>
        /// The minimum required alignment, in bytes, for the offset member of the
        /// <see cref="DescriptorBufferInfo"/> structure for uniform buffers. When a descriptor of type
        /// <see cref="DescriptorType.UniformBuffer"/> or <see cref="DescriptorType.UniformBufferDynamic"/> is
        /// updated, the offset must be an integer multiple of this limit. Similarly, dynamic offsets
        /// for uniform buffers must be multiples of this limit.
        /// </summary>
        public long MinUniformBufferOffsetAlignment;
        /// <summary>
        /// The minimum required alignment, in bytes, for the offset member of the
        /// <see cref="DescriptorBufferInfo"/> structure for storage buffers. When a descriptor of type
        /// <see cref="DescriptorType.StorageBuffer"/> or <see cref="DescriptorType.StorageBufferDynamic"/> is
        /// updated, the offset must be an integer multiple of this limit. Similarly, dynamic offsets
        /// for storage buffers must be multiples of this limit.
        /// </summary>
        public long MinStorageBufferOffsetAlignment;
        /// <summary>
        /// The minimum offset value for the ConstOffset image operand of any of the OpImageSample*
        /// or OpImageFetch* image instructions.
        /// </summary>
        public int MinTexelOffset;
        /// <summary>
        /// The maximum offset value for the ConstOffset image operand of any of the OpImageSample*
        /// or OpImageFetch* image instructions.
        /// </summary>
        public int MaxTexelOffset;
        /// <summary>
        /// The minimum offset value for the Offset or ConstOffsets image operands of any of the
        /// OpImage*Gather image instructions.
        /// </summary>
        public int MinTexelGatherOffset;
        /// <summary>
        /// The maximum offset value for the Offset or ConstOffsets image operands of any of the
        /// OpImage*Gather image instructions.
        /// </summary>
        public int MaxTexelGatherOffset;
        /// <summary>
        /// The minimum negative offset value for the offset operand of the InterpolateAtOffset
        /// extended instruction.
        /// </summary>
        public float MinInterpolationOffset;
        /// <summary>
        /// The maximum positive offset value for the offset operand of the InterpolateAtOffset
        /// extended instruction.
        /// </summary>
        public float MaxInterpolationOffset;
        /// <summary>
        /// The number of subpixel fractional bits that the x and y offsets to the
        /// InterpolateAtOffset extended instruction may be rounded to as fixed-point values.
        /// </summary>
        public int SubPixelInterpolationOffsetBits;
        /// <summary>
        /// The maximum width for a framebuffer. The <see cref="FramebufferCreateInfo.Width"/> member
        /// must be less than or equal to this limit.
        /// </summary>
        public int MaxFramebufferWidth;
        /// <summary>
        /// The maximum height for a framebuffer. The <see cref="FramebufferCreateInfo.Height"/>
        /// member must be less than or equal to this limit.
        /// </summary>
        public int MaxFramebufferHeight;
        /// <summary>
        /// The maximum layer count for a layered framebuffer. The <see
        /// cref="FramebufferCreateInfo.Layers"/> member must be less than or equal to this limit.
        /// </summary>
        public int MaxFramebufferLayers;
        /// <summary>
        /// A bitmask indicating the color sample counts that are supported for all framebuffer color attachments.
        /// </summary>
        public SampleCounts FramebufferColorSampleCounts;
        /// <summary>
        /// A bitmask indicating the supported depth sample counts for all framebuffer depth/stencil
        /// attachments, when the format includes a depth component.
        /// </summary>
        public SampleCounts FramebufferDepthSampleCounts;
        /// <summary>
        /// A bitmask of <see cref="SampleCounts"/> bits indicating the supported stencil sample
        /// counts for all framebuffer depth/stencil attachments, when the format includes a stencil component.
        /// </summary>
        public SampleCounts FramebufferStencilSampleCounts;
        /// <summary>
        /// A bitmask of <see cref="SampleCounts"/> bits indicating the supported sample counts for a
        /// framebuffer with no attachments.
        /// </summary>
        public SampleCounts FramebufferNoAttachmentsSampleCounts;
        /// <summary>
        /// The maximum number of color attachments that can be used by a subpass in a render pass.
        /// The <see cref="SubpassDescription.ColorAttachments"/> length must be less than or equal
        /// to this limit.
        /// </summary>
        public int MaxColorAttachments;
        /// <summary>
        /// A bitmask indicating the sample counts supported for all 2D images created with <see
        /// cref="ImageTiling.Optimal"/>, usage containing <see cref="ImageUsages.Sampled"/>, and a
        /// non-integer color format.
        /// </summary>
        public SampleCounts SampledImageColorSampleCounts;
        /// <summary>
        /// A bitmask indicating the sample counts supported for all 2D images created with <see
        /// cref="ImageTiling.Optimal"/>, usage containing <see cref="ImageUsages.Sampled"/>, and an
        /// integer color format.
        /// </summary>
        public SampleCounts SampledImageIntegerSampleCounts;
        /// <summary>
        /// A bitmask indicating the sample counts supported for all 2D images created with <see
        /// cref="ImageTiling.Optimal"/>, usage containing <see cref="ImageUsages.Sampled"/>, and a
        /// depth format.
        /// </summary>
        public SampleCounts SampledImageDepthSampleCounts;
        /// <summary>
        /// A bitmask indicating the sample supported for all 2D images created with <see
        /// cref="ImageTiling.Optimal"/>, usage containing <see cref="ImageUsages.Sampled"/>, and a
        /// stencil format.
        /// </summary>
        public SampleCounts SampledImageStencilSampleCounts;
        /// <summary>
        /// A bitmask indicating the sample counts supported for all 2D images created with <see
        /// cref="ImageTiling.Optimal"/>, and usage containing <see cref="ImageUsages.Storage"/>.
        /// </summary>
        public SampleCounts StorageImageSampleCounts;
        /// <summary>
        /// The maximum number of array elements of a variable decorated with the SampleMask built-in decoration.
        /// </summary>
        public int MaxSampleMaskWords;
        /// <summary>
        /// Indicates support for timestamps on all graphics and compute queues. If this limit is set
        /// to <c>true</c>, all queues that advertise the <see cref="Queues.Graphics"/> or <see
        /// cref="Queues.Compute"/> in the <see cref="QueueFamilyProperties.QueueFlags"/> support
        /// <see cref="QueueFamilyProperties.TimestampValidBits"/> of at least 36.
        /// </summary>
        public Bool TimestampComputeAndGraphics;
        /// <summary>
        /// The number of nanoseconds required for a timestamp query to be incremented by 1.
        /// </summary>
        public float TimestampPeriod;
        /// <summary>
        /// The maximum number of clip distances that can be used in a single shader stage. The size
        /// of any array declared with the ClipDistance built-in decoration in a shader module must
        /// be less than or equal to this limit.
        /// </summary>
        public int MaxClipDistances;
        /// <summary>
        /// The maximum number of cull distances that can be used in a single shader stage. The size
        /// of any array declared with the CullDistance built-in decoration in a shader module must
        /// be less than or equal to this limit.
        /// </summary>
        public int MaxCullDistances;
        /// <summary>
        /// The maximum combined number of clip and cull distances that can be used in a single
        /// shader stage. The sum of the sizes of any pair of arrays declared with the ClipDistance
        /// and CullDistance built-in decoration used by a single shader stage in a shader module
        /// must be less than or equal to this limit.
        /// </summary>
        public int MaxCombinedClipAndCullDistances;
        /// <summary>
        /// The number of discrete priorities that can be assigned to a queue based on the value of
        /// each member of VkDeviceQueueCreateInfo::pQueuePriorities. This must be at least 2, and
        /// levels must be spread evenly over the range, with at least one level at 1.0, and another
        /// at 0.0.
        /// </summary>
        public int DiscreteQueuePriorities;
        /// <summary>
        /// The minimum of the range of supported sizes for points. Values written to variables
        /// decorated with the PointSize built-in decoration are clamped to this range.
        /// </summary>
        public float MinPointSize;
        /// <summary>
        /// The maximum of the range of supported sizes for points. Values written to variables
        /// decorated with the PointSize built-in decoration are clamped to this range.
        /// </summary>
        public float MaxPointSize;
        /// <summary>
        /// The minimum of the range of supported widths for lines. Values specified by the <see
        /// cref="PipelineRasterizationStateCreateInfo.LineWidth"/> member or the line width
        /// parameter to <see cref="CommandBuffer.CmdSetLineWidth"/> are clamped to this range.
        /// </summary>
        public float MinLineWidth;
        /// <summary>
        /// The maximum of the range of supported widths for lines. Values specified by the <see
        /// cref="PipelineRasterizationStateCreateInfo.LineWidth"/> member or the line width
        /// parameter to <see cref="CommandBuffer.CmdSetLineWidth"/> are clamped to this range.
        /// </summary>
        public float MaxLineWidth;
        /// <summary>
        /// The granularity of supported point sizes. Not all point sizes in the range defined by
        /// <see cref="MinPointSize"/> and <see cref="MaxPointSize"/> are supported. This limit
        /// specifies the granularity (or increment) between successive supported point sizes.
        /// </summary>
        public float PointSizeGranularity;
        /// <summary>
        /// The granularity of supported line widths. Not all line widths in the range defined by
        /// <see cref="MinLineWidth"/> and <see cref="MaxLineWidth"/> are supported. This limit
        /// specifies the granularity (or increment) between successive supported line widths.
        /// </summary>
        public float LineWidthGranularity;
        /// <summary>
        /// Indicates whether lines are rasterized according to the preferred method of
        /// rasterization. If set to <c>false</c>, lines may be rasterized under a relaxed set of rules.
        /// If set to <c>true</c>, lines are rasterized as per the strict definition.
        /// </summary>
        public Bool StrictLines;
        /// <summary>
        /// Indicates whether rasterization uses the standard sample locations. If set to
        /// <c>true</c>, the implementation uses the documented sample locations. If set to
        /// <c>false</c>, the implementation may use different sample locations.
        /// </summary>
        public Bool StandardSampleLocations;
        /// <summary>
        /// The optimal buffer offset alignment in bytes for <see
        /// cref="CommandBuffer.CmdCopyBufferToImage"/> and <see
        /// cref="CommandBuffer.CmdCopyImageToBuffer"/>. The per texel alignment requirements are
        /// still enforced, this is just an additional alignment recommendation for optimal
        /// performance and power.
        /// </summary>
        public long OptimalBufferCopyOffsetAlignment;
        /// <summary>
        /// The optimal buffer row pitch alignment in bytes for <see
        /// cref="CommandBuffer.CmdCopyBufferToImage"/> and <see
        /// cref="CommandBuffer.CmdCopyImageToBuffer"/>. Row pitch is the number of bytes between
        /// texels with the same X coordinate in adjacent rows (Y coordinates differ by one). The per
        /// texel alignment requirements are still enforced, this is just an additional alignment
        /// recommendation for optimal performance and power.
        /// </summary>
        public long OptimalBufferCopyRowPitchAlignment;
        /// <summary>
        /// The size and alignment in bytes that bounds concurrent access to host-mapped device memory.
        /// </summary>
        public long NonCoherentAtomSize;
    }

    /// <summary>
    /// Structure specifying physical device memory properties.
    /// </summary>
    public struct PhysicalDeviceMemoryProperties
    {
        /// <summary>
        /// Structures describing the memory types that can be used to access memory allocated from
        /// the heaps specified by <see cref="MemoryHeaps"/>.
        /// </summary>
        public MemoryType[] MemoryTypes;
        /// <summary>
        /// Structures describing the memory heaps from which memory can be allocated.
        /// </summary>
        public MemoryHeap[] MemoryHeaps;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public int MemoryTypeCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxMemoryTypes)]
            public MemoryType[] MemoryTypes;
            public int MemoryHeapCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxMemoryHeaps)]
            public MemoryHeap[] MemoryHeaps;
        }

        internal static void FromNative(ref Native native, out PhysicalDeviceMemoryProperties managed)
        {
            int memoryTypeCount = native.MemoryTypeCount;
            managed.MemoryTypes = new MemoryType[memoryTypeCount];
            for (int i = 0; i < memoryTypeCount; i++)
                managed.MemoryTypes[i] = native.MemoryTypes[i];

            int memoryHeapCount = native.MemoryHeapCount;
            managed.MemoryHeaps = new MemoryHeap[memoryHeapCount];
            for (int i = 0; i < memoryHeapCount; i++)
                managed.MemoryHeaps[i] = native.MemoryHeaps[i];
        }
    }

    /// <summary>
    /// Structure specifying memory type.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryType
    {
        /// <summary>
        /// A bitmask of properties for this memory type.
        /// </summary>
        public MemoryProperties PropertyFlags;
        /// <summary>
        /// Describes which memory heap this memory type corresponds to, and must be less than the
        /// length of <see cref="PhysicalDeviceMemoryProperties.MemoryHeaps"/>.
        /// </summary>
        public int HeapIndex;
    }

    /// <summary>
    /// Structure specifying a memory heap.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryHeap
    {
        /// <summary>
        /// The total memory size in bytes in the heap.
        /// </summary>
        public long Size;
        /// <summary>
        /// A bitmask of attribute flags for the heap.
        /// </summary>
        public MemoryHeaps Flags;
    }

    /// <summary>
    /// Bitmask specifying properties for a memory type.
    /// </summary>
    [Flags]
    public enum MemoryProperties
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates that memory allocated with this type is the most efficient for device access.
        /// This property will be set if and only if the memory type belongs to a heap with the <see
        /// cref="MemoryHeaps.DeviceLocal"/> set.
        /// </summary>
        DeviceLocal = 1 << 0,
        /// <summary>
        /// Indicates that memory allocated with this type can be mapped for host access using <see cref="DeviceMemory.Map"/>.
        /// </summary>
        HostVisible = 1 << 1,
        /// <summary>
        /// Indicates that the host cache management commands <see
        /// cref="Device.FlushMappedMemoryRanges"/> and <see
        /// cref="Device.InvalidateMappedMemoryRanges"/> are not needed to flush host writes to the
        /// device or make device writes visible to the host, respectively.
        /// </summary>
        HostCoherent = 1 << 2,
        /// <summary>
        /// Indicates that memory allocated with this type is cached on the host. Host memory
        /// accesses to uncached memory are slower than to cached memory, however uncached memory is
        /// always host coherent.
        /// </summary>
        HostCached = 1 << 3,
        /// <summary>
        /// Indicates that the memory type only allows device access to the memory. Memory types must
        /// not have both <see cref="LazilyAllocated"/> and <see cref="HostVisible"/> set.
        /// <para>
        /// Additionally, the object's backing memory may be provided by the implementation lazily.
        /// </para>
        /// </summary>
        LazilyAllocated = 1 << 4
    }

    /// <summary>
    /// Bitmask specifying attribute flags for a heap.
    /// </summary>
    [Flags]
    public enum MemoryHeaps
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates that the heap corresponds to device local memory.
        /// <para>
        /// Device local memory may have different performance characteristics than host local
        /// memory, and may support different memory property flags.
        /// </para>
        /// </summary>
        DeviceLocal = 1 << 0,
        /// <summary>
        /// Indicates that in a logical device representing more than one physical device, there is a
        /// per-physical device instance of the heap memory.
        /// <para>
        /// By default, an allocation from such a heap will be replicated to each physical device's
        /// instance of the heap.
        /// </para>
        /// </summary>
        MultiInstanceKhx = 1 << 1
    }

    /// <summary>
    /// Structure specifying physical device properties.
    /// </summary>
    public unsafe struct PhysicalDeviceProperties
    {
        /// <summary>
        /// The version of Vulkan supported by the device.
        /// </summary>
        public Version ApiVersion;
        /// <summary>
        /// The vendor-specified version of the driver.
        /// </summary>
        public Version DriverVersion;
        /// <summary>
        /// A unique identifier for the vendor of the physical device.
        /// </summary>
        public int VendorId;
        /// <summary>
        /// A unique identifier for the physical device among devices available from the vendor.
        /// </summary>
        public int DeviceId;
        /// <summary>
        /// Specifies the type of device.
        /// </summary>
        public PhysicalDeviceType DeviceType;
        /// <summary>
        /// The name of the device.
        /// </summary>
        public string DeviceName;
        /// <summary>
        /// A universally unique identifier for the device.
        /// </summary>
        public Guid PipelineCacheUuid;
        /// <summary>
        /// Specifies device-specific limits of the physical device.
        /// </summary>
        public PhysicalDeviceLimits Limits;
        /// <summary>
        /// Specifies various sparse related properties of the physical device.
        /// </summary>
        public PhysicalDeviceSparseProperties SparseProperties;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public Version ApiVersion;
            public Version DriverVersion;
            public int VendorId;
            public int DeviceId;
            public PhysicalDeviceType DeviceType;
            public fixed byte DeviceName[MaxPhysicalDeviceNameSize];
            public fixed byte PipelineCacheUuid[UuidSize];
            public PhysicalDeviceLimits Limits;
            public PhysicalDeviceSparseProperties SparseProperties;
        }

        internal static void FromNative(ref Native native, out PhysicalDeviceProperties managed)
        {
            fixed (byte* deviceNamePtr = native.DeviceName)
            fixed (byte* pipelineCacheUuidPtr = native.PipelineCacheUuid)
            {
                string deviceName = Interop.String.FromPointer(deviceNamePtr);
                var pipelineCacheUuid = new byte[UuidSize];
                for (int i = 0; i < pipelineCacheUuid.Length; i++)
                    pipelineCacheUuid[i] = pipelineCacheUuidPtr[i];

                managed = new PhysicalDeviceProperties
                {
                    ApiVersion = native.ApiVersion,
                    DriverVersion = native.DriverVersion,
                    VendorId = native.VendorId,
                    DeviceId = native.DeviceId,
                    DeviceType = native.DeviceType,
                    DeviceName = deviceName,
                    PipelineCacheUuid = new Guid(pipelineCacheUuid),
                    Limits = native.Limits,
                    SparseProperties = native.SparseProperties
                };
            }
        }
    }

    /// <summary>
    /// Structure specifying physical device sparse memory properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceSparseProperties
    {
        /// <summary>
        /// Is <c>true</c> if the physical device will access all single-sample 2D sparse resources
        /// using the standard sparse image block shapes (based on image format). If this property is
        /// not supported the value returned in the <see
        /// cref="SparseImageFormatProperties.ImageGranularity"/> member for single-sample 2D images
        /// is not required to match the standard sparse image block dimensions listed in the table.
        /// </summary>
        public Bool ResidencyStandard2DBlockShape;
        /// <summary>
        /// Is <c>true</c> if the physical device will access all multisample 2D sparse resources
        /// using the standard sparse image block shapes (based on image format). If this property is
        /// not supported, the value returned in the <see
        /// cref="SparseImageFormatProperties.ImageGranularity"/> member for multisample 2D images is
        /// not required to match the standard sparse image block dimensions listed in the table.
        /// </summary>
        public Bool ResidencyStandard2DMultisampleBlockShape;
        /// <summary>
        /// Is <c>true</c> if the physical device will access all 3D sparse resources using the
        /// standard sparse image block shapes (based on image format). If this property is not
        /// supported, the value returned in the <see
        /// cref="SparseImageFormatProperties.ImageGranularity"/> member for 3D images is not
        /// required to match the standard sparse image block dimensions listed in the table.
        /// </summary>
        public Bool ResidencyStandard3DBlockShape;
        /// <summary>
        /// Is <c>true</c> if images with mip level dimensions that are not integer multiples of the
        /// corresponding dimensions of the sparse image block may: be placed in the mip tail. If
        /// this property is not reported, only mip levels with dimensions smaller than the
        /// imageGranularity member of the <see cref="SparseImageFormatProperties"/> structure will
        /// be placed in the mip tail. If this property is reported the implementation is allowed to
        /// return <see cref="SparseImageFormats.AlignedMipSize"/> in the flags member of <see
        /// cref="SparseImageFormatProperties"/>, indicating that mip level dimensions that are not
        /// integer multiples of the corresponding dimensions of the sparse image block will be
        /// placed in the mip tail.
        /// </summary>
        public Bool ResidencyAlignedMipSize;
        /// <summary>
        /// Specifies whether the physical device can consistently access non-resident regions of a
        /// resource. If this property is <c>true</c>, access to non-resident regions of resources
        /// will be guaranteed to return values as if the resource were populated with 0; writes to
        /// non-resident regions will be discarded.
        /// </summary>
        public Bool ResidencyNonResidentStrict;
    }

    /// <summary>
    /// Structure providing information about a queue family.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct QueueFamilyProperties
    {
        /// <summary>
        /// A bitmask indicating capabilities of the queues in this queue family.
        /// </summary>
        public Queues QueueFlags;
        /// <summary>
        /// The unsigned integer count of queues in this queue family.
        /// </summary>
        public int QueueCount;
        /// <summary>
        /// The unsigned integer count of meaningful bits in the timestamps written via <see
        /// cref="CommandBuffer.CmdWriteTimestamp"/>. The valid range for the count is 36..64 bits,
        /// or a value of 0, indicating no support for timestamps. Bits outside the valid range are
        /// guaranteed to be zeros.
        /// </summary>
        public int TimestampValidBits;
        /// <summary>
        /// The minimum granularity supported for image transfer operations on the queues in this
        /// queue family.
        /// </summary>
        public Extent3D MinImageTransferGranularity;
    }

    /// <summary>
    /// Bitmask specifying capabilities of queues in a queue family.
    /// </summary>
    [Flags]
    public enum Queues
    {
        /// <summary>
        /// Indicates that queues in this queue family support graphics operations.
        /// </summary>
        Graphics = 1 << 0,
        /// <summary>
        /// Indicates that queues in this queue family support compute operations.
        /// </summary>
        Compute = 1 << 1,
        /// <summary>
        /// Indicates that queues in this queue family support transfer operations.
        /// </summary>
        Transfer = 1 << 2,
        /// <summary>
        /// Indicates that queues in this queue family support sparse resource memory management operations.
        /// </summary>
        SparseBinding = 1 << 3
    }

    /// <summary>
    /// Structure specifying image format properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FormatProperties
    {
        /// <summary>
        /// A bitmask specifying features supported by images created with a tiling parameter of <see cref="ImageTiling.Linear"/>.
        /// </summary>
        public FormatFeatures LinearTilingFeatures;
        /// <summary>
        /// A bitmask specifying features supported by images created with a tiling parameter of <see cref="ImageTiling.Optimal"/>.
        /// </summary>
        public FormatFeatures OptimalTilingFeatures;
        /// <summary>
        /// A bitmask specifying features supported by buffers.
        /// </summary>
        public FormatFeatures BufferFeatures;
    }

    /// <summary>
    /// Bitmask specifying features supported by a buffer.
    /// </summary>
    [Flags]
    public enum FormatFeatures
    {
        /// <summary>
        /// Specifies that an image view can be sampled from.
        /// </summary>
        SampledImage = 1 << 0,
        /// <summary>
        /// Specifies that an image view can be used as a storage image.
        /// </summary>
        StorageImage = 1 << 1,
        /// <summary>
        /// Specifies that an image view can be used as storage image that supports atomic operations.
        /// </summary>
        StorageImageAtomic = 1 << 2,
        /// <summary>
        /// Specifies that the format can be used to create a buffer view that can be bound to a <see
        /// cref="DescriptorType.UniformTexelBuffer"/> descriptor.
        /// </summary>
        UniformTexelBuffer = 1 << 3,
        /// <summary>
        /// Specifies that the format can be used to create a buffer view that can be bound to a <see
        /// cref="DescriptorType.StorageTexelBuffer"/> descriptor.
        /// </summary>
        StorageTexelBuffer = 1 << 4,
        /// <summary>
        /// Specifies that atomic operations are supported on <see
        /// cref="DescriptorType.StorageTexelBuffer"/> with this format.
        /// </summary>
        StorageTexelBufferAtomic = 1 << 5,
        /// <summary>
        /// Specifies that the format can be used as a vertex attribute format ( <see cref="VertexInputAttributeDescription.Format"/>).
        /// </summary>
        VertexBuffer = 1 << 6,
        /// <summary>
        /// Specifies that an image view can be used as a framebuffer color attachment and as an
        /// input attachment.
        /// </summary>
        ColorAttachment = 1 << 7,
        /// <summary>
        /// Specifies that an image view can be used as a framebuffer color attachment that supports
        /// blending and as an input attachment.
        /// </summary>
        ColorAttachmentBlend = 1 << 8,
        /// <summary>
        /// Specifies that an image view can be used as a framebuffer depth/stencil attachment and as
        /// an input attachment.
        /// </summary>
        DepthStencilAttachment = 1 << 9,
        /// <summary>
        /// Specifies that an image can be used as source image for the <see
        /// cref="CommandBuffer.CmdBlitImage"/> command.
        /// </summary>
        BlitSrc = 1 << 10,
        /// <summary>
        /// Specifies that an image can be used as destination image for the <see
        /// cref="CommandBuffer.CmdBlitImage"/> command.
        /// </summary>
        BlitDst = 1 << 11,
        /// <summary>
        /// Specifies that if <see cref="SampledImage"/> is also set, an image view can be used with
        /// a sampler that has either of magnification or minification filter set to <see
        /// cref="Filter.Linear"/>, or mipmap mode set to <see cref="SamplerMipmapMode.Linear"/>.
        /// <para>
        /// If <see cref="BlitSrc"/> is also set, an image can be used as the source image to <see
        /// cref="CommandBuffer.CmdBlitImage"/> with a <see cref="Filter.Linear"/>.
        /// </para>
        /// <para>
        /// This bit must only be exposed for formats that also support the <see
        /// cref="SampledImage"/> or <see cref="BlitSrc"/>.
        /// </para>
        /// </summary>
        SampledImageFilterLinear = 1 << 12,
        /// <summary>
        /// Specifies that <see cref="Image"/> can be used with a sampler that has either of
        /// <c>MagFilter</c> or <c>MinFilter</c> set to <see cref="Filter.CubicImg"/>, or be the
        /// source image for a blit with <c>Filter</c> set to <see cref="Filter.CubicImg"/>.
        /// <para>This bit must only be exposed for formats that also support the <see cref="SampledImage"/>.</para>
        /// <para>
        /// If the format being queried is a depth/stencil format, this only indicates that the depth
        /// aspect is cubic filterable.
        /// </para>
        /// </summary>
        SampledImageFilterCubicImg = 1 << 13,
        /// <summary>
        /// Specifies that an image can be used as a source image for copy commands.
        /// </summary>
        TransferSrcKhr = 1 << 14,
        /// <summary>
        /// Specifies that an image can be used as a destination image for copy commands and clear commands.
        /// </summary>
        TransferDstKhr = 1 << 15,
        /// <summary>
        /// Specifies <see cref="Image"/> can be used as a sampled image with a min or max <see cref="Ext.SamplerReductionModeExt"/>.
        /// <para>This bit must only be exposed for formats that also support the <see cref="SampledImage"/>.</para>
        /// </summary>
        SampledImageFilterMinmaxExt = 1 << 16,
        /// <summary>
        /// Specifies that an application can define a sampler Y'C~B~C~R~ conversion using this
        /// format as a source, and that an image of this format can be used with a <see
        /// cref="Khr.SamplerYcbcrConversionCreateInfoKhr"/><c>XChromaOffset</c> and/or
        /// <c>YChromaOffset</c> of <see cref="Khr.ChromaLocationKhr.Midpoint"/>. Otherwise both
        /// <c>XChromaOffset</c> and <c>YChromaOffset</c> must be <see
        /// cref="Khr.ChromaLocationKhr.CositedEven"/>. If a format does not incorporate chroma
        /// downsampling (it is not a "`422`" or "`420`" format) but the implementation supports
        /// sampler Y'C~B~C~R~ conversion for this format, the implementation must set <see cref="MidpointChromaSamplesKhr"/>.
        /// </summary>
        MidpointChromaSamplesKhr = 1 << 17,
        /// <summary>
        /// Specifies that the format can do linear sampler filtering (min/magFilter) whilst sampler
        /// Y'C~B~C~R~ conversion is enabled.
        /// </summary>
        SampledImageYcbcrConversionLinearFilterKhr = 1 << 18,
        /// <summary>
        /// Specifies that the format can have different chroma, min, and mag filters.
        /// </summary>
        SampledImageYcbcrConversionSeparateReconstructionFilterKhr = 1 << 19,
        /// <summary>
        /// Specifies that reconstruction is explicit, as described in
        /// textures-chroma-reconstruction. If this bit is not present, reconstruction is implicit by default.
        /// </summary>
        SampledImageYcbcrConversionChromaReconstructionExplicitKhr = 1 << 20,
        /// <summary>
        /// Specifies that reconstruction can be forcibly made explicit by setting <see
        /// cref="Khr.SamplerYcbcrConversionCreateInfoKhr.ForceExplicitReconstruction"/> to <c>true</c>.
        /// </summary>
        SampledImageYcbcrConversionChromaReconstructionExplicitForceableKhr = 1 << 21,
        /// <summary>
        /// Specifies that a multi-planar image can have the <see
        /// cref="ImageCreateFlags.DisjointKhr"/> set during image creation. An implementation must
        /// not set <see cref="DisjointKhr"/> for single-plane Formats.
        /// </summary>
        DisjointKhr = 1 << 22,
        /// <summary>
        /// Specifies that an application can define a sampler Y'C~B~C~R~ conversion using this
        /// format as a source, and that an image of this format can be used with a <see
        /// cref="Khr.SamplerYcbcrConversionCreateInfoKhr"/><c>XChromaOffset</c> and/or
        /// <c>YChromaOffset</c> of <see cref="Khr.ChromaLocationKhr.CositedEven"/>. Otherwise both
        /// <c>XChromaOffset</c> and <c>YChromaOffset</c> must be <see
        /// cref="Khr.ChromaLocationKhr.Midpoint"/>. If neither <see cref="CositedChromaSamplesKhr"/>
        /// nor <see cref="MidpointChromaSamplesKhr"/> is set, the application must not define a
        /// sampler Y'C~B~C~R~ conversion using this format as a source.
        /// </summary>
        CositedChromaSamplesKhr = 1 << 23
    }

    /// <summary>
    /// Specifies the tiling arrangement of data in an image.
    /// </summary>
    public enum ImageTiling
    {
        /// <summary>
        /// Specifies optimal tiling (texels are laid out in an implementation-dependent arrangement,
        /// for more optimal memory access).
        /// </summary>
        Optimal = 0,
        /// <summary>
        /// Specifies linear tiling (texels are laid out in memory in row-major order, possibly with
        /// some padding on each row).
        /// </summary>
        Linear = 1
    }

    /// <summary>
    /// Specify filters used for texture lookups.
    /// </summary>
    public enum Filter
    {
        /// <summary>
        /// Specifies nearest filtering.
        /// </summary>
        Nearest = 0,
        /// <summary>
        /// Specifies linear filtering.
        /// </summary>
        Linear = 1,
        /// <summary>
        /// Specifies cubic filtering.
        /// </summary>
        CubicImg = 1000015000
    }

    /// <summary>
    /// Supported physical device types.
    /// </summary>
    public enum PhysicalDeviceType
    {
        /// <summary>
        /// The device does not match any other available types.
        /// </summary>
        Other = 0,
        /// <summary>
        /// The device is typically one embedded in or tightly coupled with the host.
        /// </summary>
        IntegratedGpu = 1,
        /// <summary>
        /// The device is typically a separate processor connected to the host via an interlink.
        /// </summary>
        DiscreteGpu = 2,
        /// <summary>
        /// The device is typically a virtual node in a virtualization environment.
        /// </summary>
        VirtualGpu = 3,
        /// <summary>
        /// The device is typically running on the same processors as the host.
        /// </summary>
        Cpu = 4
    }
}
