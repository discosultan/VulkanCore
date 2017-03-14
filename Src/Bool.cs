using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// Vulkan boolean type.
    /// <para>
    /// <c>true</c> represents a boolean True (integer 1) value, and <c>false</c> a boolean False
    /// (integer 0) value.
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct Bool : IEquatable<Bool>
    {
        private readonly int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bool"/> struct.
        /// </summary>
        /// <param name="value"></param>
        public Bool(bool value)
        {
            _value = value ? Constant.True : Constant.False;
        }

        /// <summary>
        /// Returns a string representing this <see cref="Bool"/> instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => ((bool)this).ToString();

        /// <summary>
        /// Performs an implicit conversion from <see cref="bool"/> to <see cref="Bool"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static implicit operator Bool(bool value) => new Bool(value);

        /// <summary>
        /// Performs an implicit conversion from <see cref="Bool"/> to <see cref="bool"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static implicit operator bool(Bool value) => value._value == Constant.True;

        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="Bool"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static implicit operator Bool(int value) => new Bool(value != Constant.False);

        /// <summary>
        /// Performs an implicit conversion from <see cref="Bool"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static implicit operator int(Bool value) => value._value == Constant.True ? 1 : 0;

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if <paramref name="other" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public bool Equals(Bool other)
        {
            return _value == other._value;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Bool && Equals((Bool)obj);
        }

        /// <summary>
        /// Implements the ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Bool left, Bool right) => left.Equals(right);

        /// <summary>
        /// Implements the !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Bool left, Bool right) => !left.Equals(right);

        /// <inheritdoc/>
        public override int GetHashCode() => _value;
    }
}
