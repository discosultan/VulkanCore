using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

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
            Result result = CreateQueryPool(Parent, createInfo, NativeAllocator, &handle);
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
            Result result = GetQueryPoolResults(Parent, this, firstQuery, queryCount, dataSize, data, stride, flags);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Destroy a query pool object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) DestroyQueryPool(Parent, this, NativeAllocator);
            base.Dispose();
        }

        [DllImport(VulkanDll, EntryPoint = "vkCreateQueryPool", CallingConvention = CallConv)]
        private static extern Result CreateQueryPool(IntPtr device, 
            QueryPoolCreateInfo* CreateInfo, AllocationCallbacks.Native* allocator, long* queryPool);

        [DllImport(VulkanDll, EntryPoint = "vkDestroyQueryPool", CallingConvention = CallConv)]
        private static extern IntPtr DestroyQueryPool(IntPtr device, long queryPool, AllocationCallbacks.Native* allocator);

        [DllImport(VulkanDll, EntryPoint = "vkGetQueryPoolResults", CallingConvention = CallConv)]
        private static extern Result GetQueryPoolResults(IntPtr device, 
            long queryPool, int firstQuery, int queryCount, int dataSize, IntPtr data, long stride, QueryResults flags);
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
        /// The type of queries managed by the pool.
        /// </summary>
        public QueryType QueryType;
        /// <summary>
        /// The number of queries managed by the pool.
        /// </summary>
        public int QueryCount;
        /// <summary>
        /// A bitmask indicating which counters will be returned in queries on the new pool. Ignored
        /// if <see cref="QueryType"/> is not <see cref="VulkanCore.QueryType.PipelineStatistics"/>.
        /// </summary>
        public QueryPipelineStatistics PipelineStatistics;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPoolCreateInfo"/> structure.
        /// </summary>
        /// <param name="queryType">The type of queries managed by the pool.</param>
        /// <param name="queryCount">The number of queries managed by the pool.</param>
        /// <param name="pipelineStatistics">
        /// A bitmask indicating which counters will be returned in queries on the new pool. Ignored
        /// if <see cref="QueryType"/> is not <see cref="VulkanCore.QueryType.PipelineStatistics"/>
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
        Occlusion = 0,
        /// <summary>
        /// Optional.
        /// </summary>
        PipelineStatistics = 1,
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
        /// Results of the queries are written to the destination buffer as 64-bit values.
        /// </summary>
        Query64 = 1 << 0,
        /// <summary>
        /// Results of the queries are waited on before proceeding with the result copy.
        /// </summary>
        QueryWait = 1 << 1,
        /// <summary>
        /// Besides the results of the query, the availability of the results is also written.
        /// </summary>
        QueryWithAvailability = 1 << 2,
        /// <summary>
        /// Copy the partial results of the query even if the final results are not available.
        /// </summary>
        QueryPartial = 1 << 3
    }
}
