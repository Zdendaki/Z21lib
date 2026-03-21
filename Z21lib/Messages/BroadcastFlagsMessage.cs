using System.Diagnostics.CodeAnalysis;
using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class BroadcastFlagsMessage(BroadcastFlags flags) : Message(MessageType.LAN_GET_BROADCASTFLAGS)
{
    public required BroadcastFlags BroadcastFlags { get; init; } = flags;

    internal static BroadcastFlagsMessage Parse(ReadOnlySpan<byte> message)
    {
        return new BroadcastFlagsMessage((BroadcastFlags)LE.ToUInt32(message.Slice(4)));
    }
}
