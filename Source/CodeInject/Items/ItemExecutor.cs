using System.Diagnostics;
using System.Windows.Forms;


namespace CodeInject
{
    public unsafe class ItemExecutor
    {
        public int CooldDown { get; set; }
        private Stopwatch stopWatch = new Stopwatch();
        public InvItem Item2Cast { get; set; }
        public int MinValueToExecute = 0;

        public ItemExecutor(int cooldDown,int minValue, InvItem item)
        {
            this.CooldDown = cooldDown;
            this.Item2Cast = item;
            this.MinValueToExecute = minValue;
        }

        public void Use(float currVal)
        {
            if (Item2Cast != null)
            {
                if (currVal < MinValueToExecute)
                {

                    if (stopWatch.IsRunning == false)
                    {
                        stopWatch.Start();
                        Item2Cast.UseItem();
                    }

                    if (stopWatch.Elapsed.Seconds > CooldDown)
                    {
                        Item2Cast.UseItem();
                        stopWatch.Reset();
                    }
                }
            }
        }
    }
}
