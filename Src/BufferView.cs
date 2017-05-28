using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a buffer view object.
    /// <para>
    /// A buffer view represents a contiguous range of a buffer and a specific format to be used to
    /// interpret the data. Buffer views are used to enable shaders to access buffer contents
    /// interpreted as formatted data.
    /// </para>
    /// </summary>
    public unsafe class BufferView : DisposableHandle<long>
    {
        internal BufferView(Device parent, Buffer buffer, BufferViewCreateInfo* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            createInfo->Prepare(buffer);

            long handle;
            Result result = vkCreateBufferView(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Destroy a buffer view object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyBufferView(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateBufferViewDelegate(IntPtr device, BufferViewCreateInfo* createInfo, AllocationCallbacks.Native* allocator, long* view);
        private static readonly vkCreateBufferViewDelegate vkCreateBufferView = VulkanLibrary.GetStaticProc<vkCreateBufferViewDelegate>(nameof(vkCreateBufferView));

        private delegate void vkDestroyBufferViewDelegate(IntPtr device, long bufferView, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyBufferViewDelegate vkDestroyBufferView = VulkanLibrary.GetStaticProc<vkDestroyBufferViewDelegate>(nameof(vkDestroyBufferView));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created buffer view.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BufferViewCreateInfo
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal BufferViewCreateFlags Flags;
        internal long Buffer;

        /// <summary>
        /// The format of the data elements in the buffer.
        /// </summary>
        public Format Format;
        /// <summary>
        /// An offset in bytes from the base address of the buffer. Accesses to the buffer view from
        /// shaders use addressing that is relative to this starting offset.
        /// <para>Must be less than the size of buffer.</para>
        /// <para>Must be a multiple of <see cref="PhysicalDeviceLimits.MinTexelBufferOffsetAlignment"/>.</para>
        /// </summary>
        public long Offset;
        /// <summary>
        /// A size in bytes of the buffer view. If range is equal to <see
        /// cref="WholeSize"/>, the range from offset to the end of the buffer is used. If
        /// <see cref="WholeSize"/> is used and the remaining size of the buffer is not a
        /// multiple of the element size of format, then the nearest smaller multiple is used.
        /// </summary>
        public long Range;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferViewCreateInfo"/> structure.
        /// </summary>
        /// <param name="format">The format of the data elements in the buffer.</param>
        /// <param name="offset">
        /// An offset in bytes from the base address of the buffer. Accesses to the buffer view from
        /// shaders use addressing that is relative to this starting offset.
        /// <para>Must be less than the size of buffer.</para>
        /// <para>Must be a multiple of <see cref="PhysicalDeviceLimits.MinTexelBufferOffsetAlignment"/>.</para>
        /// </param>
        /// <param name="range">
        /// A size in bytes of the buffer view. If range is equal to <see
        /// cref="Constant.WholeSize"/>, the range from offset to the end of the buffer is used. If
        /// <see cref="Constant.WholeSize"/> is used and the remaining size of the buffer is not a
        /// multiple of the element size of format, then the nearest smaller multiple is used.
        /// </param>
        public BufferViewCreateInfo(Format format, long offset = 0, long range = WholeSize)
        {
            Type = StructureType.BufferViewCreateInfo;
            Next = IntPtr.Zero;
            Flags = 0;
            Buffer = 0;
            Format = format;
            Offset = offset;
            Range = range;
        }

        internal void Prepare(Buffer buffer)
        {
            Type = StructureType.BufferViewCreateInfo;
            Buffer = buffer;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum BufferViewCreateFlags
    {
        None = 0,
    }
}
