using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages;

public class ZlinkHwInfoMessage() : Message(MessageType.LAN_ZLINK_GET_HWINFO)
{
    public required ushort HwID { get; init; }

    public required Version Version { get; init; }

    public required string MacAddress { get; init; }

    public required string Name { get; init; }

    internal static ZlinkHwInfoMessage Parse(ReadOnlySpan<byte> message)
    {
        return new()
        {
            HwID = LE.ToUInt16(message.Slice(4)),
            Version = new Version(message[6], message[7], LE.ToUInt16(message.Slice(8))),
            MacAddress = Utils.ReadString(message.Slice(10), 8),
            Name = Utils.ReadString(message.Slice(28), 32)
        };
    }
}
