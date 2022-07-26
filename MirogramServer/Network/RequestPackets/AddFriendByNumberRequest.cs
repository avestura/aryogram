using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class AddFriendByNumberRequest : Packet
    {

        public AddFriendByNumberRequest() : base(PacketKinds.AddFriendByNumberRequest) { }

        public string PhoneNumber { get; set; }

    }
}
