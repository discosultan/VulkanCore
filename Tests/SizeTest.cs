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

            Size intSize = intVal;
            Size longSize = longVal;

            Assert.Equal(intVal, intSize);
            Assert.Equal(longVal, longSize);
        }
    }
}
