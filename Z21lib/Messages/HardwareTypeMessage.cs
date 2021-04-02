using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z21lib.Messages
{
    public class HardwareTypeMessage : Message
    {
        public HardwareType HardwareType { get; set; }

        public int VersionMajor { get; set; }

        public int VersionMinor { get; set; }

        public HardwareTypeMessage(HardwareType type, int major, int minor) : base(MessageType.LAN_GET_HWINFO)
        {
            HardwareType = type;
            VersionMajor = major;
            VersionMinor = minor;
        }
    }

    public enum HardwareType : uint
    {
        UNKNOWN = 0,
        Z21_OLD = 0x00000200,
        Z21_NEW = 0x00000201,
        SMARTRAIL = 0x00000202,
        Z21_SMALL = 0x00000203,
        Z21_START = 0x00000204,
        Z21_XL = 0x00000211,
        SINGLE_BOOSTER = 0x00000205,
        DUAL_BOOSTER = 0x00000206,
        Z21_SWITCH_DECODER = 0x00000301,
        Z21_SIGNAL_DECODER = 0x00000302
    }
}
