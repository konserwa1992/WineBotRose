using CodeInject.MemoryTools;
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
    internal class Load
    {
        public Load(BotContext contex, int ID)
        {
            StreamReader wr = new StreamReader(DataBase.DataPath + "\\TEST.txt", false);
            LoadAutopotion(contex, JsonConvert.DeserializeObject<AutoPotionSettingsModel>(wr.ReadToEnd()));

            wr.Close();
        }


        private void LoadAutopotion(BotContext contex, AutoPotionSettingsModel model)
        {
            AutoPotionModule module = new AutoPotionModule();
            module.AutoHp = LoadHealthPotion(model);
            module.AutoMp = LoadManaPotion(model);

            AutoPotionModule reffer = contex.GetModule<AutoPotionModule>(module.Name);
            reffer = module;
        }


        private unsafe ItemExecutor LoadHealthPotion(AutoPotionSettingsModel model)
        {
            List<InvItem> items = new List<InvItem>();
            GameHackFunc.Game.ClientData.GetConsumableItemsFromInventory(items);

            InvItem hpPotionItem = items.FirstOrDefault(x => *x.ItemData == model.HealthItemIndex);

            if (hpPotionItem != null) {
                ItemExecutor exechp = new ItemExecutor(model.HelathDurration, model.MinHelath, hpPotionItem);
                return exechp;
            }
            else
            {
                Exception ex = new Exception("There is no longer saved item hp id in inventory.");
            }

            return null;
       }

        private unsafe ItemExecutor LoadManaPotion(AutoPotionSettingsModel model)
        {
            List<InvItem> items = new List<InvItem>();
            GameHackFunc.Game.ClientData.GetConsumableItemsFromInventory(items);

            InvItem mpPotionItem = items.FirstOrDefault(x => *x.ItemData == model.ManaItemIndex);

            if (mpPotionItem != null)
            {
                ItemExecutor exechp = new ItemExecutor(model.ManaDurration, model.MinMana, mpPotionItem);
                return exechp;
            }
            else
            {
                Exception ex = new Exception("There is no longer saved item mp id in inventory.");
            }

            return null;
        }
    }
}
