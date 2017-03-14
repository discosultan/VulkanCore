using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Structure specifying a two-dimensional subregion.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect2D : IEquatable<Rect2D>
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
		/// An empty Rect2D.
		/// </summary>
		public static readonly Rect2D Empty = default(Rect2D);

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect2D"/> structure.
        /// </summary>
        public Rect2D(Offset2D offset, Extent2D extent)
        {
            Offset = offset;
            Extent = extent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect2D"/> structure.
        /// </summary>
        public Rect2D(int x, int y, int width, int height)
        {
            Offset = new Offset2D(x, y);
            Extent = new Extent2D(width, height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect2D"/> structure.
        /// </summary>
        public Rect2D(int width, int height)
        {
            Offset = default(Offset2D);
            Extent = new Extent2D(width, height);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ref Rect2D other)
        {
            return Offset.Equals(ref other.Offset) && Extent.Equals(ref other.Extent);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Rect2D other) => Equals(ref other);

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (!(obj is Rect2D))
                return false;

            var strongValue = (Rect2D)obj;
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
        public static bool operator ==(Rect2D left, Rect2D right) => left.Equals(ref right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Rect2D left, Rect2D right) => !left.Equals(ref right);

        /// <summary>
		/// /Gets the hash code for this object.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Offset.GetHashCode();
                hashCode = (hashCode * 397) ^ Extent.GetHashCode();
                return hashCode;
            }
        }
    }
}
