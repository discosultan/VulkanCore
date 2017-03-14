using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace VulkanCore
{
    /// <summary>
    /// Structure specifying a two-dimensional offset.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Offset2D
    {
        /// <summary>
        /// The X component of the offset.
        /// </summary>
        public int X;
        /// <summary>
        /// The Y component of the offset.
        /// </summary>
        public int Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Offset2D"/> structure.
        /// </summary>
        public Offset2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns a string representing this <see cref="Offset2D"/> instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns a string representing this <see cref="Offset2D"/> instance, using the specified
        /// format to format individual elements and the given <paramref name="formatProvider"/>.
        /// </summary>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(X);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(Y);
            sb.Append('>');
            return sb.ToString();
        }

        /// <summary>
        /// Gets an <see cref="Offset2D"/> with all of its components set to zero.
        /// </summary>
        public static Offset2D Zero => new Offset2D(0, 0);
    }

    /// <summary>
    /// Structure specifying a three-dimensional offset.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Offset3D
    {
        /// <summary>
        /// The X component of the offset.
        /// </summary>
        public int X;
        /// <summary>
        /// The Y component of the offset.
        /// </summary>
        public int Y;
        /// <summary>
        /// The Z component of the offset.
        /// </summary>
        public int Z;

        /// <summary>
        /// Initializes a new instance of the <see cref="Offset3D"/> structure.
        /// </summary>
        public Offset3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Returns a string representing this <see cref="Offset3D"/> instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns a string representing this <see cref="Offset3D"/> instance, using the specified
        /// format to format individual elements and the given <paramref name="formatProvider"/>.
        /// </summary>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(X);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(Y);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(Z);
            sb.Append('>');
            return sb.ToString();
        }

        /// <summary>
        /// Gets an <see cref="Offset3D"/> with all of its components set to zero.
        /// </summary>
        public static Offset3D Zero => new Offset3D(0, 0, 0);
    }
}