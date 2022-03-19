using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z21lib.Endianity;

namespace Z21lib.Messages
{
    public class SystemStateMessage : Message
    {
        public short MainCurrent { get; set; }

        public short ProgCurrent { get; set; }

        public short FilteredMaincurrent { get; set; }

        public short Temperature { get; set; }

        public ushort SupplyVoltage { get; set; }

        public ushort VCCVoltage { get; set; }

        public CentralState CentralState { get; set; }

        public CentralStateEx CentralStateEx { get; set; }

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

        public SystemStateMessage(byte[] data) : base(MessageType.LAN_SYSTEMSTATE_DATACHANGED)
        {
            using (LittleEndianReader lr = new LittleEndianReader(data))
            {
                MainCurrent = lr.ReadInt16();
                ProgCurrent = lr.ReadInt16();
                FilteredMaincurrent = lr.ReadInt16();
                Temperature = lr.ReadInt16();
                SupplyVoltage = lr.ReadUInt16();
                VCCVoltage = lr.ReadUInt16();
                CentralState = (CentralState)lr.ReadByte();
                CentralStateEx = (CentralStateEx)lr.ReadByte();
            }
        }
    }
}
