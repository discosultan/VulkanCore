using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// A platform-specific type that is used to represent a size (in bytes) of an object in memory.
    /// <para>Equivalent to C/C++ size_t type.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Size : IEquatable<Size>, IComparable<Size>
    {
        // We use IntPtr instead of UIntPtr to stay CLS compliant.
        // https://msdn.microsoft.com/en-us/library/system.intptr(v=vs.110).aspx
        private IntPtr _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> structure.
        /// </summary>
        /// <param name="size">The size of an object in bytes.</param>
        public Size(int size)
        {
            _value = new IntPtr(size);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> structure.
        /// </summary>
        /// <param name="size">The size of an object in bytes.</param>
        public Size(long size)
        {
            _value = new IntPtr(size);
        }

        /// <summary>
        /// Converts the numeric value of the current <see cref="Size"/> object to its equivalent
        /// string representation.
        /// </summary>
        /// <returns>The string representation of the value of this instance.</returns>
        public override string ToString() => _value.ToString();

        /// <summary>
        /// Returns a boolean indicating whether the given <see cref="Size"/> is equal to this <see
        /// cref="Size"/> instance.
        /// </summary>
        /// <param name="other">The <see cref="Size"/> to compare this instance to.</param>
        /// <returns>
        /// <c>true</c> if the other <see cref="Size"/> is equal to this instance; <c>false</c> otherwise.
        /// </returns>
        public bool Equals(Size other) => _value == other._value;

        /// <summary>
        /// Returns a boolean indicating whether the given <see cref="object"/> is equal to this <see
        /// cref="Size"/> instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare against.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="object"/> is equal to this instance; <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj) => obj is Size && Equals((Size)obj);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() => _value.GetHashCode();

        /// <summary>
        /// Compares the current instance with another size and returns an integer that indicates
        /// whether the current instance precedes, follows, or occurs in the same position in the
        /// sort order as the other version.
        /// </summary>
        /// <param name="other">A size to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the sizes being compared.</returns>
        public int CompareTo(Size other) => _value.ToInt64().CompareTo(other._value.ToInt64());

        /// <summary>
        /// Returns a boolean indicating whether the two given sizes are equal.
        /// </summary>
        /// <param name="left">The first size to compare.</param>
        /// <param name="right">The second size to compare.</param>
        /// <returns><c>true</c> if the sizes are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Size left, Size right) => left.Equals(right);

        /// <summary>
        /// Returns a boolean indicating whether the two given sizes are not equal.
        /// </summary>
        /// <param name="left">The first size to compare.</param>
        /// <param name="right">The second size to compare.</param>
        /// <returns>
        /// <c>true</c> if the sizes are not equal; <c>false</c> if they are equal.
        /// </returns>
        public static bool operator !=(Size left, Size right) => !left.Equals(right);

        /// <summary>
        /// Implicitly converts an <see cref="int"/> to a <see cref="Size"/>.
        /// </summary>
        /// <param name="value">Instance to convert.</param>
        public static implicit operator Size(int value) => new Size { _value = (IntPtr)value };

        /// <summary>
        /// Implicitly converts a <see cref="long"/> to a <see cref="Size"/>.
        /// </summary>
        /// <param name="value">Instance to convert.</param>
        public static implicit operator Size(long value) => new Size { _value = (IntPtr)value };

        /// <summary>
        /// Implicitly converts an <see cref="IntPtr"/> to a <see cref="Size"/>.
        /// </summary>
        /// <param name="value">Instance to convert.</param>
        public static implicit operator Size(IntPtr value) => new Size { _value = value };

        /// <summary>
        /// Explicitly converts a <see cref="Size"/> to an <see cref="int"/>.
        /// </summary>
        /// <param name="size">Instance to convert.</param>
        public static explicit operator int(Size size) => size._value.ToInt32();

        /// <summary>
        /// Implicitly converts a <see cref="Size"/> to a <see cref="long"/>.
        /// </summary>
        /// <param name="size">Instance to convert.</param>
        public static implicit operator long(Size size) => size._value.ToInt64();

        /// <summary>
        /// Implicitly converts a <see cref="Size"/> to an <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="size">Instance to convert.</param>
        public static implicit operator IntPtr(Size size) => size._value;
    }
}
