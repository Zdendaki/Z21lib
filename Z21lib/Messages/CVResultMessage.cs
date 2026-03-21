using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class CVResultMessage(byte msb, byte lsb, byte value) : Message(MessageType.LAN_X_CV_RESULT)
{
    public required Address Address { get; init; } = new(msb, lsb);

    public required byte Value { get; init; } = value;

    internal static CVResultMessage Parse(ReadOnlySpan<byte> message)
    {
        return new(message[6], message[7], message[8]);
    }
}
