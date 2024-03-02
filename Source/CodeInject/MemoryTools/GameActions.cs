using CodeInject.Actors;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;



namespace CodeInject.MemoryTools
{
    public unsafe class GameActions
    {
        private delegate Int64 AttackWithSkillAction(int skill, int enemy, float* arg0);
     //   private delegate Int64 PickUpAction(long networkClass, long playerObjectAdr, int itemIndex, int arg4);
        private delegate Int64 PickUpAction(long networkClass, int itemIndex, long playerObjectAdr);
        private delegate void UseIcoItemAction(long* IcoAdr);
        private delegate void UseQuickAction(long cQuickBarAddr, int key);
        private delegate void UseItemAction(long itemAdr);
        private delegate void NormalAttackAction(long networkClass, int enemy);
        private delegate void MoveToAction(long networkClass, int unknow0,float* destinationPoint);
        private delegate void TalkWithNPC(long arg0, ushort npcIndex);

        private delegate void PacketSendDelegate(long arg0, byte* packet);


        public delegate void Log(long staticAddr, string stringPointer, int cType, int color); 

        //121bc50

        private UseIcoItemAction UseIcpItemFunc;
        private UseItemAction UseItemFunc;
        private PickUpAction PickUpFunc;
        private AttackWithSkillAction AttackWithSkillFunc;
        private NormalAttackAction NormalAttackFunc;
        private UseQuickAction QuickActionFunc;
        private MoveToAction MoveToPointFunc;
        private TalkWithNPC TalkToNPCFunc;
        private PacketSendDelegate SendPacketFunc;

        public Log LoggerFunc;
        private long BaseAddres;
        private long BaseNetworkClass;
        private long BaseOfDialogBoxes;

        private long ChatBaseAddres;

        public GameActions()
        {
            Init();
        }
        public void Init()
        {
            BaseAddres = Process.GetCurrentProcess().MainModule.BaseAddress.ToInt64();
            BaseNetworkClass = MemoryTools.GetVariableAddres("48 8B 47 28 48 8D 4F 28 FF 90 D8 01 00 00 48 8B 0D ?? ?? ?? ??").ToInt64();//2023.10.03
            ChatBaseAddres = MemoryTools.GetVariableAddres("48 8d 54 24 40 e8 ?? ?? ?? ?? 48 8b d0 45 33 c9 45 8d 41 05 48 8d 0d ?? ?? ?? ??").ToInt64();//2023.10.03

            Console.WriteLine($"BaseNetworkClass {BaseNetworkClass.ToString("X")}");

            //48 8b d0 45 33 c9 45 8d 41 05 48 8d 0d ?? ?? ?? ?? e8 ?? ?? ?? ??
            LoggerFunc = (Log)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres + 0x44bd30), typeof(Log));
          //  LoggerFunc = (Log)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("48 8d 54 24 40 e8 ?? ?? ?? ?? 48 8b d0 45 33 c9 45 8d 41 05 48 8d 0d ?? ?? ?? ?? e8 ?? ?? ?? ??"), typeof(Log));

            UseItemFunc = (UseItemAction)Marshal.GetDelegateForFunctionPointer((IntPtr)MemoryTools.GetFunctionAddress("40 53 48 83 ec 20 48 83 79 30 00 48 8b d9"), typeof(UseItemAction)); //MSG#INV5 
            Console.WriteLine($"UseItemFunc {BaseNetworkClass.ToString("X")}");

            AttackWithSkillFunc = (AttackWithSkillAction)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("4c 8d 44 24 20 8b d0 e8 ?? ?? ?? ??"), typeof(AttackWithSkillAction));
            Console.WriteLine($"AttackWithSkillFunc {BaseNetworkClass.ToString("X")}");

            NormalAttackFunc = (NormalAttackAction)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("48 8b cf e8 ?? ?? ?? ?? 84 c0 0f 84 ?? ?? ?? ?? 40 84 f6 0f 84 ?? ?? ?? ?? 48 8b 0d ?? ?? ?? ?? 8b d3 48 81 c1 ?? ?? ?? ?? e8 ?? ?? ?? ??"), typeof(NormalAttackAction));
            Console.WriteLine($"NormalAttackFunc {BaseNetworkClass.ToString("X")}");

            MoveToPointFunc = (MoveToAction)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("48 8b cf e8 ?? ?? ?? ?? 84 c0 ?? ?? ?? ?? ?? ?? 48 8b 0d ?? ?? ?? ?? 4c 8b c6 48 81 c1 ?? ?? ?? ?? 33 d2 e8 ?? ?? ?? ??"), typeof(MoveToAction));
            Console.WriteLine($"MoveToPointFunc {BaseNetworkClass.ToString("X")}");

            PickUpFunc = (PickUpAction)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres+0x55d670), typeof(PickUpAction)); //MSG#INV4
            Console.WriteLine($"GameActions Init");


            SendPacketFunc = (PacketSendDelegate)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres + 0xAF6300), typeof(PacketSendDelegate)); //MSG#INV4

        }


        public void Logger(string text, int chatType = 5)
        {
            LoggerFunc(BaseAddres+0x14b1d80, text, chatType, Color.LimeGreen.ToArgb());
        }

        public void TalkToNPC(ushort ID)
        {
            long* rcxAddr = (long*)BaseAddres + 0x16DAF00;
            TalkToNPCFunc(*rcxAddr, ID);
        }

        public void SendPacket(byte* packet)
        {
            long networkStructAddr = *((long*)(BaseAddres + 0x1526a00)) + 0x16b8 + 0x1e0;
            SendPacketFunc(networkStructAddr, packet);
        }


        public void PickUp(Item item)
        {
            //48 8b cf e8 ?? ?? ?? ?? 84 c0 0f 84 ?? ?? ?? ?? 48 8b cf e8 ?? ?? ?? ?? 6683 f804 75 7b 48 8b 47 28  48 8d 4f 28
            long r8 = ((long)item.ObjectPointer) +0x10;
            PickUpFunc((*(long*)(BaseNetworkClass) + 0x16b8), item.ID, r8);
        }

        /// <summary>
        /// Pattern: 4c 8d 44 24 20 8b d0
        /// </summary>
        /// <param name="targedID"></param>
        /// <param name="skillIndex"></param>
        /// 
        public void CastSpell(IObject target, int skillIndex)
        {
            float[] SkillPosition = new float[]
            {
                target.X,target.Y,target.Z
            };

            fixed (float* markPointer = SkillPosition)
            {
                AttackWithSkillFunc(skillIndex, target.ID, markPointer);
            }
        }


        /// <summary>
        /// Position is multiplied by 100, for better reading coordinates in game client. 
        /// Z is unnessesery
        /// </summary>
        /// <param name="position"></param>
        public void MoveToPoint(Vector2 position)
        {
            Logger($"Go to: {position.ToString()}");
            float[] position2float = new float[]
            {
                position.X*100, position.Y*100,0
            };
            fixed (float* p = position2float)
            {
                MoveToPointFunc((*(long*)BaseNetworkClass) + 0x16b8, 0, p);
            }
        }

        public void CastSpell(int skillIndex)
        {
            IObject player = GameHackFunc.ClientData.GetPlayer();

            float[] markPosition = new float[]
            {
                player.X, player.Y, player.Z
            };
            fixed (float* markPointer = markPosition)
            {
                AttackWithSkillFunc(skillIndex, player.ID, markPointer);
            }
        }

        public void Attack(int targedID)
        {
            NormalAttackFunc((*(long*)(BaseNetworkClass) + 0x16b8), targedID);
        }

        /// <summary>
        /// Numbers between 0 and 0x0b
        /// </summary>
        /// <param name="key"></param>
        public void QuickBarExecute(int key)
        {
            QuickActionFunc(*(long*)(BaseAddres + 0x112CC20), key);
        }


        public void ItemUse(long itemAddr)
        {
            UseItemFunc(itemAddr);
        }


        public void UseItemByIco(long* iconAdr)
        {
            UseIcpItemFunc = (UseIcoItemAction)Marshal.GetDelegateForFunctionPointer(new IntPtr(*(long*)(*iconAdr + 0x18)), typeof(UseIcoItemAction));
            UseIcpItemFunc(iconAdr);
        }
    }
}
