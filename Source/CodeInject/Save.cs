using CodeInject.Modules;
using CodeInject.WebServ.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject
{
    public class Save
    {
        public Save(BotContext contex,int ID)
        {
            StreamWriter wr = new StreamWriter(DataBase.DataPath + "\\TEST.txt", false);
            wr.WriteLine(SaveAutopotion(contex));
            wr.Close();
        }


        private unsafe string SaveAutopotion(BotContext contex)
        {
            AutoPotionModule module = contex.GetModule<AutoPotionModule>("AUTOPOTION");

            AutoPotionSettingsModel model = new AutoPotionSettingsModel();
            model.HealthItemIndex = (int)*module.AutoHp.Item2Cast.ItemData;
            model.ManaItemIndex = (int)*module.AutoMp.Item2Cast.ItemData;

            model.MinHelath = module.AutoHp.MinValueToExecute;
            model.MinMana = module.AutoMp.MinValueToExecute;

            model.HelathDurration = module.AutoHp.CooldDown;
            model.ManaDurration = module.AutoMp.CooldDown;

            return JsonConvert.SerializeObject(model);
        }
    }
}
