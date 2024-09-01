using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class NotImplementedMessage : Message
    {
        byte[] binary;

        public string Data { get => BitConverter.ToString(binary).ToUpper().Replace('-', ' '); }

        public NotImplementedMessage(byte[] data) : base(MessageType.NOT_IMPLEMENTED)
        {
            binary = data;
        }
    }
}
