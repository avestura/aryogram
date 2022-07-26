using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.DataBase
{
    public static class DataBaseStringParse
    {


        public static string AddElementToString(this string @string, string element)
        {
            var temp = @string;
            if (temp != string.Empty && temp.Split(',').Contains(element)) return element;

            if (temp == string.Empty) temp += element;
            else temp += "," + element;

            return temp;
        }

        public static bool ElementHasItem(this string content, string element)
        {
            try
            {
                return content.Split(',').Contains(element);
            }
            catch
            {
                return false;
            }
        }


    }
}
