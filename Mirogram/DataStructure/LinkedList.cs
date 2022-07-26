using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirogram.DataStructure
{
    public class LinkedList : IEnumerable
    {

        public object Value { get; set; }
        public LinkedList Next { get; set; }

        public object this[int index]
        {
            get
            {
                return ElementAt(index);
            }
        }

        public object ElementAt(int index)
        {
            var copy = this;
            for(int i = 0; i < index; i++)
            {
                if (copy.Next != null) copy = copy.Next;
                else return null;
            }
            return copy.Value;
        }

        public IEnumerator GetEnumerator()
        {
            var copy = this;
            while (copy.Next != null)
            {
                yield return copy;
                copy = copy.Next;
            }
        }
    }
}
