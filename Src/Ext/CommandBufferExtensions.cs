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

    /// <summary>
    /// Structure specifying the sample locations state to use in the initial layout transition of attachments.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AttachmentSampleLocationsExt
    {
        /// <summary>
        /// The index of the attachment for which the sample locations state is provided.
        /// </summary>
        public int AttachmentIndex;
        /// <summary>
        /// The sample locations state to use for the layout transition of the given attachment from
        /// the initial layout of the attachment to the image layout specified for the attachment in
        /// the first subpass using it.
        /// </summary>
        public SampleLocationsInfoExt SampleLocationsInfo;
    }

    /// <summary>
    /// Structure specifying the sample locations state to use for layout transitions of
    /// attachments performed after a given subpass.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SubpassSampleLocationsExt
    {
        /// <summary>
        /// Is the index of the subpass for which the sample locations state is provided.
        /// </summary>
        public int SubpassIndex;
        /// <summary>
        /// Is the sample locations state to use for the layout transition of the depth/stencil
        /// attachment away from the image layout the attachment is used with in the subpass
        /// specified in <c>SubpassIndex</c>.
        /// </summary>
        public SampleLocationsInfoExt SampleLocationsInfo;
    }

    /// <summary>
    /// Structure specifying sample locations to use for the layout transition of custom sample
    /// locations compatible depth/stencil attachments.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RenderPassSampleLocationsBeginInfoExt
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
        /// The number of elements in the <see cref="AttachmentInitialSampleLocations"/> array.
        /// </summary>
        public int AttachmentInitialSampleLocationsCount;
        /// <summary>
        /// Is an array of <see cref="AttachmentInitialSampleLocationsCount"/><see
        /// cref="AttachmentSampleLocationsExt"/> structures specifying the attachment indices and
        /// their corresponding sample location state. Each element of <see
        /// cref="AttachmentInitialSampleLocations"/> can specify the sample location state to use in
        /// the automatic layout transition performed to transition a depth/stencil attachment from
        /// the initial layout of the attachment to the image layout specified for the attachment in
        /// the first subpass using it.
        /// </summary>
        public IntPtr AttachmentInitialSampleLocations;
        /// <summary>
        /// The number of elements in the <see cref="PostSubpassSampleLocations"/> array.
        /// </summary>
        public int PostSubpassSampleLocationsCount;
        /// <summary>
        /// Is an array of <see cref="PostSubpassSampleLocationsCount"/><see
        /// cref="SubpassSampleLocationsExt"/> structures specifying the subpass indices and their
        /// corresponding sample location state.
        /// <para>
        /// Each element of <see cref="PostSubpassSampleLocations"/> can specify the sample location
        /// state to use in the automatic layout transition performed to transition the depth/stencil
        /// attachment used by the specified subpass to the image layout specified in a dependent
        /// subpass or to the final layout of the attachment in case the specified subpass is the
        /// last subpass using that attachment.
        /// </para>
        /// <para>
        /// In addition, if <see
        /// cref="PhysicalDeviceSampleLocationsPropertiesExt.VariableSampleLocations"/> is
        /// <c>false</c>, each element of <see cref="PostSubpassSampleLocations"/> must specify the
        /// sample location state that matches the sample locations used by all pipelines that will
        /// be bound to a command buffer during the specified subpass.
        /// </para>
        /// <para>
        /// If <c>VariableSampleLocations</c> is <c>true</c>, the sample locations used for
        /// rasterization do not depend on <see cref="PostSubpassSampleLocations"/>.
        /// </para>
        /// </summary>
        public IntPtr PostSubpassSampleLocations;
    }

    /// <summary>
    /// Structure describing sample location limits that can be supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct PhysicalDeviceSampleLocationsPropertiesExt
    {
        public StructureType Type;
        public IntPtr Next;
        public int SampleLocationSampleCounts;
        public Extent2D MaxSampleLocationGridSize;
        public fixed float SampleLocationCoordinateRange[2];
        public int SampleLocationSubPixelBits;
        public Bool VariableSampleLocations;
    }
}
