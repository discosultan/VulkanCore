using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace VulkanCore
{
    /// <summary>
    /// Structure specifying a two-dimensional offset.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay(@"{X:{X} Y:{Y}}")]
    public struct Offset2D : IEquatable<Offset2D>
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
		/// A <see cref="Offset2D"/> with (0,0) coordinates.
		/// </summary>
		public static readonly Offset2D Zero = new Offset2D(0, 0);

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
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ref Offset2D other)
        {
            return other.X == X && other.Y == Y;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Offset2D other) => Equals(ref other);

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (!(obj is Offset2D))
                return false;

            var strongValue = (Offset2D)obj;
            return Equals(ref strongValue);
        }

        /// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Offset2D left, Offset2D right) => left.Equals(ref right);


        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Offset2D left, Offset2D right) => !left.Equals(ref right);

        /// <summary>
		/// /Gets the hash code for this object.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }

    /// <summary>
    /// Structure specifying a three-dimensional offset.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Offset3D : IEquatable<Offset3D>
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
        /// Gets an <see cref="Offset3D"/> with all of its components set to zero.
        /// </summary>
        public static readonly Offset3D Zero = new Offset3D(0, 0, 0);

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
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ref Offset3D other)
        {
            return other.X == X && other.Y == Y && Z == other.Z;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Offset3D other) => Equals(ref other);

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (!(obj is Offset3D))
                return false;

            var strongValue = (Offset3D)obj;
            return Equals(ref strongValue);
        }

        /// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Offset3D left, Offset3D right) => left.Equals(ref right);


        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Offset3D left, Offset3D right) => !left.Equals(ref right);

        /// <summary>
		/// /Gets the hash code for this object.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }
    }
}