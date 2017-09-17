using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Khr
{
    /// <summary>
    /// A sampler Y'C~B~C~R~ conversion is an opaque representation of a device-specific sampler
    /// Y'C~B~C~R~ conversion description.
    /// </summary>
    public unsafe class SamplerYcbcrConversionKhr : DisposableHandle<long>
    {
        internal SamplerYcbcrConversionKhr(Device parent, SamplerYcbcrConversionCreateInfoKhr* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            long handle;
            Result result = vkCreateSamplerYcbcrConversionKHR(Parent)(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Destroy a created Y'CbCr conversion.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroySamplerYcbcrConversionKHR(Parent)(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateSamplerYcbcrConversionKHRDelegate(IntPtr device, SamplerYcbcrConversionCreateInfoKhr* createInfo, AllocationCallbacks.Native* allocator, long* ycbcrConversion);
        private static vkCreateSamplerYcbcrConversionKHRDelegate vkCreateSamplerYcbcrConversionKHR(Device device) => device.GetProc<vkCreateSamplerYcbcrConversionKHRDelegate>(nameof(vkCreateSamplerYcbcrConversionKHR));

        private delegate void vkDestroySamplerYcbcrConversionKHRDelegate(IntPtr device, long ycbcrConversion, AllocationCallbacks.Native* allocator);
        private static vkDestroySamplerYcbcrConversionKHRDelegate vkDestroySamplerYcbcrConversionKHR(Device device) => device.GetProc<vkDestroySamplerYcbcrConversionKHRDelegate>(nameof(vkDestroySamplerYcbcrConversionKHR));
    }

    /// <summary>
    /// Structure specifying the parameters of the newly created conversion.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SamplerYcbcrConversionCreateInfoKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The format of the image from which color information will be retrieved.
        /// </summary>
        public Format Format;
        /// <summary>
        /// Describes the color matrix for conversion between color models.
        /// </summary>
        public SamplerYcbcrModelConversionKhr YcbcrModel;
        /// <summary>
        /// Describes whether the encoded values have headroom and foot room, or whether the encoding
        /// uses the full numerical range.
        /// </summary>
        public SamplerYcbcrRangeKhr YcbcrRange;
        /// <summary>
        /// Applies a swizzle based on <see cref="ComponentSwizzle"/> enums prior to range expansion
        /// and color model conversion.
        /// </summary>
        public ComponentMapping Components;
        /// <summary>
        /// Describes the sample location associated with downsampled chroma channels in the x
        /// dimension. <c>XChromaOffset</c> has no effect for formats in which chroma channels are
        /// the same resolution as the luma channel.
        /// </summary>
        public ChromaLocationKhr XChromaOffset;
        /// <summary>
        /// Describes the sample location associated with downsampled chroma channels in the y
        /// dimension. <c>YChromaOffset</c> has no effect for formats in which the chroma channels
        /// are not downsampled vertically.
        /// </summary>
        public ChromaLocationKhr YChromaOffset;
        /// <summary>
        /// The filter for chroma reconstruction.
        /// </summary>
        public Filter ChromaFilter;
        /// <summary>
        /// Can be used to ensure that reconstruction is done explicitly, if supported.
        /// </summary>
        public Bool ForceExplicitReconstruction;
    }

    /// <summary>
    /// Color model component of a color space.
    /// </summary>
    public enum SamplerYcbcrModelConversionKhr
    {
        /// <summary>
        /// Specifies that the input values to the conversion are unmodified.
        /// </summary>
        RgbIdentity = 0,
        /// <summary>
        /// Specifies no model conversion but the inputs are range expanded as for Y'C~B~C~R~.
        /// </summary>
        YcbcrIdentity = 1,
        /// <summary>
        /// Specifies the color model conversion from Y'C~B~C~R~ to R'G'B' defined in BT.709 and
        /// described in the "`BT.709 Y’C~B~C~R~ conversion`" section of the Khronos Data Format Specification.
        /// </summary>
        Ycbcr709 = 2,
        /// <summary>
        /// Specifies the color model conversion from Y'C~B~C~R~ to R'G'B' defined in BT.601 and
        /// described in the "`BT.601 Y’C~B~C~R~ conversion`" section of the Khronos Data Format Specification.
        /// </summary>
        Ycbcr601 = 3,
        /// <summary>
        /// Specifies the color model conversion from Y'C~B~C~R~ to R'G'B' defined in BT.2020 and
        /// described in the "`BT.2020 Y’C~B~C~R~ conversion`" section of the Khronos Data Format Specification.
        /// </summary>
        Ycbcr2020 = 4
    }

    /// <summary>
    /// Range of encoded values in a color space.
    /// </summary>
    public enum SamplerYcbcrRangeKhr
    {
        /// <summary>
        /// Indicates that the full range of the encoded values are valid and interpreted according
        /// to the ITU "`full range`" quantization rules.
        /// </summary>
        ItuFull = 0,
        /// <summary>
        /// Indicates that headroom and foot room are reserved in the numerical range of encoded
        /// values, and the remaining values are expanded according to the ITU "`narrow range`"
        /// quantization rules.
        /// </summary>
        ItuNarrow = 1
    }

    /// <summary>
    /// Position of downsampled chroma samples.
    /// </summary>
    public enum ChromaLocationKhr
    {
        /// <summary>
        /// Indicates that downsampled chroma samples are aligned with luma samples with even coordinates.
        /// </summary>
        CositedEven = 0,
        /// <summary>
        /// Indicates that downsampled chroma samples are located half way between each even luma
        /// sample and the nearest higher odd luma sample.
        /// </summary>
        Midpoint = 1
    }

    /// <summary>
    /// Structure specifying Y'CbCr conversion to a sampler or image view.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SamplerYcbcrConversionInfoKhr
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A <see cref="SamplerYcbcrConversionKhr"/> handle created with <see cref="DeviceExtensions.CreateSamplerYcbcrConversionKhr"/>.
        /// </summary>
        public long Conversion;
    }
}
