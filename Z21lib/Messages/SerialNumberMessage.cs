namespace Z21lib.Messages
{
    public class SerialNumberMessage : Message
    {
        public int SerialNumber { get; set; }

        public SerialNumberMessage(int serial) : base(MessageType.LAN_GET_SERIAL_NUMBER)
        {
            SerialNumber = serial;
        }
    }
}
