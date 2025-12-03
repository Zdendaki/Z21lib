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

        internal static LanBoosterDescriptionMessage Parse(ReadOnlySpan<byte> message)
        {
            return new(Utils.ReadString(message.Slice(4), 32));
        }
    }
}
