using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class TurnoutModeMessage : Message
    {
        public Address Address { get; init; }

        public DecoderMode Mode { get; init; }

        public TurnoutModeMessage(Address address, DecoderMode mode) : base(MessageType.LAN_GET_TURNOUTMODE)
        {
            Address = address;
            Mode = mode;
        }

        internal static TurnoutModeMessage Parse(byte[] message)
        {
            return new TurnoutModeMessage(new Address(message[4], message[5]), (DecoderMode)message[6]);
        }
    }
}
