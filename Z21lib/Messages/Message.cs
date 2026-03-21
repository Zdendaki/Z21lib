using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

public class Message(MessageType type)
{
    public MessageType Type { get; init; } = type;
}
