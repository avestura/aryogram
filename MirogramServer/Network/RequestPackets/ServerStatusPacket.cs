using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.RequestPackets
{
    [Serializable]
    public class ServerStatusPacket : Packet
    {

        public ServerStatusPacket() : base(PacketKinds.StatusServerResonse) { }

        public enum Status
        {
            Successful, Failed, Warning, Error
        }
        public enum ResponseTos
        {
            Register, Login, AddFriend
        }

        public Status ServerResonseStatus { get; set; }
        public ResponseTos ResponseTo { get; set; }


        public string SuccessMessage { get; set; }
        public string WarningMessage { get; set; }
        public string FailMessage { get; set; }
        public string ErrorMessage { get; set; }
        public object DataAttachment { get; set; }

    }
}
