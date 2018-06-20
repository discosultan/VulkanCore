using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Khr
{
    /// <summary>
    /// Opaque handle to a swapchain object.
    /// <para>
    /// A swapchain object (a.k.a. swapchain) provides the ability to present rendering results to a surface.
    /// </para>
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

            long handle;
            createInfo.ToNative(out SwapchainCreateInfoKhr.Native nativeCreateInfo);
            Result result = vkCreateSwapchainKHR(Parent, &nativeCreateInfo, NativeAllocator, &handle);
            nativeCreateInfo.Free();
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
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
            Result result = vkGetSwapchainImagesKHR(Parent, this, &swapchainImageCount, null);
            VulkanException.ThrowForInvalidResult(result);

            var swapchainImages = stackalloc long[swapchainImageCount];
            result = vkGetSwapchainImagesKHR(Parent, this, &swapchainImageCount, swapchainImages);
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
            Result result = vkAcquireNextImageKHR(Parent, this, timeout, semaphore, fence, &nextImageIndex);
            VulkanException.ThrowForInvalidResult(result);
            return nextImageIndex;
        }

        /// <summary>
        /// Get a swapchain's status. The possible return values should be interpreted as follows:
        /// <para>
        /// * <see cref="Result.Success"/> - Indicates the presentation engine is presenting the
        ///   contents of the shared presentable image, as per the swapchain's <see cref="PresentModeKhr"/>
        /// </para>
        /// <para>
        /// * <see cref="Result.SuboptimalKhr"/> - The swapchain no longer matches the surface
        ///   properties exactly, but the presentation engine is presenting the contents of the
        ///   shared presentable image, as per the swapchain's <see cref="PresentModeKhr"/>
        /// </para>
        /// <para>
        /// * <see cref="Result.ErrorOutOfDateKhr"/> - The surface has changed in such a way that it
        ///   is no longer compatible with the swapchain
        /// </para>
        /// <para>* <see cref="Result.ErrorSurfaceLostKhr"/> - The surface is no longer available</para>
        /// </summary>
        /// <returns>Status of the swapchain.</returns>
        public Result GetStatus()
        {
            return vkGetSwapchainStatusKHR(this)(Parent, this);
        }

        internal static SwapchainKhr[] CreateSharedKhr(Device parent, SwapchainCreateInfoKhr[] createInfos,
            ref AllocationCallbacks? allocator)
        {
            int count = createInfos?.Length ?? 0;
            var nativeCreateInfos = stackalloc SwapchainCreateInfoKhr.Native[count];
            for (int i = 0; i < count; i++)
                createInfos[i].ToNative(out nativeCreateInfos[i]);

            AllocationCallbacks.Native nativeAllocator;
            allocator?.ToNative(&nativeAllocator);

            long* handles = stackalloc long[count];
            Result result = vkCreateSharedSwapchainsKHR(parent, count, nativeCreateInfos,
                allocator.HasValue ? &nativeAllocator : null, handles);
            for (int i = 0; i < count; i++)
                nativeCreateInfos[i].Free();
            VulkanException.ThrowForInvalidResult(result);

            var swapchains = new SwapchainKhr[count];
            for (int i = 0; i < count; i++)
                swapchains[i] = new SwapchainKhr(parent, handles[i], ref allocator);
            return swapchains;
        }

        /// <summary>
        /// Destroy a swapchain object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroySwapchainKHR(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateSwapchainKHRDelegate(IntPtr device, SwapchainCreateInfoKhr.Native* createInfo, AllocationCallbacks.Native* allocator, long* swapchain);
        private static readonly vkCreateSwapchainKHRDelegate vkCreateSwapchainKHR = VulkanLibrary.GetStaticProc<vkCreateSwapchainKHRDelegate>(nameof(vkCreateSwapchainKHR));

        private delegate void vkDestroySwapchainKHRDelegate(IntPtr device, long swapchain, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroySwapchainKHRDelegate vkDestroySwapchainKHR = VulkanLibrary.GetStaticProc<vkDestroySwapchainKHRDelegate>(nameof(vkDestroySwapchainKHR));

        private delegate Result vkGetSwapchainImagesKHRDelegate(IntPtr device, long swapchain, int* swapchainImageCount, long* swapchainImages);
        private static readonly vkGetSwapchainImagesKHRDelegate vkGetSwapchainImagesKHR = VulkanLibrary.GetStaticProc<vkGetSwapchainImagesKHRDelegate>(nameof(vkGetSwapchainImagesKHR));

        private delegate Result vkAcquireNextImageKHRDelegate(IntPtr device, long swapchain, long timeout, long semaphore, long fence, int* imageIndex);
        private static readonly vkAcquireNextImageKHRDelegate vkAcquireNextImageKHR = VulkanLibrary.GetStaticProc<vkAcquireNextImageKHRDelegate>(nameof(vkAcquireNextImageKHR));

        private delegate Result vkCreateSharedSwapchainsKHRDelegate(IntPtr device, int swapchainCount, SwapchainCreateInfoKhr.Native* createInfos, AllocationCallbacks.Native* allocator, long* swapchains);
        private static readonly vkCreateSharedSwapchainsKHRDelegate vkCreateSharedSwapchainsKHR = VulkanLibrary.GetStaticProc<vkCreateSharedSwapchainsKHRDelegate>(nameof(vkCreateSharedSwapchainsKHR));

        private delegate Result vkGetSwapchainStatusKHRDelegate(IntPtr device, long swapchain);
        private static vkGetSwapchainStatusKHRDelegate vkGetSwapchainStatusKHR(SwapchainKhr swapchain) => swapchain.Parent.GetProc<vkGetSwapchainStatusKHRDelegate>(nameof(vkGetSwapchainStatusKHR));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created swapchain object.
    /// </summary>
    public struct SwapchainCreateInfoKhr
    {
        /// <summary>
        /// A bitmask indicating parameters of the swapchain creation.
        /// </summary>
        public SwapchainCreateFlagsKhr Flags;
        /// <summary>
        /// The <see cref="SurfaceKhr"/> onto which the swapchain will present images. If the
        /// creation succeeds, the swapchain becomes associated with <see cref="SurfaceKhr"/>.
        /// </summary>
        public long Surface;
        /// <summary>
        /// The minimum number of presentable images that the application needs.
        /// <para>
        /// The implementation will either create the swapchain with at least that many images, or it
        /// will fail to create the swapchain.
        /// </para>
        /// </summary>
        public int MinImageCount;
        /// <summary>
        /// A format value specifying the format the swapchain image(s) will be created with.
        /// </summary>
        public Format ImageFormat;
        /// <summary>
        /// Color space value specifying the way the swapchain interprets image data.
        /// </summary>
        public ColorSpaceKhr ImageColorSpace;
        /// <summary>
        /// The size (in pixels) of the swapchain image(s).
        /// <para>
        /// The behavior is platform-dependent if the image extent does not match the surface's <see
        /// cref="SurfaceCapabilitiesKhr.CurrentExtent"/> as returned by <see cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/>.
        /// </para>
        /// </summary>
        public Extent2D ImageExtent;
        /// <summary>
        /// The number of views in a multiview/stereo surface.
        /// <para>For non-stereoscopic-3D applications, this value is 1.</para>
        /// </summary>
        public int ImageArrayLayers;
        /// <summary>
        /// A bitmask describing the intended usage of the (acquired) swapchain images.
        /// </summary>
        public ImageUsages ImageUsage;
        /// <summary>
        /// The sharing mode used for the image(s) of the swapchain.
        /// </summary>
        public SharingMode ImageSharingMode;
        /// <summary>
        /// Queue family indices having access to the image(s) of the swapchain when <see
        /// cref="ImageSharingMode"/> is <see cref="SharingMode.Concurrent"/>.
        /// </summary>
        public int[] QueueFamilyIndices;
        /// <summary>
        /// A value describing the transform, relative to the presentation engine's natural
        /// orientation, applied to the image content prior to presentation.
        /// <para>
        /// If it does not match the <see cref="SurfaceCapabilitiesKhr.CurrentTransform"/> value
        /// returned by <see cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/>, the
        /// presentation engine will transform the image content as part of the presentation operation.
        /// </para>
        /// </summary>
        public SurfaceTransformsKhr PreTransform;
        /// <summary>
        /// A value indicating the alpha compositing mode to use when this surface is composited
        /// together with other surfaces on certain window systems.
        /// </summary>
        public CompositeAlphasKhr CompositeAlpha;
        /// <summary>
        /// The presentation mode the swapchain will use.
        /// <para>
        /// A swapchain's present mode determines how incoming present requests will be processed and
        /// queued internally.
        /// </para>
        /// </summary>
        public PresentModeKhr PresentMode;
        /// <summary>
        /// Indicates whether the Vulkan implementation is allowed to discard rendering operations
        /// that affect regions of the surface that are not visible.
        /// <para>
        /// If set to <c>true</c>, the presentable images associated with the swapchain may not own
        /// all of their pixels. Pixels in the presentable images that correspond to regions of the
        /// target surface obscured by another window on the desktop, or subject to some other
        /// clipping mechanism will have undefined content when read back. Pixel shaders may not
        /// execute for these pixels, and thus any side effects they would have had will not occur.
        /// </para>
        /// <para>
        /// <c>true</c> value does not guarantee any clipping will occur, but allows more optimal
        /// presentation methods to be used on some platforms.
        /// </para>
        /// <para>
        /// If set to <c>false</c>, presentable images associated with the swapchain will own all of
        /// the pixels they contain.
        /// </para>
        /// </summary>
        public Bool Clipped;
        /// <summary>
        /// Is <c>null</c>, or the existing non-retired swapchain currently associated with <c>Surface</c>.
        /// <para>
        /// Providing a valid <see cref="OldSwapchain"/> may aid in the resource reuse, and also
        /// allows the application to still present any images that are already acquired from it.
        /// </para>
        /// </summary>
        public SwapchainKhr OldSwapchain;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwapchainCreateInfoKhr"/> structure.
        /// </summary>
        /// <param name="surface">
        /// The <see cref="SurfaceKhr"/> that the swapchain will present images to.
        /// </param>
        /// <param name="imageFormat">A format that is valid for swapchains on the specified surface.</param>
        /// <param name="imageExtent">
        /// The size (in pixels) of the swapchain.
        /// <para>
        /// Behavior is platform-dependent when the image extent does not match the surface's <see
        /// cref="SurfaceCapabilitiesKhr.CurrentExtent"/> as returned by <see cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/>.
        /// </para>
        /// </param>
        /// <param name="minImageCount">
        /// The minimum number of presentable images that the application needs. The platform will
        /// either create the swapchain with at least that many images, or will fail to create the swapchain.
        /// </param>
        /// <param name="imageColorSpace">Color space value specifying the way the swapchain interprets image data.</param>
        /// <param name="imageArrayLayers">
        /// The number of views in a multiview/stereo surface.
        /// <para>For non-stereoscopic-3D applications, this value is 1.</para>
        /// </param>
        /// <param name="imageUsage">A bitmask describing the intended usage of the (acquired) swapchain images.</param>
        /// <param name="imageSharingMode">The sharing mode used for the image(s) of the swapchain.</param>
        /// <param name="queueFamilyIndices">
        /// Queue family indices having access to the image(s) of the swapchain when <see
        /// cref="ImageSharingMode"/> is <see cref="SharingMode.Concurrent"/>.
        /// </param>
        /// <param name="preTransform">
        /// A value describing the transform, relative to the presentation engine's natural
        /// orientation, applied to the image content prior to presentation.
        /// <para>
        /// If it does not match the <see cref="SurfaceCapabilitiesKhr.CurrentTransform"/> value
        /// returned by <see cref="PhysicalDeviceExtensions.GetSurfaceCapabilitiesKhr"/>, the
        /// presentation engine will transform the image content as part of the presentation operation.
        /// </para>
        /// </param>
        /// <param name="compositeAlpha">
        /// A bitmask indicating the alpha compositing mode to use when this surface is composited
        /// together with other surfaces on certain window systems.
        /// </param>
        /// <param name="presentMode">
        /// The presentation mode the swapchain will use.
        /// <para>
        /// A swapchain's present mode determines how incoming present requests will be processed and
        /// queued internally.
        /// </para>
        /// </param>
        /// <param name="clipped">
        /// Indicates whether the Vulkan implementation is allowed to discard rendering operations
        /// that affect regions of the surface which are not visible.</param>
        /// <param name="oldSwapchain">Existing swapchain to replace, if any.</param>
        public SwapchainCreateInfoKhr(
            SurfaceKhr surface,
            Format imageFormat,
            Extent2D imageExtent,
            int minImageCount = 2,
            ColorSpaceKhr imageColorSpace = ColorSpaceKhr.SRgbNonlinear,
            int imageArrayLayers = 1,
            ImageUsages imageUsage = ImageUsages.ColorAttachment | ImageUsages.TransferDst,
            SharingMode imageSharingMode = SharingMode.Exclusive,
            int[] queueFamilyIndices = null,
            SurfaceTransformsKhr preTransform = SurfaceTransformsKhr.Identity,
            CompositeAlphasKhr compositeAlpha = CompositeAlphasKhr.Opaque,
            PresentModeKhr presentMode = PresentModeKhr.Fifo,
            bool clipped = true,
            SwapchainKhr oldSwapchain = null)
        {
            Flags = SwapchainCreateFlagsKhr.None;
            Surface = surface;
            MinImageCount = minImageCount;
            ImageFormat = imageFormat;
            ImageColorSpace = imageColorSpace;
            ImageExtent = imageExtent;
            ImageArrayLayers = imageArrayLayers;
            ImageUsage = imageUsage;
            ImageSharingMode = imageSharingMode;
            QueueFamilyIndices = queueFamilyIndices;
            PreTransform = preTransform;
            CompositeAlpha = compositeAlpha;
            PresentMode = presentMode;
            Clipped = clipped;
            OldSwapchain = oldSwapchain;
        }

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
            public IntPtr QueueFamilyIndices;
            public SurfaceTransformsKhr PreTransform;
            public CompositeAlphasKhr CompositeAlpha;
            public PresentModeKhr PresentMode;
            public Bool Clipped;
            public long OldSwapchain;

            public void Free()
            {
                Interop.Free(QueueFamilyIndices);
            }
        }

        internal void ToNative(out Native native)
        {
            native.Type = StructureType.SwapchainCreateInfoKhr;
            native.Next = IntPtr.Zero;
            native.Flags = Flags;
            native.Surface = Surface;
            native.MinImageCount = MinImageCount;
            native.ImageFormat = ImageFormat;
            native.ImageColorSpace = ImageColorSpace;
            native.ImageExtent = ImageExtent;
            native.ImageArrayLayers = ImageArrayLayers;
            native.ImageUsage = ImageUsage;
            native.ImageSharingMode = ImageSharingMode;
            native.QueueFamilyIndexCount = QueueFamilyIndices?.Length ?? 0;
            native.QueueFamilyIndices = Interop.Struct.AllocToPointer(QueueFamilyIndices);
            native.PreTransform = PreTransform;
            native.CompositeAlpha = CompositeAlpha;
            native.PresentMode = PresentMode;
            native.Clipped = Clipped;
            native.OldSwapchain = OldSwapchain;
        }
    }

    /// <summary>
    /// Bitmask controlling swapchain creation.
    /// </summary>
    [Flags]
    public enum SwapchainCreateFlagsKhr
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0
    }

    /// <summary>
    /// Supported color space of the presentation engine.
    /// </summary>
    public enum ColorSpaceKhr
    {
        /// <summary>
        /// Indicates support for the sRGB color space.
        /// </summary>
        SRgbNonlinear = 0,
        /// <summary>
        /// Indicates support for the Display-P3 color space and applies an sRGB-like transfer function.
        /// </summary>
        DisplayP3NonlinearExt = 1000104001,
        /// <summary>
        /// Indicates support for the extended sRGB color space and applies a linear transfer function.
        /// </summary>
        ExtendedSRgbLinearExt = 1000104002,
        /// <summary>
        /// Indicates support for the DCI-P3 color space and applies a linear OETF.
        /// </summary>
        DciP3LinearExt = 1000104003,
        /// <summary>
        /// Indicates support for the DCI-P3 color space and applies the Gamma 2.6 OETF.
        /// </summary>
        DciP3NonlinearExt = 1000104004,
        /// <summary>
        /// Indicates support for the BT709 color space and applies a linear OETF.
        /// </summary>
        BT709LinearExt = 1000104005,
        /// <summary>
        /// Indicates support for the BT709 color space and applies the SMPTE 170M OETF.
        /// </summary>
        BT709NonlinearExt = 1000104006,
        /// <summary>
        /// Indicates support for the BT2020 color space and applies a linear OETF.
        /// </summary>
        BT2020LinearExt = 1000104007,
        /// <summary>
        /// Indicates support for HDR10 (BT2020 color) space and applies the SMPTE ST2084 Perceptual
        /// Quantizer (PQ) OETF.
        /// </summary>
        Hdr10ST2084Ext = 1000104008,
        /// <summary>
        /// Indicates support for Dolby Vision (BT2020 color space), proprietary encoding, and
        /// applies the SMPTE ST2084 OETF.
        /// </summary>
        DolbyVisionExt = 1000104009,
        /// <summary>
        /// Indicates support for HDR10 (BT2020 color space) and applies the Hybrid Log Gamma (HLG) OETF.
        /// </summary>
        Hdr10HlgExt = 1000104010,
        /// <summary>
        /// Indicates support for the AdobeRGB color space and applies a linear OETF.
        /// </summary>
        AdobeRgbLinearExt = 1000104011,
        /// <summary>
        /// Indicates support for the AdobeRGB color space and applies the Gamma 2.2 OETF.
        /// </summary>
        AdobeRgbNonlinearExt = 1000104012,
        /// <summary>
        /// Indicates that color components are used "as is". This is intended to allow application
        /// to supply data for color spaces not described here.
        /// </summary>
        PassThroughExt = 1000104013,
        /// <summary>
        /// Indicates support for the extended sRGB color space and applies an sRGB transfer function.
        /// </summary>
        ExtendedSRgbNonlinearExt = 1000104014
    }

    /// <summary>
    /// Presentation transforms supported on a device.
    /// </summary>
    [Flags]
    public enum SurfaceTransformsKhr
    {
        /// <summary>
        /// Indicates that image content is presented without being transformed.
        /// </summary>
        Identity = 1 << 0,
        /// <summary>
        /// Indicates that image content is rotated 90 degrees clockwise.
        /// </summary>
        Rotate90 = 1 << 1,
        /// <summary>
        /// Indicates that image content is rotated 180 degrees clockwise.
        /// </summary>
        Rotate180 = 1 << 2,
        /// <summary>
        /// Indicates that image content is rotated 270 degrees clockwise.
        /// </summary>
        Rotate270 = 1 << 3,
        /// <summary>
        /// Indicates that image content is mirrored horizontally.
        /// </summary>
        HorizontalMirror = 1 << 4,
        /// <summary>
        /// Indicates that image content is mirrored horizontally, then rotated 90 degrees clockwise.
        /// </summary>
        HorizontalMirrorRotate90 = 1 << 5,
        /// <summary>
        /// Indicates that image content is mirrored horizontally, then rotated 180 degrees clockwise.
        /// </summary>
        HorizontalMirrorRotate180 = 1 << 6,
        /// <summary>
        /// Indicates that image content is mirrored horizontally, then rotated 270 degrees clockwise.
        /// </summary>
        HorizontalMirrorRotate270 = 1 << 7,
        /// <summary>
        /// Indicates that presentation transform is not specified, and is instead determined by
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
        /// Indicates that the presentation engine does not wait for a vertical blanking period to
        /// update the current image, meaning this mode may result in visible tearing. No internal
        /// queuing of presentation requests is needed, as the requests are applied immediately.
        /// </summary>
        Immediate = 0,
        /// <summary>
        /// Indicates that the presentation engine waits for the next vertical blanking period to
        /// update the current image. Tearing cannot be observed. An internal single-entry queue is
        /// used to hold pending presentation requests. If the queue is full when a new presentation
        /// request is received, the new request replaces the existing entry, and any images
        /// associated with the prior entry become available for re-use by the application. One
        /// request is removed from the queue and processed during each vertical blanking period in
        /// which the queue is non-empty.
        /// </summary>
        Mailbox = 1,
        /// <summary>
        /// Indicates that the presentation engine waits for the next vertical blanking period to
        /// update the current image. Tearing cannot be observed. An internal queue is used to hold
        /// pending presentation requests. New requests are appended to the end of the queue, and one
        /// request is removed from the beginning of the queue and processed during each vertical
        /// blanking period in which the queue is non-empty. This is the only value of presentMode
        /// that is required: to be supported.
        /// </summary>
        Fifo = 2,
        /// <summary>
        /// Indicates that the presentation engine generally waits for the next vertical blanking
        /// period to update the current image. If a vertical blanking period has already passed
        /// since the last update of the current image then the presentation engine does not wait for
        /// another vertical blanking period for the update, meaning this mode may result in visible
        /// tearing in this case. This mode is useful for reducing visual stutter with an application
        /// that will mostly present a new image before the next vertical blanking period, but may
        /// occasionally be late, and present a new image just after the next vertical blanking
        /// period. An internal queue is used to hold pending presentation requests. New requests are
        /// appended to the end of the queue, and one request is removed from the beginning of the
        /// queue and processed during or after each vertical blanking period in which the queue is non-empty.
        /// </summary>
        FifoRelaxed = 3,
        /// <summary>
        /// Indicates that the presentation engine and application have concurrent access to a single
        /// image, which is referred to as a shared presentable image. The presentation engine is
        /// only required to update the current image after a new presentation request is received.
        /// Therefore the application must make a presentation request whenever an update is
        /// required. However, the presentation engine may update the current image at any point,
        /// meaning this mode may result in visible tearing.
        /// </summary>
        SharedDemandRefreshKhr = 1000111000,
        /// <summary>
        /// Indicates that the presentation engine and application have concurrent access to a single
        /// image, which is referred to as a shared presentable image. The presentation engine
        /// periodically updates the current image on its regular refresh cycle. The application is
        /// only required to make one initial presentation request, after which the presentation
        /// engine must update the current image without any need for further presentation requests.
        /// The application can indicate the image contents have been updated by making a
        /// presentation request, but this does not guarantee the timing of when it will be updated.
        /// This mode may result in visible tearing if rendering to the image is not timed correctly.
        /// </summary>
        SharedContinuousRefreshKhr = 1000111001
    }
}
