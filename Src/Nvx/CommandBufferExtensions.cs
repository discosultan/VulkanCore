using System;
using System.Runtime.InteropServices;

namespace VulkanCore.Nvx
{
    /// <summary>
    /// Provides NVIDIA specific extension methods for the <see cref="CommandBuffer"/> class.
    /// </summary>
    public static unsafe class CommandBufferExtensions
    {
        /// <summary>
        /// Performs the generation of commands on the device.
        /// </summary>
        /// <param name="commandBuffer">
        /// The primary command buffer in which the generation process takes space.
        /// </param>
        /// <param name="processCommandsInfo">
        /// The structure containing parameters affecting the processing of commands.
        /// </param>
        public static void CmdProcessCommandsNvx(this CommandBuffer commandBuffer,
            CmdProcessCommandsInfoNvx processCommandsInfo)
        {
            fixed (IndirectCommandsTokenNvx* tokensPtr = processCommandsInfo.IndirectCommandsTokens)
            {
                processCommandsInfo.ToNative(out CmdProcessCommandsInfoNvx.Native nativeProcessCommandsInfo, tokensPtr);
                vkCmdProcessCommandsNVX(commandBuffer)(commandBuffer, &nativeProcessCommandsInfo);
            }
        }

        /// <summary>
        /// Perform a reservation of command buffer space.
        /// <para>
        /// The <paramref name="commandBuffer"/> must not have had a prior space reservation since
        /// its creation or the last reset.
        /// </para>
        /// <para>
        /// The state of the <paramref name="commandBuffer"/> must be legal to execute all commands
        /// within the sequence provided by the <see
        /// cref="CmdProcessCommandsInfoNvx.IndirectCommandsLayout"/> member.
        /// </para>
        /// </summary>
        /// <param name="commandBuffer">
        /// The secondary command buffer in which the space for device-generated commands is reserved.
        /// </param>
        /// <param name="reserveSpaceInfo">
        /// The structure containing parameters affecting the reservation of command buffer space.
        /// </param>
        public static void CmdReserveSpaceForCommandsNvx(this CommandBuffer commandBuffer,
            CmdReserveSpaceForCommandsInfoNvx reserveSpaceInfo)
        {
            reserveSpaceInfo.Prepare();
            vkCmdReserveSpaceForCommandsNVX(commandBuffer)(commandBuffer, &reserveSpaceInfo);
        }

        private delegate void vkCmdProcessCommandsNVXDelegate(IntPtr commandBuffer, CmdProcessCommandsInfoNvx.Native* processCommandsInfo);
        private static vkCmdProcessCommandsNVXDelegate vkCmdProcessCommandsNVX(CommandBuffer commandBuffer) => GetProc<vkCmdProcessCommandsNVXDelegate>(commandBuffer, nameof(vkCmdProcessCommandsNVX));

        private delegate void vkCmdReserveSpaceForCommandsNVXDelegate(IntPtr commandBuffer, CmdReserveSpaceForCommandsInfoNvx* reserveSpaceInfo);
        private static vkCmdReserveSpaceForCommandsNVXDelegate vkCmdReserveSpaceForCommandsNVX(CommandBuffer commandBuffer) => GetProc<vkCmdReserveSpaceForCommandsNVXDelegate>(commandBuffer, nameof(vkCmdReserveSpaceForCommandsNVX));

        private static TDelegate GetProc<TDelegate>(CommandBuffer commandBuffer, string name) where TDelegate : class => commandBuffer.Parent.Parent.GetProc<TDelegate>(name);
    }

    /// <summary>
    /// Structure specifying parameters for the generation of commands.
    /// </summary>
    public unsafe struct CmdProcessCommandsInfoNvx
    {
        /// <summary>
        /// The <see cref="ObjectTableNvx"/> to be used for the generation process. Only registered
        /// objects at the time <see cref="CommandBufferExtensions.CmdReserveSpaceForCommandsNvx"/>
        /// is called, will be taken into account for the reservation.
        /// </summary>
        public long ObjectTable;
        /// <summary>
        /// The <see cref="IndirectCommandsLayoutNvx"/> that provides the command sequence to generate.
        /// </summary>
        public long IndirectCommandsLayout;
        /// <summary>
        /// Provides an array of <see cref="IndirectCommandsTokenNvx"/> that reference the input data
        /// for each token command.
        /// </summary>
        public IndirectCommandsTokenNvx[] IndirectCommandsTokens;
        /// <summary>
        /// The maximum number of sequences for which command buffer space will be reserved. If <see
        /// cref="SequencesCountBuffer"/> is 0, this is also the actual number of sequences generated.
        /// </summary>
        public int MaxSequencesCount;
        /// <summary>
        /// Can be the secondary <see cref="CommandBuffer"/> in which the commands should be
        /// recorded. If <see cref="TargetCommandBuffer"/> is <see cref="IntPtr.Zero"/> an implicit
        /// reservation as well as execution takes place on the processing <see cref="CommandBuffer"/>.
        /// </summary>
        public IntPtr TargetCommandBuffer;
        /// <summary>
        /// Can be <see cref="Buffer"/> from which the actual amount of sequences is sourced from as
        /// <see cref="int"/> value.
        /// </summary>
        public long SequencesCountBuffer;
        /// <summary>
        /// The byte offset into <see cref="SequencesCountBuffer"/> where the count value is stored.
        /// </summary>
        public long SequencesCountOffset;
        /// <summary>
        /// Must be set if <see cref="IndirectCommandsLayout"/>'s <see
        /// cref="IndirectCommandsLayoutUsagesNvx.IndexedSequences"/> bit is set and provides the
        /// used sequence indices as <see cref="int"/> array. Otherwise it must be 0.
        /// </summary>
        public long SequencesIndexBuffer;
        /// <summary>
        /// The byte offset into <see cref="SequencesIndexBuffer"/> where the index values start.
        /// </summary>
        public long SequencesIndexOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdProcessCommandsInfoNvx"/> structure.
        /// </summary>
        /// <param name="objectTable">
        /// The <see cref="ObjectTableNvx"/> to be used for the generation process. Only registered
        /// objects at the time <see cref="CommandBufferExtensions.CmdReserveSpaceForCommandsNvx"/>
        /// is called, will be taken into account for the reservation.
        /// </param>
        /// <param name="indirectCommandsLayout">
        /// The <see cref="IndirectCommandsLayoutNvx"/> that provides the command sequence to generate.
        /// </param>
        /// <param name="indirectCommandsTokens">
        /// Provides an array of <see cref="IndirectCommandsTokenNvx"/> that reference the input data
        /// for each token command.
        /// </param>
        /// <param name="maxSequencesCount">
        /// The maximum number of sequences for which command buffer space will be reserved. If <see
        /// cref="SequencesCountBuffer"/> is 0, this is also the actual number of sequences generated.
        /// </param>
        /// <param name="targetCommandBuffer">
        /// Can be the secondary <see cref="CommandBuffer"/> in which the commands should be
        /// recorded. If <c>null</c> an implicit reservation as well as execution takes place on the
        /// processing <see cref="CommandBuffer"/>.
        /// </param>
        /// <param name="sequencesCountBuffer">
        /// Can be <see cref="Buffer"/> from which the actual amount of sequences is sourced from as
        /// <see cref="int"/> value.
        /// </param>
        /// <param name="sequencesCountOffset">
        /// The byte offset into <see cref="SequencesCountBuffer"/> where the count value is stored.
        /// </param>
        /// <param name="sequencesIndexBuffer">
        /// Must be set if <see cref="IndirectCommandsLayout"/>'s <see
        /// cref="IndirectCommandsLayoutUsagesNvx.IndexedSequences"/> bit is set and provides the
        /// used sequence indices as <see cref="int"/> array. Otherwise it must be 0.
        /// </param>
        /// <param name="sequencesIndexOffset">
        /// The byte offset into <see cref="SequencesIndexBuffer"/> where the index values start.
        /// </param>
        public CmdProcessCommandsInfoNvx(ObjectTableNvx objectTable, IndirectCommandsLayoutNvx indirectCommandsLayout,
            IndirectCommandsTokenNvx[] indirectCommandsTokens, int maxSequencesCount = 0, CommandBuffer targetCommandBuffer = null,
            Buffer sequencesCountBuffer = null, long sequencesCountOffset = 0,
            Buffer sequencesIndexBuffer = null, long sequencesIndexOffset = 0)
        {
            ObjectTable = objectTable;
            IndirectCommandsLayout = indirectCommandsLayout;
            IndirectCommandsTokens = indirectCommandsTokens;
            MaxSequencesCount = maxSequencesCount;
            TargetCommandBuffer = targetCommandBuffer;
            SequencesCountBuffer = sequencesCountBuffer;
            SequencesCountOffset = sequencesCountOffset;
            SequencesIndexBuffer = sequencesIndexBuffer;
            SequencesIndexOffset = sequencesIndexOffset;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public long ObjectTable;
            public long IndirectCommandsLayout;
            public int IndirectCommandsTokenCount;
            public IndirectCommandsTokenNvx* IndirectCommandsTokens;
            public int MaxSequencesCount;
            public IntPtr TargetCommandBuffer;
            public long SequencesCountBuffer;
            public long SequencesCountOffset;
            public long SequencesIndexBuffer;
            public long SequencesIndexOffset;
        }

        internal void ToNative(out Native native, IndirectCommandsTokenNvx* indirectCommandsTokens)
        {
            native.Type = StructureType.CmdProcessCommandsInfoNvx;
            native.Next = IntPtr.Zero;
            native.ObjectTable = ObjectTable;
            native.IndirectCommandsLayout = IndirectCommandsLayout;
            native.IndirectCommandsTokenCount = IndirectCommandsTokens?.Length ?? 0;
            native.IndirectCommandsTokens = indirectCommandsTokens;
            native.MaxSequencesCount = MaxSequencesCount;
            native.TargetCommandBuffer = TargetCommandBuffer;
            native.SequencesCountBuffer = SequencesCountBuffer;
            native.SequencesCountOffset = SequencesCountOffset;
            native.SequencesIndexBuffer = SequencesIndexBuffer;
            native.SequencesIndexOffset = SequencesIndexOffset;
        }
    }

    /// <summary>
    /// Structure specifying parameters for the reservation of command buffer space.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IndirectCommandsTokenNvx
    {
        /// <summary>
        /// Specifies the token command type.
        /// </summary>
        public IndirectCommandsTokenTypeNvx TokenType;
        /// <summary>
        /// Specifies the <see cref="VulkanCore.Buffer"/> storing the functional arguments for each
        /// squence. These argumetns can be written by the device.
        /// </summary>
        public long Buffer;
        /// <summary>
        /// Specified an offset into <see cref="Buffer"/> where the arguments start.
        /// </summary>
        public long Offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndirectCommandsTokenNvx"/> structure.
        /// </summary>
        /// <param name="tokenType">Specifies the token command type.</param>
        /// <param name="buffer">
        /// Specifies the <see cref="VulkanCore.Buffer"/> storing the functional arguments for each
        /// squence. These argumetns can be written by the device.
        /// </param>
        /// <param name="offset">
        /// Specified an offset into <see cref="Buffer"/> where the arguments start.
        /// </param>
        public IndirectCommandsTokenNvx(IndirectCommandsTokenTypeNvx tokenType, Buffer buffer, long offset = 0)
        {
            TokenType = tokenType;
            Buffer = buffer;
            Offset = offset;
        }
    }

    /// <summary>
    /// Structure specifying parameters for the reservation of command buffer space.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CmdReserveSpaceForCommandsInfoNvx
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// The <see cref="ObjectTableNvx"/> to be used for the generation process. Only registered
        /// objects at the time <see cref="CommandBufferExtensions.CmdReserveSpaceForCommandsNvx"/>
        /// is called, will be taken into account for the reservation.
        /// </summary>
        public long ObjectTable;
        /// <summary>
        /// The <see cref="IndirectCommandsLayoutNvx"/> that must also be used at generation time.
        /// </summary>
        public long IndirectCommandsLayout;
        /// <summary>
        /// The maximum number of sequences for which command buffer space will be reserved.
        /// </summary>
        public int MaxSequencesCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdReserveSpaceForCommandsInfoNvx"/> structure.
        /// </summary>
        /// <param name="objectTable">
        /// The <see cref="ObjectTableNvx"/> to be used for the generation process. Only registered
        /// objects at the time <see cref="CommandBufferExtensions.CmdReserveSpaceForCommandsNvx"/>
        /// is called, will be taken into account for the reservation.
        /// </param>
        /// <param name="indirectCommandsLayout">
        /// The <see cref="IndirectCommandsLayoutNvx"/> that must also be used at generation time.
        /// </param>
        /// <param name="maxSequencesCount">
        /// The maximum number of sequences for which command buffer space will be reserved.
        /// </param>
        public CmdReserveSpaceForCommandsInfoNvx(ObjectTableNvx objectTable,
            IndirectCommandsLayoutNvx indirectCommandsLayout, int maxSequencesCount)
        {
            Type = StructureType.CmdReserveSpaceForCommandsInfoNvx;
            Next = IntPtr.Zero;
            ObjectTable = objectTable;
            IndirectCommandsLayout = indirectCommandsLayout;
            MaxSequencesCount = maxSequencesCount;
        }

        internal void Prepare()
        {
            Type = StructureType.CmdReserveSpaceForCommandsInfoNvx;
        }
    }
}
