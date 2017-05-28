using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a semaphore object.
    /// <para>
    /// Semaphores are a synchronization primitive that can be used to insert a dependency between
    /// batches submitted to queues. Semaphores have two states - signaled and unsignaled. The state
    /// of a semaphore can be signaled after execution of a batch of commands is completed. A batch
    /// can wait for a semaphore to become signaled before it begins execution, and the semaphore is
    /// also unsignaled before the batch begins execution.
    /// </para>
    /// </summary>
    public unsafe class Semaphore : DisposableHandle<long>
    {
        internal Semaphore(Device parent, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            var createInfo = new SemaphoreCreateInfo();
            createInfo.Prepare();

            long handle;
            Result result = vkCreateSemaphore(Parent, &createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        internal Semaphore(long handle, Device parent)
        {
            Handle = handle;
            Parent = parent;
        }

        /// <summary>
        /// Gets the parent of this resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Destroy a semaphore object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroySemaphore(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateSemaphoreDelegate(IntPtr device, SemaphoreCreateInfo* createInfo, AllocationCallbacks.Native* allocator, long* semaphore);
        private static readonly vkCreateSemaphoreDelegate vkCreateSemaphore = VulkanLibrary.GetStaticProc<vkCreateSemaphoreDelegate>(nameof(vkCreateSemaphore));

        private delegate void vkDestroySemaphoreDelegate(IntPtr device, long semaphore, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroySemaphoreDelegate vkDestroySemaphore = VulkanLibrary.GetStaticProc<vkDestroySemaphoreDelegate>(nameof(vkDestroySemaphore));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created semaphore.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SemaphoreCreateInfo
    {
        public StructureType Type;
        public IntPtr Next;
        public SemaphoreCreateFlags Flags;

        public void Prepare()
        {
            Type = StructureType.SemaphoreCreateInfo;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum SemaphoreCreateFlags
    {
        None = 0
    }
}
