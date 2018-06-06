using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Mvk
{
    /// <summary>
    /// Structure specifying MoltenVK configuration settings.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MVKDeviceConfiguration
    {
        /// <summary>
        /// If enabled, several debugging capabilities will be enabled. Shader code will be logged 
        /// during Runtime Shader Conversion. Adjusts settings that might trigger Metal validation but 
        /// are otherwise acceptable to Metal runtime. Improves support for Xcode GPU Frame Capture. 
        /// Default value is true in the presence of the DEBUG build setting, and false otherwise.
        /// </summary>
        public Bool DebugMode;
        /// <summary>
        /// If enabled, MSL vertex shader code created during Runtime Shader Conversion will flip the 
        /// Y-axis of each vertex, as Vulkan coordinate system is inverse of OpenGL. Default is true. 
        /// </summary>
        public Bool ShaderConversionFlipVertexY;
        /// <summary>
        /// If enabled, queue command submissions (vkQueueSubmit() and vkQueuePresentKHR()) will be 
        /// processed on the thread that called the submission function. If disabled, processing will
        /// be dispatched to a GCD dispatch_queue whose priority is determined by 
        /// VkDeviceQueueCreateInfo::pQueuePriorities during vkCreateDevice(). This setting affects how 
        /// much command processing should be performed on the rendering thread, or offloaded to a secondary 
        /// thread. Default value is false, and command processing will be handled on a prioritizable queue thread.
        /// </summary>
        public Bool SynchronousQueueSubmits;
        /// <summary>
        /// Metal allows only 8192 occlusion queries per MTLBuffer. If enabled, MoltenVK allocates a MTLBuffer 
        /// for each query pool, allowing each query pool to support 8192 queries, which may slow 
        /// performance or cause unexpected behaviour if the query pool is not established prior to a 
        /// Metal renderpass, or if the query pool is changed within a Metal renderpass. If disabled, 
        /// one MTLBuffer will be shared by all query pools, which improves performance, but limits the total 
        /// device queries to 8192. Default value is true.
        /// </summary>
        public Bool SupportLargeQueryPools;
        /// <summary>
        /// If enabled, each surface presentation is scheduled using a command buffer. Enabling this may 
        /// improve rendering frame synchronization, but may result in reduced frame rates. Default value 
        /// is false if the MVK_PRESENT_WITHOUT_COMMAND_BUFFER build setting is defined when MoltenVK is 
        /// compiled, and true otherwise. By default the MVK_PRESENT_WITHOUT_COMMAND_BUFFER build setting 
        /// is not defined and the value of this setting is true.
        /// </summary>
        public Bool PresentWithCommandBuffer;
        /// <summary>
        /// If enabled, a MoltenVK logo watermark will be rendered on top of the scene. This can be
        /// enabled for publicity during demos. Default value is true if the MVK_DISPLAY_WATERMARK 
        /// build setting is defined when MoltenVK is compiled, and false otherwise. By default the 
        /// MVK_DISPLAY_WATERMARK build setting is not defined.
        /// </summary>
        public Bool DisplayWatermark;
        /// <summary>
        /// If enabled, per-frame performance statistics are tracked, optionally logged, and can be retrieved 
        /// via the vkGetSwapchainPerformanceMVK() function, and various performance statistics are tracked, 
        /// logged, and can be retrieved via the vkGetPerformanceStatisticsMVK() function. Default value 
        /// is true in the presence of the DEBUG build setting, and false otherwise.
        /// </summary>
        public Bool PerformanceTracking;
        /// <summary>
        /// If non-zero, performance statistics will be periodically logged to the console, on a repeating 
        /// cycle of this many frames per swapchain. The performanceTracking capability must also be enabled. 
        /// Default value is 300 in the presence of the DEBUG build setting, and zero otherwise.
        /// </summary>
        public UInt32 PerformanceLoggingFrameCount;
        /// <summary>
        /// The maximum amount of time, in nanoseconds, to wait for a Metal library, function or pipeline state 
        /// object to be compiled and created. If an internal error occurs with the Metal compiler, it can stall 
        /// the thread for up to 30 seconds. Setting this value limits the delay to that amount of time. 
        /// Default value is infinite.
        /// </summary>
        public UInt64 MetalCompileTimeout;
    }
}
