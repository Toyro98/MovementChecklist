using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovementChecklist
{
    public class Movement
    {
        public int Id;
        public string Name;
        public string ToolTip;
        public bool Completed;

        public Movement(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
