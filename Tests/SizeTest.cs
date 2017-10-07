using System;
using Xunit;

namespace VulkanCore.Tests
{
    public class SizeTest
    {
        [Fact]
        public void ImplicitConversions()
        {
            const int intVal = 1;
            const long longVal = 2;
            var intPtrVal = new IntPtr(3);

            Size intSize = intVal;
            Size longSize = longVal;
            Size intPtrSize = intPtrVal;

            Assert.Equal(intVal, (int)intSize);
            Assert.Equal(longVal, (long)longSize);
            Assert.Equal(intPtrVal, (IntPtr)intPtrSize);
        }

        [Fact]
        public void ConvertToString()
        {
            IntPtr intPtrVal = new IntPtr(1);
            Size sizeVal = intPtrVal;
            Assert.Equal(intPtrVal.ToString(), sizeVal.ToString());
        }

        [Fact]
        public void SizeEquals()
        {
            var val1 = new Size(0);
            var val2 = new Size(1);

            Assert.True(val1.Equals(val1));
            Assert.False(val1.Equals(val2));
            Assert.True(val1 == val1);
            Assert.False(val1 == val2);
            Assert.False(val1 != val1);
            Assert.True(val1 != val2);
            Assert.NotEqual(val1.GetHashCode(), val2.GetHashCode());
        }

        [Fact]
        public void CompareTo()
        {
            var val1 = new Size(0);
            var val2 = new Size(1);

            Assert.Equal(-1, val1.CompareTo(val2));
            Assert.Equal(0, val1.CompareTo(val1));
            Assert.Equal(1, val2.CompareTo(val1));
        }
    }
}
