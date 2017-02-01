
using Xunit;

namespace VulkanCore.Tests
{
    public class BoolTest
    {
        [Fact]
        public void ImplicitConversions()
        {
            Bool boolTrue = true;
            Bool boolFalse = false;

            Assert.True(boolTrue);
            Assert.False(boolFalse);
        }

        [Fact]
        public void ToString_SameOutputAsPrimitive()
        {
            Bool boolTrue = true;
            Bool boolFalse = false;

            Assert.Equal(true.ToString(), boolTrue.ToString());
            Assert.Equal(false.ToString(), boolFalse.ToString());
        }
    }
}
