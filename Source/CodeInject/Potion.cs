using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject
{
    class Potion
    {
        public int ColdDown { get; set; }
        private Stopwatch stopWatch = new Stopwatch();

        public Potion(int coldDown)
        {
            this.ColdDown = coldDown;
        }


        public void Use(InvItem item)
        {
            if (stopWatch.IsRunning == false)
            {
                stopWatch.Start();
                item.UseItem();
            }

            if (stopWatch.Elapsed.Seconds > ColdDown)
            {
                item.UseItem();
                stopWatch.Reset();
            }
        }
    }
}
