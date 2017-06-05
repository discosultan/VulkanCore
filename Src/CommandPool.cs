using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a command pool object.
    /// <para>
    /// Command pools are opaque objects that command buffer memory is allocated from, and which
    /// allow the implementation to amortize the cost of resource creation across multiple command
    /// buffers. Command pools are application-synchronized, meaning that a command pool must not be
    /// used concurrently in multiple threads. That includes use via recording commands on any
    /// command buffers allocated from the pool, as well as operations that allocate, free, and reset
    /// command buffers or the pool itself.
    /// </para>
    /// </summary>
    public unsafe class CommandPool : DisposableHandle<long>
    {
        internal CommandPool(Device parent, CommandPoolCreateInfo* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            createInfo->Prepare();
            long handle;
            Result result = vkCreateCommandPool(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Reset a command pool.
        /// </summary>
        /// <param name="flags">
        /// Contains additional flags controlling the behavior of the reset. If flags includes <see
        /// cref="CommandPoolResetFlags.ReleaseResources"/>, resetting a command pool recycles all of
        /// the resources from the command pool back to the system.
        /// </param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void Reset(CommandPoolResetFlags flags = 0)
        {
            Result result = vkResetCommandPool(Parent, this, flags);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Allocate command buffers from an existing command pool.
        /// </summary>
        /// <param name="allocateInfo">The structure describing parameters of the allocation.</param>
        /// <returns>
        /// The resulting command buffer objects returned. Each allocated command buffer begins in
        /// the initial state.
        /// </returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public CommandBuffer[] AllocateBuffers(CommandBufferAllocateInfo allocateInfo)
        {
            return CommandBuffer.Allocate(this, &allocateInfo);
        }

        /// <summary>
        /// Free command buffers.
        /// </summary>
        /// <param name="commandBuffers">Command buffers to free.</param>
        public void FreeBuffers(params CommandBuffer[] commandBuffers)
        {
            CommandBuffer.Free(this, commandBuffers);
        }

        /// <summary>
        /// Destroy a command pool object.
        /// <para>
        /// When a pool is destroyed, all command buffers allocated from the pool are implicitly
        /// freed and become invalid. Command buffers allocated from a given pool do not need to be
        /// freed before destroying that command pool.
        /// </para>
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyCommandPool(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateCommandPoolDelegate(IntPtr device, CommandPoolCreateInfo* createInfo, AllocationCallbacks.Native* allocator, long* commandPool);
        private static readonly vkCreateCommandPoolDelegate vkCreateCommandPool = VulkanLibrary.GetStaticProc<vkCreateCommandPoolDelegate>(nameof(vkCreateCommandPool));

        private delegate void vkDestroyCommandPoolDelegate(IntPtr device, long commandPool, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyCommandPoolDelegate vkDestroyCommandPool = VulkanLibrary.GetStaticProc<vkDestroyCommandPoolDelegate>(nameof(vkDestroyCommandPool));

        private delegate Result vkResetCommandPoolDelegate(IntPtr device, long commandPool, CommandPoolResetFlags flags);
        private static readonly vkResetCommandPoolDelegate vkResetCommandPool = VulkanLibrary.GetStaticProc<vkResetCommandPoolDelegate>(nameof(vkResetCommandPool));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created command pool.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CommandPoolCreateInfo
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// A bitmask indicating usage behavior for the pool and command buffers allocated from it.
        /// </summary>
        public CommandPoolCreateFlags Flags;
        /// <summary>
        /// Designates a queue family.
        /// <para>
        /// All command buffers allocated from this command pool must be submitted on queues from the
        /// same queue family.
        /// </para>
        /// </summary>
        public int QueueFamilyIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandPoolCreateInfo"/> structure.
        /// </summary>
        /// <param name="queueFamilyIndex">
        /// Designates a queue family.
        /// <para>
        /// All command buffers allocated from this command pool must be submitted on queues from the
        /// same queue family.
        /// </para>
        /// </param>
        /// <param name="flags">
        /// A bitmask indicating usage behavior for the pool and command buffers allocated from it.
        /// </param>
        public CommandPoolCreateInfo(int queueFamilyIndex, CommandPoolCreateFlags flags = 0)
        {
            Type = StructureType.CommandPoolCreateInfo;
            Next = IntPtr.Zero;
            Flags = flags;
            QueueFamilyIndex = queueFamilyIndex;
        }

        internal void Prepare()
        {
            Type = StructureType.CommandPoolCreateInfo;
        }
    }

    /// <summary>
    /// Bitmask specifying usage behavior for a command pool.
    /// </summary>
    [Flags]
    public enum CommandPoolCreateFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates that command buffers allocated from the pool will be short-lived,
        /// meaning that they will be reset or freed in a relatively short timeframe.
        /// <para>This
        /// flag may be used by the implementation to control memory allocation behavior
        /// within the pool.</para>
        /// </summary>
        Transient = 1 << 0,
        /// <summary>
        /// Allows any command buffer allocated from a pool to be individually reset to the initial
        /// state either by calling <see cref="CommandBuffer.Reset"/>, or via the implicit reset when
        /// calling <see cref="CommandBuffer.Begin"/>.
        /// <para>
        /// If this flag is not set on a pool, then <see cref="CommandBuffer.Reset"/> must not be
        /// called for any command buffer allocated from that pool.
        /// </para>
        /// </summary>
        ResetCommandBuffer = 1 << 1
    }

    /// <summary>
    /// Bitmask controlling behavior of a command pool reset.
    /// </summary>
    [Flags]
    public enum CommandPoolResetFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that resetting a command pool recycles all of the resources from the command
        /// pool back to the system.
        /// </summary>
        ReleaseResources = 1 << 0
    }
}
