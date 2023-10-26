using CodeInject.Actors;
using CodeInject.BotStates;
using CodeInject.Hunt;
using CodeInject.MemoryTools;
using CodeInject.Modules;
using CodeInject.Party;
using CodeInject.PickupFilters;
using System;
using System.Collections.Generic;
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
   
        Party.Party party;
        public static BotContext BotContext = new BotContext();
        bool BotState = false;

        public List<InvItem> ConsumeItems = new List<InvItem>();

        public cBot()
        {
            InitializeComponent();

            BotContext.Start(
                    new HuntState(
                        new HealerHunt(lMonster2Attack.Items.Cast<MobInfo>().ToList(),
                        new Vector3(float.Parse(tXHuntArea.Text), float.Parse(tYHuntArea.Text),
                        float.Parse(tZHuntArea.Text)), int.Parse(tHuntRadius.Text), lUseSkill.Items.OfType<Skills>().ToList(),lPlayers2Heal.Items.OfType<IObject>().ToList(), int
                        .Parse(tHealWhenProc.Text), this)
                       ));
           BotContext.Stop();
        }

        private void bSkillRefresh_Click(object sender, EventArgs e)
        {
            lSkillList.Items.Clear();
            lSkillList.Items.AddRange(PlayerCharacter.GetPlayerSkills.ToArray());
        }

        private void bSkillAdd_Click(object sender, EventArgs e)
        {
            if(lSkillList.SelectedIndex!=-1)
               BotContext.GetState<HuntState>("HUNT").HuntInstance.AddSkill((Skills)lSkillList.SelectedItem,SkillTypes.AttackSkill);
        }

        private void bSkillRemove_Click(object sender, EventArgs e)
        {
            if (lUseSkill.SelectedItem!=null)
                BotContext.GetState<HuntState>("HUNT").HuntInstance.RemoveSkill((Skills)lUseSkill.SelectedItem);
        }


        public void SkillListUpdate()
        {
            lUseSkill.Items.Clear();
            lHealSkills.Items.Clear();
            lUseSkill.Items.AddRange(BotContext.GetState<HuntState>("HUNT").HuntInstance.BotSkills.Where(x=>x.SkillType == SkillTypes.AttackSkill).ToArray());
            lHealSkills.Items.AddRange(BotContext.GetState<HuntState>("HUNT").HuntInstance.BotSkills.Where(x => x.SkillType == SkillTypes.HealTarget).ToArray());
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            BotContext.Update();

            listBox1.Items.Clear();
            listBox1.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetNPCs().ToArray());
        }

        private void lNearItemsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((Item)lNearItemsList.SelectedItem).Pickup();
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
            cbHealHPItem.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetConsumableItemsFromInventory(cbHealHPItem.Items.OfType<InvItem>().ToList()).ToArray());
        }

        private void cbHealMPItem_DropDown(object sender, EventArgs e)
        {
            cbHealMPItem.Items.Clear();
            cbHealMPItem.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetConsumableItemsFromInventory(cbHealMPItem.Items.OfType<InvItem>().ToList()).ToArray());
        }

        private void bHuntToggle_Click_1(object sender, EventArgs e)
        {
            if (BotState == false)
            {
                GameFunctionsAndObjects.Actions.Logger($"Bot is running: {timer2.Enabled}", Color.Orange);


                if (cEnableHealParty.Checked)
                {
                    List<Skills> SkillList = new List<Skills>();
                    SkillList.AddRange(lUseSkill.Items.Cast<Skills>().ToArray());
                    SkillList.AddRange(lHealSkills.Items.Cast<Skills>().ToArray());

                    BotContext.Start(new HuntState(
                        new HealerHunt(lMonster2Attack.Items.Cast<MobInfo>().ToList(),
                        new Vector3(float.Parse(tXHuntArea.Text), float.Parse(tYHuntArea.Text),
                        float.Parse(tZHuntArea.Text)), int.Parse(tHuntRadius.Text), SkillList, lPlayers2Heal.Items.OfType<IObject>().ToList(), int
                        .Parse(tHealWhenProc.Text), this)
                       ));
                }
                else
                {
                    BotContext.Start(
                        new HuntState(
                            new DefaultHunt(lMonster2Attack.Items.Cast<MobInfo>().ToList(),
                            new Vector3(float.Parse(tXHuntArea.Text), float.Parse(tYHuntArea.Text),
                            float.Parse(tZHuntArea.Text)), int.Parse(tHuntRadius.Text), lUseSkill.Items.OfType<Skills>().ToList(), this)
                           ));
                }
            }
            else
            {
                BotContext.Stop();
            }
            BotState = !BotState;
            bHuntToggle.Text = BotState ? "Stop" : "Start";
        }

        private void cFilterMaterials_CheckedChanged(object sender, EventArgs e)
        {
            if(cFilterMaterials.Checked)
            {
                ((QuickFilter)BotContext.Filter).AddToPick(ItemType.Material);
            }
            else
            {
                ((QuickFilter)BotContext.Filter).RemoveFromPick(ItemType.Material);
            }
        }

        private void cFilterArmor_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterArmor.Checked)
            {
                ((QuickFilter)BotContext.Filter).AddToPick(ItemType.ChestArmor);
            }
            else
            {
                ((QuickFilter)BotContext.Filter).RemoveFromPick(ItemType.ChestArmor);
            }
        }

        private void cFilterGloves_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterGloves.Checked)
            {
                ((QuickFilter)BotContext.Filter).AddToPick(ItemType.Gloves);
            }
            else
            {
                ((QuickFilter)BotContext.Filter).RemoveFromPick(ItemType.Gloves);
            }
        }

        private void cFilterHat_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterHat.Checked)
            {
                ((QuickFilter)BotContext.Filter).AddToPick(ItemType.Hat);
            }
            else
            {
                ((QuickFilter)BotContext.Filter).RemoveFromPick(ItemType.Hat);
            }
        }

        private void cFilterShoes_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterShoes.Checked)
            {
                ((QuickFilter)BotContext.Filter).AddToPick(ItemType.Shoes);
            }
            else
            {
                ((QuickFilter)BotContext.Filter).RemoveFromPick(ItemType.Shoes);
            }
        }

        private void cFilterUsable_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterUsable.Checked)
            {
                ((QuickFilter)BotContext.Filter).AddToPick(ItemType.UsableItem);
            }
            else
            {
                ((QuickFilter)BotContext.Filter).RemoveFromPick(ItemType.UsableItem);
            }
        }

        private void cFilterWeapon_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterWeapon.Checked)
            {
                ((QuickFilter)BotContext.Filter).AddToPick(ItemType.Weapon);
            }
            else
            {
                ((QuickFilter)BotContext.Filter).RemoveFromPick(ItemType.Weapon);
            }
        }

        private void cFilterShield_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterShield.Checked)
            {
                ((QuickFilter)BotContext.Filter).AddToPick(ItemType.Shield);
            }
            else
            {
                ((QuickFilter)BotContext.Filter).RemoveFromPick(ItemType.Shield);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdvancedFilterForm advFilterWindow = new AdvancedFilterForm(BotContext.Filter);
            advFilterWindow.ShowDialog();
        }

        private void cAdvanceEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!cAdvanceEnable.Checked)
            {
                BotContext.Filter = new QuickFilter();
                SimpleFilterGroup.Controls.OfType<CheckBox>().ToList().ForEach(c => c.Checked = false);
            }
            else
            {
                BotContext.Filter = new AdvancedFilter();
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
                BotContext.RemoveModule("AUTOPOTION");
            }
            else
            {
                if (cbHealHPItem.SelectedIndex != -1 && cbHealMPItem.SelectedIndex != -1)
                {
                    AutoPotionModule autoPotion = BotContext.GetModule<AutoPotionModule>("AUTOPOTION");
                    if (autoPotion == null)
                        autoPotion = (AutoPotionModule)BotContext.AddModule(new AutoPotionModule());

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


        private void BuffTimer_Tick(object sender, EventArgs e)
        {
      
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
            if (lSkillList.SelectedIndex != -1)
                BotContext.GetState<HuntState>("HUNT").HuntInstance.AddSkill((Skills)lSkillList.SelectedItem, SkillTypes.HealTarget);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            BotContext.AddModule(new WebMenuModule());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(((long)((IObject)listBox1.SelectedItem).ObjectPointer).ToString("X"));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lPlayersList.Items.Clear();
            lPlayersList.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetNPCs().Where(x=>x.GetType() == typeof(Player) || x.GetType() == typeof(OtherPlayer)).ToArray());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            lPlayers2Heal.Items.Add(lPlayersList.SelectedItem);
        }
    }
}
