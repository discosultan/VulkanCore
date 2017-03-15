using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a sampler object.
    /// <para>
    /// <see cref="Sampler"/> objects represent the state of an image sampler which is used by the
    /// implementation to read image data and apply filtering and other transformations for the shader.
    /// </para>
    /// </summary>
    public unsafe class Sampler : DisposableHandle<long>
    {
        internal Sampler(Device parent, SamplerCreateInfo* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            createInfo->Prepare();

            long handle;
            Result result = vkCreateSampler(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Destroy a sampler object.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroySampler(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateSamplerDelegate(IntPtr device, SamplerCreateInfo* createInfo, AllocationCallbacks.Native* allocator, long* sampler);
        private static readonly vkCreateSamplerDelegate vkCreateSampler = VulkanLibrary.GetProc<vkCreateSamplerDelegate>(nameof(vkCreateSampler));

        private delegate void vkDestroySamplerDelegate(IntPtr device, long sampler, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroySamplerDelegate vkDestroySampler = VulkanLibrary.GetProc<vkDestroySamplerDelegate>(nameof(vkDestroySampler));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created sampler.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SamplerCreateInfo
    {
        internal StructureType Type;
        internal IntPtr Next;
        internal SamplerCreateFlags Flags;

        /// <summary>
        /// The magnification filter to apply to lookups.
        /// </summary>
        public Filter MagFilter;
        /// <summary>
        /// The minification filter to apply to lookups.
        /// </summary>
        public Filter MinFilter;
        /// <summary>
        /// The mipmap filter to apply to lookups.
        /// </summary>
        public SamplerMipmapMode MipmapMode;
        /// <summary>
        /// The addressing mode for outside [0..1] range for U coordinate.
        /// </summary>
        public SamplerAddressMode AddressModeU;
        /// <summary>
        /// The addressing mode for outside [0..1] range for V coordinate.
        /// </summary>
        public SamplerAddressMode AddressModeV;
        /// <summary>
        /// The addressing mode for outside [0..1] range for W coordinate.
        /// </summary>
        public SamplerAddressMode AddressModeW;
        /// <summary>
        /// The bias to be added to mipmap LOD calculation and bias provided by image sampling
        /// functions in SPIR-V.
        /// </summary>
        public float MipLodBias;
        /// <summary>
        /// Is <c>true</c> to enable anisotropic filtering, or <c>false</c> otherwise.
        /// </summary>
        public Bool AnisotropyEnable;
        /// <summary>
        /// The anisotropy value clamp.
        /// </summary>
        public float MaxAnisotropy;
        /// <summary>
        /// Is <c>true</c> to enable comparison against a reference value during lookups, or
        /// <c>false</c> otherwise.
        /// </summary>
        public Bool CompareEnable;
        /// <summary>
        /// The comparison function to apply to fetched data before filtering.
        /// </summary>
        public CompareOp CompareOp;
        /// <summary>
        /// The value used to clamp the computed level-of-detail value.
        /// <para>Must be less than or equal to <see cref="MaxLod"/>.</para>
        /// </summary>
        public float MinLod;
        /// <summary>
        /// The value used to clamp the computed level-of-detail value.
        /// <para>Must be greater than or equal to <see cref="MinLod"/>.</para>
        /// </summary>
        public float MaxLod;
        /// <summary>
        /// The predefined border color to use.
        /// </summary>
        public BorderColor BorderColor;
        /// <summary>
        /// Controls whether to use unnormalized or normalized texel coordinates to address texels of
        /// the image. When set to <c>true</c>, the range of the image coordinates used to lookup the
        /// texel is in the range of zero to the image dimensions for x, y and z. When set to
        /// <c>false</c> the range of image coordinates is zero to one.
        /// </summary>
        public Bool UnnormalizedCoordinates;

        internal void Prepare()
        {
            Type = StructureType.SamplerCreateInfo;
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum SamplerCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Specify mipmap mode used for texture lookups.
    /// </summary>
    public enum SamplerMipmapMode
    {
        /// <summary>
        /// Choose nearest mip level.
        /// </summary>
        Nearest = 0,
        /// <summary>
        /// Linear filter between mip levels.
        /// </summary>
        Linear = 1
    }

    /// <summary>
    /// Specify behavior of sampling with texture coordinates outside an image.
    /// </summary>
    public enum SamplerAddressMode
    {
        /// <summary>
        /// Indicates that the repeat wrap mode will be used.
        /// </summary>
        Repeat = 0,
        /// <summary>
        /// Indicates that the mirrored repeat wrap mode will be used.
        /// </summary>
        MirroredRepeat = 1,
        /// <summary>
        /// Indicates that the clamp to edge wrap mode will be used.
        /// </summary>
        ClampToEdge = 2,
        /// <summary>
        /// Indicates that the clamp to border wrap mode will be used.
        /// </summary>
        ClampToBorder = 3,
        /// <summary>
        /// Indicates that the mirror clamp to edge wrap mode will be used. This is only valid if the
        /// "VK_KHR_mirror_clamp_to_edge" extension is enabled.
        /// </summary>
        MirrorClampToEdge = 4
    }

    /// <summary>
    /// Specify border color used for texture lookups.
    /// </summary>
    public enum BorderColor
    {
        FloatTransparentBlack = 0,
        IntTransparentBlack = 1,
        FloatOpaqueBlack = 2,
        IntOpaqueBlack = 3,
        FloatOpaqueWhite = 4,
        IntOpaqueWhite = 5
    }
}
