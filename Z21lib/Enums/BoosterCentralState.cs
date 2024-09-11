namespace Z21lib.Enums
{
    [Flags]
    public enum BoosterCentralState : byte
    {
        /// <summary>
        /// Track is switched off
        /// </summary>
        TrackVoltageOff = 0x02,
        /// <summary>
        /// Configuration mode is active
        /// </summary>
        ConfigMode = 0x10,
        /// <summary>
        /// CAN connection with Z21 is OK
        /// </summary>
        CanConnected = 0x20
    }

    [Flags]
    public enum BoosterCentralStateEx : byte
    {
        /// <summary>
        /// Temperature is too high
        /// </summary>
        HighTemperature = 0x01,
        /// <summary>
        /// Supply voltage is too low
        /// </summary>
        PowerLost = 0x02,
        /// <summary>
        /// Short circuit on 1st output
        /// </summary>
        Booster1ShortCircuit = 0x04,
        /// <summary>
        /// Short circuit on 2nd output
        /// </summary>
        Booster2ShortCircuit = 0x08,
        /// <summary>
        /// Supply voltage error
        /// </summary>
        ReversedPolarity = 0x10,
        /// <summary>
        /// No DCC input signal
        /// </summary>
        NoDCCInput = 0x80
    }

    [Flags]
    public enum BoosterCentralStateEx2 : byte
    {
        /// <summary>
        /// RailCom active on 1st output
        /// </summary>
        Booster1RailComActive = 0x01,
        /// <summary>
        /// RailCom active on 2nd output
        /// </summary>
        Booster2RailComActive = 0x02,
        /// <summary>
        /// CAN autosettings active on 1st output
        /// </summary>
        Booster1MasterSettings = 0x04,
        /// <summary>
        /// CAN autosettings active on 2nd output
        /// </summary>
        Booster2MasterSettings = 0x08,
        /// <summary>
        /// Brake generator active on 1st output
        /// </summary>
        Booster1BgActive = 0x10,
        /// <summary>
        /// Brake generator active on 2nd output
        /// </summary>
        Booster2BgActive = 0x20,
        /// <summary>
        /// RailCom forwarding active on 1st output
        /// </summary>
        Booster1RailComForward = 0x40,
        /// <summary>
        /// RailCom forwarding active on 2nd output
        /// </summary>
        Booster2RailComForward = 0x80
    }

    [Flags]
    public enum BoosterCentralStateEx3 : byte
    {
        /// <summary>
        /// 1st track output inverted (autoinvert)
        /// </summary>
        Booster1OutputInverted = 0x01,
        /// <summary>
        /// 2nd track output inverted (autoinvert)
        /// </summary>
        Booster2OutputInverted = 0x02,
        /// <summary>
        /// Track output 1 deactivated (by user)
        /// </summary>
        Booster1OutputDisabled = 0x10,
        /// <summary>
        /// Track output 2 deactivated (by user)
        /// </summary>
        Booster2OutputDisabled = 0x20
    }
}
