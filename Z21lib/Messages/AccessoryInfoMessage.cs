namespace Z21lib.Messages
{
    public class AccessoryInfoMessage : Message
    {
        public AccessoryAddress Address { get; set; }

        public byte State { get; set; }

        public AccessoryStatus Status { get; set; }

        public AccessoryInfoMessage(AccessoryAddress address, byte state, AccessoryStatus status) : base(MessageType.LAN_X_EXT_ACCESSORY_INFO)
        {
            Address = address;
            State = state;
            Status = status;
        }
    }

    public enum AccessoryStatus : byte
    {
        DataValid = 0x00,
        DataUnknown = 0xFF
    }
}
