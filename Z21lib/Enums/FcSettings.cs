namespace Z21lib.Enums
{
    [Flags]
    public enum FcSettings : byte
    {
        /// <summary>
        /// Activate output on LocoNet (polled)
        /// </summary>
        FastClockLocoNetEn = 0x01,
        /// <summary>
        /// Activate broadcast on XBUS
        /// </summary>
        FastClockXBUSEn = 0x02,
        /// <summary>
        /// Activate DCC broadcast on main track
        /// </summary>
        FastClockDCCEn = 0x08,
        /// <summary>
        /// Enable sendingMRclock multicasts
        /// </summary>
        FastClockMRclockEn = 0x10,
        /// <summary>
        /// Halt fast clock on emergency stop automatically
        /// </summary>
        FastClockEmergencyHaltEn = 0x40,
        /// <summary>
        /// Activate fast clock
        /// </summary>
        FastClockEnabled = 0x80
    }
}
