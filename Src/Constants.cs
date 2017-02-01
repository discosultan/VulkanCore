using System.Runtime.InteropServices;

namespace VulkanCore
{
    public class Constants
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
    }

    // https://www.khronos.org/registry/vulkan/

    /// <summary>
    /// Provides name constants for common Vulkan extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// This extension defines a way for layers and the implementation to call back to the
        /// application for events of interest to the application.
        /// </summary>
        public const string ExtDebugReport = "VK_EXT_debug_report";

        /// <summary>
        /// The "VK_EXT_debug_marker" extension is a device extension. It introduces concepts of object
        /// naming and tagging, for better tracking of Vulkan objects, as well as additional commands
        /// for recording annotations of named sections of a workload to aid organisation and offline
        /// analysis in external tools.
        /// </summary>
        public const string ExtDebugMarker = "VK_EXT_debug_marker";

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
        /// The "VK_KHR_win32_surface" extension is an instance extension. It provides a mechanism to
        /// create a <see cref="Khr.SurfaceKhr"/> object (defined by the "VK_KHR_surface" extension) that
        /// refers to a Win32 HWND, as well as a query to determine support for rendering to the
        /// windows desktop.
        /// </summary>
        public const string KhrWin32Surface = "VK_KHR_win32_surface";

        /// <summary>
        /// The "VK_KHR_swapchain" extension is the device-level companion to the "VK_KHR_surface"
        /// extension. It introduces <see cref="SwapchainKhr"/> objects, which provide the ability to
        /// present rendering results to a surface.
        /// </summary>
        public const string KhrSwapchain = "VK_KHR_swapchain";

        /// <summary>
        /// "VK_KHR_maintenance1" adds a collection of minor features that were intentionally left
        /// out or overlooked from the original Vulkan 1.0 release.
        /// </summary>
        public const string KhrMaintenance1 = "VK_KHR_maintenance1";

        /// <summary>
        /// Implementations that expose this function allow GLSL shaders to be referenced by <see
        /// cref="ShaderModuleCreateInfo.Code"/> as an alternative to SPIR-V shaders. The
        /// implementation will automatically detect whether SPIR-V or GLSL is passed in. In order to
        /// support Vulkan features the GLSL shaders must be authored to the "GL_KHR_vulkan_glsl"
        /// extension specification.
        /// </summary>
        public const string NVGlslShader = "VK_NV_glsl_shader";
    }

    /// <summary>
    /// Provides name constants for common Vulkan layers.
    /// </summary>
    public static class Layers
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