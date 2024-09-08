namespace Z21lib.Messages
{
    public class CVResultMessage : Message
    {
        public Address Address { get; init; }

        public byte Value { get; init; }

        public CVResultMessage(byte msb, byte lsb, byte value) : base(Enums.MessageType.LAN_X_CV_RESULT)
        {
            Address = new(msb, lsb);
            Value = value;
        }

        internal static CVResultMessage Parse(byte[] message)
        {
            return new(message[6], message[7], message[8]);
        }
    }
}
