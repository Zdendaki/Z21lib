using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class LocoModeMessage : Message
    {
        public LocoAddress Address { get; init; }

        public DecoderMode Mode { get; init; }

        public LocoModeMessage(LocoAddress address, DecoderMode mode) : base(MessageType.LAN_GET_LOCOMODE)
        {
            Address = address;
            Mode = mode;
        }

        internal static LocoModeMessage Parse(byte[] message)
        {
            return new LocoModeMessage(new LocoAddress(message[4], message[5]), (DecoderMode)message[6]);
        }
    }
}
