using CodeInject.Actors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace CodeInject
{
    public unsafe partial class cBot : Form
    {
        public cBot()
        {
            InitializeComponent();
        }



        private unsafe void timer1_Tick(object sender, EventArgs e)
        {
            lNPClist.Items.Clear();
            lNPClist.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetNPCs().ToArray());
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
            if(!lUseSkill.Items.Contains(lSkillList.SelectedItem))
            lUseSkill.Items.Add(lSkillList.SelectedItem);
        }

        private void bSkillRemove_Click(object sender, EventArgs e)
        {
            if (lUseSkill.SelectedItem!=null)
                lUseSkill.Items.Remove(lUseSkill.SelectedItem);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if(lUseSkill.SelectedIndex == lUseSkill.Items.Count)
            { 
                lUseSkill.SelectedIndex = 0;
            }
            else
            {
                lUseSkill.SelectedIndex++;
            }

            lNPClist.SelectedItem = lNPClist.Items[1];
            GameFunctionsAndObjects.Actions.Attack(PlayerCharacter.GetPlayerSkills.IndexOf(PlayerCharacter.GetPlayerSkills.FirstOrDefault(x => x.skillInfo.ID == ((Skills)lUseSkill.SelectedItem).skillInfo.ID)), *((NPC)lNPClist.SelectedItem).ID);
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
            for (ushort i = 0xFFFF; i >0; i--)
            {
                if (GameFunctionsAndObjects.DataFetch.GetItemPointer(i) != 0)
                {
                    GameFunctionsAndObjects.Actions.PickUp(i);
                    break;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            lNearItemsList.Items.Clear();
            lNearItemsList.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetItemsAroundPlayer().ToArray());
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void lNearItemsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((UsableItem)lNearItemsList.SelectedItem).Pickup();
            lNearItemsList.Items.Clear();
            lNearItemsList.Items.AddRange(GameFunctionsAndObjects.DataFetch.GetItemsAroundPlayer().ToArray());
        }
    }
}
