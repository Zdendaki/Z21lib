using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class TrackStatusChangedMessage(CentralState status) : Message(MessageType.LAN_X_STATUS_CHANGED)
{
    public required CentralState Status { get; init; } = status;
}
