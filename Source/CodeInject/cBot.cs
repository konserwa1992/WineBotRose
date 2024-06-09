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
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Security;

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
                         .Parse(tHealWhenProc.Text), cNormalAttackEnable.Checked, this)
                        ));
            BotContext.Stop();
        }



        private void bSkillRefresh_Click(object sender, EventArgs e)
        {
            lSkillList.Items.Clear();
            lSkillList.Items.AddRange(PlayerCharacter.GetPlayerSkills.Where(x=>x.skillInfo.Type!="Passive").ToArray());

            comboBox5.Items.Clear();
            comboBox5.Items.AddRange(PlayerCharacter.GetPlayerSkills.ToArray());
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
            listBox1.Items.Clear();
            lNearItemsList.Items.Clear();
            listBox1.Items.AddRange(GameHackFunc.Game.ClientData.GetNPCs().ToArray());
            lNearItemsList.Items.AddRange(GameHackFunc.Game.ClientData.GetItemsAroundPlayerV2().ToArray());
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

            tXHuntArea.Text = (player.X).ToString();
            tYHuntArea.Text = (player.Y).ToString();
            tZHuntArea.Text = (player.Z).ToString();
        }


        private void cbHealHPItem_DropDown(object sender, EventArgs e)
        {
            cbHealHPItem.Items.Clear();
            cbHealHPItem.Items.AddRange(GameHackFunc.Game.ClientData.GetConsumableItemsFromInventory(cbHealHPItem.Items.OfType<InvItem>().ToList()).ToArray());
        }

        private void cbHealMPItem_DropDown(object sender, EventArgs e)
        {
            cbHealMPItem.Items.Clear();
            cbHealMPItem.Items.AddRange(GameHackFunc.Game.ClientData.GetConsumableItemsFromInventory(cbHealMPItem.Items.OfType<InvItem>().ToList()).ToArray());
        }


        private Skills GetSkillFromFile(string line)
        {
            string[] data = line.Split(';');

            return new Skills(DataBase.GameDataBase.SkillDatabase.FirstOrDefault(x => x.ID == int.Parse(data[1])), (SkillTypes)int.Parse(data[0]));
        }

        private void LoadConfig(string name)
        {
            StreamReader config = new StreamReader(name + ".skills.txt");

            lUseSkill.Items.Clear();
            lBuffs.Items.Clear();
            lHealSkills.Items.Clear();

            while (!config.EndOfStream)
            {
                Skills skill = GetSkillFromFile(config.ReadLine());

                switch (skill.SkillType)
                {
                    case SkillTypes.AttackSkill:
                        BotContext.GetState<HuntState>("HUNT").HuntInstance.AddSkill(skill, SkillTypes.AttackSkill);
                        lUseSkill.Items.Add(skill);
                        break;
                    case SkillTypes.Buff:
                        BotContext.GetState<HuntState>("HUNT").HuntInstance.AddSkill(skill, SkillTypes.Buff);
                        lBuffs.Items.Add(skill);
                        break;
                    case SkillTypes.HealTarget:
                        BotContext.GetState<HuntState>("HUNT").HuntInstance.AddSkill(skill, SkillTypes.HealTarget);
                        lHealSkills.Items.Add(skill);
                        cEnableHealParty.Checked = true;
                        break;

                }
            }
            config.Close();

            config = new StreamReader(name + ".mobs.txt");
            lMonster2Attack.Items.Clear();
            while (!config.EndOfStream)
            {
                int id = int.Parse(config.ReadLine());
                lMonster2Attack.Items.Add(DataBase.GameDataBase.MonsterDatabase.FirstOrDefault(x => x.ID == id));
            }
            config.Close();



            if (File.Exists(name + ".AutoPotion.txt"))
            {
                cbHealHPItem.Items.Clear();
                cbHealMPItem.Items.Clear();
                config = new StreamReader(name + ".AutoPotion.txt");
                string[] data = config.ReadLine().Split(';');

                if (data.Length < 3)
                {
                    config.Close();
                    return;
                }


                int itemId = int.Parse(data[2]);
                InvItem ihp = GameHackFunc.Game.ClientData.GetConsumableItemsFromInventory(new List<InvItem>()).FirstOrDefault(x => *x.ItemData == itemId && *x.ItemType == 0xA);

                if (ihp == null)
                {
                    MessageBox.Show($"There is no longer Hp potion avaiable in inventory: {DataBase.GameDataBase.UsableItemsDatabase.FirstOrDefault(x => x.ID == itemId).DisplayName}");
                    config.Close();
                    return;
                }
                cbHealHPItem.SelectedIndex = cbHealHPItem.Items.Add(ihp);
                tHPPotionUseProc.Text = data[0];
                tHpDurr.Text = data[1];
                data = config.ReadLine().Split(';');

                itemId = int.Parse(data[2]);
                InvItem imp = GameHackFunc.Game.ClientData.GetConsumableItemsFromInventory(new List<InvItem>()).FirstOrDefault(x => *x.ItemData == itemId && *x.ItemType == 0xA);

                if (ihp == null)
                {
                    MessageBox.Show($"There is no longer Mp potion avaiable in inventory: {DataBase.GameDataBase.UsableItemsDatabase.FirstOrDefault(x => x.ID == itemId).DisplayName}");
                    config.Close();
                    return;
                }
                else
                {
                    cbHealMPItem.SelectedIndex = cbHealMPItem.Items.Add(imp);
                    tMPPotionUseProc.Text = data[0];
                    tMpDurr.Text = data[1];
                }
                cAutoPotionEnabled.Checked = true;
                config.Close();
            }
        }

        private void SaveConfig(string configName)
        {
            StreamWriter wConfig = new StreamWriter(configName + ".skills.txt", false);

            foreach (Skills skill in lUseSkill.Items)
            {
                wConfig.WriteLine((int)skill.SkillType + ";" + skill.skillInfo.ID);
            }
            foreach (Skills skill in lHealSkills.Items)
            {
                wConfig.WriteLine((int)skill.SkillType + ";" + skill.skillInfo.ID);
            }
            foreach (Skills skill in lBuffs.Items)
            {
                wConfig.WriteLine((int)skill.SkillType + ";" + skill.skillInfo.ID);
            }
            wConfig.Close();

            wConfig = new StreamWriter(configName + ".mobs.txt", false);

            foreach (MobInfo mob in lMonster2Attack.Items)
            {
                wConfig.WriteLine(mob.ID);
            }
            wConfig.Close();


            if (cbHealHPItem.SelectedItem != null)
            {
                wConfig = new StreamWriter(configName + ".AutoPotion.txt", false);

                wConfig.WriteLine($"{tHPPotionUseProc.Text};{tHpDurr.Text};{*((InvItem)cbHealHPItem.SelectedItem).ItemData}");
                wConfig.WriteLine($"{tMPPotionUseProc.Text};{tMpDurr.Text};{*((InvItem)cbHealMPItem.SelectedItem).ItemData}");
            }

            wConfig.Close();
        }

        private void bHuntToggle_Click_1(object sender, EventArgs e)
        {
            if (BotState == false)
            {

                List<Skills> SkillList = new List<Skills>();
                SkillList.AddRange(lUseSkill.Items.Cast<Skills>().ToArray());
                SkillList.AddRange(lHealSkills.Items.Cast<Skills>().ToArray());
                if (comboBox5.SelectedIndex != -1)
                {
                    SkillList.Add(comboBox5.SelectedItem as Skills);
                }
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

                    if (comboBox2.SelectedIndex != -1)
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
            MessageBox.Show(((long)GameHackFunc.Game.ClientData.GetPlayer().ObjectPointer).ToString("X"));
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

            this.Text = $"WineBot Character: {GameHackFunc.Game.ClientData.GetPlayer().Name}";

            comboBox2.Items.Add(new BackToCenterPanel(lMonster2Attack));
            comboBox2.Items.Add(new GoToPlayerPanel(lMonster2Attack, tXHuntArea, tYHuntArea, tHuntRadius));


            string[] configFiles = Directory.GetDirectories(DataBase.DataPath + "Profiles");

            foreach (var file in configFiles)
            {
                string filename = file.Substring(file.LastIndexOf('\\') + 1);
                comboBox3.Items.Add(filename);
            }

            tProfileName.Text = GameHackFunc.Game.ClientData.GetPlayer().Name;

            if (comboBox3.Items.Count != 0)
            {
                comboBox3.SelectedItem = comboBox3.Items.OfType<string>().FirstOrDefault(x => (string)x == GameHackFunc.Game.ClientData.GetPlayer().Name);
            }
        }


        private void PlayerInfo()
        {
            IPlayer player = GameHackFunc.Game.ClientData.GetPlayer();

            pbHpBar.Minimum = 0;
            pbHpBar.Maximum = player.MaxHp;
            pbHpBar.Value = player.Hp;
        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            if (lSkillList.SelectedIndex != -1)
                BotContext.GetState<HuntState>("HUNT").HuntInstance.AddSkill((Skills)lSkillList.SelectedItem, SkillTypes.HealTarget);
        }



        private void button5_Click(object sender, EventArgs e)
        {
            lPlayersList.Items.Clear();
            lPlayersList.Items.AddRange(GameHackFunc.Game.ClientData.GetNPCs().Where(x => x.GetType() == typeof(Player) || x.GetType() == typeof(OtherPlayer)).ToArray());
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
            {
                // lHealSkills.Items.Remove(lHealSkills.SelectedItem);
                BotContext.GetState<HuntState>("HUNT").HuntInstance.RemoveSkill((Skills)lHealSkills.SelectedItem);
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(GameHackFunc.Game.ClientData.GetNPCs().Where(x => x.GetType() == typeof(Player) || x.GetType() == typeof(OtherPlayer)).ToArray());
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

            List<ushort> buffs = GameHackFunc.Game.ClientData.GetPlayer().GetBuffsIDs();
            foreach (int i in buffs)
            {

                GameHackFunc.Game.Actions.Logger(i.ToString());
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

            map = new Map("Bez tytułu1.png", "Bez tytułu.png", new double[,] { { 5417, 5373 }, { 5651, 5368 }, { 5742, 5095 } }, new double[,] { { 162, 93 }, { 241, 97 }, { 272, 176 } });


            listBox4.Items.Clear();

            GameHackFunc.Game.Actions.MoveToPoint(map.CalculatePositionFromMap2World(me.X, me.Y));


            Point pos = map.PlayerPositionOnMap();

            List<Point> p = map.FindShortestPathOnMap(pos, new Point(me.X, me.Y));


            Bitmap bitmap = new Bitmap(map.RoadMap);

            pictureBox2.Image = new Bitmap(map.OryginalMap);
            pictureBox3.Image = bitmap;
            using (Graphics g = Graphics.FromImage(pictureBox2.Image))
            {
                g.DrawEllipse(new Pen(Color.Blue, 4), pos.X, pos.Y, 4, 4);
                int sizeC = 4;
                for (int x = 0; x < pictureBox3.Width - sizeC; x += sizeC)
                {
                    for (int y = 0; y < pictureBox3.Height - sizeC; y += sizeC)
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

            if (listBox4.SelectedIndex == listBox4.Items.Count - 1)
            {
                timer3.Enabled = false;
                return;
            }




            Point p = (Point)listBox4.SelectedItem;
            Vector2 playerPos = new Vector2(GameHackFunc.Game.ClientData.GetPlayer().X, GameHackFunc.Game.ClientData.GetPlayer().Y);
            Vector2 destination = map.CalculatePositionFromMap2World(p.X, p.Y);

            if (Vector2.Distance(playerPos / 100, destination) < 3)
            {
                listBox4.SelectedIndex = listBox4.SelectedIndex + 1;
            }
            GameHackFunc.Game.Actions.MoveToPoint(destination);
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
            MessageBox.Show(((long)GameHackFunc.Game.ClientData.GetNPCs().FirstOrDefault().ObjectPointer).ToString("X"));
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(DataBase.DataPath + tProfileName.Text))
            {
                SaveConfig(DataBase.DataPath + "\\Profiles\\" + tProfileName.Text);
            }
            else
            {
                Directory.CreateDirectory(DataBase.DataPath + "\\Profiles\\" + tProfileName.Text);
                SaveConfig(DataBase.DataPath + "\\Profiles\\" + tProfileName.Text + "\\" + tProfileName.Text);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            LoadConfig(DataBase.DataPath + "\\Profiles\\" + (string)comboBox3.SelectedItem + "\\" + (string)comboBox3.SelectedItem);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/paypalme/winebot");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            byte[] packet = new byte[]
            {
               0x0e ,0x00 ,0xa3 ,0x07 ,0xD1 ,0x58 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00
            };

            //List<InvItem> items = new List<InvItem>();
            InvItem item = comboBox4.SelectedItem as InvItem;

            long itemServPacketId = *(long*)((long)item.ObjectPointer + 0x48);
            //  * (long*)((long)ObjectPointer + 0x48)
            byte[] servIdArray = BitConverter.GetBytes(itemServPacketId);

            Array.Copy(servIdArray, 0, packet, 6, servIdArray.Length);

            fixed (byte* packetPointer = packet)
            {
                GameHackFunc.Game.Actions.SendPacket(packetPointer);
            }
        }

        private void comboBox4_DropDown(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            comboBox4.Items.AddRange(GameHackFunc.Game.ClientData.GetConsumableItemsFromInventory(comboBox4.Items.OfType<InvItem>().ToList()).ToArray());
        }

        private void button22_Click(object sender, EventArgs e)
        {
            listBox5.Items.Clear();
            listBox5.Items.AddRange(GameHackFunc.Game.ClientData.GetAllItemsFromInventory(listBox5.Items.OfType<InvItem>().ToList()).ToArray());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lPlayers2Heal.Items.Remove(lPlayers2Heal.SelectedItem);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            (comboBox5.SelectedItem as Skills).SkillType = SkillTypes.Revive;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void SimpleFilterGroup_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter_1(object sender, EventArgs e)
        {

        }

        private void tHealWhenProc_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Controleer of de ingedrukte toets een cijfer is of een besturingsteken zoals backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Als de toets geen cijfer of besturingsteken is, annuleren we de invoer
                e.Handled = true;
            }
        }

        private void tHealWhenProc_TextChanged(object sender, EventArgs e)
        {
            // Probeer de waarde van de TextBox te parsen
            if (int.TryParse(tHealWhenProc.Text, out int value))
            {
                // Controleer of de waarde tussen 0 en 100 ligt
                if (value < 0 || value > 100)
                {
                    MessageBox.Show("Please enter a number between 0 and 100.");
                    tHealWhenProc.Text = string.Empty;
                }
            }
            else
            {
                // Als de waarde niet kan worden geparsed, leeg de TextBox
                if (!string.IsNullOrEmpty(tHealWhenProc.Text))
                {
                    MessageBox.Show("Please enter a valid number.");
                    tHealWhenProc.Text = string.Empty;
                }
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            healskills.Items.Clear();
            healskills.Items.AddRange(PlayerCharacter.GetPlayerSkills.Where(x=>x.skillInfo.Type!="Passive").ToArray());

            comboBox5.Items.Clear();
            comboBox5.Items.AddRange(PlayerCharacter.GetPlayerSkills.ToArray());
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            lPlayersList.Items.Clear();
            lPlayersList.Items.AddRange(GameHackFunc.Game.ClientData.GetNPCs().Where(x => x.GetType() == typeof(Player) || x.GetType() == typeof(OtherPlayer)).ToArray());
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            lHealSkills.Items.Add(healskills.SelectedItem);
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            

            lPlayers2Heal.Items.Add(lPlayersList.SelectedItem);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            

            lPlayers2Heal.Items.Remove(lPlayers2Heal.SelectedItem);
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            lHealSkills.Items.Remove(lHealSkills.SelectedItem);
        }

        private void healskills_MouseDown(object sender, MouseEventArgs e)
        {
            button23.Enabled = true;
        }

        private void healskills_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Controleer of er een item is geselecteerd
            if (healskills.SelectedIndex != -1)
            {
                button1.Enabled = true;  // Zet de knop aan
            }
            else
            {
                button1.Enabled = false; // Zet de knop uit
            }
        }

        private void lHealSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Controleer of er een item is geselecteerd
            if (lHealSkills.SelectedIndex != -1)
            {
                button3.Enabled = true;  // Zet de knop aan
            }
            else
            {
                button3.Enabled = false; // Zet de knop uit
            }
        }

        private void lPlayersList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Controleer of er een item is geselecteerd
            if (lPlayersList.SelectedIndex != -1)
            {
                button7.Enabled = true;  // Zet de knop aan
            }
            else
            {
                button7.Enabled = false; // Zet de knop uit
            }
        }

        private void lPlayers2Heal_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Controleer of er een item is geselecteerd
            if (lPlayers2Heal.SelectedIndex != -1)
            {
                button6.Enabled = true;  // Zet de knop aan
            }
            else
            {
                button6.Enabled = false; // Zet de knop uit
            }
        }

        private void button20_Click_1(object sender, EventArgs e)
        {
            lPlayers2Heal.Items.Clear();
        }

        private void healskills_MouseHover(object sender, EventArgs e)
        {
            Statustext.Text = "Available Skills";
        }

        private void lPlayersList_MouseHover(object sender, EventArgs e)
        {
            Statustext.Text = "Available Players";
        }

        private void lPlayers2Heal_MouseHover(object sender, EventArgs e)
        {
            Statustext.Text = "Players you sellected to Heal";
        }

        private void lHealSkills_MouseHover(object sender, EventArgs e)
        {
            Statustext.Text = "Healing Skills you sellected to use";
        }

        private void cEnableHealParty_MouseHover(object sender, EventArgs e)
        {
            if (cEnableHealParty.Checked != true)
            {
                Statustext.Text = "PartyHeal is Disabled";
            }
            else
            {
                Statustext.Text = "PartyHeal is Enabled";
            }
        }

        private void lBuffs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button24_Click(object sender, EventArgs e)
        {
            lUseSkill.Items.Clear();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            lBuffs.Items.Clear();   
        }
    }
}


