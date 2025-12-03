namespace Z21lib.Enums
{
    [Flags]
    public enum DCentralState : byte
    {
        /// <summary>
        /// Emergency stop for decoder
        /// </summary>
        EmergencyStop = 0x01,
        /// <summary>
        /// The track voltage is switched off
        /// </summary>
        TrackVoltageOff = 0x02,
        /// <summary>
        /// Short circuit
        /// </summary>
        ShortCircuit = 0x04,
        /// <summary>
        /// Configuration mode is active
        /// </summary>
        ConfigMode = 0x10
    }

    [Flags]
    public enum DCentralStateEx : byte
    {
        /// <summary>
        /// Input voltage is too low
        /// </summary>
        PowerLost = 0x02,
        /// <summary>
        /// Addressing conform with RCN-213
        /// </summary>
        RCN213 = 0x20,
        /// <summary>
        /// No DCC input signal
        /// </summary>
        NoDCCInput = 0x80
    }
}
