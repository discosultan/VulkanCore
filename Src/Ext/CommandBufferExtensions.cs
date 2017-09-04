using System;
using System.Runtime.InteropServices;

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
            int byteCount = Interop.String.GetMaxByteCount(markerInfo.MarkerName);
            var markerNamePtr = stackalloc byte[byteCount];
            Interop.String.ToPointer(markerInfo.MarkerName, markerNamePtr, byteCount);

            markerInfo.ToNative(out DebugMarkerMarkerInfoExt.Native nativeMarkerInfo, markerNamePtr);
            vkCmdDebugMarkerBeginEXT(commandBuffer)(commandBuffer, &nativeMarkerInfo);
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
            vkCmdDebugMarkerEndEXT(commandBuffer)(commandBuffer);
        }

        /// <summary>
        /// Insert a marker label into a command buffer.
        /// <para>Allows insertion of a single label within a command buffer.</para>
        /// </summary>
        /// <param name="commandBuffer">The command buffer into which the command is recorded.</param>
        /// <param name="markerInfo">Specifies the parameters of the marker to insert.</param>
        public static void CmdDebugMarkerInsertExt(this CommandBuffer commandBuffer, DebugMarkerMarkerInfoExt markerInfo)
        {
            int byteCount = Interop.String.GetMaxByteCount(markerInfo.MarkerName);
            var markerNamePtr = stackalloc byte[byteCount];
            Interop.String.ToPointer(markerInfo.MarkerName, markerNamePtr, byteCount);

            markerInfo.ToNative(out DebugMarkerMarkerInfoExt.Native nativeMarkerInfo, markerNamePtr);
            vkCmdDebugMarkerInsertEXT(commandBuffer)(commandBuffer, &nativeMarkerInfo);
        }

        /// <summary>
        /// If the pipeline state object was created with the <see
        /// cref="DynamicState.DiscardRectangleExt"/> dynamic state enabled, the discard rectangles
        /// are dynamically set and changed with this command.
        /// </summary>
        /// <param name="commandBuffer">The command buffer into which the command will be recorded.</param>
        /// <param name="firstDiscardRectangle">
        /// The index of the first discard rectangle whose state is updated by the command.
        /// </param>
        /// <param name="discardRectangles">Structures specifying discard rectangles.</param>
        public static void CmdSetDiscardRectangleExt(this CommandBuffer commandBuffer,
            int firstDiscardRectangle, Rect2D[] discardRectangles)
        {
            fixed (Rect2D* discardRectanglesPtr = discardRectangles)
                vkCmdSetDiscardRectangleEXT(commandBuffer)(commandBuffer, firstDiscardRectangle, discardRectangles?.Length ?? 0, discardRectanglesPtr);
        }

        /// <summary>
        /// Set the dynamic sample locations state.
        /// </summary>
        /// <param name="commandBuffer">The command buffer into which the command will be recorded.</param>
        /// <param name="sampleLocationsInfo">The sample locations state to set.</param>
        public static void CmdSetSampleLocationsExt(this CommandBuffer commandBuffer,
            SampleLocationsInfoExt sampleLocationsInfo)
        {
            vkCmdSetSampleLocationsEXT(commandBuffer)(commandBuffer, &sampleLocationsInfo);
        }

        private delegate void vkCmdDebugMarkerBeginEXTDelegate(IntPtr commandBuffer, DebugMarkerMarkerInfoExt.Native* markerInfo);
        private static vkCmdDebugMarkerBeginEXTDelegate vkCmdDebugMarkerBeginEXT(CommandBuffer commandBuffer) => GetProc<vkCmdDebugMarkerBeginEXTDelegate>(commandBuffer, nameof(vkCmdDebugMarkerBeginEXT));

        private delegate void vkCmdDebugMarkerEndEXTDelegate(IntPtr commandBuffer);
        private static vkCmdDebugMarkerEndEXTDelegate vkCmdDebugMarkerEndEXT(CommandBuffer commandBuffer) => GetProc<vkCmdDebugMarkerEndEXTDelegate>(commandBuffer, nameof(vkCmdDebugMarkerEndEXT));

        private delegate void vkCmdDebugMarkerInsertEXTDelegate(IntPtr commandBuffer, DebugMarkerMarkerInfoExt.Native* markerInfo);
        private static vkCmdDebugMarkerInsertEXTDelegate vkCmdDebugMarkerInsertEXT(CommandBuffer commandBuffer) => GetProc<vkCmdDebugMarkerInsertEXTDelegate>(commandBuffer, nameof(vkCmdDebugMarkerInsertEXT));

        private delegate void vkCmdSetDiscardRectangleEXTDelegate(IntPtr commandBuffer, int firstDiscardRectangle, int discardRectangleCount, Rect2D* discardRectangles);
        private static vkCmdSetDiscardRectangleEXTDelegate vkCmdSetDiscardRectangleEXT(CommandBuffer commandBuffer) => GetProc<vkCmdSetDiscardRectangleEXTDelegate>(commandBuffer, nameof(vkCmdSetDiscardRectangleEXT));

        private delegate void vkCmdSetSampleLocationsEXTDelegate(IntPtr commandBuffer, SampleLocationsInfoExt* sampleLocationsInfo);
        private static vkCmdSetSampleLocationsEXTDelegate vkCmdSetSampleLocationsEXT(CommandBuffer commandBuffer) => GetProc<vkCmdSetSampleLocationsEXTDelegate>(commandBuffer, nameof(vkCmdSetSampleLocationsEXT));

        private static TDelegate GetProc<TDelegate>(CommandBuffer commandBuffer, string name) where TDelegate : class => commandBuffer.Parent.Parent.GetProc<TDelegate>(name);
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

    /// <summary>
    /// Structure specifying a set of sample locations.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SampleLocationsInfoExt
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
        /// Specifies the number of sample locations per pixel.
        /// </summary>
        public SampleCounts SampleLocationsPerPixel;
        /// <summary>
        /// The size of the sample location grid to select custom sample locations for.
        /// </summary>
        public Extent2D SampleLocationGridSize;
        /// <summary>
        /// The number of sample locations in <see cref="SampleLocations"/>.
        /// </summary>
        public int SampleLocationsCount;
        /// <summary>
        /// An array of <see cref="SampleLocationExt"/> structures.
        /// </summary>
        public IntPtr SampleLocations;
    }

    /// <summary>
    /// Structure specifying the coordinates of a sample location.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SampleLocationExt
    {
        /// <summary>
        /// The horizontal coordinate of the sample's location.
        /// </summary>
        public float X;
        /// <summary>
        /// The vertical coordinate of the sample's location.
        /// </summary>
        public float Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleLocationExt"/> structure.
        /// </summary>
        /// <param name="x">The horizontal coordinate of the sample's location.</param>
        /// <param name="y">The vertical coordinate of the sample's location.</param>
        public SampleLocationExt(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
