using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VulkanCore
{
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
