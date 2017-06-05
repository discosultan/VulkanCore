using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Ext
{
    /// <summary>
    /// Provides extension methods for the <see cref="Instance"/> class.
    /// </summary>
    public static unsafe class InstanceExtensions
    {
        /// <summary>
        /// Create a debug report callback object.
        /// </summary>
        /// <param name="instance">The instance the callback will be logged on.</param>
        /// <param name="createInfo">
        /// The structure which defines the conditions under which this callback will be called.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>A <see cref="DebugReportCallbackExt"/> handle.</returns>
        /// <exception cref="InvalidOperationException">Vulkan command not found.</exception>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static DebugReportCallbackExt CreateDebugReportCallbackExt(this Instance instance,
            DebugReportCallbackCreateInfoExt createInfo, AllocationCallbacks? allocator = null)
        {
            return new DebugReportCallbackExt(instance, ref createInfo, ref allocator);
        }

        /// <summary>
        /// To inject it's own messages into the debug stream an application uses this method.
        /// </summary>
        /// <param name="instance">The instance the callback will be logged on.</param>
        /// <param name="flags">
        /// Indicates the <see cref="DebugReportFlagsExt"/> that triggered this callback.
        /// </param>
        /// <param name="objectType">
        /// The type of object being used / created at the time the event was triggered.
        /// </param>
        /// <param name="object">
        /// Gives the object where the issue was detected. Object may be 0 if there is no object
        /// associated with the event.
        /// </param>
        /// <param name="location">
        /// A component (layer, driver, loader) defined value that indicates the "location" of the
        /// trigger. This is an optional value.
        /// </param>
        /// <param name="messageCode">
        /// A layer defined value indicating what test triggered this callback.
        /// </param>
        /// <param name="layerPrefix">Abbreviation of the component making the callback.</param>
        /// <param name="message">Unicode string detailing the trigger conditions.</param>
        /// <exception cref="InvalidOperationException">Vulkan command not found.</exception>
        public static void DebugReportMessageExt(this Instance instance, DebugReportFlagsExt flags,
            string message, DebugReportObjectTypeExt objectType = DebugReportObjectTypeExt.Unknown,
            long @object = 0, IntPtr location = default(IntPtr), int messageCode = 0, string layerPrefix = null)
        {
            int byteCount = Interop.String.GetMaxByteCount(layerPrefix);
            var layerPrefixBytes = stackalloc byte[byteCount];
            Interop.String.ToPointer(layerPrefix, layerPrefixBytes, byteCount);

            byteCount = Interop.String.GetMaxByteCount(message);
            var messageBytes = stackalloc byte[byteCount];
            Interop.String.ToPointer(message, messageBytes, byteCount);

            vkDebugReportMessageEXT(instance)
                (instance, flags, objectType, @object, location, messageCode, layerPrefixBytes, messageBytes);
        }

        private delegate void vkDebugReportMessageEXTDelegate(IntPtr instance, DebugReportFlagsExt flags,
            DebugReportObjectTypeExt objectType, long @object, IntPtr location, int messageCode, byte* layerPrefix, byte* message);
        private static vkDebugReportMessageEXTDelegate vkDebugReportMessageEXT(Instance instance) => instance.GetProc<vkDebugReportMessageEXTDelegate>(nameof(vkDebugReportMessageEXT));
    }

    /// <summary>
    /// Specify validation checks to disable for a Vulkan instance.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ValidationFlagsExt
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
        /// The number of checks to disable.
        /// </summary>
        public int DisabledValidationCheckCount;
        /// <summary>
        /// A pointer to an array of <see cref="ValidationCheckExt"/> values specifying the
        /// validation checks to be disabled.
        /// </summary>
        public IntPtr DisabledValidationChecks;
    }

    /// <summary>
    /// Specify validation checks to disable.
    /// </summary>
    public enum ValidationCheckExt
    {
        /// <summary>
        /// Specifies that all validation checks are disabled.
        /// </summary>
        All = 0,
        /// <summary>
        /// Specifies that shader validation is disabled.
        /// </summary>
        Shaders = 1
    }
}
