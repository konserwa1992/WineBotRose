using CodeInject.MemoryTools;
using CodeInject.WebServ.Models;
using System.Linq;

namespace CodeInject
{
    public unsafe class InvItem
    {
        public long* ObjectPointer { get; set; }
        public long* CItemAddr { get; set; }
        public short* ItemData { get; set; }

        /// <summary>
        /// 0x08 Weapon
        /// 0x09 Shield
        /// 0xA - usable items potions etc
        /// 0x02 - hat
        /// 0x03 - chest armor
        /// 0x04 - gloves
        /// 0x05 - shoes
        /// 0x0C - Material
        /// </summary>
        public short* ItemType { get; set; }

        public InvItem(long* ItemDBAddr, long* cItemAddr)
        {
            ObjectPointer = ItemDBAddr;
            CItemAddr = cItemAddr;

            ItemData = (short*)((long)ItemDBAddr + 0x0c);
            ItemType = (short*)((long)ItemDBAddr + 0x08);
            CItemAddr = cItemAddr;
        }

        public ItemModel ToWSObject()
        {
            return new ItemModel()
            {
                Id = (long)ObjectPointer,
                Name = ToString()
            };
        }


        public void UseItem()
        {
            GameFunctionsAndObjects.Actions.ItemUse((long)CItemAddr);
        }

        public override string ToString()
        {
            IBasicInfo temp;


            switch (*ItemType)
            {
                case 0x08:
                    {
                        temp = DataBase.GameDataBase.WeaponItemsDatabase.FirstOrDefault(x => x.ID == *ItemData);
                        return $"{(temp != null ? temp.ID+ " "+ temp.Name : "Unknow")}";
                    }

                case 0x0A:
                    {
                        temp = DataBase.GameDataBase.UsableItemsDatabase.FirstOrDefault(x => x.ID == *ItemData);
                        return $"{(temp != null ? temp.ID + " " + ((UsableItemsInfo)temp).DisplayName : "Unknow")}";
                    }

                case 0x03:
                    {
                        temp = DataBase.GameDataBase.BodyItemsDatabase.FirstOrDefault(x => x.ID == *ItemData);
                        return $"{(temp != null ? temp.ID + " " + temp.Name : "Unknow")}";
                    }

                case 0x05:
                    {
                        temp = DataBase.GameDataBase.FootItemsDatabase.FirstOrDefault(x => x.ID == *ItemData);
                        return $"{(temp != null ? temp.ID + " " + temp.Name : "Unknow")}";
                    }

                case 0x04:
                    {
                        temp = DataBase.GameDataBase.ArmItemsDatabase.FirstOrDefault(x => x.ID == *ItemData);
                        return $"{(temp != null ? temp.ID + " " + temp.Name : "Unknow")}";
                    }
                case 0x09:
                    {
                        temp = DataBase.GameDataBase.SheildItemsDatabase.FirstOrDefault(x => x.ID == *ItemData);
                        return $"{(temp != null ? temp.ID + " " + temp.Name : "Unknow")}";
                    }

                case 0x0C:
                    {
                        temp = DataBase.GameDataBase.MaterialItemsDatabase.FirstOrDefault(x => x.ID == *ItemData);
                        return $"{(temp != null ? temp.ID + " " + temp.Name : "Unknow")}";
                    }
                default:
                    {
                        return $"Unknow type:{(*ItemType).ToString("X")} id:{(*ItemData).ToString("X")}";
                    }
            }
        }
    }
}
