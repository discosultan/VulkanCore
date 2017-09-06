using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Ext
{
    /// <summary>
    /// Provides extension methods for the <see cref="ShaderModule"/> class.
    /// </summary>
    public static class ShaderModuleExtensions
    {
        /// <summary>
        /// Specify validation cache to use during shader module creation.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ShaderModuleValidationCacheCreateInfoExt
        {
            /// <summary>
            /// The type of this structure.
            /// </summary>
            public StructureType Type;
            /// <summary>
            /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
            /// </summary>
            public IntPtr Next;
            /// <summary>
            /// The <see cref="Ext.ValidationCacheExt"/> object from which the results of prior
            /// validation attempts will be written, and to which new validation results for this
            /// <see cref="ShaderModule"/> will be written (if not already present).
            /// </summary>
            public long ValidationCache;
        }
    }
}
