using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.AutoReStock
{
    class Points
    {
        public Vector2 Position { get; set; }
        public string Name { get; set; }

        public List<Points> Nodes { get; set;} = new List<Points>();


        public override string ToString()
        {
            return Name +" "+Position.ToString();
        }
    }
}
