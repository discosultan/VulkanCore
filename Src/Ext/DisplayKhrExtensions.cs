using System;
using VulkanCore.Khr;

namespace VulkanCore.Ext
{
    /// <summary>
    /// Provides extension methods for the <see cref="DisplayKhr"/> class.
    /// </summary>
    public static unsafe class DisplayKhrExtensions
    {
        /// <summary>
        /// Acquire access to a DisplayKhr using Xlib.
        /// <para>
        /// All permissions necessary to control the display are granted to the Vulkan instance
        /// associated with <see cref="PhysicalDevice"/> until the display is released or the X11
        /// connection specified by <paramref name="dpy"/> is terminated.
        /// </para>
        /// <para>
        /// Permission to access the display may be temporarily revoked during periods when the X11
        /// server from which control was acquired itself looses access to <paramref name="display"/>.
        /// </para>
        /// <para>
        /// During such periods, operations which require access to the display must fail with an
        /// approriate error code.
        /// </para>
        /// <para>
        /// If the X11 server associated with <paramref name="dpy"/> does not own <paramref
        /// name="display"/>, or if permission to access it has already been acquired by another
        /// entity, the call must throw with the error code <see cref="Result.ErrorInitializationFailed"/>.
        /// </para>
        /// </summary>
        /// <param name="display"></param>
        /// <param name="dpy"></param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void AcquireXlibDisplayExt(this DisplayKhr display, IntPtr dpy)
        {
            Result result = vkAcquireXlibDisplayEXT(display)(display.Parent, &dpy, display);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Release access to an acquired DisplayKhr.
        /// </summary>
        /// <param name="display">The display to release control of.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static void ReleaseDisplayExt(this DisplayKhr display)
        {
            Result result = vkReleaseDisplayEXT(display)(display.Parent, display);
            VulkanException.ThrowForInvalidResult(result);
        }

        private delegate Result vkAcquireXlibDisplayEXTDelegate(IntPtr physicalDevice, IntPtr* dpy, long display);
        private static vkAcquireXlibDisplayEXTDelegate vkAcquireXlibDisplayEXT(DisplayKhr display) => GetProc<vkAcquireXlibDisplayEXTDelegate>(display, nameof(vkAcquireXlibDisplayEXT));

        private delegate Result vkReleaseDisplayEXTDelegate(IntPtr physicalDevice, long display);
        private static vkReleaseDisplayEXTDelegate vkReleaseDisplayEXT(DisplayKhr display) => GetProc<vkReleaseDisplayEXTDelegate>(display, nameof(vkReleaseDisplayEXT));

        private static TDelegate GetProc<TDelegate>(DisplayKhr display, string name) where TDelegate : class => display.Parent.Parent.GetProc<TDelegate>(name);
    }
}
