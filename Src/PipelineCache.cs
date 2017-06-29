using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a pipeline cache object.
    /// <para>
    /// Pipeline cache objects allow the result of pipeline construction to be reused between
    /// pipelines and between runs of an application. Reuse between pipelines is achieved by passing
    /// the same pipeline cache object when creating multiple related pipelines. Reuse across runs of
    /// an application is achieved by retrieving pipeline cache contents in one run of an
    /// application, saving the contents, and using them to preinitialize a pipeline cache on a
    /// subsequent run. The contents of the pipeline cache objects are managed by the implementation.
    /// Applications can manage the host memory consumed by a pipeline cache object and control the
    /// amount of data retrieved from a pipeline cache object.
    /// </para>
    /// </summary>
    public unsafe class PipelineCache : DisposableHandle<long>
    {
        internal PipelineCache(Device parent, ref PipelineCacheCreateInfo createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            fixed (byte* initialDataPtr = createInfo.InitialData)
            {
                createInfo.ToNative(out PipelineCacheCreateInfo.Native nativeCreateInfo, initialDataPtr);
                long handle;
                Result result = vkCreatePipelineCache(Parent, &nativeCreateInfo, NativeAllocator, &handle);
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Get the data store from a pipeline cache.
        /// </summary>
        /// <returns>Buffer.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public byte[] GetData()
        {
            int size;
            Result result = vkGetPipelineCacheData(Parent, this, &size, null);
            VulkanException.ThrowForInvalidResult(result);

            var data = new byte[size];
            fixed (byte* dataPtr = data)
                result = vkGetPipelineCacheData(Parent, this, &size, dataPtr);
            VulkanException.ThrowForInvalidResult(result);
            return data;
        }

        /// <summary>
        /// Combine the data stores of pipeline caches.
        /// </summary>
        /// <param name="sourceCache">Pipeline cache to merge into this.</param>
        public void MergeCache(PipelineCache sourceCache)
        {
            long handle = sourceCache;
            Result result = vkMergePipelineCaches(Parent, this, 1, &handle);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Combine the data stores of pipeline caches.
        /// </summary>
        /// <param name="sourceCaches">Pipeline caches to merge into this.</param>
        public void MergeCaches(params PipelineCache[] sourceCaches)
        {
            int count = sourceCaches?.Length ?? 0;

            var sourceCachesPtr = stackalloc long[count];
            for (int i = 0; i < count; i++)
                sourceCachesPtr[i] = sourceCaches[i].Handle;

            Result result = vkMergePipelineCaches(Parent, this, count, sourceCachesPtr);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Destroy a pipeline cache object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyPipelineCache(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreatePipelineCacheDelegate(IntPtr device, PipelineCacheCreateInfo.Native* createInfo, AllocationCallbacks.Native* allocator, long* pipelineCache);
        private static readonly vkCreatePipelineCacheDelegate vkCreatePipelineCache = VulkanLibrary.GetStaticProc<vkCreatePipelineCacheDelegate>(nameof(vkCreatePipelineCache));

        private delegate void vkDestroyPipelineCacheDelegate(IntPtr device, long pipelineCache, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyPipelineCacheDelegate vkDestroyPipelineCache = VulkanLibrary.GetStaticProc<vkDestroyPipelineCacheDelegate>(nameof(vkDestroyPipelineCache));

        private delegate Result vkGetPipelineCacheDataDelegate(IntPtr device, long pipelineCache, int* dataSize, byte* data);
        private static readonly vkGetPipelineCacheDataDelegate vkGetPipelineCacheData = VulkanLibrary.GetStaticProc<vkGetPipelineCacheDataDelegate>(nameof(vkGetPipelineCacheData));

        private delegate Result vkMergePipelineCachesDelegate(IntPtr device, long dstCache, int srcCacheCount, long* srcCaches);
        private static readonly vkMergePipelineCachesDelegate vkMergePipelineCaches = VulkanLibrary.GetStaticProc<vkMergePipelineCachesDelegate>(nameof(vkMergePipelineCaches));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created pipeline cache.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct PipelineCacheCreateInfo
    {
        /// <summary>
        /// Previously retrieved pipeline cache data. If the pipeline cache data is incompatible with
        /// the device, the pipeline cache will be initially empty. If length is zero, <see
        /// cref="InitialData"/> is ignored.
        /// </summary>
        public byte[] InitialData;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineCacheCreateInfo"/> structure.
        /// </summary>
        /// <param name="initialData">
        /// Previously retrieved pipeline cache data. If the pipeline cache data is incompatible with
        /// the device, the pipeline cache will be initially empty. If length is zero, <see
        /// cref="InitialData"/> is ignored.
        /// </param>
        public PipelineCacheCreateInfo(byte[] initialData)
        {
            InitialData = initialData;
        }

        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PipelineCacheCreateFlags Flags;
            public int InitialDataSize;
            public byte* InitialData;
        }

        internal void ToNative(out Native native, byte* initialData)
        {
            native.Type = StructureType.PipelineCacheCreateInfo;
            native.Next = IntPtr.Zero;
            native.Flags = 0;
            native.InitialDataSize = InitialData?.Length ?? 0;
            native.InitialData = initialData;
        }
    }

    // Is reserved for future use.
    internal enum PipelineCacheCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Encode pipeline cache version.
    /// </summary>
    public enum PipelineCacheHeaderVersion
    {
        /// <summary>
        /// Specifies version one of the pipeline cache.
        /// </summary>
        One = 1
    }
}
