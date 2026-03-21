using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class LanBoosterDescriptionMessage(string name) : Message(MessageType.LAN_BOOSTER_GET_DESCRIPTION)
{
    public required string Name { get; init; } = name;

    internal static LanBoosterDescriptionMessage Parse(ReadOnlySpan<byte> message)
    {
        return new(Utils.ReadString(message.Slice(4), 32));
    }
}
