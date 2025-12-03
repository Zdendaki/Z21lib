using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class LanBoosterSystemStateMessage : Message
    {
        public short Booster1MainCurrent { get; init; }

        public short Booster2MainCurrent { get; init; }

        public short Booster1FilteredMainCurrent { get; init; }

        public short Booster2FilteredMainCurrent { get; init; }

        public short Booster1Temperature { get; init; }

        public short Booster2Temperature { get; init; }

        public ushort SupplyVoltage { get; init; }

        public ushort Booster1VCCVoltage { get; init; }

        public ushort Booster2VCCVoltage { get; init; }

        public BoosterCentralState CentralState { get; init; }

        public BoosterCentralStateEx CentralStateEx { get; init; }

        public BoosterCentralStateEx2 CentralStateEx2 { get; init; }

        public BoosterCentralStateEx3 CentralStateEx3 { get; init; }

        public LanBoosterSystemStateMessage() : base(MessageType.LAN_BOOSTER_SYSTEMSTATE_DATACHANGED)
        {
        }

        internal static LanBoosterSystemStateMessage Parse(ReadOnlySpan<byte> message)
        {
            return new()
            {
                Booster1MainCurrent = LE.ToInt16(message.Slice(4)),
                Booster2MainCurrent = LE.ToInt16(message.Slice(6)),
                Booster1FilteredMainCurrent = LE.ToInt16(message.Slice(8)),
                Booster2FilteredMainCurrent = LE.ToInt16(message.Slice(10)),
                Booster1Temperature = LE.ToInt16(message.Slice(12)),
                Booster2Temperature = LE.ToInt16(message.Slice(14)),
                SupplyVoltage = LE.ToUInt16(message.Slice(16)),
                Booster1VCCVoltage = LE.ToUInt16(message.Slice(18)),
                Booster2VCCVoltage = LE.ToUInt16(message.Slice(20)),
                CentralState = (BoosterCentralState)message[22],
                CentralStateEx = (BoosterCentralStateEx)message[23],
                CentralStateEx2 = (BoosterCentralStateEx2)message[24],
                CentralStateEx3 = (BoosterCentralStateEx3)message[26]
            };
        }
    }
}
