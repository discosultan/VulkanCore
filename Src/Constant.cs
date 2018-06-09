namespace VulkanCore
{
    // https://www.khronos.org/registry/vulkan/

    /// <summary>
    /// Provides Vulkan specific constants for special values, layer names and extension names.
    /// </summary>
    public static class Constant
    {
        public const int MaxPhysicalDeviceNameSize = 256;
        public const int UuidSize = 16;
        public const int LuidSizeKhr = 8;
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
        public const int QueueFamilyForeignExt = ~0 - 2;
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
            /// This extension defines additional enums for <see cref="Khr.ColorSpaceKhr"/>.
            /// </summary>
            public const string ExtSwapchainColorspace = "VK_EXT_swapchain_colorspace";
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
            public const string KhrExternalMemoryCapabilities = "VK_KHR_external_memory_capabilities";
            /// <summary>
            /// An application may wish to reference device semaphores in multiple Vulkan logical
            /// devices or instances, in multiple processes, and/or in multiple APIs. This extension
            /// provides a set of capability queries and handle definitions that allow an application
            /// to determine what types of “external” semaphore handles an implementation supports
            /// for a given set of use cases.
            /// </summary>
            public const string KhrExternalSemaphoreCapabilities = "VK_KHR_external_semaphore_capabilities";
            /// <summary>
            /// An application may wish to reference device fences in multiple Vulkan logical devices
            /// or instances, in multiple processes, and/or in multiple APIs. This extension provides
            /// a set of capability queries and handle definitions that allow an application to
            /// determine what types of "external" fence handles an implementation supports for a
            /// given set of use cases.
            /// </summary>
            public const string KhrExternalFenceCapabilities = "VK_KHR_external_fence_capabilities";
            /// <summary>
            /// This extension provides new entry points to query device surface capabilities in a
            /// way that can be easily extended by other extensions, without introducing any further
            /// entry points.
            /// </summary>
            public const string KhrGetSurfaceCapabilities2 = "VK_KHR_get_surface_capabilities2";
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
            /// This extension is the device-level companion to the "VK_KHR_surface" extension. It
            /// introduces <see cref="Khr.SwapchainKhr"/> objects, which provide the ability to
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
            /// cref="ShaderModuleCreateInfo.Code"/> as an alternative to SPIR-V shaders.
            /// </summary>
            public const string NVGlslShader = "VK_NV_glsl_shader";
            /// <summary>
            /// This extension extends the set of sampler address modes to include an additional mode
            /// (<see cref="SamplerAddressMode.MirrorClampToEdge"/>) that effectively uses a texture
            /// map twice as large as the original image in which the additional half of the new
            /// image is a mirror image of the original image.
            /// </summary>
            public const string KhrSamplerMirrorClampToEdge = "VK_KHR_sampler_mirror_clamp_to_edge";
            /// <summary>
            /// This extension adds an additional, high quality cubic filtering mode to Vulkan,
            /// using a Catmull-Rom bicubic filter.
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
            /// This extension introduces concepts of object naming and tagging, for better tracking
            /// of Vulkan objects, as well as additional commands for recording annotations of named
            /// sections of a workload to aid organisation and offline analysis in external tools.
            /// </summary>
            public const string ExtDebugMarker = "VK_EXT_debug_marker";
            /// <summary>
            /// This extension adds support for the "SPV_AMD_gcn_shader" SPIR-V extension.
            /// </summary>
            public const string AmdGcnShader = "VK_AMD_gcn_shader";
            /// <summary>
            /// This extension allows device memory to be allocated for a particular buffer or image
            /// resource, which on some devices can significantly improve the performance of that
            /// resource.
            /// </summary>
            public const string NVDedicatedAllocation = "VK_NV_dedicated_allocation";
            /// <summary>
            /// This extension allows an application to source the number of draw calls for indirect
            /// draw calls from a buffer.
            /// </summary>
            public const string AmdDrawIndirectCount = "VK_AMD_draw_indirect_count";
            /// <summary>
            /// This extension allows an application to specify a negative viewport height.
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
            /// This extension enables applications to create exportable Vulkan memory objects such
            /// that the underlying resources can be referenced outside the Vulkan instance that
            /// created them.
            /// </summary>
            public const string NVExternalMemory = "VK_NV_external_memory";
            /// <summary>
            /// This extension enables win32 applications to export win32 handles from Vulkan memory
            /// objects such that the underlying resources can be referenced outside the Vulkan
            /// instance that created them, and import win32 handles created in the Direct3D API to
            /// Vulkan memory objects.
            /// </summary>
            public const string NVExternalMemoryWin32 = "VK_NV_external_memory_win32";
            /// <summary>
            /// This extension provides a way for an application to access the keyed mutex associated
            /// with an imported Vulkan memory object when submitting command buffers to a queue.
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
            /// This extension adds a collection of minor features that were intentionally left out
            /// or overlooked from the original Vulkan 1.0 release.
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
            /// This extension defines two new structures and a function to assign SMPTE (the Society
            /// of Motion Picture and Television Engineers) 2086 metadata and CTA (Consumer
            /// Technology Assocation) 861.3 metadata to a swapchain.
            /// </summary>
            public const string ExtHdrMetadata = "VK_EXT_hdr_metadata";
            /// <summary>
            /// This extension allows an application that uses the "VK_KHR_swapchain" extension to
            /// obtain information about the presentation engine's display, to obtain timing
            /// information about each present, and to schedule a present to happen no earlier than a
            /// desired time.
            /// </summary>
            public const string GoogleDisplayTiming = "VK_GOOGLE_display_timing";
            /// <summary>
            /// This extension has the same goal as the OpenGL ES "GL_OVR_multiview" extension - it
            /// enables rendering to multiple "views" by recording a single set of commands to be
            /// executed with slightly different behavior for each view.
            /// </summary>
            public const string KhxMultiview = "VK_KHX_multiview";
            /// <summary>
            /// This extension provides functionality to use a logical device that consists of
            /// multiple physical devices, as created with the "VK_KHX_device_group_creation" extension.
            /// </summary>
            public const string KhxDeviceGroup = "VK_KHX_device_group";
            /// <summary>
            /// This extension enables an application to export non-Vulkan handles from Vulkan memory
            /// objects such that the underlying resources can be referenced outside the scope of the
            /// Vulkan logical device that created them.
            /// </summary>
            public const string KhrExternalMemory = "VK_KHR_external_memory";
            /// <summary>
            /// This extension enables an application to export Windows handles from Vulkan memory
            /// objects and to import Vulkan memory objects from Windows handles exported from other
            /// Vulkan memory objects or from similar resources in other APIs.
            /// </summary>
            public const string KhrExternalMemoryWin32 = "VK_KHR_external_memory_win32";
            /// <summary>
            /// This extension enables an application to export POSIX file descriptor handles from
            /// Vulkan memory objects and to import Vulkan memory objects from POSIX file descriptor
            /// handles exported from other Vulkan memory objects or from similar resources in other APIs.
            /// </summary>
            public const string KhrExternalMemoryFd = "VK_KHR_external_memory_fd";
            /// <summary>
            /// This extension provides a way for an application to access the keyed mutex associated
            /// with an imported Vulkan memory object when submitting command buffers to a queue.
            /// </summary>
            public const string KhrWin32KeyedMutex = "VK_KHR_win32_keyed_mutex";
            /// <summary>
            /// This extension enables an application to create semaphores from which non-Vulkan
            /// handles that reference the underlying synchronization primitive can be exported.
            /// </summary>
            public const string KhrExternalSemaphore = "VK_KHR_external_semaphore";
            /// <summary>
            /// This extension enables an application to export semaphore state to and import
            /// semaphore state from Windows handles.
            /// </summary>
            public const string KhrExternalSemaphoreWin32 = "VK_KHR_external_semaphore_win32";
            /// <summary>
            /// This extension enables an application to export semaphore state to and import
            /// semaphore state from POSIX file descriptors.
            /// </summary>
            public const string KhrExternalSemaphoreFd = "VK_KHR_external_semaphore_fd";
            /// <summary>
            /// This extension allows descriptors to be written into the command buffer, with the
            /// implementation being responsible for managing their memory.
            /// </summary>
            public const string KhrPushDescriptor = "VK_KHR_push_descriptor";
            /// <summary>
            /// The VK_KHR_16bit_storage extension allows use of 16-bit types in shader input and
            /// output interfaces, and push constant blocks.
            /// </summary>
            public const string Khr16BitStorage = "VK_KHR_16bit_storage";
            /// <summary>
            /// This extension provides a way to update a fixed set of descriptors in a single <see
            /// cref="DescriptorSet"/> with a pointer to a user defined data structure which
            /// describes the new descriptors.
            /// </summary>
            public const string KhrIncrementalPresent = "VK_KHR_incremental_present";
            /// <summary>
            /// This extension provides a way to update a fixed set of descriptors in a single <see
            /// cref="DescriptorSet"/> with a pointer to a user defined data structure which
            /// describes the new descriptors.
            /// </summary>
            public const string KhrDescriptorUpdateTemplate = "VK_KHR_descriptor_update_template";
            /// <summary>
            /// This extension provides a mechanism to render VR scenes at a non-uniform resolution,
            /// in particular a resolution that falls linearly from the center towards the edges.
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
            /// vertex processing stages.
            /// </summary>
            public const string NvxMultiviewPerViewAttributes = "VK_NVX_multiview_per_view_attributes";
            /// <summary>
            /// This extension provides a new per-viewport swizzle that can modify the position of
            /// primitives sent to each viewport.
            /// </summary>
            public const string NVViewportSwizzle = "VK_NV_viewport_swizzle";
            /// <summary>
            /// This extension provides additional orthogonally aligned "discard rectangles"
            /// specified in framebuffer-space coordinates that restrict rasterization of all points,
            /// lines and triangles.
            /// </summary>
            public const string ExtDiscardRectangles = "VK_EXT_discard_rectangles";
            /// <summary>
            /// This extension adds support for the following SPIR-V extension:
            /// "SPV_AMD_shader_trinary_minmax". Secondly, the extension allows the application to
            /// query, which formats can be used together with the new function prototypes introduced
            /// by the SPIR-V extension.
            /// </summary>
            public const string AmdTextureGatherBiasLod = "VK_AMD_texture_gather_bias_lod";
            /// <summary>
            /// This extension extends VK_KHR_swapchain to enable creation of a shared presentable
            /// image. This allows the application to use the image while the presention engine is
            /// accessing it, in order to reduce the latency between rendering and presentation.
            /// </summary>
            public const string KhrSharedPresentableImage = "VK_KHR_shared_presentable_image";
            /// <summary>
            /// An application using external memory may wish to synchronize access to that memory
            /// using fences. This extension enables an application to create fences from which
            /// non-Vulkan handles that reference the underlying synchronization primitive can be exported.
            /// </summary>
            public const string KhrExternalFence = "VK_KHR_external_fence";
            /// <summary>
            /// An application using external memory may wish to synchronize access to that memory
            /// using fences. This extension enables an application to export fence payload to and
            /// import fence payload from Windows handles.
            /// </summary>
            public const string KhrExternalFenceWin32 = "VK_KHR_external_fence_win32";
            /// <summary>
            /// An application using external memory may wish to synchronize access to that memory
            /// using fences. This extension enables an application to export fence payload to and
            /// import fence payload from POSIX file descriptors.
            /// </summary>
            public const string KhrExternalFenceFD = "VK_KHR_external_fence_fd";
            /// <summary>
            /// The VK_KHR_variable_pointers extension allows implementations to indicate their level
            /// of support for the SPV_KHR_variable_pointers SPIR-V extension.
            /// </summary>
            public const string KhrVariablePointers = "VK_KHR_variable_pointers";
            /// <summary>
            /// This extension enables resources to be bound to a dedicated allocation, rather than suballocated.
            /// </summary>
            public const string KhrDedicatedAllocation = "VK_KHR_dedicated_allocation";
            /// <summary>
            /// This extension provides a new sampler parameter which allows applications to produce
            /// a filtered texel value by computing a component-wise minimum (MIN) or maximum (MAX)
            /// of the texels that would normally be averaged.
            /// </summary>
            public const string ExtSamplerFilterMinmax = "VK_EXT_sampler_filter_minmax";
            /// <summary>
            /// This extension adds support for the following SPIR-V extension in Vulkan: "SPV_KHR_storage_buffer_storage_class".
            /// </summary>
            public const string KhrStorageBufferStorageClass = "VK_KHR_storage_buffer_storage_class";
            /// <summary>
            /// This extension adds support for the following SPIR-V extension in Vulkan: "SPV_AMD_gpu_shader_int16".
            /// </summary>
            public const string AmdGpuShaderInt16 = "VK_AMD_gpu_shader_int16";
            /// <summary>
            /// This extension provides new entry points to query memory requirements of images and
            /// buffers in a way that can be easily extended by other extensions, without introducing
            /// any further entry points.
            /// </summary>
            public const string KhrGetMemoryRequirements2 = "VK_KHR_get_memory_requirements2";
            /// <summary>
            /// This extension adds a number of "advanced" blending operations that can be used to
            /// perform new color blending operations, many of which are more complex than the
            /// standard blend modes provided by unextended Vulkan.
            /// </summary>
            public const string ExtBlendOperationAdvanced = "VK_EXT_blend_operation_advanced";
            /// <summary>
            /// This extension allows the fragment coverage value, represented as an integer bitmask,
            /// to be substituted for a color output being written to a single-component color
            /// attachment with integer components (e.g. <see cref="Format.R8UInt"/>).
            /// </summary>
            public const string NVFragmentCoverageToColor = "VK_NV_fragment_coverage_to_color";
            /// <summary>
            /// This extension allows multisample rendering with a raster and depth/stencil sample
            /// count that is larger than the color sample count.
            /// </summary>
            public const string NVFramebufferMixedSamples = "VK_NV_framebuffer_mixed_samples";
            /// <summary>
            /// This extension adds a new <see cref="PolygonMode.FillRectangleNV"/> enum where a
            /// triangle is rasterized by computing and filling its axis-aligned screen-space
            /// bounding box, disregarding the actual triangle edges.
            /// </summary>
            public const string NVFillRectangle = "VK_NV_fill_rectangle";
            /// <summary>
            /// This extension adds support for the following SPIR-V extension in Vulkan:
            /// "SPV_KHR_post_depth_coverage" which allows the fragment shader to control whether
            /// values in the <c>SampleMask</c> built-in input variable reflect the coverage after
            /// the early per-fragment depth and stencil tests are applied.
            /// </summary>
            public const string ExtPostDepthCoverage = "VK_EXT_post_depth_coverage";
            /// <summary>
            /// This extension adds support for the <c>ShaderViewportIndexLayerEXT</c> capability
            /// from the "SPV_EXT_shader_viewport_index_layer" extension in Vulkan.
            /// </summary>
            public const string ExtShaderViewportIndexLayer = "VK_EXT_shader_viewport_index_layer";
            /// <summary>
            /// This extension adds support for the SPIR-V extension "SPV_EXT_shader_stencil_export",
            /// providing a mechanism whereby a shader may generate the stencil reference value per invocation.
            /// </summary>
            public const string ExtShaderStencilExport = "VK_EXT_shader_stencil_export";
            /// <summary>
            /// This extension enables applications to use multisampled rendering with a
            /// depth/stencil sample count that is larger than the color sample count.
            /// </summary>
            public const string AmdMixedAttachmentSamples = "VK_AMD_mixed_attachment_samples";
            /// <summary>
            /// This extension provides efficient read access to the fragment mask in compressed
            /// multisampled color surfaces.
            /// </summary>
            public const string AmdShaderFragmentMask = "VK_AMD_shader_fragment_mask";
            /// <summary>
            /// This extension allows an application to modify the locations of samples within a
            /// pixel used in rasterization.
            /// </summary>
            public const string ExtSampleLocations = "VK_EXT_sample_locations";
            /// <summary>
            /// This extension provides a mechanism for caching the results of potentially expensive
            /// internal validation operations across multiple runs of a Vulkan application.
            /// </summary>
            public const string ExtValidationCache = "VK_EXT_validation_cache";
            /// <summary>
            /// This extension adds a collection of minor features that were intentionally left out
            /// or overlooked from the original Vulkan 1.0 release.
            /// </summary>
            public const string KhrMaintenance2 = "VK_KHR_maintenance2";
            /// <summary>
            /// This extension allows an application to provide the list of all formats that can be
            /// used with an image when it is created.
            /// </summary>
            public const string KhrImageFormatList = "VK_KHR_image_format_list";
            /// <summary>
            /// This extension provides the ability to perform specified color space conversions
            /// during texture sampling operations.
            /// </summary>
            public const string KhrSamplerYcbcrConversion = "VK_KHR_sampler_ycbcr_conversion";
            /// <summary>
            /// This extension provides versions of <see cref="Buffer.BindMemory"/> and <see
            /// cref="Image.BindMemory"/> that allow multiple bindings to be performed at once, and
            /// are extensible.
            /// </summary>
            public const string KhrBindMemory2 = "VK_KHR_bind_memory2";
            public const string AndroidNativeBuffer = "VK_ANDROID_native_buffer";
            public const string AmdShaderImageLoadStoreLod = "VK_AMD_shader_image_load_store_lod";
            /// <summary>
            /// In Vulkan, users can specify device-scope queue priorities. In some cases it may be
            /// useful to extend this concept to a system-wide scope. This extension provides a
            /// mechanism for caller’s to set their system-wide priority. The default queue priority
            /// is <see cref="Ext.QueueGlobalPriorityExt.Medium"/>.
            /// </summary>
            public const string ExtGlobalPriority = "VK_EXT_global_priority";
            /// <summary>
            /// This extension adds a way to query certain information about a compiled shader which
            /// is part of a pipeline. This information may include shader disassembly, shader binary
            /// and various statistics about a shader's resource usage.
            /// </summary>
            public const string AmdShaderInfo = "VK_AMD_shader_info";
            /// <summary>
            /// This extension enables applications to import a dma_buf as <see
            /// cref="DeviceMemory"/>; to export <see cref="DeviceMemory"/> as a dma_buf; and to
            /// create <see cref="Buffer"/> objects that can be bound to that memory.
            /// </summary>
            public const string ExtExternalMemoryDmaBuf = "VK_EXT_external_memory_dma_buf";
            /// <summary>
            /// This extension defines a special queue family, VK_QUEUE_FAMILY_FOREIGN_EXT, which can
            /// be used to transfer ownership of resources backed by external memory to foreign,
            /// external queues.
            /// </summary>
            public const string ExtQueueFamilyForeign = "VK_EXT_queue_family_foreign";
            /// <summary>
            /// This extension enables an application to import host allocations and host mapped
            /// foreign device memory to Vulkan memory objects.
            /// </summary>
            public const string ExtExternalMemoryHost = "VK_EXT_external_memory_host";
            /// <summary>
            /// This extension adds a new rasterization mode called conservative rasterization. There
            /// are two modes of conservative rasterization; overestimation and underestimation.
            /// </summary>
            public const string ExtConservativeRasterization = "VK_EXT_conservative_rasterization";
            /// <summary>
            /// This extension adds a new operation to execute pipelined writes of small marker
            /// values into a <see cref="Buffer"/> object.
            /// </summary>
            public const string AmdBufferMarker = "VK_AMD_buffer_marker";
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
