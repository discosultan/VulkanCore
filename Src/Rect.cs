using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Structure specifying a two-dimensional subregion.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect2D
    {
        /// <summary>
        /// The offset component of the rectangle.
        /// </summary>
        public Offset2D Offset;
        /// <summary>
        /// The extent component of the rectangle.
        /// </summary>
        public Extent2D Extent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect2D"/> structure.
        /// </summary>
        /// <param name="offset">The offset component of the rectangle.</param>
        /// <param name="extent">The extent component of the rectangle.</param>
        public Rect2D(Offset2D offset, Extent2D extent)
        {
            Offset = offset;
            Extent = extent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect2D"/> structure.
        /// </summary>
        /// <param name="x">The X component of the offset.</param>
        /// <param name="y">The Y component of the offset.</param>
        /// <param name="width">The width component of the extent.</param>
        /// <param name="height">The height component of the extent.</param>
        public Rect2D(int x, int y, int width, int height)
            : this(new Offset2D(x, y), new Extent2D(width, height))
        {
        }
    }

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
        /// <param name="offset">The offset component of the rectangle.</param>
        /// <param name="extent">The extent component of the rectangle.</param>
        public Rect3D(Offset3D offset, Extent3D extent)
        {
            Offset = offset;
            Extent = extent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect2D"/> structure.
        /// </summary>
        /// <param name="x">The X component of the offset.</param>
        /// <param name="y">The Y component of the offset.</param>
        /// <param name="z">The Z component of the offset.</param>
        /// <param name="width">The width component of the extent.</param>
        /// <param name="height">The height component of the extent.</param>
        /// <param name="depth">The depth component of the extent.</param>
        public Rect3D(int x, int y, int z, int width, int height, int depth)
            : this(new Offset3D(x, y, z), new Extent3D(width, height, depth))
        {
        }
    }

    /// <summary>
    /// Structure specifying a clear rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ClearRect
    {
        /// <summary>
        /// The two-dimensional region to be cleared.
        /// </summary>
        public Rect2D Rect;
        /// <summary>
        /// The first layer to be cleared.
        /// </summary>
        public int BaseArrayLayer;
        /// <summary>
        /// The number of layers to clear.
        /// </summary>
        public int LayerCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearRect"/> structure.
        /// </summary>
        /// <param name="rect">The two-dimensional region to be cleared.</param>
        /// <param name="baseArrayLayer">The first layer to be cleared.</param>
        /// <param name="layerCount">The number of layers to clear.</param>
        public ClearRect(Rect2D rect, int baseArrayLayer, int layerCount)
        {
            Rect = rect;
            BaseArrayLayer = baseArrayLayer;
            LayerCount = layerCount;
        }
    }
}
