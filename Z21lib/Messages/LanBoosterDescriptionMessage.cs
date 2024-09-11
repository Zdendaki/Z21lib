using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class LanBoosterDescriptionMessage : Message
    {
        public string Name { get; init; }

        public LanBoosterDescriptionMessage(string name) : base(MessageType.LAN_BOOSTER_GET_DESCRIPTION)
        {
            Name = name;
        }

        public static LanBoosterDescriptionMessage Parse(byte[] message)
        {
            return new(Utils.ReadString(message, 4, 32));
        }
    }
}
