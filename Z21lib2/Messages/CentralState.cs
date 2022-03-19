using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
