using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace VulkanCore
{
    /// <summary>
    /// Structure specifying a two-dimensional extent. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Extent2D
    {
        /// <summary>
        /// The width component of the extent.
        /// </summary>
        public int Width;
        /// <summary>
        /// The height component of the extent.
        /// </summary>
        public int Height;

        /// <summary>
        /// Initializes a new instance of <see cref="Extent2D"/> structure.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Extent2D(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Returns a string representing this <see cref="Extent2D"/> instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns a string representing this <see cref="Extent2D"/> instance, using the specified
        /// format to format individual elements and the given <paramref name="formatProvider"/>.
        /// </summary>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(Width.ToString(formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(Height.ToString(formatProvider));
            sb.Append('>');
            return sb.ToString();
        }

        /// <summary>
        /// Gets a special valued <see cref="Extent2D"/>.
        /// </summary>
        public static Extent2D WholeSize => new Extent2D(~0, ~0);

        /// <summary>
        /// Gets an <see cref="Extent2D"/> with all of its components set to zero.
        /// </summary>
        public static Extent2D Zero => new Extent2D(0, 0);
    }

    /// <summary>
    /// Structure specifying a three-dimensional extent. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Extent3D
    {
        /// <summary>
        /// The width component of the extent.
        /// </summary>
        public int Width;
        /// <summary>
        /// The height component of the extent.
        /// </summary>
        public int Height;
        /// <summary>
        /// The depth component of the extent.
        /// </summary>
        public int Depth;

        /// <summary>
        /// Initializes a new instance of <see cref="Extent3D"/> structure.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        public Extent3D(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        /// <summary>
        /// Returns a string representing this <see cref="Extent3D"/> instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns a string representing this <see cref="Extent3D"/> instance, using the specified
        /// format to format individual elements and the given <paramref name="formatProvider"/>.
        /// </summary>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(Width.ToString(formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(Height.ToString(formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(Depth.ToString(formatProvider));
            sb.Append('>');
            return sb.ToString();
        }

        /// <summary>
        /// Gets a special valued <see cref="Extent3D"/>.
        /// </summary>
        public static Extent3D WholeSize => new Extent3D(~0, ~0, ~0);

        /// <summary>
        /// Gets an <see cref="Extent3D"/> with all of its components set to zero.
        /// </summary>
        public static Extent3D Zero => new Extent3D(0, 0, 0);
    }
}
