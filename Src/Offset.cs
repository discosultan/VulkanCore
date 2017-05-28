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
    public struct Offset2D : IEquatable<Offset2D>
    {
        /// <summary>
        /// An <see cref="Offset2D"/> with all of its components set to zero.
        /// </summary>
        public static readonly Offset2D Zero = new Offset2D(0, 0);

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
        /// <param name="x">The X component of the offset</param>
        /// <param name="y">The Y component of the offset.</param>
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
        /// Determines whether the specified <see cref="Offset2D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Offset2D"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Offset2D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ref Offset2D other) => other.X == X && other.Y == Y;

        /// <summary>
        /// Determines whether the specified <see cref="Offset2D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Offset2D"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Offset2D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Offset2D other) => Equals(ref other);

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => obj is Offset2D && Equals((Offset2D)obj);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given offsets are equal.
        /// </summary>
        /// <param name="left">The first offset to compare.</param>
        /// <param name="right">The second offset to compare.</param>
        /// <returns><c>true</c> if the offsets are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Offset2D left, Offset2D right) => left.Equals(ref right);

        /// <summary>
        /// Returns a boolean indicating whether the two given offsets are not equal.
        /// </summary>
        /// <param name="left">The first offset to compare.</param>
        /// <param name="right">The second offset to compare.</param>
        /// <returns>
        /// <c>true</c> if the offsets are not equal; <c>false</c> if they are equal.
        /// </returns>
        public static bool operator !=(Offset2D left, Offset2D right) => !left.Equals(ref right);
    }

    /// <summary>
    /// Structure specifying a three-dimensional offset.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Offset3D : IEquatable<Offset3D>
    {
        /// <summary>
        /// An <see cref="Offset3D"/> with all of its components set to zero.
        /// </summary>
        public static readonly Offset3D Zero = new Offset3D(0, 0, 0);

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
        /// <param name="x">The X component of the offset</param>
        /// <param name="y">The Y component of the offset.</param>
        /// <param name="z">The Z component of the offset.</param>
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
        /// Determines whether the specified <see cref="Offset3D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Offset3D"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Offset3D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ref Offset3D other) => other.X == X && other.Y == Y && other.Z == Z;

        /// <summary>
        /// Determines whether the specified <see cref="Offset3D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Offset3D"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Offset3D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Offset3D other) => Equals(ref other);

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => obj is Offset3D && Equals((Offset3D)obj);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given offsets are equal.
        /// </summary>
        /// <param name="left">The first offset to compare.</param>
        /// <param name="right">The second offset to compare.</param>
        /// <returns><c>true</c> if the offsets are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Offset3D left, Offset3D right) => left.Equals(ref right);

        /// <summary>
        /// Returns a boolean indicating whether the two given offsets are not equal.
        /// </summary>
        /// <param name="left">The first offset to compare.</param>
        /// <param name="right">The second offset to compare.</param>
        /// <returns>
        /// <c>true</c> if the offsets are not equal; <c>false</c> if they are equal.
        /// </returns>
        public static bool operator !=(Offset3D left, Offset3D right) => !left.Equals(ref right);
    }
}
