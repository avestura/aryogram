using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class AddFriendByIDRequest : Packet
    {

        public AddFriendByIDRequest() : base(PacketKinds.AddFriendByIDRequest) { }

        public string ToUserID { get; set; }

    }
}
