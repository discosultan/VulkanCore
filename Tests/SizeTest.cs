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
    }
}
