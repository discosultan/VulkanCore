using System;

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
    public unsafe class ValidationCacheExt : VulkanHandle<long>
    {
        private delegate Result vkCreateValidationCacheEXTDelegate(IntPtr device, ValidationCacheCreateInfoExt* createInfo, AllocationCallbacks.Native* allocator, long* validationCache);
        private static readonly vkCreateValidationCacheEXTDelegate vkCreateValidationCacheEXT = VulkanLibrary.GetProc<vkCreateValidationCacheEXTDelegate>(nameof(vkCreateValidationCacheEXT));

        private delegate Result vkMergeValidationCachesEXTDelegate(IntPtr device, long dstCache, int srcCacheCount, long* srcCaches);
        private static readonly vkMergeValidationCachesEXTDelegate vkMergeValidationCachesEXT = VulkanLibrary.GetProc<vkMergeValidationCachesEXTDelegate>(nameof(vkMergeValidationCachesEXT));

        private delegate IntPtr vkDestroyValidationCacheEXTDelegate(IntPtr device, long validationCache, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyValidationCacheEXTDelegate vkDestroyValidationCacheEXT = VulkanLibrary.GetProc<vkDestroyValidationCacheEXTDelegate>(nameof(vkDestroyValidationCacheEXT));

        private delegate Result vkGetValidationCacheDataEXTDelegate(IntPtr device, long validationCache, int* dataSize, IntPtr* data);
        private static readonly vkGetValidationCacheDataEXTDelegate vkGetValidationCacheDataEXT = VulkanLibrary.GetProc<vkGetValidationCacheDataEXTDelegate>(nameof(vkGetValidationCacheDataEXT));
    }
}
