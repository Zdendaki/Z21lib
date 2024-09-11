using System.Text;
using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class CanBoosterDescriptionMessage : Message
    {
        public ushort NID { get; init; }

        public string Description { get; init; } = string.Empty;

        public CanBoosterDescriptionMessage() : base(MessageType.LAN_CAN_DEVICE_GET_DESCRIPTION)
        {

        }

        public static CanBoosterDescriptionMessage Parse(byte[] message)
        {
            return new CanBoosterDescriptionMessage
            {
                NID = LE.ToUInt16(message, 4),
                Description = Utils.ReadString(message, 6, 16)
            };
        }
    }
}
