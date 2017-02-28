using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a render pass object.
    /// <para>
    /// A render pass represents a collection of attachments, subpasses, and dependencies between the
    /// subpasses, and describes how the attachments are used over the course of the subpasses. The
    /// use of a render pass in a command buffer is a render pass instance.
    /// </para>
    /// </summary>
    public unsafe class RenderPass : DisposableHandle<long>
    {
        internal RenderPass(Device parent, ref RenderPassCreateInfo createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            fixed (AttachmentDescription* attachmentsPtr = createInfo.Attachments)
            fixed (SubpassDependency* dependenciesPtr = createInfo.Dependencies)
            {
                createInfo.ToNative(out RenderPassCreateInfo.Native nativeCreateInfo, attachmentsPtr, dependenciesPtr);
                long handle;
                Result result = CreateRenderPass(Parent, &nativeCreateInfo, NativeAllocator, &handle);
                nativeCreateInfo.Free();
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Returns the granularity for optimal render area.
        /// </summary>
        /// <returns>The structure in which the granularity is returned.</returns>
        public Extent2D GetRenderAreaGranularity()
        {
            Extent2D granularity;
            GetRenderAreaGranularity(Parent, this, &granularity);
            return granularity;
        }

        /// <summary>
        /// Create a new framebuffer object.
        /// </summary>
        /// <param name="createInfo">
        /// The structure which describes additional information about framebuffer creation.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>The resulting framebuffer object.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Framebuffer CreateFramebuffer(FramebufferCreateInfo createInfo, AllocationCallbacks? allocator = null)
        {
            return new Framebuffer(Parent, this, ref createInfo, ref allocator);
        }

        /// <summary>
        /// Destroy a render pass object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) DestroyRenderPass(Parent, this, NativeAllocator);
            base.Dispose();
        }

        [DllImport(VulkanDll, EntryPoint = "vkCreateRenderPass", CallingConvention = CallConv)]
        private static extern Result CreateRenderPass(IntPtr device, 
            RenderPassCreateInfo.Native* createInfo, AllocationCallbacks.Native* allocator, long* renderPass);

        [DllImport(VulkanDll, EntryPoint = "vkDestroyRenderPass", CallingConvention = CallConv)]
        private static extern void DestroyRenderPass(IntPtr device, long renderPass, AllocationCallbacks.Native* allocator);

        [DllImport(VulkanDll, EntryPoint = "vkGetRenderAreaGranularity", CallingConvention = CallConv)]
        private static extern void GetRenderAreaGranularity(IntPtr device, long renderPass, Extent2D* granularity);
    }

    /// <summary>
    /// Structure specifying parameters of a newly created render pass.
    /// </summary>
    public unsafe struct RenderPassCreateInfo
    {
        /// <summary>
        /// Structures describing properties of the attachments, or <c>null</c>.
        /// </summary>
        public AttachmentDescription[] Attachments;
        /// <summary>
        /// Structures describing properties of the subpasses.
        /// </summary>
        public SubpassDescription[] Subpasses;
        /// <summary>
        /// Structures describing dependencies between pairs of subpasses, or <c>null</c>.
        /// </summary>
        public SubpassDependency[] Dependencies;

        /// <summary>
        /// Initializes a new instnace of the <see cref="RenderPassCreateInfo"/> structure.
        /// </summary>
        /// <param name="subpasses">Structures describing properties of the subpasses.</param>
        /// <param name="attachments">Structures describing properties of the attachments, or <c>null</c>.</param>
        /// <param name="dependencies">
        /// Structures describing dependencies between pairs of subpasses, or <c>null</c>.
        /// </param>
        public RenderPassCreateInfo(
            SubpassDescription[] subpasses, 
            AttachmentDescription[] attachments = null, 
            SubpassDependency[] dependencies = null)
        {
            Attachments = attachments;
            Subpasses = subpasses;
            Dependencies = dependencies;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public RenderPassCreateFlags Flags;
            public int AttachmentCount;
            public AttachmentDescription* Attachments;
            public int SubpassCount;
            public SubpassDescription.Native* Subpasses;
            public int DependencyCount;
            public SubpassDependency* Dependencies;

            public void Free()
            {
                for (int i = 0; i < SubpassCount; i++)
                    Subpasses[i].Free();
                Interop.Free(Subpasses);
            }
        }

        internal void ToNative(out Native native, AttachmentDescription* attachments, SubpassDependency* dependencies)
        {
            int subpassCount = Subpasses?.Length ?? 0;
            var subpasses = (SubpassDescription.Native*)Interop.Alloc<SubpassDescription.Native>(subpassCount);
            for (int i = 0; i < subpassCount; i++)
                Subpasses[i].ToNative(out subpasses[i]);

            native.Type = StructureType.RenderPassCreateInfo;
            native.Next = IntPtr.Zero;
            native.Flags = 0;
            native.AttachmentCount = Attachments?.Length ?? 0;
            native.Attachments = attachments;
            native.SubpassCount = subpassCount;
            native.Subpasses = subpasses;
            native.DependencyCount = Dependencies?.Length ?? 0;
            native.Dependencies = dependencies;
        }
    }

    [Flags]
    // Is reserved for future use.
    internal enum RenderPassCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying a subpass description.
    /// </summary>
    public struct SubpassDescription
    {
        /// <summary>
        /// A bitmask indicating usage of the subpass.
        /// </summary>
        public SubpassDescriptionFlags Flags;
        /// <summary>
        /// Structures that lists which of the render pass’s attachments will be used as color
        /// attachments in the subpass, and what layout each attachment will be in during the
        /// subpass. Each element of the array corresponds to a fragment shader output location, i.e.
        /// if the shader declared an output variable layout(location=X) then it uses the attachment
        /// provided in <see cref="ColorAttachments"/>[X].
        /// </summary>
        public AttachmentReference[] ColorAttachments;
        /// <summary>
        /// Structures that lists which of the render pass's attachments can be read in the shader
        /// during the subpass, and what layout each attachment will be in during the subpass. Each
        /// element of the array corresponds to an input attachment unit number in the shader, i.e.
        /// if the shader declares an input variable `layout(inputAttachmentIndex=X, set=Y,
        /// binding=Z)` then it uses the attachment provided in <see cref="InputAttachments"/>[X].
        /// Input attachments must also be bound to the pipeline with a descriptor set, with the
        /// input attachment descriptor written in the location (set=Y, binding=Z).
        /// </summary>
        public AttachmentReference[] InputAttachments;
        /// <summary>
        /// Is <c>null</c> or an array of structures that lists which of the render pass's
        /// attachments are resolved to at the end of the subpass, and what layout each attachment
        /// will be in during the multisample resolve operation. If <see cref="ResolveAttachments"/>
        /// is not <c>null</c>, each of its elements corresponds to a color attachment (the element
        /// in <see cref="ColorAttachments"/> at the same index), and a multisample resolve operation
        /// is defined for each attachment. At the end of each subpass, multisample resolve
        /// operations read the subpass's color attachments, and resolve the samples for each pixel
        /// to the same pixel location in the corresponding resolve attachments, unless the resolve
        /// attachment index is <see cref="AttachmentUnused"/>. If the first use of an attachment in
        /// a render pass is as a resolve attachment, then the <see cref="AttachmentLoadOp"/> is
        /// effectively ignored as the resolve is guaranteed to overwrite all pixels in the render area.
        /// </summary>
        public AttachmentReference[] ResolveAttachments;
        /// <summary>
        /// Specifies which attachment will be used for depth/stencil data and the layout it will be
        /// in during the subpass. Setting the attachment index to <see cref="AttachmentUnused"/> or
        /// leaving this as <c>null</c> indicates that no depth/stencil attachment will be used in
        /// the subpass.
        /// </summary>
        public AttachmentReference? DepthStencilAttachment;
        /// <summary>
        /// Render pass attachment indices describing the attachments that are not used by a subpass,
        /// but whose contents must be preserved throughout the subpass.
        /// </summary>
        public int[] PreserveAttachments;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubpassDescription"/> structure.
        /// </summary>
        /// <param name="flags">A bitmask indicating usage of the subpass.</param>
        /// <param name="colorAttachments">
        /// Structures that lists which of the render pass’s attachments will be used as color
        /// attachments in the subpass, and what layout each attachment will be in during the
        /// subpass. Each element of the array corresponds to a fragment shader output location, i.e.
        /// if the shader declared an output variable layout(location=X) then it uses the attachment
        /// provided in <see cref="ColorAttachments"/>[X].
        /// </param>
        /// <param name="inputAttachments">
        /// Structures that lists which of the render pass's attachments can be read in the shader
        /// during the subpass, and what layout each attachment will be in during the subpass. Each
        /// element of the array corresponds to an input attachment unit number in the shader, i.e.
        /// if the shader declares an input variable `layout(inputAttachmentIndex=X, set=Y,
        /// binding=Z)` then it uses the attachment provided in <see cref="InputAttachments"/>[X].
        /// Input attachments must also be bound to the pipeline with a descriptor set, with the
        /// input attachment descriptor written in the location (set=Y, binding=Z).
        /// </param>
        /// <param name="resolveAttachments">
        /// Is <c>null</c> or an array of structures that lists which of the render pass's
        /// attachments are resolved to at the end of the subpass, and what layout each attachment
        /// will be in during the multisample resolve operation. If <see cref="ResolveAttachments"/>
        /// is not <c>null</c>, each of its elements corresponds to a color attachment (the element
        /// in <see cref="ColorAttachments"/> at the same index), and a multisample resolve operation
        /// is defined for each attachment. At the end of each subpass, multisample resolve
        /// operations read the subpass's color attachments, and resolve the samples for each pixel
        /// to the same pixel location in the corresponding resolve attachments, unless the resolve
        /// attachment index is <see cref="AttachmentUnused"/>. If the first use of an attachment in
        /// a render pass is as a resolve attachment, then the <see cref="AttachmentLoadOp"/> is
        /// effectively ignored as the resolve is guaranteed to overwrite all pixels in the render area.
        /// </param>
        /// <param name="depthStencilAttachment">
        /// Specifies which attachment will be used for depth/stencil data and the layout it will be
        /// in during the subpass. Setting the attachment index to <see cref="AttachmentUnused"/> or
        /// leaving this as <c>null</c> indicates that no depth/stencil attachment will be used in
        /// the subpass.
        /// </param>
        /// <param name="preserveAttachments">
        /// Render pass attachment indices describing the attachments that are not used by a subpass,
        /// but whose contents must be preserved throughout the subpass.
        /// </param>
        public SubpassDescription(
            SubpassDescriptionFlags flags = 0,
            AttachmentReference[] colorAttachments = null,
            AttachmentReference[] inputAttachments = null,
            AttachmentReference[] resolveAttachments = null,
            AttachmentReference? depthStencilAttachment = null,
            int[] preserveAttachments = null)
        {
            Flags = flags;
            ColorAttachments = colorAttachments;
            InputAttachments = inputAttachments;
            ResolveAttachments = resolveAttachments;
            DepthStencilAttachment = depthStencilAttachment;
            PreserveAttachments = preserveAttachments;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubpassDescription"/> structure.
        /// </summary>
        /// <param name="colorAttachments">
        /// Structures that lists which of the render pass’s attachments will be used as color
        /// attachments in the subpass, and what layout each attachment will be in during the
        /// subpass. Each element of the array corresponds to a fragment shader output location, i.e.
        /// if the shader declared an output variable layout(location=X) then it uses the attachment
        /// provided in <see cref="ColorAttachments"/>[X].
        /// </param>
        /// <param name="depthStencilAttachment">
        /// Specifies which attachment will be used for depth/stencil data and the layout it will be
        /// in during the subpass. Setting the attachment index to <see cref="AttachmentUnused"/> or
        /// leaving this as <c>null</c> indicates that no depth/stencil attachment will be used in
        /// the subpass.
        /// </param>
        public SubpassDescription(
            AttachmentReference[] colorAttachments,
            AttachmentReference? depthStencilAttachment = null)
        {
            Flags = 0;
            ColorAttachments = colorAttachments;
            InputAttachments = null;
            ResolveAttachments = null;
            DepthStencilAttachment = depthStencilAttachment;
            PreserveAttachments = null;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public SubpassDescriptionFlags Flags;
            public PipelineBindPoint PipelineBindPoint;
            public int InputAttachmentCount;
            public IntPtr InputAttachments;
            public int ColorAttachmentCount;
            public IntPtr ColorAttachments;
            public IntPtr ResolveAttachments;
            public IntPtr DepthStencilAttachment;
            public int PreserveAttachmentCount;
            public IntPtr PreserveAttachments;

            public void Free()
            {
                Interop.Free(InputAttachments);
                Interop.Free(ColorAttachments);
                Interop.Free(ResolveAttachments);
                Interop.Free(DepthStencilAttachment);
                Interop.Free(PreserveAttachments);
            }
        }

        internal void ToNative(out Native native)
        {
            // Only graphics subpasses are supported.
            native.Flags = Flags;
            native.PipelineBindPoint = PipelineBindPoint.Graphics;
            native.InputAttachmentCount = InputAttachments?.Length ?? 0;
            native.InputAttachments = Interop.Struct.AllocToPointer(InputAttachments);
            native.ColorAttachmentCount = ColorAttachments?.Length ?? 0;
            native.ColorAttachments = Interop.Struct.AllocToPointer(ColorAttachments);
            native.ResolveAttachments = Interop.Struct.AllocToPointer(ResolveAttachments);
            native.DepthStencilAttachment = Interop.Struct.AllocToPointer(ref DepthStencilAttachment);
            native.PreserveAttachmentCount = PreserveAttachments?.Length ?? 0;
            native.PreserveAttachments = Interop.Struct.AllocToPointer(PreserveAttachments);
        }
    }

    /// <summary>
    /// Bitmask specifying usage of a subpass.
    /// </summary>
    [Flags]
    public enum SubpassDescriptionFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        PerViewAttributesNvx = 1 << 0,
        PerViewPositionXOnlyNvx = 1 << 1
    }

    /// <summary>
    /// Specify the bind point of a pipeline object to a command buffer.
    /// </summary>
    public enum PipelineBindPoint
    {
        Graphics = 0,
        Compute = 1
    }

    /// <summary>
    /// Structure specifying an attachment reference.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AttachmentReference
    {
        /// <summary>
        /// The index of the attachment of the render pass, and corresponds to the index of the
        /// corresponding element in the <see cref="RenderPassCreateInfo.Attachments"/> array. If any
        /// color or depth/stencil attachments are <see cref="AttachmentUnused"/>, then no writes
        /// occur for those attachments.
        /// </summary>
        public int Attachment;
        /// <summary>
        /// Specifies the layout the attachment uses during the subpass.
        /// <para>Must not be <see cref="ImageLayout.Undefined"/> or <see cref="ImageLayout.Preinitialized"/>.</para>
        /// </summary>
        public ImageLayout Layout;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentReference"/> structure.
        /// </summary>
        /// <param name="attachment">
        /// The index of the attachment of the render pass, and corresponds to the index of the
        /// corresponding element in the <see cref="RenderPassCreateInfo.Attachments"/> array. If any
        /// color or depth/stencil attachments are <see cref="AttachmentUnused"/>, then no writes
        /// occur for those attachments.
        /// </param>
        /// <param name="layout">Specifies the layout the attachment uses during the subpass.</param>
        public AttachmentReference(int attachment, ImageLayout layout)
        {
            Attachment = attachment;
            Layout = layout;
        }
    }

    /// <summary>
    /// Structure specifying a subpass dependency.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SubpassDependency
    {
        /// <summary>
        /// The subpass index of the first subpass in the dependency, or <see cref="SubpassExternal"/>.
        /// </summary>
        public int SrcSubpass;
        /// <summary>
        /// The subpass index of the second subpass in the dependency, or <see cref="SubpassExternal"/>.
        /// </summary>
        public int DstSubpass;
        /// <summary>
        /// Defines a source stage mask.
        /// </summary>
        public PipelineStages SrcStageMask;
        /// <summary>
        /// Defines a destination stage mask.
        /// </summary>
        public PipelineStages DstStageMask;
        /// <summary>
        /// Defines a source access mask.
        /// </summary>
        public Accesses SrcAccessMask;
        /// <summary>
        /// Defines a destination access mask.
        /// </summary>
        public Accesses DstAccessMask;
        /// <summary>
        /// A bitmask of <see cref="Dependencies"/>.
        /// </summary>
        public Dependencies DependencyFlags;
    }

    /// <summary>
    /// Bitmask specifying how execution and memory dependencies are formed.
    /// </summary>
    [Flags]
    public enum Dependencies
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Dependency is per pixel region.
        /// </summary>
        ByRegion = 1 << 0,
        ViewLocalKhx = 1 << 1,
        DeviceGroupKhx = 1 << 2
    }
}