using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class SystemStateMessage : Message
    {
        /// <summary>
        /// Main current [mA]
        /// </summary>
        public short MainCurrent { get; set; }

        /// <summary>
        /// Programming track current [mA]
        /// </summary>
        public short ProgCurrent { get; set; }

        /// <summary>
        /// Smoothed current on main track [mA]
        /// </summary>
        public short FilteredMaincurrent { get; set; }

        /// <summary>
        /// Command station internal temprerature [°C]
        /// </summary>
        public short Temperature { get; set; }

        /// <summary>
        /// Supply voltage [mV]
        /// </summary>
        public ushort SupplyVoltage { get; set; }

        /// <summary>
        /// Internal voltage, identical to track voltage [mV]
        /// </summary>
        public ushort VCCVoltage { get; set; }

        public CentralState CentralState { get; set; }

        public CentralStateEx CentralStateEx { get; set; }

        public Capabilities Capabilities { get; set; }

        public SystemStateMessage(short mainCurrent, short progCurrent, short filteredMaincurrent, short temperature, ushort supplyVoltage, ushort vCCVoltage, CentralState centralState, CentralStateEx centralStateEx) : base(MessageType.LAN_SYSTEMSTATE_DATACHANGED)
        {
            MainCurrent = mainCurrent;
            ProgCurrent = progCurrent;
            FilteredMaincurrent = filteredMaincurrent;
            Temperature = temperature;
            SupplyVoltage = supplyVoltage;
            VCCVoltage = vCCVoltage;
            CentralState = centralState;
            CentralStateEx = centralStateEx;
        }

        internal SystemStateMessage(ReadOnlySpan<byte> data) : base(MessageType.LAN_SYSTEMSTATE_DATACHANGED)
        {
            var lr = new LittleEndianReader(data, 4);
            MainCurrent = lr.ReadInt16();
            ProgCurrent = lr.ReadInt16();
            FilteredMaincurrent = lr.ReadInt16();
            Temperature = lr.ReadInt16();
            SupplyVoltage = lr.ReadUInt16();
            VCCVoltage = lr.ReadUInt16();
            CentralState = (CentralState)lr.ReadByte();
            CentralStateEx = (CentralStateEx)lr.ReadByte();
            Capabilities = (Capabilities)lr.ReadByte();
        }

        internal static SystemStateMessage Parse(ReadOnlySpan<byte> message)
        {
            return new SystemStateMessage(message);
        }
    }
}
