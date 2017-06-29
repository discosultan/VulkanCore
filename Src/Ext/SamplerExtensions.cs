using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Ext
{
    /// <summary>
    /// Specify reduction mode for texture filtering.
    /// </summary>
    public enum SamplerReductionModeExt
    {
        /// <summary>
        /// Indicates that texel values are combined by computing a weighted average of values in the
        /// footprint, using weights.
        /// </summary>
        WeightedAverage = 0,
        /// <summary>
        /// Indicates that texel values are combined by taking the component-wise minimum of values
        /// in the footprint with non-zero weights.
        /// </summary>
        Min = 1,
        /// <summary>
        /// Indicates that texel values are combined by taking the component-wise maximum of values
        /// in the footprint with non-zero weights.
        /// </summary>
        Max = 2
    }

    /// <summary>
    /// Structure describing sampler filter minmax limits that can be supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceSamplerFilterMinmaxPropertiesExt
    {
        public StructureType Type;
        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        public Bool FilterMinmaxSingleComponentFormats;
        public Bool FilterMinmaxImageComponentMapping;
    }

    /// <summary>
    /// Structure specifying sampler reduction mode.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SamplerReductionModeCreateInfoExt
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
        /// Controls how texture filtering combines texel values.
        /// </summary>
        public SamplerReductionModeExt ReductionMode;
    }
}
