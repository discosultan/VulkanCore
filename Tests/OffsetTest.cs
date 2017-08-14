
using Xunit;

namespace VulkanCore.Tests
{
    public class OffsetTest
    {
#pragma warning disable CS1718 // Comparison made to same variable.
        [Fact]
        public void Offset2DEquals()
        {
            var val1 = new Offset2D(0, 1);
            var val2 = new Offset2D(2, 3);

            Assert.True(val1.Equals(val1));
            Assert.False(val1.Equals(val2));
            Assert.True(val1 == val1);
            Assert.False(val1 == val2);
            Assert.False(val1 != val1);
            Assert.True(val1 != val2);
            Assert.NotEqual(val1.GetHashCode(), val2.GetHashCode());
        }

        [Fact]
        public void Offset3DEquals()
        {
            var val1 = new Offset3D(0, 1, 2);
            var val2 = new Offset3D(3, 4, 5);

            Assert.True(val1.Equals(val1));
            Assert.False(val1.Equals(val2));
            Assert.True(val1 == val1);
            Assert.False(val1 == val2);
            Assert.False(val1 != val1);
            Assert.True(val1 != val2);
            Assert.NotEqual(val1.GetHashCode(), val2.GetHashCode());
        }
#pragma warning restore CS1718 // Comparison made to same variable.
    }
}
