﻿using CodeInject.Actors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject
{
    /// <summary>
    /// Game Methods to use.
    /// </summary>
    public unsafe class Actions
    {
        public delegate Int64 AttackWithSkill(int skill, int enemy, int arg0);
        public delegate Int64 PickUpAction(long arg1, long arg2, int arg3, int arg4);

        private PickUpAction PickUpFunc;// = (PickUpAction)Marshal.GetDelegateForFunctionPointer(new IntPtr(Process.GetCurrentProcess().MainModule.BaseAddress.ToInt64() + 0x26e90), typeof(PickUpAction));
        private AttackWithSkill AttackWithSkillFunc;
        public Actions()
        {
            Init();
        }

        public void Init()
        {
            Process _proc = Process.GetCurrentProcess();
            PickUpFunc = (PickUpAction)Marshal.GetDelegateForFunctionPointer(new IntPtr(_proc.MainModule.BaseAddress.ToInt64() + 0x26e90), typeof(PickUpAction));
            AttackWithSkillFunc = (AttackWithSkill)Marshal.GetDelegateForFunctionPointer(new IntPtr(_proc.MainModule.BaseAddress.ToInt64() + 0x4260E), typeof(AttackWithSkill));
        }


        public void PickUp(int ItemID)
        {
            Process _proc = Process.GetCurrentProcess();
            PickUpFunc((*(long*)(_proc.MainModule.BaseAddress.ToInt64() + 0x1122EF0) + 0x16D8), *((long*)(_proc.MainModule.BaseAddress.ToInt64() + 0x1118E60)), ItemID, 0);
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

    public unsafe class DataReader
    {
        /// <summary>
        /// pattern 48 81 c1 d0 0c 00 00 0f b7 d7
        /// </summary>
        /// <returns></returns>
        public List<Skills> GetPlayerSkills()
        {
            List<Skills> skillList = new List<Skills>();

            Process _proc = Process.GetCurrentProcess();

            ulong * adrPtr1 = (ulong*)(_proc.MainModule.BaseAddress.ToInt64() + 0x1118E90);
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
            long* wskObj = (long*)((*(long*)(_proc.MainModule.BaseAddress.ToInt64() + 0x111be28)) + (ID * 8) + 0x22078);
            if(typeof(T) == typeof(NPC))
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

            long* wsp = (long*)(*(long*)(_proc.MainModule.BaseAddress.ToInt64() + 0x111be28) + 0x22050);
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
            long * wsp = (long*)(*(long*)(_proc.MainModule.BaseAddress.ToInt64() + 0x111be28) + 0x22050);
            int* monsterIDList = (int*)*wsp;
            int* count = (int*)(*(long*)(_proc.MainModule.BaseAddress.ToInt64() + 0x111be28) + 0x0002A078);

            for (int i = 0; i < *count; i++)
            {
                wholeNpcList.Add(GetObject<NPC>(*monsterIDList));
                monsterIDList++;
            }

            var sortedList = wholeNpcList.OrderBy(x => x.CalcDistance(wholeNpcList[0]));

            return sortedList.ToList();
        }
    }

    internal class GameFunctionsAndObjects
    {
        public static Actions Actions { get; private set; } = new Actions();
        public static DataReader DataFetch { get; private set; } = new DataReader();

        private GameFunctionsAndObjects() { }
    }
}
