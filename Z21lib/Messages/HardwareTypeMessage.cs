using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class HardwareTypeMessage : Message
    {
        public HardwareType HardwareType { get; init; }

        public byte VersionMajor { get; init; }

        public byte VersionMinor { get; init; }

        public HardwareTypeMessage(HardwareType type, byte major, byte minor) : base(MessageType.LAN_GET_HWINFO)
        {
            HardwareType = type;
            VersionMajor = major;
            VersionMinor = minor;
        }

        internal static HardwareTypeMessage Parse(byte[] message)
        {
            HardwareType hw = (HardwareType)LE.ToUInt32(message, 4);
            return new HardwareTypeMessage(hw, message[9].FromBCD(), message[8].FromBCD());
        }
    }
}
