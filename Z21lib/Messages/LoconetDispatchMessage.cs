using Z21lib.Enums;

namespace Z21lib.Messages;

public class LoconetDispatchMessage() : Message(MessageType.LAN_LOCONET_DISPATCH_ADDR)
{
    public required LocoAddress LocoAddress { get; init; }

    /// <summary>
    /// <see langword="true"/> - DISPATCH_PUT was executed successfuly, <see langword="false"/> - DISPATCH_PUT failed for given address
    /// </summary>
    public required bool Result { get; init; }

    public required byte Slot { get; init; }

    internal static LoconetDispatchMessage Parse(ReadOnlySpan<byte> message)
    {
        return new()
        {
            LocoAddress = new LocoAddress(message[5], message[4]),
            Result = message[6] > 0,
            Slot = message[6]
        };
    }
}
