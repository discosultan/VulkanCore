using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore.Khr
{
    /// <summary>
    /// A <see cref="SurfaceKhr"/> object abstracts a native platform surface or window object for
    /// use with Vulkan.
    /// <para>
    /// The <see cref="InstanceExtension.KhrSurface"/> extension declares the <see
    /// cref="SurfaceKhr"/> object, and provides a function for destroying <see cref="SurfaceKhr"/>
    /// objects. Separate platform-specific extensions each provide a function for creating a <see
    /// cref="SurfaceKhr"/> object for the respective platform. From the application’s perspective
    /// this is an opaque handle, just like the handles of other Vulkan objects.
    /// </para>
    /// </summary>
    public unsafe class SurfaceKhr : DisposableHandle<long>
    {
        internal SurfaceKhr(Instance parent, AndroidSurfaceCreateInfoKhr* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            long handle;
            createInfo->Prepare();
            Result result = vkCreateAndroidSurfaceKHR(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        internal SurfaceKhr(Instance parent, MirSurfaceCreateInfoKhr* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            long handle;
            createInfo->Prepare();
            Result result = vkCreateMirSurfaceKHR(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        internal SurfaceKhr(Instance parent, WaylandSurfaceCreateInfoKhr* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            long handle;
            createInfo->Prepare();
            Result result = vkCreateWaylandSurfaceKHR(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        internal SurfaceKhr(Instance parent, Win32SurfaceCreateInfoKhr* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            long handle;
            createInfo->Prepare();
            Result result = vkCreateWin32SurfaceKHR(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        internal SurfaceKhr(Instance parent, XlibSurfaceCreateInfoKhr* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            long handle;
            createInfo->Prepare();
            Result result = vkCreateXlibSurfaceKHR(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        internal SurfaceKhr(Instance parent, DisplaySurfaceCreateInfoKhr* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            long handle;
            createInfo->Prepare();
            Result result = vkCreateDisplayPlaneSurfaceKHR(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        internal SurfaceKhr(Instance parent, XcbSurfaceCreateInfoKhr* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            long handle;
            createInfo->Prepare();
            Result result = vkCreateXcbSurfaceKHR(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        internal SurfaceKhr(Instance parent, ref AllocationCallbacks? allocator, long handle)
        {
            Parent = parent;
            Allocator = allocator;
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of this resource.
        /// </summary>
        public Instance Parent { get; }

        /// <summary>
        /// Destroy a surface object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroySurfaceKHR(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateAndroidSurfaceKHRDelegate(IntPtr instance, AndroidSurfaceCreateInfoKhr* createInfo, AllocationCallbacks.Native* allocator, long* surface);
        private static readonly vkCreateAndroidSurfaceKHRDelegate vkCreateAndroidSurfaceKHR = VulkanLibrary.GetStaticProc<vkCreateAndroidSurfaceKHRDelegate>(nameof(vkCreateAndroidSurfaceKHR));

        private delegate Result vkCreateDisplayPlaneSurfaceKHRDelegate(IntPtr instance, DisplaySurfaceCreateInfoKhr* createInfo, AllocationCallbacks.Native* allocator, long* surface);
        private static readonly vkCreateDisplayPlaneSurfaceKHRDelegate vkCreateDisplayPlaneSurfaceKHR = VulkanLibrary.GetStaticProc<vkCreateDisplayPlaneSurfaceKHRDelegate>(nameof(vkCreateDisplayPlaneSurfaceKHR));

        private delegate Result vkCreateMirSurfaceKHRDelegate(IntPtr instance, MirSurfaceCreateInfoKhr* createInfo, AllocationCallbacks.Native* allocator, long* surface);
        private static readonly vkCreateMirSurfaceKHRDelegate vkCreateMirSurfaceKHR = VulkanLibrary.GetStaticProc<vkCreateMirSurfaceKHRDelegate>(nameof(vkCreateMirSurfaceKHR));

        private delegate Result vkCreateWaylandSurfaceKHRDelegate(IntPtr instance, WaylandSurfaceCreateInfoKhr* createInfo, AllocationCallbacks.Native* allocator, long* surface);
        private static readonly vkCreateWaylandSurfaceKHRDelegate vkCreateWaylandSurfaceKHR = VulkanLibrary.GetStaticProc<vkCreateWaylandSurfaceKHRDelegate>(nameof(vkCreateWaylandSurfaceKHR));

        private delegate Result vkCreateWin32SurfaceKHRDelegate(IntPtr instance, Win32SurfaceCreateInfoKhr* createInfo, AllocationCallbacks.Native* allocator, long* surface);
        private static readonly vkCreateWin32SurfaceKHRDelegate vkCreateWin32SurfaceKHR = VulkanLibrary.GetStaticProc<vkCreateWin32SurfaceKHRDelegate>(nameof(vkCreateWin32SurfaceKHR));

        private delegate Result vkCreateXlibSurfaceKHRDelegate(IntPtr instance, XlibSurfaceCreateInfoKhr* createInfo, AllocationCallbacks.Native* allocator, long* surface);
        private static readonly vkCreateXlibSurfaceKHRDelegate vkCreateXlibSurfaceKHR = VulkanLibrary.GetStaticProc<vkCreateXlibSurfaceKHRDelegate>(nameof(vkCreateXlibSurfaceKHR));

        private delegate Result vkCreateXcbSurfaceKHRDelegate(IntPtr instance, XcbSurfaceCreateInfoKhr* createInfo, AllocationCallbacks.Native* allocator, long* surface);
        private static readonly vkCreateXcbSurfaceKHRDelegate vkCreateXcbSurfaceKHR = VulkanLibrary.GetStaticProc<vkCreateXcbSurfaceKHRDelegate>(nameof(vkCreateXcbSurfaceKHR));

        private delegate void vkDestroySurfaceKHRDelegate(IntPtr instance, long surface, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroySurfaceKHRDelegate vkDestroySurfaceKHR = VulkanLibrary.GetStaticProc<vkDestroySurfaceKHRDelegate>(nameof(vkDestroySurfaceKHR));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created Android surface object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AndroidSurfaceCreateInfoKhr
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal AndroidSurfaceCreateFlagsKhr Flags;

        /// <summary>
        /// Pointer to the Android ANativeWindow to associate the surface with.
        /// </summary>
        public IntPtr Window;

        /// <summary>
        /// Initializes a new instance of <see cref="AndroidSurfaceCreateInfoKhr"/> structure.
        /// </summary>
        /// <param name="window">
        /// Pointer to the Android ANativeWindow to associate the surface with.
        /// </param>
        public AndroidSurfaceCreateInfoKhr(IntPtr window)
        {
            Type = StructureType.AndroidSurfaceCreateInfoKhr;
            Next = IntPtr.Zero;
            Flags = 0;
            Window = window;
        }

        internal void Prepare()
        {
            Type = StructureType.AndroidSurfaceCreateInfoKhr;
        }
    }

    // Is reserved for future use.
    internal enum AndroidSurfaceCreateFlagsKhr
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying parameters of a newly created Mir surface object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MirSurfaceCreateInfoKhr
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal MirSurfaceCreateFlagsKhr Flags;

        /// <summary>
        /// Pointer to the MirConnection to associate the surface with.
        /// </summary>
        public IntPtr Connection;
        /// <summary>
        /// Pointer to the MirSurface for the window to associate the surface with.
        /// </summary>
        public IntPtr MirSurface;

        /// <summary>
        /// Initializes a new instance of <see cref="MirSurfaceCreateInfoKhr"/> structure.
        /// </summary>
        /// <param name="connection">Pointer to the MirConnection to associate the surface with.</param>
        /// <param name="mirSurface">
        /// Pointer to the MirSurface for the window to associate the surface with.
        /// </param>
        public MirSurfaceCreateInfoKhr(IntPtr connection, IntPtr mirSurface)
        {
            Connection = connection;
            MirSurface = mirSurface;
            Type = StructureType.MirSurfaceCreateInfoKhr;
            Next = IntPtr.Zero;
            Flags = 0;
        }

        internal void Prepare()
        {
            Type = StructureType.MirSurfaceCreateInfoKhr;
        }
    }

    // Reserved for future use.
    internal enum MirSurfaceCreateFlagsKhr
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying parameters of a newly created Wayland surface object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WaylandSurfaceCreateInfoKhr
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal WaylandSurfaceCreateFlagsKhr Flags;

        /// <summary>
        /// Wayland wl_display to associate the surface with.
        /// </summary>
        public IntPtr Display;
        /// <summary>
        /// Wayland wl_surface to associate the surface with.
        /// </summary>
        public IntPtr Surface;

        /// <summary>
        /// Initializes a new instance of <see cref="WaylandSurfaceCreateInfoKhr"/> structure.
        /// </summary>
        /// <param name="display">Wayland wl_display to associate the surface with.</param>
        /// <param name="surface">Wayland wl_surface to associate the surface with.</param>
        public WaylandSurfaceCreateInfoKhr(IntPtr display, IntPtr surface)
        {
            Display = display;
            Surface = surface;
            Type = StructureType.WaylandSurfaceCreateInfoKhr;
            Next = IntPtr.Zero;
            Flags = 0;
        }

        internal void Prepare()
        {
            Type = StructureType.WaylandSurfaceCreateInfoKhr;
        }
    }

    internal enum WaylandSurfaceCreateFlagsKhr
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying parameters of a newly created Win32 surface object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Win32SurfaceCreateInfoKhr
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal Win32SurfaceCreateFlagsKhr Flags;

        /// <summary>
        /// Win32 HINSTANCE to associate the surface with.
        /// </summary>
        public IntPtr HInstance;
        /// <summary>
        /// Win32 HWND for the window to associate the surface with.
        /// </summary>
        public IntPtr Hwnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="Win32SurfaceCreateInfoKhr"/> structure.
        /// </summary>
        /// <param name="hInstance">Win32 HINSTANCE to associate the surface with.</param>
        /// <param name="hwnd">Win32 HWND to associate the surface with.</param>
        public Win32SurfaceCreateInfoKhr(IntPtr hInstance, IntPtr hwnd)
        {
            HInstance = hInstance;
            Hwnd = hwnd;
            Type = StructureType.Win32SurfaceCreateInfoKhr;
            Next = IntPtr.Zero;
            Flags = 0;
        }

        internal void Prepare()
        {
            Type = StructureType.Win32SurfaceCreateInfoKhr;
        }
    }

    // Reserved for future use.
    [Flags]
    internal enum Win32SurfaceCreateFlagsKhr
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying parameters of a newly created Xlib surface object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct XlibSurfaceCreateInfoKhr
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal XlibSurfaceCreateFlagsKhr Flags;

        /// <summary>
        /// A pointer to an Xlib Display connection to the X server.
        /// </summary>
        public IntPtr Dpy;
        /// <summary>
        /// A pointer to an Xlib Window to associate the surface with.
        /// </summary>
        public IntPtr Window;

        /// <summary>
        /// Initializes a new instance of the <see cref="XlibSurfaceCreateInfoKhr"/> structure.
        /// </summary>
        /// <param name="display">A pointer to an Xlib Display connection to the X server.</param>
        /// <param name="window">A pointer to an Xlib Window to associate the surface with.</param>
        public XlibSurfaceCreateInfoKhr(IntPtr display, IntPtr window)
        {
            Type = StructureType.XlibSurfaceCreateInfoKhr;
            Next = IntPtr.Zero;
            Flags = 0;
            Dpy = display;
            Window = window;
        }

        internal void Prepare()
        {
            Type = StructureType.XlibSurfaceCreateInfoKhr;
        }
    }

    // Reserved for future use.
    [Flags]
    internal enum XlibSurfaceCreateFlagsKhr
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying parameters of a newly created display plane surface object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplaySurfaceCreateInfoKhr
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal DisplaySurfaceCreateFlagsKhr Flags;

        /// <summary>
        /// A <see cref="DisplayModeKhr"/> handle specifying the mode to use when displaying this surface.
        /// </summary>
        public long DisplayMode;
        /// <summary>
        /// The plane on which this surface appears.
        /// </summary>
        public int PlaneIndex;
        /// <summary>
        /// The z-order of the plane.
        /// </summary>
        public int PlaneStackIndex;
        /// <summary>
        /// Specifies the transform to apply to the images as part of the scanout operation.
        /// </summary>
        public SurfaceTransformsKhr Transform;
        /// <summary>
        /// The global alpha value.
        /// <para>This value is ignored if <see cref="AlphaMode"/> is not <see cref="DisplayPlaneAlphasKhr.Global"/>.</para>
        /// </summary>
        public float GlobalAlpha;
        /// <summary>
        /// Specifies the type of alpha blending to use.
        /// </summary>
        public DisplayPlaneAlphasKhr AlphaMode;
        /// <summary>
        /// The size of the presentable images to use with the surface.
        /// </summary>
        public Extent2D ImageExtent;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplaySurfaceCreateInfoKhr"/> structure.
        /// </summary>
        /// <param name="displayMode">
        /// A <see cref="DisplayModeKhr"/> handle specifying the mode to use when displaying this surface.
        /// </param>
        /// <param name="planeIndex">The plane on which this surface appears.</param>
        /// <param name="planeStackIndex">The z-order of the plane.</param>
        /// <param name="transform">
        /// Specifies the transform to apply to the images as part of the scanout operation.
        /// </param>
        /// <param name="globalAlpha">
        /// The global alpha value. This value is ignored if <see cref="AlphaMode"/> is not <see cref="DisplayPlaneAlphasKhr.Global"/>.
        /// </param>
        /// <param name="alphaMode">Specifies the type of alpha blending to use.</param>
        /// <param name="imageExtent">Size of the images to use with this surface.</param>
        public DisplaySurfaceCreateInfoKhr(DisplayModeKhr displayMode, int planeIndex, int planeStackIndex,
            SurfaceTransformsKhr transform, float globalAlpha, DisplayPlaneAlphasKhr alphaMode, Extent2D imageExtent)
        {
            Type = StructureType.DisplaySurfaceCreateInfoKhr;
            Next = IntPtr.Zero;
            Flags = 0;
            DisplayMode = displayMode;
            PlaneIndex = planeIndex;
            PlaneStackIndex = planeStackIndex;
            Transform = transform;
            GlobalAlpha = globalAlpha;
            AlphaMode = alphaMode;
            ImageExtent = imageExtent;
        }

        internal void Prepare()
        {
            Type = StructureType.DisplaySurfaceCreateInfoKhr;
        }
    }

    // Reserved for future use.
    [Flags]
    internal enum DisplaySurfaceCreateFlagsKhr
    {
        None = 0
    }

    /// <summary>
    /// Alpha blending type.
    /// </summary>
    [Flags]
    public enum DisplayPlaneAlphasKhr
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that the source image will be treated as opaque.
        /// </summary>
        Opaque = 1 << 0,
        /// <summary>
        /// Specifies that a global alpha value must be specified that will be applied to all pixels
        /// in the source image.
        /// </summary>
        Global = 1 << 1,
        /// <summary>
        /// Specifies that the alpha value will be determined by the alpha channel of the source
        /// image's pixels.
        /// <para>If the source format contains no alpha values, no blending will be applied.</para>
        /// <para>
        /// The source alpha values are not premultiplied into the source image's other color channels.
        /// </para>
        /// </summary>
        PerPixel = 1 << 2,
        /// <summary>
        /// Is equivalent to <see cref="PerPixel"/>, except the source alpha values are assumed to be
        /// premultiplied into the source image's other color channels.
        /// </summary>
        PerPixelPremultiplied = 1 << 3
    }

    /// <summary>
    /// Structure specifying parameters of a newly created Xcb surface object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct XcbSurfaceCreateInfoKhr
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal XcbSurfaceCreateFlagsKhr Flags;

        /// <summary>
        /// A pointer to an xcb_connection_t to the X server.
        /// </summary>
        public IntPtr Connection;
        /// <summary>
        /// The xcb_window_t for the X11 window to associate the surface with.
        /// </summary>
        public IntPtr Window;

        /// <summary>
        /// Initializes a new instance of the <see cref="XcbSurfaceCreateInfoKhr"/> structure.
        /// </summary>
        /// <param name="connection">A pointer to an xcb_connection_t to the X server.</param>
        /// <param name="window">The xcb_window_t for the X11 window to associate the surface with.</param>
        public XcbSurfaceCreateInfoKhr(IntPtr connection, IntPtr window)
        {
            Type = StructureType.XcbSurfaceCreateInfoKhr;
            Next = IntPtr.Zero;
            Flags = 0;
            Connection = connection;
            Window = window;
        }

        internal void Prepare()
        {
            Type = StructureType.XcbSurfaceCreateInfoKhr;
        }
    }

    // Reserved for future use.
    [Flags]
    internal enum XcbSurfaceCreateFlagsKhr
    {
        None = 0
    }
}
