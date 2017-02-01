namespace VulkanCore.Tests.Utilities
{
    internal static class Int32Extensions
    {
        public static int IndexOfFirstFlag(this int value)
        {
            const int bitCount = sizeof(int) * 8;
            
            for (int i = 0; i < bitCount; i++)
                if ((value & (1 << i)) > 0) return i;

            return -1;
        }
    }
}
