using CodeInject.Actors;
using CodeInject.PickupFilters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeInject
{
    public partial class AdvancedFilterForm : Form
    {
        AdvancedFilter Filter { get; set; }
        List<IBasicInfo> FullItemList = new List<IBasicInfo>();

        public AdvancedFilterForm(IFilter filter)
        {
            InitializeComponent();
            this.Filter = (AdvancedFilter)filter;
            listBox2.Items.Clear();
            listBox2.Items.AddRange(Filter.PickWeapon.ToArray());
        }

        private void AdvancedFilterForm_Load(object sender, EventArgs e)
        {
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(FullItemList.Where(x => x.ToString() != "" && x.ToString().ToUpper().Contains(textBox2.Text.ToUpper())).ToArray());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            foreach(var selectedElement in listBox1.SelectedItems)
            {
                if (!Filter.PickWeapon.Any(x => x.ID == (selectedElement as IBasicInfo).ID && selectedElement.GetType() == x.GetType()))
                {
                    Filter.PickWeapon.Add((IBasicInfo)selectedElement);
                }
            }


            listBox2.Items.Clear();
            listBox2.Items.AddRange(Filter.PickWeapon.ToArray());
        }

     
        private void SetSearchList<T>() where T : class
        {
            listBox1.Items.Clear();
            FullItemList = DataBase.GameDataBase.GetList<T>().OfType<IBasicInfo>().ToList();
            listBox1.Items.AddRange(FullItemList.ToArray());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetSearchList<WeaponItemsInfo>();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetSearchList<UsableItemsInfo>();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SetSearchList<BodyItemsInfo>();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SetSearchList<ShieldItemsInfo>();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SetSearchList<ArmItemsInfo>();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetSearchList<FootItemsInfo>();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            SetSearchList<MaterialItemsInfo>();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            foreach (var selectedElement in listBox1.Items)
            {
                if(listBox2.Items.IndexOf(selectedElement)==-1)
                 Filter.PickWeapon.Add((IBasicInfo)selectedElement);
            }

            listBox2.Items.Clear();
            listBox2.Items.AddRange(Filter.PickWeapon.ToArray());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Filter.PickWeapon.Remove((IBasicInfo)listBox2.SelectedItem);
            listBox2.Items.Clear();
            listBox2.Items.AddRange(Filter.PickWeapon.ToArray());
        }
    }
}
