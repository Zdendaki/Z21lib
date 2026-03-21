using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class AccessoryInfoMessage(Address address, byte state, AccessoryStatus status) : Message(MessageType.LAN_X_EXT_ACCESSORY_INFO)
{
    public required Address Address { get; init; } = address;

    public required byte State { get; init; } = state;

    public required AccessoryStatus Status { get; init; } = status;

    internal static AccessoryInfoMessage Parse(ReadOnlySpan<byte> message)
    {
        return new AccessoryInfoMessage(new Address(message[5], message[6]), message[7], (AccessoryStatus)message[8]);
    }
}
