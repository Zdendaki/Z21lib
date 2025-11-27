using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class NotImplementedMessage : Message
    {
        public byte[] Binary { get; init; }

        public string Data => BitConverter.ToString(Binary).ToUpperInvariant().Replace('-', ' ');

        public NotImplementedMessage(byte[] data) : base(MessageType.NOT_IMPLEMENTED)
        {
            Binary = data;
        }

        public NotImplementedMessage(ReadOnlySpan<byte> data) : base(MessageType.NOT_IMPLEMENTED)
        {
            Binary = data.ToArray();
        }
    }
}
