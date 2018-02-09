using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Amd
{
    /// <summary>
    /// Provides AMD specific extension methods for the <see cref="Pipeline"/> class.
    /// </summary>
    public unsafe static class PipelineExtensions
    {
        /// <summary>
        /// Get information about a shader in a pipeline.
        /// </summary>
        /// <param name="pipeline">The target of the query.</param>
        /// <param name="shaderStage">
        /// Identifies the particular shader within the pipeline about which information is being queried.
        /// </param>
        /// <param name="infoType">Describes what kind of information is being queried.</param>
        /// <returns>A buffer of shader information.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static byte[] GetShaderInfoAmd(this Pipeline pipeline, ShaderStages shaderStage, ShaderInfoTypeAmd infoType)
        {
            Size size;
            Result result = vkGetShaderInfoAMD(pipeline)(pipeline.Parent, pipeline, shaderStage, infoType, &size, null);
            VulkanException.ThrowForInvalidResult(result);

            var data = new byte[size];
            fixed (byte* dataPtr = data)
            {
                result = vkGetShaderInfoAMD(pipeline)(pipeline.Parent, pipeline, shaderStage, infoType, &size, dataPtr);
                VulkanException.ThrowForInvalidResult(result);
            }

            return data;
        }

        private delegate Result vkGetShaderInfoAMDDelegate(IntPtr device, long pipeline, ShaderStages shaderStage, ShaderInfoTypeAmd infoType, Size* infoSize, void* info);
        private static vkGetShaderInfoAMDDelegate vkGetShaderInfoAMD(Pipeline pipeline) => pipeline.Parent.GetProc<vkGetShaderInfoAMDDelegate>(nameof(vkGetShaderInfoAMD));
    }

    public enum ShaderInfoTypeAmd
    {
        Statistics = 0,
        Binary = 1,
        Disassembly = 2
    }

    /// <summary>
    /// Resource usage information about a particular shader within a pipeline.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ShaderResourceUsageAmd
    {
        /// <summary>
        /// The number of vector instruction general-purpose registers used by this shader.
        /// </summary>
        public int NumUsedVgprs;
        /// <summary>
        /// The number of scalar instruction general-purpose registers used by this shader.
        /// </summary>
        public int NumUsedSgprs;
        /// <summary>
        /// The maximum local data store size per work group in bytes.
        /// </summary>
        public int LdsSizePerLocalWorkGroup;
        /// <summary>
        /// The LDS usage size in bytes per work group by this shader.
        /// </summary>
        public int LdsUsageSizeInBytes;
        /// <summary>
        /// The scratch memory usage in bytes by this shader.
        /// </summary>
        public int ScratchMemUsageInBytes;
    }

    /// <summary>
    /// Statistical information about a particular shader within a pipeline.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ShaderStatisticsInfoAmd
    {
        /// <summary>
        /// Are the combination of logical shader stages contained within this shader.
        /// </summary>
        public ShaderStages ShaderStageMask;
        /// <summary>
        /// An instance of <see cref="ShaderResourceUsageAmd"/> describing internal physical
        /// device resources used by this shader.
        /// </summary>
        public ShaderResourceUsageAmd ResourceUsage;
        /// <summary>
        /// The maximum number of vector instruction general-purpose registers (VGPRs) available
        /// to the physical device.
        /// </summary>
        public int NumPhysicalVgprs;
        /// <summary>
        /// The maximum number of scalar instruction general-purpose registers (SGPRs) available
        /// to the physical device.
        /// </summary>
        public int NumPhysicalSgprs;
        /// <summary>
        /// The maximum limit of VGPRs made available to the shader compiler.
        /// </summary>
        public int NumAvailableVgprs;
        /// <summary>
        /// The maximum limit of SGPRs made available to the shader compiler.
        /// </summary>
        public int NumAvailableSgprs;
        /// <summary>
        /// The local workgroup size of this shader in { X, Y, Z } dimensions.
        /// </summary>
        public fixed int ComputeWorkGroupSize[3];
    }
}
