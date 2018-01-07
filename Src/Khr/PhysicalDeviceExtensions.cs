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
        /// <param name="physicalDevice">
        /// The physical device from which to query the supported features.
        /// </param>
        /// <returns>A structure in which the physical device features are returned.</returns>
        public static PhysicalDeviceFeatures2Khr GetFeatures2Khr(this PhysicalDevice physicalDevice)
        {
            PhysicalDeviceFeatures2Khr features;
            vkGetPhysicalDeviceFeatures2KHR(physicalDevice)(physicalDevice, &features);
            return features;
        }

        /// <summary>
        /// Returns properties of a physical device.
        /// </summary>
        /// <param name="physicalDevice">
        /// The handle to the physical device whose properties will be queried.
        /// </param>
        /// <returns>The structure, that will be filled with returned information.</returns>
        public static PhysicalDeviceProperties2Khr GetProperties2Khr(this PhysicalDevice physicalDevice)
        {
            PhysicalDeviceProperties2Khr.Native nativeProperties;
            vkGetPhysicalDeviceProperties2KHR(physicalDevice)(physicalDevice, &nativeProperties);
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
            vkGetPhysicalDeviceFormatProperties2KHR(physicalDevice)(physicalDevice, format, &properties);
            return properties;
        }

        /// <summary>
        /// Lists physical device's image format capabilities.
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
            Result result = vkGetPhysicalDeviceImageFormatProperties2KHR(physicalDevice)
                (physicalDevice, &imageFormatInfo, &properties);
            VulkanException.ThrowForInvalidResult(result);
            return properties;
        }

        /// <summary>
        /// Reports properties of the queues of the specified physical device.
        /// </summary>
        /// <param name="physicalDevice">
        /// The handle to the physical device whose properties will be queried.
        /// </param>
        /// <returns>An array of <see cref="QueueFamilyProperties2Khr"/> structures.</returns>
        public static QueueFamilyProperties2Khr[] GetQueueFamilyProperties2Khr(this PhysicalDevice physicalDevice)
        {
            int count;
            vkGetPhysicalDeviceQueueFamilyProperties2KHR(physicalDevice)(physicalDevice, &count, null);

            var properties = new QueueFamilyProperties2Khr[count];
            fixed (QueueFamilyProperties2Khr* propertiesPtr = properties)
                vkGetPhysicalDeviceQueueFamilyProperties2KHR(physicalDevice)(physicalDevice, &count, propertiesPtr);

            return properties;
        }

        /// <summary>
        /// Reports memory information for the specified physical device.
        /// </summary>
        /// <param name="physicalDevice">The handle to the device to query.</param>
        /// <returns>The structure in which the properties are returned.</returns>
        public static PhysicalDeviceMemoryProperties2Khr GetMemoryProperties2Khr(this PhysicalDevice physicalDevice)
        {
            var nativeProperties = new PhysicalDeviceMemoryProperties2Khr.Native();
            vkGetPhysicalDeviceMemoryProperties2KHR(physicalDevice)(physicalDevice, ref nativeProperties);
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
            vkGetPhysicalDeviceSparseImageFormatProperties2KHR(physicalDevice)(physicalDevice, &formatInfo, &count, null);

            var properties = new SparseImageFormatProperties2Khr[count];
            fixed (SparseImageFormatProperties2Khr* propertiesPtr = properties)
                vkGetPhysicalDeviceSparseImageFormatProperties2KHR(physicalDevice)(physicalDevice, &formatInfo, &count, propertiesPtr);
            return properties;
        }

        /// <summary>
        /// Reports capabilities of a surface on a physical device.
        /// </summary>
        /// <param name="physicalDevice">
        /// The physical device that will be associated with the swapchain to be created, as
        /// described for <see cref="DeviceExtensions.CreateSwapchainKhr"/>.
        /// </param>
        /// <param name="surfaceInfo">
        /// Describes the surface and other fixed parameters that would be consumed by <see cref="DeviceExtensions.CreateSwapchainKhr"/>.
        /// </param>
        /// <returns>A structure in which the capabilities are returned.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceCapabilities2Khr GetSurfaceCapabilities2Khr(this PhysicalDevice physicalDevice, PhysicalDeviceSurfaceInfo2Khr surfaceInfo)
        {
            surfaceInfo.Prepare();
            SurfaceCapabilities2Khr capabilities;
            Result result = vkGetPhysicalDeviceSurfaceCapabilities2KHR(physicalDevice)(physicalDevice, &surfaceInfo, &capabilities);
            VulkanException.ThrowForInvalidResult(result);
            capabilities.Prepare();
            return capabilities;
        }

        /// <summary>
        /// Query color formats supported by surface.
        /// </summary>
        /// <param name="physicalDevice">
        /// The physical device that will be associated with the swapchain to be created, as
        /// described for <see cref="DeviceExtensions.CreateSwapchainKhr"/>.
        /// </param>
        /// <param name="surfaceInfo">
        /// Describes the surface and other fixed parameters that would be consumed by <see cref="DeviceExtensions.CreateSwapchainKhr"/>.
        /// </param>
        /// <returns>An array of <see cref="SurfaceFormat2Khr"/> structures.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceFormat2Khr[] GetSurfaceFormats2Khr(this PhysicalDevice physicalDevice, PhysicalDeviceSurfaceInfo2Khr surfaceInfo)
        {
            surfaceInfo.Prepare();

            int count;
            Result result = vkGetPhysicalDeviceSurfaceFormats2KHR(physicalDevice)(physicalDevice, &surfaceInfo, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var formats = new SurfaceFormat2Khr[count];
            fixed (SurfaceFormat2Khr* formatsPtr = formats)
                result = vkGetPhysicalDeviceSurfaceFormats2KHR(physicalDevice)(physicalDevice, &surfaceInfo, &count, formatsPtr);
            VulkanException.ThrowForInvalidResult(result);
            for (int i = 0; i < count; i++)
                formats[i].Prepare();

            return formats;
        }

        /// <summary>
        /// Query the external handle types supported by buffers.
        /// </summary>
        /// <param name="physicalDevice">The physical device from which to query the buffer capabilities.</param>
        /// <param name="externalBufferInfo">
        /// Structure, describing the parameters that would be consumed by <see cref="Device.CreateBuffer"/>.
        /// </param>
        /// <returns>A structure in which capabilities are returned.</returns>
        public static ExternalBufferPropertiesKhr GetExternalBufferPropertiesKhr(this PhysicalDevice physicalDevice,
            PhysicalDeviceExternalBufferInfoKhr externalBufferInfo)
        {
            ExternalBufferPropertiesKhr properties;
            vkGetPhysicalDeviceExternalBufferPropertiesKHR(physicalDevice)(physicalDevice, &externalBufferInfo, &properties);
            return properties;
        }

        /// <summary>
        /// Function for querying external semaphore handle capabilities.
        /// <para>Semaphores may support import and export of external semaphore handles.</para>
        /// </summary>
        /// <param name="physicalDevice">
        /// The physical device from which to query the semaphore capabilities.
        /// </param>
        /// <param name="externalSemaphoreInfo">
        /// Describes the parameters that would be consumed by <see cref="Device.CreateSemaphore"/>.
        /// </param>
        /// <returns>Structure in which capabilities are returned.</returns>
        public static ExternalSemaphorePropertiesKhr GetExternalSemaphorePropertiesKhx(this PhysicalDevice physicalDevice,
            PhysicalDeviceExternalSemaphoreInfoKhr externalSemaphoreInfo)
        {
            ExternalSemaphorePropertiesKhr properties;
            vkGetPhysicalDeviceExternalSemaphorePropertiesKHR(physicalDevice)(physicalDevice, &externalSemaphoreInfo, &properties);
            return properties;
        }

        private delegate Result vkGetPhysicalDeviceSurfaceSupportKHRDelegate(IntPtr physicalDevice, int queueFamilyIndex, long surface, Bool* supported);
        private static readonly vkGetPhysicalDeviceSurfaceSupportKHRDelegate vkGetPhysicalDeviceSurfaceSupportKHR = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceSurfaceSupportKHRDelegate>(nameof(vkGetPhysicalDeviceSurfaceSupportKHR));

        private delegate Bool vkGetPhysicalDeviceWaylandPresentationSupportKHRDelegate(IntPtr physicalDevice, int queueFamilyIndex, IntPtr* display);
        private static readonly vkGetPhysicalDeviceWaylandPresentationSupportKHRDelegate vkGetPhysicalDeviceWaylandPresentationSupportKHR = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceWaylandPresentationSupportKHRDelegate>(nameof(vkGetPhysicalDeviceWaylandPresentationSupportKHR));

        private delegate Bool vkGetPhysicalDeviceWin32PresentationSupportKHRDelegate(IntPtr physicalDevice, int queueFamilyIndex);
        private static readonly vkGetPhysicalDeviceWin32PresentationSupportKHRDelegate vkGetPhysicalDeviceWin32PresentationSupportKHR = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceWin32PresentationSupportKHRDelegate>(nameof(vkGetPhysicalDeviceWin32PresentationSupportKHR));

        private delegate Bool vkGetPhysicalDeviceXlibPresentationSupportKHRDelegate(IntPtr physicalDevice, int queueFamilyIndex, IntPtr* dpy, IntPtr visualId);
        private static readonly vkGetPhysicalDeviceXlibPresentationSupportKHRDelegate vkGetPhysicalDeviceXlibPresentationSupportKHR = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceXlibPresentationSupportKHRDelegate>(nameof(vkGetPhysicalDeviceXlibPresentationSupportKHR));

        private delegate Bool vkGetPhysicalDeviceXcbPresentationSupportKHRDelegate(IntPtr physicalDevice, int queueFamilyIndex, IntPtr* connection, IntPtr visual_id);
        private static readonly vkGetPhysicalDeviceXcbPresentationSupportKHRDelegate vkGetPhysicalDeviceXcbPresentationSupportKHR = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceXcbPresentationSupportKHRDelegate>(nameof(vkGetPhysicalDeviceXcbPresentationSupportKHR));

        private delegate Result vkGetPhysicalDeviceSurfaceCapabilitiesKHRDelegate(IntPtr physicalDevice, long surface, SurfaceCapabilitiesKhr* surfaceCapabilities);
        private static readonly vkGetPhysicalDeviceSurfaceCapabilitiesKHRDelegate vkGetPhysicalDeviceSurfaceCapabilitiesKHR = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceSurfaceCapabilitiesKHRDelegate>(nameof(vkGetPhysicalDeviceSurfaceCapabilitiesKHR));

        private delegate Result vkGetPhysicalDeviceSurfaceFormatsKHRDelegate(IntPtr physicalDevice, long surface, int* surfaceFormatCount, SurfaceFormatKhr* surfaceFormats);
        private static readonly vkGetPhysicalDeviceSurfaceFormatsKHRDelegate vkGetPhysicalDeviceSurfaceFormatsKHR = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceSurfaceFormatsKHRDelegate>(nameof(vkGetPhysicalDeviceSurfaceFormatsKHR));

        private delegate Result vkGetPhysicalDeviceSurfacePresentModesKHRDelegate(IntPtr physicalDevice, long surface, int* presentModeCount, PresentModeKhr* presentModes);
        private static readonly vkGetPhysicalDeviceSurfacePresentModesKHRDelegate vkGetPhysicalDeviceSurfacePresentModesKHR = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceSurfacePresentModesKHRDelegate>(nameof(vkGetPhysicalDeviceSurfacePresentModesKHR));

        private delegate Result vkGetPhysicalDeviceDisplayPropertiesKHRDelegate(IntPtr physicalDevice, int* propertyCount, DisplayPropertiesKhr.Native* properties);
        private static readonly vkGetPhysicalDeviceDisplayPropertiesKHRDelegate vkGetPhysicalDeviceDisplayPropertiesKHR = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceDisplayPropertiesKHRDelegate>(nameof(vkGetPhysicalDeviceDisplayPropertiesKHR));

        private delegate Result vkGetPhysicalDeviceDisplayPlanePropertiesKHRDelegate(IntPtr physicalDevice, int* propertyCount, DisplayPlanePropertiesKhr* properties);
        private static readonly vkGetPhysicalDeviceDisplayPlanePropertiesKHRDelegate vkGetPhysicalDeviceDisplayPlanePropertiesKHR = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceDisplayPlanePropertiesKHRDelegate>(nameof(vkGetPhysicalDeviceDisplayPlanePropertiesKHR));

        private delegate Result vkGetDisplayPlaneSupportedDisplaysKHRDelegate(IntPtr physicalDevice, int planeIndex, int* displayCount, long* displays);
        private static readonly vkGetDisplayPlaneSupportedDisplaysKHRDelegate vkGetDisplayPlaneSupportedDisplaysKHR = VulkanLibrary.GetStaticProc<vkGetDisplayPlaneSupportedDisplaysKHRDelegate>(nameof(vkGetDisplayPlaneSupportedDisplaysKHR));

        private delegate Bool vkGetPhysicalDeviceMirPresentationSupportKHRDelegate(IntPtr physicalDevice, int queueFamilyIndex, IntPtr* connection);
        private static readonly vkGetPhysicalDeviceMirPresentationSupportKHRDelegate vkGetPhysicalDeviceMirPresentationSupportKHR = VulkanLibrary.GetStaticProc<vkGetPhysicalDeviceMirPresentationSupportKHRDelegate>(nameof(vkGetPhysicalDeviceMirPresentationSupportKHR));

        private delegate Result vkGetDisplayPlaneCapabilitiesKHRDelegate(IntPtr physicalDevice, long mode, int planeIndex, DisplayPlaneCapabilitiesKhr* capabilities);
        private static readonly vkGetDisplayPlaneCapabilitiesKHRDelegate vkGetDisplayPlaneCapabilitiesKHR = VulkanLibrary.GetStaticProc<vkGetDisplayPlaneCapabilitiesKHRDelegate>(nameof(vkGetDisplayPlaneCapabilitiesKHR));

        private delegate void vkGetPhysicalDeviceFeatures2KHRDelegate(IntPtr physicalDevice, PhysicalDeviceFeatures2Khr* features);
        private static vkGetPhysicalDeviceFeatures2KHRDelegate vkGetPhysicalDeviceFeatures2KHR(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDeviceFeatures2KHRDelegate>(physicalDevice, nameof(vkGetPhysicalDeviceFeatures2KHR));

        private delegate void vkGetPhysicalDeviceProperties2KHRDelegate(IntPtr physicalDevice, PhysicalDeviceProperties2Khr.Native* properties);
        private static vkGetPhysicalDeviceProperties2KHRDelegate vkGetPhysicalDeviceProperties2KHR(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDeviceProperties2KHRDelegate>(physicalDevice, nameof(vkGetPhysicalDeviceProperties2KHR));

        private delegate void vkGetPhysicalDeviceFormatProperties2KHRDelegate(IntPtr physicalDevice, Format format, FormatProperties2Khr* formatProperties);
        private static vkGetPhysicalDeviceFormatProperties2KHRDelegate vkGetPhysicalDeviceFormatProperties2KHR(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDeviceFormatProperties2KHRDelegate>(physicalDevice, nameof(vkGetPhysicalDeviceFormatProperties2KHR));

        private delegate Result vkGetPhysicalDeviceImageFormatProperties2KHRDelegate(IntPtr physicalDevice, PhysicalDeviceImageFormatInfo2Khr* imageFormatInfo, ImageFormatProperties2Khr* imageFormatProperties);
        private static vkGetPhysicalDeviceImageFormatProperties2KHRDelegate vkGetPhysicalDeviceImageFormatProperties2KHR(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDeviceImageFormatProperties2KHRDelegate>(physicalDevice, nameof(vkGetPhysicalDeviceImageFormatProperties2KHR));

        private delegate void vkGetPhysicalDeviceQueueFamilyProperties2KHRDelegate(IntPtr physicalDevice, int* queueFamilyPropertyCount, QueueFamilyProperties2Khr* queueFamilyProperties);
        private static vkGetPhysicalDeviceQueueFamilyProperties2KHRDelegate vkGetPhysicalDeviceQueueFamilyProperties2KHR(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDeviceQueueFamilyProperties2KHRDelegate>(physicalDevice, nameof(vkGetPhysicalDeviceQueueFamilyProperties2KHR));

        private delegate void vkGetPhysicalDeviceMemoryProperties2KHRDelegate(IntPtr physicalDevice, ref PhysicalDeviceMemoryProperties2Khr.Native memoryProperties);
        private static vkGetPhysicalDeviceMemoryProperties2KHRDelegate vkGetPhysicalDeviceMemoryProperties2KHR(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDeviceMemoryProperties2KHRDelegate>(physicalDevice, nameof(vkGetPhysicalDeviceMemoryProperties2KHR));

        private delegate void vkGetPhysicalDeviceSparseImageFormatProperties2KHRDelegate(IntPtr physicalDevice, PhysicalDeviceSparseImageFormatInfo2Khr* formatInfo, int* propertyCount, SparseImageFormatProperties2Khr* properties);
        private static vkGetPhysicalDeviceSparseImageFormatProperties2KHRDelegate vkGetPhysicalDeviceSparseImageFormatProperties2KHR(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDeviceSparseImageFormatProperties2KHRDelegate>(physicalDevice, nameof(vkGetPhysicalDeviceSparseImageFormatProperties2KHR));

        private delegate Result vkGetPhysicalDeviceSurfaceCapabilities2KHRDelegate(IntPtr physicalDevice, PhysicalDeviceSurfaceInfo2Khr* surfaceInfo, SurfaceCapabilities2Khr* surfaceCapabilities);
        private static vkGetPhysicalDeviceSurfaceCapabilities2KHRDelegate vkGetPhysicalDeviceSurfaceCapabilities2KHR(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDeviceSurfaceCapabilities2KHRDelegate>(physicalDevice, nameof(vkGetPhysicalDeviceSurfaceCapabilities2KHR));
        
        private delegate Result vkGetPhysicalDeviceSurfaceFormats2KHRDelegate(IntPtr physicalDevice, PhysicalDeviceSurfaceInfo2Khr* surfaceInfo, int* surfaceFormatCount, SurfaceFormat2Khr* surfaceFormats);
        private static vkGetPhysicalDeviceSurfaceFormats2KHRDelegate vkGetPhysicalDeviceSurfaceFormats2KHR(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDeviceSurfaceFormats2KHRDelegate>(physicalDevice, nameof(vkGetPhysicalDeviceSurfaceFormats2KHR));

        private delegate void vkGetPhysicalDeviceExternalBufferPropertiesKHRDelegate(IntPtr physicalDevice, PhysicalDeviceExternalBufferInfoKhr* externalBufferInfo, ExternalBufferPropertiesKhr* externalBufferProperties);
        private static vkGetPhysicalDeviceExternalBufferPropertiesKHRDelegate vkGetPhysicalDeviceExternalBufferPropertiesKHR(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDeviceExternalBufferPropertiesKHRDelegate>(physicalDevice, nameof(vkGetPhysicalDeviceExternalBufferPropertiesKHR));

        private delegate void vkGetPhysicalDeviceExternalSemaphorePropertiesKHRDelegate(IntPtr physicalDevice, PhysicalDeviceExternalSemaphoreInfoKhr* externalSemaphoreInfo, ExternalSemaphorePropertiesKhr* externalSemaphoreProperties);
        private static vkGetPhysicalDeviceExternalSemaphorePropertiesKHRDelegate vkGetPhysicalDeviceExternalSemaphorePropertiesKHR(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDeviceExternalSemaphorePropertiesKHRDelegate>(physicalDevice, nameof(vkGetPhysicalDeviceExternalSemaphorePropertiesKHR));

        private static TDelegate GetProc<TDelegate>(PhysicalDevice physicalDevice, string name) where TDelegate : class => physicalDevice.Parent.GetProc<TDelegate>(name);
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
        /// Contains the smallest valid swapchain extent for the surface on the specified device.
        /// </summary>
        public Extent2D MinImageExtent;
        /// <summary>
        /// Contains the largest valid swapchain extent for the surface on the specified device.
        /// <para>
        /// The width and height of the extent will each be greater than or equal to the
        /// corresponding width and height of <see cref="MinImageExtent"/>.
        /// </para>
        /// <para>
        /// The width and height of the extent will each be greater than or equal to the
        /// corresponding width and height of <see cref="CurrentExtent"/>, unless <see
        /// cref="CurrentExtent"/> has the special value described above.
        /// </para>
        /// </summary>
        public Extent2D MaxImageExtent;
        /// <summary>
        /// The maximum number of layers presentable images can have for a swapchain created for this
        /// device and surface, and will be at least one.
        /// </summary>
        public int MaxImageArrayLayers;
        /// <summary>
        /// A bitmask of <see cref="SurfaceTransformsKhr"/>, indicating the presentation transforms
        /// supported for the surface on the specified device.
        /// <para>At least one bit will be set.</para>
        /// </summary>
        public SurfaceTransformsKhr SupportedTransforms;
        /// <summary>
        /// Indicates the surface's current transform relative to the presentation engine's natural orientation.
        /// </summary>
        public SurfaceTransformsKhr CurrentTransform;
        /// <summary>
        /// A bitmask of <see cref="CompositeAlphasKhr"/>, representing the alpha compositing modes
        /// supported by the presentation engine for the surface on the specified device, and at
        /// least one bit will be set. Opaque composition can be achieved in any alpha compositing
        /// mode by either using an image format that has no alpha component, or by ensuring that all
        /// pixels in the presentable images have an alpha value of 1.0.
        /// </summary>
        public CompositeAlphasKhr SupportedCompositeAlpha;
        /// <summary>
        /// A bitmask of <see cref="ImageUsages"/> representing the ways the application can use the
        /// presentable images of a swapchain created for the surface on the specified device.
        /// <para>
        /// <see cref="ImageUsages.ColorAttachment"/> must be included in the set but implementations
        /// may support additional usages.
        /// </para>
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
        /// A <see cref="Format"/> that is compatible with the specified surface.
        /// </summary>
        public Format Format;
        /// <summary>
        /// A presentation <see cref="ColorSpaceKhr"/> that is compatible with the surface.
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
        /// If not <c>null</c>, the memory it points to must remain accessible as long as display is valid.
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
        /// A bitmask of <see cref="DisplayPlaneAlphasKhr"/> describing the supported alpha blending modes.
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
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
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
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
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
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure. The <see
        /// cref="Next"/> chain is used to allow the specification of additional capabilities to be
        /// returned from <see cref="PhysicalDeviceExtensions.GetImageFormatProperties2Khr"/>.
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
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure. The <see
        /// cref="Next"/> chain is used to provide additional image parameters to <see cref="PhysicalDeviceExtensions.GetImageFormatProperties2Khr"/>.
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
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
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
    public struct PhysicalDeviceMemoryProperties2Khr
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
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
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
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
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
        /// The number of samples per texel as defined in <see cref="SampleCounts"/>.
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
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
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
        /// Must not include <see cref="SurfaceCountersExt.VBlank"/> unless the surface queried is a
        /// display surface.
        /// </summary>
        public SurfaceCountersExt SupportedSurfaceCounters;
    }

    /// <summary>
    /// Structure describing push descriptor limits that can be supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDevicePushDescriptorPropertiesKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The maximum number of descriptors that can be used in a descriptor set created with <see
        /// cref="DescriptorSetLayoutCreateFlags.PushDescriptorKhr"/> set.
        /// </summary>
        public int MaxPushDescriptors;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalDevicePushDescriptorPropertiesKhr"/> structure.
        /// </summary>
        /// <param name="maxPushDescriptors">
        /// The maximum number of descriptors that can be used in a descriptor set created with <see
        /// cref="DescriptorSetLayoutCreateFlags.PushDescriptorKhr"/> set.
        /// </param>
        /// <param name="next">Pointer to next structure.</param>
        public PhysicalDevicePushDescriptorPropertiesKhr(int maxPushDescriptors, IntPtr next = default(IntPtr))
        {
            Type = StructureType.PhysicalDevicePushDescriptorPropertiesKhr;
            Next = next;
            MaxPushDescriptors = maxPushDescriptors;
        }
    }

    /// <summary>
    /// Structure specifying a surface and related swapchain creation parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceSurfaceInfo2Khr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The <see cref="SurfaceKhr"/> that will be associated with the swapchain.
        /// </summary>
        public long Surface;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalDeviceSurfaceInfo2Khr"/> structure.
        /// </summary>
        /// <param name="surface">
        /// The <see cref="SurfaceKhr"/> that will be associated with the swapchain.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public PhysicalDeviceSurfaceInfo2Khr(SurfaceKhr surface, IntPtr next = default(IntPtr))
        {
            Type = StructureType.PhysicalDeviceSurfaceInfo2Khr;
            Next = next;
            Surface = surface;
        }

        internal void Prepare()
        {
            Type = StructureType.PhysicalDeviceSurfaceInfo2Khr;
        }
    }

    /// <summary>
    /// Structure describing capabilities of a surface.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SurfaceCapabilities2Khr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Describes the capabilities of the specified surface.
        /// </summary>
        public SurfaceCapabilitiesKhr SurfaceCapabilities;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceCapabilities2Khr"/> structure.
        /// </summary>
        /// <param name="surfaceCapabilities">Describes the capabilities of the specified surface.</param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public SurfaceCapabilities2Khr(SurfaceCapabilitiesKhr surfaceCapabilities, IntPtr next = default(IntPtr))
        {
            Type = StructureType.SurfaceCapabilities2Khr;
            Next = next;
            SurfaceCapabilities = surfaceCapabilities;
        }

        internal void Prepare()
        {
            Type = StructureType.SurfaceCapabilities2Khr;
        }
    }

    /// <summary>
    /// Structure describing a supported swapchain format tuple.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SurfaceFormat2Khr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Describes a format-color space pair that is compatible with the specified surface.
        /// </summary>
        public SurfaceFormatKhr SurfaceFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceFormat2Khr"/> structure.
        /// </summary>
        /// <param name="surfaceFormat">
        /// Describes a format-color space pair that is compatible with the specified surface.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public SurfaceFormat2Khr(SurfaceFormatKhr surfaceFormat, IntPtr next = default(IntPtr))
        {
            Type = StructureType.SurfaceFormat2Khr;
            Next = next;
            SurfaceFormat = surfaceFormat;
        }

        internal void Prepare()
        {
            Type = StructureType.SurfaceFormat2Khr;
        }
    }

    /// <summary>
    /// Structure describing capabilities of a surface for shared presentation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SharedPresentSurfaceCapabilitiesKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bitmask representing the ways the application can use the shared presentable image from
        /// a swapchain created with <see cref="PresentModeKhr"/> set to <see
        /// cref="PresentModeKhr.SharedDemandRefreshKhr"/> or <see
        /// cref="PresentModeKhr.SharedContinuousRefreshKhr"/> for the surface on the specified device.
        /// <para>
        /// <see cref="ImageUsages.ColorAttachment"/> must be included in the set but implementations
        /// may support additional usages.
        /// </para>
        /// </summary>
        public ImageUsages SharedPresentSupportedUsages;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedPresentSurfaceCapabilitiesKhr"/> structure.
        /// </summary>
        /// <param name="sharedPresentSupportedUsages">
        /// A bitmask representing the ways the application can use the shared presentable image from
        /// a swapchain created with <see cref="PresentModeKhr"/> set to <see
        /// cref="PresentModeKhr.SharedDemandRefreshKhr"/> or <see
        /// cref="PresentModeKhr.SharedContinuousRefreshKhr"/> for the surface on the specified device.
        /// <para>
        /// <see cref="ImageUsages.ColorAttachment"/> must be included in the set but implementations
        /// may support additional usages.
        /// </para>
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public SharedPresentSurfaceCapabilitiesKhr(ImageUsages sharedPresentSupportedUsages, IntPtr next = default(IntPtr))
        {
            Type = StructureType.SharedPresentSurfaceCapabilitiesKhr;
            Next = next;
            SharedPresentSupportedUsages = sharedPresentSupportedUsages;
        }

        internal void Prepare()
        {
            Type = StructureType.SharedPresentSurfaceCapabilitiesKhr;
        }
    }

    /// <summary>
    /// Structure specifying external image creation parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceExternalImageFormatInfoKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bit indicating a memory handle type that will be used with the memory associated with
        /// the image.
        /// </summary>
        public ExternalMemoryHandleTypesKhr HandleType;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalDeviceExternalImageFormatInfoKhr"/> structure.
        /// </summary>
        /// <param name="handleType">
        /// A bit indicating a memory handle type that will be used with the memory associated with
        /// the image.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public PhysicalDeviceExternalImageFormatInfoKhr(ExternalMemoryHandleTypesKhr handleType,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.PhysicalDeviceExternalImageFormatInfoKhr;
            Next = next;
            HandleType = handleType;
        }
    }

    /// <summary>
    /// Structure specifying external memory handle type capabilities.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalMemoryPropertiesKhr
    {
        /// <summary>
        /// A bitmask describing the features of handle type.
        /// </summary>
        public ExternalMemoryFeaturesKhr ExternalMemoryFeatures;
        /// <summary>
        /// A bitmask specifying handle types that can be used to import objects from which handle
        /// type can be exported.
        /// </summary>
        public ExternalMemoryHandleTypesKhr ExportFromImportedHandleTypes;
        /// <summary>
        /// A bitmask specifying handle types which can be specified at the same time as handle type
        /// when creating an image compatible with external memory.
        /// </summary>
        public ExternalMemoryHandleTypesKhr CompatibleHandleTypes;
    }

    /// <summary>
    /// Structure specifying supported external handle capabilities.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalBufferPropertiesKhr
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies various capabilities of the external handle type when used with the specified
        /// buffer creation parameters.
        /// </summary>
        public ExternalMemoryPropertiesKhr ExternalMemoryProperties;
    }

    /// <summary>
    /// Structure specifying buffer creation parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceExternalBufferInfoKhr
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bitmask describing additional parameters of the buffer, corresponding to <see cref="BufferCreateInfo.Flags"/>.
        /// </summary>
        public BufferCreateFlags Flags;
        /// <summary>
        /// A bitmask describing the intended usage of the buffer, corresponding to <see cref="BufferCreateInfo.Usage"/>.
        /// </summary>
        public BufferUsages Usage;
        /// <summary>
        /// A bit indicating a memory handle type that will be used with the memory associated with
        /// the buffer.
        /// </summary>
        public ExternalMemoryHandleTypesKhr HandleType;
    }

    /// <summary>
    /// Structure specifying supported external handle properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalImageFormatPropertiesKhr
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies various capabilities of the external handle type when used with the specified
        /// image creation parameters.
        /// </summary>
        public ExternalMemoryPropertiesKhr ExternalMemoryProperties;
    }

    /// <summary>
    /// Bitmask specifying external memory handle types.
    /// </summary>
    [Flags]
    public enum ExternalMemoryHandleTypesKhr
    {
        /// <summary>
        /// Specifies a POSIX file descriptor handle that has only limited valid usage outside of
        /// Vulkan and other compatible APIs.
        /// <para>
        /// It must be compatible with the POSIX system calls <c>dup</c>, <c>dup2</c>, <c>close</c>,
        /// and the non-standard system call <c>dup3</c>. Additionally, it must be transportable over
        /// a socket using an <c>SCM_RIGHTS</c> control message.
        /// </para>
        /// <para>
        /// It owns a reference to the underlying memory resource represented by its Vulkan memory object.
        /// </para>
        /// </summary>
        OpaqueFd = 1 << 0,
        /// <summary>
        /// Specifies an NT handle that has only limited valid usage outside of Vulkan and other
        /// compatible APIs.
        /// <para>
        /// It must: be compatible with the functions <c>DuplicateHandle</c>, <c>CloseHandle</c>,
        /// <c>CompareObjectHandles</c>, <c>GetHandleInformation</c>, and <c>SetHandleInformation</c>.
        /// </para>
        /// <para>
        /// It owns a reference to the underlying memory resource represented by its Vulkan memory object.
        /// </para>
        /// </summary>
        OpaqueWin32 = 1 << 1,
        /// <summary>
        /// Specifies a global share handle that has only limited valid usage outside of Vulkan and
        /// other compatible APIs.
        /// <para>It is not compatible with any native APIs.</para>
        /// <para>
        /// It does not own own a reference to the underlying memory resource represented its Vulkan
        /// memory object, and will therefore become invalid when all Vulkan memory objects
        /// associated with it are destroyed.
        /// </para>
        /// </summary>
        OpaqueWin32Kmt = 1 << 2,
        /// <summary>
        /// Specifies an NT handle returned by <c>IDXGIResource1::CreateSharedHandle</c> referring to
        /// a Direct3D 10 or 11 texture resource.
        /// <para>It owns a reference to the memory used by the Direct3D resource.</para>
        /// </summary>
        D3D11Texture = 1 << 3,
        /// <summary>
        /// Specifies a global share handle returned by <c>IDXGIResource::GetSharedHandle</c>
        /// referring to a Direct3D 10 or 11 texture resource.
        /// <para>
        /// It does not own own a reference to the underlying Direct3D resource, and will therefore
        /// become invalid when all Vulkan memory objects and Direct3D resources associated with it
        /// are destroyed.
        /// </para>
        /// </summary>
        D3D11TextureKmt = 1 << 4,
        /// <summary>
        /// Specifies an NT handle returned by <c>ID3D12Device::CreateSharedHandle</c> referring to a
        /// Direct3D 12 heap resource.
        /// <para>It owns a reference to the resources used by the Direct3D heap.</para>
        /// </summary>
        D3D12Heap = 1 << 5,
        /// <summary>
        /// Specifies an NT handle returned by <c>ID3D12Device::CreateSharedHandle</c> referring to a
        /// Direct3D 12 committed resource.
        /// <para>It owns a reference to the memory used by the Direct3D resource.</para>
        /// </summary>
        D3D12Resource = 1 << 6,
        /// <summary>
        /// Is a file descriptor for a Linux DmaBuf. It owns a reference to the underlying memory
        /// resource represented by its Vulkan memory object.
        /// </summary>
        DmaBufExt = 1 << 9,
        /// <summary>
        /// Specifies a host pointer returned by a host memory allocation command. It does not own a
        /// reference to the underlying memory resource, and will therefore become invalid if the
        /// host memory is freed.
        /// </summary>
        HostAllocationExt = 1 << 7,
        /// <summary>
        /// Specifies a host pointer to host mapped foreign Memory. It does not own a reference to
        /// the underlying memory resource, and will therefore become invalid if the foreign memory
        /// is unmapped or otherwise becomes no longer available.
        /// </summary>
        HostMappedForeignMemoryExt = 1 << 8
    }

    /// <summary>
    /// Bitmask specifying features of an external memory handle type.
    /// </summary>
    [Flags]
    public enum ExternalMemoryFeaturesKhr
    {
        /// <summary>
        /// Specifies that images or buffers created with the specified parameters and handle type
        /// must use the mechanisms defined in the "VK_NV_dedicated_allocation" extension to to
        /// create (or import) a dedicated allocation for the image or buffer.
        /// </summary>
        DedicatedOnly = 1 << 0,
        /// <summary>
        /// Specifies that handles of this type can be exported from Vulkan memory objects.
        /// </summary>
        Exportable = 1 << 1,
        /// <summary>
        /// Specifies that handles of this type can be imported as Vulkan memory objects.
        /// </summary>
        Importable = 1 << 2
    }

    /// <summary>
    /// Structure specifying semaphore creation parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceExternalSemaphoreInfoKhr
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bit indicating an external semaphore handle type for which capabilities will be returned.
        /// </summary>
        public ExternalSemaphoreHandleTypesKhr HandleType;
    }

    /// <summary>
    /// Structure describing supported external semaphore handle features.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalSemaphorePropertiesKhr
    {
        internal StructureType Type;

        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bitmask specifying handle types that can be used to import objects from which
        /// handleType can be exported.
        /// </summary>
        public ExternalSemaphoreHandleTypesKhr ExportFromImportedHandleTypes;
        /// <summary>
        /// A bitmask specifying handle types which can be specified at the same time as handleType
        /// when creating a semaphore.
        /// </summary>
        public ExternalSemaphoreHandleTypesKhr CompatibleHandleTypes;
        /// <summary>
        /// A bitmask describing the features of handle type.
        /// </summary>
        public ExternalSemaphoreFeaturesKhr ExternalSemaphoreFeatures;
    }

    /// <summary>
    /// Bitfield describing features of an external semaphore handle type.
    /// </summary>
    [Flags]
    public enum ExternalSemaphoreFeaturesKhr
    {
        /// <summary>
        /// Specifies that handles of this type can be exported from Vulkan semaphore objects.
        /// </summary>
        Exportable = 1 << 0,
        /// <summary>
        /// Specifies that handles of this type can be imported as Vulkan semaphore objects.
        /// </summary>
        Importable = 1 << 1
    }

    /// <summary>
    /// Structure describing variable pointers features that can be supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceVariablePointerFeaturesKhr
    {
        public StructureType Type;
        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        public Bool VariablePointersStorageBuffer;
        public Bool VariablePointers;
    }

    /// <summary>
    /// Bitmask specifying additional parameters of semaphore payload import.
    /// </summary>
    [Flags]
    public enum SemaphoreImportFlagsKhr
    {
        /// <summary>
        /// Indicates that the semaphore payload will be imported only temporarily, regardless of the
        /// permanence of handleType.
        /// </summary>
        Temporary = 1 << 0
    }
    
    /// <summary>
    /// Bitmask of valid external fence handle types.
    /// </summary>
    [Flags]
    public enum ExternalFenceHandleTypeFlagsKhr
    {
        /// <summary>
        /// Indicates a POSIX file descriptor handle that has only limited valid usage outside of
        /// Vulkan and other compatible APIs. It must be compatible with the POSIX system calls
        /// <c>dup</c>, <c>dup2</c>, <c>close</c>, and the non-standard system call <c>dup3</c>.
        /// Additionally, it must be transportable over a socket using an <c>SCMRIGHTS</c> control
        /// message. It owns a reference to the underlying synchronization primitive represented by
        /// its Vulkan fence object.
        /// </summary>
        OpaqueFd = 1 << 0,
        /// <summary>
        /// Indicates an NT handle that has only limited valid usage outside of Vulkan and other
        /// compatible APIs. It must be compatible with the functions <c>DuplicateHandle</c>,
        /// <c>CloseHandle</c>, <c>CompareObjectHandles</c>, <c>GetHandleInformation</c>, and
        /// <c>SetHandleInformation</c>. It owns a reference to the underlying synchronization
        /// primitive represented by its Vulkan fence object.
        /// </summary>
        OpaqueWin32 = 1 << 1,
        /// <summary>
        /// Indicates a global share handle that has only limited valid usage outside of Vulkan and
        /// other compatible APIs. It is not compatible with any native APIs. It does not own a
        /// reference to the underlying synchronization primitive represented by its Vulkan fence
        /// object, and will therefore become invalid when all Vulkan fence objects associated with
        /// it are destroyed.
        /// </summary>
        OpaqueWin32Kmt = 1 << 2,
        /// <summary>
        /// Indicates a POSIX file descriptor handle to a Linux Sync File or Android Fence. It can be
        /// used with any native API accepting a valid sync file or fence as input. It owns a
        /// reference to the underlying synchronization primitive associated with the file
        /// descriptor. Implementations which support importing this handle type must accept any type
        /// of sync or fence FD supported by the native system they are running on.
        /// </summary>
        SyncFd = 1 << 3
    }
    
    /// <summary>
    /// Bitfield describing features of an external fence handle type.
    /// </summary>
    [Flags]
    public enum ExternalFenceFeatureFlagsKhr
    {
        /// <summary>
        /// Indicates handles of this type can be exported from Vulkan fence objects.
        /// </summary>
        Exportable = 1 << 0,
        /// <summary>
        /// Indicates handles of this type can be imported to Vulkan fence objects.
        /// </summary>
        Importable = 1 << 1
    }
    
    /// <summary>
    /// Bitmask specifying additional parameters of fence payload import.
    /// </summary>
    [Flags]
    public enum FenceImportFlagsKhr
    {
        /// <summary>
        /// Specifies that the fence payload will be imported only temporarily, regardless of the
        /// permanence of handle type.
        /// </summary>
        Temporary = 1 << 0
    }

    /// <summary>
    /// Structure specifying IDs related to the physical device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct PhysicalDeviceIdPropertiesKhr
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// An array of size <see cref="UuidSize"/>, containing 8-bit values that represent a
        /// universally unique identifier for the device.
        /// </summary>
        public fixed byte DeviceUuid[UuidSize];
        /// <summary>
        /// An array of size <see cref="UuidSize"/>, containing 8-bit values that represent a
        /// universally unique identifier for the driver build in use by the device.
        /// </summary>
        public fixed byte DriverUuid[UuidSize];
        /// <summary>
        /// A array of size <see cref="LuidSizeKhr"/>, containing 8-bit values that represent a
        /// locally unique identifier for the device.
        /// </summary>
        public fixed byte DeviceLuid[LuidSizeKhr];
        /// <summary>
        /// A bitfield identifying the node within a linked device adapter corresponding to the device.
        /// </summary>
        public int DeviceNodeMask;
        /// <summary>
        /// A boolean value that will be <c>true</c> if <see cref="DeviceLuid"/> contains a valid
        /// LUID, and <c>false</c> if it does not.
        /// </summary>
        public Bool DeviceLuidValid;
    }

    /// <summary>
    /// Specify that an image may be backed by external memory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalMemoryImageCreateInfoKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies one or more external memory handle types.
        /// </summary>
        public ExternalMemoryHandleTypesKhr HandleTypes;
    }

    /// <summary>
    /// Specify that a buffer may be backed by external memory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalMemoryBufferCreateInfoKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies one or more external memory handle types.
        /// </summary>
        public ExternalMemoryHandleTypesKhr HandleTypes;
    }

    /// <summary>
    /// Specify exportable handle types for a device memory object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExportMemoryAllocateInfoKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies one or more memory handle types the application can export from the resulting
        /// allocation. The application can request multiple handle types for the same allocation.
        /// </summary>
        public ExternalMemoryHandleTypesKhr HandleTypes;
    }

    /// <summary>
    /// Import Win32 memory created on the same physical device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImportMemoryWin32HandleInfoKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies the type of handle or name.
        /// </summary>
        public ExternalMemoryHandleTypesKhr HandleType;
        /// <summary>
        /// The external handle to import, or <c>null</c>.
        /// </summary>
        public IntPtr Handle;
        /// <summary>
        /// A NULL-terminated UTF-16 string naming the underlying memory resource to import, or <c>null</c>.
        /// </summary>
        public IntPtr Name;
    }

    /// <summary>
    /// Structure specifying additional attributes of Windows handles exported from a memory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExportMemoryWin32HandleInfoKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A pointer to a Windows <c>SECURITY_ATTRIBUTES</c> structure specifying security
        /// attributes of the handle.
        /// </summary>
        public IntPtr Attributes;
        /// <summary>
        /// A <c>DWORD</c> specifying access rights of the handle.
        /// </summary>
        public int Access;
        /// <summary>
        /// A NULL-terminated UTF-16 string to associate with the underlying resource referenced by
        /// NT handles exported from the created memory.
        /// </summary>
        public IntPtr Name;
    }

    /// <summary>
    /// Structure describing a Win32 handle semaphore export operation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryGetWin32HandleInfoKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The memory object from which the handle will be exported.
        /// </summary>
        public long Memory;
        /// <summary>
        /// The type of handle requested.
        /// </summary>
        public ExternalMemoryHandleTypesKhr HandleType;
    }

    /// <summary>
    /// Structure describing a POSIX FD semaphore export operation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryGetFdInfoKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The memory object from which the handle will be exported.
        /// </summary>
        public long Memory;
        /// <summary>
        /// The type of handle requested.
        /// </summary>
        public ExternalMemoryHandleTypesKhr HandleType;
    }

    /// <summary>
    /// Structure specifying additional attributes of Windows handles exported from a semaphore.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExportSemaphoreWin32HandleInfoKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A pointer to a Windows <c>SECURITY_ATTRIBUTES</c> structure specifying security
        /// attributes of the handle.
        /// </summary>
        public IntPtr Attributes;
        /// <summary>
        /// A <c>DWORD</c> specifying access rights of the handle.
        /// </summary>
        public int Access;
        /// <summary>
        /// A NULL-terminated UTF-16 string to associate with the underlying synchronization
        /// primitive referenced by NT handles exported from the created semaphore.
        /// </summary>
        public IntPtr Name;
    }

    /// <summary>
    /// Structure describing a Win32 handle semaphore export operation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SemaphoreGetWin32HandleInfoKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The semaphore from which state will be exported.
        /// </summary>
        public long Semaphore;
        /// <summary>
        /// The type of handle requested.
        /// </summary>
        public ExternalSemaphoreHandleTypesKhr HandleType;
    }

    /// <summary>
    /// Structure describing a POSIX FD semaphore export operation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SemaphoreGetFdInfoKhr
    {
        /// <summary>
        /// Is the type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Is the semaphore from which state will be exported.
        /// </summary>
        public long Semaphore;
        /// <summary>
        /// Is the type of handle requested.
        /// </summary>
        public ExternalSemaphoreHandleTypesKhr HandleType;
    }

    /// <summary>
    /// Structure specifying fence creation parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceExternalFenceInfoKhr
    {
        /// <summary>
        /// Is the type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is NULL or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Is a <see cref="ExternalFenceHandleTypeFlagsKhr"/> value indicating an external fence
        /// handle type for which capabilities will be returned.
        /// </summary>
        public ExternalFenceHandleTypeFlagsKhr HandleType;
    }

    /// <summary>
    /// Structure describing supported external fence handle features.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalFencePropertiesKhr
    {
        public StructureType Type;
        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Is a bitmask of <see cref="ExternalFenceHandleTypeFlagsKhr"/> indicating which types of
        /// imported handle handleType can be exported from.
        /// </summary>
        public ExternalFenceHandleTypeFlagsKhr ExportFromImportedHandleTypes;
        /// <summary>
        /// Is a bitmask of <see cref="ExternalFenceHandleTypeFlagsKhr"/> specifying handle types
        /// which can be specified at the same time as handleType when creating a fence.
        /// </summary>
        public ExternalFenceHandleTypeFlagsKhr CompatibleHandleTypes;
        /// <summary>
        /// Is a bitmask of <see cref="ExternalFenceFeatureFlagsKhr"/> indicating the features of handleType.
        /// </summary>
        public ExternalFenceFeatureFlagsKhr ExternalFenceFeatures;
    }

    /// <summary>
    /// Structure specifying handle types that can be exported from a fence.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExportFenceCreateInfoKhr
    {
        /// <summary>
        /// Is the type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Is a bitmask of <see cref="ExternalFenceHandleTypeFlagsKhr"/> specifying one or more
        /// fence handle types the application can export from the resulting fence. The application
        /// can request multiple handle types for the same fence.
        /// </summary>
        public ExternalFenceHandleTypeFlagsKhr HandleTypes;
    }

    /// <summary>
    /// (None).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImportFenceWin32HandleInfoKhr
    {
        /// <summary>
        /// Is the type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Is the fence into which the state will be imported.
        /// </summary>
        public long Fence;
        /// <summary>
        /// Is a bitmask of <see cref="FenceImportFlagsKhr"/> specifying additional parameters for
        /// the fence payload import operation.
        /// </summary>
        public FenceImportFlagsKhr Flags;
        /// <summary>
        /// Specifies the type of handle.
        /// </summary>
        public ExternalFenceHandleTypeFlagsKhr HandleType;
        /// <summary>
        /// Is the external handle to import, or <c>null</c>.
        /// </summary>
        public IntPtr Handle;
        /// <summary>
        /// Is the NULL-terminated UTF-16 string naming the underlying synchronization primitive to
        /// import, or <c>null</c>.
        /// </summary>
        public IntPtr Name;
    }

    /// <summary>
    /// Structure specifying additional attributes of Windows handles exported from a fence.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExportFenceWin32HandleInfoKhr
    {
        /// <summary>
        /// Is the type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Is a pointer to a Windows <c>SECURITY_ATTRIBUTES</c> structure specifying security
        /// attributes of the handle.
        /// </summary>
        public IntPtr Attributes;
        /// <summary>
        /// Is a <c>xDWORD</c> specifying access rights of the handle.
        /// </summary>
        public int Access;
        /// <summary>
        /// Is a NULL-terminated UTF-16 string to associate with the underlying synchronization
        /// primitive referenced by NT handles exported from the created fence.
        /// </summary>
        public IntPtr Name;
    }

    /// <summary>
    /// Structure describing a Win32 handle fence export operation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FenceGetWin32HandleInfoKhr
    {
        /// <summary>
        /// Is the type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Is the fence from which state will be exported.
        /// </summary>
        public long Fence;
        /// <summary>
        /// Is the type of handle requested.
        /// </summary>
        public ExternalFenceHandleTypeFlagsKhr HandleType;
    }

    /// <summary>
    /// (None).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImportFenceFdInfoKhr
    {
        /// <summary>
        /// Is the type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Is the fence into which the payload will be imported.
        /// </summary>
        public long Fence;
        /// <summary>
        /// Is a bitmask of <see cref="FenceImportFlagsKhr"/> specifying additional parameters for
        /// the fence payload import operation.
        /// </summary>
        public FenceImportFlagsKhr Flags;
        /// <summary>
        /// Specifies the type of fd.
        /// </summary>
        public ExternalFenceHandleTypeFlagsKhr HandleType;
        /// <summary>
        /// Is the external handle to import.
        /// </summary>
        public int Fd;
    }

    /// <summary>
    /// Structure describing a POSIX FD fence export operation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FenceGetFdInfoKhr
    {
        /// <summary>
        /// Is the type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Is the fence from which state will be exported.
        /// </summary>
        public long Fence;
        /// <summary>
        /// Is the type of handle requested.
        /// </summary>
        public ExternalFenceHandleTypeFlagsKhr HandleType;
    }

    /// <summary>
    /// Structure describing features supported by VKKHR16bitStorage.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDevice16BitStorageFeaturesKhr
    {
        public StructureType Type;
        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// 16-bit integer/floating-point variables supported in BufferBlock.
        /// </summary>
        public Bool StorageBuffer16BitAccess;
        /// <summary>
        /// 16-bit integer/floating-point variables supported in BufferBlock and Block.
        /// </summary>
        public Bool UniformAndStorageBuffer16BitAccess;
        /// <summary>
        /// 16-bit integer/floating-point variables supported in PushConstant.
        /// </summary>
        public Bool StoragePushConstant16;
        /// <summary>
        /// 16-bit integer/floating-point variables supported in shader inputs and outputs.
        /// </summary>
        public Bool StorageInputOutput16;
    }

    /// <summary>
    /// Structure describing dedicated allocation requirements of buffer and image resources.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryDedicatedRequirementsKhr
    {
        /// <summary>
        /// Is the type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Indicates that the implementation would prefer a dedicated allocation for this resource.
        /// The application is still free to suballocate the resource but it may get better
        /// performance if a dedicated allocation is used.
        /// </summary>
        public Bool PrefersDedicatedAllocation;
        /// <summary>
        /// Indicates that a dedicated allocation is required for this resource.
        /// </summary>
        public Bool RequiresDedicatedAllocation;
    }

    /// <summary>
    /// Specify a dedicated memory allocation resource.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryDedicatedAllocateInfoKhr
    {
        /// <summary>
        /// Is the type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Is 0 or a handle of an image which this memory will be bound to.
        /// </summary>
        public long Image;
        /// <summary>
        /// Is 0 or a handle of a buffer which this memory will be bound to.
        /// </summary>
        public long Buffer;
    }

    /// <summary>
    /// Structure describing the point clipping behavior supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDevicePointClippingPropertiesKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The point clipping behavior supported by the implementation.
        /// </summary>
        public PointClippingBehaviorKhr PointClippingBehavior;
    }

    /// <summary>
    /// Structure describing Y'CbCr conversion features that can be supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceSamplerYcbcrConversionFeaturesKhr
    {
        public StructureType Type;
        public IntPtr Next;
        public Bool SamplerYcbcrConversion;
    }

    /// <summary>
    /// Structure specifying combined image sampler descriptor count for multi-planar images.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SamplerYcbcrConversionImageFormatPropertiesKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Tthe number of combined image sampler descriptors that the implementation uses to access
        /// the format.
        /// </summary>
        public int CombinedImageSamplerDescriptorCount;
    }
}
