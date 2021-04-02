namespace Z21lib.Messages
{
    public class FlagsMessage : Message
    {
        public int Flags { get; set; }

        public FlagsMessage(int flags, MessageType type) : base(type)
        {
            Flags = flags;
        }
    }
}
