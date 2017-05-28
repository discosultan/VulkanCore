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
    [StructLayout(LayoutKind.Sequential)]
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
        /// Returns a boolean indicating whether the given <see cref="Bool"/> is equal to this <see
        /// cref="Bool"/> instance.
        /// </summary>
        /// <param name="other">The <see cref="Bool"/> to compare this instance to.</param>
        /// <returns>
        /// <c>true</c> if the other <see cref="Bool"/> is equal to this instance; <c>false</c> otherwise.
        /// </returns>
        public bool Equals(Bool other) => _value == other._value;

        /// <summary>
        /// Returns a boolean indicating whether the given <see cref="object"/> is equal to this <see
        /// cref="Bool"/> instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare against.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="object"/> is equal to this instance; <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj) => obj is Bool && Equals((Bool)obj);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() => _value.GetHashCode();

        /// <summary>
        /// Returns a boolean indicating whether the two given booleans are equal.
        /// </summary>
        /// <param name="left">The first boolean to compare.</param>
        /// <param name="right">The second boolean to compare.</param>
        /// <returns><c>true</c> if the booleans are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Bool left, Bool right) => left.Equals(right);

        /// <summary>
        /// Returns a boolean indicating whether the two given booleans are not equal.
        /// </summary>
        /// <param name="left">The first boolean to compare.</param>
        /// <param name="right">The second boolean to compare.</param>
        /// <returns>
        /// <c>true</c> if the booleans are not equal; <c>false</c> if they are equal.
        /// </returns>
        public static bool operator !=(Bool left, Bool right) => !left.Equals(right);

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
    }
}
