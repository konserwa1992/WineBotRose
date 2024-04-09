using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Modules
{
    [Serializable]
    internal unsafe class AutoPotionModule : IModule
    {
        public ItemExecutor AutoHp;
        public ItemExecutor AutoMp;

        public string Name { get; set; } = "AUTOPOTION";

        public void Update()
        {
            AutoPotionFunction();
        }

        public void AutoPotionFunction()
        {
            AutoHp.Use((((float)(GameHackFunc.Game.ClientData.GetPlayer()).Hp / (GameHackFunc.Game.ClientData.GetPlayer()).MaxHp) * 100));
            AutoMp.Use((((float)(GameHackFunc.Game.ClientData.GetPlayer()).Mp / (GameHackFunc.Game.ClientData.GetPlayer()).MaxMp) * 100));
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
