using CodeInject.Actors;
using CodeInject.BotStates;
using CodeInject.Hunt;
using CodeInject.MemoryTools;
using CodeInject.Modules;
using CodeInject.Party;
using CodeInject.PickupFilters;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Windows.Forms;



namespace CodeInject
{
    public unsafe partial class cBot : Form
    {
        WebServer ws = new WebServer();


        Party.Party party;

        public cBot()
        {
            InitializeComponent();
            ws.SetupWebSocketServer();
        }

        private void bSkillRefresh_Click(object sender, EventArgs e)
        {
            lSkillList.Items.Clear();
            lSkillList.Items.AddRange(PlayerCharacter.GetPlayerSkills.ToArray());
        }

        private void bSkillAdd_Click(object sender, EventArgs e)
        {
            if(lSkillList.SelectedIndex!=-1)
            ((HuntState)WineBot.WineBot.Instance.BotContext.States["HUNT"]).HuntInstance.AddSkill((Skills)lSkillList.SelectedItem);
        }

        private void bSkillRemove_Click(object sender, EventArgs e)
        {
            if (lUseSkill.SelectedItem!=null)
                ((HuntState)WineBot.WineBot.Instance.BotContext.States["HUNT"]).HuntInstance.RemoveSkill((Skills)lUseSkill.SelectedItem);
        }


        public void SkillListUpdate()
        {
            GameFunctionsAndObjects.Actions.Logger($"SkillListUpdate", Color.Orange);
            lUseSkill.Items.Clear();
            lUseSkill.Items.AddRange(((HuntState)WineBot.WineBot.Instance.BotContext.States["HUNT"]).HuntInstance.BotSkills.ToArray());
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            WineBot.WineBot.Instance.Update();


            if(cEnableHealParty.Checked)
            {
                int minProcHp = int.Parse(textBox1.Text);
                party.Update();
                lPartyMembers.Items.AddRange(party.PartyMemberList.ToArray());

                PartyMember member = party.PartyMemberList.OrderBy(x => ((float)*((NPC)x.PartyMemberObject).Hp / *((NPC)x.PartyMemberObject).MaxHp * 100)).FirstOrDefault();

               if (member!=null && ((float)*((NPC)member.PartyMemberObject).Hp / *((NPC)member.PartyMemberObject).MaxHp * 100) < minProcHp)
               {
                    Skills Skill2Cast = PlayerCharacter.GetPlayerSkills.FirstOrDefault(x => x.skillInfo.ID == ((Skills)lHealSkills.Items[0]).skillInfo.ID);
                    GameFunctionsAndObjects.Actions.CastSpell(*member.PartyMemberObject.ID, WineBot.WineBot.Instance.GetSkillIndex(Skill2Cast.skillInfo.ID));
                }
            }
        }

        private void pickUpTimer_Tick(object sender, EventArgs e)
        {
            /*
            lNearItemsList.Items.Clear();

            lNearItemsList.Items.AddRange(WineBot.WineBot.Instance.UpdateItemsAroundPlayer(int.Parse(tPickupRadius.Text)).ToArray());


            if (cPickUpEnable.Checked)
               WineBot.WineBot.Instance.PickClosestItem();*/
        }

        private void lNearItemsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((Item)lNearItemsList.SelectedItem).Pickup();
         //   tPickupRadius.Text = ((long)((Item)lNearItemsList.SelectedItem).ObjectPointer).ToString("X");
         //   ((UsableItem)lNearItemsList.SelectedItem).Pickup();
         //   lNearItemsList.Items.Clear();
         //   lNearItemsList.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetItemsAroundPlayerV2().ToArray());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            lFullMonsterList.Items.Clear();
            lFullMonsterList.Items.AddRange(DataBase.GameDataBase.MonsterDatabase.Where(x =>x.Name!="" && x.Name.ToUpper().Contains(tSearchMobTextBox.Text.ToUpper())).ToArray());
        }

        private void bAddMonster2Attack_Click(object sender, EventArgs e)
        {
            if(lMonster2Attack.Items.Cast<MobInfo>().FirstOrDefault(x=>x.ID==((MobInfo)lFullMonsterList.SelectedItem).ID)==null)
            lMonster2Attack.Items.Add(lFullMonsterList.SelectedItem);
        }

        private void bRemoveMonster2Attack_Click(object sender, EventArgs e)
        {
            if(lMonster2Attack.SelectedIndex!=-1)
                lMonster2Attack.Items.Remove(lMonster2Attack.SelectedItem);
        }

        private void bSetArea_Click(object sender, EventArgs e)
        {
            IObject player = PlayerCharacter.PlayerInfo;

            tXHuntArea.Text = (*player.X).ToString();
            tYHuntArea.Text = (*player.Y).ToString();
            tZHuntArea.Text = (*player.Z).ToString();
        }

 
        private void cbHealHPItem_DropDown(object sender, EventArgs e)
        {
            cbHealHPItem.Items.Clear();
            cbHealHPItem.Items.AddRange(WineBot.WineBot.Instance.UpdateConsumeList().ToArray());
        }

        private void cbHealMPItem_DropDown(object sender, EventArgs e)
        {
            cbHealMPItem.Items.Clear();
            cbHealMPItem.Items.AddRange(WineBot.WineBot.Instance.UpdateConsumeList().ToArray());
        }

        private void bHuntToggle_Click_1(object sender, EventArgs e)
        {
            BuffTimer.Enabled = !BuffTimer.Enabled;

            WineBot.WineBot.Instance.Start();

            GameFunctionsAndObjects.Actions.Logger($"Bot is running: {timer2.Enabled}", Color.Orange);

            if(cHuntEnable.Checked)
               WineBot.WineBot.Instance.BotContext.Start(
                   new HuntState(
                       new DefaultHunt(lMonster2Attack.Items.Cast<MobInfo>().ToList(), 
                       new Vector3(float.Parse(tXHuntArea.Text), float.Parse(tYHuntArea.Text),
                       float.Parse(tZHuntArea.Text)), int.Parse(tHuntRadius.Text), lUseSkill.Items.OfType<Skills>().ToList(), this) 
                      ));



            if (cEnableHealParty.Checked)
            {
                party = new Party.Party();
            }

            bHuntToggle.Text = timer2.Enabled == true ? "STOP" : "START";
        }

        private void cFilterMaterials_CheckedChanged(object sender, EventArgs e)
        {
            if(cFilterMaterials.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).AddToPick(ItemType.Material);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).RemoveFromPick(ItemType.Material);
            }
        }

        private void cFilterArmor_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterArmor.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).AddToPick(ItemType.ChestArmor);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).RemoveFromPick(ItemType.ChestArmor);
            }
        }

        private void cFilterGloves_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterGloves.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).AddToPick(ItemType.Gloves);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).RemoveFromPick(ItemType.Gloves);
            }
        }

        private void cFilterHat_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterHat.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).AddToPick(ItemType.Hat);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).RemoveFromPick(ItemType.Hat);
            }
        }

        private void cFilterShoes_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterShoes.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).AddToPick(ItemType.Shoes);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).RemoveFromPick(ItemType.Shoes);
            }
        }

        private void cFilterUsable_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterUsable.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).AddToPick(ItemType.UsableItem);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).RemoveFromPick(ItemType.UsableItem);
            }
        }

        private void cFilterWeapon_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterWeapon.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).AddToPick(ItemType.Weapon);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).RemoveFromPick(ItemType.Weapon);
            }
        }

        private void cFilterShield_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterShield.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).AddToPick(ItemType.Shield);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.BotContext.Filter).RemoveFromPick(ItemType.Shield);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdvancedFilterForm advFilterWindow = new AdvancedFilterForm(WineBot.WineBot.Instance.BotContext.Filter);
            advFilterWindow.ShowDialog();
        }

        private void cAdvanceEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!cAdvanceEnable.Checked)
            {
                WineBot.WineBot.Instance.BotContext.Filter = new QuickFilter();
                SimpleFilterGroup.Controls.OfType<CheckBox>().ToList().ForEach(c => c.Checked = false);
            }
            else
            {
                WineBot.WineBot.Instance.BotContext.Filter = new AdvancedFilter();
            }
            SimpleFilterGroup.Enabled = !cAdvanceEnable.Checked;
            bAdvancedFilter.Enabled = cAdvanceEnable.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (MobInfo mob in  lFullMonsterList.Items)
            {
                if (!lMonster2Attack.Items.Cast<MobInfo>().Any(x => x.ID == mob.ID))
                    lMonster2Attack.Items.Add(mob);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(((long)GameFunctionsAndObjects.DataFetch.GetPlayer().ObjectPointer).ToString("X"));
        }

        private void cAutoPotionEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!cAutoPotionEnabled.Checked)
            {
                WineBot.WineBot.Instance.BotContext.RemoveModule("AUTOPOTION");
            }
            else
            {
                if (cbHealHPItem.SelectedIndex != -1 && cbHealMPItem.SelectedIndex != -1)
                {
                    AutoPotionModule autoPotion = (AutoPotionModule)WineBot.WineBot.Instance.BotContext.GetModule("AUTOPOTION");
                    if (autoPotion == null)
                        autoPotion = (AutoPotionModule)WineBot.WineBot.Instance.BotContext.AddModule(new AutoPotionModule());

                    autoPotion.SetAutoHPpotion(int.Parse(tHPPotionUseProc.Text), int.Parse(tHpDurr.Text), (InvItem)cbHealHPItem.SelectedItem);
                    autoPotion.SetAutoMPpotion(int.Parse(tMPPotionUseProc.Text), int.Parse(tMpDurr.Text), (InvItem)cbHealMPItem.SelectedItem);
                }
                else
                {
                    throw new Exception("Please Select both item in Autopotion option if you want to have it enabled");
                }
            }
        }

        private unsafe void cBot_Load(object sender, EventArgs e)
        {
            GameFunctionsAndObjects.Actions.Logger($"Hello.",Color.GreenYellow);
            GameFunctionsAndObjects.Actions.Logger($"Bot version {Assembly.GetExecutingAssembly().GetName().Version.ToString()}", Color.GreenYellow);
        }


        private void bBuffAdd_Click(object sender, EventArgs e)
        {
            if (!lUseBuffs.Items.Cast<Skills>().Any(x => x.skillInfo.ID == ((Skills)lSkillList.SelectedItem).skillInfo.ID))
                lUseBuffs.Items.Add(lSkillList.SelectedItem);


            if (!WineBot.WineBot.Instance.BotBuffs.Any(x => x.skillInfo.ID == ((Skills)lSkillList.SelectedItem).skillInfo.ID))
            {
                WineBot.WineBot.Instance.BotBuffs.Add((Skills)lSkillList.SelectedItem);
            }

            lUseBuffs.Items.Clear();
            lUseBuffs.Items.AddRange(WineBot.WineBot.Instance.BotBuffs.ToArray());
        }

        private void bBuffRemove_Click(object sender, EventArgs e)
        {
            if (lUseBuffs.SelectedItem != null)
                WineBot.WineBot.Instance.BotBuffs.Remove((Skills)lUseBuffs.SelectedItem);

            lUseBuffs.Items.Clear();
            lUseBuffs.Items.AddRange(WineBot.WineBot.Instance.BotBuffs.ToArray());
        }

        private void BuffTimer_Tick(object sender, EventArgs e)
        {
            WineBot.WineBot.Instance.UseBuffs();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            GameFunctionsAndObjects.DataFetch.GetItemsAroundPlayerV2();
          /*  string path = MemoryTools.MemoryTools.GetModulePath("clrbootstrap").Substring(0,MemoryTools.MemoryTools.GetModulePath("clrbootstrap").LastIndexOf("\\"));
            Process.Start(new ProcessStartInfo(path+ "WebMenu\\Release\\net7.0\\Web Menu.exe", textBox4.Text));*/
        }


        private void lNPClist_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(((long)((IObject)lNPClist.SelectedItem).ObjectPointer).ToString("X"));
        }

        private void timerParty_Tick(object sender, EventArgs e)
        {
            lPartyMembers.Items.Clear();
            party.Update();
            lPartyMembers.Items.AddRange(party.PartyMemberList.ToArray());
        }

        private void bPartyStart_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!lHealSkills.Items.Cast<Skills>().Any(x => x.skillInfo.ID == ((Skills)lSkillList.SelectedItem).skillInfo.ID))
                lHealSkills.Items.Add(lSkillList.SelectedItem);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ws.SendPlayerInformation();
            ws.SendNPCsInformation();
        }
    }
}
