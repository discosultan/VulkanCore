using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;

namespace VulkanCore.Mvk
{
    // TODO: doc
    /// <summary>
    /// Provides Brenwill Workshop specific extension methods for the <see cref="Instance"/> class.
    /// </summary>
    public static unsafe class InstanceExtensions
    {
        /// <summary>
        /// Create a <see cref="SurfaceKhr"/> object for an iOS UIView.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="createInfo"></param>
        /// <param name="allocator"></param>
        /// <returns></returns>
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
            Result result = vkCreateIOSSurfaceMVK(instance, &createInfo, nativeAllocator, &handle);

            Interop.Free(nativeAllocator);

            VulkanException.ThrowForInvalidResult(result);
            return new SurfaceKhr(instance, ref allocator, handle);
        }

        /// <summary>
        /// Create a <see cref="SurfaceKhr"/> object for a macOS NSView.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="createInfo"></param>
        /// <param name="allocator"></param>
        /// <returns></returns>
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
            Result result = vkCreateMacOSSurfaceMVK(instance, &createInfo, nativeAllocator, &handle);

            Interop.Free(nativeAllocator);

            VulkanException.ThrowForInvalidResult(result);
            return new SurfaceKhr(instance, ref allocator, handle);
        }

        private delegate Result vkCreateIOSSurfaceMVKDelegate(IntPtr instance, IOSSurfaceCreateInfoMvk* createInfo, AllocationCallbacks.Native* allocator, long* surface);
        private static readonly vkCreateIOSSurfaceMVKDelegate vkCreateIOSSurfaceMVK = VulkanLibrary.GetProc<vkCreateIOSSurfaceMVKDelegate>(nameof(vkCreateIOSSurfaceMVK));

        private delegate Result vkCreateMacOSSurfaceMVKDelegate(IntPtr instance, MacOSSurfaceCreateInfoMvk* createInfo, AllocationCallbacks.Native* allocator, long* surface);
        private static readonly vkCreateMacOSSurfaceMVKDelegate vkCreateMacOSSurfaceMVK = VulkanLibrary.GetProc<vkCreateMacOSSurfaceMVKDelegate>(nameof(vkCreateMacOSSurfaceMVK));
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
