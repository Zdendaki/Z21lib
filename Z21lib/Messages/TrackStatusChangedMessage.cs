using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z21lib.Messages
{
    public class TrackStatusChangedMessage : Message
    {
        public CentralState Status { get; set; }

        public TrackStatusChangedMessage(CentralState status) : base(MessageType.LAN_X_STATUS_CHANGED)
        {
            Status = status;
        }
    }

    
}
