using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;
using static VulkanCore.Constants;

namespace VulkanCore.Ext
{
    /// <summary>
    /// Provides extension methods for the <see cref="PhysicalDevice"/> class.
    /// </summary>
    public static unsafe class PhysicalDeviceExtensions
    {
        /// <summary>
        /// Query surface capabilities.
        /// </summary>
        /// <param name="physicalDevice">
        /// The physical device that will be associated with the swapchain to be created, as
        /// described for <see cref="Khr.DeviceExtensions.CreateSwapchainKhr"/>.
        /// </param>
        /// <param name="surface">The surface that will be associated with the swapchain.</param>
        /// <returns>The structure in which the capabilities are returned.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceCapabilities2Ext GetSurfaceCapabilities2Ext(this PhysicalDevice physicalDevice, SurfaceKhr surface)
        {
            SurfaceCapabilities2Ext capabilities;
            Result result = GetPhysicalDeviceSurfaceCapabilities2Ext(physicalDevice, surface, &capabilities);
            VulkanException.ThrowForInvalidResult(result);
            return capabilities;
        }

        /// <summary>
        /// Query the <see cref="DisplayKhr"/> corresponding to an X11 RandR Output.
        /// <para>
        /// When acquiring displays from an X11 server, an application may also wish to enumerate and
        /// identify them using a native handle rather than a <see cref="DisplayKhr"/> handle.
        /// </para>
        /// </summary>
        /// <param name="physicalDevice">The physical device to query the display handle on.</param>
        /// <param name="dpy">A connection to the X11 server from which pname:rrOutput was queried.</param>
        /// <param name="rrOutput">An X11 RandR output ID.</param>
        /// <returns>The corresponding <see cref="DisplayKhr"/> handle.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static DisplayKhr GetRandROutputDisplayExt(this PhysicalDevice physicalDevice, IntPtr dpy,
            IntPtr rrOutput)
        {
            long handle;
            Result result = GetRandROutputDisplayExt(physicalDevice, dpy, rrOutput, &handle);
            VulkanException.ThrowForInvalidResult(result);
            return new DisplayKhr(physicalDevice, handle);
        }

        [DllImport(VulkanDll, EntryPoint = "vkGetPhysicalDeviceSurfaceCapabilities2EXT", CallingConvention = CallConv)]
        private static extern Result GetPhysicalDeviceSurfaceCapabilities2Ext(IntPtr physicalDevice,
            long surface, SurfaceCapabilities2Ext* surfaceCapabilities);

        [DllImport(VulkanDll, EntryPoint = "vkGetRandROutputDisplayEXT", CallingConvention = CallConv)]
        private static extern Result GetRandROutputDisplayExt(IntPtr physicalDevice, IntPtr dpy, IntPtr rrOutput, long* display);
    }

    /// <summary>
    /// Structure describing capabilities of a surface.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SurfaceCapabilities2Ext
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The minimum number of images the specified device supports for a swapchain created for
        /// the surface, and will be at least one.
        /// </summary>
        public int MinImageCount;
        /// <summary>
        /// The maximum number of images the specified device supports for a swapchain created for
        /// the surface, and will be either 0, or greater than or equal to <see
        /// cref="MinImageCount"/>. A value of 0 means that there is no limit on the number of
        /// images, though there may be limits related to the total amount of memory used by
        /// swapchain images.
        /// </summary>
        public int MaxImageCount;
        /// <summary>
        /// The current width and height of the surface, or the special value <see
        /// cref="Extent2D.WholeSize"/> indicating that the surface size will be determined by the
        /// extent of a swapchain targeting the surface.
        /// </summary>
        public Extent2D CurrentExtent;
        /// <summary>
        /// Contains the smallest valid swapchain extent for the surface on the specified device. The
        /// width and height of the extent will each be less than or equal to the corresponding width
        /// and height of <see cref="CurrentExtent"/>, unless <see cref="CurrentExtent"/> has the
        /// special value described above.
        /// </summary>
        public Extent2D MinImageExtent;
        /// <summary>
        /// Contains the largest valid swapchain extent for the surface on the specified device. The
        /// width and height of the extent will each be greater than or equal to the corresponding
        /// width and height of <see cref="MinImageExtent"/>. The width and height of the extent will
        /// each be greater than or equal to the corresponding width and height of <see
        /// cref="CurrentExtent"/>, unless <see cref="CurrentExtent"/> has the special value
        /// described above.
        /// </summary>
        public Extent2D MaxImageExtent;
        /// <summary>
        /// The maximum number of layers swapchain images can have for a swapchain created for this
        /// device and surface, and will be at least one.
        /// </summary>
        public int MaxImageArrayLayers;
        /// <summary>
        /// A bitmask of <see cref="SurfaceTransformsKhr"/>, describing the presentation
        /// transforms supported for the surface on the specified device, and at least one bit will
        /// be set.
        /// </summary>
        public SurfaceTransformsKhr SupportedTransforms;
        /// <summary>
        /// The surface's current transform relative to the presentation engine's natural
        /// orientation, as described by <see cref="SurfaceTransformsKhr"/>.
        /// </summary>
        public SurfaceTransformsKhr CurrentTransform;
        /// <summary>
        /// A bitmask of <see cref="CompositeAlphasKhr"/>, representing the alpha compositing
        /// modes supported by the presentation engine for the surface on the specified device, and
        /// at least one bit will be set. Opaque composition can be achieved in any alpha compositing
        /// mode by either using a swapchain image format that has no alpha component, or by ensuring
        /// that all pixels in the swapchain images have an alpha value of 1.0.
        /// </summary>
        public CompositeAlphasKhr SupportedCompositeAlpha;
        /// <summary>
        /// A bitmask of <see cref="ImageUsages"/> representing the ways the
        /// application can use the presentable images of a swapchain created for the
        /// surface on the specified device. <see cref="ImageUsages.ColorAttachment"/>
        /// must be included in the set but implementations may support additional usages. 
        /// </summary>
        public ImageUsages SupportedUsageFlags;
        /// <summary>
        /// A bitfield containing one bit set for each surface counter type supported.
        /// <para>
        /// Must not include <see cref="SurfaceCountersExt.VBlank"/> unless the surface queried
        /// is a display surface.
        /// </para>
        /// </summary>
        public int SupportedSurfaceCounters;
    }

    /// <summary>
    /// Surface-relative counter types.
    /// </summary>
    [Flags]
    public enum SurfaceCountersExt
    {
        /// <summary>
        /// A counter incrementing once every time a vblank period occurs on the display
        /// associated with the surface.
        /// </summary>
        VBlank = 1 << 0
    }
}