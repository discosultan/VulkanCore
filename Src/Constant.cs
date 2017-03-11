using System.Runtime.InteropServices;

namespace VulkanCore
{
    // https://www.khronos.org/registry/vulkan/

    /// <summary>
    /// Provides Vulkan specific constants for special values, layer names and extension names.
    /// </summary>
    public static class Constant
    {
        // The dll is forwarded to 'vulkan.so' on Linux based systems running under Mono.
        internal const string VulkanDll = "vulkan-1.dll";
        internal const CallingConvention CallConv = CallingConvention.Winapi;

        public const int MaxPhysicalDeviceNameSize = 256;
        public const int UuidSize = 16;
        public const int LuidSizeKhx = 8;
        public const int MaxExtensionNameSize = 256;
        public const int MaxDescriptionSize = 256;
        public const int MaxMemoryTypes = 32;
        /// <summary>
        /// The maximum number of unique memory heaps, each of which supporting 1 or more memory types.
        /// </summary>
        public const int MaxMemoryHeaps = 16;
        public const float LodClampNone = 1000.0f;
        public const int RemainingMipLevels = ~0;
        public const int RemainingArrayLevels = ~0;
        public const long WholeSize = ~0;
        public const int AttachmentUnused = ~0;
        public const int True = 1;
        public const int False = 0;
        public const int QueueFamilyIgnored = ~0;
        public const int QueueFamilyExternalKhx = ~0 - 1;
        public const int SubpassExternal = ~0;
        public const int MaxDeviceGroupSizeKhx = 32;

        /// <summary>
        /// Provides name constants for common Vulkan instance extensions.
        /// </summary>
        public static class InstanceExtension
        {
            /// <summary>
            /// The "VK_KHR_xlib_surface" extension is an instance extension. It provides a mechanism
            /// to create a <see cref="Khr.SurfaceKhr"/> object (defined by the "VK_KHR_surface"
            /// extension) that refers to an X11 window, using the Xlib client-side library, as well
            /// as a query to determine support for rendering via Xlib.
            /// </summary>
            public const string KhrXlibSurface = "VK_KHR_xlib_surface";
            /// <summary>
            /// The "VK_KHR_xcb_surface" extension is an instance extension. It provides a mechanism
            /// to create a <see cref="Khr.SurfaceKhr"/> object (defined by the "VK_KHR_surface"
            /// extension) that refers to an X11 window, using the XCB client-side library, as well
            /// as a query to determine support for rendering via XCB.
            /// </summary>
            public const string KhrXcbSurface = "VK_KHR_xcb_surface";
            /// <summary>
            /// The "VK_KHR_wayland_surface" extension is an instance extension. It provides a
            /// mechanism to create a <see cref="Khr.SurfaceKhr"/> object (defined by the
            /// "VK_KHR_surface" extension) that refers to a Wayland wl_surface, as well as a query
            /// to determine support for rendering to the windows desktop.
            /// </summary>
            public const string KhrWaylandSurface = "VK_KHR_wayland_surface";
            /// <summary>
            /// The "VK_KHR_mir_surface" extension is an instance extension. It provides a mechanism
            /// to create a <see cref="Khr.SurfaceKhr"/> object (defined by the "VK_KHR_surface"
            /// extension) that refers to a Mir surface, as well as a query to determine support for
            /// rendering to the windows desktop.
            /// </summary>
            public const string KhrMirSurface = "VK_KHR_mir_surface";
            /// <summary>
            /// The "VK_KHR_android_surface" extension is an instance extension. It provides a
            /// mechanism to create a <see cref="Khr.SurfaceKhr"/> object (defined by the
            /// "VK_KHR_surface" extension) that refers to an ANativeWindow, Android’s native surface
            /// type. The ANativeWindow represents the producer endpoint of any buffer queue,
            /// regardless of consumer endpoint. Common consumer endpoints for ANativeWindows are the
            /// system window compositor, video encoders, and application-specific compositors
            /// importing the images through a SurfaceTexture.
            /// </summary>
            public const string KhrAndroidSurface = "VK_KHR_android_surface";
            /// <summary>
            /// The "VK_KHR_win32_surface" extension is an instance extension. It provides a mechanism to
            /// create a <see cref="Khr.SurfaceKhr"/> object (defined by the "VK_KHR_surface" extension) that
            /// refers to a Win32 HWND, as well as a query to determine support for rendering to the
            /// windows desktop.
            /// </summary>
            public const string KhrWin32Surface = "VK_KHR_win32_surface";
            /// <summary>
            /// This extension defines a way for layers and the implementation to call back to the
            /// application for events of interest to the application.
            /// </summary>
            public const string ExtDebugReport = "VK_EXT_debug_report";
            /// <summary>
            /// <para>
            /// The "VK_KHR_surface" extension is an instance extension. It introduces <see
            /// cref="Khr.SurfaceKhr"/> objects, which abstract native platform surface or window objects for
            /// use with Vulkan. It also provides a way to determine whether a queue family in a physical
            /// device supports presenting to particular surface.
            /// </para>
            /// <para>
            /// Separate extensions for each each platform provide the mechanisms for creating <see
            /// cref="Khr.SurfaceKhr"/> objects, but once created they may be used in this and other
            /// platform-independent extensions, in particular the "VK_KHR_swapchain" extension.
            /// </para>
            /// </summary>
            public const string KhrSurface = "VK_KHR_surface";
            /// <summary>
            /// Applications may wish to import memory from the Direct 3D API, or export memory to
            /// other Vulkan instances. This extension provides a set of capability queries that
            /// allow applications determine what types of win32 memory handles an implementation
            /// supports for a given set of use cases.
            /// </summary>
            public const string NVExternalMemoryCapabilities = "VK_NV_external_memory_capabilities";
            /// <summary>
            /// This extension provides new entry points to query device features, device properties,
            /// and format properties in a way that can be easily extended by other extensions,
            /// without introducing any further entry points. The Vulkan 1.0
            /// feature/limit/formatproperty structures do not include a Type/Next, this extension
            /// wraps them in new structures with Type/Next so an application can query a chain of
            /// feature/limit/formatproperty structures by constructing the chain and letting the
            /// implementation fill them in. A new command is added for each <see
            /// cref="PhysicalDevice"/>.Get* command in core Vulkan 1.0. The new feature structure
            /// (and a chain of extensions) can also be passed in to device creation to enable features.
            /// </summary>
            public const string KhrGetPhysicalDeviceProperties2 = "VK_KHR_get_physical_device_properties2";
            /// <summary>
            /// This extension provides the <c>VkValidationFlagsEXT</c> struct that can be included
            /// in the <see cref="InstanceCreateInfo.Next"/> chain at instance creation time. The new
            /// struct contains an array of <c>VkValidationCheckEXT</c> values that will be disabled
            /// by the validation layers.
            /// </summary>
            public const string ExtValidationFlags = "VK_EXT_validation_flags";
            /// <summary>
            /// The "VK_NN_vi_surface" extension is an instance extension. It provides a mechanism to
            /// create a <see cref="Khr.SurfaceKhr"/> object (defined by the "VK_KHR_surface"
            /// extension) associated with an <c>nn::vi::Layer</c>.
            /// </summary>
            public const string NNVISurface = "VK_NN_vi_surface";
            /// <summary>
            /// This is extension, along with related platform exentions, allows applications to take
            /// exclusive control of displays associated with a native windowing system. This is
            /// especially useful for virtual reality applications that wish to hide HMDs (head
            /// mounted displays) from the native platform’s display management system, desktop,
            /// and/or other applications.
            /// </summary>
            public const string ExtDirectModeDisplay = "VK_EXT_direct_mode_display";
            /// <summary>
            /// This extension allows an application to take exclusive control on a display currently
            /// associated with an X11 screen. When control is acquired, the display will be
            /// deassociated from the X11 screen until control is released or the specified display
            /// connection is closed. Essentially, the X11 screen will behave as if the monitor has
            /// been unplugged until control is released.
            /// </summary>
            public const string ExtAcquireXlibDisplay = "VK_EXT_acquire_xlib_display";
            /// <summary>
            /// This is extension defines a vertical blanking period counter associated with display
            /// surfaces. It provides a mechanism to query support for such a counter from a
            /// <see cref="Khr.SurfaceKhr"/> object.
            /// </summary>
            public const string ExtDisplaySurfaceCounter = "VK_EXT_display_surface_counter";
            /// <summary>
            /// This extension provides instance-level commands to enumerate groups of physical
            /// devices, and to create a logical device from a subset of one of those groups. Such a
            /// logical device can then be used with new features in the "VK_KHX_device_group" extension.
            /// </summary>
            public const string KhxDeviceGroupCreation = "VK_KHX_device_group_creation";
            /// <summary>
            /// An application may wish to reference device memory in multiple Vulkan logical devices
            /// or instances, in multiple processes, and/or in multiple APIs. This extension provides
            /// a set of capability queries and handle definitions that allow an application to
            /// determine what types of "external" memory handles an implementation supports for a
            /// given set of use cases.
            /// </summary>
            public const string KhxExternalMemoryCapabilities = "VK_KHX_external_memory_capabilities";
            /// <summary>
            /// An application may wish to reference device semaphores in multiple Vulkan logical
            /// devices or instances, in multiple processes, and/or in multiple APIs. This extension
            /// provides a set of capability queries and handle definitions that allow an application
            /// to determine what types of “external” semaphore handles an implementation supports
            /// for a given set of use cases.
            /// </summary>
            public const string KhxExternalSemaphoreCapabilities = "VK_KHX_external_semaphore_capabilities";
            /// <summary>
            /// The "VK_MVK_ios_surface" extension is an instance extension. It provides a mechanism
            /// to create a <see cref="Khr.SurfaceKhr"/> object (defined by the "VK_KHR_surface"
            /// extension) that refers to a <c>UIView</c>, the native surface type of iOS, which is
            /// underpinned by a <c>CAMetalLayer</c>, to support rendering to the surface using
            /// Apple’s Metal framework.
            /// </summary>
            public const string MvkIOSSurface = "VK_MVK_ios_surface";
            /// <summary>
            /// The "VK_MVK_macos_surface" extension is an instance extension. It provides a
            /// mechanism to create a <see cref="Khr.SurfaceKhr"/> object (defined by the
            /// "VK_KHR_surface" extension) that refers to an <c>NSView</c>, the native surface type
            /// of macOS, which is underpinned by a <c>CAMetalLayer</c>, to support rendering to the
            /// surface using Apple’s Metal framework.
            /// </summary>
            public const string MvkMacOSSurface = "VK_MVK_macos_surface";
            public const string MvkMoltenVK = "VK_MVK_moltenvk";
        }

        /// <summary>
        /// Provides name constants for common Vulkan device extensions.
        /// </summary>
        public static class DeviceExtension
        {
            /// <summary>
            /// The "VK_KHR_swapchain" extension is the device-level companion to the "VK_KHR_surface"
            /// extension. It introduces <see cref="Khr.SwapchainKhr"/> objects, which provide the ability to
            /// present rendering results to a surface.
            /// </summary>
            public const string KhrSwapchain = "VK_KHR_swapchain";
            /// <summary>
            /// This extension provides the API to enumerate displays and available modes on a given device.
            /// </summary>
            public const string KhrDisplay = "VK_KHR_display";
            /// <summary>
            /// This extension provides an API to create a swapchain directly on a device’s display
            /// without any underlying window system.
            /// </summary>
            public const string KhrDisplaySwapchain = "VK_KHR_display_swapchain";
            /// <summary>
            /// Implementations that expose this function allow GLSL shaders to be referenced by <see
            /// cref="ShaderModuleCreateInfo.Code"/> as an alternative to SPIR-V shaders. The
            /// implementation will automatically detect whether SPIR-V or GLSL is passed in. In order to
            /// support Vulkan features the GLSL shaders must be authored to the "GL_KHR_vulkan_glsl"
            /// extension specification.
            /// </summary>
            public const string NVGlslShader = "VK_NV_glsl_shader";
            /// <summary>
            /// "VK_KHR_sampler_mirror_clamp_to_edge" extends the set of sampler address modes to
            /// include an additional mode (<see cref="SamplerAddressMode.MirrorClampToEdge"/>) that
            /// effectively uses a texture map twice as large as the original image in which the
            /// additional half of the new image is a mirror image of the original image.
            /// </summary>
            public const string KhrSamplerMirrorClampToEdge = "VK_KHR_sampler_mirror_clamp_to_edge";
            /// <summary>
            /// "VK_IMG_filter_cubic" adds an additional, high quality cubic filtering mode to Vulkan,
            /// using a Catmull-Rom bicubic filter. Performing this kind of filtering can be done in
            /// a shader by using 16 samples and a number of instructions, but this can be
            /// inefficient. The cubic filter mode exposes an optimized high quality texture sampling
            /// using fixed texture sampling hardware.
            /// </summary>
            public const string ImgFilterCubic = "VK_IMG_filter_cubic";
            /// <summary>
            /// This extension introduces the possibility for the application to control the order of
            /// primitive rasterization.
            /// </summary>
            public const string AmdRasterizationOrder = "VK_AMD_rasterization_order";
            /// <summary>
            /// This extension adds support for the "SPV_AMD_shader_trinary_minmax" SPIR-V extension.
            /// </summary>
            public const string AmdShaderTrinaryMinMax = "VK_AMD_shader_trinary_minmax";
            /// <summary>
            /// This extension adds support for the "SPV_AMD_shader_explicit_vertex_parameter" SPIR-V extension.
            /// </summary>
            public const string AmdShaderExplicitVertexParameter = "VK_AMD_shader_explicit_vertex_parameter";
            /// <summary>
            /// The "VK_EXT_debug_marker" extension is a device extension. It introduces concepts of object
            /// naming and tagging, for better tracking of Vulkan objects, as well as additional commands
            /// for recording annotations of named sections of a workload to aid organisation and offline
            /// analysis in external tools.
            /// </summary>
            public const string ExtDebugMarker = "VK_EXT_debug_marker";
            /// <summary>
            /// This extension adds support for the "SPV_AMD_gcn_shader" SPIR-V extension.
            /// </summary>
            public const string AmdGcnShader = "VK_AMD_gcn_shader";
            /// <summary>
            /// This extension allows device memory to be allocated for a particular buffer or image
            /// resource, which on some devices can significantly improve the performance of that
            /// resource. Normal device memory allocations must support memory aliasing and sparse
            /// binding, which could interfere with optimizations like framebuffer compression or
            /// efficient page table usage. This is important for render targets and very large
            /// resources, but need not (and probably should not) be used for smaller resources that
            /// can benefit from suballocation.
            /// </summary>
            public const string NVDedicatedAllocation = "VK_NV_dedicated_allocation";
            /// <summary>
            /// This extension allows an application to source the number of draw calls for indirect
            /// draw calls from a buffer. This enables applications to generate arbitrary amounts of
            /// draw commands and execute them without host intervention.
            /// </summary>
            public const string AmdDrawIndirectCount = "VK_AMD_draw_indirect_count";
            /// <summary>
            /// This extension allows an application to specify a negative viewport height. The
            /// result is that the viewport transformation will flip along the y-axis.
            /// </summary>
            public const string AmdNegativeViewportHeight = "VK_AMD_negative_viewport_height";
            /// <summary>
            /// This extension adds support for the "SPV_AMD_gpu_shader_half_float" SPIR-V extension.
            /// </summary>
            public const string AmdGpuShaderHalfFloat = "VK_AMD_gpu_shader_half_float";
            /// <summary>
            /// This extension adds support for the "SPV_AMD_shader_ballot" SPIR-V extension.
            /// </summary>
            public const string AmdShaderBallot = "VK_AMD_shader_ballot";
            public const string ImgFormatPvrtc = "VK_IMG_format_pvrtc";
            /// <summary>
            /// Applications may wish to export memory to other Vulkan instances or other APIs, or
            /// import memory from other Vulkan instances or other APIs to enable Vulkan workloads to
            /// be split up across application module, process, or API boundaries. This extension
            /// enables applications to create exportable Vulkan memory objects such that the
            /// underlying resources can be referenced outside the Vulkan instance that created them.
            /// </summary>
            public const string NVExternalMemory = "VK_NV_external_memory";
            /// <summary>
            /// Applications may wish to export memory to other Vulkan instances or other APIs, or
            /// import memory from other Vulkan instances or other APIs to enable Vulkan workloads to
            /// be split up across application module, process, or API boundaries. This extension
            /// enables win32 applications to export win32 handles from Vulkan memory objects such
            /// that the underlying resources can be referenced outside the Vulkan instance that
            /// created them, and import win32 handles created in the Direct3D API to Vulkan memory objects.
            /// </summary>
            public const string NVExternalMemoryWin32 = "VK_NV_external_memory_win32";
            /// <summary>
            /// Applications that wish to import Direct3D 11 memory objects into the Vulkan API may
            /// wish to use the native keyed mutex mechanism to synchronize access to the memory
            /// between Vulkan and Direct3D. This extension provides a way for an application to
            /// access the keyed mutex associated with an imported Vulkan memory object when
            /// submitting command buffers to a queue.
            /// </summary>
            public const string NVWin32KeyedMutex = "VK_NV_win32_keyed_mutex";
            /// <summary>
            /// This extension adds support for the "SPV_KHR_shader_draw_parameters" SPIR-V extension.
            /// </summary>
            public const string KhrShaderDrawParamters = "VK_KHR_shader_draw_parameters";
            /// <summary>
            /// This extension adds support for the "SPV_KHR_shader_ballot" SPIR-V extension.
            /// </summary>
            public const string ExtShaderSubgroupBallot = "VK_EXT_shader_subgroup_ballot";
            /// <summary>
            /// This extension adds support for the "SPV_KHR_subgroup_vote" SPIR-V extension.
            /// </summary>
            public const string ExtShaderSubgroupVote = "VK_EXT_shader_subgroup_vote";
            /// <summary>
            /// "VK_KHR_maintenance1" adds a collection of minor features that were intentionally left
            /// out or overlooked from the original Vulkan 1.0 release.
            /// </summary>
            public const string KhrMaintenance1 = "VK_KHR_maintenance1";
            /// <summary>
            /// This extension allows the device to generate a number of critical commands for command buffers.
            /// </summary>
            public const string NvxDeviceGeneratedCommands = "VK_NVX_device_generated_commands";
            /// <summary>
            /// This extension defines a set of utility functions for use with the "VK_KHR_display"
            /// and "VK_KHR_display_swapchain" extensions.
            /// </summary>
            public const string ExtDisplayControl = "VK_EXT_display_control";
            /// <summary>
            /// This extension defines additional enums for <see cref="Khr.ColorSpaceKhr"/>.
            /// </summary>
            public const string ExtSwapchainColorspace = "VK_EXT_swapchain_colorspace";
            /// <summary>
            /// This extension defines two new structures and a function to assign SMPTE (the Society
            /// of Motion Picture and Television Engineers) 2086 metadata and CTA (Consumer
            /// Technology Assocation) 861.3 metadata to a swapchain. The metadata includes the color
            /// primaries, white point, and luminance range of the mastering display, which all
            /// together define the color volume that contains all the possible colors the mastering
            /// display can produce. The mastering display is the display where creative work is done
            /// and creative intent is established. To preserve such creative intent as much as
            /// possible and achieve consistent color reproduction on different viewing displays, it
            /// is useful for the display pipeline to know the color volume of the original mastering
            /// display where content was created or tuned. This avoids performing unnecessary
            /// mapping of colors that are not displayable on the original mastering display. The
            /// metadata also includes the <see cref="Ext.HdrMetadataExt.MaxContentLightLevel"/> and
            /// <see cref="Ext.HdrMetadataExt.MaxFrameAverageLightLevel"/> as defined by CTA 861.3.
            /// </summary>
            public const string ExtHdrMetadata = "VK_EXT_hdr_metadata";
            /// <summary>
            /// This device extension allows an application that uses the "VK_KHR_swapchain"
            /// extension to obtain information about the presentation engine's display, to obtain
            /// timing information about each present, and to schedule a present to happen no earlier
            /// than a desired time. An application can use this to minimize various visual anomalies
            /// (e.g. stuttering).
            /// </summary>
            public const string GoogleDisplayTiming = "VK_GOOGLE_display_timing";
            /// <summary>
            /// This extension has the same goal as the OpenGL ES "GL_OVR_multiview" extension - it
            /// enables rendering to multiple "views" by recording a single set of commands to be
            /// executed with slightly different behavior for each view. It includes a concise way to
            /// declare a render pass with multiple views, and gives implementations freedom to
            /// render the views in the most efficient way possible.
            /// </summary>
            public const string KhxMultiview = "VK_KHX_multiview";
            /// <summary>
            /// This extension provides functionality to use a logical device that consists of
            /// multiple physical devices, as created with the "VK_KHX_device_group_creation"
            /// extension. A device group can allocate memory across the subdevices, bind memory from
            /// one subdevice to a resource on another subdevice, record command buffers where some
            /// work executes on an arbitrary subset of the subdevices, and potentially present a
            /// swapchain image from one or more subdevices.
            /// </summary>
            public const string KhxDeviceGroup = "VK_KHX_device_group";
            /// <summary>
            /// An application may wish to reference device memory in multiple Vulkan logical devices
            /// or instances, in multiple processes, and/or in multiple APIs. This extension enables
            /// an application to export non-Vulkan handles from Vulkan memory objects such that the
            /// underlying resources can be referenced outside the scope of the Vulkan logical device
            /// that created them.
            /// </summary>
            public const string KhxExternalMemory = "VK_KHX_external_memory";
            /// <summary>
            /// An application may wish to reference device memory in multiple Vulkan logical devices
            /// or instances, in multiple processes, and/or in multiple APIs. This extension enables
            /// an application to export Windows handles from Vulkan memory objects and to import
            /// Vulkan memory objects from Windows handles exported from other Vulkan memory objects
            /// or from similar resources in other APIs.
            /// </summary>
            public const string KhxExternalMemoryWin32 = "VK_KHX_external_memory_win32";
            /// <summary>
            /// An application may wish to reference device memory in multiple Vulkan logical devices
            /// or instances, in multiple processes, and/or in multiple APIs. This extension enables
            /// an application to export POSIX file descriptor handles from Vulkan memory objects and
            /// to import Vulkan memory objects from POSIX file descriptor handles exported from
            /// other Vulkan memory objects or from similar resources in other APIs.
            /// </summary>
            public const string KhxExternalMemoryFd = "VK_KHX_external_memory_fd";
            /// <summary>
            /// Applications that wish to import Direct3D 11 memory objects into the Vulkan API may
            /// wish to use the native keyed mutex mechanism to synchronize access to the memory
            /// between Vulkan and Direct3D. This extension provides a way for an application to
            /// access the keyed mutex associated with an imported Vulkan memory object when
            /// submitting command buffers to a queue.
            /// </summary>
            public const string KhxWin32KeyedMutex = "VK_KHX_win32_keyed_mutex";
            /// <summary>
            /// An application using external memory may wish to synchronize access to that memory
            /// using semaphores. This extension enables an application to create semaphores from
            /// which non-Vulkan handles that reference the underlying synchronization primitive can
            /// be exported.
            /// </summary>
            public const string KhxExternalSemaphore = "VK_KHX_external_semaphore";
            /// <summary>
            /// An application using external memory may wish to synchronize access to that memory
            /// using semaphores. This extension enables an application to export semaphore state to
            /// and import semaphore state from Windows handles.
            /// </summary>
            public const string KhxExternalSemaphoreWin32 = "VK_KHX_external_semaphore_win32";
            /// <summary>
            /// An application using external memory may wish to synchronize access to that memory
            /// using semaphores. This extension enables an application to export semaphore state to
            /// and import semaphore state from POSIX file descriptors.
            /// </summary>
            public const string KhxExternalSemaphoreFd = "VK_KHX_external_semaphore_fd";
            /// <summary>
            /// This extension allows descriptors to be written into the command buffer, with the
            /// implementation being responsible for managing their memory. Push descriptors may
            /// enable easier porting from older APIs and in some cases can be more efficient than
            /// writing descriptors into descriptor sets.
            /// </summary>
            public const string KhrPushDescriptor = "VK_KHR_push_descriptor";
            /// <summary>
            /// Applications may wish to update a fixed set of descriptors in a large number of
            /// descriptors sets very frequently, i.e. during initializaton phase or if it’s required
            /// to rebuild descriptor sets for each frame. For those cases it’s also not unlikely
            /// that all information required to update a single descriptor set is stored in a single
            /// struct. This extension provides a way to update a fixed set of descriptors in a
            /// single <see cref="DescriptorSet"/> with a pointer to a user defined data structure
            /// which describes the new descriptors.
            /// </summary>
            public const string KhrDescriptorUpdateTemplate = "VK_KHR_descriptor_update_template";
            /// <summary>
            /// This extension provides a mechanism to render VR scenes at a non-uniform resolution,
            /// in particular a resolution that falls linearly from the center towards the edges.
            /// This is achieved by scaling the "w" coordinate of the vertices in the clip space
            /// before perspective divide.
            /// </summary>
            public const string NVClipSpaceWScaling = "VK_NV_clip_space_w_scaling";
            /// <summary>
            /// This extension adds support for the following SPIR-V extension in Vulkan: "SPV_NV_sample_mask_override_coverage".
            /// </summary>
            public const string NVSampleMaskOverrideCoverage = "VK_NV_sample_mask_override_coverage";
            /// <summary>
            /// This extension adds support for the following SPIR-V extension in Vulkan: "SPV_NV_geometry_shader_passthrough".
            /// </summary>
            public const string NVGeometryShaderPassthrough = "VK_NV_geometry_shader_passthrough";
            /// <summary>
            /// This extension adds support for the following SPIR-V extension in Vulkan: "SPV_NV_viewport_array2".
            /// </summary>
            public const string NVViewportArray2 = "VK_NV_viewport_array2";
            /// <summary>
            /// This extension adds a new way to write shaders to be used with multiview subpasses,
            /// where the attributes for all views are written out by a single invocation of the
            /// vertex processing stages. Related SPIR-V and GLSL extensions
            /// "SPV_NVX_multiview_per_view_attributes" and "GL_NVX_multiview_per_view_attributes"
            /// introduce per-view position and viewport mask attributes arrays, and this extension
            /// defines how those per-view attribute arrays are interpreted by Vulkan. Pipelines
            /// using per-view attributes may only execute the vertex processing stages once for all
            /// views rather than once per-view, which reduces redundant shading work.
            /// </summary>
            public const string NvxMultiviewPerViewAttributes = "VK_NVX_multiview_per_view_attributes";
            /// <summary>
            /// This extension provides a new per-viewport swizzle that can modify the position of
            /// primitives sent to each viewport. New viewport swizzle state is added for each
            /// viewport, and a new position vector is computed for each vertex by selecting from and
            /// optionally negating any of the four components of the original position vector.
            /// </summary>
            public const string NVViewportSwizzle = "VK_NV_viewport_swizzle";
            /// <summary>
            /// This extension provides additional orthogonally aligned "discard rectangles"
            /// specified in framebuffer-space coordinates that restrict rasterization of all points,
            /// lines and triangles.
            /// </summary>
            public const string ExtDiscardRectangles = "VK_EXT_discard_rectangles";
        }

        /// <summary>
        /// Provides name constants for common Vulkan instance layers.
        /// </summary>
        public static class InstanceLayer
        {
            /// <summary>
            /// A built-in meta-layer definition which simplifies validation for applications. Specifying
            /// this short-hand layer definition will load a standard set of validation layers in the
            /// optimal order:
            /// <para>"VK_LAYER_GOOGLE_threading"</para>
            /// <para>"VK_LAYER_LUNARG_parameter_validation"</para>
            /// <para>"VK_LAYER_LUNARG_device_limits"</para>
            /// <para>"VK_LAYER_LUNARG_object_tracker"</para>
            /// <para>"VK_LAYER_LUNARG_image"</para>
            /// <para>"VK_LAYER_LUNARG_core_validation"</para>
            /// <para>"VK_LAYER_LUNARG_swapchain"</para>
            /// <para>"VK_LAYER_GOOGLE_unique_objects"</para>
            /// </summary>
            public const string LunarGStandardValidation = "VK_LAYER_LUNARG_standard_validation";
            /// <summary>
            /// Wrap all Vulkan objects in a unique pointer at create time and unwrap them at use time.
            /// </summary>
            public const string GoogleUniqueObjects = "VK_LAYER_GOOGLE_unique_objects";
            /// <summary>
            /// Print API calls and their parameters and values.
            /// </summary>
            public const string LunarGApiDump = "VK_LAYER_LUNARG_api_dump";
            /// <summary>
            /// Validate that app properly queries features and obeys feature limitations.
            /// </summary>
            public const string LunarGDeviceLimits = "VK_LAYER_LUNARG_device_limits";
            /// <summary>
            /// Validate the descriptor set, pipeline state, and dynamic state; validate the interfaces
            /// between SPIR-V modules and the graphics pipeline; track and validate GPU memory and its
            /// binding to objects and command buffers.
            /// </summary>
            public const string LunarGCoreValidation = "VK_LAYER_LUNARG_core_validation";
            /// <summary>
            /// Validate texture formats and render target formats.
            /// </summary>
            public const string LunarGImage = "VK_LAYER_LUNARG_image";
            /// <summary>
            /// Track all Vulkan objects and flag invalid objects and object memory leaks.
            /// </summary>
            public const string LunarGObjectTracker = "VK_LAYER_LUNARG_object_tracker";
            /// <summary>
            /// Validate API parameter values.
            /// </summary>
            public const string LunarGParameterValidation = "VK_LAYER_LUNARG_parameter_validation";
            /// <summary>
            /// Validate the use of the WSI "swapchain" extensions.
            /// </summary>
            public const string LunarGSwapchain = "VK_LAYER_LUNARG_swapchain";
            /// <summary>
            /// Check validity of multi-threaded API usage.
            /// </summary>
            public const string GoogleThreading = "VK_LAYER_GOOGLE_threading";
        }
    }
}