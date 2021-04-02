using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z21lib
{
    public struct AccesoryAddress
    {
        public int FAdr { get; set; }

        public byte MSB
        {
            get
            {
                return (byte)(FAdr >> 8);
            }
        }

        public byte LSB
        {
            get
            {
                return (byte)(FAdr % 256);
            }
        }

        public AccesoryAddress(int dcc, int port)
        {
            FAdr = 4 * dcc + port;
        }
    }
}
