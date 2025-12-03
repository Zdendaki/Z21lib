namespace Z21lib.Enums
{
    [Flags]
    public enum OutputState
    {
        Unknown = 0,
        RedActive = 0x11,
        RedInactive = 0x01,
        GreenActive = 0x12,
        GreenInactive = 0x02
    }

    public enum OutputConfig
    {
        /// <summary>
        /// Pulse operation (default)
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Alternating blinker
        /// </summary>
        Blinker = 1,
        /// <summary>
        /// Alternating blinker with fade-in and fade-out
        /// </summary>
        BlinkSm = 2,
        /// <summary>
        /// Momentary operation like 10775
        /// </summary>
        D10775 = 3,
        /// <summary>
        /// Continuous operation (e.g. for lightning)
        /// </summary>
        K84 = 4,
        /// <summary>
        /// Continuous operation with fade-in and fade-out
        /// </summary>
        K84Sm = 5
    }
}
