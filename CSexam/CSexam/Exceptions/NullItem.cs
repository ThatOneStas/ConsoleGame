using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSexam.Exceptions
{
    internal class NullItem : Exception
    {
        public NullItem(string message)
        : base(message)
        {
        }
    }
}
