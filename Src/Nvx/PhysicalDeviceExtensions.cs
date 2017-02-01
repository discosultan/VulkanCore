using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constants;

namespace VulkanCore.Nvx
{
    /// <summary>
    /// Provides extension methods for the <see cref="PhysicalDevice"/> class.
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
            GetPhysicalDeviceGeneratedCommandsPropertiesNvx(physicalDevice, &features, &limits);
            return (features, limits);
        }

        [DllImport(VulkanDll, EntryPoint = "vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX", CallingConvention = CallConv)]
        private static extern void GetPhysicalDeviceGeneratedCommandsPropertiesNvx(IntPtr physicalDevice, 
            DeviceGeneratedCommandsFeaturesNvx* features, DeviceGeneratedCommandsLimitsNvx* limits);
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
}
