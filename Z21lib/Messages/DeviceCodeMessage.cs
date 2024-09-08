using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class DeviceCodeMessage : Message
    {
        public DeviceCode Code { get; init; }

        public DeviceCodeMessage(DeviceCode code) : base(MessageType.LAN_GET_CODE)
        {
            Code = code;
        }
    }

    public enum DeviceCode : byte
    {
        /// <summary>
        /// All features permitted
        /// </summary>
        Z21_NO_LOCK = 0x00,
        /// <summary>
        /// Z21 start: driving and switching is blocked
        /// </summary>
        Z21_START_LOCKED = 0x01,
        /// <summary>
        /// Z21 start: driving and switching is permitted
        /// </summary>
        Z21_START_UNLOCKED = 0x02
    }
}
