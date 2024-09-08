using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class SerialNumberMessage : Message
    {
        public uint SerialNumber { get; set; }

        public SerialNumberMessage(uint serial) : base(MessageType.LAN_GET_SERIAL_NUMBER)
        {
            SerialNumber = serial;
        }
    }
}
