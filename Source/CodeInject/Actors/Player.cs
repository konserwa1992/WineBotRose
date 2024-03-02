using CodeInject.MemoryTools;
using CodeInject.WebServ.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CodeInject.Actors
{
    public unsafe class Player : IObject, IPlayer
    {
        public long ObjectPointer { get; set; }
        public ushort ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public int MaxMp { get; set; }
        public int Mp { get; set; }
        public string Name { get; set; } = "";
        public short BuffCount { get; set; }
        public int modelNaME { get; set; }

        public Player(long* Entry)
        {
            ObjectPointer = (long)(long*)*Entry;

            X = *(float*)(*Entry + 0x10);
            Y = *(float*)(*Entry + 0x14);
            Z = *(float*)(*Entry + 0x18);
            ID = *(ushort*)(*((long*)(*Entry + 0x20)));
            Hp = *(int*)(*Entry + 0x3B90);
            MaxHp = *(int*)(*Entry + 0x3D3C);
            Mp = *(int*)(*Entry + 0x3B94);
            MaxMp = *(int*)(*Entry + 0x46CC);
            BuffCount = *(short*)(*Entry + 0x858);
            Name = Marshal.PtrToStringAnsi(new IntPtr((*Entry + 0xBB8)));
        }

        public double CalcDistance(IObject targerObject)
        {
            return Math.Sqrt(Math.Pow((targerObject.X / 100) - (this.X / 100), 2) + Math.Pow((targerObject.Y / 100) - (this.Y / 100), 2) + Math.Pow((targerObject.Z / 100) - (this.Z / 100), 2));
        }

        public double CalcDistance(float x, float y, float z)
        {
            return Math.Sqrt(
                  Math.Pow((x / 100) - (this.X / 100), 2)
                + Math.Pow((y / 100) - (this.Y / 100), 2)
                + Math.Pow((z / 100) - (this.Z / 100), 2));
        }
        public string ToWSObject()
        {
            string playerJson =
                JsonConvert.SerializeObject(new PlayerInfoModel()
                { 
                    Hp = Hp,
                    MaxHp = MaxHp,
                    Mp = Mp,
                    MaxMp = MaxMp,
                    X = X,
                    Y = Y,
                    Z = Z,
                    BuffCount = BuffCount,
                    Name = Name
                }, Formatting.Indented); 

            return playerJson;
        }

        public List<ushort> GetBuffsIDs()
        {

            List<ushort> list = new List<ushort>();
     

            long baseBuffAddres = MemoryTools.MemoryTools.GetInt64(GameHackFunc.ClientData.BaseAddres + 0x014ab8b0, new short[] { 0x410 }); //#IMG12
            baseBuffAddres += 0x440;


            long* buffAddr =(long*)*(long*)baseBuffAddres;  //RAX
            int b = 0;


            long* firstElement;
            firstElement=(long*)*buffAddr; // (long*)(*(long*)(*buffAddr)) or (long*)(*buffAddr);

            
            while (b < BuffCount)
            {
                    long* buffDetails = (long*)(*(long*)((long)firstElement + 0x18));
                    if (*(long*)((long)buffDetails + 0x08) == (long)ObjectPointer)
                    {
                        ushort* buffID = (ushort*)((long)buffDetails + 0x18);

                        list.Add(*buffID);
                        b++;
                    }
                    firstElement = (long*)(*firstElement);
             } 
       

            return list;
        }

        public override string ToString()
        {
            return $"[{(ID).ToString("X")}] {ObjectPointer.ToString("X")} {Name} {BuffCount}";
        }
 
    }
}
