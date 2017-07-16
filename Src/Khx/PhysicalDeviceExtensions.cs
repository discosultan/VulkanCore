using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;
using static VulkanCore.Constant;

namespace VulkanCore.Khx
{
    /// <summary>
    /// Provides experimental Khronos specific extension methods for the <see cref="PhysicalDevice"/> class.
    /// </summary>
    public static unsafe class PhysicalDeviceExtensions
    {
        /// <summary>
        /// Query present rectangles for a surface on a physical device.
        /// <para>
        /// When using <see cref="DeviceGroupPresentModesKhx.LocalMultiDevice"/>, the application may
        /// need to know which regions of the surface are used when presenting locally on each
        /// physical device.
        /// </para>
        /// <para>
        /// Presentation of swapchain images to this surface need only have valid contents in the
        /// regions returned by this command.
        /// </para>
        /// </summary>
        /// <param name="physicalDevice">The physical device.</param>
        /// <param name="surface">The surface.</param>
        /// <returns>An array of <see cref="Rect2D"/> structures.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static Rect2D[] GetPresentRectanglesKhx(this PhysicalDevice physicalDevice, SurfaceKhr surface)
        {
            int count;
            Result result = vkGetPhysicalDevicePresentRectanglesKHX(physicalDevice)(physicalDevice, surface, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var rectangles = new Rect2D[count];
            fixed (Rect2D* rectanglesPtr = rectangles)
            {
                result = vkGetPhysicalDevicePresentRectanglesKHX(physicalDevice)(physicalDevice, surface, &count, rectanglesPtr);
                VulkanException.ThrowForInvalidResult(result);
                return rectangles;
            }
        }

        private delegate Result vkGetPhysicalDevicePresentRectanglesKHXDelegate(IntPtr physicalDevice, long surface, int* rectCount, Rect2D* rects);
        private static vkGetPhysicalDevicePresentRectanglesKHXDelegate vkGetPhysicalDevicePresentRectanglesKHX(PhysicalDevice physicalDevice) => GetProc<vkGetPhysicalDevicePresentRectanglesKHXDelegate>(physicalDevice, nameof(vkGetPhysicalDevicePresentRectanglesKHX));

        private static TDelegate GetProc<TDelegate>(PhysicalDevice physicalDevice, string name) where TDelegate : class => physicalDevice.Parent.GetProc<TDelegate>(name);
    }

    /// <summary>
    /// Structure describing multiview limits that can be supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceMultiviewPropertiesKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Max number of views in a subpass.
        /// </summary>
        public int MaxMultiviewViewCount;
        /// <summary>
        /// Max instance index for a draw in a multiview subpass.
        /// </summary>
        public int MaxMultiviewInstanceIndex;
    }
}
