using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Ext
{
    /// <summary>
    /// Opaque handle to a validation cache object.
    /// <para>
    /// Validation cache objects allow the result of internal validation to be reused, both within a
    /// single application run and between multiple runs.
    /// </para>
    /// <para>
    /// Reuse within a single run is achieved by passing the same validation cache object when
    /// creating supported Vulkan objects.
    /// </para>
    /// <para>
    /// Reuse across runs of an application is achieved by retrieving validation cache contents in
    /// one run of an application, saving the contents, and using them to preinitialize a validation
    /// cache on a subsequent run.
    /// </para>
    /// <para>The contents of the validation cache objects are managed by the validation layers.</para>
    /// <para>
    /// Applications can manage the host memory consumed by a validation cache object and control the
    /// amount of data retrieved from a validation cache object.
    /// </para>
    /// </summary>
    public unsafe class ValidationCacheExt : DisposableHandle<long>
    {
        internal ValidationCacheExt(Device parent, ValidationCacheCreateInfoExt* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            createInfo->Prepare();

            long handle;
            Result result = vkCreateValidationCacheEXT(parent)(parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Get the data store from a validation cache.
        /// </summary>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public byte[] GetData()
        {
            int size;
            Result result = vkGetValidationCacheDataEXT(Parent)(Parent, this, &size, null);
            VulkanException.ThrowForInvalidResult(result);

            var data = new byte[size];
            fixed (byte* dataHandle = data)
            {
                result = vkGetValidationCacheDataEXT(Parent)(Parent, this, &size, (IntPtr*)dataHandle);
                VulkanException.ThrowForInvalidResult(result);
            }

            return data;
        }

        /// <summary>
        /// Combine the data stores of validation caches.
        /// </summary>
        /// <param name="srcCaches">
        /// An array of validation cache handles, which will be merged into this cache.
        /// </param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void Merge(ValidationCacheExt[] srcCaches)
        {
            int srcCacheCount = srcCaches?.Length ?? 0;
            long* srcCacheHandles = stackalloc long[srcCacheCount];
            for (int i = 0; i < srcCacheCount; i++)
                srcCacheHandles[i] = srcCacheHandles[i];
            Result result = vkMergeValidationCachesEXT(Parent)(Parent, this, srcCacheCount, srcCacheHandles);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Destroy a validation cache object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyValidationCacheEXT(Parent)(Parent, Handle, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateValidationCacheEXTDelegate(IntPtr device, ValidationCacheCreateInfoExt* createInfo, AllocationCallbacks.Native* allocator, long* validationCache);
        private static vkCreateValidationCacheEXTDelegate vkCreateValidationCacheEXT(Device device) => device.GetProc<vkCreateValidationCacheEXTDelegate>(nameof(vkCreateValidationCacheEXT));

        private delegate Result vkMergeValidationCachesEXTDelegate(IntPtr device, long dstCache, int srcCacheCount, long* srcCaches);
        private static vkMergeValidationCachesEXTDelegate vkMergeValidationCachesEXT(Device device) => device.GetProc<vkMergeValidationCachesEXTDelegate>(nameof(vkMergeValidationCachesEXT));

        private delegate void vkDestroyValidationCacheEXTDelegate(IntPtr device, long validationCache, AllocationCallbacks.Native* allocator);
        private static vkDestroyValidationCacheEXTDelegate vkDestroyValidationCacheEXT(Device device) => device.GetProc<vkDestroyValidationCacheEXTDelegate>(nameof(vkDestroyValidationCacheEXT));

        private delegate Result vkGetValidationCacheDataEXTDelegate(IntPtr device, long validationCache, int* dataSize, IntPtr* data);
        private static vkGetValidationCacheDataEXTDelegate vkGetValidationCacheDataEXT(Device device) => device.GetProc<vkGetValidationCacheDataEXTDelegate>(nameof(vkGetValidationCacheDataEXT));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created validation cache.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ValidationCacheCreateInfoExt
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        internal StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        internal ValidationCacheCreateFlags Flags;
        /// <summary>
        /// The number of bytes in <see cref="InitialData"/>. If <see cref="InitialDataSize"/> is
        /// zero, the validation cache will initially be empty.
        /// </summary>
        public Size InitialDataSize;
        /// <summary>
        /// Is a pointer to previously retrieved validation cache data. If the validation cache data
        /// is incompatible with the device, the validation cache will be initially empty. If <see
        /// cref="InitialDataSize"/> is zero, <see cref="InitialData"/> is ignored.
        /// </summary>
        public IntPtr InitialData;

        internal void Prepare()
        {
            Type = StructureType.ValidationCacheCreateInfoExt;
        }
    }

    internal enum ValidationCacheCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Encode validation cache version.
    /// </summary>
    public enum ValidationCacheHeaderVersionExt
    {
        /// <summary>
        /// Specifies version one of the validation cache.
        /// </summary>
        One = 1
    }
}
