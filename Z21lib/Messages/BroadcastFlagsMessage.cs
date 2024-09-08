using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class BroadcastFlagsMessage : Message
    {
        public BroadcastFlags BroadcastFlags { get; init; }

        public BroadcastFlagsMessage(BroadcastFlags flags) : base(MessageType.LAN_GET_BROADCASTFLAGS)
        {
            BroadcastFlags = flags;
        }

        internal static BroadcastFlagsMessage Parse(byte[] message)
        {
            return new BroadcastFlagsMessage((BroadcastFlags)LE.ToUInt32(message, 4));
        }
    }
}
