using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSexam.Exceptions
{
    public class NullEntity : Exception
    {
        public NullEntity(string message)
        : base(message)
        {
        }
    }
}
