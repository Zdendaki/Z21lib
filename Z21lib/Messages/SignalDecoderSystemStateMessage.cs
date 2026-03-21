using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages;

public class SignalDecoderSystemStateMessage() : Message(MessageType.LAN_DECODER_SYSTEMSTATE_DATACHANGED_SIGNAL)
{
    public required ushort Current { get; init; }

    public required ushort FilteredCurrent { get; init; }

    public required ushort Voltage { get; init; }

    public required DCentralState State { get; init; }

    public required DCentralStateEx StateEx { get; init; }

    public required byte[] OutputStates { get; init; }

    public required byte[] BlinkStates { get; init; }

    public required byte[] SignalDccExt { get; init; }

    public required byte[] SignalCurrentAspect { get; init; }

    public required byte SignalCount { get; init; }

    public required byte[] SignalConfig { get; init; }

    public required byte[] SignalInitAaspect { get; init; }

    public required ushort Address { get; init; }

    internal static SignalDecoderSystemStateMessage Parse(ReadOnlySpan<byte> data)
    {
        return new SignalDecoderSystemStateMessage
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
