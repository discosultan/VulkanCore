using Xunit;

namespace VulkanCore.Tests
{
    public class VersionTest
    {
        [Theory]
        [InlineData(1, 0, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(1, 2, 3)]
        public void ComponentsMatch(int major, int minor, int patch)
        {
            var version = new Version(major, minor, patch);

            Assert.Equal(major, version.Major);
            Assert.Equal(minor, version.Minor);
            Assert.Equal(patch, version.Patch);
        }

        [Fact]
        public void Equals_Equal()
        {
            var version1 = new Version(1, 2, 3);
            var version2 = new Version(1, 2, 3);

            Assert.Equal(version1, version2);
        }

        [Fact]
        public void Equals_NotEqual()
        {
            var version1 = new Version(1, 2, 3);
            var version2 = new Version(3, 2, 1);

            Assert.NotEqual(version1, version2);
        }

        [Fact]
        public void CompareTo_LessThan()
        {
            var version1 = new Version(1, 2, 3);
            var version2 = new Version(1, 2, 4);

            Assert.Equal(-1, version1.CompareTo(version2));
        }

        [Fact]
        public void CompareTo_Equal()
        {
            var version1 = new Version(1, 2, 3);
            var version2 = new Version(1, 2, 3);

            Assert.Equal(0, version1.CompareTo(version2));
        }

        [Fact]
        public void CompareTo_GreaterThan()
        {
            var version1 = new Version(1, 2, 3);
            var version2 = new Version(1, 2, 2);

            Assert.Equal(1, version1.CompareTo(version2));
        }
    }
}
