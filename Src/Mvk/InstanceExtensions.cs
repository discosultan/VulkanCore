using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;

namespace VulkanCore.Mvk
{
    /// <summary>
    /// Provides Brenwill Workshop specific extension methods for the <see cref="Instance"/> class.
    /// </summary>
    public static unsafe class InstanceExtensions
    {
        /// <summary>
        /// Create a <see cref="SurfaceKhr"/> object for an iOS UIView.
        /// </summary>
        /// <param name="instance">The instance with which to associate the surface.</param>
        /// <param name="createInfo">
        /// Structure containing parameters affecting the creation of the surface object.
        /// </param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the surface object when there is no more
        /// specific allocator available
        /// </param>
        /// <returns>The created surface object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceKhr CreateIOSSurfaceMvk(this Instance instance,
            IOSSurfaceCreateInfoMvk createInfo, AllocationCallbacks? allocator = null)
        {
            createInfo.Prepare();
            AllocationCallbacks.Native* nativeAllocator = null;
            if (allocator.HasValue)
            {
                nativeAllocator = (AllocationCallbacks.Native*)Interop.Alloc<AllocationCallbacks.Native>();
                allocator.Value.ToNative(nativeAllocator);
            }

            long handle;
            Result result = vkCreateIOSSurfaceMVK(instance)(instance, &createInfo, nativeAllocator, &handle);

            Interop.Free(nativeAllocator);

            VulkanException.ThrowForInvalidResult(result);
            return new SurfaceKhr(instance, ref allocator, handle);
        }

        /// <summary>
        /// Create a <see cref="SurfaceKhr"/> object for a macOS NSView.
        /// </summary>
        /// <param name="instance">The instance with which to associate the surface.</param>
        /// <param name="createInfo">
        /// Structure containing parameters affecting the creation of the surface object.
        /// </param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the surface object when there is no more
        /// specific allocator available.
        /// </param>
        /// <returns>The created surface object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceKhr CreateMacOSSurfaceMvk(this Instance instance,
            MacOSSurfaceCreateInfoMvk createInfo, AllocationCallbacks? allocator = null)
        {
            createInfo.Prepare();
            AllocationCallbacks.Native* nativeAllocator = null;
            if (allocator.HasValue)
            {
                nativeAllocator = (AllocationCallbacks.Native*)Interop.Alloc<AllocationCallbacks.Native>();
                allocator.Value.ToNative(nativeAllocator);
            }

            long handle;
            Result result = vkCreateMacOSSurfaceMVK(instance)(instance, &createInfo, nativeAllocator, &handle);

            Interop.Free(nativeAllocator);

            VulkanException.ThrowForInvalidResult(result);
            return new SurfaceKhr(instance, ref allocator, handle);
        }

        private delegate Result vkCreateIOSSurfaceMVKDelegate(IntPtr instance, IOSSurfaceCreateInfoMvk* createInfo, AllocationCallbacks.Native* allocator, long* surface);
        private static vkCreateIOSSurfaceMVKDelegate vkCreateIOSSurfaceMVK(Instance instance) => instance.GetProc<vkCreateIOSSurfaceMVKDelegate>(nameof(vkCreateIOSSurfaceMVK));

        private delegate Result vkCreateMacOSSurfaceMVKDelegate(IntPtr instance, MacOSSurfaceCreateInfoMvk* createInfo, AllocationCallbacks.Native* allocator, long* surface);
        private static vkCreateMacOSSurfaceMVKDelegate vkCreateMacOSSurfaceMVK(Instance instance) => instance.GetProc<vkCreateMacOSSurfaceMVKDelegate>(nameof(vkCreateMacOSSurfaceMVK));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created iOS surface object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IOSSurfaceCreateInfoMvk
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;

        internal IOSSurfaceCreateFlagsMvk Flags;

        /// <summary>
        /// Must be a valid <c>UIView</c> and must be backed by a <c>CALayer</c> instance of type <c>CAMetalLayer</c>.
        /// </summary>
        public IntPtr View;

        internal void Prepare()
        {
            Type = StructureType.IOSSurfaceCreateInfoMvk;
        }
    }

    /// Is reserved for future use.
    [Flags]
    internal enum IOSSurfaceCreateFlagsMvk
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying parameters of a newly created macOS surface object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MacOSSurfaceCreateInfoMvk
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;

        internal MacOSSurfaceCreateFlagsMvk Flags;

        /// <summary>
        /// Must be a valid <c>NSView</c> and must be backed by a <c>CALayer</c> instance of type <c>CAMetalLayer</c>.
        /// </summary>
        public IntPtr View;

        /// <summary>
        /// Initializes a new instance of the <see cref="MacOSSurfaceCreateInfoMvk"/> structure.
        /// </summary>
        /// <param name="view">Pointer to a <c>NSView</c> that is backed by a <c>CALayer</c> instance of type <c>CAMetalLayer</c>.</param>
        public MacOSSurfaceCreateInfoMvk(IntPtr view)
        {
            View = view;
            Type = StructureType.MacOSSurfaceCreateInfoMvk;
            Next = IntPtr.Zero;
            Flags = 0;
        }

        internal void Prepare()
        {
            Type = StructureType.MacOSSurfaceCreateInfoMvk;
        }
    }

    /// Is reserved for future use.
    [Flags]
    internal enum MacOSSurfaceCreateFlagsMvk
    {
        None = 0
    }
}
