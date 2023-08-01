using CodeInject.Actors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CodeInject.Actions;

namespace CodeInject
{
    /// <summary>
    /// Game Methods to use.
    /// </summary>
    public unsafe class Actions
    {
        public delegate Int64 AttackWithSkill(int skill, int enemy, int arg0);
        public delegate Int64 PickUpAction(long arg1, long arg2, int arg3, int arg4);

        private PickUpAction PickUpFunc;
        private AttackWithSkill AttackWithSkillFunc;
        private long BaseAddres;

        public Actions()
        {
            Init();
        }
        public void Init()
        {
            BaseAddres = Process.GetCurrentProcess().MainModule.BaseAddress.ToInt64();
            PickUpFunc = (PickUpAction)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres + 0x26e90), typeof(PickUpAction));
            AttackWithSkillFunc = (AttackWithSkill)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres + 0x4260E), typeof(AttackWithSkill));
        }
        public void PickUp(int ItemID)
        {
            Process _proc = Process.GetCurrentProcess();
            PickUpFunc((*(long*)(BaseAddres + 0x1122EF0) + 0x16D8), *((long*)(BaseAddres + 0x1118E60)), ItemID, 0);
        }

        /// <summary>
        /// Pattern: 4c 8d 44 24 20 8b d0
        /// </summary>
        /// <param name="TargedID"></param>
        /// <param name="SkillIndex"></param>
        public void Attack(int TargedID,int SkillIndex)
        {
            AttackWithSkillFunc(SkillIndex, TargedID, 0);
        }

    }

    /// <summary>
    /// Methods to get game data
    /// </summary>
    public unsafe class DataReader
    {
        private GetItemAdr getItemFunc;
        private long BaseAddres;

        public DataReader()
        {
            Init();
        }

        private void Init()
        {
            Process _proc = Process.GetCurrentProcess();

            BaseAddres = _proc.MainModule.BaseAddress.ToInt64();

            getItemFunc = (GetItemAdr)Marshal.GetDelegateForFunctionPointer(new IntPtr(BaseAddres + 0x40A89), typeof(GetItemAdr));
        }

        /// <summary>
        /// pattern 48 81 c1 d0 0c 00 00 0f b7 d7
        /// </summary>
        /// <returns></returns>
        public List<Skills> GetPlayerSkills()
        {
            List<Skills> skillList = new List<Skills>();

            Process _proc = Process.GetCurrentProcess();

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
        public IActor GetObject<T>(int ID)
        {
            Process _proc = Process.GetCurrentProcess();
            long* wskObj = (long*)((*(long*)(BaseAddres + 0x111be28)) + (ID * 8) + 0x22078);
            if (typeof(T) == typeof(NPC))
                return new NPC(wskObj);

            return null;
        }

        /// <summary>
        /// Pattern 45 33 c0 8b 14 88
        /// 
        /// Pattern is that same as it is in GetNPCs because first element of entry is player
        /// </summary>
        /// <returns></returns>
        public IActor GetPlayer()
        {
            Process _proc = Process.GetCurrentProcess();

            long* wsp = (long*)(*(long*)(BaseAddres + 0x111be28) + 0x22050);
            int* monsterIDList = (int*)*wsp;

            return GetObject<NPC>(*monsterIDList);
        }

        /// <summary>
        /// Pattern 45 33 c0 8b 14 88
        /// </summary>
        /// <returns></returns>
        public List<IActor> GetNPCs()
        {
            Process _proc = Process.GetCurrentProcess();

            List<IActor> wholeNpcList = new List<IActor>();
            long* wsp = (long*)(*(long*)(BaseAddres + 0x111be28) + 0x22050);
            int* monsterIDList = (int*)*wsp;
            int* count = (int*)(*(long*)(BaseAddres + 0x111be28) + 0x0002A078);

            for (int i = 0; i < *count; i++)
            {
                wholeNpcList.Add(GetObject<NPC>(*monsterIDList));
                monsterIDList++;
            }

            var sortedList = wholeNpcList.OrderBy(x => x.CalcDistance(wholeNpcList[0]));

            return sortedList.ToList();
        }

        /// <summary>
        /// DONT WORK
        /// </summary>
        /// <returns></returns>
        public List<IActor> GetItems()
        {
            Process _proc = Process.GetCurrentProcess();

            List<IActor> wholeNpcList = new List<IActor>();
            long* wsp = (long*)(*(long*)(BaseAddres + 0x111be28) + 0x2000A);
            int* monsterIDList = (int*)*wsp;
            int* count = (int*)(*(long*)(BaseAddres + 0x111be28) + 0x0002A078);

            for (int i = 0; i < *count; i++)
            {
                wholeNpcList.Add(GetObject<NPC>(*monsterIDList));
                monsterIDList++;
            }

            var sortedList = wholeNpcList.OrderBy(x => x.CalcDistance(wholeNpcList[0]));

            return sortedList.ToList();
        }

        /// <summary>
        /// Im not sure what kind of index is. Propobly some kind index of clickable object?
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="idObj"></param>
        /// <returns></returns>
        public delegate Int64 GetItemAdr(long arg1, int index);

        public  Int64 GetItemPointer(ushort index)
        {
            Process _proc = Process.GetCurrentProcess();
   
            return getItemFunc(new IntPtr(BaseAddres + 0x111BE28).ToInt64(), *(int*)(*(long*)(BaseAddres + 0x111BE28) + (2 * index) + 0x0c));
        }

    }

    internal class GameFunctionsAndObjects
    {
        public static Actions Actions { get; private set; } = new Actions();
        public static DataReader DataFetch { get; private set; } = new DataReader();

        private GameFunctionsAndObjects() { }
    }
}
