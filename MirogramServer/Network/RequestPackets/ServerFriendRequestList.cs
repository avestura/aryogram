using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class ServerFriendRequestList : Packet
    {

        public ServerFriendRequestList() : base(PacketKinds.ServerFriendRequestList) { }
        
        public List<string> Requests { get; set;}

    }
}
