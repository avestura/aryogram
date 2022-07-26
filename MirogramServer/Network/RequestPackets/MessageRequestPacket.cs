using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class MessageRequestPacket : Packet
    {
        public MessageRequestPacket() : base(PacketKinds.MessageRequest) { }

        public string Content { get; set; }
        public string FromUserID { get; set; }
        public string ToUserId { get; set; }
        public string TimeSent { get; set; }
        public int InReplyTo { get; set; }
        public string FromNumber { get; set; }
        public string ToNumber { get; set; }
    }
}
