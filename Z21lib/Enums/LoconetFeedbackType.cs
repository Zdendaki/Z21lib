namespace Z21lib.Enums
{
    public enum LoconetFeedbackType : byte
    {
        Uhlenbrock = 0x01,
        TransponderEntersBlock = 0x02,
        TransponderExitsBlock = 0x03,
        LISSYLocoAddress = 0x10,
        LISSYBlockStatus = 0x11,
        LISSYSpeed = 0x12
    }
}
