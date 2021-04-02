using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z21lib
{
    public struct LocoAddress
    {
        public byte MSB { get; set; }

        public byte LSB { get; set; }

        public LocoAddress(byte msb, byte lsb)
        {
            MSB = msb;
            LSB = lsb;
        }

        public static LocoAddress FromByte(byte[] data)
        {
            return new LocoAddress(data[0], data[1]);
        }
    }
}
