using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Modules
{
    internal unsafe class AutoPotionModule : IModule
    {
        public ItemExecutor AutoHp;
        public ItemExecutor AutoMp;

        public string Name { get; set; } = "AUTOPOTION";

        public void update()
        {
            AutoPotionFunction();
        }

        public void AutoPotionFunction()
        {
            AutoHp.Use((((float)*(GameFunctionsAndObjects.DataFetch.GetPlayer()).Hp / *(GameFunctionsAndObjects.DataFetch.GetPlayer()).MaxHp) * 100));
            AutoMp.Use((((float)*(GameFunctionsAndObjects.DataFetch.GetPlayer()).Mp / *(GameFunctionsAndObjects.DataFetch.GetPlayer()).MaxMp) * 100));
        }

        public void SetAutoHPpotion(int minHelathProc, int colddawn, InvItem item)
        {
            AutoHp = new ItemExecutor(colddawn, minHelathProc, item);
        }
        public void SetAutoMPpotion(int minManaProc, int colddawn, InvItem item)
        {
            AutoMp = new ItemExecutor(colddawn, minManaProc, item);
        }
    }
}
