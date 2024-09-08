using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class TurnoutInfoMessage : Message
    {
        public Address Address { get; init; }

        public TurnoutState State { get; init; }

        public TurnoutInfoMessage(Address address, TurnoutState state) : base(MessageType.LAN_X_TURNOUT_INFO)
        {
            Address = address;
            State = state;
        }

        internal static TurnoutInfoMessage Parse(byte[] message)
        {
            return new(new Address(message[5], message[6]), (TurnoutState)message[7]);
        }
    }
}
