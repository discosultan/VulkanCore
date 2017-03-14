using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// A platform-specific type that is used to represent a size.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PointerSize : IEquatable<PointerSize>
    {
        // We use IntPtr instead of UIntPtr to stay CLS compliant.
        // https://msdn.microsoft.com/en-us/library/system.intptr(v=vs.110).aspx
        private IntPtr _value;

        /// <summary>
        /// An empty pointer size initialized to zero.
        /// </summary>
        public static readonly PointerSize Zero = new PointerSize { _value = IntPtr.Zero };

        /// <summary>
        /// Converts the numeric value of the current <see cref="PointerSize"/> object to its equivalent
        /// string representation.
        /// </summary>
        /// <returns>The string representation of the value of this instance.</returns>
        public override string ToString() => _value.ToString();

        /// <summary>
        /// Implicitly converts an <see cref="int"/> to a <see cref="PointerSize"/>.
        /// </summary>
        /// <param name="value">Instance to convert.</param>
        public static implicit operator PointerSize(int value) => new PointerSize { _value = (IntPtr)value };

        /// <summary>
        /// Implicitly converts a <see cref="long"/> to a <see cref="PointerSize"/>.
        /// </summary>
        /// <param name="value">Instance to convert.</param>
        public static implicit operator PointerSize(long value) => new PointerSize { _value = (IntPtr)value };

        /// <summary>
        /// Implicitly converts an <see cref="IntPtr"/> to a <see cref="PointerSize"/>.
        /// </summary>
        /// <param name="value">Instance to convert.</param>
        public static implicit operator PointerSize(IntPtr value) => new PointerSize { _value = value };

        /// <summary>
        /// Explicitly converts a <see cref="PointerSize"/> to an <see cref="int"/>.
        /// </summary>
        /// <param name="size">Instance to convert.</param>
        public static explicit operator int(PointerSize size) => size._value.ToInt32();

        /// <summary>
        /// Implicitly converts a <see cref="PointerSize"/> to a <see cref="long"/>.
        /// </summary>
        /// <param name="size">Instance to convert.</param>
        public static implicit operator long(PointerSize size) => size._value.ToInt64();

        /// <summary>
        /// Implicitly converts a <see cref="PointerSize"/> to an <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="size">Instance to convert.</param>
        public static implicit operator IntPtr(PointerSize size) => size._value;

        /// <summary>
        ///   Determines whether the specified <see cref = "PointerSize" /> is equal to this instance.
        /// </summary>
        /// <param name = "other">The <see cref = "PointerSize" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref = "PointerSize" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(PointerSize other)
        {
            return _value == other._value;
        }

        /// <summary>
        ///   Determines whether the specified <see cref = "System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name = "value">The <see cref = "System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref = "System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object value)
        {
            if (value == null)
                return false;

            if (!ReferenceEquals(value.GetType(), typeof(PointerSize)))
                return false;

            return Equals((PointerSize)value);
        }

        /// <summary>
        ///   Tests for equality between two objects.
        /// </summary>
        /// <param name = "left">The first value to compare.</param>
        /// <param name = "right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name = "left" /> has the same value as <paramref name = "right" />; otherwise, <c>false</c>.</returns>
        public static bool operator ==(PointerSize left, PointerSize right) => left.Equals(right);

        /// <summary>
        ///   Tests for inequality between two objects.
        /// </summary>
        /// <param name = "left">The first value to compare.</param>
        /// <param name = "right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name = "left" /> has a different value than <paramref name = "right" />; otherwise, <c>false</c>.</returns>
        public static bool operator !=(PointerSize left, PointerSize right) => !left.Equals(right);

        /// <summary>
        ///   Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///   A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => _value.ToInt32();
    }
}
