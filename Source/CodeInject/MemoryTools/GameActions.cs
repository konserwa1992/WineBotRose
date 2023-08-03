using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeInject.MemoryTools
{
    internal unsafe class GameActions
    {
        public delegate Int64 AttackWithSkillAction(int skill, int enemy, int arg0);
        public delegate Int64 PickUpAction(long arg1, long arg2, int itemIndex, int arg4);
        public delegate void UseItemAction(long* IcoAdr);
        public delegate void UseItemAction2(long rcx,long wskrcx,int ebx);

        private UseItemAction UseItemFunc;
        private UseItemAction2 UseItemFunc2;
        private PickUpAction PickUpFunc;
        private AttackWithSkillAction AttackWithSkillFunc;
        private long BaseAddres;
        private long BaseNetworkClass;

        public GameActions()
        {
            Init();
        }
        public void Init()
        {
            BaseAddres = Process.GetCurrentProcess().MainModule.BaseAddress.ToInt64();
            BaseNetworkClass = MemoryTools.GetVariableAddres("48 8B 47 28 48 8D 4F 28 FF 90 D8 01 00 00 48 8B 0D ?? ?? ?? ??").ToInt64();

            PickUpFunc = (PickUpAction)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("FF 90 D8 01 00 00 48 8B 0D ?? ?? ?? ?? 45 33 C9 44 8B 47 38 48 81 C1 D8 16 00 00 48 8B D7 E8 ?? ?? ?? ??"), typeof(PickUpAction));
            AttackWithSkillFunc = (AttackWithSkillAction)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("4c 8d 44 24 20 8b d0 e8 ?? ?? ?? ??"), typeof(AttackWithSkillAction));
        }

        public void PickUp(ushort ItemID)
        {
            PickUpFunc((*(long*)(BaseNetworkClass) + 0x16D8), *((long*)(BaseAddres + 0x1118E60)), ItemID, 0);
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



        //0x000000F8 mamy cItem
        public void UseItemByIco(long* IconAdr)
        {
            UseItemFunc = (UseItemAction)Marshal.GetDelegateForFunctionPointer(new IntPtr(*(long*)(*IconAdr + 0x18)), typeof(UseItemAction));
            UseItemFunc(IconAdr);
        }
    }
}
