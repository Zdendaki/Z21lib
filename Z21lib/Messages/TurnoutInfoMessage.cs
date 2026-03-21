using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class TurnoutInfoMessage(Address address, TurnoutState state) : Message(MessageType.LAN_X_TURNOUT_INFO)
{
    public required Address Address { get; init; } = address;

    public required TurnoutState State { get; init; } = state;

    internal static TurnoutInfoMessage Parse(ReadOnlySpan<byte> message)
    {
        return new(new Address(message[5], message[6]), (TurnoutState)message[7]);
    }
}
