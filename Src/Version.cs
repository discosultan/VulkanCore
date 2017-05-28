using System;

namespace VulkanCore
{
    /// <summary>
    /// Semantic Versioning number.
    /// </summary>
    /// <remarks>http://semver.org/</remarks>
    public struct Version : IEquatable<Version>, IComparable<Version>
    {
        private readonly int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> struct.
        /// </summary>
        /// <param name="major">Major component of semver.</param>
        /// <param name="minor">Minor component of semver.</param>
        /// <param name="patch">Patch component of semver.</param>
        public Version(int major, int minor, int patch)
        {
            _value = major << 22 | minor << 12 | patch;
        }

        /// <summary>
        /// Gets the major component of semver.
        /// </summary>
        public int Major => _value >> 22;
        /// <summary>
        /// Gets the minor component of semver.
        /// </summary>
        public int Minor => (_value >> 12) & 0x03FF;
        /// <summary>
        /// Gets the patch component of semver.
        /// </summary>
        public int Patch => _value & 0x0FFF;

        /// <summary>
        /// Returns a string that represents the current version in the form of MAJOR.MINOR.PATCH.
        /// </summary>
        /// <returns>A string that represents the current version.</returns>
        public override string ToString() => $"{Major}.{Minor}.{Patch}";

        /// <summary>
        /// Indicates whether the current version is equal to another version.
        /// </summary>
        /// <param name="other">A version to compare with this version.</param>
        /// <returns>
        /// <c>true</c> if the current version is equal to the other parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Version other) => _value == other._value;

        /// <summary>
        /// Compares the current instance with another version and returns an integer that indicates
        /// whether the current instance precedes, follows, or occurs in the same position in the
        /// sort order as the other version.
        /// </summary>
        /// <param name="other">A version to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the versions being compared.</returns>
        public int CompareTo(Version other) => _value.CompareTo(other._value);

        /// <summary>
        /// A shorthand for writing <c>new Version(0, 0, 0)</c>.
        /// </summary>
        public static Version Zero => new Version(0, 0, 0);
    }
}
