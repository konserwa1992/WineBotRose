using CodeInject.Actors;
using CodeInject.Party;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CodeInject.MemoryTools
{
    public unsafe class DataFetcher
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

        private GetInventoryItemDetailsAdr getInventoryItemDetailsFunc;
        private GetPartyMemberDetailsAdr getPartyMemberDetailsFunc;

        public long BaseAddres;
        private long GameBaseAddres;


        public DataFetcher()
        {
            Init();
        }

  

        private void Init()
        {
            Process _proc = Process.GetCurrentProcess();

            BaseAddres = _proc.MainModule.BaseAddress.ToInt64();
     

           GameBaseAddres = MemoryTools.GetVariableAddres("83 f8 07 0f 8f ?? ?? ?? ?? 48 63 0f 48 8b 05 ?? ?? ?? ??").ToInt64(); //UOB#U6
           getInventoryItemDetailsFunc = (GetInventoryItemDetailsAdr)Marshal.GetDelegateForFunctionPointer((IntPtr)MemoryTools.GetFunctionAddress("48 89 5c 24 08 57 48 83 ec ?? 48 8b f9 48 8d 59 50 e8 ?? ?? ?? ?? 83 f8 ?? 75 ?? 48 8b 0d ?? ?? ?? ??"), typeof(GetInventoryItemDetailsAdr)); //MSG#INV8
           Console.WriteLine($"DataReader Init");
       
            //  getPartyMemberDetailsFunc = (GetPartyMemberDetailsAdr)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres + 0x1b7c5), typeof(GetPartyMemberDetailsAdr));
        }


        public List<InvItem> GetAllItemsFromInventory(List<InvItem> currentList)
        {
            List<IntPtr> itemsAddrs = GameHackFunc.Game.ClientData.getInventoryItems();
            List<InvItem> invDescriptions = new List<InvItem>();

            foreach (IntPtr item in itemsAddrs)
            {
                if (item.ToInt64() != 0x0)
                {
                    InvItem inv = new InvItem((long*)GameHackFunc.Game.ClientData.GetInventoryItemDetails((item.ToInt64())), (long*)item.ToInt64());
                    invDescriptions.Add(inv);
                }
            }

            //ADD NEW
            foreach (InvItem item in invDescriptions)
            {
                if (!currentList.Any(x => (long)x.ObjectPointer == (long)item.ObjectPointer))
                {
                    currentList.Add(item);
                }
            }
            //REMOVE OLD
            currentList.RemoveAll(a => !invDescriptions.Any(b => (long)b.ObjectPointer == (long)a.ObjectPointer));
            return currentList;
        }

        public List<InvItem> GetConsumableItemsFromInventory(List<InvItem> currentList)
        {
            List<IntPtr> itemsAddrs = GameHackFunc.Game.ClientData.getInventoryItems();
            List<InvItem> invDescriptions = new List<InvItem>();

            foreach (IntPtr item in itemsAddrs)
            {
                if (item.ToInt64() != 0x0)
                {
                    InvItem inv = new InvItem((long*)GameHackFunc.Game.ClientData.GetInventoryItemDetails((item.ToInt64())), (long*)item.ToInt64());
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
            long* PartyMemberDataAddres = (long*)*(long*)MemoryTools.GetInt64(GameHackFunc.Game.ClientData.BaseAddres + 0x0121A130, new short[] { 0x0, 0x10, 0x08 });

            int partyMemberCount = *(int*)(GameHackFunc.Game.ClientData.BaseAddres + 0x121A170);

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

            ulong* adrPtr1 = (ulong*)(BaseAddres + 0x1578d10); //2023.10.04

            int s = 0;
            while (*(short*)(*adrPtr1 + ((ulong)s * 2) + 0x50 + 0xb68) != 0)//OBS#S2
            {
                ushort skillID = *(ushort*)(*adrPtr1 + ((ulong)s * 2) + 0x50 + 0xb68);
         
                SkillInfo skill = DataBase.GameDataBase.SkillDatabase.FirstOrDefault(x => x.ID == skillID);
                if (skill == null)
                {
                    skill = new SkillInfo()
                    {
                        ID = skillID,
                        Name = "Unknow"
                    };
                }
                skillList.Add(new Skills(skill,SkillTypes.Unknow));

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
        public IObject GetObject(int id)
        {
            long* wskObj = (long*)((*(long*)(GameBaseAddres)) + (id * 8) + 0x22088); //OBS#N3
    
      

            long ObjectTypeFuncTable = *(long*)*wskObj;
           // MessageBox.Show(((long)wskObj).ToString("X"));



            if (GameHackFunc.Game.ClientData.BaseAddres + 0x1058060 == ObjectTypeFuncTable)
                    return new OtherPlayer(wskObj);
            if (GameHackFunc.Game.ClientData.BaseAddres + 0x1059B00 == ObjectTypeFuncTable) // player avatar
                   return new Player(wskObj);
            if (GameHackFunc.Game.ClientData.BaseAddres + 0x1057130 == ObjectTypeFuncTable) // mobs and npcs
                return new NPC(wskObj);

            return new NPC(wskObj);
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
            int * monsterIDList = (int*)*wsp;

            return (Player)GetObject(*monsterIDList);
        }


        /// <summary>
        /// Pattern 45 33 c0 8b 14 88
        /// </summary>
        /// <returns></returns>
        public List<IObject> GetNPCs()
        {
            try
            {
                List<IObject> wholeNpcList = new List<IObject>();
                long* wsp = (long*)(*(long*)(GameBaseAddres) + 0x22058);//OBS#S3
                int* monsterIDList = (int*)*wsp;
                int* count = (int*)(*(long*)(GameBaseAddres) + 0x2A088);//OBS#S4

                for (int i = 0; i < *count; i++)
                {
                    IObject obj = GetObject(*monsterIDList);
                    if (obj != null)
                    {
                        wholeNpcList.Add(GetObject(*monsterIDList));
                    }
                    monsterIDList++;
                }

                var sortedList = wholeNpcList.OrderBy(x => x.CalcDistance(wholeNpcList[0]));
                return sortedList.ToList();
            }
            catch(Exception e)
            {
                return new List<IObject>();
            }
      
        }

        public long GetInventoryItemDetails(long cItemAddres)
        {
            return getInventoryItemDetailsFunc(cItemAddres);
        }


        public List<IntPtr> getInventoryItems()
        {
            List<IntPtr> inventorySlotAddrs = new List<IntPtr>();
            //movsxd rax, dword ptr [rdi+000001B0] 2B 5E90
            long* startList = (long*)(*(long*)((long)GameHackFunc.Game.ClientData.GetPlayer().ObjectPointer + 0x6908 + 0x20));//MSG#INV4



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

            long* RDI = (long*)*(long*)(GameBaseAddres);
            long * RBX = (long*)(*(long*)(GameBaseAddres) + 0x2a0c0);
          //  long * RBX = (long*)((long)RDI + 0x2a0b8);

            RBX = (long*)*RBX; //select first item from list

            while ((long)RBX != 0x0)
            {
                long* itemAddr = (long*)*(long*)((long)RDI + ((*(short*)RBX) * 8) + 0x22088);
                Item nearItem = new Item(itemAddr);
                itemList.Add(nearItem);
                RBX = (long*)*((long*)((long)RBX+0x8));
            }

            return itemList;
        }

    }
}
