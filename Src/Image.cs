using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a image object.
    /// <para>
    /// Images represent multidimensional - up to 3 - arrays of data which can be used for various
    /// purposes (e.g. attachments, textures), by binding them to a graphics or compute pipeline via
    /// descriptor sets, or by directly specifying them as parameters to certain commands.
    /// </para>
    /// </summary>
    public unsafe class Image : DisposableHandle<long>
    {
        internal Image(Device parent, ref ImageCreateInfo createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            fixed (int* queueFamilyIndicesPtr = createInfo.QueueFamilyIndices)
            {
                createInfo.ToNative(out ImageCreateInfo.Native nativeCreateInfo, queueFamilyIndicesPtr);
                long handle;
                Result result = vkCreateImage(parent, &nativeCreateInfo, NativeAllocator, &handle);
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        internal Image(Device parent, long handle, AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Bind device memory to an image object.
        /// </summary>
        /// <param name="memory">The object describing the device memory to attach.</param>
        /// <param name="memoryOffset">
        /// The start offset of the region of memory which is to be bound to the image. The number of
        /// bytes returned in the <see cref="MemoryRequirements.Size"/> member in memory, starting
        /// from <paramref name="memoryOffset"/> bytes, will be bound to the specified image.
        /// </param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void BindMemory(DeviceMemory memory, long memoryOffset = 0)
        {
            Result result = vkBindImageMemory(Parent, this, memory, memoryOffset);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Returns the memory requirements for the image.
        /// </summary>
        /// <returns>Memory requirements of the image object.</returns>
        public MemoryRequirements GetMemoryRequirements()
        {
            MemoryRequirements requirements;
            vkGetImageMemoryRequirements(Parent, this, &requirements);
            return requirements;
        }

        /// <summary>
        /// Retrieve information about an image subresource.
        /// </summary>
        /// <returns>Subresource layout of an image.</returns>
        public SubresourceLayout GetSubresourceLayout(ImageSubresource subresource)
        {
            SubresourceLayout layout;
            vkGetImageSubresourceLayout(Parent, this, &subresource, &layout);
            return layout;
        }

        /// <summary>
        /// Create an image view from an existing image.
        /// </summary>
        /// <param name="createInfo">
        /// The structure containing parameters to be used to create the image view.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public ImageView CreateView(ImageViewCreateInfo createInfo, AllocationCallbacks? allocator = null)
        {
            return new ImageView(Parent, this, &createInfo, ref allocator);
        }

        /// <summary>
        /// Query the memory requirements for a sparse image.
        /// <para>
        /// If the image was not created with <see cref="ImageCreateFlags.SparseResidency"/> then the
        /// result will be empty.
        /// </para>
        /// </summary>
        /// <returns>Memory requirements for a sparse image.</returns>
        public SparseImageMemoryRequirements[] GetSparseMemoryRequirements()
        {
            int count;
            vkGetImageSparseMemoryRequirements(Parent, this, &count, null);

            var requirements = new SparseImageMemoryRequirements[count];
            fixed (SparseImageMemoryRequirements* requirementsPtr = requirements)
                vkGetImageSparseMemoryRequirements(Parent, this, &count, requirementsPtr);
            return requirements;
        }

        /// <summary>
        /// Destroy an image object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyImage(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateImageDelegate(IntPtr device, ImageCreateInfo.Native* createInfo, AllocationCallbacks.Native* allocator, long* image);
        private static readonly vkCreateImageDelegate vkCreateImage = VulkanLibrary.GetStaticProc<vkCreateImageDelegate>(nameof(vkCreateImage));

        private delegate void vkGetImageMemoryRequirementsDelegate(IntPtr device, long image, MemoryRequirements* memoryRequirements);
        private static readonly vkGetImageMemoryRequirementsDelegate vkGetImageMemoryRequirements = VulkanLibrary.GetStaticProc<vkGetImageMemoryRequirementsDelegate>(nameof(vkGetImageMemoryRequirements));

        private delegate void vkGetImageSparseMemoryRequirementsDelegate(IntPtr device, long image, int* sparseMemoryRequirementCount, SparseImageMemoryRequirements* sparseMemoryRequirements);
        private static readonly vkGetImageSparseMemoryRequirementsDelegate vkGetImageSparseMemoryRequirements = VulkanLibrary.GetStaticProc<vkGetImageSparseMemoryRequirementsDelegate>(nameof(vkGetImageSparseMemoryRequirements));

        private delegate void vkDestroyImageDelegate(IntPtr device, long image, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyImageDelegate vkDestroyImage = VulkanLibrary.GetStaticProc<vkDestroyImageDelegate>(nameof(vkDestroyImage));

        private delegate void vkGetImageSubresourceLayoutDelegate(IntPtr device, long image, ImageSubresource* subresource, SubresourceLayout* layout);
        private static readonly vkGetImageSubresourceLayoutDelegate vkGetImageSubresourceLayout = VulkanLibrary.GetStaticProc<vkGetImageSubresourceLayoutDelegate>(nameof(vkGetImageSubresourceLayout));

        private delegate Result vkBindImageMemoryDelegate(IntPtr device, long image, long memory, long memoryOffset);
        private static readonly vkBindImageMemoryDelegate vkBindImageMemory = VulkanLibrary.GetStaticProc<vkBindImageMemoryDelegate>(nameof(vkBindImageMemory));
    }

    /// <summary>
    /// Structure specifying the parameters of a newly created image object.
    /// </summary>
    public unsafe struct ImageCreateInfo
    {
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Bitmask describing additional parameters of the image.
        /// </summary>
        public ImageCreateFlags Flags;
        /// <summary>
        /// Specifies the basic dimensionality of the image.
        /// <para>
        /// Layers in array textures do not count as a dimension for the purposes of the image type.
        /// </para>
        /// </summary>
        public ImageType ImageType;
        /// <summary>
        /// Describes the format and type of the data elements that will be contained in the image.
        /// </summary>
        public Format Format;
        /// <summary>
        /// Describes the number of data elements in each dimension of the base level.
        /// <para>The width, height, and depth members of extent must all be greater than 0.</para>
        /// </summary>
        public Extent3D Extent;
        /// <summary>
        /// Describes the number of levels of detail available for minified sampling of the image.
        /// </summary>
        public int MipLevels;
        /// <summary>
        /// The number of layers in the image.
        /// </summary>
        public int ArrayLayers;
        /// <summary>
        /// The number of sub-data element samples in the image.
        /// </summary>
        public SampleCounts Samples;
        /// <summary>
        /// Specifies the tiling arrangement of the data elements in memory.
        /// </summary>
        public ImageTiling Tiling;
        /// <summary>
        /// A bitmask describing the intended usage of the image.
        /// </summary>
        public ImageUsages Usage;
        /// <summary>
        /// Specifies the sharing mode of the image when it will be accessed by multiple queue families.
        /// </summary>
        public SharingMode SharingMode;
        /// <summary>
        /// A list of queue families that will access this image (ignored if <see
        /// cref="SharingMode"/> is not <see cref="VulkanCore.SharingMode.Concurrent"/>).
        /// </summary>
        public int[] QueueFamilyIndices;
        /// <summary>
        /// Specifies the initial <see cref="ImageLayout"/> of all image subresources of the image.
        /// </summary>
        public ImageLayout InitialLayout;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public ImageCreateFlags Flags;
            public ImageType ImageType;
            public Format Format;
            public Extent3D Extent;
            public int MipLevels;
            public int ArrayLayers;
            public SampleCounts Samples;
            public ImageTiling Tiling;
            public ImageUsages Usage;
            public SharingMode SharingMode;
            public int QueueFamilyIndexCount;
            public int* QueueFamilyIndices;
            public ImageLayout InitialLayout;
        }

        internal void ToNative(out Native native, int* queueFamilyIndices)
        {
            native.Type = StructureType.ImageCreateInfo;
            native.Next = Next;
            native.Flags = Flags;
            native.ImageType = ImageType;
            native.Format = Format;
            native.Extent = Extent;
            native.MipLevels = MipLevels;
            native.ArrayLayers = ArrayLayers;
            native.Samples = Samples;
            native.Tiling = Tiling;
            native.Usage = Usage;
            native.SharingMode = SharingMode;
            native.QueueFamilyIndexCount = QueueFamilyIndices?.Length ?? 0;
            native.QueueFamilyIndices = queueFamilyIndices;
            native.InitialLayout = InitialLayout;
        }
    }

    /// <summary>
    /// Bitmask specifying additional parameters of an image.
    /// </summary>
    [Flags]
    public enum ImageCreateFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that the image will be backed using sparse memory binding.
        /// </summary>
        SparseBinding = 1 << 0,
        /// <summary>
        /// Specifies that the image can be partially backed using sparse memory binding. Images
        /// created with this flag must also be created with the <see cref="SparseBinding"/> flag.
        /// </summary>
        SparseResidency = 1 << 1,
        /// <summary>
        /// Specifies that the image will be backed using sparse memory binding with memory ranges
        /// that might also simultaneously be backing another image (or another portion of the same
        /// image). Images created with this flag must also be created with the <see
        /// cref="SparseBinding"/> flag.
        /// </summary>
        SparseAliased = 1 << 2,
        /// <summary>
        /// Specifies that the image can be used to create an <see cref="ImageView"/> with a
        /// different format from the image. For multi-planar formats, <see cref="MutableFormat"/>
        /// indicates that a <see cref="ImageView"/> can be created of a plane of the image.
        /// </summary>
        MutableFormat = 1 << 3,
        /// <summary>
        /// Specifies that the image can be used to create an <see cref="ImageView"/> of type <see
        /// cref="ImageViewType.ImageCube"/> or <see cref="ImageViewType.ImageCubeArray"/>.
        /// </summary>
        CubeCompatible = 1 << 4,
        /// <summary>
        /// Specifies that the image can be used to create a <see cref="ImageView"/> of type <see
        /// cref="ImageViewType.Image2D"/> or <see cref="ImageViewType.Image2DArray"/>.
        /// </summary>
        Image2DArrayCompatibleKhr = 1 << 5,
        /// <summary>
        /// Indicates that the image having a compressed format can be used to create a <see
        /// cref="ImageView"/> with an uncompressed format where each texel in the image view
        /// corresponds to a compressed texel block of the image.
        /// </summary>
        BlockTexelViewCompatibleKhr = 1 << 7,
        /// <summary>
        /// Indicates that the image can be created with usage flags that are not supported for the
        /// format the image is created with but are supported for at least one format a <see
        /// cref="ImageView"/> created from the image can have.
        /// </summary>
        ExtendedUsageKhr = 1 << 8,
        /// <summary>
        /// Indicates that an image with a multi-planar format must have each plane separately bound
        /// to memory, rather than having a single memory binding for the whole image; the presence
        /// of this bit distinguishes a disjoint Image from an image without this bit set.
        /// </summary>
        DisjointKhr = 1 << 9,
        /// <summary>
        /// Indicates that two images created with the same creation parameters and aliased to the
        /// same memory can interpret the contents of the memory consistently with each other,
        /// subject to the rules described in the Memory Aliasing section. This flag further
        /// indicates that each plane of a disjoint image can share an in-memory non-linear
        /// representation with single-plane images, and that a single-plane image can share an
        /// in-memory non-linear representation with a plane of a multi-planar disjoint image,
        /// according to the rules in features-formats-compatible-planes. If the <c>PNext</c> chain
        /// includes a structure whose <c>HandleTypes</c> member is not `0`, it is as if <see
        /// cref="AliasKhr"/> is set.
        /// </summary>
        AliasKhr = 1 << 10,
        /// <summary>
        /// Specifies that an image with a depth or depth/stencil format can be used with custom
        /// sample locations when used as a depth/stencil attachment.
        /// </summary>
        SampleLocationsCompatibleDepthExt = 1 << 12
    }

    /// <summary>
    /// Specifies the type of an image object.
    /// </summary>
    public enum ImageType
    {
        /// <summary>
        /// Specify a one-dimensional image.
        /// </summary>
        Image1D = 0,
        /// <summary>
        /// Specify a two-dimensional image.
        /// </summary>
        Image2D = 1,
        /// <summary>
        /// Specify a three-dimensional image.
        /// </summary>
        Image3D = 2
    }

    /// <summary>
    /// Bitmask specifying intended usage of an image.
    /// </summary>
    [Flags]
    public enum ImageUsages
    {
        /// <summary>
        /// Specifies that the image can be used as the source of a transfer command.
        /// </summary>
        TransferSrc = 1 << 0,
        /// <summary>
        /// Specifies that the image can be used as the destination of a transfer command.
        /// </summary>
        TransferDst = 1 << 1,
        /// <summary>
        /// Specifies that the image can be used to create a <see cref="ImageView"/> suitable for
        /// occupying a <see cref="DescriptorSet"/> slot either of type <see
        /// cref="DescriptorType.SampledImage"/> or <see
        /// cref="DescriptorType.CombinedImageSampler"/>, and be sampled by a shader.
        /// </summary>
        Sampled = 1 << 2,
        /// <summary>
        /// Specifies that the image can be used to create a <see cref="ImageView"/> suitable for
        /// occupying a <see cref="DescriptorSet"/> slot of type <see cref="DescriptorType.StorageImage"/>.
        /// </summary>
        Storage = 1 << 3,
        /// <summary>
        /// Specifies that the image can be used to create a <see cref="ImageView"/> suitable for use
        /// as a color or resolve attachment in a <see cref="Framebuffer"/>.
        /// </summary>
        ColorAttachment = 1 << 4,
        /// <summary>
        /// Specifies that the image can be used to create a <see cref="ImageView"/> suitable for use
        /// as a depth/stencil attachment in a <see cref="Framebuffer"/>.
        /// </summary>
        DepthStencilAttachment = 1 << 5,
        /// <summary>
        /// Specifies that the memory bound to this image will have been allocated with the <see
        /// cref="MemoryProperties.LazilyAllocated"/>. This bit can be set for any image that can
        /// be used to create a <see cref="ImageView"/> suitable for use as a color, resolve,
        /// depth/stencil, or input attachment.
        /// </summary>
        TransientAttachment = 1 << 6,
        /// <summary>
        /// Specifies that the image can be used to create a <see cref="ImageView"/> suitable for
        /// occupying <see cref="DescriptorSet"/> slot of type <see
        /// cref="DescriptorType.InputAttachment"/>; be read from a shader as an input attachment;
        /// and be used as an input attachment in a framebuffer.
        /// </summary>
        InputAttachment = 1 << 7
    }

    /// <summary>
    /// Layout of image and image subresources.
    /// </summary>
    public enum ImageLayout
    {
        /// <summary>
        /// Does not support device access. This layout must only be used as the <see
        /// cref="ImageCreateInfo.InitialLayout"/> or <see
        /// cref="AttachmentDescription.InitialLayout"/> member, or as the old layout in an image
        /// transition. When transitioning out of this layout, the contents of the memory are not
        /// guaranteed to be preserved.
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Supports all types of device access.
        /// </summary>
        General = 1,
        /// <summary>
        /// Must only be used as a color or resolve attachment in a <see cref="Framebuffer"/>. This
        /// layout is valid only for image subresources of images created with the <see
        /// cref="ImageUsages.ColorAttachment"/> usage bit enabled.
        /// </summary>
        ColorAttachmentOptimal = 2,
        /// <summary>
        /// Must only be used as a depth/stencil attachment in a <see cref="Framebuffer"/>. This
        /// layout is valid only for image subresources of images created with the <see
        /// cref="ImageUsages.DepthStencilAttachment"/> usage bit enabled.
        /// </summary>
        DepthStencilAttachmentOptimal = 3,
        /// <summary>
        /// Must only be used as a read-only depth/stencil attachment in a <see cref="Framebuffer"/>
        /// and/or as a read-only image in a shader (which can be read as a sampled image, combined
        /// image/sampler and/or input attachment). This layout is valid only for image subresources
        /// of images created with the <see cref="ImageUsages.DepthStencilAttachment"/> usage bit
        /// enabled. Only image subresources of images created with <see cref="ImageUsages.Sampled"/>
        /// can be used as a sampled image or combined image/sampler in a shader. Similarly, only
        /// image subresources of images created with <see cref="ImageUsages.InputAttachment"/> can
        /// be used as input attachments.
        /// </summary>
        DepthStencilReadOnlyOptimal = 4,
        /// <summary>
        /// Must only be used as a read-only image in a shader (which can be read as a sampled image,
        /// combined image/sampler and/or input attachment). This layout is valid only for image
        /// subresources of images created with the <see cref="ImageUsages.Sampled"/> or <see
        /// cref="ImageUsages.InputAttachment"/> usage bit enabled.
        /// </summary>
        ShaderReadOnlyOptimal = 5,
        /// <summary>
        /// Must only be used as a source image of a transfer command (see the definition of <see
        /// cref="PipelineStages.Transfer"/>. This layout is valid only for image subresources of
        /// images created with the <see cref="ImageUsages.TransferSrc"/> usage bit enabled.
        /// </summary>
        TransferSrcOptimal = 6,
        /// <summary>
        /// Must only be used as a destination image of a transfer command. This layout is valid only
        /// for image subresources of images created with the <see
        /// cref="ImageUsages.TransferDst"/> usage bit enabled.
        /// </summary>
        TransferDstOptimal = 7,
        /// <summary>
        /// Does not support device access. This layout must only be used as the <see
        /// cref="ImageCreateInfo.InitialLayout"/> or <see
        /// cref="AttachmentDescription.InitialLayout"/> member, or as the old layout in an image
        /// transition. When transitioning out of this layout, the contents of the memory are
        /// preserved. This layout is intended to be used as the initial layout for an image whose
        /// contents are written by the host, and hence the data can be written to memory
        /// immediately, without first executing a layout transition. Currently, <see
        /// cref="Preinitialized"/> is only useful with <see cref="ImageTiling.Linear"/> images
        /// because there is not a standard layout defined for <see cref="ImageTiling.Optimal"/> images.
        /// </summary>
        Preinitialized = 8,
        /// <summary>
        /// Must only be used for presenting a swapchain image for display. A swapchain's image must
        /// be transitioned to this layout before calling <see
        /// cref="QueueExtensions.PresentKhr(Queue, PresentInfoKhr)"/>, and must be transitioned away
        /// from this layout after calling <see cref="SwapchainKhr.vkAcquireNextImageKHR"/>.
        /// </summary>
        PresentSrcKhr = 1000001002,
        /// <summary>
        /// Is valid only for shared presentable images, and must be used for any usage the image supports.
        /// </summary>
        SharedPresentKhr = 1000111000,
        DepthReadOnlyStencilAttachmentOptimalKhr = 1000117000,
        DepthAttachmentStencilReadOnlyOptimalKhr = 1000117001
    }

    /// <summary>
    /// Structure specifying an attachment description.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AttachmentDescription
    {
        /// <summary>
        /// A bitmask specifying additional properties of the attachment.
        /// </summary>
        public AttachmentDescriptions Flags;
        /// <summary>
        /// Specifies the format of the image that will be used for the attachment.
        /// </summary>
        public Format Format;
        /// <summary>
        /// The number of samples of the image.
        /// </summary>
        public SampleCounts Samples;
        /// <summary>
        /// Specifies how the contents of color and depth components of the attachment are treated at
        /// the beginning of the subpass where it is first used.
        /// </summary>
        public AttachmentLoadOp LoadOp;
        /// <summary>
        /// Specifies how the contents of color and depth components of the attachment are treated at
        /// the end of the subpass where it is last used.
        /// </summary>
        public AttachmentStoreOp StoreOp;
        /// <summary>
        /// Specifies how the contents of stencil components of the attachment are treated at the
        /// beginning of the subpass where it is first used, and must be one of the same values
        /// allowed for <see cref="LoadOp"/> above.
        /// </summary>
        public AttachmentLoadOp StencilLoadOp;
        /// <summary>
        /// Specifies how the contents of stencil components of the attachment are treated at the end
        /// of the last subpass where it is used, and must be one of the same values allowed for <see
        /// cref="StoreOp"/> above.
        /// </summary>
        public AttachmentStoreOp StencilStoreOp;
        /// <summary>
        /// The layout the attachment image subresource will be in when a render pass instance begins.
        /// </summary>
        public ImageLayout InitialLayout;
        /// <summary>
        /// The layout the attachment image subresource will be transitioned to when a render pass
        /// instance ends. During a render pass instance, an attachment can use a different layout in
        /// each subpass, if desired.
        /// </summary>
        public ImageLayout FinalLayout;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentDescription"/> structure.
        /// </summary>
        /// <param name="flags">A bitmask specifying additional properties of the attachment.</param>
        /// <param name="format">
        /// Specifies the format of the image that will be used for the attachment.
        /// </param>
        /// <param name="samples">The number of samples of the image.</param>
        /// <param name="loadOp">
        /// Specifies how the contents of color and depth components of the attachment are treated at
        /// the beginning of the subpass where it is first used.
        /// </param>
        /// <param name="storeOp">
        /// Specifies how the contents of color and depth components of the attachment are treated at
        /// the end of the subpass where it is last used.
        /// </param>
        /// <param name="stencilLoadOp">
        /// Specifies how the contents of stencil components of the attachment are treated at the
        /// beginning of the subpass where it is first used, and must be one of the same values
        /// allowed for <see cref="LoadOp"/> above.
        /// </param>
        /// <param name="stencilStoreOp">
        /// Specifies how the contents of stencil components of the attachment are treated at the end
        /// of the last subpass where it is used, and must be one of the same values allowed for <see
        /// cref="StoreOp"/> above.
        /// </param>
        /// <param name="initialLayout">
        /// The layout the attachment image subresource will be in when a render pass instance begins.
        /// </param>
        /// <param name="finalLayout">
        /// The layout the attachment image subresource will be transitioned to when a render pass
        /// instance ends. During a render pass instance, an attachment can use a different layout in
        /// each subpass, if desired.
        /// </param>
        public AttachmentDescription(
            AttachmentDescriptions flags,
            Format format,
            SampleCounts samples,
            AttachmentLoadOp loadOp,
            AttachmentStoreOp storeOp,
            AttachmentLoadOp stencilLoadOp,
            AttachmentStoreOp stencilStoreOp,
            ImageLayout initialLayout,
            ImageLayout finalLayout)
        {
            Flags = flags;
            Format = format;
            Samples = samples;
            LoadOp = loadOp;
            StoreOp = storeOp;
            StencilLoadOp = stencilLoadOp;
            StencilStoreOp = stencilStoreOp;
            InitialLayout = initialLayout;
            FinalLayout = finalLayout;
        }
    }

    /// <summary>
    /// Bitmask specifying additional properties of an attachment.
    /// </summary>
    [Flags]
    public enum AttachmentDescriptions
    {
        /// <summary>
        /// Specifies that the attachment aliases the same device memory as other attachments.
        /// </summary>
        MayAlias = 1 << 0
    }

    /// <summary>
    /// Specify how contents of an attachment are treated at the beginning of a subpass.
    /// </summary>
    public enum AttachmentLoadOp
    {
        /// <summary>
        /// Specifies that the previous contents of the image within the render area will be
        /// preserved. For attachments with a depth/stencil format, this uses the access type <see
        /// cref="Accesses.DepthStencilAttachmentRead"/>. For attachments with a color format, this
        /// uses the access type <see cref="Accesses.ColorAttachmentRead"/>.
        /// </summary>
        Load = 0,
        /// <summary>
        /// Specifies that the contents within the render area will be cleared to a uniform value,
        /// which is specified when a render pass instance is begun. For attachments with a
        /// depth/stencil format, this uses the access type <see
        /// cref="Accesses.DepthStencilAttachmentWrite"/>. For attachments with a color format, this
        /// uses the access type <see cref="Accesses.ColorAttachmentWrite"/>.
        /// </summary>
        Clear = 1,
        /// <summary>
        /// Specifies that the previous contents within the area need not be preserved; the contents
        /// of the attachment will be undefined inside the render area. For attachments with a
        /// depth/stencil format, this uses the access type <see
        /// cref="Accesses.DepthStencilAttachmentWrite"/>. For attachments with a color format, this
        /// uses the access type <see cref="Accesses.ColorAttachmentWrite"/>.
        /// </summary>
        DontCare = 2
    }

    /// <summary>
    /// Specify how contents of an attachment are treated at the end of a subpass.
    /// </summary>
    public enum AttachmentStoreOp
    {
        /// <summary>
        /// Specifies the contents generated during the render pass and within the render area are
        /// written to memory. For attachments with a depth/stencil format, this uses the access type
        /// <see cref="Accesses.DepthStencilAttachmentWrite"/>. For attachments with a color format,
        /// this uses the access type <see cref="Accesses.ColorAttachmentWrite"/>.
        /// </summary>
        Store = 0,
        /// <summary>
        /// Specifies the contents within the render area are not needed after rendering, and may be
        /// discarded; the contents of the attachment will be undefined inside the render area. For
        /// attachments with a depth/stencil format, this uses the access type <see
        /// cref="Accesses.DepthStencilAttachmentWrite"/>. For attachments with a color format, this
        /// uses the access type <see cref="Accesses.ColorAttachmentWrite"/>.
        /// </summary>
        DontCare = 1
    }

    /// <summary>
    /// Structure specifying a image format properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageFormatProperties
    {
        /// <summary>
        /// Are the maximum image dimensions.
        /// </summary>
        public Extent3D MaxExtent;
        /// <summary>
        /// The maximum number of mipmap levels. Must either be equal to 1 (valid only if tiling is
        /// <see cref="ImageTiling.Linear"/>) or be equal to `ceil(log2(max(width, height, depth))) +
        /// 1`. Width, height and depth are taken from the corresponding members of <see cref="MaxExtent"/>.
        /// </summary>
        public int MaxMipLevels;
        /// <summary>
        /// The maximum number of array layers. Must either be equal to 1 or be greater than or equal
        /// to the <see cref="PhysicalDeviceLimits.MaxImageArrayLayers"/> member. A value of 1 is
        /// valid only if tiling is <see cref="ImageTiling.Linear"/> or if type is <see cref="ImageType.Image3D"/>.
        /// </summary>
        public int MaxArrayLayers;
        /// <summary>
        /// A bitmask of <see cref="VulkanCore.SampleCounts"/> specifying all the supported sample
        /// counts for this image.
        /// </summary>
        public SampleCounts SampleCounts;
        /// <summary>
        /// An upper bound on the total image size in bytes, inclusive of all image subresources.
        /// Implementations may have an address space limit on total size of a resource, which is
        /// advertised by this property. Must be at least 2^31.
        /// </summary>
        public long MaxResourceSize;
    }

    /// <summary>
    /// Structure specifying an image subresource.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageSubresource
    {
        /// <summary>
        /// An <see cref="ImageAspects"/> selecting the image aspect.
        /// </summary>
        public ImageAspects AspectMask;
        /// <summary>
        /// Selects the mipmap level.
        /// </summary>
        public int MipLevel;
        /// <summary>
        /// Selects the array layer.
        /// </summary>
        public int ArrayLayer;

        /// <summary>
        /// Initializes a new instance of <see cref="ImageSubresource"/> structure.
        /// </summary>
        /// <param name="aspectMask">An <see cref="ImageAspects"/> selecting the image aspect.</param>
        /// <param name="mipLevel">Selects the mipmap level.</param>
        /// <param name="arrayLayer">Selects the array layer.</param>
        public ImageSubresource(ImageAspects aspectMask, int mipLevel, int arrayLayer)
        {
            AspectMask = aspectMask;
            MipLevel = mipLevel;
            ArrayLayer = arrayLayer;
        }
    }

    /// <summary>
    /// Structure specifying sparse image format properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SparseImageFormatProperties
    {
        /// <summary>
        /// A bitmask specifying which aspects of the image the properties apply to.
        /// </summary>
        public ImageAspects AspectMask;
        /// <summary>
        /// The width, height, and depth of the sparse image block in texels or compressed texel blocks.
        /// </summary>
        public Extent3D ImageGranularity;
        /// <summary>
        /// A bitmask specifying additional information about the sparse resource.
        /// </summary>
        public SparseImageFormats Flags;
    }

    /// <summary>
    /// Bitmask specifying additional information about a sparse image resource.
    /// </summary>
    [Flags]
    public enum SparseImageFormats
    {
        /// <summary>
        /// Specifies that the image uses a single mip tail region for all array layers.
        /// </summary>
        SingleMiptail = 1 << 0,
        /// <summary>
        /// Specifies that the first mip level whose dimensions are not integer multiples of the
        /// corresponding dimensions of the sparse image block begins the mip tail region.
        /// </summary>
        AlignedMipSize = 1 << 1,
        /// <summary>
        /// Specifies that the image uses non-standard sparse image block dimensions, and the image
        /// granularity values do not match the standard sparse image block dimensions for the given format.
        /// </summary>
        NonstandardBlockSize = 1 << 2
    }

    /// <summary>
    /// Structure specifying sparse image memory requirements.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SparseImageMemoryRequirements
    {
        /// <summary>
        /// The sparse image format properties.
        /// </summary>
        public SparseImageFormatProperties FormatProperties;
        /// <summary>
        /// The first mip level at which image subresources are included in the mip tail region.
        /// </summary>
        public int ImageMipTailFirstLod;
        /// <summary>
        /// The memory size (in bytes) of the mip tail region. If <see cref="FormatProperties"/>
        /// contains <see cref="SparseImageFormats.SingleMiptail"/>, this is the size of the
        /// whole mip tail, otherwise this is the size of the mip tail of a single array layer. This
        /// value is guaranteed to be a multiple of the sparse block size in bytes.
        /// </summary>
        public long ImageMipTailSize;
        /// <summary>
        /// The opaque memory offset used with <see cref="SparseImageOpaqueMemoryBindInfo"/> to bind
        /// the mip tail region(s).
        /// </summary>
        public long ImageMipTailOffset;
        /// <summary>
        /// The offset stride between each array-layer's mip tail, if <see cref="FormatProperties"/>
        /// does not contain <see cref="SparseImageFormats.SingleMiptail"/> (otherwise the value
        /// is undefined).
        /// </summary>
        public long ImageMipTailStride;
    }

    /// <summary>
    /// Structure specifying subresource layout.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SubresourceLayout
    {
        /// <summary>
        /// The byte offset from the start of the image where the image subresource begins.
        /// </summary>
        public long Offset;
        /// <summary>
        /// The size in bytes of the image subresource. size includes any extra memory that is
        /// required based on <see cref="RowPitch"/>.
        /// </summary>
        public long Size;
        /// <summary>
        /// Describes the number of bytes between each row of texels in an image.
        /// </summary>
        public long RowPitch;
        /// <summary>
        /// Describes the number of bytes between each array layer of an image.
        /// </summary>
        public long ArrayPitch;
        /// <summary>
        /// Describes the number of bytes between each slice of 3D image.
        /// </summary>
        public long DepthPitch;
    }

    /// <summary>
    /// Bitmask specifying sample counts supported for an image used for storage
    /// operations.
    /// </summary>
    [Flags]
    public enum SampleCounts
    {
        /// <summary>
        /// Specifies an image with one sample per pixel.
        /// </summary>
        Count1 = 1 << 0,
        /// <summary>
        /// Specifies an image with 2 samples per pixel.
        /// </summary>
        Count2 = 1 << 1,
        /// <summary>
        /// Specifies an image with 4 samples per pixel.
        /// </summary>
        Count4 = 1 << 2,
        /// <summary>
        /// Specifies an image with 8 samples per pixel.
        /// </summary>
        Count8 = 1 << 3,
        /// <summary>
        /// Specifies an image with 16 samples per pixel.
        /// </summary>
        Count16 = 1 << 4,
        /// <summary>
        /// Specifies an image with 32 samples per pixel.
        /// </summary>
        Count32 = 1 << 5,
        /// <summary>
        /// Specifies an image with 64 samples per pixel.
        /// </summary>
        Count64 = 1 << 6
    }
}
