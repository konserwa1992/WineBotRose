using CodeInject.Actors;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CodeInject
{
    public unsafe partial class cBot : Form
    {
        public cBot()
        {
            InitializeComponent();
            lFullMonsterList.Items.AddRange(DataBase.GameDataBase.MonsterDatabase.Where(x => x.Name != "").ToArray());
        }



        private unsafe void timer1_Tick(object sender, EventArgs e)
        {
            lNPClist.Items.Clear();
            lNPClist.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetNPCs().Where(x=> lMonster2Attack.Items.Cast<MobInfo>().Any(y=> ((NPC)x).Info!=null && y.ID == ((NPC)x).Info.ID)).ToArray());
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
            if(lUseSkill.SelectedIndex > lUseSkill.Items.Count)
            { 
                lUseSkill.SelectedIndex = 0;
            }
            else
            {
                lUseSkill.SelectedIndex++;
            }

            if (lNPClist.Items.Count > 0)
            {
                lNPClist.SelectedItem = lNPClist.Items[0];
                GameFunctionsAndObjects.Actions.Attack(PlayerCharacter.GetPlayerSkills.IndexOf(PlayerCharacter.GetPlayerSkills.FirstOrDefault(x => x.skillInfo.ID == ((Skills)lUseSkill.SelectedItem).skillInfo.ID)), *((NPC)lNPClist.SelectedItem).ID);
            }
        }

        private void bHuntToggle_Click(object sender, EventArgs e)
        {
            pickUpTimer.Enabled = !pickUpTimer.Enabled;
            timer2.Enabled = !timer2.Enabled;

            bHuntToggle.Text = timer2.Enabled == true? "STOP":"START";
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void pickUpTimer_Tick(object sender, EventArgs e)
        {

            lNearItemsList.Items.Clear();
            float maxDistance;
            if(!float.TryParse(tPickupRadius.Text,out maxDistance))
            {
                maxDistance = 100f;
            }
            IObject player = GameFunctionsAndObjects.DataFetch.GetNPCs()[0];
            lNearItemsList.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetItemsAroundPlayer().Where(x => x.CalcDistance(player) < maxDistance).OrderBy(x=>x.CalcDistance(player)).ToArray());

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
            IObject player = GameFunctionsAndObjects.DataFetch.GetNPCs()[0];
            lNearItemsList.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetItemsAroundPlayer().Where(x => x.CalcDistance(player) < maxDistance).OrderBy(x => x.CalcDistance(player)).ToArray());

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

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
    }
}
