using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class FirmwareVersionMessage(byte major, byte minor) : Message(MessageType.LAN_X_GET_FIRMWARE_VERSION)
{
    public required byte Major { get; init; } = major;

    public required byte Minor { get; init; } = minor;

    internal static FirmwareVersionMessage Parse(ReadOnlySpan<byte> message)
    {
        return new FirmwareVersionMessage(message[6].FromBCD(), message[7].FromBCD());
    }
}
