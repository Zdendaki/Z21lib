using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class CanBoosterSystemstateMessage : Message
    {
        public ushort NID { get; init; }

        public ushort OutputPort { get; init; }

        public BoosterState State { get; init; }

        /// <summary>
        /// [mV]
        /// </summary>
        public ushort Voltage { get; init; }

        /// <summary>
        /// [mA]
        /// </summary>
        public ushort Current { get; init; }

        public CanBoosterSystemstateMessage() : base(MessageType.LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD)
        {
        }

        public static CanBoosterSystemstateMessage Parse(byte[] message)
        {
            return new CanBoosterSystemstateMessage
            {
                NID = LE.ToUInt16(message, 4),
                OutputPort = LE.ToUInt16(message, 6),
                State = (BoosterState)LE.ToUInt16(message, 8),
                Voltage = LE.ToUInt16(message, 10),
                Current = LE.ToUInt16(message, 12)
            };
        }
    }
}
