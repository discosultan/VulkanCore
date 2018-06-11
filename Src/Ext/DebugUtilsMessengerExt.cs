using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Ext
{
    /// <summary>
    /// Opaque handle to a debug messenger object.
    /// <para>
    /// The debug messenger will provide detailed feedback on the application's use of Vulkan when
    /// events of interest occur.
    /// </para>
    /// <para>
    /// When an event of interest does occur, the debug messenger will submit a debug message to the
    /// debug callback that was provided during its creation.
    /// </para>
    /// <para>
    /// Additionally, the debug messenger is responsible with filtering out debug messages that the
    /// callback isn't interested in and will only provide desired debug messages.
    /// </para>
    /// </summary>
    public unsafe class DebugUtilsMessengerExt : DisposableHandle<long>
    {
        internal DebugUtilsMessengerExt(Instance parent, DebugUtilsMessengerCreateInfoExt* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            long handle;
            Result result = vkCreateDebugUtilsMessengerEXT(parent)(parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Instance Parent { get; }

        /// <summary>
        /// Destroy a debug messenger object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyDebugUtilsMessengerEXT(Parent)(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateDebugUtilsMessengerEXTDelegate(IntPtr instance, DebugUtilsMessengerCreateInfoExt* createInfo, AllocationCallbacks.Native* allocator, long* messenger);
        private static vkCreateDebugUtilsMessengerEXTDelegate vkCreateDebugUtilsMessengerEXT(Instance instance) => instance.GetProc<vkCreateDebugUtilsMessengerEXTDelegate>(nameof(vkCreateDebugUtilsMessengerEXT));

        private delegate IntPtr vkDestroyDebugUtilsMessengerEXTDelegate(IntPtr instance, long messenger, AllocationCallbacks.Native* allocator);
        private static vkDestroyDebugUtilsMessengerEXTDelegate vkDestroyDebugUtilsMessengerEXT(Instance instance) => instance.GetProc<vkDestroyDebugUtilsMessengerEXTDelegate>(nameof(vkDestroyDebugUtilsMessengerEXT));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created debug messenger.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DebugUtilsMessengerCreateInfoExt
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
        /// Is 0 and reserved for future use.
        /// </summary>
        public int Flags;
        /// <summary>
        /// Specifies which severity of event(s) will cause this callback to be called.
        /// </summary>
        public DebugUtilsMessageSeverityFlagsExt MessageSeverity;
        public int MessageType;
        /// <summary>
        /// The application callback function to call.
        /// </summary>
        public IntPtr PfnUserCallback;
        /// <summary>
        /// User data to be passed to the callback.
        /// </summary>
        public IntPtr UserData;
    }

    /// <summary>
    /// Bitmask specifying which severities of events cause a debug messenger callback.
    /// </summary>
    [Flags]
    public enum DebugUtilsMessageSeverityFlagsExt
    {
        /// <summary>
        /// Specifies the most verbose output indicating all diagnostic messages from the Vulkan
        /// loader, layers, and drivers should be captured.
        /// </summary>
        Verbose = 1 << 0,
        /// <summary>
        /// Specifies an informational message such as resource details that may be handy when
        /// debugging an application.
        /// </summary>
        Info = 1 << 4,
        /// <summary>
        /// Specifies use of Vulkan that may: expose an app bug. Such cases may not be immediately
        /// harmful, such as a fragment shader outputting to a location with no attachment. Other
        /// cases may: point to behavior that is almost certainly bad when unintended such as using
        /// an image whose memory has not been filled. In general if you see a warning but you know
        /// that the behavior is intended/desired, then simply ignore the warning.
        /// </summary>
        Warning = 1 << 8,
        /// <summary>
        /// Specifies that an error that may cause undefined results, including an application crash.
        /// </summary>
        Error = 1 << 12
    }
}
