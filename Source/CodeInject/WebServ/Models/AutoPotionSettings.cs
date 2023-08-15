using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.WebServ.Models
{
    public class AutoPotionSettings
    {
        public List<ItemModel> ItemsList = new List<ItemModel>();
        public int MinHelath;
        public int MinMana;
        public int HealthItemIndex;
        public int ManaItemIndex;
        public int HelathDurration;
        public int ManaDurration;
    }
}
