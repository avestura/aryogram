using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class FriendAcceptDeclineRequest : Packet
    {
        public FriendAcceptDeclineRequest() : base(PacketKinds.FriendAcceptDeclineRequest) { }
        public bool Accepted { get; set; }
        public string RequestedUserID { get; set; }

    }
}
