using CodeInject.Actors;
using CodeInject.PickupFilters;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            string[] configFiles = Directory.GetFiles(DataBase.DataPath + "config\\filters");

            foreach (var file in configFiles)
            {
                string filename = file.Substring(file.LastIndexOf('\\') + 1);
                filename = filename.Substring(0, filename.LastIndexOf('.'));
                comboBox1.Items.Add(filename);
            }


            if(comboBox1.Items.Count!=0)
            {
                comboBox1.SelectedIndex = 0;
            }
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

        private void button4_Click_1(object sender, EventArgs e)
        {
            SetSearchList<HeadItemsInfo>();
        }


        private IBasicInfo GetItemFromConfig(string itemLine)
        {
            string[] line = itemLine.Split(';');
            if(typeof(WeaponItemsInfo).ToString() == line[0])
            {
                return DataBase.GameDataBase.WeaponItemsDatabase.FirstOrDefault(x => x.ID == int.Parse(line[1]));
            }else if(typeof(UsableItemsInfo).ToString() == line[0])
            {
                return DataBase.GameDataBase.UsableItemsDatabase.FirstOrDefault(x => x.ID == int.Parse(line[1]));
            }
            else if (typeof(BodyItemsInfo).ToString() == line[0])
            {
                return DataBase.GameDataBase.BodyItemsDatabase.FirstOrDefault(x => x.ID == int.Parse(line[1]));
            }
            else if (typeof(ShieldItemsInfo).ToString() == line[0])
            {
                return DataBase.GameDataBase.SheildItemsDatabase.FirstOrDefault(x => x.ID == int.Parse(line[1]));
            }
            else if (typeof(ArmItemsInfo).ToString() == line[0])
            {
                return DataBase.GameDataBase.ArmItemsDatabase.FirstOrDefault(x => x.ID == int.Parse(line[1]));
            }
            else if (typeof(FootItemsInfo).ToString() == line[0])
            {
                return DataBase.GameDataBase.FootItemsDatabase.FirstOrDefault(x => x.ID == int.Parse(line[1]));
            }
            else if (typeof(MaterialItemsInfo).ToString() == line[0])
            {
                return DataBase.GameDataBase.MaterialItemsDatabase.FirstOrDefault(x => x.ID == int.Parse(line[1]));
            }

            return null;
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            StreamWriter save2File = new StreamWriter(DataBase.DataPath + "config\\filters\\"+ textBox1.Text+".bin", false);

            foreach(var item in Filter.PickWeapon)
            {
                save2File.WriteLine(item.GetType()+";"+item.ID);
            }

            save2File.Close();
        }

        private void bLoad_Click(object sender, EventArgs e)
        {
            StreamReader config = new StreamReader(DataBase.DataPath + "config\\filters\\" + comboBox1.SelectedItem+".bin");

            Filter.PickWeapon.Clear();

            while (!config.EndOfStream)
            {
                IBasicInfo itemFromLine = GetItemFromConfig(config.ReadLine());

                if (itemFromLine != null)
                    Filter.PickWeapon.Add(itemFromLine);
            }
            config.Close();
            listBox2.Items.Clear();
            listBox2.Items.AddRange(Filter.PickWeapon.ToArray());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = comboBox1.SelectedItem.ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SetSearchList<MountItemsInfo>();
        }
    }
}
