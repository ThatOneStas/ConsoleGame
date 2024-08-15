using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSexam.Exceptions
{
    public class EntityIsDeadAlready : Exception
    {
        public EntityIsDeadAlready(string message)
        : base(message)
        {
        }
    }
}
