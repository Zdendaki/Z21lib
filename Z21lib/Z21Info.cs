using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z21lib
{
    public struct Z21Info
    {
        public string IP { get; set; }

        public int Port { get; set; }

        public Z21Info(string ip, int port)
        {
            IP = ip;
            Port = port;
        }
    }
}
