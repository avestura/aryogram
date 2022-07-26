using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class GetFriendsRequest : Packet
    {

        public GetFriendsRequest() : base(PacketKinds.GetFriendsRequest) { }

    }
}
