using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a event object.
    /// <para>
    /// Events are a synchronization primitive that can be used to insert a fine-grained dependency
    /// between commands submitted to the same queue, or between the host and a queue. Events have
    /// two states - signaled and unsignaled. An application can signal an event, or unsignal it, on
    /// either the host or the device. A device can wait for an event to become signaled before
    /// executing further operations. No command exists to wait for an event to become signaled on
    /// the host, but the current state of an event can be queried.
    /// </para>
    /// </summary>
    public unsafe class Event : DisposableHandle<long>
    {
        internal Event(Device parent, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            var createInfo = new EventCreateInfo();
            createInfo.Prepare();

            long handle;
            Result result = vkCreateEvent(Parent, &createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of this resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Retrieve the status of an event object. Upon success, the command returns the state of
        /// the event object with the following return codes:
        /// <para>* <see cref="Result.EventSet"/> - The event is signaled</para>
        /// <para>* <see cref="Result.EventReset"/> - The event is unsignaled</para>
        /// </summary>
        /// <returns><see cref="Result.EventSet"/> if the event is signaled; otherwise <see cref="Result.EventReset"/>.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Result GetStatus()
        {
            Result result = vkGetEventStatus(Parent, this);
            if (result != Result.EventSet && result != Result.EventReset)
                VulkanException.ThrowForInvalidResult(result);
            return result;
        }

        /// <summary>
        /// Set an event to signaled state.
        /// </summary>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void Set()
        {
            Result result = vkSetEvent(Parent, this);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Reset an event to non-signaled state.
        /// </summary>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void Reset()
        {
            Result result = vkResetEvent(Parent, this);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Destroy an event object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyEvent(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateEventDelegate(IntPtr device, EventCreateInfo* createInfo, AllocationCallbacks.Native* allocator, long* @event);
        private static readonly vkCreateEventDelegate vkCreateEvent = VulkanLibrary.GetStaticProc<vkCreateEventDelegate>(nameof(vkCreateEvent));

        private delegate void vkDestroyEventDelegate(IntPtr device, long @event, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyEventDelegate vkDestroyEvent = VulkanLibrary.GetStaticProc<vkDestroyEventDelegate>(nameof(vkDestroyEvent));

        private delegate Result vkGetEventStatusDelegate(IntPtr device, long @event);
        private static readonly vkGetEventStatusDelegate vkGetEventStatus = VulkanLibrary.GetStaticProc<vkGetEventStatusDelegate>(nameof(vkGetEventStatus));

        private delegate Result vkSetEventDelegate(IntPtr device, long @event);
        private static readonly vkSetEventDelegate vkSetEvent = VulkanLibrary.GetStaticProc<vkSetEventDelegate>(nameof(vkSetEvent));

        private delegate Result vkResetEventDelegate(IntPtr device, long @event);
        private static readonly vkResetEventDelegate vkResetEvent = VulkanLibrary.GetStaticProc<vkResetEventDelegate>(nameof(vkResetEvent));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created event.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct EventCreateInfo
    {
        public StructureType Type;
        public IntPtr Next;
        public EventCreateFlags Flags;

        public void Prepare()
        {
            Type = StructureType.EventCreateInfo;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum EventCreateFlags
    {
        None = 0
    }
}
