using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class DeviceCodeMessage : Message
    {
        public DeviceCode Code { get; set; }

        public DeviceCodeMessage(DeviceCode code) : base(MessageType.LAN_GET_CODE)
        {
            Code = code;
        }
    }

    public enum DeviceCode : byte
    {
        Z21_NO_LOCK = 0x00,
        Z21_START_LOCKED = 0x01,
        Z21_START_UNLOCKED = 0x02
    }
}
