using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network.TransferMiniClasses
{
    [Serializable]
    public class UserAndPhones
    {
        public UserAndPhones(string username, List<string> numbers)
        {
            UserName = username; PhoneNumbers = numbers;
        }
        public string UserName { get; set; }
        
        public List<string> PhoneNumbers { get; set; }

    }
}
