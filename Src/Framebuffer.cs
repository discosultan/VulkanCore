using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a framebuffer object.
    /// <para>
    /// Render passes operate in conjunction with framebuffers. Framebuffers represent a collection
    /// of specific memory attachments that a render pass instance uses.
    /// </para>
    /// </summary>
    public unsafe class Framebuffer : DisposableHandle<long>
    {
        internal Framebuffer(Device parent, RenderPass renderPass, ref FramebufferCreateInfo createInfo,
            ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            RenderPass = renderPass;
            Allocator = allocator;

            fixed (long* attachmentsPtr = createInfo.Attachments)
            {
                createInfo.ToNative(out FramebufferCreateInfo.Native nativeCreateInfo, attachmentsPtr, renderPass);
                long handle;
                Result result = vkCreateFramebuffer(Parent, &nativeCreateInfo, NativeAllocator, &handle);
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Gets the render pass the framebuffer is compatible with.
        /// </summary>
        public RenderPass RenderPass { get; }

        /// <summary>
        /// Destroy a framebuffer object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyFramebuffer(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateFramebufferDelegate(IntPtr device, FramebufferCreateInfo.Native* createInfo, AllocationCallbacks.Native* allocator, long* framebuffer);
        private static readonly vkCreateFramebufferDelegate vkCreateFramebuffer = VulkanLibrary.GetStaticProc<vkCreateFramebufferDelegate>(nameof(vkCreateFramebuffer));

        private delegate void vkDestroyFramebufferDelegate(IntPtr device, long framebuffer, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyFramebufferDelegate vkDestroyFramebuffer = VulkanLibrary.GetStaticProc<vkDestroyFramebufferDelegate>(nameof(vkDestroyFramebuffer));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created framebuffer.
    /// <para>
    /// Image subresources used as attachments must not be used via any non-attachment usage for the
    /// duration of a render pass instance. This restriction means that the render pass has full
    /// knowledge of all uses of all of the attachments, so that the implementation is able to make
    /// correct decisions about when and how to perform layout transitions, when to overlap execution
    /// of subpasses, etc.
    /// </para>
    /// <para>
    /// It is legal for a subpass to use no color or depth/stencil attachments, and rather use shader
    /// side effects such as image stores and atomics to produce an output. In this case, the subpass
    /// continues to use the width, height, and layers of the framebuffer to define the dimensions of
    /// the rendering area, and the rasterizationSamples from each pipeline’s <see
    /// cref="PipelineMultisampleStateCreateInfo"/> to define the number of samples used in
    /// rasterization; however, if <see cref="PhysicalDeviceFeatures.VariableMultisampleRate"/> is
    /// <c>false</c>, then all pipelines to be bound with a given zero-attachment subpass must have
    /// the same value for <see cref="PipelineMultisampleStateCreateInfo.RasterizationSamples"/>.
    /// </para>
    /// </summary>
    public unsafe struct FramebufferCreateInfo
    {
        /// <summary>
        /// An array of <see cref="ImageView"/> handles, each of which will be used as the
        /// corresponding attachment in a render pass instance.
        /// </summary>
        public long[] Attachments;
        /// <summary>
        /// Dimension of the framebuffer.
        /// <para>Must be less than or equal to <see cref="PhysicalDeviceLimits.MaxFramebufferWidth"/>.</para>
        /// </summary>
        public int Width;
        /// <summary>
        /// Dimension of the framebuffer.
        /// <para>Must be less than or equal to <see cref="PhysicalDeviceLimits.MaxFramebufferHeight"/>.</para>
        /// </summary>
        public int Height;
        /// <summary>
        /// Dimension of the framebuffer.
        /// <para>Must be less than or equal to <see cref="PhysicalDeviceLimits.MaxFramebufferLayers"/>.</para>
        /// </summary>
        public int Layers;

        /// <summary>
        /// Initializes a new instance of the <see cref="FramebufferCreateInfo"/> structure.
        /// </summary>
        /// <param name="attachments">
        /// An array of <see cref="ImageView"/> handles, each of which will be used as the
        /// corresponding attachment in a render pass instance.
        /// </param>
        /// <param name="width">Dimension of the framebuffer.</param>
        /// <param name="height">Dimension of the framebuffer.</param>
        /// <param name="layers">Dimension of the framebuffer.</param>
        public FramebufferCreateInfo(ImageView[] attachments, int width, int height, int layers = 1)
        {
            Attachments = attachments?.ToHandleArray();
            Width = width;
            Height = height;
            Layers = layers;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public FramebufferCreateFlags Flags;
            public long RenderPass;
            public int AttachmentCount;
            public long* Attachments;
            public int Width;
            public int Height;
            public int Layers;
        }

        internal void ToNative(out Native native, long* attachments, RenderPass renderPass)
        {
            native.Type = StructureType.FramebufferCreateInfo;
            native.Next = IntPtr.Zero;
            native.Flags = 0;
            native.RenderPass = renderPass;
            native.AttachmentCount = Attachments?.Length ?? 0;
            native.Attachments = attachments;
            native.Width = Width;
            native.Height = Height;
            native.Layers = Layers;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum FramebufferCreateFlags
    {
        None = 0
    }
}
