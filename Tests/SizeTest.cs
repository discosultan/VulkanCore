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

            PointerSize intSize = intVal;
            PointerSize longSize = longVal;
            PointerSize intPtrSize = intPtrVal;

            Assert.Equal(intVal, (int)intSize);
            Assert.Equal(longVal, (long)longSize);
            Assert.Equal(intPtrVal, (IntPtr)intPtrSize);
        }

        [Fact]
        public void ConvertToString()
        {
            IntPtr intPtrVal = new IntPtr(1);
            PointerSize sizeVal = intPtrVal;
            Assert.Equal(intPtrVal.ToString(), sizeVal.ToString());
        }
    }
}
