using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject
{
    public class Potion
    {
        public int ColdDown { get; set; }
        private Stopwatch stopWatch = new Stopwatch();
        public InvItem Item2Cast { get; set; }

        public Potion(int coldDown, InvItem item)
        {
            this.ColdDown = coldDown;
            this.Item2Cast = item;
        }


        public void Use()
        {
            if (stopWatch.IsRunning == false)
            {
                stopWatch.Start();
                Item2Cast.UseItem();
            }

            if (stopWatch.Elapsed.Seconds > ColdDown)
            {
                Item2Cast.UseItem();
                stopWatch.Reset();
            }
        }
    }
}
