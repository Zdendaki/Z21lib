using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class LanSwitchDecoderSystemStateMessage : Message
    {
        public short Current { get; init; }

        public short FilteredCurrent { get; init; }

        public ushort Voltage { get; init; }

        public DCentralState State { get; init; }

        public DCentralStateEx StateEx { get; init; }

        public OutputState[] Outputs { get; init; } = [];

        public OutputConfig[] Configs { get; init; } = [];

        public byte[] OutputDimms { get; init; } = [];

        public ushort Address1 { get; init; }

        public ushort Address2 { get; init; }

        public byte Dimmed { get; init; }

        public LanSwitchDecoderSystemStateMessage() : base(MessageType.LAN_DECODER_SYSTEMSTATE_DATACHANGED_SWITCH)
        {
        }

        internal static LanSwitchDecoderSystemStateMessage Parse(ReadOnlySpan<byte> data)
        {
            return new LanSwitchDecoderSystemStateMessage
            {
                Current = LE.ToInt16(data.Slice(4)),
                FilteredCurrent = LE.ToInt16(data.Slice(6)),
                Voltage = LE.ToUInt16(data.Slice(8)),
                State = (DCentralState)data[10],
                StateEx = (DCentralStateEx)data[11],
                Outputs = Utils.GetEnums<OutputState>(data.Slice(12, 8)),
                Configs = Utils.GetEnums<OutputConfig>(data.Slice(20, 8)),
                OutputDimms = data.Slice(28, 8).ToArray(),
                Address1 = LE.ToUInt16(data.Slice(36)),
                Address2 = LE.ToUInt16(data.Slice(38)),
                // Reserved 1
                Dimmed = data[46]
                // Reserved 2
            };
        }
    }
}
