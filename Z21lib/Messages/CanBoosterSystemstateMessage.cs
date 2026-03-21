using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages;

public class CanBoosterSystemStateMessage() : Message(MessageType.LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD)
{
    public required ushort NID { get; init; }

    public required ushort OutputPort { get; init; }

    public required BoosterState State { get; init; }

    /// <summary>
    /// [mV]
    /// </summary>
    public required ushort Voltage { get; init; }

    /// <summary>
    /// [mA]
    /// </summary>
    public required ushort Current { get; init; }

    internal static CanBoosterSystemStateMessage Parse(ReadOnlySpan<byte> message)
    {
        return new CanBoosterSystemStateMessage
        {
            NID = LE.ToUInt16(message.Slice(4)),
            OutputPort = LE.ToUInt16(message.Slice(6)),
            State = (BoosterState)LE.ToUInt16(message.Slice(8)),
            Voltage = LE.ToUInt16(message.Slice(10)),
            Current = LE.ToUInt16(message.Slice(12))
        };
    }
}
