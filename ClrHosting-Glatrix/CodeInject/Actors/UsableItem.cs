using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Actors
{
    internal unsafe class UsableItem : IObject
    {
        public long* ObjectPointer { get; set; }
        public int* ID { get; set; }
        public ushort? Index { get; set; }
        public short* ItemType { get; set; }
        public float* X { get; set; }
        public float* Y { get; set; }
        public float* Z { get; set; }
  

        public UsableItem(long* Entry)
        {
            ObjectPointer = Entry;


            X = (float*)((long)Entry + 0x10);
            Y = (float*)((long)Entry + 0x14);
            Z = (float*)((long)Entry + 0x18);
            ID = (int*)(*((long*)((long)Entry + 0x1c)));
            ItemType = (short*)((long)Entry + 0x6c);
        }

        public UsableItem(ushort index,long* Entry)
        {
            ObjectPointer = Entry;

            Index = index;
            X = (float*)((long)Entry + 0x10);
            Y = (float*)((long)Entry + 0x14);
            Z = (float*)((long)Entry + 0x18);
            ID = (int*)(*((long*)((long)Entry + 0x1c)));
            ItemType = (short*)((long)Entry + 0x6c);
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
            UsableItemsInfo temp = DataBase.GameDataBase.UsableItmesDatabase.FirstOrDefault(x => x.ID == *ItemType);
            return $"{(temp!=null?temp.DisplayName:"Unknow")}";
        }
    }
}
