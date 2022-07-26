using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class LoginRequestPacket : Packet
    {
        public LoginRequestPacket() : base(PacketKinds.LoginRequest) { }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }


    }
}
