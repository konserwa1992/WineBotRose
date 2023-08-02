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
        private GetItemAdr getItemFunc;


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
            GameBaseAddres = MemoryTools.GetVariableAddres("45 0F 57 DB 0F 1F 44 00 00 4C 8B 0D ?? ?? ?? ??").ToInt64();
            getItemFunc = (GetItemAdr)Marshal.GetDelegateForFunctionPointer(MemoryTools.GetCallAddress("48 8B 0D ?? ?? ?? ?? 0F B7 DD 0F BF 54 59 0C E8 ?? ?? ?? ??"), typeof(GetItemAdr));
        }

        /// <summary>
        /// pattern 48 81 c1 d0 0c 00 00 0f b7 d7
        /// </summary>
        /// <returns></returns>
        public List<Skills> GetPlayerSkills()
        {
            List<Skills> skillList = new List<Skills>();

            ulong* adrPtr1 = (ulong*)(BaseAddres + 0x1118E90);
            int s = 0;
            while (*(short*)(*adrPtr1 + ((ulong)s * 2) + 0x50 + 0xCD0) != 0)
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
            long* wskObj = (long*)((*(long*)(GameBaseAddres)) + (ID * 8) + 0x22078);

            if (typeof(T) == typeof(NPC))
                return new NPC(wskObj);

            if (typeof(T) == typeof(Item))
                return new Item(wskObj);

            return null;
        }

        /// <summary>
        /// Pattern 45 33 c0 8b 14 88
        /// 
        /// Pattern is that same as it is in GetNPCs because first element of entry is player
        /// </summary>
        /// <returns></returns>
        public IObject GetPlayer()
        {
            long* wsp = (long*)(*(long*)(GameBaseAddres) + 0x22050);
            int* monsterIDList = (int*)*wsp;

            return GetObject<NPC>(*monsterIDList);
        }

        /// <summary>
        /// Pattern 45 33 c0 8b 14 88
        /// </summary>
        /// <returns></returns>
        public List<IObject> GetNPCs()
        {

            List<IObject> wholeNpcList = new List<IObject>();
            long* wsp = (long*)(*(long*)(GameBaseAddres) + 0x22050);
            int* monsterIDList = (int*)*wsp;
            int* count = (int*)(*(long*)(GameBaseAddres) + 0x0002A078);

            for (int i = 0; i < *count; i++)
            {
                wholeNpcList.Add(GetObject<NPC>(*monsterIDList));
                monsterIDList++;
            }

            var sortedList = wholeNpcList.OrderBy(x => x.CalcDistance(wholeNpcList[0]));

            return sortedList.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IObject> GetItemsAroundPlayer()
        {
            List<IObject> itemList = new List<IObject>();

            for (ushort i = 0xFFFF; i > 0; i--)
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
            return getItemFunc(new IntPtr(GameBaseAddres).ToInt64(), *(int*)(*(long*)(GameBaseAddres) + (2 * index) + 0x0c));
        }

    }
}
