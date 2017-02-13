using System.Runtime.InteropServices;

namespace VulkanCore
{
    // https://www.khronos.org/registry/vulkan/

    /// <summary>
    /// Provides Vulkan specific constants for special values, layer names and extension names.
    /// </summary>
    public static class Constant
    {
        internal const string VulkanDll = "vulkan-1.dll";
        internal const CallingConvention CallConv = CallingConvention.Winapi;

        public const int MaxPhysicalDeviceNameSize = 256;
        public const int UuidSize = 16;
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
        public const int SubpassExternal = ~0;

        /// <summary>
        /// Provides name constants for common Vulkan instance extensions.
        /// </summary>
        public static class InstanceExtension
        {
            public const string KhrXlibSurface = "VK_KHR_xlib_surface";
            public const string KhrXcbSurface = "VK_KHR_xcb_surface";
            public const string KhrWaylandSurface = "VK_KHR_wayland_surface";
            public const string KhrMirSurface = "VK_KHR_mir_surface";
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
            public const string NVExternalMemoryCapabilities = "VK_NV_external_memory_capabilities";
            public const string KhrGetPhysicalDeviceProperties2 = "VK_KHR_get_physical_device_properties2";
            /// <summary>
            /// This extension provides the <c>VkValidationFlagsEXT</c> struct that can be included
            /// in the <see cref="InstanceCreateInfo.Next"/> chain at instance creation time. The new
            /// struct contains an array of <c>VkValidationCheckEXT</c> values that will be disabled
            /// by the validation layers.
            /// </summary>
            public const string ExtValidationFlags = "VK_EXT_validation_flags";
            public const string NNVISurface = "VK_NN_vi_surface";
            public const string ExtDirectModeDisplay = "VK_EXT_direct_mode_display";
            public const string ExtAcquireXlibDisplay = "VK_EXT_acquire_xlib_display";
            public const string ExtDisplaySurfaceCounter = "VK_EXT_display_surface_counter";
        }

        /// <summary>
        /// Provides name constants for common Vulkan device extensions.
        /// </summary>
        public static class DeviceExtension
        {
            /// <summary>
            /// The "VK_KHR_swapchain" extension is the device-level companion to the "VK_KHR_surface"
            /// extension. It introduces <see cref="SwapchainKhr"/> objects, which provide the ability to
            /// present rendering results to a surface.
            /// </summary>
            public const string KhrSwapchain = "VK_KHR_swapchain";
            public const string KhrDisplay = "VK_KHR_display";
            public const string KhrDisplaySwapchain = "VK_KHR_display_swapchain";
            /// <summary>
            /// Implementations that expose this function allow GLSL shaders to be referenced by <see
            /// cref="ShaderModuleCreateInfo.Code"/> as an alternative to SPIR-V shaders. The
            /// implementation will automatically detect whether SPIR-V or GLSL is passed in. In order to
            /// support Vulkan features the GLSL shaders must be authored to the "GL_KHR_vulkan_glsl"
            /// extension specification.
            /// </summary>
            public const string NVGlslShader = "VK_NV_glsl_shader";
            public const string KhrSamplerMirrorClampToEdge = "VK_KHR_sampler_mirror_clamp_to_edge";
            public const string ImgFilterCubic = "VK_IMG_filter_cubic";
            public const string AmdRasterizationOrder = "VK_AMD_rasterization_order";
            public const string AmdShaderTrinaryMinMax = "VK_AMD_shader_trinary_minmax";
            public const string AmdShaderExplicitVertexParameter = "VK_AMD_shader_explicit_vertex_parameter";
            /// <summary>
            /// The "VK_EXT_debug_marker" extension is a device extension. It introduces concepts of object
            /// naming and tagging, for better tracking of Vulkan objects, as well as additional commands
            /// for recording annotations of named sections of a workload to aid organisation and offline
            /// analysis in external tools.
            /// </summary>
            public const string ExtDebugMarker = "VK_EXT_debug_marker";
            public const string AmdGcnShader = "VK_AMD_gcn_shader";
            public const string NVDedicatedAllocation = "VK_NV_dedicated_allocation";
            public const string AmdDrawIndirectCount = "VK_AMD_draw_indirect_count";
            public const string AmdNegativeViewportHeight = "VK_AMD_negative_viewport_height";
            public const string AmdGpuShaderHalfFloat = "VK_AMD_gpu_shader_half_float";
            public const string AmdShaderBallot = "VK_AMD_shader_ballot";
            public const string ImgFormatPvrtc = "VK_IMG_format_pvrtc";
            public const string NVExternalMemory = "VK_NV_external_memory";
            public const string NVExternalMemoryWin32 = "VK_NV_external_memory_win32";
            public const string NVWin32KeyedMutex = "VK_NV_win32_keyed_mutex";
            public const string KhrShaderDrawParamters = "VK_KHR_shader_draw_parameters";
            public const string ExtShaderSubgroupBallot = "VK_EXT_shader_subgroup_ballot";
            public const string ExtShaderSubgroupVote = "VK_EXT_shader_subgroup_vote";
            /// <summary>
            /// "VK_KHR_maintenance1" adds a collection of minor features that were intentionally left
            /// out or overlooked from the original Vulkan 1.0 release.
            /// </summary>
            public const string KhrMaintenance1 = "VK_KHR_maintenance1";
            public const string NvxDeviceGeneratedCommands = "VK_NVX_device_generated_commands";
            public const string ExtDisplayControl = "VK_EXT_display_control";
            public const string ExtSwapchainColorspace = "VK_EXT_swapchain_colorspace";
            public const string ExtSmpte2086Metadata = "VK_EXT_SMPTE2086_metadata";
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