using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a image view object.
    /// <para>
    /// Image objects are not directly accessed by pipeline shaders for reading or writing image
    /// data. Instead, image views representing contiguous ranges of the image subresources and
    /// containing additional metadata are used for that purpose. Views must be created on images of
    /// compatible types, and must represent a valid subset of image subresources.
    /// </para>
    /// </summary>
    public unsafe class ImageView : DisposableHandle<long>
    {
        internal ImageView(Device parent, Image image, ImageViewCreateInfo* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            createInfo->Prepare(image);

            long handle;
            Result result = vkCreateImageView(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Destroy an image view object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyImageView(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateImageViewDelegate(IntPtr device, ImageViewCreateInfo* createInfo, AllocationCallbacks.Native* allocator, long* view);
        private static readonly vkCreateImageViewDelegate vkCreateImageView = VulkanLibrary.GetStaticProc<vkCreateImageViewDelegate>(nameof(vkCreateImageView));

        private delegate void vkDestroyImageViewDelegate(IntPtr device, long imageView, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyImageViewDelegate vkDestroyImageView = VulkanLibrary.GetStaticProc<vkDestroyImageViewDelegate>(nameof(vkDestroyImageView));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created image view.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageViewCreateInfo
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal ImageViewCreateFlags Flags;
        internal long Image;

        /// <summary>
        /// Specifies the type of the image view.
        /// </summary>
        public ImageViewType ViewType;
        /// <summary>
        /// A <see cref="Format"/> describing the format and type used to interpret data elements in
        /// the image.
        /// </summary>
        public Format Format;
        /// <summary>
        /// Specifies a remapping of color components (or of depth or stencil components after they
        /// have been converted into color components).
        /// </summary>
        public ComponentMapping Components;
        /// <summary>
        /// A range selecting the set of mipmap levels and array layers to be accessible to the view.
        /// <para>
        /// Must be a valid image subresource range for image. If image was created with the <see
        /// cref="ImageCreateFlags.MutableFormat"/> flag, format must be compatible with the format
        /// used to create image. If image was not created with the <see
        /// cref="ImageCreateFlags.MutableFormat"/> flag, format must be identical to the format used
        /// to create image. If image is non-sparse then it must be bound completely and contiguously
        /// to a single <see cref="DeviceMemory"/> object.
        /// </para>
        /// </summary>
        public ImageSubresourceRange SubresourceRange;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageViewCreateInfo"/> structure.
        /// </summary>
        /// <param name="format">
        /// A <see cref="Format"/> describing the format and type used to interpret data elements in
        /// the image.
        /// </param>
        /// <param name="subresourceRange">
        /// A range selecting the set of mipmap levels and array layers to be accessible to the view.
        /// </param>
        /// <param name="viewType">Specifies the type of the image view.</param>
        /// <param name="components">
        /// Specifies a remapping of color components (or of depth or stencil components after they
        /// have been converted into color components).
        /// </param>
        public ImageViewCreateInfo(
            Format format,
            ImageSubresourceRange subresourceRange,
            ImageViewType viewType = ImageViewType.Image2D,
            ComponentMapping components = default(ComponentMapping))
        {
            Type = StructureType.ImageViewCreateInfo;
            Next = IntPtr.Zero;
            Flags = 0;
            Image = 0;
            Format = format;
            SubresourceRange = subresourceRange;
            ViewType = viewType;
            Components = components;
        }

        internal void Prepare(Image image)
        {
            Type = StructureType.ImageViewCreateInfo;
            Image = image;
        }
    }

    // Reserved for future use.
    [Flags]
    internal enum ImageViewCreateFlags
    {
        None = 0,
    }

    /// <summary>
    /// Image view types.
    /// </summary>
    public enum ImageViewType
    {
        Image1D = 0,
        Image2D = 1,
        Image3D = 2,
        ImageCube = 3,
        Image1DArray = 4,
        Image2DArray = 5,
        ImageCubeArray = 6
    }

    /// <summary>
    /// Structure specifying a color component mapping.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ComponentMapping
    {
        /// <summary>
        /// Specifies the component value placed in the R component of the output vector.
        /// </summary>
        public ComponentSwizzle R;
        /// <summary>
        /// Specifies the component value placed in the G component of the output vector.
        /// </summary>
        public ComponentSwizzle G;
        /// <summary>
        /// Specifies the component value placed in the B component of the output vector.
        /// </summary>
        public ComponentSwizzle B;
        /// <summary>
        /// Specifies the component value placed in the A component of the output vector.
        /// </summary>
        public ComponentSwizzle A;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentMapping"/> structure.
        /// </summary>
        /// <param name="r">
        /// Specifies the component value placed in the R component of the output vector.
        /// </param>
        /// <param name="g">
        /// Specifies the component value placed in the G component of the output vector.
        /// </param>
        /// <param name="b">
        /// Specifies the component value placed in the B component of the output vector.
        /// </param>
        /// <param name="a">
        /// Specifies the component value placed in the A component of the output vector.
        /// </param>
        public ComponentMapping(ComponentSwizzle r, ComponentSwizzle g, ComponentSwizzle b, ComponentSwizzle a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }

    /// <summary>
    /// Specify how a component is swizzled.
    /// </summary>
    public enum ComponentSwizzle
    {
        /// <summary>
        /// Specifies that the component is set to the identity swizzle.
        /// </summary>
        Identity = 0,
        /// <summary>
        /// Specifies that the component is set to zero.
        /// </summary>
        Zero = 1,
        /// <summary>
        /// Specifies that the component is set to either 1 or 1.0, depending on whether the type of
        /// the image view format is integer or floating-point respectively.
        /// </summary>
        One = 2,
        /// <summary>
        /// Specifies that the component is set to the value of the R component of the image.
        /// </summary>
        R = 3,
        /// <summary>
        /// Specifies that the component is set to the value of the G component of the image.
        /// </summary>
        G = 4,
        /// <summary>
        /// Specifies that the component is set to the value of the B component of the image.
        /// </summary>
        B = 5,
        /// <summary>
        /// Specifies that the component is set to the value of the A component of the image.
        /// </summary>
        A = 6
    }

    /// <summary>
    /// Structure specifying a image subresource range.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageSubresourceRange
    {
        /// <summary>
        /// A bitmask specifying which aspect(s) of the image are included in the view.
        /// </summary>
        public ImageAspects AspectMask;
        /// <summary>
        /// The first mipmap level accessible to the view.
        /// </summary>
        public int BaseMipLevel;
        /// <summary>
        /// The number of mipmap levels (starting from <see cref="BaseMipLevel"/>) accessible to the view.
        /// </summary>
        public int LevelCount;
        /// <summary>
        /// The first array layer accessible to the view.
        /// </summary>
        public int BaseArrayLayer;
        /// <summary>
        /// The number of array layers (starting from <see cref="BaseArrayLayer"/>) accessible to the view.
        /// </summary>
        public int LayerCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSubresourceRange"/> structure.
        /// </summary>
        /// <param name="aspectMask">
        /// A bitmask specifying which aspect(s) of the image are included in the view.
        /// </param>
        /// <param name="baseMipLevel">The first mipmap level accessible to the view.</param>
        /// <param name="levelCount">
        /// The number of mipmap levels (starting from <see cref="BaseMipLevel"/>) accessible to the view.
        /// </param>
        /// <param name="baseArrayLayer">The first array layer accessible to the view.</param>
        /// <param name="layerCount">
        /// The number of array layers (starting from <see cref="BaseArrayLayer"/>) accessible to the view.
        /// </param>
        public ImageSubresourceRange(ImageAspects aspectMask,
            int baseMipLevel, int levelCount,
            int baseArrayLayer, int layerCount)
        {
            AspectMask = aspectMask;
            BaseMipLevel = baseMipLevel;
            LevelCount = levelCount;
            BaseArrayLayer = baseArrayLayer;
            LayerCount = layerCount;
        }
    }

    /// <summary>
    /// Bitmask specifying which aspects of an image are included in a view.
    /// </summary>
    [Flags]
    public enum ImageAspects
    {
        /// <summary>
        /// Specifies the color aspect.
        /// </summary>
        Color = 1 << 0,
        /// <summary>
        /// Specifies the depth aspect.
        /// </summary>
        Depth = 1 << 1,
        /// <summary>
        /// Specifies the stencil aspect.
        /// </summary>
        Stencil = 1 << 2,
        /// <summary>
        /// Specifies the metadata aspect, used for sparse resource operations.
        /// </summary>
        Metadata = 1 << 3,
        Plane0Khr = 1 << 4,
        Plane1Khr = 1 << 5,
        Plane2Khr = 1 << 6
    }
}
