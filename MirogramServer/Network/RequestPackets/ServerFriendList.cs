using MirogramServer.Network.TransferMiniClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class ServerFriendList : Packet
    {
        public ServerFriendList() : base(PacketKinds.ServerFriendList) { }

        public List<UserAndPhones> Friends { get; set; }
    }
}
