using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z21lib.Messages
{
    public class NotImplementedMessage : Message
    {
        public byte[] Data { get; set; }

        public NotImplementedMessage(byte[] data) : base(MessageType.NOT_IMPLEMENTED)
        {
            Data = data;
        }
    }
}
