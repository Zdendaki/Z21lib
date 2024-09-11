using Z21lib.Endianity;
using Z21lib.Enums;

#nullable disable
namespace Z21lib.Messages
{
    public class ZlinkHwInfoMessage : Message
    {
        public ushort HwID { get; init; }

        public Version Version { get; init; }

        public string MacAddress { get; init; }

        public string Name { get; init; }

        public ZlinkHwInfoMessage() : base(MessageType.LAN_ZLINK_GET_HWINFO)
        {
        }

        public static ZlinkHwInfoMessage Parse(byte[] message)
        {
            return new()
            {
                HwID = LE.ToUInt16(message, 4),
                Version = new Version(message[6], message[7], LE.ToUInt16(message, 8)),
                MacAddress = Utils.ReadString(message, 10, 8),
                Name = Utils.ReadString(message, 28, 33)
            };
        }
    }
}
