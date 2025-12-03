using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages
{
    internal class LanSignalDecoderSystemStateMessage : Message
    {
        public ushort Current { get; init; }

        public ushort FilteredCurrent { get; init; }

        public ushort Voltage { get; init; }

        public DCentralState State { get; init; }

        public DCentralStateEx StateEx { get; init; }

        public byte[] OutputStates { get; init; } = [];

        public byte[] BlinkStates { get; init; } = [];

        public byte[] SignalDccExt { get; init; } = [];

        public byte[] SignalCurrentAspect { get; init; } = [];

        public byte SignalCount { get; init; }

        public byte[] SignalConfig { get; init; } = [];

        public byte[] SignalInitAaspect { get; init; } = [];

        public ushort Address { get; init; }

        public LanSignalDecoderSystemStateMessage() : base(MessageType.LAN_DECODER_SYSTEMSTATE_DATACHANGED_SIGNAL)
        {
        }

        internal static LanSignalDecoderSystemStateMessage Parse(ReadOnlySpan<byte> data)
        {
            return new LanSignalDecoderSystemStateMessage
            {
                Current = LE.ToUInt16(data.Slice(4)),
                FilteredCurrent = LE.ToUInt16(data.Slice(6)),
                Voltage = LE.ToUInt16(data.Slice(8)),
                State = (DCentralState)data[10],
                StateEx = (DCentralStateEx)data[11],
                OutputStates = data.Slice(12, 2).ToArray(),
                BlinkStates = data.Slice(14, 2).ToArray(),
                SignalDccExt = data.Slice(16, 4).ToArray(),
                SignalCurrentAspect = data.Slice(20, 4).ToArray(),
                // Reserved 1
                SignalCount = data[27],
                SignalConfig = data.Slice(28, 4).ToArray(),
                SignalInitAaspect = data.Slice(32, 4).ToArray(),
                Address = LE.ToUInt16(data.Slice(36))
                // Reserved 2
            };
        }
    }
}
