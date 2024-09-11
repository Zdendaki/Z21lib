using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class LoconetDispatchMessage : Message
    {
        public LocoAddress LocoAddress { get; init; }

        /// <summary>
        /// <see langword="true"/> - DISPATCH_PUT was executed successfuly, <see langword="false"/> - DISPATCH_PUT failed for given address
        /// </summary>
        public bool Result { get; init; }

        public byte Slot { get; init; }

        public LoconetDispatchMessage() : base(MessageType.LAN_LOCONET_DISPATCH_ADDR)
        {

        }

        public static LoconetDispatchMessage Parse(byte[] message)
        {
            return new()
            {
                LocoAddress = new LocoAddress(message[5], message[4]),
                Result = message[6] > 0,
                Slot = message[6]
            };
        }
    }
}
