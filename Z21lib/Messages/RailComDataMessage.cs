using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class RailComDataMessage : Message
    {
        public LocoAddress LocoAddress { get; init; }

        public uint ReceiveCounter { get; init; }

        public ushort ErrorCounter { get; init; }

        public RailComOptions Options { get; init; }

        public byte Speed { get; init; }

        public byte QoS { get; init; }

        public RailComDataMessage(LocoAddress address, uint receiveCounter, ushort errorCounter, RailComOptions options, byte speed, byte qos) : base(MessageType.LAN_RAILCOM_DATACHANGED)
        {
            LocoAddress = address;
            ReceiveCounter = receiveCounter;
            ErrorCounter = errorCounter;
            Options = options;
            Speed = speed;
            QoS = qos;
        }

        internal static RailComDataMessage Parse(byte[] message)
        {
            return new RailComDataMessage(new(message[5], message[4]), LE.ToUInt32(message, 6), LE.ToUInt16(message, 10), (RailComOptions)message[13], message[14], message[15]);
        }
    }
}
