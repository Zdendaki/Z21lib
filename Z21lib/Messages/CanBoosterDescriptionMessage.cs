using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages;

public class CanBoosterDescriptionMessage() : Message(MessageType.LAN_CAN_DEVICE_GET_DESCRIPTION)
{
    public required ushort NID { get; init; }

    public required string Description { get; init; }

    internal static CanBoosterDescriptionMessage Parse(ReadOnlySpan<byte> message)
    {
        return new CanBoosterDescriptionMessage
        {
            NID = LE.ToUInt16(message.Slice(4)),
            Description = Utils.ReadString(message.Slice(6), 16)
        };
    }
}
