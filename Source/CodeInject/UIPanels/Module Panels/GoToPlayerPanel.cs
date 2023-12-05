using CodeInject.Actors;
using CodeInject.MemoryTools;
using CodeInject.Modules;
using CodeInject.Modules.Mods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeInject.UIPanels.Module_Panels
{
    public partial class GoToPlayerPanel : UserControl,IModuleUI
    {
        public string DisplayName { get; set; } = "Go to player";
        private ListBox MonsterListRef { get; set; }
        private TextBox TXtextboxRef { get; set; }
        private TextBox TYtextboxRef { get; set; }
        private TextBox TRadiustextboxRef { get; set; }


        public GoToPlayerPanel(ListBox monsterListRef, TextBox tX,TextBox tY,TextBox tRadius)
        {
            InitializeComponent();
            MonsterListRef= monsterListRef;
            TXtextboxRef = tX; 
            TYtextboxRef = tY;
            TRadiustextboxRef = tRadius;
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(GameHackFunc.ClientData.GetNPCs().Where(x => x.GetType() == typeof(Player) || x.GetType() == typeof(OtherPlayer)).ToArray());
        }

        private void GoToPlayer_Load(object sender, EventArgs e)
        {

        }

        IModule IModuleUI.GetModule()
        {
            return new GoToFellowModule(MonsterListRef.Items.Cast<MobInfo>().ToList(),
                ((IPlayer)comboBox1.SelectedItem).Name,
                new Vector3(float.Parse(TXtextboxRef.Text), 
                            float.Parse(TYtextboxRef.Text),
                            0),
                float.Parse(TRadiustextboxRef.Text));
        }
    }
}
