using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

public class LoconetMessage : Message
{
    public required byte[] LoconetData { get; init; }

    [SetsRequiredMembers]
    internal LoconetMessage(MessageType type, ReadOnlySpan<byte> message) : base(type)
    {
        if (type != MessageType.LAN_LOCONET_Z21_RX && type != MessageType.LAN_LOCONET_Z21_TX && type != MessageType.LAN_LOCONET_FROM_LAN)
            throw new ArgumentException("Invalid message type for this message handler!", nameof(type));

        LoconetData = message.Slice(4).ToArray();
    }
}
