namespace Bot_Menu.Models
{
    public class ItemModel
    {
        public string Name;
        public long Id;
    }


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
