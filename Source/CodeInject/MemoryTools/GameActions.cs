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
        private delegate Int64 AttackWithSkillAction(int skill, int enemy, int arg0);
        private delegate Int64 PickUpAction(long arg1, long arg2, int itemIndex, int arg4);
        private delegate void UseIcoItemAction(long* IcoAdr);
        private delegate void UseQuickAction(long cQuickBarAddr, int key);
        private delegate void UseItemAction(long itemAdr);

        private UseIcoItemAction UseIcpItemFunc;
        private UseItemAction UseItemFunc;
        private PickUpAction PickUpFunc;
        private AttackWithSkillAction AttackWithSkillFunc;
        private UseQuickAction QuickActionFunc;
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
            QuickActionFunc = (UseQuickAction)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres + 0x87c4), typeof(UseQuickAction));
            UseItemFunc = (UseItemAction)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres + 0x133200), typeof(UseItemAction)); //MSG#INV5
            PickUpFunc = (PickUpAction)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("FF 90 D8 01 00 00 48 8B 0D ?? ?? ?? ?? 45 33 C9 44 8B 47 38 48 81 C1 D8 16 00 00 48 8B D7 E8 ?? ?? ?? ??"), typeof(PickUpAction)); //MSG#INV4
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

        /// <summary>
        /// Numbers between 0 and 0x0b
        /// </summary>
        /// <param name="key"></param>
        public void QuickBarExecute(int key)
        {
            QuickActionFunc(*(long*)(BaseAddres+ 0x112CC20), key);
        }


        public void itemUse(long itemAddr)
        {
            UseItemFunc(itemAddr);
        }


        public void UseItemByIco(long* IconAdr)
        {
            UseIcpItemFunc = (UseIcoItemAction)Marshal.GetDelegateForFunctionPointer(new IntPtr(*(long*)(*IconAdr + 0x18)), typeof(UseIcoItemAction));
            UseIcpItemFunc(IconAdr);
        }
    }
}
