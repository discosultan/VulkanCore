using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace VulkanCore
{
    /// <summary>
    /// Structure specifying color value when the format of the image or attachment is one of the
    /// formats other than signed integer or unsigned integer. Floating point values are
    /// automatically converted to the format of the image.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorF4
    {
        /// <summary>
        /// Gets a <see cref="ColorF4"/> with all of its components set to zero.
        /// </summary>
        public static ColorF4 Zero => new ColorF4();

        /// <summary>
        /// The red component of the color.
        /// </summary>
        public float R;
        /// <summary>
        /// The green component of the color.
        /// </summary>
        public float G;
        /// <summary>
        /// The blue component of the color.
        /// </summary>
        public float B;
        /// <summary>
        /// The alpha component of the color.
        /// </summary>
        public float A;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorF4"/> structure.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        public ColorF4(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Returns a string representing this <see cref="ColorF4"/> instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns a string representing this <see cref="ColorF4"/> instance, using the specified
        /// format to format individual elements and the given <paramref name="formatProvider"/>.
        /// </summary>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(R.ToString(formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(G.ToString(formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(B.ToString(formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(A.ToString(formatProvider));
            sb.Append('>');
            return sb.ToString();
        }
    }

    /// <summary>
    /// Structure specifying color value when the format of the image or attachment is signed
    /// integer. Signed integer values are converted to the format of the image by casting to the
    /// smaller type (with negative 32-bit values mapping to negative values in the smaller type). If
    /// the integer value is not representable in the target type (e.g. would overflow in conversion
    /// to that type), the value is undefined.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorI4
    {
        /// <summary>
        /// Gets a <see cref="ColorI4"/> with all of its components set to zero.
        /// </summary>
        public static ColorI4 Zero => new ColorI4();

        /// <summary>
        /// The red component of the color.
        /// </summary>
        public int R;
        /// <summary>
        /// The green component of the color.
        /// </summary>
        public int G;
        /// <summary>
        /// The blue component of the color.
        /// </summary>
        public int B;
        /// <summary>
        /// The alpha component of the color.
        /// </summary>
        public int A;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorI4"/> structure.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        public ColorI4(int r, int g, int b, int a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Returns a string representing this <see cref="ColorI4"/> instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns a string representing this <see cref="ColorI4"/> instance, using the specified
        /// format to format individual elements and the given <paramref name="formatProvider"/>.
        /// </summary>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(R);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(G);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(B);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(A);
            sb.Append('>');
            return sb.ToString();
        }
    }

    /// <summary>
    /// Structure specifying color value when the format of the image or attachment is unsigned
    /// integer. Unsigned integer values are converted to the format of the image by casting to the
    /// integer type with fewer bits.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorU4
    {
        /// <summary>
        /// Gets a <see cref="ColorU4"/> with all of its components set to zero.
        /// </summary>
        public static ColorU4 Zero => new ColorU4();

        /// <summary>
        /// The red component of the color.
        /// </summary>
        public uint R;
        /// <summary>
        /// The green component of the color.
        /// </summary>
        public uint G;
        /// <summary>
        /// The blue component of the color.
        /// </summary>
        public uint B;
        /// <summary>
        /// The alpha component of the color.
        /// </summary>
        public uint A;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorU4"/> structure.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        public ColorU4(uint r, uint g, uint b, uint a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Returns a string representing this <see cref="ColorU4"/> instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns a string representing this <see cref="ColorU4"/> instance, using the specified
        /// format to format individual elements and the given <paramref name="formatProvider"/>.
        /// </summary>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(R);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(G);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(B);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(A);
            sb.Append('>');
            return sb.ToString();
        }
    }
}
