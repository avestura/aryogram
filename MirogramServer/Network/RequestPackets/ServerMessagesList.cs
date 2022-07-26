using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class ServerMessagesList : Packet
    {

        public ServerMessagesList() : base(PacketKinds.ServerMessagesList) { }

        public List<TransferMiniClasses.Message> Messages { get; set; }

    }
}
