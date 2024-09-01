namespace Z21lib.Enums
{
    /// <summary>
    /// Z21 broadcasting data
    /// </summary>
    [Flags]
    public enum BroadcastFlags : uint
    {
        /// <summary>
        /// Basic driving and switching data
        /// </summary>
        BasicData = 0x00000001,
        /// <summary>
        /// R-Bus data
        /// </summary>
        RBus = 0x00000002,
        /// <summary>
        /// RailCom data of subscribed locomotives
        /// </summary>
        RailCom = 0x00000004,
        /// <summary>
        /// Z21 system status changes
        /// </summary>
        Z21Status = 0x00000100,
        /// <summary>
        /// Basic data for all locomotives, not intended for mobile hand controllers, from FW V1.24 sends only modified data
        /// </summary>
        AllLocomotives = 0x00010000,
        /// <summary>
        /// LocoNet messages without locomotives and switches
        /// </summary>
        LocoNet = 0x01000000,
        /// <summary>
        /// LocoNet messages from locomotives
        /// </summary>
        LocoNetLocomotives = 0x02000000,
        /// <summary>
        /// LocoNet messages from switches
        /// </summary>
        LocoNetSwitches = 0x04000000,
        /// <summary>
        /// LocoNet messages from occupancy detectors
        /// </summary>
        LocoNetOccupancyDetector = 0x08000000,
        /// <summary>
        /// RailCom data changes for all locomotives, not intended for mobile hand controllers
        /// </summary>
        RailComChanges = 0x00040000,
        /// <summary>
        /// CAN-Bus track occupancy detector changes
        /// </summary>
        CANBusChanges = 0x00080000,
        /// <summary>
        /// CAN-Bus booster status messages
        /// </summary>
        CANBusBooster = 0x00020000,
        /// <summary>
        /// Fast clock time messages
        /// </summary>
        FastClockTime = 0x00000010
    }
}
