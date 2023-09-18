using CodeInject.Actors;
using CodeInject.MemoryTools;
using CodeInject.PickupFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeInject.WineBot
{
   public unsafe class WineBot
    {
        public static WineBot Instance { get; private set; } = new WineBot();

        public List<IObject> NpcAround = new List<IObject>();
        public List<Skills> BotSkills = new List<Skills>(); //Skills what gonna be used on monsters
        public List<Skills> BotBuffs = new List<Skills>(); //Buffs
        public List<InvItem> ConsumeItems = new List<InvItem>();
        public List<IObject> ItemAround = new List<IObject>();

        public IFilter filter = new QuickFilter();

        public int SkillIndex = 0;
        public IObject Target;

        public ItemExecutor AutoHp;
        public ItemExecutor AutoMp;

        private WineBot() 
        {
      
        }


        public List<IObject> UpdateItemsAroundPlayer(int radius)
        {
            IObject player = GameFunctionsAndObjects.DataFetch.GetPlayer();
            ItemAround = GameFunctionsAndObjects.DataFetch.GetItemsAroundPlayer().Where(x => filter.CanPickup(x) && x.CalcDistance(player) < radius).OrderBy(x => x.CalcDistance(player)).ToList();
            return ItemAround;
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

        public void SetAutoHPpotion(int minHelathProc,int colddawn, InvItem item)
        {
   
            AutoHp = new ItemExecutor(colddawn, minHelathProc, item);
        }

        public void SetAutoMPpotion(int minManaProc, int colddawn, InvItem item)
        {
            AutoMp = new ItemExecutor(colddawn, minManaProc, item);
        }

        public void AttackClosestMonster()
        {
            if (this.SkillIndex < this.BotSkills.Count - 1)
            {
                this.SkillIndex++;
            }
            else
            {
                this.SkillIndex = 0;
            }

            if (this.NpcAround.Count > 0)
            {
                if (Target == null || *((NPC)Target).Hp <= 0)
                {
                    this.Target = this.NpcAround.FirstOrDefault(x => *(((NPC)x).Hp) > 0); //select closest monster with have over 0 hp
                }

                if (this.Target != null)
                {
                    if (this.BotSkills.Count > 0)
                    {
                        Skills Skill2Cast = PlayerCharacter.GetPlayerSkills.FirstOrDefault(x => x.skillInfo.ID == this.BotSkills[this.SkillIndex].skillInfo.ID);
                        GameFunctionsAndObjects.Actions.CastSpell(*this.Target.ID, GetSkillIndex(Skill2Cast.skillInfo.ID));
                    }

                    GameFunctionsAndObjects.Actions.Attack(*this.Target.ID);
                }
            }
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

        public void AutoPotionFunction(int minHpp,int minMpp)
        {
                AutoHp.Use((((float)*(GameFunctionsAndObjects.DataFetch.GetPlayer()).Hp / *(GameFunctionsAndObjects.DataFetch.GetPlayer()).MaxHp) * 100));
                AutoMp.Use((((float)*(GameFunctionsAndObjects.DataFetch.GetPlayer()).Mp / *(GameFunctionsAndObjects.DataFetch.GetPlayer()).MaxMp) * 100));
        }

        public void PickClosestItem()
        {
            if(ItemAround.Count > 0)
             ((Item)ItemAround[0]).Pickup();
        }
    }
}
