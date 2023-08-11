using CodeInject.Actors;
using CodeInject.MemoryTools;
using CodeInject.PickupFilters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;


namespace CodeInject
{
    public unsafe partial class cBot : Form
    {
        Potion mp;
        Potion hp;
        CodeInject.WineBot.WineBot WineBotInstance;


        public cBot()
        {
            InitializeComponent();
            WineBotInstance = new CodeInject.WineBot.WineBot();
        }



        private  void lNPClist_SelectedIndexChanged(object sender, EventArgs e)
        {
            lUseSkill.SelectedItem = lUseSkill.Items[new Random().Next(0, lUseSkill.Items.Count-1)];

            int skillIndex = PlayerCharacter.GetPlayerSkills.FindIndex(x => x.skillInfo.ID == ((Skills)lUseSkill.SelectedItem).skillInfo.ID);

            GameFunctionsAndObjects.Actions.Attack(*((NPC)lNPClist.SelectedItem).ID,
                skillIndex
                );
        }

        private void bSkillRefresh_Click(object sender, EventArgs e)
        {
            lSkillList.Items.Clear();
            lSkillList.Items.AddRange(PlayerCharacter.GetPlayerSkills.ToArray());
        }

        private void bSkillAdd_Click(object sender, EventArgs e)
        {
            if(!lUseSkill.Items.Cast<Skills>().Any(x=>x.skillInfo.ID == ((Skills)lSkillList.SelectedItem).skillInfo.ID))
            lUseSkill.Items.Add(lSkillList.SelectedItem);


            if(!WineBotInstance.BotSkills.Any(x=>x.skillInfo.ID == ((Skills)lSkillList.SelectedItem).skillInfo.ID))
            {
                WineBotInstance.BotSkills.Add((Skills)lSkillList.SelectedItem);
            }

            lUseSkill.Items.Clear();
            lUseSkill.Items.AddRange(WineBotInstance.BotSkills.ToArray());
        }

        private void bSkillRemove_Click(object sender, EventArgs e)
        {
            if (lUseSkill.SelectedItem!=null)
                WineBotInstance.BotSkills.Remove((Skills)lUseSkill.SelectedItem);

            lUseSkill.Items.Clear();
            lUseSkill.Items.AddRange(WineBotInstance.BotSkills.ToArray());
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lNPClist.Items.Clear();

            if (cEnableHuntingArea.Checked)
            {
                WineBotInstance.NpcAround = GameFunctionsAndObjects.DataFetch.GetNPCs()
                    .Where(x => lMonster2Attack.Items.Count == 0 || lMonster2Attack.Items.Cast<MobInfo>().Any(y => ((NPC)x).Info != null && y.ID == ((NPC)x).Info.ID))
                    .Where(x => ((NPC)x).CalcDistance(float.Parse(tXHuntArea.Text), float.Parse(tYHuntArea.Text), float.Parse(tZHuntArea.Text)) < float.Parse(tHuntRadius.Text)).ToList();


                lNPClist.Items.AddRange(WineBotInstance.NpcAround.ToArray());
            }
            else
            {
                WineBotInstance.NpcAround = GameFunctionsAndObjects.DataFetch.GetNPCs()
               .Where(x => lMonster2Attack.Items.Count == 0 || lMonster2Attack.Items.Cast<MobInfo>().Any(y => ((NPC)x).Info != null && y.ID == ((NPC)x).Info.ID)).ToList();

                lNPClist.Items.AddRange(WineBotInstance.NpcAround.ToArray());
            }



            if (cHuntEnable.Checked)
            {
                if (cPickUpEnable.Checked == false || lNearItemsList.Items.Count == 0)
                {
                    WineBotInstance.AttackClosestMonster();
                }
            }

            if (cAutoPotionEnabled.Checked)
            {
                WineBotInstance.AutoPotionFunction(int.Parse(tHPPotionUseProc.Text),int.Parse(tMPPotionUseProc.Text));
            }
        }




        private void pickUpTimer_Tick(object sender, EventArgs e)
        {

            lNearItemsList.Items.Clear();

            lNearItemsList.Items.AddRange(WineBotInstance.UpdateItemsAroundPlayer(int.Parse(tPickupRadius.Text)).ToArray());


            if (cPickUpEnable.Checked)
                WineBotInstance.PickClosestItem();
        }


        private void lNearItemsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            tPickupRadius.Text = ((long)((Item)lNearItemsList.SelectedItem).ObjectPointer).ToString("X");
         //   ((UsableItem)lNearItemsList.SelectedItem).Pickup();
            lNearItemsList.Items.Clear();
            lNearItemsList.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetItemsAroundPlayer().ToArray());
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
            cbHealHPItem.Items.AddRange(WineBotInstance.UpdateConsumeList().ToArray());
        }

        private void cbHealMPItem_DropDown(object sender, EventArgs e)
        {
            cbHealMPItem.Items.Clear();
            cbHealMPItem.Items.AddRange(WineBotInstance.UpdateConsumeList().ToArray());
        }

        private void bHuntToggle_Click_1(object sender, EventArgs e)
        {
            pickUpTimer.Enabled = !pickUpTimer.Enabled;
            timer2.Enabled = !timer2.Enabled;

            bHuntToggle.Text = timer2.Enabled == true ? "STOP" : "START";

        }

        private void cFilterMaterials_CheckedChanged(object sender, EventArgs e)
        {
            if(cFilterMaterials.Checked)
            {
                ((QuickFilter)WineBotInstance.filter).AddToPick(ItemType.Material);
            }
            else
            {
                ((QuickFilter)WineBotInstance.filter).RemoveFromPick(ItemType.Material);
            }
        }

        private void cFilterArmor_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterArmor.Checked)
            {
                ((QuickFilter)WineBotInstance.filter).AddToPick(ItemType.ChestArmor);
            }
            else
            {
                ((QuickFilter)WineBotInstance.filter).RemoveFromPick(ItemType.ChestArmor);
            }
        }

        private void cFilterGloves_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterGloves.Checked)
            {
                ((QuickFilter)WineBotInstance.filter).AddToPick(ItemType.Gloves);
            }
            else
            {
                ((QuickFilter)WineBotInstance.filter).RemoveFromPick(ItemType.Gloves);
            }
        }

        private void cFilterHat_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterHat.Checked)
            {
                ((QuickFilter)WineBotInstance.filter).AddToPick(ItemType.Hat);
            }
            else
            {
                ((QuickFilter)WineBotInstance.filter).RemoveFromPick(ItemType.Hat);
            }
        }

        private void cFilterShoes_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterShoes.Checked)
            {
                ((QuickFilter)WineBotInstance.filter).AddToPick(ItemType.Shoes);
            }
            else
            {
                ((QuickFilter)WineBotInstance.filter).RemoveFromPick(ItemType.Shoes);
            }
        }

        private void cFilterUsable_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterUsable.Checked)
            {
                ((QuickFilter)WineBotInstance.filter).AddToPick(ItemType.UsableItem);
            }
            else
            {
                ((QuickFilter)WineBotInstance.filter).RemoveFromPick(ItemType.UsableItem);
            }
        }

        private void cFilterWeapon_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterWeapon.Checked)
            {
                ((QuickFilter)WineBotInstance.filter).AddToPick(ItemType.Weapon);
            }
            else
            {
                ((QuickFilter)WineBotInstance.filter).RemoveFromPick(ItemType.Weapon);
            }
        }

        private void cFilterShield_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterShield.Checked)
            {
                ((QuickFilter)WineBotInstance.filter).AddToPick(ItemType.Shield);
            }
            else
            {
                ((QuickFilter)WineBotInstance.filter).RemoveFromPick(ItemType.Shield);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdvancedFilterForm advFilterWindow = new AdvancedFilterForm(WineBotInstance.filter);
            advFilterWindow.ShowDialog();
        }

        private void cAdvanceEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!cAdvanceEnable.Checked)
            {
                WineBotInstance.filter = new QuickFilter();
                SimpleFilterGroup.Controls.OfType<CheckBox>().ToList().ForEach(c => c.Checked = false);
            }
            else
            {
                WineBotInstance.filter = new AdvancedFilter();
            }
            SimpleFilterGroup.Enabled = !cAdvanceEnable.Checked;
            bAdvancedFilter.Enabled = cAdvanceEnable.Checked;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            lNearItemsList.Items.Clear();
            lNearItemsList.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetItemsAroundPlayer().ToArray());
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
            if(cbHealHPItem.SelectedIndex!=-1 && cbHealMPItem.SelectedIndex!= -1)
            {
                WineBotInstance.SetAutoHPpotion(int.Parse(tHpDurr.Text), (InvItem)cbHealHPItem.SelectedItem);
                WineBotInstance.SetAutoMPpotion(int.Parse(tMpDurr.Text), (InvItem)cbHealMPItem.SelectedItem);
            }else
            {
                throw new Exception("Please Select Both item in Autopotion option if you want to have it enabled");
            }
        }
    }
}
