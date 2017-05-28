using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Khr
{
    /// <summary>
    /// Opaque handle to a display mode object.
    /// <para>There is currently no way to destroy "built in" modes.</para>
    /// </summary>
    public unsafe class DisplayModeKhr : DisposableHandle<long>
    {
        internal DisplayModeKhr(DisplayKhr parent, DisplayModeCreateInfoKhr* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            long handle;
            Result result = vkCreateDisplayModeKHR(Parent.Parent, Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public DisplayKhr Parent { get; }

        /// <summary>
        /// Query capabilities of a mode and plane combination.
        /// <para>
        /// Applications that wish to present directly to a display must select which layer, or
        /// "plane" of the display they wish to target, and a mode to use with the display.
        /// </para>
        /// <para>Each display supports at least one plane.</para>
        /// </summary>
        /// <param name="planeIndex">
        /// The plane which the application intends to use with the display, and is less than the
        /// number of display planes supported by the device.
        /// </param>
        /// <returns>The structure in which the capabilities are returned.</returns>
        public DisplayPlaneCapabilitiesKhr GetDisplayPlaneCapabilities(int planeIndex)
        {
            DisplayPlaneCapabilitiesKhr capabilities;
            Result result = vkGetDisplayPlaneCapabilitiesKHR(Parent.Parent, this, planeIndex, &capabilities);
            VulkanException.ThrowForInvalidResult(result);
            return capabilities;
        }

        private delegate Result vkCreateDisplayModeKHRDelegate(IntPtr physicalDevice, long display, DisplayModeCreateInfoKhr* createInfo, AllocationCallbacks.Native* allocator, long* mode);
        private static readonly vkCreateDisplayModeKHRDelegate vkCreateDisplayModeKHR = VulkanLibrary.GetStaticProc<vkCreateDisplayModeKHRDelegate>(nameof(vkCreateDisplayModeKHR));

        private delegate Result vkGetDisplayPlaneCapabilitiesKHRDelegate(IntPtr physicalDevice, long mode, int planeIndex, DisplayPlaneCapabilitiesKhr* capabilities);
        private static readonly vkGetDisplayPlaneCapabilitiesKHRDelegate vkGetDisplayPlaneCapabilitiesKHR = VulkanLibrary.GetStaticProc<vkGetDisplayPlaneCapabilitiesKHRDelegate>(nameof(vkGetDisplayPlaneCapabilitiesKHR));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created display mode object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayModeCreateInfoKhr
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal DisplayModeCreateFlagsKhr Flags;

        /// <summary>
        /// A structure describing the display parameters to use in creating the new mode. If the
        /// parameters are not compatible with the specified display, the implementation must throw
        /// with <see cref="Result.ErrorInitializationFailed"/>.
        /// </summary>
        public DisplayModeParametersKhr Parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayModeParametersKhr"/> structure.
        /// </summary>
        /// <param name="parameters">
        /// A structure describing the display parameters to use in creating the new mode. If the
        /// parameters are not compatible with the specified display, the implementation must throw
        /// with <see cref="Result.ErrorInitializationFailed"/>.
        /// </param>
        public DisplayModeCreateInfoKhr(DisplayModeParametersKhr parameters)
        {
            Type = StructureType.DisplayModeCreateInfoKhr;
            Next = IntPtr.Zero;
            Flags = 0;
            Parameters = parameters;
        }

        internal void Prepare()
        {
            Type = StructureType.DisplayModeCreateInfoKhr;
        }
    }

    [Flags]
    internal enum DisplayModeCreateFlagsKhr
    {
        None = 0
    }
}
