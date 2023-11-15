using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeInject.Modules
{
    internal class WalkModule : IModule
    {
        public string Name { get; set; } = "WALKMODULE";

        List<Point> points = new List<Point>();
        int index = 0;
        public WalkModule(List<Point> points)
        {
            this.points = points;
        }


        public unsafe void update()
        {
            if (index == points.Count)
            {
                return;
            }

        }
    }
}
