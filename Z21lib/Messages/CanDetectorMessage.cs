using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages;

public class CanDetectorMessage() : Message(MessageType.LAN_CAN_DETECTOR)
{
    public required ushort NID { get; init; }

    public required ushort Address { get; init; }

    public required byte Port { get; init; }

    /// <summary>
    /// See Z21 LAN specification for more info
    /// </summary>
    public required byte CanType { get; init; }

    /// <summary>
    /// See Z21 LAN specification for more info about data
    /// </summary>
    public required ushort Value1 { get; init; }

    /// <summary>
    /// See Z21 LAN specification for more info about data
    /// </summary>
    public required ushort Value2 { get; init; }

    internal static CanDetectorMessage Parse(ReadOnlySpan<byte> message)
    {
        return new()
        {
            NID = LE.ToUInt16(message.Slice(4)),
            Address = LE.ToUInt16(message.Slice(6)),
            Port = message[8],
            CanType = message[9],
            Value1 = LE.ToUInt16(message.Slice(10)),
            Value2 = LE.ToUInt16(message.Slice(12))
        };
    }
}
