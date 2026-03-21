using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;


[method: SetsRequiredMembers]

public class XBusVersionMessage(int version, byte station) : Message(MessageType.LAN_X_GET_VERSION)
{
    public required float Version { get; set; } = version / 10f;

    public required byte Station { get; set; } = station;

    internal static XBusVersionMessage Parse(ReadOnlySpan<byte> message)
    {
        return new XBusVersionMessage(message[6].FromBCD(), message[7]);
    }
}
