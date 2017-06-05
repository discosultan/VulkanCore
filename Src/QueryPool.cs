using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a query pool object.
    /// <para>
    /// Queries are managed using query pool objects. Each query pool is a collection of a specific
    /// number of queries of a particular type.
    /// </para>
    /// </summary>
    public unsafe class QueryPool : DisposableHandle<long>
    {
        internal QueryPool(Device parent, QueryPoolCreateInfo* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            createInfo->Prepare();

            long handle;
            Result result = vkCreateQueryPool(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Copy results of queries in a query pool to a host memory region.
        /// </summary>
        /// <param name="firstQuery">The initial query index.</param>
        /// <param name="queryCount">
        /// The number of queries. <paramref name="firstQuery"/> and <paramref name="queryCount"/>
        /// together define a range of queries.
        /// </param>
        /// <param name="dataSize">The size in bytes of the buffer pointed to by <paramref name="data"/>.</param>
        /// <param name="data">A pointer to a user-allocated buffer where the results will be written.</param>
        /// <param name="stride">
        /// The stride in bytes between results for individual queries within <paramref name="data"/>.
        /// </param>
        /// <param name="flags">A bitmask specifying how and when results are returned.</param>
        public void GetResults(int firstQuery, int queryCount, int dataSize, IntPtr data, long stride, QueryResults flags = 0)
        {
            Result result = vkGetQueryPoolResults(Parent, this, firstQuery, queryCount, dataSize, data, stride, flags);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Destroy a query pool object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyQueryPool(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateQueryPoolDelegate(IntPtr device, QueryPoolCreateInfo* createInfo, AllocationCallbacks.Native* allocator, long* queryPool);
        private static readonly vkCreateQueryPoolDelegate vkCreateQueryPool = VulkanLibrary.GetStaticProc<vkCreateQueryPoolDelegate>(nameof(vkCreateQueryPool));

        private delegate void vkDestroyQueryPoolDelegate(IntPtr device, long queryPool, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyQueryPoolDelegate vkDestroyQueryPool = VulkanLibrary.GetStaticProc<vkDestroyQueryPoolDelegate>(nameof(vkDestroyQueryPool));

        private delegate Result vkGetQueryPoolResultsDelegate(IntPtr device, long queryPool, int firstQuery, int queryCount, int dataSize, IntPtr data, long stride, QueryResults flags);
        private static readonly vkGetQueryPoolResultsDelegate vkGetQueryPoolResults = VulkanLibrary.GetStaticProc<vkGetQueryPoolResultsDelegate>(nameof(vkGetQueryPoolResults));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created query pool.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct QueryPoolCreateInfo
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal QueryPoolCreateFlags Flags;

        /// <summary>
        /// Specifies the type of queries managed by the pool.
        /// </summary>
        public QueryType QueryType;
        /// <summary>
        /// The number of queries managed by the pool.
        /// </summary>
        public int QueryCount;
        /// <summary>
        /// A bitmask specifying which counters will be returned in queries on the new pool.
        /// <para>Ignored if <see cref="QueryType"/> is not <see cref="VulkanCore.QueryType.PipelineStatistics"/>.</para>
        /// </summary>
        public QueryPipelineStatistics PipelineStatistics;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPoolCreateInfo"/> structure.
        /// </summary>
        /// <param name="queryType">Specifies the type of queries managed by the pool.</param>
        /// <param name="queryCount">The number of queries managed by the pool.</param>
        /// <param name="pipelineStatistics">
        /// A bitmask specifying which counters will be returned in queries on the new pool.
        /// <para>Ignored if <see cref="QueryType"/> is not <see cref="VulkanCore.QueryType.PipelineStatistics"/>.</para>
        /// </param>
        public QueryPoolCreateInfo(QueryType queryType, int queryCount,
            QueryPipelineStatistics pipelineStatistics = QueryPipelineStatistics.None)
        {
            Type = StructureType.QueryPoolCreateInfo;
            Next = IntPtr.Zero;
            Flags = 0;
            QueryType = queryType;
            QueryCount = queryCount;
            PipelineStatistics = pipelineStatistics;
        }

        internal void Prepare()
        {
            Type = StructureType.QueryPoolCreateInfo;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum QueryPoolCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Specify the type of queries managed by a query pool.
    /// </summary>
    public enum QueryType
    {
        /// <summary>
        /// Specifies an occlusion query.
        /// </summary>
        Occlusion = 0,
        /// <summary>
        /// Specifies a pipeline statistics query.
        /// </summary>
        PipelineStatistics = 1,
        /// <summary>
        /// Specifies a timestamp query.
        /// </summary>
        Timestamp = 2
    }

    /// <summary>
    /// Bitmask specifying how and when query results are returned.
    /// </summary>
    [Flags]
    public enum QueryResults
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies the results will be written as an array of 64-bit unsigned integer values. If
        /// this bit is not set, the results will be written as an array of 32-bit unsigned integer values.
        /// </summary>
        Query64 = 1 << 0,
        /// <summary>
        /// Specifies that Vulkan will wait for each query's status to become available before
        /// retrieving its results.
        /// </summary>
        QueryWait = 1 << 1,
        /// <summary>
        /// Specifies that the availability status accompanies the results.
        /// </summary>
        QueryWithAvailability = 1 << 2,
        /// <summary>
        /// Specifies that returning partial results is acceptable.
        /// </summary>
        QueryPartial = 1 << 3
    }
}
