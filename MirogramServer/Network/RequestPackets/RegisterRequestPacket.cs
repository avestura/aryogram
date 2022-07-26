using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class RegisterRequestPacket : Packet
    {
        public RegisterRequestPacket() : base(PacketKinds.RegisterRequest) { }
        public string Password { get; set; }
        public string PhoneNumbers { get; set; }
    }
}
