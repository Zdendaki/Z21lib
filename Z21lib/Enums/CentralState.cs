namespace Z21lib.Enums
{
    [Flags]
    public enum CentralState : byte
    {
        /// <summary>
        /// Emergency stop is switched on
        /// </summary>
        EmergencyStop = 0x01,
        /// <summary>
        /// Track voltage is switched off
        /// </summary>
        TrackVoltageOff = 0x02,
        /// <summary>
        /// Short-circuit
        /// </summary>
        ShortCircuit = 0x04,
        /// <summary>
        /// Programming mode is active
        /// </summary>
        ProgrammingModeActive = 0x20
    }

    [Flags]
    public enum CentralStateEx : byte
    {
        /// <summary>
        /// Temperature is too high
        /// </summary>
        HighTemperature = 0x01,
        /// <summary>
        /// Input voltage is too low
        /// </summary>
        TrackVoltageOff = 0x02,
        /// <summary>
        /// Short circuit at the external booster output
        /// </summary>
        ShortCircuitExternal = 0x04,
        /// <summary>
        /// Short circuit at the main track or programming track
        /// </summary>
        ShortCircuitInternal = 0x08,
        /// <summary>
        /// Turnout addresses according to RCN-213 (from FW 1.42)
        /// </summary>
        RCN213 = 0x20
    }

    [Flags]
    public enum Capabilities : byte
    {
        /// <summary>
        /// Device is running on older firmware than 1.42
        /// </summary>
        None = 0,
        /// <summary>
        /// Capable of DCC
        /// </summary>
        DCC = 0x01,
        /// <summary>
        /// Capable of MM
        /// </summary>
        MM = 0x02,
        /// <summary>
        /// Reserved for future development
        /// </summary>
        //Reserved = 0x04,
        /// <summary>
        /// RailCom is activated
        /// </summary>
        RailCom = 0x08,
        /// <summary>
        /// Accepts LAN commands for locomotive decoders
        /// </summary>
        LocoCmds = 0x10,
        /// <summary>
        /// Accepts LAN commands for accressory decodeers
        /// </summary>
        AccessoryCmds = 0x20,
        /// <summary>
        /// Accepts LAN commands for detectors
        /// </summary>
        DetectorCmds = 0x40,
        /// <summary>
        /// Device needs activate code (Z21 start)
        /// </summary>
        NeedsUnlockCode = 0x80
    }
}
