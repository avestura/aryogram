using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.TransferMiniClasses
{
    [Serializable]
    public class UserAndPhone
    {

        public UserAndPhone(string username, string number)
        {
            UserName = username; PhoneNumber = number;
        }
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

    }
}
