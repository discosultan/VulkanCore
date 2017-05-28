using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Nvx
{
    /// <summary>
    /// Provides NVIDIA specific extension methods for the <see cref="PhysicalDevice"/> class.
    /// </summary>
    public static unsafe class PhysicalDeviceExtensions
    {
        /// <summary>
        /// Returns device-generated commands related properties of a physical device.
        /// </summary>
        /// <param name="physicalDevice">
        /// The handle to the physical device whose properties will be queried.
        /// </param>
        /// <returns>Structures that will be filled with returned information.</returns>
        public static (DeviceGeneratedCommandsFeaturesNvx, DeviceGeneratedCommandsLimitsNvx) GetGeneratedCommandsPropertiesNvx(
            this PhysicalDevice physicalDevice)
        {
            DeviceGeneratedCommandsFeaturesNvx features;
            DeviceGeneratedCommandsLimitsNvx limits;
            vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX(physicalDevice)(physicalDevice, &features, &limits);
            return (features, limits);
        }

        private delegate void vkGetPhysicalDeviceGeneratedCommandsPropertiesNVXDelegate(IntPtr physicalDevice, DeviceGeneratedCommandsFeaturesNvx* features, DeviceGeneratedCommandsLimitsNvx* limits);
        private static vkGetPhysicalDeviceGeneratedCommandsPropertiesNVXDelegate vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX(PhysicalDevice physicalDevice) => physicalDevice.Parent.GetProc<vkGetPhysicalDeviceGeneratedCommandsPropertiesNVXDelegate>(nameof(vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX));
    }

    /// <summary>
    /// Structure specifying physical device support.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceGeneratedCommandsFeaturesNvx
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// Indicates whether the <see cref="ObjectTableNvx"/> supports entries with <see
        /// cref="ObjectEntryUsagesNvx.Graphics"/> bit set and <see
        /// cref="IndirectCommandsLayoutNvx"/> supports <see cref="PipelineBindPoint.Compute"/>.
        /// </summary>
        public Bool ComputeBindingPointSupport;
    }

    /// <summary>
    /// Structure specifying physical device limits.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceGeneratedCommandsLimitsNvx
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// The maximum number of tokens in <see cref="IndirectCommandsLayoutNvx"/>.
        /// </summary>
        public int MaxIndirectCommandsLayoutTokenCount;
        /// <summary>
        /// The maximum number of entries per resource type in <see cref="ObjectTableNvx"/>.
        /// </summary>
        public int MaxObjectEntryCounts;
        /// <summary>
        /// The minimum alignment for memory addresses optionally used in <see cref="CommandBufferExtensions.CmdProcessCommandsNvx"/>.
        /// </summary>
        public int MinSequenceCountBufferOffsetAlignment;
        /// <summary>
        /// The minimum alignment for memory addresses optionally used in <see cref="CommandBufferExtensions.CmdProcessCommandsNvx"/>.
        /// </summary>
        public int MinSequenceIndexBufferOffsetAlignment;
        /// <summary>
        /// The minimum alignment for memory addresses optionally used in <see cref="CommandBufferExtensions.CmdProcessCommandsNvx"/>.
        /// </summary>
        public int MinCommandsTokenBufferOffsetAlignment;
    }

    /// <summary>
    /// Structure describing multiview limits that can be supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceMultiviewPerViewAttributesPropertiesNvx
    {
        internal StructureType Type;

        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Is <c>true</c> if the implementation supports per-view position values that differ in
        /// components other than the X component.
        /// </summary>
        public Bool PerViewPositionAllComponents;
    }
}
