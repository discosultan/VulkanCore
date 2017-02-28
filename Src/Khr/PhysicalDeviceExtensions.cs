using System;
using System.Runtime.InteropServices;
using VulkanCore.Ext;
using static VulkanCore.Constant;

namespace VulkanCore.Khr
{
    /// <summary>
    /// Provides Khronos specific extension methods for the <see cref="PhysicalDevice"/> class.
    /// </summary>
    public static unsafe class PhysicalDeviceExtensions
    {
        /// <summary>
        /// Query if presentation is supported.
        /// </summary>
        /// <param name="physicalDevice">The physical device.</param>
        /// <param name="queueFamilyIndex">The queue family.</param>
        /// <param name="surface">The surface to query for.</param>
        /// <returns><c>true</c> if supported; otherwise <c>false</c>.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static Bool GetSurfaceSupportKhr(this PhysicalDevice physicalDevice, int queueFamilyIndex, SurfaceKhr surface)
        {
            Bool isSupported;
            Result result = vkGetPhysicalDeviceSurfaceSupportKHR(physicalDevice, queueFamilyIndex, surface, &isSupported);
            VulkanException.ThrowForInvalidResult(result);
            return isSupported;
        }

        /// <summary>
        /// Query surface capabilities.
        /// </summary>
        /// <param name="physicalDevice">
        /// The physical device that will be associated with the swapchain to be created,
        /// </param>
        /// <param name="surface">The surface that will be associated with the swapchain.</param>
        /// <returns>A structure in which the capabilities are returned.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceCapabilitiesKhr GetSurfaceCapabilitiesKhr(this PhysicalDevice physicalDevice, SurfaceKhr surface)
        {
            SurfaceCapabilitiesKhr capabilities;
            Result result = vkGetPhysicalDeviceSurfaceCapabilitiesKHR(physicalDevice, surface, &capabilities);
            VulkanException.ThrowForInvalidResult(result);
            return capabilities;
        }

        /// <summary>
        /// Query color formats supported by surface.
        /// </summary>
        /// <param name="device">A valid physical device.</param>
        /// <param name="surface">The surface to query.</param>
        /// <returns>An array of valid surface formats.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceFormatKhr[] GetSurfaceFormatsKhr(this PhysicalDevice device, SurfaceKhr surface)
        {
            int count;
            Result result = vkGetPhysicalDeviceSurfaceFormatsKHR(device, surface, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var formats = new SurfaceFormatKhr[count];
            fixed (SurfaceFormatKhr* formatsPtr = formats)
                result = vkGetPhysicalDeviceSurfaceFormatsKHR(device, surface, &count, formatsPtr);
            VulkanException.ThrowForInvalidResult(result);
            return formats;
        }

        /// <summary>
        /// Query supported presentation modes.
        /// </summary>
        /// <param name="physicalDevice">
        /// The physical device that will be associated with the swapchain to be created.
        /// </param>
        /// <param name="surface">The surface that will be associated with the swapchain.</param>
        /// <returns>An array of valid present modes.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static PresentModeKhr[] GetSurfacePresentModesKhr(this PhysicalDevice physicalDevice, SurfaceKhr surface)
        {
            int count;
            Result result = vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var presentModes = new PresentModeKhr[count];
            fixed (PresentModeKhr* presentModesPtr = presentModes)
                result = vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, &count, presentModesPtr);
            VulkanException.ThrowForInvalidResult(result);
            return presentModes;
        }

        /// <summary>
        /// Query information about the available displays.
        /// </summary>
        /// <param name="device">A valid physical device.</param>
        /// <returns>An array of <see cref="DisplayPropertiesKhr"/> structures.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static DisplayPropertiesKhr[] GetDisplayPropertiesKhr(this PhysicalDevice device)
        {
            int count;
            Result result = vkGetPhysicalDeviceDisplayPropertiesKHR(device, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var nativeDisplayProperties = stackalloc DisplayPropertiesKhr.Native[count];
            result = vkGetPhysicalDeviceDisplayPropertiesKHR(device, &count, nativeDisplayProperties);
            VulkanException.ThrowForInvalidResult(result);

            DisplayPropertiesKhr.Native* walk = nativeDisplayProperties;
            var properties = new DisplayPropertiesKhr[count];
            for (int i = 0; i < count; i++)
                DisplayPropertiesKhr.FromNative(walk++, out properties[i]);
            return properties;
        }

        /// <summary>
        /// Query the plane properties.
        /// <para>
        /// Images are presented to individual planes on a display. Devices must support at least one
        /// plane on each display.Planes can be stacked and blended to composite multiple images on
        /// one display. Devices may support only a fixed stacking order and fixed mapping between
        /// planes and displays, or they may allow arbitrary application specified stacking orders
        /// and mappings between planes and displays.
        /// </para>
        /// </summary>
        /// <param name="physicalDevice">A valid physical device.</param>
        /// <returns>An array of <see cref="DisplayPlanePropertiesKhr"/> structures.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static DisplayPlanePropertiesKhr[] GetDisplayPlanePropertiesKhr(this PhysicalDevice physicalDevice)
        {
            int count;
            Result result = vkGetPhysicalDeviceDisplayPlanePropertiesKHR(physicalDevice, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var properties = new DisplayPlanePropertiesKhr[count];
            fixed (DisplayPlanePropertiesKhr* propertiesPtr = properties)
                result = vkGetPhysicalDeviceDisplayPlanePropertiesKHR(physicalDevice, &count, propertiesPtr);
            VulkanException.ThrowForInvalidResult(result);
            return properties;
        }

        /// <summary>
        /// Get list of displays a plane supports.
        /// </summary>
        /// <param name="device">A valid physical device.</param>
        /// <param name="planeIndex">The plane which the application wishes to use.</param>
        /// <returns>An array of display handles.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static DisplayKhr[] GetDisplayPlaneSupportedDisplaysKhr(this PhysicalDevice device, int planeIndex)
        {
            int count;
            Result result = vkGetDisplayPlaneSupportedDisplaysKHR(device, planeIndex, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var displaysPtr = stackalloc long[count];
            result = vkGetDisplayPlaneSupportedDisplaysKHR(device, planeIndex, &count, displaysPtr);
            VulkanException.ThrowForInvalidResult(result);

            var displays = new DisplayKhr[count];
            for (int i = 0; i < count; i++)
                displays[i] = new DisplayKhr(device, displaysPtr[i]);
            return displays;
        }

        /// <summary>
        /// Get the capabilities of a mode and plane combination.
        /// </summary>
        /// <param name="physicalDevice">The physical device associated with the display.</param>
        /// <param name="mode">
        /// The display mode the application intends to program when using the specified plane.
        /// </param>
        /// <param name="planeIndex">The plane which the application intends to use with the display.</param>
        /// <returns>A <see cref="DisplayPlaneCapabilitiesKhr"/> structure.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static DisplayPlaneCapabilitiesKhr GetDisplayPlaneCapabilitiesKhr(
            this PhysicalDevice physicalDevice, DisplayModeKhr mode, int planeIndex)
        {
            var capabilities = new DisplayPlaneCapabilitiesKhr();
            Result result = vkGetDisplayPlaneCapabilitiesKHR(physicalDevice, mode, planeIndex, &capabilities);
            VulkanException.ThrowForInvalidResult(result);
            return capabilities;
        }

        /// <summary>
        /// Query physical device for presentation to Mir.
        /// <para>
        /// Determines whether a queue family of a physical device supports presentation to the Mir compositor.
        /// </para>
        /// </summary>
        /// <param name="device">A physical device handle.</param>
        /// <param name="queueFamilyIndex">Index to a queue family.</param>
        /// <param name="connection">A pointer to a MirConnection.</param>
        /// <returns><c>true</c> if the physical device supports presentation to the Mir compositor.</returns>
        public static bool GetMirPresentationSupportKhr(this PhysicalDevice device, 
            int queueFamilyIndex, IntPtr connection)
        {
            return vkGetPhysicalDeviceMirPresentationSupportKHR(device, queueFamilyIndex, &connection);
        }

        /// <summary>
        /// Query physical device for presentation to Wayland.
        /// <para>
        /// Determines whether a queue family of a physical device supports presentation to the
        /// Wayland compositor.
        /// </para>
        /// </summary>
        /// <param name="physicalDevice">A physical device handle.</param>
        /// <param name="queueFamilyIndex">Index to a queue family.</param>
        /// <param name="display">A pointer to a wl_display value.</param>
        /// <returns>
        /// <c>true</c> if the physical device supports presentation to the Wayland compositor.
        /// </returns>
        public static bool GetWaylandPresentationSupportKhr(this PhysicalDevice physicalDevice, 
            int queueFamilyIndex, IntPtr display)
        {
            return vkGetPhysicalDeviceWaylandPresentationSupportKHR(physicalDevice, queueFamilyIndex, &display);
        }

        /// <summary>
        /// Query physical device for presentation to X11 server using XCB.
        /// <para>
        /// Determines whether a queue family of a physical device supports presentation to an X11
        /// server, using the XCB client-side library.
        /// </para>
        /// </summary>
        /// <param name="device">A physical device handle.</param>
        /// <param name="queueFamilyIndex">Index to a queue family.</param>
        /// <param name="connection">Pointer to a xcb_connection_t value.</param>
        /// <param name="visualId">An xcb_visualid_t.</param>
        /// <returns><c>true</c> if the physical device supports presentation to an X11 server.</returns>
        public static bool GetXcbPresentationSupportKhr(
            this PhysicalDevice device, int queueFamilyIndex, IntPtr connection, IntPtr visualId)
        {
            return vkGetPhysicalDeviceXcbPresentationSupportKHR(device, queueFamilyIndex, &connection, visualId);
        }

        /// <summary>
        /// Query physical device for presentation to Windows desktop.
        /// <para>
        /// Determines whether a queue family of a physical device supports presentation to the
        /// Microsoft Windows desktop.
        /// </para>
        /// </summary>
        /// <param name="physicalDevice">A physical device handle.</param>
        /// <param name="queueFamilyIndex">Index to a queue family.</param>
        /// <returns>
        /// <c>true</c> if the physical device supports presentation to The Windows desktop.
        /// </returns>
        public static bool GetWin32PresentationSupportKhr(this PhysicalDevice physicalDevice, int queueFamilyIndex)
        {
            return vkGetPhysicalDeviceWin32PresentationSupportKHR(physicalDevice, queueFamilyIndex);
        }

        /// <summary>
        /// Query physical device for presentation to X11 server using Xlib.
        /// <para>
        /// Determines whether a queue family of a physical device supports presentation to an X11
        /// server, using the Xlib client-side library.
        /// </para>
        /// </summary>
        /// <param name="device">A physical device handle.</param>
        /// <param name="queueFamilyIndex">Index to a queue family.</param>
        /// <param name="connection">Pointer to an Xlib Display.</param>
        /// <param name="visualId">An X11 VisualID.</param>
        /// <returns><c>true</c> if the physical device supports presentation to an X11 server.</returns>
        public static bool GetXlibPresentationSupportKhr(
            this PhysicalDevice device, int queueFamilyIndex, IntPtr connection, IntPtr visualId)
        {
            return vkGetPhysicalDeviceXlibPresentationSupportKHR(device, queueFamilyIndex, &connection, visualId);
        }

        /// <summary>
        /// Reports capabilities of a physical device.
        /// </summary>
        /// <param name="device">The physical device from which to query the supported features.</param>
        /// <returns>A structure in which the physical device features are returned.</returns>
        public static PhysicalDeviceFeatures2Khr GetFeatures2Khr(this PhysicalDevice device)
        {
            PhysicalDeviceFeatures2Khr features;
            vkGetPhysicalDeviceFeatures2KHR(device, &features);
            return features;
        }

        /// <summary>
        /// Returns properties of a physical device.
        /// </summary>
        /// <param name="physicalDevice">
        /// the handle to the physical device whose properties will be queried.
        /// </param>
        /// <returns>The structure, that will be filled with returned information.</returns>
        public static PhysicalDeviceProperties2Khr GetProperties2Khr(this PhysicalDevice physicalDevice)
        {
            PhysicalDeviceProperties2Khr.Native nativeProperties;
            vkGetPhysicalDeviceProperties2KHR(physicalDevice, &nativeProperties);
            PhysicalDeviceProperties2Khr.FromNative(
                ref nativeProperties, 
                out PhysicalDeviceProperties2Khr properties);
            return properties;
        }

        /// <summary>
        /// Lists physical device's format capabilities.
        /// </summary>
        /// <param name="physicalDevice">The physical device from which to query the format properties.</param>
        /// <param name="format">The format whose properties are queried.</param>
        /// <returns>
        /// The structure in which physical device properties for <paramref name="format"/> are returned.
        /// </returns>
        public static FormatProperties2Khr GetFormatProperties2Khr(this PhysicalDevice physicalDevice, Format format)
        {
            FormatProperties2Khr properties;
            vkGetPhysicalDeviceFormatProperties2KHR(physicalDevice, format, &properties);
            return properties;
        }

        /// <summary>
        /// Lists physical device's image format capabilities.
        /// <para>
        /// Behaves similarly to <see cref="PhysicalDevice.GetImageFormatProperties"/>, with the
        /// ability to return extended information via chained output structures.
        /// </para>
        /// <para>
        /// If the loader implementation emulates the command on a device that doesn't support the
        /// extension, and the query involves a structure the loader does not support, the command
        /// throws with <see cref="Result.ErrorFormatNotSupported"/>.
        /// </para>
        /// </summary>
        /// <param name="physicalDevice">The physical device from which to query the image capabilities.</param>
        /// <param name="imageFormatInfo">
        /// A structure, describing the parameters that would be consumed by <see cref="Device.CreateImage"/>.
        /// </param>
        /// <returns>A structure in which capabilities are returned.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static ImageFormatProperties2Khr GetImageFormatProperties2Khr(this PhysicalDevice physicalDevice, 
            PhysicalDeviceImageFormatInfo2Khr imageFormatInfo)
        {
            ImageFormatProperties2Khr properties;
            Result result = vkGetPhysicalDeviceImageFormatProperties2KHR(physicalDevice, &imageFormatInfo, &properties);
            VulkanException.ThrowForInvalidResult(result);
            return properties;
        }

        /// <summary>
        /// Reports properties of the queues of the specified physical device.
        /// </summary>
        /// <param name="device">The handle to the physical device whose properties will be queried.</param>
        /// <returns>An array of <see cref="QueueFamilyProperties2Khr"/> structures.</returns>
        public static QueueFamilyProperties2Khr[] GetQueueFamilyProperties2Khr(this PhysicalDevice device)
        {
            int count;
            vkGetPhysicalDeviceQueueFamilyProperties2KHR(device, &count, null);

            var properties = new QueueFamilyProperties2Khr[count];
            fixed (QueueFamilyProperties2Khr* propertiesPtr = properties)
                vkGetPhysicalDeviceQueueFamilyProperties2KHR(device, &count, propertiesPtr);

            return properties;
        }

        /// <summary>
        /// Reports memory information for the specified physical device.
        /// <para>
        /// Behaves similarly to <see cref="PhysicalDevice.GetMemoryProperties"/>, with the ability
        /// to return extended information via chained output structures.
        /// </para>
        /// </summary>
        /// <param name="physicalDevice">The handle to the device to query.</param>
        /// <returns>The structure in which the properties are returned.</returns>
        public static PhysicalDeviceMemoryProperties2Khr GetMemoryProperties2Khr(this PhysicalDevice physicalDevice)
        {
            var nativeProperties = new PhysicalDeviceMemoryProperties2Khr.Native();
            vkGetPhysicalDeviceMemoryProperties2KHR(physicalDevice, ref nativeProperties);
            PhysicalDeviceMemoryProperties2Khr.FromNative(ref nativeProperties, out var properties);
            return properties;
        }

        /// <summary>
        /// Retrieve properties of an image format applied to sparse images.
        /// <para>
        /// Each element will describe properties for one set of image aspects that are bound
        /// simultaneously in the image.
        /// </para>
        /// <para>
        /// This is usually one element for each aspect in the image, but for interleaved
        /// depth/stencil images there is only one element describing the combined aspects.
        /// </para>
        /// </summary>
        /// <param name="physicalDevice">
        /// The physical device from which to query the sparse image capabilities.
        /// </param>
        /// <param name="formatInfo">Contains input parameters to the command.</param>
        /// <returns>An array of <see cref="SparseImageFormatProperties2Khr"/> structures.</returns>
        public static SparseImageFormatProperties2Khr[] GetSparseImageFormatProperties2Khr(this PhysicalDevice physicalDevice,
            PhysicalDeviceSparseImageFormatInfo2Khr formatInfo)
        {
            int count;
            vkGetPhysicalDeviceSparseImageFormatProperties2KHR(physicalDevice, &formatInfo, &count, null);

            var properties = new SparseImageFormatProperties2Khr[count];
            fixed (SparseImageFormatProperties2Khr* propertiesPtr = properties)
                vkGetPhysicalDeviceSparseImageFormatProperties2KHR(physicalDevice, &formatInfo, &count, propertiesPtr);
            return properties;
        }

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetPhysicalDeviceSurfaceSupportKHR(IntPtr physicalDevice, 
            int queueFamilyIndex, long surface, Bool* supported);
        
        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetPhysicalDeviceSurfaceCapabilitiesKHR(IntPtr physicalDevice, 
            long surface, SurfaceCapabilitiesKhr* surfaceCapabilities);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetPhysicalDeviceSurfaceFormatsKHR(IntPtr physicalDevice, 
            long surface, int* surfaceFormatCount, SurfaceFormatKhr* surfaceFormats);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetPhysicalDeviceSurfacePresentModesKHR(IntPtr physicalDevice,
            long surface, int* presentModeCount, PresentModeKhr* presentModes);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetPhysicalDeviceDisplayPropertiesKHR(IntPtr physicalDevice, 
            int* propertyCount, DisplayPropertiesKhr.Native* properties);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetPhysicalDeviceDisplayPlanePropertiesKHR(
            IntPtr physicalDevice, int* propertyCount, DisplayPlanePropertiesKhr* properties);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetDisplayPlaneSupportedDisplaysKHR(
            IntPtr physicalDevice, int planeIndex, int* displayCount, long* displays);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetDisplayPlaneCapabilitiesKHR(
            IntPtr physicalDevice, long mode, int planeIndex, DisplayPlaneCapabilitiesKhr* capabilities);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Bool vkGetPhysicalDeviceMirPresentationSupportKHR(
            IntPtr physicalDevice, int queueFamilyIndex, IntPtr* connection);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Bool vkGetPhysicalDeviceWaylandPresentationSupportKHR(
            IntPtr physicalDevice, int queueFamilyIndex, IntPtr* display);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Bool vkGetPhysicalDeviceXcbPresentationSupportKHR(
            IntPtr physicalDevice, int queueFamilyIndex, IntPtr* connection, IntPtr visualId);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Bool vkGetPhysicalDeviceWin32PresentationSupportKHR(IntPtr physicalDevice, int queueFamilyIndex);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Bool vkGetPhysicalDeviceXlibPresentationSupportKHR(
            IntPtr physicalDevice, int queueFamilyIndex, IntPtr* dpy, IntPtr visualId);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkGetPhysicalDeviceFeatures2KHR(IntPtr physicalDevice, PhysicalDeviceFeatures2Khr* features);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkGetPhysicalDeviceProperties2KHR(IntPtr physicalDevice, PhysicalDeviceProperties2Khr.Native* properties);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkGetPhysicalDeviceFormatProperties2KHR(IntPtr physicalDevice, Format format, FormatProperties2Khr* formatProperties);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetPhysicalDeviceImageFormatProperties2KHR(IntPtr physicalDevice,
            PhysicalDeviceImageFormatInfo2Khr* imageFormatInfo, ImageFormatProperties2Khr* imageFormatProperties);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkGetPhysicalDeviceQueueFamilyProperties2KHR(IntPtr physicalDevice,
            int* queueFamilyPropertyCount, QueueFamilyProperties2Khr* queueFamilyProperties);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkGetPhysicalDeviceMemoryProperties2KHR(
            IntPtr physicalDevice, ref PhysicalDeviceMemoryProperties2Khr.Native memoryProperties);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkGetPhysicalDeviceSparseImageFormatProperties2KHR(IntPtr physicalDevice,
            PhysicalDeviceSparseImageFormatInfo2Khr* formatInfo, int* propertyCount, SparseImageFormatProperties2Khr* properties);
    }

    /// <summary>
    /// Structure describing capabilities of a surface.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SurfaceCapabilitiesKhr
    {
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
        /// presentable images.
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
        /// The maximum number of layers presentable images can have for a swapchain created for this
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
        /// mode by either using an image format that has no alpha component, or by ensuring
        /// that all pixels in the presentable images have an alpha value of 1.0.
        /// </summary>
        public CompositeAlphasKhr SupportedCompositeAlpha;
        /// <summary>
        /// A bitmask of <see cref="ImageUsages"/> representing the ways the application can use
        /// the presentable images of a swapchain created for the surface on the specified device.
        /// <see cref="ImageUsages.ColorAttachment"/> must be included in the set but
        /// implementations may support additional usages.
        /// </summary>
        public ImageUsages SupportedUsageFlags;
    }

    /// <summary>
    /// Structure describing a supported swapchain format-color space pair.
    /// <para>
    /// While the <see cref="Format"/> of a presentable image refers to the encoding of each pixel,
    /// the <see cref="ColorSpace"/> determines how the presentation engine interprets the pixel values.
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SurfaceFormatKhr
    {
        /// <summary>
        /// A format that is compatible with the specified surface.
        /// </summary>
        public Format Format;
        /// <summary>
        /// A presentation color space that is compatible with the surface.
        /// </summary>
        public ColorSpaceKhr ColorSpace;
    }

    /// <summary>
    /// Structure describing an available display device.
    /// </summary>
    public unsafe struct DisplayPropertiesKhr
    {
        /// <summary>
        /// A handle that is used to refer to the display described here. This handle will be valid
        /// for the lifetime of the Vulkan instance.
        /// </summary>
        public long Display;
        /// <summary>
        /// A unicode string containing the name of the display. Generally, this will be the name
        /// provided by the display's EDID. It can be <c>null</c> if no suitable name is available.
        /// </summary>
        public string DisplayName;
        /// <summary>
        /// Describes the physical width and height of the visible portion of the display, in millimeters.
        /// </summary>
        public Extent2D PhysicalDimensions;
        /// <summary>
        /// Describes the physical, native, or preferred resolution of the display.
        /// </summary>
        public Extent2D PhysicalResolution;
        /// <summary>
        /// Tells which transforms are supported by this display. This will contain one or more of
        /// the bits from <see cref="SurfaceTransformsKhr"/>.
        /// </summary>
        public SurfaceTransformsKhr SupportedTransforms;
        /// <summary>
        /// Tells whether the planes on this display can have their z order changed. If this is
        /// <c>true</c>, the application can re-arrange the planes on this display in any order
        /// relative to each other.
        /// </summary>
        public Bool PlaneReorderPossible;
        /// <summary>
        /// Tells whether the display supports self-refresh/internal buffering. If this is
        /// <c>true</c>, the application can submit persistent present operations on swapchains
        /// created against this display.
        /// </summary>
        public Bool PersistentContent;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public long Display;
            public byte* DisplayName;
            public Extent2D PhysicalDimensions;
            public Extent2D PhysicalResolution;
            public SurfaceTransformsKhr SupportedTransforms;
            public Bool PlaneReorderPossible;
            public Bool PersistentContent;
        }

        internal static void FromNative(Native* native, out DisplayPropertiesKhr val)
        {
            val.Display = native->Display;
            val.DisplayName = Interop.String.FromPointer(native->DisplayName);
            val.PhysicalDimensions = native->PhysicalDimensions;
            val.PhysicalResolution = native->PhysicalResolution;
            val.SupportedTransforms = native->SupportedTransforms;
            val.PlaneReorderPossible = native->PlaneReorderPossible;
            val.PersistentContent = native->PersistentContent;
        }
    }

    /// <summary>
    /// Structure describing display plane properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayPlanePropertiesKhr
    {
        /// <summary>
        /// The handle of the display the plane is currently associated with. If the plane is not
        /// currently attached to any displays, this will be <see cref="IntPtr.Zero"/>.
        /// </summary>
        public long CurrentDisplay;
        /// <summary>
        /// The current z-order of the plane. This will be between 0 and the count of the elements
        /// returned by <see cref="PhysicalDeviceExtensions.GetDisplayPlanePropertiesKhr"/>.
        /// </summary>
        public int CurrentStackIndex;
    }

    /// <summary>
    /// Structure describing display parameters associated with a display mode.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayModeParametersKhr
    {
        /// <summary>
        /// The 2D extents of the visible region.
        /// </summary>
        public Extent2D VisibleRegion;
        /// <summary>
        /// The number of times the display is refreshed each second multiplied by 1000.
        /// </summary>
        public int RefreshRate;
    }

    /// <summary>
    /// Structure describing display mode properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayModePropertiesKhr
    {
        /// <summary>
        /// A handle to the display mode described in this structure. This handle will be valid for
        /// the lifetime of the Vulkan instance.
        /// </summary>
        public long DisplayMode;
        /// <summary>
        /// Is a <see cref="DisplayModeParametersKhr"/> structure describing the display parameters
        /// associated with displayMode.
        /// </summary>
        public DisplayModeParametersKhr Parameters;
    }

    /// <summary>
    /// Structure describing capabilities of a mode and plane combination.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayPlaneCapabilitiesKhr
    {
        /// <summary>
        /// A bitmask of <see cref="DisplayPlaneAlphasKhr"/> describing the supported alpha
        /// blending modes.
        /// </summary>
        public DisplayPlaneAlphasKhr SupportedAlpha;
        /// <summary>
        /// Is the minimum source rectangle offset supported by this plane using the specified mode.
        /// </summary>
        public Offset2D MinSrcPosition;
        /// <summary>
        /// The maximum source rectangle offset supported by this plane using the specified mode. The
        /// x and y components of maxSrcPosition must each be greater than or equal to the x and y
        /// components of <see cref="MinSrcPosition"/>, respectively.
        /// </summary>
        public Offset2D MaxSrcPosition;
        /// <summary>
        /// The minimum source rectangle size supported by this plane using the specified mode.
        /// </summary>
        public Extent2D MinSrcExtent;
        /// <summary>
        /// The maximum source rectangle size supported by this plane using the specified mode.
        /// </summary>
        public Extent2D MaxSrcExtent;
        /// <summary>
        /// Has similar semantics to <see cref="MinSrcPosition"/>, but apply to the output region
        /// within the mode rather than the input region within the source image.
        /// <para>Unlike <see cref="MinSrcPosition"/>, may contain negative values.</para>
        /// </summary>
        public Offset2D MinDstPosition;
        /// <summary>
        /// Has similar semantics to <see cref="MaxSrcPosition"/>, but apply to the output region
        /// within the mode rather than the input region within the source image.
        /// <para>Unlike <see cref="MaxSrcPosition"/>, may contain negative values.</para>
        /// </summary>
        public Offset2D MaxDstPosition;
        /// <summary>
        /// Has similar semantics to <see cref="MinSrcExtent"/>, but apply to the output region
        /// within the mode rather than the input region within the source image.
        /// </summary>
        public Extent2D MinDstExtent;
        /// <summary>
        /// Has similar semantics to <see cref="MaxSrcExtent"/>, but apply to the output region
        /// within the mode rather than the input region within the source image.
        /// </summary>
        public Extent2D MaxDstExtent;
    }

    /// <summary>
    /// Structure describing the fine-grained features that can be supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceFeatures2Khr
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A structure describing the fine-grained features of the Vulkan 1.0 API.
        /// </summary>
        public PhysicalDeviceFeatures Features;
    }

    /// <summary>
    /// Structure specifying physical device properties.
    /// </summary>
    public struct PhysicalDeviceProperties2Khr
    {
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A structure describing the properties of the physical device. This structure is written
        /// with the same values as if it were written by <see cref="PhysicalDevice.GetProperties"/>.
        /// </summary>
        public PhysicalDeviceProperties Properties;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PhysicalDeviceProperties.Native Properties;
        }

        internal static void FromNative(ref Native native, out PhysicalDeviceProperties2Khr properties)
        {
            properties = new PhysicalDeviceProperties2Khr
            {
                Next = native.Next
            };
            PhysicalDeviceProperties.FromNative(ref native.Properties, out properties.Properties);
        }
    }

    /// <summary>
    /// Structure specifying image format properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FormatProperties2Khr
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A structure describing features supported by the requested format.
        /// </summary>
        public FormatProperties FormatProperties;
    }

    /// <summary>
    /// Structure specifying a image format properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageFormatProperties2Khr
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A structure in which capabilities are returned.
        /// </summary>
        public ImageFormatProperties ImageFormatProperties;
    }

    /// <summary>
    /// Structure specifying image creation parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceImageFormatInfo2Khr
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The image format, corresponding to <see cref="ImageCreateInfo.Format"/>.
        /// </summary>
        public Format Format;
        /// <summary>
        /// The image type, corresponding to <see cref="ImageCreateInfo.ImageType"/>.
        /// </summary>
        public ImageType ImageType;
        /// <summary>
        /// The image tiling, corresponding to <see cref="ImageCreateInfo.Tiling"/>.
        /// </summary>
        public ImageTiling Tiling;
        /// <summary>
        /// The intended usage of the image, corresponding to <see cref="ImageCreateInfo.Usage"/>.
        /// </summary>
        public ImageUsages Usage;
        /// <summary>
        /// A bitmask describing additional parameters of the image, corresponding to <see cref="ImageCreateInfo.Flags"/>.
        /// </summary>
        public ImageCreateFlags Flags;
    }

    /// <summary>
    /// Structure providing information about a queue family.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct QueueFamilyProperties2Khr
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A structure which is populated with the same values as in <see cref="PhysicalDevice.GetQueueFamilyProperties"/>.
        /// </summary>
        public QueueFamilyProperties QueueFamilyProperties;
    }

    /// <summary>
    /// Structure specifying physical device memory properties.
    /// </summary>
    public unsafe struct PhysicalDeviceMemoryProperties2Khr
    {
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A structure which is populated with the same values as in <see cref="PhysicalDevice.GetMemoryProperties"/>.
        /// </summary>
        public PhysicalDeviceMemoryProperties MemoryProperties;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PhysicalDeviceMemoryProperties.Native MemoryProperties;
        }

        internal static void FromNative(ref Native native, out PhysicalDeviceMemoryProperties2Khr managed)
        {
            managed.Next = native.Next;
            PhysicalDeviceMemoryProperties.FromNative(ref native.MemoryProperties, out managed.MemoryProperties);
        }
    }

    /// <summary>
    /// Structure specifying sparse image format properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SparseImageFormatProperties2Khr
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A structure which is populated with the same values as in <see cref="PhysicalDevice.GetSparseImageFormatProperties"/>.
        /// </summary>
        public SparseImageFormatProperties Properties;
    }

    /// <summary>
    /// Structure specifying sparse image format inputs.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceSparseImageFormatInfo2Khr
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The image format.
        /// </summary>
        public Format Format;
        /// <summary>
        /// The dimensionality of image.
        /// </summary>
        public ImageType ImageType;
        /// <summary>
        /// The number of samples per pixel as defined in <see cref="SampleCounts"/>.
        /// </summary>
        public SampleCounts Samples;
        /// <summary>
        /// A bitmask describing the intended usage of the image.
        /// </summary>
        public ImageUsages Usage;
        /// <summary>
        /// The tiling arrangement of the data elements in memory.
        /// </summary>
        public ImageTiling Tiling;
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
        /// Supported minimum number of images for the surface.
        /// </summary>
        public int MinImageCount;
        /// <summary>
        /// Supported maximum number of images for the surface, 0 for unlimited.
        /// </summary>
        public int MaxImageCount;
        /// <summary>
        /// Current image width and height for the surface, (0, 0) if undefined.
        /// </summary>
        public Extent2D CurrentExtent;
        /// <summary>
        /// Supported minimum image width and height for the surface.
        /// </summary>
        public Extent2D MinImageExtent;
        /// <summary>
        /// Supported maximum image width and height for the surface.
        /// </summary>
        public Extent2D MaxImageExtent;
        /// <summary>
        /// Supported maximum number of image layers for the surface.
        /// </summary>
        public int MaxImageArrayLayers;
        /// <summary>
        /// 1 or more bits representing the transforms supported.
        /// </summary>
        public SurfaceTransformsKhr SupportedTransforms;
        /// <summary>
        /// The surface's current transform relative to the device's natural orientation.
        /// </summary>
        public SurfaceTransformsKhr CurrentTransform;
        /// <summary>
        /// 1 or more bits representing the alpha compositing modes supported.
        /// </summary>
        public CompositeAlphasKhr SupportedCompositeAlpha;
        /// <summary>
        /// Supported image usage flags for the surface.
        /// </summary>
        public ImageUsages SupportedUsageFlags;
        /// <summary>
        /// Must not include <see cref="SurfaceCountersExt.VBlank"/> unless the surface queried
        /// is a display surface.
        /// </summary>
        public SurfaceCountersExt SupportedSurfaceCounters;
    }
}
