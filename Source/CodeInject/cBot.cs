using CodeInject.Actors;
using CodeInject.MemoryTools;
using CodeInject.PickupFilters;
using CodeInject.WineBot;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WebSocketSharp.Server;
using static CodeInject.WebSocketServices;



namespace CodeInject
{
    public unsafe partial class cBot : Form
    {
        ItemExecutor mp;
        ItemExecutor hp;
        WebSocketServer server;


        private void SetupWebSocketServer(int port = 8080)
        {
             server = new WebSocketServer($"ws://localhost:{port}");

            server.AddWebSocketService<MyWebSocketService>("/CharacterInfo");
            server.AddWebSocketService<AutoPotionService>("/AutoPotion");
            server.AddWebSocketService<NPCService>("/NpcList");
            server.AddWebSocketService<SkillService>("/SkillList");
            server.AddWebSocketService<PickUpFilterService>("/Filter");

            server.Start();
        }

        public cBot()
        {
            InitializeComponent();
            //  SetupWebSocketServer();
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


            if(!WineBot.WineBot.Instance.BotSkills.Any(x=>x.skillInfo.ID == ((Skills)lSkillList.SelectedItem).skillInfo.ID))
            {
                WineBot.WineBot.Instance.BotSkills.Add((Skills)lSkillList.SelectedItem);
            }

            lUseSkill.Items.Clear();
            lUseSkill.Items.AddRange(WineBot.WineBot.Instance.BotSkills.ToArray());
        }

        private void bSkillRemove_Click(object sender, EventArgs e)
        {
            if (lUseSkill.SelectedItem!=null)
                WineBot.WineBot.Instance.BotSkills.Remove((Skills)lUseSkill.SelectedItem);

            lUseSkill.Items.Clear();
            lUseSkill.Items.AddRange(WineBot.WineBot.Instance.BotSkills.ToArray());
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lNPClist.Items.Clear();

            if (cEnableHuntingArea.Checked)
            {
                WineBot.WineBot.Instance.NpcAround = GameFunctionsAndObjects.DataFetch.GetNPCs()
                    .Where(x => lMonster2Attack.Items.Count == 0 || lMonster2Attack.Items.Cast<MobInfo>().Any(y => ((NPC)x).Info != null && y.ID == ((NPC)x).Info.ID))
                    .Where(x => ((NPC)x).CalcDistance(float.Parse(tXHuntArea.Text), float.Parse(tYHuntArea.Text), float.Parse(tZHuntArea.Text)) < float.Parse(tHuntRadius.Text)).ToList();


                lNPClist.Items.AddRange(WineBot.WineBot.Instance.NpcAround.ToArray());
            }
            else
            {
                WineBot.WineBot.Instance.NpcAround = GameFunctionsAndObjects.DataFetch.GetNPCs()
               .Where(x => lMonster2Attack.Items.Count == 0 || lMonster2Attack.Items.Cast<MobInfo>().Any(y => ((NPC)x).Info != null && y.ID == ((NPC)x).Info.ID)).ToList();

                lNPClist.Items.AddRange(WineBot.WineBot.Instance.NpcAround.ToArray());
            }



            if (cHuntEnable.Checked)
            {
                if (cPickUpEnable.Checked == false || lNearItemsList.Items.Count == 0)
                {
                    WineBot.WineBot.Instance.AttackClosestMonster();
                }
            }

            if (cAutoPotionEnabled.Checked)
            {
                WineBot.WineBot.Instance.AutoPotionFunction(int.Parse(tHPPotionUseProc.Text),int.Parse(tMPPotionUseProc.Text));
            }
        }

        private void pickUpTimer_Tick(object sender, EventArgs e)
        {

            lNearItemsList.Items.Clear();

            lNearItemsList.Items.AddRange(WineBot.WineBot.Instance.UpdateItemsAroundPlayer(int.Parse(tPickupRadius.Text)).ToArray());


            if (cPickUpEnable.Checked)
                WineBot.WineBot.Instance.PickClosestItem();
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
            cbHealHPItem.Items.AddRange(WineBot.WineBot.Instance.UpdateConsumeList().ToArray());
        }

        private void cbHealMPItem_DropDown(object sender, EventArgs e)
        {
            cbHealMPItem.Items.Clear();
            cbHealMPItem.Items.AddRange(WineBot.WineBot.Instance.UpdateConsumeList().ToArray());
        }

        private void bHuntToggle_Click_1(object sender, EventArgs e)
        {
            pickUpTimer.Enabled = !pickUpTimer.Enabled;
            timer2.Enabled = !timer2.Enabled;

            BuffTimer.Enabled = !BuffTimer.Enabled;

            WineBot.WineBot.Instance.UseBuffs();

            bHuntToggle.Text = timer2.Enabled == true ? "STOP" : "START";

        }

        private void cFilterMaterials_CheckedChanged(object sender, EventArgs e)
        {
            if(cFilterMaterials.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).AddToPick(ItemType.Material);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).RemoveFromPick(ItemType.Material);
            }
        }

        private void cFilterArmor_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterArmor.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).AddToPick(ItemType.ChestArmor);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).RemoveFromPick(ItemType.ChestArmor);
            }
        }

        private void cFilterGloves_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterGloves.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).AddToPick(ItemType.Gloves);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).RemoveFromPick(ItemType.Gloves);
            }
        }

        private void cFilterHat_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterHat.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).AddToPick(ItemType.Hat);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).RemoveFromPick(ItemType.Hat);
            }
        }

        private void cFilterShoes_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterShoes.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).AddToPick(ItemType.Shoes);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).RemoveFromPick(ItemType.Shoes);
            }
        }

        private void cFilterUsable_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterUsable.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).AddToPick(ItemType.UsableItem);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).RemoveFromPick(ItemType.UsableItem);
            }
        }

        private void cFilterWeapon_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterWeapon.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).AddToPick(ItemType.Weapon);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).RemoveFromPick(ItemType.Weapon);
            }
        }

        private void cFilterShield_CheckedChanged(object sender, EventArgs e)
        {
            if (cFilterShield.Checked)
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).AddToPick(ItemType.Shield);
            }
            else
            {
                ((QuickFilter)WineBot.WineBot.Instance.filter).RemoveFromPick(ItemType.Shield);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdvancedFilterForm advFilterWindow = new AdvancedFilterForm(WineBot.WineBot.Instance.filter);
            advFilterWindow.ShowDialog();
        }

        private void cAdvanceEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!cAdvanceEnable.Checked)
            {
                WineBot.WineBot.Instance.filter = new QuickFilter();
                SimpleFilterGroup.Controls.OfType<CheckBox>().ToList().ForEach(c => c.Checked = false);
            }
            else
            {
                WineBot.WineBot.Instance.filter = new AdvancedFilter();
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
            if(cbHealHPItem.SelectedIndex!=-1 && cbHealMPItem.SelectedIndex!= -1)
            {
                WineBot.WineBot.Instance.SetAutoHPpotion(int.Parse(tHPPotionUseProc.Text), int.Parse(tHpDurr.Text), (InvItem)cbHealHPItem.SelectedItem);
                WineBot.WineBot.Instance.SetAutoMPpotion( int.Parse(tMPPotionUseProc.Text), int.Parse(tMpDurr.Text), (InvItem)cbHealMPItem.SelectedItem);
            }else
            {
                throw new Exception("Please Select Both item in Autopotion option if you want to have it enabled");
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
            string path = MemoryTools.MemoryTools.GetModulePath("clrbootstrap").Substring(0,MemoryTools.MemoryTools.GetModulePath("clrbootstrap").LastIndexOf("\\"));
            Process.Start(new ProcessStartInfo(path+ "WebMenu\\Release\\net7.0\\Web Menu.exe", textBox4.Text));
        }

    }
}
