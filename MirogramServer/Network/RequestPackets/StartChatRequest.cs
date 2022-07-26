using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class StartChatRequest : Packet
    {

        public StartChatRequest() : base(PacketKinds.StartChatRequest) { }

        public string Number { get; set; }
        public string WithUser { get; set; }
        public string WithNumber { get; set; }

    }
}
