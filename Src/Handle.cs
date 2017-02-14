using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance or a null reference.</param>
        /// <returns>
        /// <c>true</c> if obj is an instance of <see cref="VulkanHandle{THandle}"/> and its handle
        /// equals the handle of this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Handle.Equals(((VulkanHandle<THandle>)obj).Handle);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => Handle.GetHashCode();

        /// <summary>
        /// Returns a boolean indicating whether the two given handles are equal.
        /// </summary>
        /// <param name="left">The first handle to compare.</param>
        /// <param name="right">The second handle to compare.</param>
        /// <returns><c>true</c> if the handles are equal; otherwise <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(VulkanHandle<THandle> left, VulkanHandle<THandle> right)
            => left?.Equals(right) ?? false;

        /// <summary>
        /// Returns a boolean indicating whether the two given handles are not equal.
        /// </summary>
        /// <param name="left">The first handle to compare.</param>
        /// <param name="right">The second handle to compare.</param>
        /// <returns><c>true</c> if the handles are not equal; otherwise <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(VulkanHandle<THandle> left, VulkanHandle<THandle> right)
            => !(left == right);

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
        private bool _disposed;
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

    internal static class VulkanHandleExtensions
    {
        // We need to duplicate these extensions instead of using a generic one
        // due to compiler's inability to implicitly infer generic type params in this case.

        public static IntPtr[] ToHandleArray(this IEnumerable<VulkanHandle<IntPtr>> values)
            => values.Select(val => val.Handle).ToArray();

        public static long[] ToHandleArray(this IEnumerable<VulkanHandle<long>> values)
            => values.Select(val => val.Handle).ToArray();
    }
}
