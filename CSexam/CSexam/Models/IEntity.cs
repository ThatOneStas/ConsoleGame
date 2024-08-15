using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSexam.Models
{
    public interface IEntity : ICloneable
    {
        public string _name { get; set; }
        public int _hp { get; set; }
        public int _lvl { get; set; }
        public void PrintInfo();
        public string GetInfo();
    }
}
