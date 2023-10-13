using CodeInject.Actors;
using CodeInject.MemoryTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp.Server;
using WebSocketSharp;
using CodeInject.WebServ.Models;
using CodeInject.PickupFilters;
using CodeInject.WebServ.Models.PickUpFilter;
using CodeInject.BotStates;

namespace CodeInject
{
    public unsafe class WebSocketServices
    {

        public class MyWebSocketService : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                Send($"{GameFunctionsAndObjects.DataFetch.GetPlayer().ToString()}");
            }

        }

        public class NPCService : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                List<IObject> list = GameFunctionsAndObjects.DataFetch.GetNPCs();

                List<object> toSerialzie = new List<object>();
                NPC last = null;
                foreach (IObject npc in list)
                {
                    last = (NPC)npc;
                    toSerialzie.Add(((NPC)npc).ToWSObject());
                }

                Send($"{JsonConvert.SerializeObject(toSerialzie)}");


                if (WineBot.WineBot.Instance.Target != null)
                {
                    Send(JsonConvert.SerializeObject(
                        new TargetInfoModel()
                        {
                            AttackedNPC = ((NPC)WineBot.WineBot.Instance.Target).ToWSObject()
                        }));
                }
            }
        }


        public class SkillService : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                if (e.Data.Contains("setSkills"))
                {
                    SetSkillsModel newSkillSet = JsonConvert.DeserializeObject<SetSkillsModel>(e.Data);

                    ((HuntState)WineBot.WineBot.Instance.BotContext.States["HUNT"]).HuntInstance.BotSkills.RemoveAll(x => 1 == 1);

                    foreach (int skillId in newSkillSet.setSkills)
                    {
                        ((HuntState)WineBot.WineBot.Instance.BotContext.States["HUNT"]).HuntInstance.AddSkill(Skills.GetSkillByID(skillId));
                    }
                }
                else if (e.Data.Contains("GetSkills"))
                {
                    PlayerSkillModel skillsList = new PlayerSkillModel();
                    foreach (Skills singleSkill in PlayerCharacter.GetPlayerSkills)
                    {
                        if (!((HuntState)WineBot.WineBot.Instance.BotContext.States["HUNT"]).HuntInstance.BotSkills.Any(x => x.skillInfo.ID == singleSkill.skillInfo.ID))
                            skillsList.UnUsedSkillList.Add(singleSkill.ToWSObject());

                        if (((HuntState)WineBot.WineBot.Instance.BotContext.States["HUNT"]).HuntInstance.BotSkills.Any(x => x.skillInfo.ID == singleSkill.skillInfo.ID))
                            skillsList.SkillInUseList.Add(singleSkill.ToWSObject());
                    }

                    Send($"{JsonConvert.SerializeObject(skillsList)}");
                }
            }
        }


        public class PickUpFilterService : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                if (e.Data.Contains("GetFilter"))
                {
                    var pickUpFilter = new SimpleFilterModel()
                    {
                        Name = "Simple",
                        Filter = ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).pickTypeList
                    };

                    Send($"{JsonConvert.SerializeObject((object)pickUpFilter)}");
                }
            }
        }


        public class AutoPotionService : WebSocketBehavior
        {
            protected override void OnOpen()
            {
                List<IntPtr> items = GameFunctionsAndObjects.DataFetch.getInventoryItems();

                List<ItemModel> ItemToSend = new List<ItemModel>();

                foreach (IntPtr item in items)
                {
                    if (item.ToInt64() != 0x0)
                    {
                        InvItem inv = new InvItem((long*)GameFunctionsAndObjects.DataFetch.GetInventoryItemDetails((item.ToInt64())), (long*)item.ToInt64());

                        if (*inv.ItemType == 0xA)
                        {
                            ItemToSend.Add(inv.ToWSObject());
                        }
                    }
                }


                /*
                 * UNCOMMENT
                if (WineBot.WineBot.Instance.AutoHp == null || WineBot.WineBot.Instance.AutoMp == null)
                {
                    Send(JsonConvert.SerializeObject(new AutoPotionSettingsModel()
                    {
                        ItemsList = ItemToSend,
                        MinHelath = 0,
                        MinMana = 0,
                        HealthItemIndex = 0,
                        ManaItemIndex = 0,
                        HelathDurration = 0,
                        ManaDurration = 0
                    }));
                    return;
                }


                Send(JsonConvert.SerializeObject(new AutoPotionSettingsModel()
                {
                    ItemsList = ItemToSend,
                    MinHelath = WineBot.WineBot.Instance.AutoHp.MinValueToExecute,
                    MinMana = WineBot.WineBot.Instance.AutoMp.MinValueToExecute,
                    HealthItemIndex = ItemToSend.FindIndex(x => x.Id == (long)WineBot.WineBot.Instance.AutoHp.Item2Cast.ObjectPointer),
                    ManaItemIndex = ItemToSend.FindIndex(x => x.Id == (long)WineBot.WineBot.Instance.AutoMp.Item2Cast.ObjectPointer),
                    HelathDurration = WineBot.WineBot.Instance.AutoHp.CooldDown,
                    ManaDurration = WineBot.WineBot.Instance.AutoMp.CooldDown
                }));
                */
                base.OnOpen();
            }
            protected override void OnMessage(MessageEventArgs e)
            {
                if (e.Data.Contains("GetAutoPotionSettings"))
                {
                   
                }
                if (e.Data.Contains("SetPotions"))
                {
                    dynamic setPotion = JsonConvert.DeserializeObject<dynamic>(e.Data);
                    InvItem[] items = WineBot.WineBot.Instance.UpdateConsumeList().ToArray();

                    //UNCOMMENT
                  //  WineBot.WineBot.Instance.SetAutoHPpotion((int)setPotion.procHelath, (int)setPotion.hpItemDurr, items[(int)setPotion.hpItemIndex]);
                  //  WineBot.WineBot.Instance.SetAutoMPpotion((int)setPotion.procMana, (int)setPotion.mpItemDurr, items[(int)setPotion.mpItemIndex]);
                }
            }
        }
    }
}
