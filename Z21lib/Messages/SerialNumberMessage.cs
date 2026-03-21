using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class SerialNumberMessage(uint serial) : Message(MessageType.LAN_GET_SERIAL_NUMBER)
{
    public required uint SerialNumber { get; init; } = serial;
}
