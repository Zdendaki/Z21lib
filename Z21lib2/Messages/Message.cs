using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z21lib.Messages
{
    public class Message
    {
        public MessageType Type { get; set; }

        public Message(MessageType type)
        {
            Type = type;
        }
    }
}
