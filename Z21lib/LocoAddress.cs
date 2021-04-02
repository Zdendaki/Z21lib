using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z21lib
{
    public struct LocoAddress
    {
        public byte MSB { 
            get 
            {
                byte val = (byte)(Number >> 8);
                if (val >= 128)
                    val |= 0xC0;
                return val;
            } 
        }

        public byte LSB { get => (byte)(Number % 256); }

        public int Number { get; set; }

        public LocoAddress(byte msb, byte lsb)
        {
            Number = (msb & 0x3F) << 8 + lsb;
        }

        public LocoAddress(int number)
        {
            Number = number;
        }

        public static LocoAddress FromByte(byte[] data)
        {
            return new LocoAddress(data[0], data[1]);
        }
    }
}
