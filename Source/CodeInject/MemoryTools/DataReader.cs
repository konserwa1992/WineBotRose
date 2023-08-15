using CodeInject.Actors;
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
    internal unsafe class DataReader
    {
        /// <summary>
        /// Im not sure what kind of index is. Propobly some kind index of clickable object?
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="idObj"></param>
        /// <returns></returns>
        public delegate Int64 GetItemAdr(long arg1, int index);
        public delegate long GetInventoryItemDetailsAdr(long cItemAddr);

        private GetItemAdr getItemFunc;
        private GetInventoryItemDetailsAdr getInventoryItemDetailsFunc;

        private long BaseAddres;
        private long GameBaseAddres;




        public DataReader()
        {
            Init();
        }

        private void Init()
        {
            Process _proc = Process.GetCurrentProcess();

            BaseAddres = _proc.MainModule.BaseAddress.ToInt64();
            GameBaseAddres = MemoryTools.GetVariableAddres("45 0F 57 DB 0F 1F 44 00 00 4C 8B 0D ?? ?? ?? ??").ToInt64(); //UOB#U6
            getItemFunc = (GetItemAdr)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("48 8B 0D ?? ?? ?? ?? 0F B7 DD 0F BF 54 59 0C E8 ?? ?? ?? ??"), typeof(GetItemAdr));//MSG#INV3
            getInventoryItemDetailsFunc = (GetInventoryItemDetailsAdr)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres+0x13b050), typeof(GetInventoryItemDetailsAdr)); //MSG#INV8
        }

        /// <summary>
        /// pattern 48 81 c1 d0 0c 00 00 0f b7 d7
        /// </summary>
        /// <returns></returns>
        public List<Skills> GetPlayerSkills()
        {
            List<Skills> skillList = new List<Skills>();

            ulong* adrPtr1 = (ulong*)(BaseAddres + 0x11b5740);
            int s = 0;
            while (*(short*)(*adrPtr1 + ((ulong)s * 2) + 0x50 + 0xCD0) != 0)//OBS#S2
            {
                short skillID = *(short*)(*adrPtr1 + ((ulong)s * 2) + 0x50 + 0xCD0);
                SkillInfo skill = DataBase.GameDataBase.SkillDatabase.FirstOrDefault(x => x.ID == skillID);
                if (skill == null)
                {
                    skill = new SkillInfo()
                    {
                        ID = skillID,
                        Name = "Unknow"
                    };
                }
                skillList.Add(new Skills(skill));

                s++;
            }
            return skillList;
        }

        /// <summary>
        /// Get GameObject from ID 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IObject GetObject<T>(int ID)
        {
            long* wskObj = (long*)((*(long*)(GameBaseAddres)) + (ID * 8) + 0x22078); //OBS#N3

            if (typeof(T) == typeof(NPC))
                return new NPC(wskObj);

            if (typeof(T) == typeof(Item))
                return new Item(wskObj);


            if (typeof(T) == typeof(Player))
                return new Player(wskObj);

            return null;
        }

        /// <summary>
        /// Pattern 45 33 c0 8b 14 88
        /// 
        /// Pattern is that same as it is in GetNPCs because first element of entry is player
        /// </summary>
        /// <returns></returns>
        public Player GetPlayer()
        {
            long* wsp = (long*)(*(long*)(GameBaseAddres) + 0x22050);
            int* monsterIDList = (int*)*wsp;

            return (Player)GetObject<Player>(*monsterIDList);
        }

        /// <summary>
        /// Pattern 45 33 c0 8b 14 88
        /// </summary>
        /// <returns></returns>
        public List<IObject> GetNPCs()
        {

            List<IObject> wholeNpcList = new List<IObject>();
            long* wsp = (long*)(*(long*)(GameBaseAddres) + 0x22050);//OBS#S3
            int* monsterIDList = (int*)*wsp;
            int* count = (int*)(*(long*)(GameBaseAddres) + 0x0002A078);//OBS#S4

            for (int i = 0; i < *count; i++)
            {
                wholeNpcList.Add(GetObject<NPC>(*monsterIDList));
                monsterIDList++;
            }

            var sortedList = wholeNpcList.OrderBy(x => x.CalcDistance(wholeNpcList[0]));

            return sortedList.ToList();
        }

        public long GetInventoryItemDetails(long cItemAddres)
        {
            return getInventoryItemDetailsFunc(cItemAddres);
        }


        public List<IntPtr> getInventoryItems()
        {
            List<IntPtr> inventorySlotAddrs = new List<IntPtr>();
            //movsxd rax, dword ptr [rdi+000001B0]
            long* startList = (long*)(*(long*)((long)GameFunctionsAndObjects.DataFetch.GetPlayer().ObjectPointer + 0x6a78 + 0x18));//MSG#INV4

            for (long itemIndex = 0; itemIndex < 139; itemIndex++)
            {
                long slotAddres = *startList;
                inventorySlotAddrs.Add(new IntPtr(slotAddres));
                startList++;
            }
            return inventorySlotAddrs;
        }

        /// <summary>
        /// trose.exe+21CA3D 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="DialogBoxAdr"></param>
        /// <returns></returns>
        public List<IntPtr> getInventorySlots(int page,long DialogBoxAdr)
        {

            List<IntPtr> inventorySlotAddrs = new List<IntPtr>();
            //movsxd rax, dword ptr [rdi+000001B0]
            int itemIndexStart = page * 0x1e; //0x1e max page Size

            long i = 0;
            for (long itemIndex = itemIndexStart; itemIndex < itemIndexStart + 0x1e; itemIndex++)
            {
                long slotAddres = (itemIndexStart + i) * 0x168;
                slotAddres += 0x2d78;
                slotAddres += DialogBoxAdr;
                Console.WriteLine($"{itemIndex.ToString("X")} {slotAddres.ToString("X")}");
                inventorySlotAddrs.Add(new IntPtr(slotAddres));
                i++;
            }
            return inventorySlotAddrs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IObject> GetItemsAroundPlayer()
        {
            List<IObject> itemList = new List<IObject>();

            for (ushort i = 0; i < 0xFFFF; i++)
            {
                long* itemPointer = (long*)GameFunctionsAndObjects.DataFetch.GetItemPointer(i);
                if ((long)itemPointer != 0)
                {
                    Item nearItem = new Item(i, itemPointer);
                    itemList.Add(nearItem);
                }
            }

            return itemList;
        }



        public Int64 GetItemPointer(ushort index)
        {
            return getItemFunc(GameBaseAddres, *(int*)(*(long*)(GameBaseAddres) + (2 * index) + 0x0c));
        }

    }
}
