using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class LocoModeMessage : Message
    {
        public LocoModeMessage() : base(MessageType.LAN_GET_LOCOMODE)
        {

        }
    }
}
