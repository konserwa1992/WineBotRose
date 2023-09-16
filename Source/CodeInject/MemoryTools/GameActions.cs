using CodeInject.Actors;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WebSocketSharp;
using static System.Net.Mime.MediaTypeNames;


namespace CodeInject.MemoryTools
{
    internal unsafe class GameActions
    {
        private delegate Int64 AttackWithSkillAction(int skill, int enemy, int arg0);
        private delegate Int64 PickUpAction(long networkClass, long playerObjectAdr, int itemIndex, int arg4);
        private delegate void UseIcoItemAction(long* IcoAdr);
        private delegate void UseQuickAction(long cQuickBarAddr, int key);
        private delegate void UseItemAction(long itemAdr);
        private delegate void NormalAttackAction(long networkClass, int enemy);
        private delegate void MoveToAction(long networkClass, int unknow0,float* destinationPoint);


        public delegate void Log(long player, string stringPointer, int cType, uint color); //trose.exe+3266EE unknowarg = 0xFFFFBF00

        //121bc50

        private UseIcoItemAction UseIcpItemFunc;
        private UseItemAction UseItemFunc;
        private PickUpAction PickUpFunc;
        private AttackWithSkillAction AttackWithSkillFunc;
        private NormalAttackAction NormalAttackFunc;
        private UseQuickAction QuickActionFunc;
        private MoveToAction MoveToPointFunc;
        public Log LoggerFunc;
        private long BaseAddres;
        private long BaseNetworkClass;
        private long BaseOfDialogBoxes;

        public GameActions()
        {
            Init();
        }
        public void Init()
        {
            BaseAddres = Process.GetCurrentProcess().MainModule.BaseAddress.ToInt64();
            BaseNetworkClass = MemoryTools.GetVariableAddres("48 8B 47 28 48 8D 4F 28 FF 90 D8 01 00 00 48 8B 0D ?? ?? ?? ??").ToInt64();
            BaseOfDialogBoxes = MemoryTools.GetVariableAddres("48 81 c1 ?? ?? ?? ?? 48 8d 54 24 20 e8 ?? ?? ?? ?? 48 8b d0 45 33 c9 45 8d 41 05 48 8d 0d ?? ?? ?? ??").ToInt64();
            LoggerFunc = (Log)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("48 81 c1 ?? ?? ?? ?? 48 8d 54 24 20 e8 ?? ?? ?? ?? 48 8b d0 45 33 c9 45 8d 41 05 48 8d 0d ?? ?? ?? ?? e8 ?? ?? ?? ??"), typeof(Log));
            QuickActionFunc = (UseQuickAction)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres + 0x87c4), typeof(UseQuickAction));
            UseItemFunc = (UseItemAction)Marshal.GetDelegateForFunctionPointer((IntPtr)MemoryTools.GetFunctionAddress("40 53 48 83 ec 20 48 83 79 30 00 48 8b d9"), typeof(UseItemAction)); //MSG#INV5 
            PickUpFunc = (PickUpAction)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("FF 90 D8 01 00 00 48 8B 0D ?? ?? ?? ?? 45 33 C9 44 8B 47 38 48 81 C1 B8 16 00 00 48 8B D7 E8 ?? ?? ?? ??"), typeof(PickUpAction)); //MSG#INV4
            AttackWithSkillFunc = (AttackWithSkillAction)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("4c 8d 44 24 20 8b d0 e8 ?? ?? ?? ??"), typeof(AttackWithSkillAction));
            NormalAttackFunc = (NormalAttackAction)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("48 8b cf e8 ?? ?? ?? ?? 84 c0 0f 84 ?? ?? ?? ?? 40 84 f6 0f 84 ?? ?? ?? ?? 48 8b 0d ?? ?? ?? ?? 8b d3 48 81 c1 ?? ?? ?? ?? e8 ?? ?? ?? ??"), typeof(NormalAttackAction));
            MoveToPointFunc = (MoveToAction)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("48 8b cf e8 ?? ?? ?? ?? 84 c0 ?? ?? ?? ?? ?? ?? 48 8b 0d ?? ?? ?? ?? 4c 8b c6 48 81 c1 ?? ?? ?? ?? 33 d2 e8 ?? ?? ?? ??"), typeof(MoveToAction));
        }


        public void Logger(string text, Color color,int chatType=5)
        {
            LoggerFunc(
             BaseOfDialogBoxes, text, chatType, (uint)color.ToArgb());
        }

        public void PickUp(ushort ItemID)
        {
            PickUpFunc((*(long*)(BaseNetworkClass) + 0x16b8), (long)GameFunctionsAndObjects.DataFetch.GetPlayer().ObjectPointer, ItemID, 0);
        }

        /// <summary>
        /// Pattern: 4c 8d 44 24 20 8b d0
        /// </summary>
        /// <param name="TargedID"></param>
        /// <param name="SkillIndex"></param>
        /// 
        public void CastSpell(int TargedID, int SkillIndex)
        {
            Logger($"Target {TargedID.ToString("X")}  SkillIndex: {SkillIndex.ToString("X")}",Color.Bisque);
            AttackWithSkillFunc(SkillIndex, TargedID, 1);
        }


        /// <summary>
        /// Position is multiplied by 100, for better reading coordinates in game client. 
        /// Z is unnessesery
        /// </summary>
        /// <param name="position"></param>
        public void MoveToPoint(Vector2 position)
        {
            Logger($"Go to: {position.ToString()}", Color.Khaki);
            float[] position2float = new float[]
            {
                position.X*100, position.Y*100,0
            };
            fixed (float* p = position2float)
            {
                MoveToPointFunc((*(long*)BaseNetworkClass) + 0x16b8, 0, p);
            }
        }

        public void CastSpell(int SkillIndex)
        {
            IObject player = GameFunctionsAndObjects.DataFetch.GetPlayer();

            AttackWithSkillFunc(SkillIndex, *player.ID, 0);
        }

        public void Attack(int TargedID)
        {
            NormalAttackFunc((*(long*)(BaseNetworkClass) + 0x16b8), TargedID);
        }

        /// <summary>
        /// Numbers between 0 and 0x0b
        /// </summary>
        /// <param name="key"></param>
        public void QuickBarExecute(int key)
        {
            QuickActionFunc(*(long*)(BaseAddres + 0x112CC20), key);
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
