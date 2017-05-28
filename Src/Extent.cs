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
    public struct Extent2D : IEquatable<Extent2D>
    {
        /// <summary>
        /// A special valued <see cref="Extent3D"/>.
        /// </summary>
        public static readonly Extent2D WholeSize = new Extent2D(~0, ~0);

        /// <summary>
        /// An <see cref="Extent3D"/> with all of its components set to zero.
        /// </summary>
        public static readonly Extent2D Zero = new Extent2D(0, 0);

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
        /// <param name="width">The width component of the extent.</param>
        /// <param name="height">The height component of the extent.</param>
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
        /// Determines whether the specified <see cref="Extent2D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Extent2D"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Extent2D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ref Extent2D other) => other.Width == Width && other.Height == Height;

        /// <summary>
        /// Determines whether the specified <see cref="Extent2D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Extent2D"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Extent2D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Extent2D other) => Equals(ref other);

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => obj is Extent2D && Equals((Extent2D)obj);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given extents are equal.
        /// </summary>
        /// <param name="left">The first extent to compare.</param>
        /// <param name="right">The second extent to compare.</param>
        /// <returns><c>true</c> if the extents are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Extent2D left, Extent2D right) => left.Equals(ref right);

        /// <summary>
        /// Returns a boolean indicating whether the two given extents are not equal.
        /// </summary>
        /// <param name="left">The first extent to compare.</param>
        /// <param name="right">The second extent to compare.</param>
        /// <returns>
        /// <c>true</c> if the extents are not equal; <c>false</c> if they are equal.
        /// </returns>
        public static bool operator !=(Extent2D left, Extent2D right) => !left.Equals(ref right);
    }

    /// <summary>
    /// Structure specifying a three-dimensional extent.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Extent3D : IEquatable<Extent3D>
    {
        /// <summary>
        /// A special valued <see cref="Extent3D"/>.
        /// </summary>
        public static readonly Extent3D WholeSize = new Extent3D(~0, ~0, ~0);

        /// <summary>
        /// An <see cref="Extent3D"/> with all of its components set to zero.
        /// </summary>
        public static readonly Extent3D Zero = new Extent3D(0, 0, 0);

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
        /// <param name="width">The width component of the extent.</param>
        /// <param name="height">The height component of the extent.</param>
        /// <param name="depth">The depth component of the extent.</param>
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
        /// Determines whether the specified <see cref="Extent3D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Extent3D"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Extent3D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ref Extent3D other) => other.Width == Width && other.Height == Height && other.Depth == Depth;

        /// <summary>
        /// Determines whether the specified <see cref="Extent3D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Extent3D"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Extent3D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Extent3D other) => Equals(ref other);

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => obj is Extent3D && Equals((Extent3D)obj);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given extents are equal.
        /// </summary>
        /// <param name="left">The first extent to compare.</param>
        /// <param name="right">The second extent to compare.</param>
        /// <returns><c>true</c> if the extents are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Extent3D left, Extent3D right) => left.Equals(ref right);

        /// <summary>
        /// Returns a boolean indicating whether the two given extents are not equal.
        /// </summary>
        /// <param name="left">The first extent to compare.</param>
        /// <param name="right">The second extent to compare.</param>
        /// <returns>
        /// <c>true</c> if the extents are not equal; <c>false</c> if they are equal.
        /// </returns>
        public static bool operator !=(Extent3D left, Extent3D right) => !left.Equals(ref right);
    }
}
