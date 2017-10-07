using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Android
{
    /// <summary>
    /// Provides Android specific extension methods for the <see cref="Device"/> class.
    /// </summary>
    public static unsafe class DeviceExtensions
    {
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static int GetSwapchainGrallocUsageAndroid(this Device device, Format format, int imageUsage)
        {
            int usage;
            Result result = vkGetSwapchainGrallocUsageANDROID(device)(device, format, imageUsage, &usage);
            VulkanException.ThrowForInvalidResult(result);
            return usage;
        }

        private delegate Result vkGetSwapchainGrallocUsageANDROIDDelegate(IntPtr device, Format format, int imageUsage, int* grallocUsage);
        private static vkGetSwapchainGrallocUsageANDROIDDelegate vkGetSwapchainGrallocUsageANDROID(Device device) => device.GetProc<vkGetSwapchainGrallocUsageANDROIDDelegate>(nameof(vkGetSwapchainGrallocUsageANDROID));
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NativeBufferAndroid
    {
        public StructureType Type;
        public IntPtr Next;
        public IntPtr Handle;
        public int Stride;
        public int Format;
        public int Usage;
    }
}
