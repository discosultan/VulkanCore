using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore.Khr
{
    /// <summary>
    /// Provides the ability to present rendering results to a surface.
    /// <para>
    /// A swapchain is an abstraction for an array of presentable images that are associated with a
    /// surface. The swapchain images are represented by <see cref="Image"/> objects created by the
    /// platform. One image (which can be an array image for multiview/stereoscopic-3D surfaces) is
    /// displayed at a time, but multiple images can be queued for presentation. An application
    /// renders to the image, and then queues the image for presentation to the surface.
    /// </para>
    /// <para>A native window cannot be associated with more than one swapchain at a time.</para>
    /// <para>
    /// Further, swapchains cannot be created for native windows that have a non-Vulkan graphics API
    /// surface associated with them.
    /// </para>
    /// </summary>
    public unsafe class SwapchainKhr : DisposableHandle<long>
    {
        internal SwapchainKhr(Device parent, ref SwapchainCreateInfoKhr createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;
            Format = createInfo.ImageFormat;

            fixed (int* queueFamilyIndicesPtr = createInfo.QueueFamilyIndices)
            {
                long handle;
                createInfo.ToNative(out SwapchainCreateInfoKhr.Native nativeCreateInfo, queueFamilyIndicesPtr);
                Result result = CreateSwapchainKhr(Parent, &nativeCreateInfo, NativeAllocator, &handle);
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        internal SwapchainKhr(Device parent, long handle, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of this resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Gets the format.
        /// </summary>
        public Format Format { get; }

        /// <summary>
        /// Obtain the array of presentable images associated with a swapchain.
        /// </summary>
        /// <returns>An array of <see cref="Image"/> objects.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Image[] GetImages()
        {
            int swapchainImageCount;            
            Result result = GetSwapchainImagesKhr(Parent, this, &swapchainImageCount, null);
            VulkanException.ThrowForInvalidResult(result);

            var swapchainImages = stackalloc long[swapchainImageCount];
            result = GetSwapchainImagesKhr(Parent, this, &swapchainImageCount, swapchainImages);
            VulkanException.ThrowForInvalidResult(result);

            var images = new Image[swapchainImageCount];
            for (int i = 0; i < swapchainImageCount; i++)
                images[i] = new Image(Parent, swapchainImages[i], Allocator);
            return images;
        }

        /// <summary>
        /// Retrieve the index of the next available presentable image.
        /// </summary>
        /// <param name="timeout">
        /// Indicates how long the function waits, in nanoseconds, if no image is available.
        /// <para>
        /// If timeout is 0, the command will not block, but will either succeed or throw with <see
        /// cref="Result.NotReady"/>. If timeout is -1, the function will not return until an image
        /// is acquired from the presentation engine. Other values for timeout will cause the
        /// function to return when an image becomes available, or when the specified number of
        /// nanoseconds have passed (in which case it will return throw with <see
        /// cref="Result.Timeout"/>). An error can also cause the command to return early.
        /// </para>
        /// </param>
        /// <param name="semaphore">
        /// <c>null</c> or a semaphore to signal. <paramref name="semaphore"/> and <paramref
        /// name="fence"/> must not both be equal to <c>null</c>.
        /// </param>
        /// <param name="fence">
        /// <c>null</c> or a fence to signal. <paramref name="semaphore"/> and <paramref
        /// name="fence"/> must not both be equal to <c>null</c>.
        /// </param>
        /// <returns>The index of the next available presentable image.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public int AcquireNextImage(long timeout = ~0, Semaphore semaphore = null, Fence fence = null)
        {
            int nextImageIndex;
            Result result = AcquireNextImageKhr(Parent, this, timeout, semaphore, fence, &nextImageIndex);
            VulkanException.ThrowForInvalidResult(result);
            return nextImageIndex;
        }

        internal static SwapchainKhr[] CreateSharedKhr(Device parent, SwapchainCreateInfoKhr[] createInfos,
            ref AllocationCallbacks? allocator)
        {
            int count = createInfos?.Length ?? 0;
            var nativeCreateInfos = stackalloc SwapchainCreateInfoKhr.Native[count];
            var gcHandles = stackalloc GCHandle[count];
            for (int i = 0; i < count; i++)
            {
                gcHandles[i] = GCHandle.Alloc(createInfos[i].QueueFamilyIndices, GCHandleType.Pinned);
                createInfos[i].ToNative(out nativeCreateInfos[i], (int*)gcHandles[i].AddrOfPinnedObject());
            }
            AllocationCallbacks.Native nativeAllocator;
            allocator?.ToNative(&nativeAllocator);

            long* handles = stackalloc long[count];
            Result result = CreateSharedSwapchainsKhr(parent, count, nativeCreateInfos,
                allocator.HasValue ? &nativeAllocator : null, handles);
            for (int i = 0; i < count; i++)
                gcHandles[i].Free();

            VulkanException.ThrowForInvalidResult(result);

            var swapchains = new SwapchainKhr[count];
            for (int i = 0; i < count; i++)
                swapchains[i] = new SwapchainKhr(parent, handles[i], ref allocator);
            return swapchains;
        }

        protected override void DisposeManaged()
        {
            DestroySwapchainKhr(Parent, this, NativeAllocator);
            base.DisposeManaged();
        }        
        
        [DllImport(VulkanDll, EntryPoint = "vkCreateSwapchainKHR", CallingConvention = CallConv)]
        private static extern Result CreateSwapchainKhr(IntPtr device, 
            SwapchainCreateInfoKhr.Native* createInfo, AllocationCallbacks.Native* allocator, long* swapchain);
        
        [DllImport(VulkanDll, EntryPoint = "vkDestroySwapchainKHR", CallingConvention = CallConv)]
        private static extern void DestroySwapchainKhr(IntPtr device, long swapchain, AllocationCallbacks.Native* allocator);
        
        [DllImport(VulkanDll, EntryPoint = "vkGetSwapchainImagesKHR", CallingConvention = CallConv)]
        private static extern Result GetSwapchainImagesKhr(IntPtr device, long swapchain, int* swapchainImageCount, long* swapchainImages);
        
        [DllImport(VulkanDll, EntryPoint = "vkAcquireNextImageKHR", CallingConvention = CallConv)]
        private static extern Result AcquireNextImageKhr(IntPtr device, long swapchain, 
            long timeout, long semaphore, long fence, int* imageIndex);

        [DllImport(VulkanDll, EntryPoint = "vkCreateSharedSwapchainsKHR", CallingConvention = CallConv)]
        private static extern Result CreateSharedSwapchainsKhr(Device device,
            int swapchainCount, SwapchainCreateInfoKhr.Native* createInfos, AllocationCallbacks.Native* allocator, long* swapchains);
    }

    /// <summary>
    /// Structure specifying parameters of a newly created swapchain object.
    /// </summary>
    public unsafe struct SwapchainCreateInfoKhr
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwapchainCreateInfoKhr"/> structure.
        /// </summary>
        /// <param name="surface">
        /// The <see cref="SurfaceKhr"/> that the swapchain will present images to.
        /// </param>
        /// <param name="imageExtent">
        /// The size (in pixels) of the swapchain. Behavior is platform-dependent when the image
        /// extent does not match the surface's <see cref="SurfaceCapabilitiesKhr.CurrentExtent"/> as
        /// returned by <see cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/>.
        /// </param>
        /// <param name="preTransform">
        /// A bitmask describing the transform, relative to the presentation engine’s natural
        /// orientation, applied to the image content prior to presentation. If it does not match the
        /// <see cref="SurfaceCapabilitiesKhr.CurrentTransform"/> value returned by <see
        /// cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/>, the presentation engine will
        /// transform the image content as part of the presentation operation.
        /// </param>
        /// <param name="imageUsage">
        /// A bitmask indicating how the application will use the swapchain’s presentable images.
        /// </param>
        /// <param name="minImageCount">
        /// The minimum number of presentable images that the application needs. The platform will
        /// either create the swapchain with at least that many images, or will fail to create the swapchain.
        /// </param>
        /// <param name="imageFormat">A format that is valid for swapchains on the specified surface.</param>
        /// <param name="compositeAlpha">
        /// A bitmask indicating the alpha compositing mode to use when this surface is composited
        /// together with other surfaces on certain window systems.
        /// </param>
        /// <param name="imageArrayLayers">
        /// The number of views in a multiview/stereo surface. For non-stereoscopic-3D applications,
        /// this value is 1.
        /// </param>
        /// <param name="presentMode">
        /// The presentation mode the swapchain will use. A swapchain’s present mode determines how
        /// incoming present requests will be processed and queued internally.
        /// </param>
        /// <param name="clipped">
        /// Indicates whether the Vulkan implementation is allowed to discard rendering operations
        /// that affect regions of the surface which are not visible.
        /// </param>
        /// <param name="oldSwapchain">Existing swapchain to replace, if any.</param>
        public SwapchainCreateInfoKhr(SurfaceKhr surface, 
            Extent2D imageExtent, 
            SurfaceTransformsKhr preTransform,
            PresentModeKhr presentMode,
            ImageUsages imageUsage = ImageUsages.ColorAttachment | ImageUsages.TransferDst, 
            int minImageCount = 2,
            Format imageFormat = Format.B8G8R8A8UNorm,
            CompositeAlphasKhr compositeAlpha = CompositeAlphasKhr.Opaque, 
            int imageArrayLayers = 1,
            bool clipped = true,
            SwapchainKhr oldSwapchain = null)
        {
            Surface = surface;
            ImageUsage = imageUsage;
            MinImageCount = minImageCount;
            ImageFormat = imageFormat;
            ImageColorSpace = 0;
            ImageExtent = imageExtent;
            ImageArrayLayers = imageArrayLayers;
            ImageSharingMode = 0;
            QueueFamilyIndices = null;
            PreTransform = preTransform;
            CompositeAlpha = compositeAlpha;
            PresentMode = presentMode;
            Clipped = clipped;
            OldSwapchain = oldSwapchain;
        }

        /// <summary>
        /// The <see cref="SurfaceKhr"/> that the swapchain will present images to.
        /// <para>Must be a surface that is supported by the device as determined using <see cref="PhysicalDeviceExtensions.GetSurfaceSupportKhr"/>.</para>
        /// </summary>
        public long Surface;
        /// <summary>
        /// The minimum number of presentable images that the application needs. The platform will
        /// either create the swapchain with at least that many images, or will fail to create the swapchain.
        /// <para>
        /// Must be less than or equal to the value returned in the <see
        /// cref="SurfaceCapabilitiesKhr.MaxImageCount"/> member returned by <see
        /// cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/> for the surface if the
        /// returned max image count is not zero.
        /// </para>
        /// </summary>
        public int MinImageCount;
        /// <summary>
        /// A format that is valid for swapchains on the specified surface.
        /// <para>
        /// Must match the <see cref="SurfaceFormatKhr.Format"/> member of one of the structures
        /// returned by <see cref="PhysicalDeviceExtensions.GetSurfaceFormatsKhr"/> for the surface.
        /// </para>
        /// </summary>
        public Format ImageFormat;
        /// <summary>
        /// Color space that is valid for swapchains on the specified surface.
        /// <para>
        /// Must match the <see cref="SurfaceFormatKhr.ColorSpace"/> member of one of the structures
        /// returned by returned by <see cref="PhysicalDeviceExtensions.GetSurfaceFormatsKhr"/> for
        /// the surface.
        /// </para>
        /// </summary>
        public ColorSpaceKhr ImageColorSpace;
        /// <summary>
        /// The size (in pixels) of the swapchain. Behavior is platform-dependent when the image
        /// extent does not match the surface's <see cref="SurfaceCapabilitiesKhr.CurrentExtent"/> as
        /// returned by <see cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/>.
        /// <para>
        /// Must be between <see cref="SurfaceCapabilitiesKhr.MinImageExtent"/> and <see
        /// cref="SurfaceCapabilitiesKhr.MaxImageExtent"/>, inclusive, returned by <see
        /// cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/> for the surface.
        /// </para>
        /// </summary>
        public Extent2D ImageExtent;
        /// <summary>
        /// The number of views in a multiview/stereo surface. For non-stereoscopic-3D applications,
        /// this value is 1.
        /// <para>
        /// Must be greater than 0 and less than or equal to the <see
        /// cref="SurfaceCapabilitiesKhr.MaxImageArrayLayers"/> returned by <see
        /// cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/> for the surface.
        /// </para>
        /// </summary>
        public int ImageArrayLayers;
        /// <summary>
        /// A bitmask indicating how the application will use the swapchain’s presentable images.
        /// <para>
        /// Must be a subset of the supported usage flags present in the <see
        /// cref="SurfaceCapabilitiesKhr.SupportedUsageFlags"/> returned by <see
        /// cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/> for the surface. If <see
        /// cref="ImageSharingMode"/> is <see cref="SharingMode.Concurrent"/>, <see
        /// cref="QueueFamilyIndices"/> must be an array of at least 2 indices.
        /// </para>
        /// </summary>
        public ImageUsages ImageUsage;
        /// <summary>
        /// The sharing mode used for the images of the swapchain.
        /// </summary>
        public SharingMode ImageSharingMode;
        /// <summary>
        /// Queue family indices having access to the images of the swapchain in case <see
        /// cref="ImageSharingMode"/> is <see cref="SharingMode.Concurrent"/>.
        /// </summary>
        public int[] QueueFamilyIndices;
        /// <summary>
        /// A bitmask describing the transform, relative to the presentation engine’s natural
        /// orientation, applied to the image content prior to presentation. If it does not match the
        /// <see cref="SurfaceCapabilitiesKhr.CurrentTransform"/> value returned by <see
        /// cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/>, the presentation engine will
        /// transform the image content as part of the presentation operation.
        /// <para>
        /// Must be one of the bits present in the <see
        /// cref="SurfaceCapabilitiesKhr.SupportedTransforms"/> member the structure returned by <see
        /// cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/> for the surface.
        /// </para>
        /// </summary>
        public SurfaceTransformsKhr PreTransform;
        /// <summary>
        /// A bitmask indicating the alpha compositing mode to use when this surface is composited
        /// together with other surfaces on certain window systems.
        /// <para>
        /// Must be one of the bits present in the <see
        /// cref="SurfaceCapabilitiesKhr.SupportedCompositeAlpha"/> returned by <see
        /// cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/> for the surface.
        /// </para>
        /// </summary>
        public CompositeAlphasKhr CompositeAlpha;
        /// <summary>
        /// The presentation mode the swapchain will use. A swapchain’s present mode determines how
        /// incoming present requests will be processed and queued internally.
        /// <para>
        /// Must be one of the values returned by <see
        /// cref="PhysicalDeviceExtensions.GetSurfacePresentModesKhr"/> for the surface.
        /// </para>
        /// </summary>
        public PresentModeKhr PresentMode;
        /// <summary>
        /// Indicates whether the Vulkan implementation is allowed to discard rendering operations
        /// that affect regions of the surface which are not visible.
        /// <para>
        /// If set to <c>true</c>, the presentable images associated with the swapchain may not own
        /// all of their pixels. Pixels in the presentable images that correspond to regions of the
        /// target surface obscured by another window on the desktop or subject to some other
        /// clipping mechanism will have undefined content when read back. Pixel shaders may not
        /// execute for these pixels, and thus any side affects they would have had will not occur.
        /// </para>
        /// <para>
        /// If set to <c>false</c>, presentable images associated with the swapchain will own all the
        /// pixels they contain. Setting this value to <c>true</c> does not guarantee any clipping
        /// will occur, but allows more optimal presentation methods to be used on some platforms.
        /// </para>
        /// </summary>
        public Bool Clipped;
        /// <summary>
        /// Existing swapchain to replace, if any.
        /// </summary>
        public SwapchainKhr OldSwapchain;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public SwapchainCreateFlagsKhr Flags;
            public long Surface;
            public int MinImageCount;
            public Format ImageFormat;
            public ColorSpaceKhr ImageColorSpace;
            public Extent2D ImageExtent;
            public int ImageArrayLayers;
            public ImageUsages ImageUsage;
            public SharingMode ImageSharingMode;
            public int QueueFamilyIndexCount;
            public int* QueueFamilyIndices;
            public SurfaceTransformsKhr PreTransform;
            public CompositeAlphasKhr CompositeAlpha;
            public PresentModeKhr PresentMode;
            public Bool Clipped;
            public long OldSwapchain;
        }

        internal void ToNative(out Native native, int* queueFamilyIndices)
        {
            native.Type = StructureType.SwapchainCreateInfoKhr;
            native.Next = IntPtr.Zero;
            native.Flags = 0;
            native.Surface = Surface;
            native.MinImageCount = MinImageCount;
            native.ImageFormat = ImageFormat;
            native.ImageColorSpace = ImageColorSpace;
            native.ImageExtent = ImageExtent;
            native.ImageArrayLayers = ImageArrayLayers;
            native.ImageUsage = ImageUsage;
            native.ImageSharingMode = ImageSharingMode;
            native.QueueFamilyIndexCount = QueueFamilyIndices?.Length ?? 0;
            native.QueueFamilyIndices = queueFamilyIndices;
            native.PreTransform = PreTransform;
            native.CompositeAlpha = CompositeAlpha;
            native.PresentMode = PresentMode;
            native.Clipped = Clipped;
            native.OldSwapchain = OldSwapchain;
        }
    }

    // Reserved for future use.
    internal enum SwapchainCreateFlagsKhr
    {
        None = 0
    }

    /// <summary>
    /// Supported color space of the presentation engine.
    /// </summary>
    public enum ColorSpaceKhr
    {
        /// <summary>
        /// The presentation engine supports the sRGB color space.
        /// </summary>
        SRgbNonlinear = 0,
        DisplayP3Linear = 1000104001,
        DisplayP3Nonlinear = 1000104002,
        /// <summary>
        /// Supports the scRGB color space and applies a linear OETF.
        /// </summary>
        ScRgbLinear = 1000104003,
        /// <summary>
        /// Supports the scRGB color space and applies the scRGB OETF.
        /// </summary>
        ScRgbNonlinear = 1000104004,
        /// <summary>
        /// Supports the DCI-P3 color space and applies a linear OETF.
        /// </summary>
        DciP3Linear = 1000104005,
        /// <summary>
        /// Supports the DCI-P3 color space and applies the Gamma 2.6 OETF.
        /// </summary>
        DciP3Nonlinear = 1000104006,
        /// <summary>
        /// Supports the BT709 color space and applies a linear OETF.
        /// </summary>
        BT709Linear = 1000104007,
        /// <summary>
        /// Supports the BT709 color space and applies the SMPTE 170M OETF.
        /// </summary>
        BT709Nonlinear = 1000104008,
        /// <summary>
        /// Supports the BT2020 color space and applies a linear OETF.
        /// </summary>
        BT2020Linear = 1000104009,
        /// <summary>
        /// Supports the BT2020 color space and applies the SMPTE 170M OETF.
        /// </summary>
        BT2020Nonlinear = 1000104010,
        /// <summary>
        /// Supports the AdobeRGB color space and applies a linear OETF.
        /// </summary>
        AdobeRgbLinear = 1000104011,
        /// <summary>
        /// Supports the AdobeRGB color space and applies the Gamma 2.2 OETF.
        /// </summary>
        AdobeRgbNonlinear = 1000104012,
    }

    /// <summary>
    /// Presentation transforms supported on a device.
    /// </summary>
    [Flags]
    public enum SurfaceTransformsKhr
    {
        /// <summary>
        /// The image content is presented without being transformed.
        /// </summary>
        Identity = 1 << 0,
        /// <summary>
        /// The image content is rotated 90 degrees clockwise.
        /// </summary>
        Rotate90 = 1 << 1,
        /// <summary>
        /// The image content is rotated 180 degrees clockwise.
        /// </summary>
        Rotate180 = 1 << 2,
        /// <summary>
        /// The image content is rotated 270 degrees clockwise.
        /// </summary>
        Rotate270 = 1 << 3,
        /// <summary>
        /// The image content is mirrored horizontally.
        /// </summary>
        HorizontalMirror = 1 << 4,
        /// <summary>
        /// The image content is mirrored horizontally, then rotated 90 degrees clockwise.
        /// </summary>
        HorizontalMirrorRotate90 = 1 << 5,
        /// <summary>
        /// The image content is mirrored horizontally, then rotated 180 degrees clockwise.
        /// </summary>
        HorizontalMirrorRotate180 = 1 << 6,
        /// <summary>
        /// The image content is mirrored horizontally, then rotated 270 degrees clockwise.
        /// </summary>
        HorizontalMirrorRotate270 = 1 << 7,
        /// <summary>
        /// The presentation transform is not specified, and is instead determined by
        /// platform-specific considerations and mechanisms outside Vulkan.
        /// </summary>
        Inherit = 1 << 8
    }

    /// <summary>
    /// Alpha compositing modes supported on a device.
    /// </summary>
    [Flags]
    public enum CompositeAlphasKhr
    {
        /// <summary>
        /// The alpha channel, if it exists, of the images is ignored in the compositing process.
        /// Instead, the image is treated as if it has a constant alpha of 1.0.
        /// </summary>
        Opaque = 1 << 0,
        /// <summary>
        /// The alpha channel, if it exists, of the images is respected in the compositing process.
        /// The non-alpha channels of the image are expected to already be multiplied by the alpha
        /// channel by the application.
        /// </summary>
        PreMultiplied = 1 << 1,
        /// <summary>
        /// The alpha channel, if it exists, of the images is respected in the compositing process.
        /// The non-alpha channels of the image are not expected to already be multiplied by the
        /// alpha channel by the application; instead, the compositor will multiply the non-alpha
        /// channels of the image by the alpha channel during compositing.
        /// </summary>
        PostMultiplied = 1 << 2,
        /// <summary>
        /// The way in which the presentation engine treats the alpha channel in the images is
        /// unknown to the Vulkan API. Instead, the application is responsible for setting the
        /// composite alpha blending mode using native window system commands. If the application
        /// does not set the blending mode using native window system commands, then a
        /// platform-specific default will be used.
        /// </summary>
        Inherit = 1 << 3
    }

    /// <summary>
    /// Presentation mode supported for a surface.
    /// </summary>
    public enum PresentModeKhr
    {
        /// <summary>
        /// The presentation engine does not wait for a vertical blanking period to update the
        /// current image, meaning this mode may result in visible tearing. No internal queuing of
        /// presentation requests is needed, as the requests are applied immediately.
        /// </summary>
        Immediate = 0,
        /// <summary>
        /// The presentation engine waits for the next vertical blanking period to update the current
        /// image. Tearing cannot be observed. An internal single-entry queue is used to hold pending
        /// presentation requests. If the queue is full when a new presentation request is received,
        /// the new request replaces the existing entry, and any images associated with the prior
        /// entry become available for re-use by the application. One request is removed from the
        /// queue and processed during each vertical blanking period in which the queue is non-empty.
        /// </summary>
        Mailbox = 1,
        /// <summary>
        /// The presentation engine waits for the next vertical blanking period to update the current
        /// image. Tearing cannot be observed. An internal queue is used to hold pending presentation
        /// requests. New requests are appended to the end of the queue, and one request is removed
        /// from the beginning of the queue and processed during each vertical blanking period in
        /// which the queue is non-empty. This is the only value of presentMode that is required: to
        /// be supported.
        /// </summary>
        Fifo = 2,
        /// <summary>
        /// The presentation engine generally waits for the next vertical blanking period to update
        /// the current image. If a vertical blanking period has already passed since the last update
        /// of the current image then the presentation engine does not wait for another vertical
        /// blanking period for the update, meaning this mode
        /// may: result in visible tearing in this case. This mode is useful for reducing visual
        /// stutter with an application that will mostly present a new image before the next vertical
        /// blanking period, but may occasionally be late, and present a new image just after the
        /// next vertical blanking period. An internal queue is used to hold pending presentation
        /// requests. New requests are appended to the end of the queue, and one request is removed
        /// from the beginning of the queue and processed during or after each vertical blanking
        /// period in which the queue is non-empty.
        /// </summary>
        FifoRelaxed = 3
    }
}
