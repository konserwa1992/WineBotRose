using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Actors
{
    unsafe class Player : IObject
    {
        public long* ObjectPointer { get; set; }
        public int* ID { get; set; }
        public float* X { get; set; }
        public float* Y { get; set; }
        public float* Z { get; set; }
        public int* MaxHp { get; set; }
        public int* Hp { get; set; }
        public int* MaxMp { get; set; }
        public int* Mp { get; set; }
        public int* modelNaME { get; set; }

        public Player(long* Entry)
        {
            ObjectPointer = (long*)*Entry;


            X = (float*)(*Entry + 0x10);
            Y = (float*)(*Entry + 0x14);
            Z = (float*)(*Entry + 0x18);
            ID = (int*)(*((long*)(*Entry + 0x20)));
            Hp = (int*)(*Entry + 0x3AE8);
            MaxHp = (int*)(*Entry + 0x3C94);
            Mp = (int*)(*Entry + 0x3AEC);
            MaxMp = (int*)(*Entry + 0x4624);
        }

        public double CalcDistance(IObject actor)
        {
            return Math.Sqrt(Math.Pow((*actor.X / 100) - (*this.X / 100), 2) + Math.Pow((*actor.Y / 100) - (*this.Y / 100), 2) + Math.Pow((*actor.Z / 100) - (*this.Z / 100), 2));
        }

        public double CalcDistance(float x, float y, float z)
        {
            return Math.Sqrt(
                  Math.Pow((x / 100) - (*this.X / 100), 2)
                + Math.Pow((y / 100) - (*this.Y / 100), 2)
                + Math.Pow((z / 100) - (*this.Z / 100), 2));
        }

        public override string ToString()
        {
            return "Player";
        }
    }
}
