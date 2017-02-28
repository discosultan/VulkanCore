using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore.Ext
{
    /// <summary>
    /// Provides extension methods for the <see cref="CommandBuffer"/> class.
    /// </summary>
    public static unsafe class CommandBufferExtensions
    {
        /// <summary>
        /// Open a command buffer marker region.
        /// <para>
        /// Typical Vulkan applications will submit many command buffers in each frame, with each
        /// command buffer containing a large number of individual commands. Being able to logically
        /// annotate regions of command buffers that belong together as well as hierarchically
        /// subdivide the frame is important to a developer’s ability to navigate the commands viewed holistically.
        /// </para>
        /// <para>
        /// The marker commands <see cref="CmdDebugMarkerBeginExt"/> and <see cref="CmdDebugMarkerEndExt"/>
        /// define regions of a series of commands that are grouped together, and they can be nested
        /// to create a hierarchy. 
        /// </para>
        /// </summary>
        /// <param name="commandBuffer">The command buffer into which the command is recorded.</param>
        /// <param name="markerInfo">Specifies the parameters of the marker region to open.</param>
        public static void CmdDebugMarkerBeginExt(this CommandBuffer commandBuffer, DebugMarkerMarkerInfoExt markerInfo)
        {
            var proc = commandBuffer.Parent.Parent.GetProc<CmdDebugMarkerBeginExtDelegate>("vkCmdDebugMarkerBeginEXT");

            int byteCount = Interop.String.GetMaxByteCount(markerInfo.MarkerName);
            var markerNamePtr = stackalloc byte[byteCount];
            Interop.String.ToPointer(markerInfo.MarkerName, markerNamePtr, byteCount);

            markerInfo.ToNative(out DebugMarkerMarkerInfoExt.Native nativeMarkerInfo, markerNamePtr);
            proc(commandBuffer, &nativeMarkerInfo);
        }

        /// <summary>
        /// Close a command buffer marker region. 
        /// <para>
        /// Typical Vulkan applications will submit many command buffers in each frame, with each
        /// command buffer containing a large number of individual commands. Being able to logically
        /// annotate regions of command buffers that belong together as well as hierarchically
        /// subdivide the frame is important to a developer’s ability to navigate the commands viewed holistically.
        /// </para>
        /// <para>
        /// The marker commands <see cref="CmdDebugMarkerBeginExt"/> and <see cref="CmdDebugMarkerEndExt"/>
        /// define regions of a series of commands that are grouped together, and they can be nested
        /// to create a hierarchy. 
        /// </para>
        /// </summary>
        /// <param name="commandBuffer">The command buffer into which the command is recorded.</param>
        public static void CmdDebugMarkerEndExt(this CommandBuffer commandBuffer)
        {
            var proc = commandBuffer.Parent.Parent.GetProc<CmdDebugMarkerEndExtDelegate>("vkCmdDebugMarkerEndEXT");
            proc(commandBuffer);
        }

        /// <summary>
        /// Insert a marker label into a command buffer.
        /// <para>Allows insertion of a single label within a command buffer.</para>
        /// </summary>
        /// <param name="commandBuffer">The command buffer into which the command is recorded.</param>
        /// <param name="markerInfo">Specifies the parameters of the marker to insert.</param>
        public static void CmdDebugMarkerInsertExt(this CommandBuffer commandBuffer, DebugMarkerMarkerInfoExt markerInfo)
        {
            var proc = commandBuffer.Parent.Parent.GetProc<CmdDebugMarkerInsertExtDelegate>("vkCmdDebugMarkerInsertEXT");

            int byteCount = Interop.String.GetMaxByteCount(markerInfo.MarkerName);
            var markerNamePtr = stackalloc byte[byteCount];
            Interop.String.ToPointer(markerInfo.MarkerName, markerNamePtr, byteCount);

            markerInfo.ToNative(out DebugMarkerMarkerInfoExt.Native nativeMarkerInfo, markerNamePtr);
            proc(commandBuffer, &nativeMarkerInfo);
        }

        // TODO: doc
        public static void CmdSetDiscardRectangleExt(this CommandBuffer commandBuffer,
            int firstDiscardRectangle, Rect2D[] discardRectangles)
        {
            fixed (Rect2D* discardRectanglesPtr = discardRectangles)
                CmdSetDiscardRectangleExt(commandBuffer, firstDiscardRectangle, discardRectangles?.Length ?? 0, discardRectanglesPtr);
        }

        private delegate void CmdDebugMarkerBeginExtDelegate(IntPtr commandBuffer, DebugMarkerMarkerInfoExt.Native* markerInfo);

        private delegate void CmdDebugMarkerEndExtDelegate(IntPtr commandBuffer);

        private delegate void CmdDebugMarkerInsertExtDelegate(IntPtr commandBuffer, DebugMarkerMarkerInfoExt.Native* markerInfo);

        [DllImport(VulkanDll, EntryPoint = "vkCmdSetDiscardRectangleEXT", CallingConvention = CallConv)]
        private static extern void CmdSetDiscardRectangleExt(IntPtr commandBuffer,
            int firstDiscardRectangle, int discardRectangleCount, Rect2D* discardRectangles);
    }

    /// <summary>
    /// Specify parameters of a command buffer marker region.
    /// </summary>
    public unsafe struct DebugMarkerMarkerInfoExt
    {
        /// <summary>
        /// A unicode string that contains the name of the marker.
        /// </summary>
        public string MarkerName;
        /// <summary>
        /// An optional RGBA color value that can be associated with the marker. A particular
        /// implementation may choose to ignore this color value. The values contain RGBA values in
        /// order, in the range 0.0 to 1.0. If all elements in color are set to 0.0 then it is ignored.
        /// </summary>
        public ColorF4 Color;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugMarkerMarkerInfoExt"/> structure.
        /// </summary>
        /// <param name="markerName">A unicode string that contains the name of the marker.</param>
        /// <param name="color">
        /// An optional RGBA color value that can be associated with the marker. A particular
        /// implementation may choose to ignore this color value. The values contain RGBA values in
        /// order, in the range 0.0 to 1.0. If all elements in color are set to 0.0 then it is ignored.
        /// </param>
        public DebugMarkerMarkerInfoExt(string markerName, ColorF4 color = default(ColorF4))
        {
            MarkerName = markerName;
            Color = color;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public byte* MarkerName;
            public ColorF4 Color;
        }

        internal void ToNative(out Native native, byte* markerName)
        {
            native.Type = StructureType.DebugMarkerMarkerInfoExt;
            native.Next = IntPtr.Zero;
            native.MarkerName = markerName;
            native.Color = Color;
        }
    }
}
