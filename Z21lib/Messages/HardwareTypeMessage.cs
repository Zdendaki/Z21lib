using System.Diagnostics.CodeAnalysis;
using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class HardwareTypeMessage(HardwareType type, byte major, byte minor) : Message(MessageType.LAN_GET_HWINFO)
{
    public required HardwareType HardwareType { get; init; } = type;

    public required byte VersionMajor { get; init; } = major;

    public required byte VersionMinor { get; init; } = minor;

    internal static HardwareTypeMessage Parse(ReadOnlySpan<byte> message)
    {
        HardwareType hw = (HardwareType)LE.ToUInt32(message.Slice(4));
        return new HardwareTypeMessage(hw, message[9].FromBCD(), message[8].FromBCD());
    }
}
