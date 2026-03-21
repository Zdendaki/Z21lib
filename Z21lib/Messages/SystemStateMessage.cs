using System.Diagnostics.CodeAnalysis;
using Z21lib.Endianity;
using Z21lib.Enums;

namespace Z21lib.Messages;

public class SystemStateMessage() : Message(MessageType.LAN_SYSTEMSTATE_DATACHANGED)
{
    /// <summary>
    /// Main current [mA]
    /// </summary>
    public required short MainCurrent { get; set; }

    /// <summary>
    /// Programming track current [mA]
    /// </summary>
    public required short ProgCurrent { get; set; }

    /// <summary>
    /// Smoothed current on main track [mA]
    /// </summary>
    public required short FilteredMaincurrent { get; set; }

    /// <summary>
    /// Command station internal temprerature [°C]
    /// </summary>
    public required short Temperature { get; set; }

    /// <summary>
    /// Supply voltage [mV]
    /// </summary>
    public required ushort SupplyVoltage { get; set; }

    /// <summary>
    /// Internal voltage, identical to track voltage [mV]
    /// </summary>
    public required ushort VCCVoltage { get; set; }

    public required CentralState CentralState { get; set; }

    public required CentralStateEx CentralStateEx { get; set; }

    public required Capabilities Capabilities { get; set; }

    [SetsRequiredMembers]
    internal SystemStateMessage(ReadOnlySpan<byte> data) : this()
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
