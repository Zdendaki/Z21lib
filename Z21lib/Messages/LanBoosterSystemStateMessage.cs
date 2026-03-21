using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages;

public class LanBoosterSystemStateMessage() : Message(MessageType.LAN_BOOSTER_SYSTEMSTATE_DATACHANGED)
{
    public required short Booster1MainCurrent { get; init; }

    public required short Booster2MainCurrent { get; init; }

    public required short Booster1FilteredMainCurrent { get; init; }

    public required short Booster2FilteredMainCurrent { get; init; }

    public required short Booster1Temperature { get; init; }

    public required short Booster2Temperature { get; init; }

    public required ushort SupplyVoltage { get; init; }

    public required ushort Booster1VCCVoltage { get; init; }

    public required ushort Booster2VCCVoltage { get; init; }

    public required BoosterCentralState CentralState { get; init; }

    public required BoosterCentralStateEx CentralStateEx { get; init; }

    public required BoosterCentralStateEx2 CentralStateEx2 { get; init; }

    public required BoosterCentralStateEx3 CentralStateEx3 { get; init; }

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
