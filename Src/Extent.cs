using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace VulkanCore
{
    /// <summary>
    /// Structure specifying a two-dimensional extent. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay(@"{Width:{Width} Height:{Height}}")]
    public struct Extent2D : IEquatable<Extent2D>
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
        /// Gets a special valued <see cref="Extent2D"/>.
        /// </summary>
        public static readonly Extent2D WholeSize = new Extent2D(~0, ~0);

        /// <summary>
        /// Gets an <see cref="Extent2D"/> with all of its components set to zero.
        /// </summary>
        public static readonly Extent2D Zero = new Extent2D(0, 0);

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
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ref Extent2D other)
        {
            return other.Width == Width && other.Height == Height;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Extent2D other) => Equals(ref other);

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (!(obj is Extent2D))
                return false;

            var strongValue = (Extent2D)obj;
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
        public static bool operator ==(Extent2D left, Extent2D right) => left.Equals(ref right);


        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Extent2D left, Extent2D right) => !left.Equals(ref right);

        /// <summary>
		/// /Gets the hash code for this object.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
            }
        }
    }

    /// <summary>
    /// Structure specifying a three-dimensional extent. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay(@"{Width:{Width} Height:{Height} Depth:{Depth}}")]
    public struct Extent3D : IEquatable<Extent3D>
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
        /// Gets a special valued <see cref="Extent3D"/>.
        /// </summary>
        public static readonly Extent3D WholeSize = new Extent3D(~0, ~0, ~0);

        /// <summary>
        /// Gets an <see cref="Extent3D"/> with all of its components set to zero.
        /// </summary>
        public static readonly Extent3D Zero = new Extent3D(0, 0, 0);

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
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ref Extent3D other)
        {
            return other.Width == Width && other.Height == Height && other.Depth == Depth;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Extent3D other) => Equals(ref other);

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (!(obj is Extent3D))
                return false;

            var strongValue = (Extent3D)obj;
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
        public static bool operator ==(Extent3D left, Extent3D right) => left.Equals(ref right);


        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Extent3D left, Extent3D right) => !left.Equals(ref right);

        /// <summary>
		/// /Gets the hash code for this object.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                hashCode = (hashCode * 397) ^ Depth.GetHashCode();
                return hashCode;
            }
        }
    }
}
