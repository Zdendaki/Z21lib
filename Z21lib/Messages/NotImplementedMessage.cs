using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

public class NotImplementedMessage : Message
{
    public required byte[] Blob { get; init; }

    public string Data => BitConverter.ToString(Blob).ToUpperInvariant().Replace('-', ' ');

    [SetsRequiredMembers]
    public NotImplementedMessage(byte[] blob) : base(MessageType.NOT_IMPLEMENTED)
    {
        Blob = blob;
    }

    [SetsRequiredMembers]
    internal NotImplementedMessage(ReadOnlySpan<byte> data) : base(MessageType.NOT_IMPLEMENTED)
    {
        Blob = data.ToArray();
    }
}
