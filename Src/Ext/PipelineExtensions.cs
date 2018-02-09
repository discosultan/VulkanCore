using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Ext
{
    /// <summary>
    /// Structure specifying discard rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineDiscardRectangleStateCreateInfoExt
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
        /// Is reserved for future use.
        /// </summary>
        public PipelineDiscardRectangleStateCreateFlagsExt Flags;
        /// <summary>
        /// The mode used to determine whether fragments that lie within the discard
        /// rectangle are discarded or not.
        /// </summary>
        public DiscardRectangleModeExt DiscardRectangleMode;
        /// <summary>
        /// The number of discard rectangles used by the pipeline.
        /// </summary>
        public int DiscardRectangleCount;
        /// <summary>
        /// A pointer to an array of <see cref="Rect2D"/> structures, defining the discard
        /// rectangles.
        /// <para>If the discard rectangle state is dynamic, this member is ignored.</para>
        /// </summary>
        public IntPtr DiscardRectangles;

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="PipelineDiscardRectangleStateCreateInfoExt"/> structure.
        /// </summary>
        /// <param name="discardRectangleMode">
        /// The mode used to determine whether fragments that lie within the discard rectangle are
        /// discarded or not.
        /// </param>
        /// <param name="discardRectangleCount">The number of discard rectangles used by the pipeline.</param>
        /// <param name="discardRectangles">
        /// A pointer to an array of <see cref="Rect2D"/> structures, defining the discard rectangles.
        /// <para>If the discard rectangle state is dynamic, this member is ignored.</para>
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public PipelineDiscardRectangleStateCreateInfoExt(DiscardRectangleModeExt discardRectangleMode,
            int discardRectangleCount, IntPtr discardRectangles, IntPtr next = default(IntPtr))
        {
            Type = StructureType.PipelineDiscardRectangleStateCreateInfoExt;
            Next = next;
            Flags = PipelineDiscardRectangleStateCreateFlagsExt.None;
            DiscardRectangleMode = discardRectangleMode;
            DiscardRectangleCount = discardRectangleCount;
            DiscardRectangles = discardRectangles;
        }
    }

    [Flags]
    public enum PipelineDiscardRectangleStateCreateFlagsExt
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0
    }

    /// <summary>
    /// Specify the discard rectangle mode.
    /// </summary>
    public enum DiscardRectangleModeExt
    {
        /// <summary>
        /// Specifies that a fragment within any discard rectangle satisfies the test.
        /// </summary>
        Inclusive = 0,
        /// <summary>
        /// Specifies that a fragment not within any of the discard rectangles satisfies
        /// the test.
        /// </summary>
        Exclusive = 1
    }

    /// <summary>
    /// Enumerant specifying the blend overlap parameter.
    /// </summary>
    public enum BlendOverlapExt
    {
        /// <summary>
        /// Specifies that there is no correlation between the source and destination
        /// coverage.
        /// </summary>
        Uncorrelated = 0,
        /// <summary>
        /// Specifies that the source and destination coverage are considered to have
        /// minimal overlap.
        /// </summary>
        Disjoint = 1,
        /// <summary>
        /// Specifies that the source and destination coverage are considered to have
        /// maximal overlap.
        /// </summary>
        Conjoint = 2
    }

    /// <summary>
    /// Structure specifying parameters that affect advanced blend operations.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineColorBlendAdvancedStateCreateInfoExt
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
        /// Specifies whether the source color of the blend operation is treated as premultiplied.
        /// </summary>
        public Bool SrcPremultiplied;
        /// <summary>
        /// Specifies whether the destination color of the blend operation is treated as premultiplied.
        /// </summary>
        public Bool DstPremultiplied;
        /// <summary>
        /// Specifies how the source and destination sample's coverage is correlated.
        /// </summary>
        public BlendOverlapExt BlendOverlap;
    }

    /// <summary>
    /// Structure specifying parameters controlling coverage modulation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineCoverageModulationStateCreateInfoNV
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
        /// Is reserved for future use.
        /// </summary>
        public PipelineCoverageModulationStateCreateFlagsNV Flags;
        /// <summary>
        /// Controls which color components are modulated and is of type <see cref="CoverageModulationModeNV"/>.
        /// </summary>
        public CoverageModulationModeNV CoverageModulationMode;
        /// <summary>
        /// Controls whether the modulation factor is looked up from a table in <see cref="CoverageModulationTable"/>.
        /// </summary>
        public Bool CoverageModulationTableEnable;
        /// <summary>
        /// The number of elements in <see cref="CoverageModulationTable"/>.
        /// </summary>
        public int CoverageModulationTableCount;
        /// <summary>
        /// A pointer to a table of modulation factors containing a value for each number of covered samples.
        /// </summary>
        public IntPtr CoverageModulationTable;
    }

    [Flags]
    public enum PipelineCoverageModulationStateCreateFlagsNV
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0
    }

    /// <summary>
    /// Specify the discard rectangle mode.
    /// </summary>
    public enum CoverageModulationModeNV
    {
        /// <summary>
        /// Specifies that no components are multiplied by the modulation factor.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that the red, green, and blue components are multiplied by the modulation factor.
        /// </summary>
        Rgb = 1,
        /// <summary>
        /// Specifies that the alpha component is multiplied by the modulation factor.
        /// </summary>
        Alpha = 2,
        /// <summary>
        /// Specifies that all components are multiplied by the modulation factor.
        /// </summary>
        Rgba = 3
    }

    /// <summary>
    /// Structure specifying sample locations for a pipeline.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineSampleLocationsStateCreateInfoExt
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
        /// Controls whether custom sample locations are used. If <c>false</c>, the default sample
        /// locations are used and the values specified in <see cref="SampleLocationsInfo"/> are ignored.
        /// </summary>
        public Bool SampleLocationsEnable;
        /// <summary>
        /// The sample locations to use during rasterization if <see cref="SampleLocationsEnable"/>
        /// is <c>true</c> and the graphics pipeline isn't created with <see cref="DynamicState.SampleLocationsExt"/>.
        /// </summary>
        public SampleLocationsInfoExt SampleLocationsInfo;
    }

    /// <summary>
    /// Specify the conservative rasterization mode.
    /// </summary>
    public enum ConservativeRasterizationModeExt
    {
        /// <summary>
        /// Specifies that conservative rasterization is disabled and rasterization proceeds as normal.
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// Specifies that conservative rasterization is enabled in overestimation mode.
        /// </summary>
        Overestimate = 1,
        /// <summary>
        /// Specifies that conservative rasterization is enabled in underestimation mode.
        /// </summary>
        Underestimate = 2
    }

    /// <summary>
    /// Structure specifying conservative raster state.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineRasterizationConservativeStateCreateInfoExt
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
        /// Reserved for future use.
        /// </summary>
        public PipelineRasterizationConservativeStateCreateFlagsExt Flags;
        /// <summary>
        /// The conservative rasterization mode to use.
        /// </summary>
        public ConservativeRasterizationModeExt ConservativeRasterizationMode;
        /// <summary>
        /// The extra size in pixels to increase the generating primitive during conservative
        /// rasterization at each of its edges in `X` and `Y` equally in screen space beyond the base
        /// overestimation specified in <see cref="PhysicalDeviceConservativeRasterizationPropertiesExt.PrimitiveOverestimationSize"/>.
        /// </summary>
        public float ExtraPrimitiveOverestimationSize;
    }

    [Flags]
    public enum PipelineRasterizationConservativeStateCreateFlagsExt
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0
    }
}
