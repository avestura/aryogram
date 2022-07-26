using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class GetMessagesRequest : Packet
    {

        public GetMessagesRequest() : base(PacketKinds.GetMessagesRequest) { }

        public string FromUserID { get; set; }
        public string ToUserId { get; set; }
        public string FromNumber { get; set; }
        public string ToNumber { get; set; }

    }
}
