using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class GetListOfChatsRequest : Packet
    {
        public GetListOfChatsRequest() : base(PacketKinds.GetListOfChatsRequest) { }

        public string Number { get; set; }

    }
}
