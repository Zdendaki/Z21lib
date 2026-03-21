using System.Diagnostics.CodeAnalysis;
using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class RailComDataMessage(LocoAddress address, uint receiveCounter, ushort errorCounter, RailComOptions options, byte speed, byte qos) : Message(MessageType.LAN_RAILCOM_DATACHANGED)
{
    public required LocoAddress LocoAddress { get; init; } = address;

    public required uint ReceiveCounter { get; init; } = receiveCounter;

    public required ushort ErrorCounter { get; init; } = errorCounter;

    public required RailComOptions Options { get; init; } = options;

    public required byte Speed { get; init; } = speed;

    public required byte QoS { get; init; } = qos;

    internal static RailComDataMessage Parse(ReadOnlySpan<byte> message)
    {
        return new RailComDataMessage(new(message[5], message[4]), LE.ToUInt32(message.Slice(6)), LE.ToUInt16(message.Slice(10)), (RailComOptions)message[13], message[14], message[15]);
    }
}
