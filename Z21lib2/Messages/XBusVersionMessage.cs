using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z21lib.Messages
{
    public class XBusVersionMessage : Message
    {
        public float Version { get; set; }

        public byte Station { get; set; }

        public XBusVersionMessage(int version, byte station) : base(MessageType.LAN_X_GET_VERSION)
        {
            Version = version / 10f;
            Station = station;
        }
    }
}
