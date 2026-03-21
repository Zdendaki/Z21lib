using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class TurnoutModeMessage(Address address, DecoderMode mode) : Message(MessageType.LAN_GET_TURNOUTMODE)
{
    public required Address Address { get; init; } = address;

    public required DecoderMode Mode { get; init; } = mode;

    internal static TurnoutModeMessage Parse(ReadOnlySpan<byte> message)
    {
        return new TurnoutModeMessage(new Address(message[4], message[5]), (DecoderMode)message[6]);
    }
}
