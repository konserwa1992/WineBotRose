using CodeInject.Actors;
using CodeInject.BotStates;
using CodeInject.Hunt;
using CodeInject.MemoryTools;
using CodeInject.Modules;
using CodeInject.PickupFilters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using CodeInject.AutoWalk;
using CodeInject.UIPanels;
using CodeInject.UIPanels.Module_Panels;

using Point = AForge.Point;

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
                         float.Parse(tZHuntArea.Text)), int.Parse(tHuntRadius.Text), lUseSkill.Items.OfType<Skills>().ToList(), lPlayers2Heal.Items.OfType<IObject>().ToList(), int
                         .Parse(tHealWhenProc.Text),cNormalAttackEnable.Checked, this)
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
            if (lSkillList.SelectedIndex != -1)
                BotContext.GetState<HuntState>("HUNT").HuntInstance.AddSkill((Skills)lSkillList.SelectedItem, SkillTypes.AttackSkill);
        }

        private void bSkillRemove_Click(object sender, EventArgs e)
        {
            if (lUseSkill.SelectedItem != null)
                BotContext.GetState<HuntState>("HUNT").HuntInstance.RemoveSkill((Skills)lUseSkill.SelectedItem);
        }


        public void SkillListUpdate()
        {
            lUseSkill.Items.Clear();
            lHealSkills.Items.Clear();
            lBuffs.Items.Clear();
            lUseSkill.Items.AddRange(BotContext.GetState<HuntState>("HUNT").HuntInstance.BotSkills.Where(x => x.SkillType == SkillTypes.AttackSkill).ToArray());
            lHealSkills.Items.AddRange(BotContext.GetState<HuntState>("HUNT").HuntInstance.BotSkills.Where(x => x.SkillType == SkillTypes.HealTarget).ToArray());
            lBuffs.Items.AddRange(BotContext.GetState<HuntState>("HUNT").HuntInstance.BotSkills.Where(x => x.SkillType == SkillTypes.Buff).ToArray());
        }

        public void PorionSettingsUpdate()
        {

        }
        bool tdelete = false;
        private void timer2_Tick(object sender, EventArgs e)
        {
            BotContext.Update();
            //  PlayerInfo();
            listBox1.Items.Clear();
             lNearItemsList.Items.Clear();
            listBox1.Items.AddRange(GameHackFunc.ClientData.GetNPCs().ToArray());
            lNearItemsList.Items.AddRange(GameHackFunc.ClientData.GetItemsAroundPlayerV2().ToArray());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            lFullMonsterList.Items.Clear();
            lFullMonsterList.Items.AddRange(DataBase.GameDataBase.MonsterDatabase.Where(x => x.Name != "" && x.Name.ToUpper().Contains(tSearchMobTextBox.Text.ToUpper())).ToArray());
        }

        private void bAddMonster2Attack_Click(object sender, EventArgs e)
        {
            if (lMonster2Attack.Items.Cast<MobInfo>().FirstOrDefault(x => x.ID == ((MobInfo)lFullMonsterList.SelectedItem).ID) == null)
                lMonster2Attack.Items.Add(lFullMonsterList.SelectedItem);
        }

        private void bRemoveMonster2Attack_Click(object sender, EventArgs e)
        {
            if (lMonster2Attack.SelectedIndex != -1)
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
            cbHealHPItem.Items.AddRange(GameHackFunc.ClientData.GetConsumableItemsFromInventory(cbHealHPItem.Items.OfType<InvItem>().ToList()).ToArray());
        }

        private void cbHealMPItem_DropDown(object sender, EventArgs e)
        {
            cbHealMPItem.Items.Clear();
            cbHealMPItem.Items.AddRange(GameHackFunc.ClientData.GetConsumableItemsFromInventory(cbHealMPItem.Items.OfType<InvItem>().ToList()).ToArray());
        }

        private void bHuntToggle_Click_1(object sender, EventArgs e)
        {
            if (BotState == false)
            {
                GameHackFunc.Actions.Logger($"Bot is running: {timer2.Enabled}");

                List<Skills> SkillList = new List<Skills>();
                SkillList.AddRange(lUseSkill.Items.Cast<Skills>().ToArray());
                SkillList.AddRange(lHealSkills.Items.Cast<Skills>().ToArray());
                SkillList.AddRange(lBuffs.Items.Cast<Skills>().ToArray());

                if (cEnableHealParty.Checked)
                {

                    BotContext.Start(new HuntState(
                        new HealerHunt(lMonster2Attack.Items.Cast<MobInfo>().ToList(),
                        new Vector3(float.Parse(tXHuntArea.Text), float.Parse(tYHuntArea.Text),
                        float.Parse(tZHuntArea.Text)), int.Parse(tHuntRadius.Text), SkillList, lPlayers2Heal.Items.OfType<IObject>().ToList(), int
                        .Parse(tHealWhenProc.Text), cNormalAttackEnable.Checked, this)
                       ));
                }
                else
                {
                    DefaultHunt hunt = new DefaultHunt(lMonster2Attack.Items.Cast<MobInfo>().ToList(),
                            new Vector3(float.Parse(tXHuntArea.Text), float.Parse(tYHuntArea.Text),
                            float.Parse(tZHuntArea.Text)), int.Parse(tHuntRadius.Text), SkillList, cNormalAttackEnable.Checked, this);

                    if(comboBox2.SelectedIndex!=-1)
                     hunt.AddModule((comboBox2.SelectedItem as IModuleUI).GetModule());

                    BotContext.Start(
                       new HuntState(hunt
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
            if (cFilterMaterials.Checked)
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
            foreach (MobInfo mob in lFullMonsterList.Items)
            {
                if (!lMonster2Attack.Items.Cast<MobInfo>().Any(x => x.ID == mob.ID))
                    lMonster2Attack.Items.Add(mob);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(((long)GameHackFunc.ClientData.GetPlayer().ObjectPointer).ToString("X"));
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
            GameHackFunc.Actions.Logger($"Hello.");
            GameHackFunc.Actions.Logger($"Bot version {Assembly.GetExecutingAssembly().GetName().Version.ToString()}");


            comboBox2.Items.Add(new BackToCenterPanel(lMonster2Attack));
            comboBox2.Items.Add(new GoToPlayerPanel(lMonster2Attack,tXHuntArea,tYHuntArea, tHuntRadius));
        }


        private void PlayerInfo()
        {
            IPlayer player = GameHackFunc.ClientData.GetPlayer();

            pbHpBar.Minimum = 0;
            pbHpBar.Maximum = *player.MaxHp;
            pbHpBar.Value = *player.Hp;
        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            if (lSkillList.SelectedIndex != -1)
                BotContext.GetState<HuntState>("HUNT").HuntInstance.AddSkill((Skills)lSkillList.SelectedItem, SkillTypes.HealTarget);
        }



        private void button5_Click(object sender, EventArgs e)
        {
            lPlayersList.Items.Clear();
            lPlayersList.Items.AddRange(GameHackFunc.ClientData.GetNPCs().Where(x => x.GetType() == typeof(Player) || x.GetType() == typeof(OtherPlayer)).ToArray());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            lPlayers2Heal.Items.Add(lPlayersList.SelectedItem);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (lSkillList.SelectedIndex != -1)
                BotContext.GetState<HuntState>("HUNT").HuntInstance.AddSkill((Skills)lSkillList.SelectedItem, SkillTypes.Buff);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (lBuffs.SelectedItem != null)
                BotContext.GetState<HuntState>("HUNT").HuntInstance.RemoveSkill((Skills)lBuffs.SelectedItem);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (lHealSkills.SelectedItem != null)
                BotContext.GetState<HuntState>("HUNT").HuntInstance.RemoveSkill((Skills)lHealSkills.SelectedItem);
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(GameHackFunc.ClientData.GetNPCs().Where(x => x.GetType() == typeof(Player) || x.GetType() == typeof(OtherPlayer)).ToArray());
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                BotContext.AddModule(new FollowModule(((IPlayer)comboBox1.SelectedItem).Name));
            }
            else
            {
                BotContext.RemoveModule("FOLLOW");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

            List<ushort> buffs = GameHackFunc.ClientData.GetPlayer().GetBuffsIDs();
            foreach (int i in buffs)
            {

                GameHackFunc.Actions.Logger(i.ToString());
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            Save save = new Save(BotContext, 10);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Load load = new Load(BotContext, 10);
        }



 


        Map map;
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            map = new Map("Bez tytułu1.png", "Bez tytułu.png", new double[,]{ { 5417, 5373 }, { 5651, 5368 }, { 5742, 5095 } },new double[,]{ { 162, 93 }, { 241, 97 }, { 272, 176 } });


            listBox4.Items.Clear();

            GameHackFunc.Actions.MoveToPoint(map.CalculatePositionFromMap2World(me.X, me.Y));


            Point pos = map.PlayerPositionOnMap();

           List<Point> p = map.FindShortestPathOnMap(pos, new Point(me.X, me.Y));


            Bitmap bitmap = new Bitmap(map.RoadMap);

            pictureBox2.Image = new Bitmap(map.OryginalMap);
            pictureBox3.Image = bitmap;
            using (Graphics g = Graphics.FromImage(pictureBox2.Image))
            {
                g.DrawEllipse(new Pen(Color.Blue, 4), pos.X, pos.Y, 4, 4);
                int sizeC = 4;
                for(int x=0;x< pictureBox3.Width- sizeC; x+= sizeC)
                {
                    for (int y = 0; y < pictureBox3.Height- sizeC; y += sizeC)
                    {

                        Color pixelColor = bitmap.GetPixel(x + (sizeC / 2), y + (sizeC / 2));
                        if (pixelColor.R == 255 && pixelColor.G == 255 && pixelColor.B == 255)
                        {
                            g.DrawRectangle(new Pen(Color.Orange, 1), new Rectangle(new System.Drawing.Point(x, y), new Size(sizeC, sizeC)));
                        }
                    }
                }
            }

            pictureBox2.Refresh();
            using (Graphics g = Graphics.FromImage(pictureBox2.Image))
            {
                g.DrawEllipse(new Pen(Color.Blue, 4), pos.X, pos.Y, 4, 4);
                foreach (Point p2 in p)
                {
                    listBox4.Items.Add(p2);
                    g.DrawEllipse(new Pen(Color.Blue, 4), p2.X, p2.Y, 4, 4);
                }
            }
            pictureBox2.Refresh();
            pictureBox3.Refresh();
        }

       

        private void button15_Click(object sender, EventArgs e)
        {
            timer3.Enabled = false;
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }


        private void button16_Click(object sender, EventArgs e)
        {
         
            listBox4.SelectedIndex = 0;
            timer3.Enabled = true;
     
        }


        private void timer3_Tick(object sender, EventArgs e)
        {

                if (listBox4.SelectedIndex == listBox4.Items.Count-1)
                {
                    timer3.Enabled = false;
                    return;
                }


                Point p = (Point)listBox4.SelectedItem;
                Vector2 playerPos = new Vector2(*GameHackFunc.ClientData.GetPlayer().X, *GameHackFunc.ClientData.GetPlayer().Y);
                Vector2 destination = map.CalculatePositionFromMap2World(p.X, p.Y);

                if (Vector2.Distance(playerPos / 100, destination) < 3)
                {
                    listBox4.SelectedIndex = listBox4.SelectedIndex + 1;
              
                }
            GameHackFunc.Actions.MoveToPoint(destination);
        }

        private void lNearItemsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show($"ADDR {((long)((IObject)lNearItemsList.SelectedItem).ObjectPointer).ToString("X")}");
        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {
            
        }

        private void userControl11_Load(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            panel2.Controls.Add((UserControl)comboBox2.SelectedItem);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(((long)((listBox1.SelectedItems as IObject).ObjectPointer)).ToString("X"));
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
             MessageBox.Show((*GameHackFunc.ClientData.GetPlayer().Mp).ToString() +" / "+(*GameHackFunc.ClientData.GetPlayer().MaxMp).ToString());
            MessageBox.Show(((long)GameHackFunc.ClientData.GetPlayer().ObjectPointer).ToString("X"));
        }
    }

}


