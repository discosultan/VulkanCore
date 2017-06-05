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
}
