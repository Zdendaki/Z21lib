namespace Z21lib.Enums
{
    [Flags]
    public enum BoosterState
    {
        /// <summary>
        /// Brake generator active
        /// </summary>
        BrakeGeneratorActive = 0x01,
        /// <summary>
        /// Short circuit on track
        /// </summary>
        ShortCircuit = 0x20,
        /// <summary>
        /// Track is switched off
        /// </summary>
        TrackVoltageOff = 0x80,
        /// <summary>
        /// RailCom-Cutout is active
        /// </summary>
        RailComActive = 0x800,
        /// <summary>
        /// Track is deactivated (by user)
        /// </summary>
        OutputDisabled = 0x100
    }
}
