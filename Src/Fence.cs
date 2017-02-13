using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a fence object.
    /// <para>
    /// Fences are a synchronization primitive that can be used to insert a dependency from a queue
    /// to the host. Fences have two states - signaled and unsignaled. A fence can be signaled as
    /// part of the execution of a queue submission command. Fences can be unsignaled on the host
    /// with <see cref="Reset"/>. Fences can be waited on by the host with the <see cref="Wait"/>
    /// command, and the current state can be queried with <see cref="GetStatus"/>.
    /// </para>
    /// </summary>
    public unsafe class Fence : DisposableHandle<long>
    {
        internal Fence(Device parent, FenceCreateInfo* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            createInfo->Prepare();
            long handle;
            Result result = CreateFence(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        internal Fence(Device parent, ref AllocationCallbacks? allocator, long handle)
        {
            Parent = parent;
            Allocator = allocator;
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of this resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Return the status of a fence. Upon success, returns the status of the fence object, with
        /// the following return codes:
        /// <para>* <see cref="Result.Success"/> - The fence is signaled</para>
        /// <para>* <see cref="Result.NotReady"/> - The fence is unsignaled</para>
        /// </summary>
        /// <returns><see cref="Result.Success"/> if the fence is signaled; otherwise <see cref="Result.NotReady"/>.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Result GetStatus()
        {
            Result result = GetFenceStatus(Parent, this);
            if (result != Result.Success && result != Result.NotReady)
                VulkanException.ThrowForInvalidResult(result);
            return result;
        }

        /// <summary>
        /// Resets the fence object.
        /// <para>Defines a fence unsignal operation, which resets the fence to the unsignaled state.</para>
        /// <para>
        /// If fence is already in the unsignaled state, then the command has no effect on that fence.
        /// </para>
        /// </summary>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void Reset()
        {
            long handle = this;
            Result result = ResetFences(Parent, 1, &handle);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Wait for the fence to become signaled.
        /// <para>
        /// If the condition is satisfied when the command is called, then the command returns
        /// immediately. If the condition is not satisfied at the time the command is called, then
        /// the command will block and wait up to timeout nanoseconds for the condition to become satisfied.
        /// </para>
        /// </summary>
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
        public void Wait(long timeout = ~0)
        {
            long handle = this;
            Result result = WaitForFences(Parent, 1, &handle, false, timeout);
            VulkanException.ThrowForInvalidResult(result);
        }

        protected override void DisposeManaged()
        {
            DestroyFence(Parent, this, NativeAllocator);
            base.DisposeManaged();
        }
        
        internal static void Reset(Device parent, Fence[] fences)
        {
            if (fences != null && fences.Length > 0)
            {
                long* fenceHandles = stackalloc long[fences.Length];
                for (int i = 0; i < fences.Length; i++)
                    fenceHandles[i] = fences[i];
                Result result = ResetFences(parent, fences.Length, fenceHandles);
                VulkanException.ThrowForInvalidResult(result);
            }
        }

        internal static void Wait(Device parent, Fence[] fences, bool waitAll, long timeout)
        {
            if (fences != null && fences.Length > 0)
            {
                long* fenceHandles = stackalloc long[fences.Length];
                for (int i = 0; i < fences.Length; i++)
                    fenceHandles[i] = fences[i];
                Result result = WaitForFences(parent, fences.Length, fenceHandles, waitAll, timeout);
                VulkanException.ThrowForInvalidResult(result);
            }
        }

        [DllImport(VulkanDll, EntryPoint = "vkCreateFence", CallingConvention = CallConv)]
        private static extern Result CreateFence(IntPtr device, FenceCreateInfo* createInfo, AllocationCallbacks.Native* allocator, long* fence);

        [DllImport(VulkanDll, EntryPoint = "vkDestroyFence", CallingConvention = CallConv)]
        private static extern void DestroyFence(IntPtr device, long fence, AllocationCallbacks.Native* allocator);

        [DllImport(VulkanDll, EntryPoint = "vkResetFences", CallingConvention = CallConv)]
        private static extern Result ResetFences(IntPtr device, int fenceCount, long* fences);

        [DllImport(VulkanDll, EntryPoint = "vkGetFenceStatus", CallingConvention = CallConv)]
        private static extern Result GetFenceStatus(IntPtr device, long fence);

        [DllImport(VulkanDll, EntryPoint = "vkWaitForFences", CallingConvention = CallConv)]
        private static extern Result WaitForFences(IntPtr device, int fenceCount, long* fences, Bool waitAll, long timeout);
    }

    /// <summary>
    /// Structure specifying parameters of a newly created fence.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FenceCreateInfo
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// Defines the initial state and behavior of the fence.
        /// <para>
        /// If flags contains <see cref="FenceCreateFlags.Signaled"/> then the fence object is
        /// created in the signaled state; otherwise it is created in the unsignaled state.
        /// </para>
        /// </summary>
        public FenceCreateFlags Flags;

        /// <summary>
        /// Initializes a new instance of the <see cref="FenceCreateInfo"/> structure.
        /// </summary>
        /// <param name="flags">Defines the initial state and behavior of the fence.</param>
        public FenceCreateInfo(FenceCreateFlags flags = 0)
        {
            Type = StructureType.FenceCreateInfo;
            Next = IntPtr.Zero;
            Flags = flags;
        }

        internal void Prepare()
        {
            Type = StructureType.FenceCreateInfo;
        }
    }

    /// <summary>
    /// Bitmask specifying initial state and behavior of a fence.
    /// </summary>
    [Flags]
    public enum FenceCreateFlags
    {
        None = 0,
        Signaled = 1 << 0
    }
}
