using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a device object.
    /// </summary>
    public unsafe class Device : DisposableHandle<IntPtr>
    {
        private readonly ConcurrentDictionary<string, IntPtr> _procAddrCache
            = new ConcurrentDictionary<string, IntPtr>(StringComparer.Ordinal);

        private readonly ConcurrentDictionary<Type, object> _procCache
            = new ConcurrentDictionary<Type, object>();

        internal Device(PhysicalDevice parent, ref DeviceCreateInfo createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            createInfo.ToNative(out DeviceCreateInfo.Native nativeCreateInfo);
            IntPtr handle;
            Result result = vkCreateDevice(Parent.Handle, &nativeCreateInfo, NativeAllocator, &handle);
            nativeCreateInfo.Free();

            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public PhysicalDevice Parent { get; }

        /// <summary>
        /// Return a function handle for a command or <see cref="IntPtr.Zero"/> if not found.
        /// <para>
        /// In order to support systems with multiple Vulkan implementations comprising heterogeneous
        /// collections of hardware and software, the function pointers returned by <see
        /// cref="GetProcAddr"/> may point to dispatch code, which calls a different real
        /// implementation for different <see cref="Device"/> objects (and objects created from
        /// them). The overhead of this internal dispatch can be avoided by obtaining device-specific
        /// function pointers for any commands that use a device or device-child object as their
        /// dispatchable object.
        /// </para>
        /// </summary>
        /// <param name="name">The name of the command to obtain.</param>
        /// <returns>Function handle for a command or <see cref="IntPtr.Zero"/> if not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        public IntPtr GetProcAddr(string name)
        {
            if (!_procAddrCache.TryGetValue(name, out IntPtr addr))
            {
                int byteCount = Interop.String.GetMaxByteCount(name);
                var dstPtr = stackalloc byte[byteCount];
                Interop.String.ToPointer(name, dstPtr, byteCount);
                addr = vkGetDeviceProcAddr(this, dstPtr);
                _procAddrCache.TryAdd(name, addr);
            }
            return addr;
        }

        /// <summary>
        /// Return a function delegate for a command or <c>null</c> if not found.
        /// <para>
        /// In order to support systems with multiple Vulkan implementations comprising heterogeneous
        /// collections of hardware and software, the function delegates returned by <see
        /// cref="GetProc{TDelegate}"/> may point to dispatch code, which calls a different real
        /// implementation for different <see cref="Device"/> objects (and objects created from
        /// them). The overhead of this internal dispatch can be avoided by obtaining device-specific
        /// function delegate for any commands that use a device or device-child object as their
        /// dispatchable object.
        /// </para>
        /// </summary>
        /// <param name="name">The name of the command to obtain.</param>
        /// <returns>Function delegate for a command or <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        public TDelegate GetProc<TDelegate>(string name) where TDelegate : class
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (_procCache.TryGetValue(typeof(TDelegate), out object cachedProc))
                return (TDelegate)cachedProc;

            IntPtr ptr = GetProcAddr(name);
            TDelegate proc = ptr != IntPtr.Zero
                ? Interop.GetDelegateForFunctionPointer<TDelegate>(ptr)
                : null;
            _procCache.TryAdd(typeof(TDelegate), proc);
            return proc;
        }

        /// <summary>
        /// Get a queue handle from a device.
        /// </summary>
        /// <param name="queueFamilyIndex">
        /// The index of the queue family to which the queue belongs. Must be one of the queue family
        /// indices specified when device was created, via the <see cref="DeviceQueueCreateInfo"/> structure.
        /// </param>
        /// <param name="queueIndex">
        /// The index within this queue family of the queue to retrieve. Must be less than the number
        /// of queues created for the specified queue family index when device was created, via the
        /// length of <see cref="DeviceQueueCreateInfo.QueuePriorities"/>.
        /// </param>
        /// <returns>Handle to a queue.</returns>
        public Queue GetQueue(int queueFamilyIndex, int queueIndex = 0)
        {
            IntPtr handle;
            vkGetDeviceQueue(Handle, queueFamilyIndex, queueIndex, &handle);
            return new Queue(handle, this, queueFamilyIndex, queueIndex);
        }

        /// <summary>
        /// Wait for a device to become idle.
        /// <para>Equivalent to calling <see cref="Queue.WaitIdle"/> for all queues owned by device.</para>
        /// </summary>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void WaitIdle()
        {
            Result result = vkDeviceWaitIdle(this);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Create a new buffer object.
        /// </summary>
        /// <param name="createInfo">
        /// The structure containing parameters affecting creation of the buffer.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>Buffer object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Buffer CreateBuffer(BufferCreateInfo createInfo, AllocationCallbacks? allocator = null)
        {
            return new Buffer(this, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Create a new image object.
        /// </summary>
        /// <param name="createInfo">
        /// The structure containing parameters to be used to create the image.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Image CreateImage(ImageCreateInfo createInfo, AllocationCallbacks? allocator = null)
        {
            return new Image(this, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Allocate GPU memory.
        /// </summary>
        /// <param name="allocateInfo">
        /// The structure describing parameters of the allocation. A successful returned allocation
        /// must use the requested parameters — no substitution is permitted by the implementation.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public DeviceMemory AllocateMemory(MemoryAllocateInfo allocateInfo, AllocationCallbacks? allocator = null)
        {
            return new DeviceMemory(this, &allocateInfo, ref allocator);
        }

        /// <summary>
        /// Flush mapped memory range.
        /// <para>
        /// Must be used to guarantee that host writes to non-coherent memory are visible to the
        /// device. It must be called after the host writes to non-coherent memory have completed and
        /// before command buffers that will read or write any of those memory locations are
        /// submitted to a queue.
        /// </para>
        /// <para>
        /// Unmapping non-coherent memory does not implicitly flush the mapped memory, and host
        /// writes that have not been flushed may not ever be visible to the device.
        /// </para>
        /// </summary>
        /// <param name="memoryRange">Structure describing the memory range to flush.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void FlushMappedMemoryRange(MappedMemoryRange memoryRange)
        {
            memoryRange.Prepare();
            Result result = vkFlushMappedMemoryRanges(this, 1, &memoryRange);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Flush mapped memory ranges.
        /// <para>
        /// Must be used to guarantee that host writes to non-coherent memory are visible to the
        /// device. It must be called after the host writes to non-coherent memory have completed and
        /// before command buffers that will read or write any of those memory locations are
        /// submitted to a queue.
        /// </para>
        /// <para>
        /// Unmapping non-coherent memory does not implicitly flush the mapped memory, and host
        /// writes that have not been flushed may not ever be visible to the device.
        /// </para>
        /// </summary>
        /// <param name="memoryRanges">Structures describing the memory ranges to flush.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void FlushMappedMemoryRanges(params MappedMemoryRange[] memoryRanges)
        {
            int count = memoryRanges?.Length ?? 0;

            for (int i = 0; i < count; i++)
                memoryRanges[i].Prepare();

            fixed (MappedMemoryRange* memoryRangesPtr = memoryRanges)
            {
                Result result = vkFlushMappedMemoryRanges(this, count, memoryRangesPtr);
                VulkanException.ThrowForInvalidResult(result);
            }
        }

        /// <summary>
        /// Invalidate a range of mapped memory.
        /// <para>
        /// Must be used to guarantee that device writes to non-coherent memory are visible to the
        /// host. It must be called after command buffers that execute and flush (via memory
        /// barriers) the device writes have completed, and before the host will read or write any of
        /// those locations. If a range of non-coherent memory is written by the host and then
        /// invalidated without first being flushed, its contents are undefined.
        /// </para>
        /// <para>
        /// Mapping non-coherent memory does not implicitly invalidate the mapped memory, and device
        /// writes that have not been invalidated must be made visible before the host reads or
        /// overwrites them.
        /// </para>
        /// </summary>
        /// <param name="memoryRange">Structure describing the memory range to invalidate.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void InvalidateMappedMemoryRange(MappedMemoryRange memoryRange)
        {
            memoryRange.Prepare();
            Result result = vkInvalidateMappedMemoryRanges(this, 1, &memoryRange);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Invalidate ranges of mapped memory objects.
        /// <para>
        /// Must be used to guarantee that device writes to non-coherent memory are visible to the
        /// host. It must be called after command buffers that execute and flush (via memory
        /// barriers) the device writes have completed, and before the host will read or write any of
        /// those locations. If a range of non-coherent memory is written by the host and then
        /// invalidated without first being flushed, its contents are undefined.
        /// </para>
        /// <para>
        /// Mapping non-coherent memory does not implicitly invalidate the mapped memory, and device
        /// writes that have not been invalidated must be made visible before the host reads or
        /// overwrites them.
        /// </para>
        /// </summary>
        /// <param name="memoryRanges">Structures describing the memory ranges to invalidate.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void InvalidateMappedMemoryRanges(params MappedMemoryRange[] memoryRanges)
        {
            int count = memoryRanges?.Length ?? 0;

            for (int i = 0; i < count; i++)
                memoryRanges[i].Prepare();

            fixed (MappedMemoryRange* memoryRangesPtr = memoryRanges)
            {
                Result result = vkInvalidateMappedMemoryRanges(this, count, memoryRangesPtr);
                VulkanException.ThrowForInvalidResult(result);
            }
        }

        /// <summary>
        /// Creates a new shader module object.
        /// </summary>
        /// <param name="createInfo">
        /// The structure containing information of a newly created shader module.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>Resulting shader module object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public ShaderModule CreateShaderModule(ShaderModuleCreateInfo createInfo, AllocationCallbacks? allocator = null)
        {
            return new ShaderModule(this, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Creates a new pipeline cache.
        /// </summary>
        /// <param name="createInfo">
        /// The structure containing information of a newly created pipeline cache.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>Resulting pipeline cache.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public PipelineCache CreatePipelineCache(PipelineCacheCreateInfo createInfo = default(PipelineCacheCreateInfo),
            AllocationCallbacks? allocator = null)
        {
            return new PipelineCache(this, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Creates a new compute pipeline object.
        /// </summary>
        /// <param name="createInfo">
        /// Structure specifying parameters of a newly created compute pipeline.
        /// </param>
        /// <param name="cache">
        /// Is either <c>null</c>, indicating that pipeline caching is disabled; or the handle of a
        /// valid pipeline cache object, in which case use of that cache is enabled for the duration
        /// of the command.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting compute pipeline object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Pipeline CreateComputePipeline(ComputePipelineCreateInfo createInfo,
            PipelineCache cache = null, AllocationCallbacks? allocator = null)
        {
            return new Pipeline(this, cache, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Create new compute pipeline objects.
        /// </summary>
        /// <param name="createInfos">
        /// Structures specifying parameters of newly created compute pipelines.
        /// <para>
        /// If the flags member of any given element contains the <see
        /// cref="PipelineCreateFlags.Derivative"/> flag, and the <see
        /// cref="ComputePipelineCreateInfo.BasePipelineIndex"/> member of that same element is not
        /// -1, <see cref="ComputePipelineCreateInfo.BasePipelineIndex"/> must be less than the index
        /// into <paramref name="createInfos"/> that corresponds to that element
        /// </para>
        /// </param>
        /// <param name="cache">
        /// Is either <c>null</c>, indicating that pipeline caching is disabled; or the handle of a
        /// valid pipeline cache object, in which case use of that cache is enabled for the duration
        /// of the command.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>An array in which the resulting compute pipeline objects are returned.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Pipeline[] CreateComputePipelines(ComputePipelineCreateInfo[] createInfos,
            PipelineCache cache = null, AllocationCallbacks? allocator = null)
        {
            return Pipeline.CreateComputePipelines(this, cache, createInfos, ref allocator);
        }

        /// <summary>
        /// Create a graphics pipeline.
        /// </summary>
        /// <param name="createInfo">
        /// Structure specifying parameters of a newly created graphics pipeline.
        /// </param>
        /// <param name="cache">
        /// Is either <c>null</c>, indicating that pipeline caching is disabled; or the handle of a
        /// valid pipeline cache object, in which case use of that cache is enabled for the duration
        /// of the command.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting graphics pipeline object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Pipeline CreateGraphicsPipeline(GraphicsPipelineCreateInfo createInfo,
            PipelineCache cache = null, AllocationCallbacks? allocator = null)
        {
            return new Pipeline(this, cache, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Create graphics pipelines.
        /// </summary>
        /// <param name="createInfos">
        /// Structures specifying parameters of newly created graphics pipelines.
        /// <para>
        /// If the flags member of any given element contains the <see
        /// cref="PipelineCreateFlags.Derivative"/> flag, and the <see
        /// cref="GraphicsPipelineCreateInfo.BasePipelineIndex"/> member of that same element is not
        /// -1, <see cref="GraphicsPipelineCreateInfo.BasePipelineIndex"/> must be less than the index
        /// into <paramref name="createInfos"/> that corresponds to that element
        /// </para>
        /// </param>
        /// <param name="cache">
        /// Is either <c>null</c>, indicating that pipeline caching is disabled; or the handle of a
        /// valid pipeline cache object, in which case use of that cache is enabled for the duration
        /// of the command.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>An array in which the resulting graphics pipeline objects are returned.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Pipeline[] CreateGraphicsPipelines(GraphicsPipelineCreateInfo[] createInfos,
            PipelineCache cache = null, AllocationCallbacks? allocator = null)
        {
            return Pipeline.CreateGraphicsPipelines(this, cache, createInfos, ref allocator);
        }

        /// <summary>
        /// Creates a new pipeline layout object.
        /// </summary>
        /// <param name="createInfo">
        /// The structure specifying the state of the pipeline layout object.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>Handle in which the resulting pipeline layout object is returned.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public PipelineLayout CreatePipelineLayout(
            PipelineLayoutCreateInfo createInfo = default(PipelineLayoutCreateInfo),
            AllocationCallbacks? allocator = null)
        {
            return new PipelineLayout(this, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Create a new sampler object.
        /// </summary>
        /// <param name="createInfo">The structure specifying the state of the sampler object.</param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>Resulting sampler object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Sampler CreateSampler(SamplerCreateInfo createInfo, AllocationCallbacks? allocator = null)
        {
            return new Sampler(this, &createInfo, ref allocator);
        }

        /// <summary>
        /// Create a new descriptor set layout.
        /// </summary>
        /// <param name="createInfo">
        /// The structure specifying the state of the descriptor set layout object.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting descriptor set layout object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public DescriptorSetLayout CreateDescriptorSetLayout(DescriptorSetLayoutCreateInfo createInfo,
            AllocationCallbacks? allocator = null)
        {
            return new DescriptorSetLayout(this, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Creates a descriptor pool object.
        /// </summary>
        /// <param name="createInfo">
        /// The structure specifying the state of the descriptor pool object.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting descriptor pool object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public DescriptorPool CreateDescriptorPool(DescriptorPoolCreateInfo createInfo,
            AllocationCallbacks? allocator = null)
        {
            return new DescriptorPool(this, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Create a new render pass object.
        /// </summary>
        /// <param name="createInfo">
        /// The structure that describes the parameters of the render pass.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting render pass object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public RenderPass CreateRenderPass(RenderPassCreateInfo createInfo, AllocationCallbacks? allocator = null)
        {
            return new RenderPass(this, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Create a new command pool object.
        /// </summary>
        /// <param name="createInfo">Contains information used to create the command pool.</param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The created pool.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public CommandPool CreateCommandPool(CommandPoolCreateInfo createInfo, AllocationCallbacks? allocator = null)
        {
            return new CommandPool(this, &createInfo, ref allocator);
        }

        /// <summary>
        /// Create a new event object.
        /// </summary>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting event object.</returns>
        public Event CreateEvent(AllocationCallbacks? allocator = null)
        {
            return new Event(this, ref allocator);
        }

        /// <summary>
        /// Create a new fence object.
        /// </summary>
        /// <param name="createInfo">
        /// The structure which contains information about how the fence is to be created.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting fence object.</returns>
        public Fence CreateFence(FenceCreateInfo createInfo = default(FenceCreateInfo),
            AllocationCallbacks? allocator = null)
        {
            return new Fence(this, &createInfo, ref allocator);
        }

        /// <summary>
        /// Resets one or more fence objects.
        /// <para>
        /// Defines a fence unsignal operation for each fence, which resets the fence to the
        /// unsignaled state.
        /// </para>
        /// <para>
        /// If any member of <paramref name="fences"/> is already in the unsignaled state, then the
        /// command has no effect on that fence.
        /// </para>
        /// </summary>
        /// <param name="fences">Fence handles to reset.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void ResetFences(params Fence[] fences)
        {
            Fence.Reset(this, fences);
        }

        /// <summary>
        /// Wait for one or more fences to become signaled.
        /// <para>
        /// If the condition is satisfied when the command is called, then the command returns
        /// immediately. If the condition is not satisfied at the time the command is called, then
        /// the command will block and wait up to timeout nanoseconds for the condition to become satisfied.
        /// </para>
        /// </summary>
        /// <param name="fences">Fence handle.</param>
        /// <param name="waitAll">
        /// The condition that must be satisfied to successfully unblock the wait. If <c>true</c> ,
        /// then the condition is that all fences in <paramref name="fences"/> are signaled.
        /// Otherwise, the condition is that at least one fence in <paramref name="fences"/> is signaled.
        /// </param>
        /// <param name="timeout">
        /// The timeout period in units of nanoseconds. Timeout is adjusted to the closest value
        /// allowed by the implementation-dependent timeout accuracy, which may be substantially
        /// longer than one nanosecond, and may be longer than the requested period.
        /// <para>
        /// If timeout is zero, then the command does not wait, but simply returns the current state
        /// of the fences. The result <see cref="Result.Timeout"/> will be thrown in this case if the
        /// condition is not satisfied, even though no actual wait was performed.
        /// </para>
        /// <para>
        /// If the specified timeout period expires before the condition is satisfied, the command
        /// throws with <see cref="Result.Timeout"/>. If the condition is satisfied before timeout
        /// nanoseconds has expired, the command returns successfully.
        /// </para>
        /// </param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void WaitFences(Fence[] fences, bool waitAll, long timeout = ~0)
        {
            Fence.Wait(this, fences, waitAll, timeout);
        }

        /// <summary>
        /// Create a new queue semaphore object.
        /// </summary>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>Resulting semaphore object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Semaphore CreateSemaphore(AllocationCallbacks? allocator = null)
        {
            return new Semaphore(this, ref allocator);
        }

        /// <summary>
        /// Create a new query pool object.
        /// </summary>
        /// <param name="createInfo">
        /// Structure containing the number and type of queries to be managed by the pool.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting query pool object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public QueryPool CreateQueryPool(QueryPoolCreateInfo createInfo, AllocationCallbacks? allocator = null)
        {
            return new QueryPool(this, &createInfo, ref allocator);
        }

        /// <summary>
        /// Destroy a logical device.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyDevice(this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateDeviceDelegate(IntPtr physicalDevice, DeviceCreateInfo.Native* createInfo, AllocationCallbacks.Native* allocator, IntPtr* device);
        private static readonly vkCreateDeviceDelegate vkCreateDevice = VulkanLibrary.GetStaticProc<vkCreateDeviceDelegate>(nameof(vkCreateDevice));

        private delegate void vkDestroyDeviceDelegate(IntPtr device, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyDeviceDelegate vkDestroyDevice = VulkanLibrary.GetStaticProc<vkDestroyDeviceDelegate>(nameof(vkDestroyDevice));

        private delegate IntPtr vkGetDeviceProcAddrDelegate(IntPtr device, byte* name);
        private static readonly vkGetDeviceProcAddrDelegate vkGetDeviceProcAddr = VulkanLibrary.GetStaticProc<vkGetDeviceProcAddrDelegate>(nameof(vkGetDeviceProcAddr));

        private delegate void vkGetDeviceQueueDelegate(IntPtr device, int queueFamilyIndex, int queueIndex, IntPtr* queue);
        private static readonly vkGetDeviceQueueDelegate vkGetDeviceQueue = VulkanLibrary.GetStaticProc<vkGetDeviceQueueDelegate>(nameof(vkGetDeviceQueue));

        private delegate Result vkDeviceWaitIdleDelegate(IntPtr device);
        private static readonly vkDeviceWaitIdleDelegate vkDeviceWaitIdle = VulkanLibrary.GetStaticProc<vkDeviceWaitIdleDelegate>(nameof(vkDeviceWaitIdle));

        private delegate Result vkFlushMappedMemoryRangesDelegate(IntPtr device, int memoryRangeCount, MappedMemoryRange* memoryRanges);
        private static readonly vkFlushMappedMemoryRangesDelegate vkFlushMappedMemoryRanges = VulkanLibrary.GetStaticProc<vkFlushMappedMemoryRangesDelegate>(nameof(vkFlushMappedMemoryRanges));

        private delegate Result vkInvalidateMappedMemoryRangesDelegate(IntPtr device, int memoryRangeCount, MappedMemoryRange* memoryRanges);
        private static readonly vkInvalidateMappedMemoryRangesDelegate vkInvalidateMappedMemoryRanges = VulkanLibrary.GetStaticProc<vkInvalidateMappedMemoryRangesDelegate>(nameof(vkInvalidateMappedMemoryRanges));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created device.
    /// </summary>
    public unsafe struct DeviceCreateInfo
    {
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Structures describing the queues that are requested to be created along with the logical device.
        /// </summary>
        public DeviceQueueCreateInfo[] QueueCreateInfos;
        /// <summary>
        /// Is <c>null</c> or unicode strings containing the names of extensions to enable for the
        /// created device.
        /// </summary>
        public string[] EnabledExtensionNames;
        /// <summary>
        /// Is <c>null</c> or a <see cref="PhysicalDeviceFeatures"/> structure that contains boolean
        /// indicators of all the features to be enabled.
        /// </summary>
        public PhysicalDeviceFeatures? EnabledFeatures;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceCreateInfo"/> structure.
        /// </summary>
        /// <param name="queueCreateInfos">
        /// Structures describing the queues that are requested to be created along with the logical device.
        /// </param>
        /// <param name="enabledExtensionNames">
        /// Is <c>null</c> or unicode strings containing the names of extensions to enable for the
        /// created device.
        /// </param>
        /// <param name="enabledFeatures">
        /// Is <c>null</c> or a <see cref="PhysicalDeviceFeatures"/> structure that contains boolean
        /// indicators of all the features to be enabled.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public DeviceCreateInfo(
            DeviceQueueCreateInfo[] queueCreateInfos,
            string[] enabledExtensionNames = null,
            PhysicalDeviceFeatures? enabledFeatures = null,
            IntPtr next = default(IntPtr))
        {
            Next = next;
            QueueCreateInfos = queueCreateInfos;
            EnabledExtensionNames = enabledExtensionNames;
            EnabledFeatures = enabledFeatures;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public DeviceCreateFlags Flags;
            public int QueueCreateInfoCount;
            public DeviceQueueCreateInfo.Native* QueueCreateInfos;
            public int EnabledLayerCount;
            public IntPtr* EnabledLayerNames;
            public int EnabledExtensionCount;
            public IntPtr* EnabledExtensionNames;
            public IntPtr EnabledFeatures;

            public void Free()
            {
                for (int i = 0; i < QueueCreateInfoCount; i++)
                    QueueCreateInfos[i].Free();
                Interop.Free(QueueCreateInfos);
                Interop.Free(EnabledExtensionNames, EnabledExtensionCount);
                Interop.Free(EnabledFeatures);
            }
        }

        internal void ToNative(out Native val)
        {
            int queueCreateInfoCount = QueueCreateInfos?.Length ?? 0;
            var ptr = (DeviceQueueCreateInfo.Native*)Interop.Alloc<DeviceQueueCreateInfo.Native>(queueCreateInfoCount);
            for (var i = 0; i < queueCreateInfoCount; i++)
                QueueCreateInfos[i].ToNative(out ptr[i]);

            val.Type = StructureType.DeviceCreateInfo;
            val.Next = Next;
            val.Flags = 0;
            val.QueueCreateInfoCount = queueCreateInfoCount;
            val.QueueCreateInfos = ptr;
            val.EnabledLayerCount = 0;
            val.EnabledLayerNames = null;
            val.EnabledExtensionCount = EnabledExtensionNames?.Length ?? 0;
            val.EnabledExtensionNames = Interop.String.AllocToPointers(EnabledExtensionNames);
            val.EnabledFeatures = Interop.Struct.AllocToPointer(ref EnabledFeatures);
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum DeviceCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying parameters of a newly created device queue.
    /// </summary>
    public struct DeviceQueueCreateInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceQueueCreateInfo"/> structure.
        /// </summary>
        /// <param name="queueFamilyIndex">
        /// An unsigned integer indicating the index of the queue family to create on this device.
        /// This index corresponds to the index of an element of the <see
        /// cref="QueueFamilyProperties"/> array that was returned by <see cref="PhysicalDevice.GetQueueFamilyProperties"/>.
        /// </param>
        /// <param name="queueCount">
        /// An unsigned integer specifying the number of queues to create in the queue family
        /// indicated by <paramref name="queueFamilyIndex"/>.
        /// </param>
        /// <param name="queuePriorities">
        /// Normalized floating point values, specifying priorities of work that will be submitted to
        /// each created queue.
        /// </param>
        public DeviceQueueCreateInfo(int queueFamilyIndex, int queueCount, params float[] queuePriorities)
        {
            QueueFamilyIndex = queueFamilyIndex;
            QueueCount = queueCount;
            QueuePriorities = queuePriorities;
        }

        /// <summary>
        /// An unsigned integer indicating the index of the queue family to create on this device.
        /// This index corresponds to the index of an element of the <see
        /// cref="QueueFamilyProperties"/> array that was returned by <see cref="PhysicalDevice.GetQueueFamilyProperties"/>.
        /// </summary>
        public int QueueFamilyIndex;
        /// <summary>
        /// An unsigned integer specifying the number of queues to create in the queue family
        /// indicated by <see cref="QueueFamilyIndex"/>.
        /// </summary>
        public int QueueCount;
        /// <summary>
        /// An array of normalized floating point values, specifying priorities of work that
        /// will be submitted to each created queue.
        /// </summary>
        public float[] QueuePriorities;

        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public DeviceQueueCreateFlags Flags;
            public int QueueFamilyIndex;
            public int QueueCount;
            public IntPtr QueuePriorities;

            public void Free()
            {
                Interop.Free(QueuePriorities);
            }
        }

        internal void ToNative(out Native native)
        {
            native.Type = StructureType.DeviceQueueCreateInfo;
            native.Next = IntPtr.Zero;
            native.Flags = 0;
            native.QueueFamilyIndex = QueueFamilyIndex;
            native.QueueCount = QueueCount;
            native.QueuePriorities = Interop.Struct.AllocToPointer(QueuePriorities);
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum DeviceQueueCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying a mapped memory range.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MappedMemoryRange
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// The memory object to which this range belongs.
        /// <para>Must currently be mapped.</para>
        /// </summary>
        public long Memory;
        /// <summary>
        /// The zero-based byte offset from the beginning of the memory object.
        /// <para>Must be a multiple of <see cref="PhysicalDeviceLimits.NonCoherentAtomSize"/>.</para>
        /// <para>
        /// If <see cref="Size"/> is equal to <see cref="WholeSize"/>, offset must be within the
        /// currently mapped range of memory
        /// </para>
        /// </summary>
        public long Offset;
        /// <summary>
        /// Is either the size of range, or <see cref="WholeSize"/> to affect the range from offset
        /// to the end of the current mapping of the allocation.
        /// <para>
        /// If size is not equal to <see cref="WholeSize"/>, offset and size must specify a range
        /// contained within the currently mapped range of memory.
        /// </para>
        /// <para>
        /// If size is not equal to <see cref="WholeSize"/>, size must be a multiple of <see cref="PhysicalDeviceLimits.NonCoherentAtomSize"/>
        /// </para>
        /// </summary>
        public long Size;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedMemoryRange"/> structure.
        /// </summary>
        /// <param name="memory">The memory object to which this range belongs.</param>
        /// <param name="offset">The zero-based byte offset from the beginning of the memory object.</param>
        /// <param name="size">
        /// Is either the size of range, or <see cref="WholeSize"/> to affect the range from offset
        /// to the end of the current mapping of the allocation.
        /// </param>
        public MappedMemoryRange(DeviceMemory memory, long offset = 0, long size = WholeSize)
        {
            Type = StructureType.MappedMemoryRange;
            Next = IntPtr.Zero;
            Memory = memory;
            Offset = offset;
            Size = size;
        }

        internal void Prepare()
        {
            Type = StructureType.MappedMemoryRange;
        }
    }
}
