namespace Z21lib.Messages
{
    public class Message
    {
        public MessageType Type { get; set; }

        public Message(MessageType type)
        {
            Type = type;
        }
    }
}
