using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.MemoryTools
{
    internal unsafe class GameActions
    {
        public delegate Int64 AttackWithSkillAction(int skill, int enemy, int arg0);
        public delegate Int64 PickUpAction(long arg1, long arg2, int itemIndex, int arg4);

        private PickUpAction PickUpFunc;
        private AttackWithSkillAction AttackWithSkillFunc;
        private long BaseAddres;

        public GameActions()
        {
            Init();
        }
        public void Init()
        {
            BaseAddres = Process.GetCurrentProcess().MainModule.BaseAddress.ToInt64();
            PickUpFunc = (PickUpAction)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres + 0x26e90), typeof(PickUpAction));
            AttackWithSkillFunc = (AttackWithSkillAction)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres + 0x4260E), typeof(AttackWithSkillAction));
        }

        public void PickUp(int ItemID)
        {
            PickUpFunc((*(long*)(BaseAddres + 0x1122EF0) + 0x16D8), *((long*)(BaseAddres + 0x1118E60)), ItemID, 0);
        }

        /// <summary>
        /// Pattern: 4c 8d 44 24 20 8b d0
        /// </summary>
        /// <param name="TargedID"></param>
        /// <param name="SkillIndex"></param>
        public void Attack(int TargedID, int SkillIndex)
        {
            AttackWithSkillFunc(SkillIndex, TargedID, 0);
        }

    }
}
