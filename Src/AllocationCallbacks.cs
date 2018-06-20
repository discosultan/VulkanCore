using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Structure containing callback functions for memory allocation.
    /// </summary>
    public unsafe struct AllocationCallbacks
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationCallbacks"/> structure.
        /// </summary>
        /// <param name="alloc">The application-defined memory allocation function.</param>
        /// <param name="realloc">The application-defined memory reallocation function.</param>
        /// <param name="free">The application-defined memory free function</param>
        /// <param name="internalAlloc">
        /// The application-defined function that is called by the implementation when the
        /// implementation makes internal allocations.
        /// </param>
        /// <param name="internalFree">
        /// The application-defined function that is called by the implementation when the
        /// implementation frees internal allocations.
        /// </param>
        /// <param name="userData">
        /// The value to be interpreted by the implementation of the callbacks.
        /// <para>
        /// When any of the callbacks in <see cref="AllocationCallbacks"/> are called, the Vulkan
        /// implementation will pass this value as the first parameter to the callback.
        /// </para>
        /// <para>
        /// This value can vary each time an allocator is passed into a command, even when the same
        /// object takes an allocator in multiple commands.
        /// </para>
        /// </param>
        public AllocationCallbacks(AllocationFunction alloc, ReallocationFunction realloc, FreeFunction free,
            InternalAllocationNotification internalAlloc = null, InternalFreeNotification internalFree = null,
            IntPtr userData = default(IntPtr))
        {
            Allocation = alloc;
            Reallocation = realloc;
            Free = free;
            InternalAllocation = internalAlloc;
            InternalFree = internalFree;
            UserData = userData;
        }

        /// <summary>
        /// The value to be interpreted by the implementation of the callbacks.
        /// <para>
        /// When any of the callbacks in <see cref="AllocationCallbacks"/> are called, the Vulkan
        /// implementation will pass this value as the first parameter to the callback.
        /// </para>
        /// <para>
        /// This value can vary each time an allocator is passed into a command, even when the same
        /// object takes an allocator in multiple commands.
        /// </para>
        /// </summary>
        public IntPtr UserData;
        /// <summary>
        /// The application-defined memory allocation function.
        /// </summary>
        public AllocationFunction Allocation;
        /// <summary>
        /// Gets the application-defined memory reallocation function.
        /// </summary>
        public ReallocationFunction Reallocation;
        /// <summary>
        /// Gets the application-defined memory free function.
        /// </summary>
        public FreeFunction Free;
        /// <summary>
        /// The application-defined function that is called by the implementation when the
        /// implementation makes internal allocations. This value may be <c>null</c>.
        /// </summary>
        public InternalAllocationNotification InternalAllocation;
        /// <summary>
        /// The application-defined function that is called by the implementation when
        /// the implementation frees internal allocations. This value may be <c>null</c>.
        /// </summary>
        public InternalFreeNotification InternalFree;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public IntPtr UserData;
            public IntPtr Allocation;
            public IntPtr Reallocation;
            public IntPtr Free;
            public IntPtr InternalAllocation;
            public IntPtr InternalFree;
        }

        internal void ToNative(Native* native)
        {
            native->UserData = UserData;
            native->Allocation = Interop.GetFunctionPointerForDelegate(Allocation);
            native->Reallocation = Interop.GetFunctionPointerForDelegate(Reallocation);
            native->Free = Interop.GetFunctionPointerForDelegate(Free);
            native->InternalAllocation = InternalAllocation == null
                ? IntPtr.Zero
                : Interop.GetFunctionPointerForDelegate(InternalAllocation);
            native->InternalFree = InternalFree == null
                ? IntPtr.Zero
                : Interop.GetFunctionPointerForDelegate(InternalFree);
        }

        /// <summary>
        /// Application-defined memory allocation function.
        /// <para>
        /// If this function is unable to allocate the requested memory, it must return <see
        /// cref="IntPtr.Zero"/>. If the allocation was successful, it must return a valid handle to
        /// memory allocation containing at least <paramref name="size"/> bytes, and with the pointer
        /// value being a multiple of <paramref name="alignment"/>.
        /// </para>
        /// <para>
        /// For example, this function (or <see cref="ReallocationFunction"/>) could cause
        /// termination of running Vulkan instance(s) on a failed allocation for debugging purposes,
        /// either directly or indirectly. In these circumstances, it cannot be assumed that any part
        /// of any affected <see cref="Instance"/> objects are going to operate correctly (even <see
        /// cref="Instance.Dispose"/>), and the application must ensure it cleans up properly via
        /// other means (e.g. process termination).
        /// </para>
        /// <para>
        /// If this function returns <see cref="IntPtr.Zero"/>, and if the implementation is unable
        /// to continue correct processing of the current command without the requested allocation,
        /// it must treat this as a run-time error, and generate <see
        /// cref="Result.ErrorOutOfHostMemory"/> at the appropriate time for the command in which the
        /// condition was detected.
        /// </para>
        /// <para>
        /// If the implementation is able to continue correct processing of the current command
        /// without the requested allocation, then it may do so, and must not generate <see
        /// cref="Result.ErrorOutOfHostMemory"/> as a result of this failed allocation.
        /// </para>
        /// </summary>
        /// <param name="userData">
        /// Value specified for <see cref="UserData"/> in the allocator specified by the application.
        /// </param>
        /// <param name="size">Size in bytes of the requested allocation.</param>
        /// <param name="alignment">
        /// Requested alignment of the allocation in bytes and must be a power of two.
        /// </param>
        /// <param name="allocationScope">
        /// Value specifying the allocation scope of the lifetime of the allocation.
        /// </param>
        public delegate IntPtr AllocationFunction(
            IntPtr userData, Size size, Size alignment, SystemAllocationScope allocationScope);

        /// <summary>
        /// Application-defined memory reallocation function.
        /// <para>
        /// Must return an allocation with enough space for <paramref name="size"/> bytes, and the
        /// contents of the original allocation from bytes zero to `min(original size, new size) - 1`
        /// must be preserved in the returned allocation. If <paramref name="size"/> is larger than
        /// the old size, the contents of the additional space are undefined. If satisfying these
        /// requirements involves creating a new allocation, then the old allocation should be freed.
        /// </para>
        /// <para>
        /// If <paramref name="original"/> is <see cref="IntPtr.Zero"/>, then the function must
        /// behave equivalently to a call to <see cref="AllocationFunction"/> with the same parameter
        /// values (without <paramref name="original"/>).
        /// </para>
        /// <para>
        /// If <paramref name="size"/> is zero, then the function must behave equivalently to a call
        /// to <see cref="FreeFunction"/> with the same <paramref name="userData"/> parameter value,
        /// and 'memory' equal to <paramref name="original"/>.
        /// </para>
        /// <para>
        /// If <paramref name="original"/> is not <see cref="IntPtr.Zero"/>, the implementation must
        /// ensure that <paramref name="alignment"/> is equal to the <paramref name="alignment"/>
        /// used to originally allocate <paramref name="original"/>.
        /// </para>
        /// <para>
        /// If this function fails and <paramref name="original"/> is not <see cref="IntPtr.Zero"/>
        /// the application must not free the old allocation.
        /// </para>
        /// <para>This function must follow the same rules for return values as <see cref="AllocationFunction"/>.</para>
        /// </summary>
        /// <param name="userData">
        /// Value specified for <see cref="UserData"/> in the allocator specified by the application.
        /// </param>
        /// <param name="original">
        /// Must be either <see cref="IntPtr.Zero"/> or a pointer previously returned by <see
        /// cref="Reallocation"/> or <see cref="Allocation"/> of the same allocator.
        /// </param>
        /// <param name="size">Size in bytes of the requested allocation.</param>
        /// <param name="alignment">
        /// Requested alignment of the allocation in bytes and must be a power of two.
        /// </param>
        /// <param name="allocationScope">
        /// Value specifying the allocation scope of the lifetime of the allocation.
        /// </param>
        public delegate IntPtr ReallocationFunction(
            IntPtr userData, IntPtr original, Size size, Size alignment, SystemAllocationScope allocationScope);

        /// <summary>
        /// Application-defined memory free function.
        /// <para>
        /// <paramref name="memory"/> may be <see cref="IntPtr.Zero"/>, which the callback must
        /// handle safely. If <paramref name="memory"/> is not <see cref="IntPtr.Zero"/>, it must be
        /// a handle to previously allocated by <see cref="AllocationFunction"/> or <see
        /// cref="ReallocationFunction"/>. The application should free this memory.
        /// </para>
        /// </summary>
        /// <param name="userData">
        /// Value specified for <see cref="UserData"/> in the allocator specified
        /// by the application.
        /// </param>
        /// <param name="memory">Allocation to be freed.</param>
        public delegate void FreeFunction(IntPtr userData, IntPtr memory);

        /// <summary>
        /// Application-defined memory allocation notification function.
        /// <para>This is a purely informational callback.</para>
        /// </summary>
        /// <param name="userData">
        /// Value specified for <see cref="UserData"/> in the allocator specified by the application.
        /// </param>
        /// <param name="size">Size in bytes of the requested allocation.</param>
        /// <param name="allocationType">Requested type of an allocation.</param>
        /// <param name="allocationScope">
        /// Value specifying the allocation scope of the lifetime of the allocation.
        /// </param>
        public delegate void InternalAllocationNotification(
            IntPtr userData, Size size, InternalAllocationType allocationType, SystemAllocationScope allocationScope);

        /// <summary>
        /// Application-defined memory free notification function.
        /// </summary>
        /// <param name="userData">
        /// Value specified for <see cref="UserData"/> in the allocator specified by the application.
        /// </param>
        /// <param name="size">Size in bytes of the requested allocation.</param>
        /// <param name="allocationType">Requested type of an allocation.</param>
        /// <param name="allocationScope">
        /// Value specifying the allocation scope of the lifetime of the allocation.
        /// </param>
        public delegate void InternalFreeNotification(
            IntPtr userData, Size size, InternalAllocationType allocationType, SystemAllocationScope allocationScope);
    }

    /// <summary>
    /// Allocation scope.
    /// </summary>
    public enum SystemAllocationScope
    {
        /// <summary>
        /// Specifies that the allocation is scoped to the duration of the Vulkan command.
        /// </summary>
        Command = 0,
        /// <summary>
        /// Specifies that the allocation is scoped to the lifetime of the Vulkan object that is
        /// being created or used.
        /// </summary>
        Object = 1,
        /// <summary>
        /// Specifies that the allocation is scoped to the lifetime of a <see cref="PipelineCache"/>
        /// or <see cref="Ext.ValidationCacheExt"/> object.
        /// </summary>
        Cache = 2,
        /// <summary>
        /// Specifies that the allocation is scoped to the lifetime of the Vulkan device.
        /// </summary>
        Device = 3,
        /// <summary>
        /// Specifies that the allocation is scoped to the lifetime of the Vulkan instance.
        /// </summary>
        Instance = 4
    }

    /// <summary>
    /// Allocation type.
    /// </summary>
    public enum InternalAllocationType
    {
        /// <summary>
        /// Specifies that the allocation is intended for execution by the host.
        /// </summary>
        Executable = 0
    }
}
