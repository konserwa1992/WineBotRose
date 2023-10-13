using CodeInject.Actors;
using CodeInject.BotStates;
using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CodeInject.WineBot
{
   public unsafe class WineBot
    {
        public static WineBot Instance { get; private set; } = new WineBot();
        public BotContext BotContext { get; set; } = new BotContext();
        public List<Skills> BotBuffs = new List<Skills>(); //Buffs to change

        public List<InvItem> ConsumeItems = new List<InvItem>();


        public int SkillIndex = 0;
        public IObject Target;

        private WineBot() 
        {
        }


        public List<InvItem> UpdateConsumeList()
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
                if (*item.ItemType == 0xA && !ConsumeItems.Any(x=>(long)x.ObjectPointer==(long)item.ObjectPointer))
                {
                   ConsumeItems.Add(item);
                }
            }
            //REMOVE OLD
            ConsumeItems.RemoveAll(a => !invDescriptions.Any(b => (long)b.ObjectPointer == (long)a.ObjectPointer));
            return ConsumeItems;
        }
        public void Update()
        {
            BotContext.Update();
        }
        public void Start()
        {
            Target = null;
        }
        public unsafe void UseBuffs()
        {
            foreach (Skills skill in BotBuffs)
            {
                    GameFunctionsAndObjects.Actions.CastSpell(GetSkillIndex(skill.skillInfo.ID));
                    Thread.Sleep(1250);
            }
        }
        public int GetSkillIndex(int SkillID)
        {
            return PlayerCharacter.GetPlayerSkills.FindIndex(x => x.skillInfo.ID == SkillID);
        }

    }
}
