using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    [Flags]
    public enum BroadcastFlags : uint
    {
        EnableBroadcast = 0x00000001,
        RBus = 0x00000002,
        RailCom = 0x00000004,
        Z21 = 0x00000100,
        LocoInfo = 0x00010000,
        LocoNet = 0x01000000,
        LocoNetLocos = 0x02000000,
        LocoNetSwitches = 0x04000000,
        LocoNetOccupancyDetector = 0x08000000,
        RailComChanges = 0x00040000,
        CANBusChanges = 0x00080000,
        CANBusBooster = 0x00020000
    }
}
