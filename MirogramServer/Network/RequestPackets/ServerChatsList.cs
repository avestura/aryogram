using MirogramServer.Network.TransferMiniClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class ServerChatsList : Packet
    {
        public ServerChatsList() : base(PacketKinds.ServerChatsList) { }

        public List<UserAndPhone> Items { get; set; }

    }
}
