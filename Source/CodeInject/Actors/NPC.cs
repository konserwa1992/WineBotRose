using CodeInject.WebServ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Actors
{
    public unsafe class  NPC: IObject
    {
        public long* ObjectPointer { get; set; }
        public int* ID { get; set; }
        public MobInfo Info { get; set; }
        public float* X { get; set; }
        public float* Y { get; set; }
        public float* Z { get; set; }
        public short* Hp { get; set; }
        public short* MaxHp { get; set; }
        public int* modelNaME { get; set; }

        public NPC(long* Entry)
        {
            try//Walk around for error TODO:Fix it
            {
                ObjectPointer = (long*)*Entry;


                X = (float*)(*Entry + 0x10);
                Y = (float*)(*Entry + 0x14);
                Z = (float*)(*Entry + 0x18);
                if ((long*)(*Entry + 0x20) != null)
                    ID = (int*)(*((long*)(*Entry + 0x20)));
                Hp = (short*)(*Entry + 0xE8);
                MaxHp = (short*)(*Entry + 0xF0);

                Info = DataBase.GameDataBase.MonsterDatabase.FirstOrDefault(x => x.ID == (*(short*)(*Entry + 0x2c0)));
            }catch (Exception) { }
        }

        public double CalcDistance(IObject actor)
        {
            return Math.Sqrt(Math.Pow((*actor.X / 100) - (*this.X / 100), 2) + Math.Pow((*actor.Y / 100) - (*this.Y / 100), 2) + Math.Pow((*actor.Z / 100) - (*this.Z / 100), 2));
        }


        public NPCModel ToWSObject()
        {
            return new NPCModel
            {
                Hp = *Hp,
                MaxHp = *MaxHp,
                X = *X,
                Y = *Y,
                Z = *Z,
                Name = ToString()
            };
        }


        public double CalcDistance(float x,float y,float z)
        {
            return Math.Sqrt(
                  Math.Pow((x / 100) - (*this.X / 100), 2) 
                + Math.Pow((y / 100) - (*this.Y / 100), 2) 
                + Math.Pow((z / 100) - (*this.Z / 100), 2));
        }

        public override string ToString()
        {
            return $"{(*ID).ToString("X")} {(Info != null ? Info.Name :"Unknow")}";
        }
    }
}
