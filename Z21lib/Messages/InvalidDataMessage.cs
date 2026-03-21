using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class InvalidDataMessage(byte[] blob) : Message(MessageType.INVALID_DATA)
{
    public required byte[] Blob { get; init; } = blob;

    public string Data => BitConverter.ToString(Blob).ToUpperInvariant().Replace('-', ' ');
}
