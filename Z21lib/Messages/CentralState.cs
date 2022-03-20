namespace Z21lib.Messages
{
    [Flags]
    public enum CentralState : byte
    {
        EmergencyStop = 0x01,
        TrackVoltageOff = 0x02,
        ShortCircuit = 0x04,
        ProgrammingModeActive = 0x20
    }

    [Flags]
    public enum CentralStateEx : byte
    {
        HighTemperature = 0x01,
        TrackVoltageOff = 0x02,
        ShortCircuitExternal = 0x04,
        ShortCircuitInternal = 0x08
    }

    [Flags]
    public enum Capabilities : byte
    {
        DCC = 0x01,
        MM = 0x02,
        Reserved = 0x04,
        RailCom = 0x08,
        LocoCmds = 0x10,
        DetectorCmds = 0x20,
        NeedsUnlockCode = 0x80
    }
}
