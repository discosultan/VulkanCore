using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;

namespace VulkanCore.NN
{
    /// <summary>
    /// Provides Nintendo specific extension methods for the <see cref="Instance"/> class.
    /// </summary>
    public static unsafe class InstanceExtensions
    {
        /// <summary>
        /// Create a <see cref="SurfaceKhr"/> object for a VI layer.
        /// <para>
        /// During the lifetime of a surface created using a particular `NativeWindowHandle`, any
        /// attempts to create another surface for the same `Layer` and any attempts to connect to
        /// the same layer through other platform mechanisms will have undefined results.
        /// </para>
        /// <para>
        /// The <see cref="SurfaceCapabilitiesKhr.CurrentExtent"/> of a VI surface is always
        /// undefined. Applications are expected to choose an appropriate size for the swapchain's
        /// <see cref="SwapchainCreateInfoKhr.ImageExtent"/> (e.g., by matching the the result of a
        /// call to `GetDisplayResolution`).
        /// </para>
        /// </summary>
        /// <param name="instance">The <see cref="Instance"/> to associate with the surface.</param>
        /// <param name="createInfo">
        /// The structure containing the parameters affecting the creation of the surface object.
        /// </param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the surface object.
        /// </param>
        /// <returns>The resulting surface object handle.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceKhr CreateVISurfaceNN(this Instance instance,
            VISurfaceCreateInfoNN createInfo, AllocationCallbacks? allocator = null)
        {
            createInfo.Prepare();
            AllocationCallbacks.Native* nativeAllocator = null;
            if (allocator.HasValue)
            {
                nativeAllocator = (AllocationCallbacks.Native*)Interop.Alloc<AllocationCallbacks.Native>();
                allocator.Value.ToNative(nativeAllocator);
            }

            long handle;
            Result result = vkCreateViSurfaceNN(instance)(instance, &createInfo, nativeAllocator, &handle);

            Interop.Free(nativeAllocator);

            VulkanException.ThrowForInvalidResult(result);
            return new SurfaceKhr(instance, ref allocator, handle);
        }

        private delegate Result vkCreateViSurfaceNNDelegate(IntPtr instance, VISurfaceCreateInfoNN* createInfo, AllocationCallbacks.Native* allocator, long* surface);
        private static vkCreateViSurfaceNNDelegate vkCreateViSurfaceNN(Instance instance) => instance.GetProc<vkCreateViSurfaceNNDelegate>(nameof(vkCreateViSurfaceNN));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created VI surface object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VISurfaceCreateInfoNN
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal VISurfaceCreateFlagsNN Flags;

        /// <summary>
        /// The `NativeWindowHandle` for the `Layer` with which to associate the surface.
        /// </summary>
        public IntPtr Window;

        internal void Prepare()
        {
            Type = StructureType.VISurfaceCreateInfoNN;
        }
    }

    [Flags]
    internal enum VISurfaceCreateFlagsNN
    {
        None = 0
    }
}
