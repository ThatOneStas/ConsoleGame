using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSexam.Exceptions
{
    internal class OutOfGold : Exception
    {
        public OutOfGold(string message)
        : base(message)
        {
        }
    }
}
