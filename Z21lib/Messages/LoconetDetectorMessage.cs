using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class LoconetDetectorMessage : Message
    {
        public LoconetFeedbackType FeedbackType { get; init; }

        public Address FeedbackAddress { get; init; }

        /// <summary>
        /// See LAN protocol specification for more info about data
        /// </summary>
        public byte[] Data { get; init; } = [];

        public LoconetDetectorMessage() : base(MessageType.LAN_LOCONET_DETECTOR)
        {

        }

        public static LoconetDetectorMessage Parse(ReadOnlySpan<byte> message)
        {
            byte dataLength;
            LoconetFeedbackType type = (LoconetFeedbackType)message[4];
            switch (type)
            {
                case LoconetFeedbackType.Uhlenbrock:
                case LoconetFeedbackType.LISSYBlockStatus:
                    dataLength = 1;
                    break;
                case LoconetFeedbackType.TransponderEntersBlock:
                case LoconetFeedbackType.TransponderExitsBlock:
                case LoconetFeedbackType.LISSYSpeed:
                    dataLength = 2;
                    break;
                case LoconetFeedbackType.LISSYLocoAddress:
                    dataLength = 3;
                    break;
                default:
                    throw new Exception("Unknown LoconetFeedbackType");
            }

            return new()
            {
                FeedbackType = type,
                FeedbackAddress = new Address(message[6], message[5]),
                Data = message.Slice(7, dataLength).ToArray()
            };
        }
    }
}
