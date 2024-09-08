using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class LoconetMessage : Message
    {
        public byte[] LoconetData { get; init; }

        public LoconetMessage(MessageType type, byte[] message) : base(type)
        {
            if (type != MessageType.LAN_LOCONET_Z21_RX || type != MessageType.LAN_LOCONET_Z21_TX || type != MessageType.LAN_LOCONET_FROM_LAN)
                throw new ArgumentException("Invalid message type for this message handler!", nameof(type));

            LoconetData = message.SubArray(4, message.Length - 4);
        }
    }
}
