using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class FirmwareVersionMessage : Message
    {
        public byte Major { get; init; }

        public byte Minor { get; init; }

        public FirmwareVersionMessage(byte major, byte minor) : base(MessageType.LAN_X_GET_FIRMWARE_VERSION)
        {
            Major = major;
            Minor = minor;
        }

        internal static FirmwareVersionMessage Parse(byte[] message)
        {
            return new FirmwareVersionMessage(message[6].FromBCD(), message[7].FromBCD());
        }
    }
}
