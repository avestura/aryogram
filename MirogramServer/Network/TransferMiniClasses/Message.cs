using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.TransferMiniClasses
{
    [Serializable]
    public class Message 
    {
        public string Content { get; set; }
        public string FromUserID { get; set; }
        public string ToUserID { get; set; }
        public string TimeSent { get; set; }
        public Nullable<long> InReplyTo { get; set; }
        public string FromNumber { get; set; }
        public string ToNumber { get; set; }
    }
}
