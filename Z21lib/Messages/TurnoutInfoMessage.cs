namespace Z21lib.Messages
{
    public class TurnoutInfoMessage : Message
    {
        public AccessoryAddress Address { get; set; }

        public TurnoutState State { get; set; }

        public TurnoutInfoMessage(AccessoryAddress address, TurnoutState state) : base(MessageType.LAN_X_TURNOUT_INFO)
        {
            Address = address;
            State = state;
        }
    }

    public enum TurnoutState : byte
    {
        NotSwitched = 0x0,
        SwitchedStraight = 0x1,
        SwitchedReverse = 0x2,
        Invalid = 0x3
    }
}
