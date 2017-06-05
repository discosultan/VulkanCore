using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a queue object.
    /// </summary>
    public unsafe class Queue : VulkanHandle<IntPtr>
    {
        internal Queue(IntPtr handle, Device parent, int familyIndex, int index)
        {
            Handle = handle;
            Parent = parent;
            FamilyIndex = familyIndex;
            Index = index;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Gets the queue family index.
        /// </summary>
        public int FamilyIndex { get; }

        /// <summary>
        /// Gets the queue index.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Submits a sequence of semaphores or command buffers to a queue.
        /// </summary>
        /// <param name="submits">Structures, each specifying a command buffer submission batch.</param>
        /// <param name="fence">
        /// An optional handle to a fence to be signaled. If fence is not <c>null</c>, it defines a
        /// fence signal operation.
        /// </param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void Submit(SubmitInfo[] submits, Fence fence = null)
        {
            int count = submits?.Length ?? 0;
            var nativeSubmits = stackalloc SubmitInfo.Native[count];
            for (int i = 0; i < count; i++)
                submits[i].ToNative(out nativeSubmits[i]);

            Result result = vkQueueSubmit(this, count, nativeSubmits, fence);
            for (int i = 0; i < count; i++)
                nativeSubmits[i].Free();

            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Submits a sequence of semaphores or command buffers to a queue.
        /// </summary>
        /// <param name="submit">Specifies a command buffer submission batch.</param>
        /// <param name="fence">
        /// An optional handle to a fence to be signaled. If fence is not <c>null</c>, it defines a
        /// fence signal operation.
        /// </param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void Submit(SubmitInfo submit, Fence fence = null)
        {
            submit.ToNative(out SubmitInfo.Native nativeSubmit);
            Result result = vkQueueSubmit(this, 1, &nativeSubmit, fence);
            nativeSubmit.Free();
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Submits semaphores or a command buffer to a queue.
        /// </summary>
        /// <param name="fence">
        /// An optional handle to a fence to be signaled. If fence is not <c>null</c>, it defines a
        /// fence signal operation.
        /// </param>
        /// <param name="waitSemaphore">
        /// Semaphore upon which to wait before the command buffer for this batch begins execution.
        /// If semaphore to wait on is provided, it defines a semaphore wait operation.
        /// </param>
        /// <param name="waitDstStageMask">Pipeline stages at which semaphore wait will occur.</param>
        /// <param name="commandBuffer">Command buffer to execute in the batch.</param>
        /// <param name="signalSemaphore">
        /// Semaphore which will be signaled when the command buffer for this batch has completed
        /// execution. If semaphore to be signaled is provided, it defines a semaphore signal operation.
        /// </param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void Submit(Semaphore waitSemaphore, PipelineStages waitDstStageMask,
            CommandBuffer commandBuffer, Semaphore signalSemaphore, Fence fence = null)
        {
            long waitSemaphoreHandle = waitSemaphore;
            IntPtr commandBufferHandle = commandBuffer;
            long signalSemaphoreHandle = signalSemaphore;

            var nativeSubmit = new SubmitInfo.Native { Type = StructureType.SubmitInfo };
            if (waitSemaphoreHandle != 0)
            {
                nativeSubmit.WaitSemaphoreCount = 1;
                nativeSubmit.WaitSemaphores = new IntPtr(&waitSemaphoreHandle);
                nativeSubmit.WaitDstStageMask = new IntPtr(&waitDstStageMask);
            }
            if (commandBufferHandle != IntPtr.Zero)
            {
                nativeSubmit.CommandBufferCount = 1;
                nativeSubmit.CommandBuffers = new IntPtr(&commandBufferHandle);
            }
            if (signalSemaphoreHandle != 0)
            {
                nativeSubmit.SignalSemaphoreCount = 1;
                nativeSubmit.SignalSemaphores = new IntPtr(&signalSemaphoreHandle);
            }
            
            Result result = vkQueueSubmit(this, 1, &nativeSubmit, fence);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Wait for a queue to become idle.
        /// <para>
        /// Equivalent to submitting a fence to a queue and waiting with an infinite timeout for that
        /// fence to signal.
        /// </para>
        /// </summary>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void WaitIdle()
        {
            Result result = vkQueueWaitIdle(this);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Bind device memory to a sparse resource object.
        /// </summary>
        /// <param name="bindInfo">Specifying a sparse binding submission batch.</param>
        /// <param name="fence">
        /// An optional handle to a fence to be signaled. If fence is not <c>null</c>, it defines a
        /// fence signal operation.
        /// </param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void BindSparse(BindSparseInfo bindInfo, Fence fence = null)
        {
            bindInfo.ToNative(out BindSparseInfo.Native nativeBindInfo);
            Result result = vkQueueBindSparse(this, 1, &nativeBindInfo, fence);
            nativeBindInfo.Free();
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Bind device memory to a sparse resource object.
        /// </summary>
        /// <param name="bindInfo">
        /// An array of <see cref="BindSparseInfo"/> structures, each specifying a sparse binding
        /// submission batch.
        /// </param>
        /// <param name="fence">
        /// An optional handle to a fence to be signaled. If fence is not <c>null</c>, it defines a
        /// fence signal operation.
        /// </param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void BindSparse(BindSparseInfo[] bindInfo, Fence fence = null)
        {
            int count = bindInfo?.Length ?? 0;

            var nativeBindInfo = stackalloc BindSparseInfo.Native[count];
            for (int i = 0; i < count; i++)
                bindInfo[i].ToNative(out nativeBindInfo[i]);

            Result result = vkQueueBindSparse(this, count, nativeBindInfo, fence);

            for (int i = 0; i < count; i++)
                nativeBindInfo[i].Free();

            VulkanException.ThrowForInvalidResult(result);
        }

        private delegate Result vkQueueSubmitDelegate(IntPtr queue, int submitCount, SubmitInfo.Native* submits, long fence);
        private static readonly vkQueueSubmitDelegate vkQueueSubmit = VulkanLibrary.GetStaticProc<vkQueueSubmitDelegate>(nameof(vkQueueSubmit));

        private delegate Result vkQueueWaitIdleDelegate(IntPtr queue);
        private static readonly vkQueueWaitIdleDelegate vkQueueWaitIdle = VulkanLibrary.GetStaticProc<vkQueueWaitIdleDelegate>(nameof(vkQueueWaitIdle));

        private delegate Result vkQueueBindSparseDelegate(IntPtr queue, int bindInfoCount, BindSparseInfo.Native* bindInfo, long fence);
        private static readonly vkQueueBindSparseDelegate vkQueueBindSparse = VulkanLibrary.GetStaticProc<vkQueueBindSparseDelegate>(nameof(vkQueueBindSparse));
    }

    /// <summary>
    /// Structure specifying a queue submit operation.
    /// </summary>
    public struct SubmitInfo
    {
        /// <summary>
        /// Semaphores upon which to wait before the command buffers for this batch begin execution.
        /// If semaphores to wait on are provided, they define a semaphore wait operation.
        /// </summary>
        public long[] WaitSemaphores;
        /// <summary>
        /// Pipeline stages at which each corresponding semaphore wait will occur.
        /// </summary>
        public PipelineStages[] WaitDstStageMask;
        /// <summary>
        /// Command buffers to execute in the batch.
        /// </summary>
        public IntPtr[] CommandBuffers;
        /// <summary>
        /// Semaphores which will be signaled when the command buffers for this batch have completed
        /// execution. If semaphores to be signaled are provided, they define a semaphore signal operation.
        /// </summary>
        public long[] SignalSemaphores;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitInfo"/> structure.
        /// </summary>
        /// <param name="waitSemaphores">
        /// Semaphores upon which to wait before the command buffers for this batch begin execution.
        /// If semaphores to wait on are provided, they define a semaphore wait operation.
        /// </param>
        /// <param name="waitDstStageMask">
        /// Pipeline stages at which each corresponding semaphore wait will occur.
        /// </param>
        /// <param name="commandBuffers">
        /// Command buffers to execute in the batch. The command buffers submitted in a batch begin
        /// execution in the order they appear in <paramref name="commandBuffers"/>, but may complete
        /// out of order.
        /// </param>
        /// <param name="signalSemaphores">
        /// Semaphores which will be signaled when the command buffers for this batch have completed
        /// execution. If semaphores to be signaled are provided, they define a semaphore signal operation.
        /// </param>
        public SubmitInfo(Semaphore[] waitSemaphores = null, PipelineStages[] waitDstStageMask = null,
            CommandBuffer[] commandBuffers = null, Semaphore[] signalSemaphores = null)
        {
            WaitSemaphores = waitSemaphores?.ToHandleArray();
            WaitDstStageMask = waitDstStageMask;
            CommandBuffers = commandBuffers?.ToHandleArray();
            SignalSemaphores = signalSemaphores?.ToHandleArray();
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public int WaitSemaphoreCount;
            public IntPtr WaitSemaphores;
            public IntPtr WaitDstStageMask;
            public int CommandBufferCount;
            public IntPtr CommandBuffers;
            public int SignalSemaphoreCount;
            public IntPtr SignalSemaphores;

            public void Free()
            {
                Interop.Free(WaitSemaphores);
                Interop.Free(WaitDstStageMask);
                Interop.Free(CommandBuffers);
                Interop.Free(SignalSemaphores);
            }
        }

        internal void ToNative(out Native native)
        {
            native.Type = StructureType.SubmitInfo;
            native.Next = IntPtr.Zero;
            native.WaitSemaphoreCount = WaitSemaphores?.Length ?? 0;
            native.WaitSemaphores = Interop.Struct.AllocToPointer(WaitSemaphores);
            native.WaitDstStageMask = Interop.Struct.AllocToPointer(WaitDstStageMask);
            native.CommandBufferCount = CommandBuffers?.Length ?? 0;
            native.CommandBuffers = Interop.Struct.AllocToPointer(CommandBuffers);
            native.SignalSemaphoreCount = SignalSemaphores?.Length ?? 0;
            native.SignalSemaphores = Interop.Struct.AllocToPointer(SignalSemaphores);
        }
    }

    /// <summary>
    /// Structure specifying a sparse binding operation.
    /// </summary>
    public unsafe struct BindSparseInfo
    {
        /// <summary>
        /// Semaphores upon which to wait on before the sparse binding operations for this batch
        /// begin execution. If semaphores to wait on are provided, they define a semaphore wait operation.
        /// </summary>
        public long[] WaitSemaphores;
        /// <summary>
        /// An array of <see cref="SparseBufferMemoryBindInfo"/> structures.
        /// </summary>
        public SparseBufferMemoryBindInfo[] BufferBinds;
        /// <summary>
        /// An array of <see cref="SparseImageOpaqueMemoryBindInfo"/> structures, indicating opaque
        /// sparse image bindings to perform.
        /// </summary>
        public SparseImageOpaqueMemoryBindInfo[] ImageOpaqueBinds;
        /// <summary>
        /// An array of <see cref="SparseImageMemoryBindInfo"/> structures, indicating sparse image
        /// bindings to perform.
        /// </summary>
        public SparseImageMemoryBindInfo[] ImageBinds;
        /// <summary>
        /// Semaphores which will be signaled when the sparse binding operations for this batch have
        /// completed execution. If semaphores to be signaled are provided, they define a semaphore
        /// signal operation.
        /// </summary>
        public long[] SignalSemaphores;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindSparseInfo"/> structure.
        /// </summary>
        /// <param name="waitSemaphores">
        /// Semaphores upon which to wait on before the sparse binding operations for this batch
        /// begin execution. If semaphores to wait on are provided, they define a semaphore wait operation.
        /// </param>
        /// <param name="bufferBinds">An array of <see cref="SparseBufferMemoryBindInfo"/> structures.</param>
        /// <param name="imageOpaqueBinds">
        /// An array of <see cref="SparseImageOpaqueMemoryBindInfo"/> structures, indicating opaque
        /// sparse image bindings to perform.
        /// </param>
        /// <param name="imageBinds">
        /// An array of <see cref="SparseImageMemoryBindInfo"/> structures, indicating sparse image
        /// bindings to perform.
        /// </param>
        /// <param name="signalSemaphores">
        /// Semaphores which will be signaled when the sparse binding operations for this batch have
        /// completed execution. If semaphores to be signaled are provided, they define a semaphore
        /// signal operation.
        /// </param>
        public BindSparseInfo(Semaphore[] waitSemaphores, SparseBufferMemoryBindInfo[] bufferBinds,
            SparseImageOpaqueMemoryBindInfo[] imageOpaqueBinds, SparseImageMemoryBindInfo[] imageBinds,
            Semaphore[] signalSemaphores)
        {
            WaitSemaphores = waitSemaphores?.ToHandleArray();
            BufferBinds = bufferBinds;
            ImageOpaqueBinds = imageOpaqueBinds;
            ImageBinds = imageBinds;
            SignalSemaphores = signalSemaphores?.ToHandleArray();
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public int WaitSemaphoreCount;
            public IntPtr WaitSemaphores;
            public int BufferBindCount;
            public SparseBufferMemoryBindInfo.Native* BufferBinds;
            public int ImageOpaqueBindCount;
            public SparseImageOpaqueMemoryBindInfo.Native* ImageOpaqueBinds;
            public int ImageBindCount;
            public SparseImageMemoryBindInfo.Native* ImageBinds;
            public int SignalSemaphoreCount;
            public IntPtr SignalSemaphores;

            public void Free()
            {
                Interop.Free(WaitSemaphores);
                for (int i = 0; i < BufferBindCount; i++)
                    BufferBinds[i].Free();
                Interop.Free(BufferBinds);
                for (int i = 0; i < ImageOpaqueBindCount; i++)
                    ImageOpaqueBinds[i].Free();
                Interop.Free(ImageOpaqueBinds);
                for (int i = 0; i < ImageBindCount; i++)
                    ImageBinds[i].Free();
                Interop.Free(ImageBinds);
                Interop.Free(SignalSemaphores);
            }
        }

        internal void ToNative(out Native native)
        {
            int bufferBindCount = BufferBinds?.Length ?? 0;
            int imageOpaqueBindCount = ImageOpaqueBinds?.Length ?? 0;
            int imageBindCount = ImageBinds?.Length ?? 0;

            var bufferBinds = (SparseBufferMemoryBindInfo.Native*)
                Interop.Alloc<SparseBufferMemoryBindInfo.Native>(bufferBindCount);
            for (int i = 0; i < bufferBindCount; i++)
                BufferBinds[i].ToNative(&bufferBinds[i]);
            var imageOpaqueBinds = (SparseImageOpaqueMemoryBindInfo.Native*)
                Interop.Alloc<SparseImageOpaqueMemoryBindInfo.Native>(bufferBindCount);
            for (int i = 0; i < imageOpaqueBindCount; i++)
                ImageOpaqueBinds[i].ToNative(&imageOpaqueBinds[i]);
            var imageBinds = (SparseImageMemoryBindInfo.Native*)
                Interop.Alloc<SparseImageMemoryBindInfo.Native>(bufferBindCount);
            for (int i = 0; i < imageBindCount; i++)
                ImageBinds[i].ToNative(&imageBinds[i]);

            native.Type = StructureType.BindSparseInfo;
            native.Next = IntPtr.Zero;
            native.WaitSemaphoreCount = WaitSemaphores?.Length ?? 0;
            native.WaitSemaphores = Interop.Struct.AllocToPointer(WaitSemaphores);
            native.BufferBindCount = bufferBindCount;
            native.BufferBinds = bufferBinds;
            native.ImageOpaqueBindCount = imageOpaqueBindCount;
            native.ImageOpaqueBinds = imageOpaqueBinds;
            native.ImageBindCount = imageBindCount;
            native.ImageBinds = imageBinds;
            native.SignalSemaphoreCount = SignalSemaphores?.Length ?? 0;
            native.SignalSemaphores = Interop.Struct.AllocToPointer(SignalSemaphores);
        }
    }

    /// <summary>
    /// Structure specifying a sparse buffer memory bind operation.
    /// </summary>
    public unsafe struct SparseBufferMemoryBindInfo
    {
        /// <summary>
        /// The <see cref="VulkanCore.Buffer"/> object to be bound.
        /// </summary>
        public long Buffer;
        /// <summary>
        /// An array of <see cref="SparseMemoryBind"/> structures.
        /// </summary>
        public SparseMemoryBind[] Binds;

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseBufferMemoryBindInfo"/> structure.
        /// </summary>
        /// <param name="buffer">The <see cref="VulkanCore.Buffer"/> object to be bound.</param>
        /// <param name="binds">An array of <see cref="SparseMemoryBind"/> structures.</param>
        public SparseBufferMemoryBindInfo(Buffer buffer, params SparseMemoryBind[] binds)
        {
            Buffer = buffer;
            Binds = binds;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public long Buffer;
            public int BindCount;
            public IntPtr Binds;

            public void Free()
            {
                Interop.Free(Binds);
            }
        }

        internal void ToNative(Native* native)
        {
            native->Buffer = Buffer;
            native->BindCount = Binds?.Length ?? 0;
            native->Binds = Interop.Struct.AllocToPointer(Binds);
        }
    }

    /// <summary>
    /// Structure specifying a sparse memory bind operation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SparseMemoryBind
    {
        /// <summary>
        /// The offset into the resource.
        /// </summary>
        public long ResourceOffset;
        /// <summary>
        /// The size of the memory region to be bound.
        /// </summary>
        public long Size;
        /// <summary>
        /// The <see cref="DeviceMemory"/> object that the range of the resource is bound to. If
        /// memory 0, the range is unbound.
        /// </summary>
        public long Memory;
        /// <summary>
        /// The offset into the <see cref="DeviceMemory"/> object to bind the resource range to. If
        /// memory is 0, this value is ignored.
        /// </summary>
        public long MemoryOffset;
        /// <summary>
        /// A bitmask specifying usage of the binding operation.
        /// </summary>
        public SparseMemoryBindFlags Flags;

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseMemoryBind"/> structure.
        /// </summary>
        /// <param name="resourceOffset">The offset into the resource.</param>
        /// <param name="size">The size of the memory region to be bound.</param>
        /// <param name="memory">
        /// The <see cref="DeviceMemory"/> object that the range of the resource is bound to. If
        /// memory 0, the range is unbound.
        /// </param>
        /// <param name="memoryOffset">
        /// The offset into the <see cref="DeviceMemory"/> object to bind the resource range to. If
        /// memory is 0, this value is ignored.
        /// </param>
        /// <param name="flags">A bitmask specifying usage of the binding operation.</param>
        public SparseMemoryBind(long resourceOffset, long size, DeviceMemory memory = null,
            long memoryOffset = 0, SparseMemoryBindFlags flags = 0)
        {
            ResourceOffset = resourceOffset;
            Size = size;
            Memory = memory;
            MemoryOffset = memoryOffset;
            Flags = flags;
        }
    }

    /// <summary>
    /// Bitmask specifying usage of a sparse memory binding operation.
    /// </summary>
    [Flags]
    public enum SparseMemoryBindFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that the memory being bound is only for the metadata aspect.
        /// </summary>
        Metadata = 1 << 0
    }

    /// <summary>
    /// Structure specifying sparse image opaque memory bind info.
    /// </summary>
    public unsafe struct SparseImageOpaqueMemoryBindInfo
    {
        /// <summary>
        /// The <see cref="VulkanCore.Image"/> object to be bound.
        /// </summary>
        public long Image;
        /// <summary>
        /// An array of <see cref="SparseMemoryBind"/> structures.
        /// <para>Length must be greater than 0.</para>
        /// </summary>
        public SparseMemoryBind[] Binds;

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseImageOpaqueMemoryBindInfo"/> structure.
        /// </summary>
        /// <param name="image">The <see cref="VulkanCore.Image"/> object to be bound.</param>
        /// <param name="binds">An array of <see cref="SparseMemoryBind"/> structures.</param>
        public SparseImageOpaqueMemoryBindInfo(Image image, params SparseMemoryBind[] binds)
        {
            Image = image;
            Binds = binds;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public long Image;
            public int BindCount;
            public IntPtr Binds;

            public void Free()
            {
                Interop.Free(Binds);
            }
        }

        internal void ToNative(Native* native)
        {
            native->Image = Image;
            native->BindCount = Binds?.Length ?? 0;
            native->Binds = Interop.Struct.AllocToPointer(Binds);
        }
    }

    /// <summary>
    /// Structure specifying sparse image memory bind info.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SparseImageMemoryBindInfo
    {
        /// <summary>
        /// The <see cref="VulkanCore.Image"/> object to be bound.
        /// </summary>
        public long Image;
        /// <summary>
        /// An array of <see cref="SparseImageMemoryBind"/> structures.
        /// <para>Length must be greater than 0.</para>
        /// </summary>
        public SparseImageMemoryBind[] Binds;

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseImageMemoryBind"/> structure.
        /// </summary>
        /// <param name="image">The <see cref="VulkanCore.Image"/> object to be bound.</param>
        /// <param name="binds">An array of <see cref="SparseImageMemoryBind"/> structures.</param>
        public SparseImageMemoryBindInfo(Image image, params SparseImageMemoryBind[] binds)
        {
            Image = image;
            Binds = binds;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public long Image;
            public int BindCount;
            public IntPtr Binds;

            public void Free()
            {
                Interop.Free(Binds);
            }
        }

        internal void ToNative(Native* native)
        {
            native->Image = Image;
            native->BindCount = Binds?.Length ?? 0;
            native->Binds = Interop.Struct.AllocToPointer(Binds);
        }
    }

    /// <summary>
    /// Structure specifying sparse image memory bind.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SparseImageMemoryBind
    {
        /// <summary>
        /// The aspect mask and region of interest in the image.
        /// <para>Must be a valid subresource for <see cref="Image"/>.</para>
        /// </summary>
        public ImageSubresource Subresource;
        /// <summary>
        /// The coordinates of the first texel within the image subresource to bind.
        /// </summary>
        public Offset3D Offset;
        /// <summary>
        /// The size in texels of the region within the image subresource to bind. The extent must be
        /// a multiple of the sparse image block dimensions, except when binding sparse image blocks
        /// along the edge of an image subresource it can instead be such that any coordinate of <see
        /// cref="Offset"/> + <see cref="Extent"/> equals the corresponding dimensions of the image subresource.
        /// </summary>
        public Extent3D Extent;
        /// <summary>
        /// The <see cref="DeviceMemory"/> object that the sparse image blocks of the image are bound
        /// to. If memory is 0, the sparse image blocks are unbound.
        /// <para>Must match the memory requirements of the calling command's <see cref="Image"/>.</para>
        /// </summary>
        public long Memory;
        /// <summary>
        /// An offset into <see cref="DeviceMemory"/> object. If memory is 0, this value is ignored.
        /// <para>Must match the memory requirements of the calling command's <see cref="Image"/>.</para>
        /// </summary>
        public long MemoryOffset;
        /// <summary>
        /// Sparse memory binding flags.
        /// </summary>
        public SparseMemoryBindFlags Flags;
    }
}
