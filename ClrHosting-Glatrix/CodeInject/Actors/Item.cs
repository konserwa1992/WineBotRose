using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Actors
{
    internal unsafe class Item : IObject
    {
        public long* ObjectPointer { get; set; }
        public int* ID { get; set; }
        public ushort? Index { get; set; }
        public short* ItemData { get; set; }
        public float* X { get; set; }
        public float* Y { get; set; }
        public float* Z { get; set; }

        /// <summary>
        /// 0x08 Weapon
        /// 0xA - usable items potions etc
        /// 0x02 - hat
        /// 0x03 - chest armor
        /// 0x04 - gloves
        /// 0x05 - shoes
        /// 0x0C - Material
        /// </summary>
        public short* ItemType { get;set; }

        private void Init(long* Entry)
        {
            ObjectPointer = Entry;


            X = (float*)((long)Entry + 0x10);
            Y = (float*)((long)Entry + 0x14);
            Z = (float*)((long)Entry + 0x18);
            ID = (int*)(*((long*)((long)Entry + 0x1c)));
            ItemData = (short*)((long)Entry + 0x6c);

            ItemType = (short*)((long)Entry + 0x68);
        }

        public Item(long* Entry)
        {
            Init(Entry);
        }

        public Item(ushort index,long* Entry)
        {
            Index = index;
            Init(Entry);
        }

        public void Pickup()
        {
            if(Index!=null)
            GameFunctionsAndObjects.Actions.PickUp((ushort)Index);
        }

        public double CalcDistance(IObject actor)
        {
            return Math.Sqrt(Math.Pow(*actor.X - *this.X, 2) + Math.Pow(*actor.Y - *this.Y, 2) + Math.Pow(*actor.Z - *this.Z, 2));
        }

        public override string ToString()
        {
            BasicInfo temp;


            switch (*ItemType)
            {
                case 0x08:
                    {
                        temp = DataBase.GameDataBase.WeaponItemsDatabase.FirstOrDefault(x => x.ID == *ItemData);
                        return $"{(temp != null ? temp.Name : "Unknow")}";
                    }
                case 0x0A:
                    {
                        temp = DataBase.GameDataBase.UsableItemsDatabase.FirstOrDefault(x => x.ID == *ItemData);
                        return $"{(temp != null ? ((UsableItemsInfo)temp).DisplayName : "Unknow")}";
                    }

                case 0x03:
                    {
                        temp = DataBase.GameDataBase.BodyItemsDatabase.FirstOrDefault(x => x.ID == *ItemData);
                        return $"{(temp != null ? temp.Name : "Unknow")}";
                    }

                case 0x05:
                    {
                        temp = DataBase.GameDataBase.FootItemsDatabase.FirstOrDefault(x => x.ID == *ItemData);
                        return $"{(temp != null ? temp.Name : "Unknow")}";
                    }

                case 0x04:
                    {
                        temp = DataBase.GameDataBase.ArmItemsDatabase.FirstOrDefault(x => x.ID == *ItemData);
                        return $"{(temp != null ? temp.Name : "Unknow")}";
                    }
                default:
                    {
                        return $"Unknow type:{(*ItemType).ToString("X")} id:{(*ItemData).ToString("X")}";
                    }
            }
        }
    }
}
