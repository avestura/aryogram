using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class GetFriendRequestsToMeRequest : Packet
    {

        public GetFriendRequestsToMeRequest() : base(PacketKinds.GetFriendRequestsToMeRequest) { }

    }
}
