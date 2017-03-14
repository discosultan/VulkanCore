using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Structure specifying a three-dimensional subregion.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect3D
    {
        /// <summary>
        /// The offset component of the cuboid.
        /// </summary>
        public Offset3D Offset;
        /// <summary>
        /// The extent component of the cuboid.
        /// </summary>
        public Extent3D Extent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect3D"/> structure.
        /// </summary>
        public Rect3D(Offset3D offset, Extent3D extent)
        {
            Offset = offset;
            Extent = extent;
        }
    }
}
