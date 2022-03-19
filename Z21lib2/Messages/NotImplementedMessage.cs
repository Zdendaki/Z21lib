using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z21lib.Messages
{
    public class NotImplementedMessage : Message
    {
        byte[] binary;

        public string Data { get => BitConverter.ToString(binary).ToUpper().Replace('-', ' '); }

        public NotImplementedMessage(byte[] data) : base(MessageType.NOT_IMPLEMENTED)
        {
            binary = data;
        }
    }
}
