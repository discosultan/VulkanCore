
using Xunit;

namespace VulkanCore.Tests
{
    public class ExtentTest
    {
        [Fact]
        public void Extent2DEquals()
        {
            var val1 = new Extent2D(0, 1);
            var val2 = new Extent2D(2, 3);

            Assert.True(val1.Equals(val1));
            Assert.False(val1.Equals(val2));
            Assert.True(val1 == val1);
            Assert.False(val1 == val2);
            Assert.False(val1 != val1);
            Assert.True(val1 != val2);
            Assert.NotEqual(val1.GetHashCode(), val2.GetHashCode());
        }

        [Fact]
        public void Extent3DEquals()
        {
            var val1 = new Extent3D(0, 1, 2);
            var val2 = new Extent3D(3, 4, 5);

            Assert.True(val1.Equals(val1));
            Assert.False(val1.Equals(val2));
            Assert.True(val1 == val1);
            Assert.False(val1 == val2);
            Assert.False(val1 != val1);
            Assert.True(val1 != val2);
            Assert.NotEqual(val1.GetHashCode(), val2.GetHashCode());
        }
    }
}
