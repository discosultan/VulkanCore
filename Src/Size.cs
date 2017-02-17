using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    /// <summary>
    /// A platform-specific type that is used to represent a size.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Size
    {
        // We use IntPtr instead of UIntPtr to stay CLS compliant.
        // https://msdn.microsoft.com/en-us/library/system.intptr(v=vs.110).aspx
        private IntPtr _value;

        /// <summary>
        /// Converts the numeric value of the current <see cref="Size"/> object to its equivalent
        /// string representation.
        /// </summary>
        /// <returns>The string representation of the value of this instance.</returns>
        public override string ToString() => _value.ToString();

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
