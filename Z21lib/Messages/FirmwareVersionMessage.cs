using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z21lib.Messages
{
    public class FirmwareVersionMessage : Message
    {
        public int Major { get; set; }

        public int Minor { get; set; }

        public FirmwareVersionMessage(int major, int minor) : base(MessageType.LAN_X_GET_FIRMWARE_VERSION)
        {
            Major = major;
            Minor = minor;
        }
    }
}
