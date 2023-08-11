using CodeInject.Actors;
using CodeInject.MemoryTools;
using CodeInject.PickupFilters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.WineBot
{
   public unsafe class WineBot
    {
        public List<IObject> NpcAround = new List<IObject>();
        public List<Skills> BotSkills = new List<Skills>(); //Skills what gonna be used by bot
        public List<InvItem> ConsumeItems = new List<InvItem>();
        public List<IObject> ItemAround = new List<IObject>();

        public IFilter filter = new QuickFilter();

        public int SkillIndex = 0;
        public IObject Target;

        Potion AutoHp;
        Potion AutoMp;

        public WineBot() 
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
            List<IntPtr> items = GameFunctionsAndObjects.DataFetch.getInventoryItems();

            foreach (IntPtr item in items)
            {
                if (item.ToInt64() != 0x0)
                {
                    InvItem inv = new InvItem((long*)GameFunctionsAndObjects.DataFetch.GetInventoryItemDetails((item.ToInt64())), (long*)item.ToInt64());


                    if (*inv.ItemType == 0xA)
                    {
                        ConsumeItems.Add(inv);
                    }
                }
            }

            return ConsumeItems;
        }

        public void SetAutoHPpotion(int colddawn,InvItem item)
        {
            AutoHp = new Potion(colddawn, item);
        }

        public void SetAutoMPpotion(int colddawn, InvItem item)
        {
            AutoMp = new Potion(colddawn, item);
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
                this.Target = this.NpcAround[0];
                Skills Skill2Cast = PlayerCharacter.GetPlayerSkills.FirstOrDefault(x => x.skillInfo.ID == this.BotSkills[this.SkillIndex].skillInfo.ID);
                GameFunctionsAndObjects.Actions.Attack(*this.Target.ID,
                    PlayerCharacter.GetPlayerSkills.FindIndex(x => x.skillInfo.ID == Skill2Cast.skillInfo.ID)
                    );
            }
        }

        public void AutoPotionFunction(int minHpp,int minMpp)
        {
      

            if ((((float)*(GameFunctionsAndObjects.DataFetch.GetPlayer()).Hp / *(GameFunctionsAndObjects.DataFetch.GetPlayer()).MaxHp) * 100) < minHpp)
            {
                AutoHp.Use();
            }

            if ((((float)*(GameFunctionsAndObjects.DataFetch.GetPlayer()).Mp / *(GameFunctionsAndObjects.DataFetch.GetPlayer()).MaxMp) * 100) < minMpp)
            {
                AutoMp.Use();
            }
        }

        public void PickClosestItem()
        {
            if(ItemAround.Count > 0)
             ((Item)ItemAround[0]).Pickup();
        }
    }
}
