using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class LocoModeMessage(LocoAddress address, DecoderMode mode) : Message(MessageType.LAN_GET_LOCOMODE)
{
    public required LocoAddress Address { get; init; } = address;

    public required DecoderMode Mode { get; init; } = mode;

    internal static LocoModeMessage Parse(ReadOnlySpan<byte> message)
    {
        return new LocoModeMessage(new LocoAddress(message[4], message[5]), (DecoderMode)message[6]);
    }
}
