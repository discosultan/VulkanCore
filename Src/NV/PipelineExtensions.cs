using System;
using System.Runtime.InteropServices;

namespace VulkanCore.NV
{
    /// <summary>
    /// Structure specifying swizzle applied to primitive clip coordinates.
    /// <para>
    /// Each primitive sent to a given viewport has a swizzle and optional negation applied to its
    /// clip coordinates.
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineViewportSwizzleStateCreateInfoNV
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
        public PipelineViewportSwizzleStateCreateFlagsNV Flags;
        /// <summary>
        /// The number of viewport swizzles used by the pipeline.
        /// </summary>
        public int ViewportCount;
        /// <summary>
        /// A pointer to an array of <see cref="ViewportSwizzleNV"/> structures, defining the
        /// viewport swizzles.
        /// </summary>
        public IntPtr ViewportSwizzles;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineViewportSwizzleStateCreateInfoNV"/> structure.
        /// </summary>
        /// <param name="viewportCount">The number of viewport swizzles used by the pipeline.</param>
        /// <param name="viewportSwizzles">
        /// A pointer to an array of <see cref="ViewportSwizzleNV"/> structures, defining the
        /// viewport swizzles.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public PipelineViewportSwizzleStateCreateInfoNV(
            int viewportCount, IntPtr viewportSwizzles, IntPtr next = default(IntPtr))
        {
            Type = StructureType.PipelineViewportSwizzleStateCreateInfoNV;
            Next = next;
            Flags = PipelineViewportSwizzleStateCreateFlagsNV.None;
            ViewportCount = viewportCount;
            ViewportSwizzles = viewportSwizzles;
        }
    }

    [Flags]
    public enum PipelineViewportSwizzleStateCreateFlagsNV
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
    }

    /// <summary>
    /// Structure specifying a viewport swizzle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ViewportSwizzleNV
    {
        /// <summary>
        /// Specifies the swizzle operation to apply to the x component of the primitive.
        /// </summary>
        public ViewportCoordinateSwizzleNV X;
        /// <summary>
        /// Specifies the swizzle operation to apply to the y component of the primitive.
        /// </summary>
        public ViewportCoordinateSwizzleNV Y;
        /// <summary>
        /// Specifies the swizzle operation to apply to the z component of the primitive.
        /// </summary>
        public ViewportCoordinateSwizzleNV Z;
        /// <summary>
        /// Specifies the swizzle operation to apply to the w component of the primitive.
        /// </summary>
        public ViewportCoordinateSwizzleNV W;
    }

    /// <summary>
    /// Specify how a viewport coordinate is swizzled.
    /// </summary>
    public enum ViewportCoordinateSwizzleNV
    {
        PositiveX = 0,
        NegativeX = 1,
        PositiveY = 2,
        NegativeY = 3,
        PositiveZ = 4,
        NegativeZ = 5,
        PositiveW = 6,
        NegativeW = 7
    }
}
