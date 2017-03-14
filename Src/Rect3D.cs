using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Structure specifying a three-dimensional subregion.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect3D : IEquatable<Rect3D>
    {
        /// <summary>
        /// The offset component of the cuboid.
        /// </summary>
        public Offset3D Offset;

        /// <summary>
        /// The extent component of the cuboid.
        /// </summary>
        public Extent3D Extent;

        /// <summary>
        /// An empty Rect3D.
        /// </summary>
        public static readonly Rect3D Empty = default(Rect3D);

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect3D"/> structure.
        /// </summary>
        public Rect3D(Offset3D offset, Extent3D extent)
        {
            Offset = offset;
            Extent = extent;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ref Rect3D other)
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
        public bool Equals(Rect3D other) => Equals(ref other);

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (!(obj is Rect3D))
                return false;

            var strongValue = (Rect3D)obj;
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
        public static bool operator ==(Rect3D left, Rect3D right) => left.Equals(ref right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Rect3D left, Rect3D right) => !left.Equals(ref right);

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
