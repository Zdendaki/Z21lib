using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class AccessoryInfoMessage : Message
    {
        public Address Address { get; init; }

        public byte State { get; init; }

        public AccessoryStatus Status { get; init; }

        public AccessoryInfoMessage(Address address, byte state, AccessoryStatus status) : base(MessageType.LAN_X_EXT_ACCESSORY_INFO)
        {
            Address = address;
            State = state;
            Status = status;
        }

        public static AccessoryInfoMessage Parse(ReadOnlySpan<byte> message)
        {
            return new AccessoryInfoMessage(new Address(message[5], message[6]), message[7], (AccessoryStatus)message[8]);
        }
    }
}
