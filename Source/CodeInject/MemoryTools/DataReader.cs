using CodeInject.Actors;
using CodeInject.Party;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="npcID">Its that same index as we have in INPcList in form</param>
        /// <returns></returns>
        public delegate long GetPartyMemberDetailsAdr(long arg0,short npcID);

        private GetItemAdr getItemFunc;
        private GetInventoryItemDetailsAdr getInventoryItemDetailsFunc;
        private GetPartyMemberDetailsAdr getPartyMemberDetailsFunc;

        public long BaseAddres;
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
            getInventoryItemDetailsFunc = (GetInventoryItemDetailsAdr)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres+ 0x3b7800), typeof(GetInventoryItemDetailsAdr)); //MSG#INV8
          //  getPartyMemberDetailsFunc = (GetPartyMemberDetailsAdr)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres + 0x1b7c5), typeof(GetPartyMemberDetailsAdr));
        }


        public List<InvItem> GetConsumableItemsFromInventory(List<InvItem> currentList)
        {
            List<IntPtr> itemsAddrs = GameFunctionsAndObjects.DataFetch.getInventoryItems();
            List<InvItem> invDescriptions = new List<InvItem>();

            foreach (IntPtr item in itemsAddrs)
            {
                if (item.ToInt64() != 0x0)
                {
                    InvItem inv = new InvItem((long*)GameFunctionsAndObjects.DataFetch.GetInventoryItemDetails((item.ToInt64())), (long*)item.ToInt64());
                    invDescriptions.Add(inv);
                }
            }

            //ADD NEW
            foreach (InvItem item in invDescriptions)
            {
                if (*item.ItemType == 0xA && !currentList.Any(x => (long)x.ObjectPointer == (long)item.ObjectPointer))
                {
                    currentList.Add(item);
                }
            }
            //REMOVE OLD
            currentList.RemoveAll(a => !invDescriptions.Any(b => (long)b.ObjectPointer == (long)a.ObjectPointer));
            return currentList;
        }

        public IObject GetPartyMemberDetails(PartyMember member)
        {
            int partyMemberId = *(int*)(member.MemberAddres + 0x08);

            long rcx = *(long*)(BaseAddres + 0x1217268);
            short edx = *(short*)(rcx + partyMemberId * 2 + 0x0c); //so we multiply by 2 so partyMemberId should be short, we will see if its going to work

            long addr = getPartyMemberDetailsFunc(rcx, edx);


            return new NPC(&addr);
        }

        public List<PartyMember> GetPartyMembersList()
        {
            long* PartyMemberDataAddres = (long*)*(long*)MemoryTools.GetInt64(GameFunctionsAndObjects.DataFetch.BaseAddres + 0x0121A130, new short[] { 0x0, 0x10, 0x08 });

            int partyMemberCount = *(int*)(GameFunctionsAndObjects.DataFetch.BaseAddres + 0x121A170);

            List<PartyMember> PartyMemberList = new List<PartyMember>();

            for (int i = 0; i < partyMemberCount; i++)
            {
                long* currentMember = (long*)*PartyMemberDataAddres; //selecting member

                PartyMember member = new PartyMember()
                {
                    MemberAddres = (long)currentMember,
                    MemberName = Marshal.PtrToStringAnsi(new IntPtr((long)currentMember + 0x10)),
                };


                member.PartyMemberObject = GetPartyMemberDetails(member);

                if((long)member.PartyMemberObject.ObjectPointer!=0x0)
                PartyMemberList.Add(member);


                PartyMemberDataAddres++; //move to next member
            }

            return PartyMemberList;
        }

        /// <summary>
        /// pattern 48 81 c1 d0 0c 00 00 0f b7 d7
        /// </summary>
        /// <returns></returns>
        public List<Skills> GetPlayerSkills()
        {
            List<Skills> skillList = new List<Skills>();

            ulong* adrPtr1 = (ulong*)(BaseAddres + 0x16AA2F0); //2023.10.04

            int s = 0;
            while (*(short*)(*adrPtr1 + ((ulong)s * 2) + 0x50 + 0xd70) != 0)//OBS#S2
            {
                short skillID = *(short*)(*adrPtr1 + ((ulong)s * 2) + 0x50 + 0xd70);
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
        /// <param name="id"></param>
        /// <returns></returns>
        public IObject GetObject<T>(int id)
        {
            long* wskObj = (long*)((*(long*)(GameBaseAddres)) + (id * 8) + 0x22080); //OBS#N3

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
            long* wsp = (long*)(*(long*)(GameBaseAddres) + 0x22058);
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
            long* wsp = (long*)(*(long*)(GameBaseAddres) + 0x22058);//OBS#S3
            int* monsterIDList = (int*)*wsp;
            int* count = (int*)(*(long*)(GameBaseAddres) + 0x0002A080);//OBS#S4

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
            //movsxd rax, dword ptr [rdi+000001B0] 2B 5E90
            long* startList = (long*)(*(long*)((long)GameFunctionsAndObjects.DataFetch.GetPlayer().ObjectPointer + 0x6b18 + 0x18));//MSG#INV4


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
        /// <param name="dialogBoxAdr"></param>
        /// <returns></returns>
        public List<IntPtr> getInventorySlots(int page,long dialogBoxAdr)
        {

            List<IntPtr> inventorySlotAddrs = new List<IntPtr>();
            //movsxd rax, dword ptr [rdi+000001B0]
            int itemIndexStart = page * 0x1e; //0x1e max page Size

            long i = 0;
            for (long itemIndex = itemIndexStart; itemIndex < itemIndexStart + 0x1e; itemIndex++)
            {
                long slotAddres = (itemIndexStart + i) * 0x168;
                slotAddres += 0x2d78;
                slotAddres += dialogBoxAdr;
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
        public List<IObject> GetItemsAroundPlayerV2()
        {
            List<IObject> itemList = new List<IObject>();

            Process _proc = Process.GetCurrentProcess();

            long* RDI = (long*)(*(long*)(_proc.MainModule.BaseAddress.ToInt64() + 0x16ac678));
            long* RBX = (long*)((long)RDI + 0x2a0b8);

            RBX = (long*)*RBX; //select first item from list

            while ((long)RBX != 0x0)
            {
                long* itemAddr = (long*)*(long*)((long)RDI + ((*(short*)RBX) * 8) + 0x22080);
                Item nearItem = new Item(itemAddr);
                itemList.Add(nearItem);
                RBX = (long*)*((long*)((long)RBX+0x8));
            }

            return itemList;
        }

    }
}
