using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Size // TODO: doc
    {
        private IntPtr _value;

        public static implicit operator Size(int value) =>
            new Size { _value = (IntPtr)value };

        public static implicit operator Size(long value) =>
            new Size { _value = (IntPtr)value };

        public static implicit operator Size(IntPtr value) =>
            new Size { _value = value };

        public static explicit operator int(Size size) =>
            size._value.ToInt32();

        public static implicit operator long(Size size) =>
            size._value.ToInt64();

        public static implicit operator IntPtr(Size size) =>
            size._value;
    }
}
