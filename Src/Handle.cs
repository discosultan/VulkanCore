using System;
using System.Collections.Generic;
using System.Linq;

namespace VulkanCore
{
    /// <summary>
    /// Handle to an unmanaged Vulkan resource.
    /// </summary>
    /// <typeparam name="THandle">Handle type.</typeparam>
    public abstract class VulkanHandle<THandle> where THandle : struct
    {
        /// <summary>
        /// Gets the handle to the unmanaged Vulkan resource.
        /// </summary>
        public THandle Handle { get; protected set; }

        /// <summary>
        /// Implicitly converts an instance of <see cref="VulkanHandle{THandle}"/> to its handle type.
        /// </summary>
        /// <param name="value">Instance to convert.</param>
        public static implicit operator THandle(VulkanHandle<THandle> value) => value?.Handle ?? default(THandle);
    }

    /// <summary>
    /// <see cref="VulkanHandle{THandle}"/> that implements dispose pattern as described in
    /// https://msdn.microsoft.com/en-us/library/b1yfkh5e%28v=vs.110%29.aspx and stores an allocator.
    /// </summary>
    /// <typeparam name="THandle">Handle type.</typeparam>
    public abstract unsafe class DisposableHandle<THandle> : VulkanHandle<THandle>, IDisposable
        where THandle : struct
    {
        private AllocationCallbacks? _allocator;

        /// <summary>
        /// Allows the object to free unmanaged resources before it is reclaimed by garbage collection.
        /// </summary>
        ~DisposableHandle()
        {
            DisposeUnmanaged();
        }

        /// <summary>
        /// Gets the memory allocator for the resource. This may be <c>null</c>.
        /// </summary>
        public AllocationCallbacks? Allocator
        {
            get { return _allocator; }
            protected set
            {
                _allocator = value;
                Interop.Free(NativeAllocator);
                NativeAllocator = null;
                if (_allocator.HasValue)
                {
                    NativeAllocator = (AllocationCallbacks.Native*)Interop.Alloc<AllocationCallbacks.Native>();
                    _allocator.Value.ToNative(NativeAllocator);
                }
            }
        }

        internal AllocationCallbacks.Native* NativeAllocator { get; private set; }

        /// <summary>
        /// Gets if the instance is disposed.
        /// </summary>
        protected bool Disposed { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (!Disposed)
            {
                DisposeUnmanaged();
                GC.SuppressFinalize(this);
                Disposed = true;
            }
        }

        private void DisposeUnmanaged()
        {
            Interop.Free(NativeAllocator);
        }
    }

    /// <summary>
    /// Provides extensions methods for the <see cref="VulkanHandle{THandle}"/> class.
    /// </summary>
    public static class VulkanHandleExtensions
    {
        // We need to duplicate these extensions instead of using a generic one
        // due to compiler's inability to implicitly infer generic type params in this case.

        /// <summary>
        /// Creates an array of pointers from the sequence of <see cref="VulkanHandle{THandle}"/> types.
        /// </summary>
        /// <param name="values">A sequence to create an array from.</param>
        /// <returns>An array that contains the pointers to the input handles.</returns>
        public static IntPtr[] ToHandleArray(this IEnumerable<VulkanHandle<IntPtr>> values)
            => values.Select(val => val.Handle).ToArray();

        /// <summary>
        /// Creates an array of pointers from the sequence of <see cref="VulkanHandle{THandle}"/> types.
        /// </summary>
        /// <param name="values">A sequence to create an array from.</param>
        /// <returns>An array that contains the pointers to the input handles.</returns>
        public static long[] ToHandleArray(this IEnumerable<VulkanHandle<long>> values)
            => values.Select(val => val.Handle).ToArray();
    }
}
