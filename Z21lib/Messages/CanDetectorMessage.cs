using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class CanDetectorMessage : Message
    {
        public ushort NID { get; init; }

        public ushort Address { get; init; }

        public byte Port { get; init; }

        /// <summary>
        /// See Z21 LAN specification for more info
        /// </summary>
        public byte CanType { get; init; }

        /// <summary>
        /// See Z21 LAN specification for more info about data
        /// </summary>
        public ushort Value1 { get; init; }

        /// <summary>
        /// See Z21 LAN specification for more info about data
        /// </summary>
        public ushort Value2 { get; init; }

        public CanDetectorMessage() : base(MessageType.LAN_CAN_DETECTOR)
        {

        }

        public static CanDetectorMessage Parse(byte[] message)
        {
            return new()
            {
                NID = LE.ToUInt16(message, 4),
                Address = LE.ToUInt16(message, 6),
                Port = message[8],
                CanType = message[9],
                Value1 = LE.ToUInt16(message, 10),
                Value2 = LE.ToUInt16(message, 12)
            };
        }
    }
}
