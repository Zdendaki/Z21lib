using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class BroadcastFlagsMessage : Message
    {
        public BroadcastFlags BroadcastFlags { get; set; }

        public BroadcastFlagsMessage(BroadcastFlags flags) : base(MessageType.LAN_GET_BROADCASTFLAGS)
        {
            BroadcastFlags = flags;
        }
    }
}
