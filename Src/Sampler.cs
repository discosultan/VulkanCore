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
        private static readonly vkCreateSamplerDelegate vkCreateSampler = VulkanLibrary.GetStaticProc<vkCreateSamplerDelegate>(nameof(vkCreateSampler));

        private delegate void vkDestroySamplerDelegate(IntPtr device, long sampler, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroySamplerDelegate vkDestroySampler = VulkanLibrary.GetStaticProc<vkDestroySamplerDelegate>(nameof(vkDestroySampler));
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
        /// Specifies the magnification filter to apply to lookups.
        /// </summary>
        public Filter MagFilter;
        /// <summary>
        /// Specifies the minification filter to apply to lookups.
        /// </summary>
        public Filter MinFilter;
        /// <summary>
        /// Specifies the mipmap filter to apply to lookups.
        /// </summary>
        public SamplerMipmapMode MipmapMode;
        /// <summary>
        /// Specifies the addressing mode for outside [0..1] range for U coordinate.
        /// </summary>
        public SamplerAddressMode AddressModeU;
        /// <summary>
        /// Specifies the addressing mode for outside [0..1] range for V coordinate.
        /// </summary>
        public SamplerAddressMode AddressModeV;
        /// <summary>
        /// Specifies the addressing mode for outside [0..1] range for W coordinate.
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
        /// The anisotropy value clamp used by the sampler when <see cref="AnisotropyEnable"/> is <c>true</c>.
        /// <para>
        /// If <see cref="AnisotropyEnable"/> is <c>false</c>, <see cref="MaxAnisotropy"/> is ignored.
        /// </para>
        /// </summary>
        public float MaxAnisotropy;
        /// <summary>
        /// Is <c>true</c> to enable comparison against a reference value during lookups, or
        /// <c>false</c> otherwise.
        /// </summary>
        public Bool CompareEnable;
        /// <summary>
        /// Specifies the comparison function to apply to fetched data before filtering.
        /// </summary>
        public CompareOp CompareOp;
        /// <summary>
        /// The value used to clamp the computed LOD value.
        /// <para>Must be less than or equal to <see cref="MaxLod"/>.</para>
        /// </summary>
        public float MinLod;
        /// <summary>
        /// The value used to clamp the computed LOD value.
        /// <para>Must be greater than or equal to <see cref="MinLod"/>.</para>
        /// </summary>
        public float MaxLod;
        /// <summary>
        /// Specifies the predefined border color to use.
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
        /// Specifies nearest filtering.
        /// </summary>
        Nearest = 0,
        /// <summary>
        /// Specifies linear filtering.
        /// </summary>
        Linear = 1
    }

    /// <summary>
    /// Specify behavior of sampling with texture coordinates outside an image.
    /// </summary>
    public enum SamplerAddressMode
    {
        /// <summary>
        /// Specifies that the repeat wrap mode will be used.
        /// </summary>
        Repeat = 0,
        /// <summary>
        /// Specifies that the mirrored repeat wrap mode will be used.
        /// </summary>
        MirroredRepeat = 1,
        /// <summary>
        /// Specifies that the clamp to edge wrap mode will be used.
        /// </summary>
        ClampToEdge = 2,
        /// <summary>
        /// Specifies that the clamp to border wrap mode will be used.
        /// </summary>
        ClampToBorder = 3,
        /// <summary>
        /// Specifies that the mirror clamp to edge wrap mode will be used. This is only valid if the
        /// "VK_KHR_mirror_clamp_to_edge" extension is enabled.
        /// </summary>
        MirrorClampToEdge = 4
    }

    /// <summary>
    /// Specify border color used for texture lookups.
    /// </summary>
    public enum BorderColor
    {
        /// <summary>
        /// Specifies a transparent, floating-point format, black color.
        /// </summary>
        FloatTransparentBlack = 0,
        /// <summary>
        /// Specifies a transparent, integer format, black color.
        /// </summary>
        IntTransparentBlack = 1,
        /// <summary>
        /// Specifies an opaque, floating-point format, black color.
        /// </summary>
        FloatOpaqueBlack = 2,
        /// <summary>
        /// Specifies an opaque, integer format, black color.
        /// </summary>
        IntOpaqueBlack = 3,
        /// <summary>
        /// Specifies an opaque, floating-point format, white color.
        /// </summary>
        FloatOpaqueWhite = 4,
        /// <summary>
        /// Specifies an opaque, integer format, white color.
        /// </summary>
        IntOpaqueWhite = 5
    }
}
