using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace VulkanCore
{
    /// <summary>
    /// Structure specifying a two-dimensional subregion.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect2D : IEquatable<Rect2D>
    {
        /// <summary>
        /// A <see cref="Rect2D"/> with all of its components set to zero.
        /// </summary>
        public static readonly Rect2D Zero = new Rect2D(Offset2D.Zero, Extent2D.Zero);

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

        /// <summary>
        /// Returns a string representing this <see cref="Rect2D"/> instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns a string representing this <see cref="Rect2D"/> instance, using the specified
        /// format to format individual elements and the given <paramref name="formatProvider"/>.
        /// </summary>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(Offset.ToString());
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(Extent.ToString());
            sb.Append('>');
            return sb.ToString();
        }

        /// <summary>
        /// Determines whether the specified <see cref="Rect2D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Rect2D"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Rect2D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ref Rect2D other) => other.Offset.Equals(ref Offset) && other.Extent.Equals(ref Extent);

        /// <summary>
        /// Determines whether the specified <see cref="Rect2D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Rect2D"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Rect2D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Rect2D other) => Equals(ref other);

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => obj is Rect2D && Equals((Rect2D)obj);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Extent.GetHashCode();
                hashCode = (hashCode * 397) ^ Offset.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given rectangles are equal.
        /// </summary>
        /// <param name="left">The first rectangle to compare.</param>
        /// <param name="right">The second rectangle to compare.</param>
        /// <returns><c>true</c> if the rectangles are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Rect2D left, Rect2D right) => left.Equals(right);

        /// <summary>
        /// Returns a boolean indicating whether the two given rectangles are not equal.
        /// </summary>
        /// <param name="left">The first rectangle to compare.</param>
        /// <param name="right">The second rectangle to compare.</param>
        /// <returns>
        /// <c>true</c> if the rectangles are not equal; <c>false</c> if they are equal.
        /// </returns>
        public static bool operator !=(Rect2D left, Rect2D right) => !left.Equals(right);
    }
}
