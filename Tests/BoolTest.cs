
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
            Bool intTrue = 1;
            Bool intFalse = 0;

            Assert.True(boolTrue);
            Assert.False(boolFalse);
            Assert.Equal(1, (int)intTrue);
            Assert.Equal(0, (int)intFalse);
        }

        [Fact]
        public void Equals()
        {
            Bool boolTrue = true;
            Bool boolFalse = false;

            Assert.True(boolTrue.Equals(boolTrue));
            Assert.False(boolTrue.Equals(boolFalse));
            Assert.True(boolTrue == boolTrue);
            Assert.False(boolTrue == boolFalse);
            Assert.False(boolTrue != boolTrue);
            Assert.True(boolTrue != boolFalse);
            Assert.Equal(true.GetHashCode(), boolTrue.GetHashCode());
            Assert.Equal(false.GetHashCode(), boolFalse.GetHashCode());
        }

        [Fact]
        public void ConvertToString()
        {
            Bool boolTrue = true;
            Bool boolFalse = false;

            Assert.Equal(true.ToString(), boolTrue.ToString());
            Assert.Equal(false.ToString(), boolFalse.ToString());
        }
    }
}
