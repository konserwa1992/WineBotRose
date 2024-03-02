using CodeInject.Actors;
using CodeInject.MemoryTools;
using CodeInject.Modules;
using CodeInject.UIPanels.Module_Panels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeInject.UIPanels
{
    public partial class BackToCenterPanel : UserControl, IModuleUI
    {
        public string DisplayName { get; set; } = "Back to center";
        private ListBox MonsterListRef;
        public BackToCenterPanel(ListBox monsterListRef)
        {
            InitializeComponent();
            MonsterListRef = monsterListRef;
        }

        public override string ToString()
        {
            return "Back to center";
        }

        private void BackToCenterPanel_Load(object sender, EventArgs e)
        {

        }

        public IModule GetModule()
        {
            return new BackToCenterModule(MonsterListRef.Items.Cast<MobInfo>().ToList(), new System.Numerics.Vector3(float.Parse(textBox1.Text), float.Parse(textBox2.Text), 0), int.Parse(textBox3.Text));
        }

        private unsafe void bGetPosition_Click(object sender, EventArgs e)
        {
            Player player = GameHackFunc.ClientData.GetPlayer();
            textBox1.Text = (player.X).ToString();
            textBox2.Text = (player.Y).ToString();
           
        }
    }
}
