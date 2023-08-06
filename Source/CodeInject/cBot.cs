using CodeInject.Actors;
using CodeInject.MemoryTools;
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

        public cBot()
        {
            InitializeComponent();
            lFullMonsterList.Items.AddRange(DataBase.GameDataBase.MonsterDatabase.Where(x => x.Name != "").ToArray());
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
        }

        private void bSkillRemove_Click(object sender, EventArgs e)
        {
            if (lUseSkill.SelectedItem!=null)
                lUseSkill.Items.Remove(lUseSkill.SelectedItem);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lNPClist.Items.Clear();

            if (cEnableHuntingArea.Checked)
            {
                lNPClist.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetNPCs()
                    .Where(x => lMonster2Attack.Items.Count==0 || lMonster2Attack.Items.Cast<MobInfo>().Any(y => ((NPC)x).Info != null && y.ID == ((NPC)x).Info.ID))
                    .Where(x => ((NPC)x).CalcDistance(float.Parse(tXHuntArea.Text), float.Parse(tYHuntArea.Text), float.Parse(tZHuntArea.Text)) < float.Parse(tHuntRadius.Text))
                    .ToArray());

            }
            else
            {
                lNPClist.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetNPCs()
               .Where(x => lMonster2Attack.Items.Count == 0 ||  lMonster2Attack.Items.Cast<MobInfo>().Any(y => ((NPC)x).Info != null && y.ID == ((NPC)x).Info.ID))
               .ToArray());
            }



            if (lNearItemsList.Items.Count == 0)
            {
                if (lUseSkill.SelectedIndex < lUseSkill.Items.Count-1)
                {
                    lUseSkill.SelectedIndex++;
                }
                else
                {
                    lUseSkill.SelectedIndex = 0;
                }

                if (lNPClist.Items.Count > 0)
                {
                    lNPClist.SelectedItem = lNPClist.Items[0];
                    GameFunctionsAndObjects.Actions.Attack(PlayerCharacter.GetPlayerSkills.IndexOf(PlayerCharacter.GetPlayerSkills.FirstOrDefault(x => x.skillInfo.ID == ((Skills)lUseSkill.SelectedItem).skillInfo.ID)), *((NPC)lNPClist.SelectedItem).ID);
   
                }
            }

            if (cAutoPotionEnabled.Checked)
            {
                AutoPotionFunction();
            }


        }

        private void AutoPotionFunction()
        {
            int procHP = int.Parse(tHPPotionUseProc.Text);

            if((((float)*(GameFunctionsAndObjects.DataFetch.GetPlayer()).Hp / *(GameFunctionsAndObjects.DataFetch.GetPlayer()).MaxHp)*100)<procHP)
            {
                hp.Use((InvItem)cbHealHPItem.SelectedItem);
            }

            if ((((float)*(GameFunctionsAndObjects.DataFetch.GetPlayer()).Mp / *(GameFunctionsAndObjects.DataFetch.GetPlayer()).MaxMp) * 100) < procHP)
            {
                mp.Use((InvItem)cbHealMPItem.SelectedItem);
            }
        }


        private void bHuntToggle_Click(object sender, EventArgs e)
        {
            pickUpTimer.Enabled = !pickUpTimer.Enabled;
            timer2.Enabled = !timer2.Enabled;

            bHuntToggle.Text = timer2.Enabled == true? "STOP":"START";


            hp = new Potion(int.Parse(tHpDurr.Text));
            mp = new Potion(int.Parse(tMpDurr.Text));
        }


        private void pickUpTimer_Tick(object sender, EventArgs e)
        {

            lNearItemsList.Items.Clear();
            float maxDistance;
            if(!float.TryParse(tPickupRadius.Text,out maxDistance))
            {
                maxDistance = 100f;
            }

            IObject player = PlayerCharacter.PlayerInfo;

            if (!cPickupnOnlyHuntArea.Enabled)
            {
                lNearItemsList.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetItemsAroundPlayer().Where(x => x.CalcDistance(player) < int.Parse(tHuntRadius.Text)).OrderBy(x => x.CalcDistance(player)).ToArray());
            }
            else
            {
                lNearItemsList.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetItemsAroundPlayer().Where(x => x.CalcDistance(float.Parse(tXHuntArea.Text), float.Parse(tYHuntArea.Text), float.Parse(tZHuntArea.Text)) < float.Parse(tHuntRadius.Text)).OrderBy(x => x.CalcDistance(player)).ToArray());
            }

            if (lNearItemsList.Items.Count > 0)
                ((Item)lNearItemsList.Items[0]).Pickup();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            lNearItemsList.Items.Clear();
            float maxDistance;
            if (!float.TryParse(tPickupRadius.Text, out maxDistance))
            {
                maxDistance = 100f;
            }
            IObject player = PlayerCharacter.PlayerInfo;
            lNearItemsList.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetItemsAroundPlayer().Where(x => x.CalcDistance(player) < maxDistance).OrderBy(x => x.CalcDistance(player)).ToArray());

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
                lMonster2Attack.Items.Remove(lFullMonsterList.SelectedItem);
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
            List<IntPtr> items = GameFunctionsAndObjects.DataFetch.getInventoryItems();

            foreach (IntPtr item in items)
            {
                if (item.ToInt64() != 0x0)
                {
                    InvItem inv = new InvItem((long*)GameFunctionsAndObjects.DataFetch.GetInventoryItemDetails((item.ToInt64())), (long*)item.ToInt64());


                    if (*inv.ItemType == 0xA)
                    {
                        cbHealHPItem.Items.Add(inv);
                    }
                }
            }
        }

        private void cbHealMPItem_DropDown(object sender, EventArgs e)
        {
            cbHealMPItem.Items.Clear();
            List<IntPtr> items = GameFunctionsAndObjects.DataFetch.getInventoryItems();

            foreach (IntPtr item in items)
            {
                if (item.ToInt64() != 0x0)
                {
                    InvItem inv = new InvItem((long*)GameFunctionsAndObjects.DataFetch.GetInventoryItemDetails((item.ToInt64())), (long*)item.ToInt64());


                    if (*inv.ItemType == 0xA)
                    {
                        cbHealMPItem.Items.Add(inv);
                    }
                }
            }
        }

    }
}
