using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Actors
{
    public unsafe interface IPlayer
    {
         ushort ID { get; set; }
         float X { get; set; }
         float Y { get; set; }
         float Z { get; set; }

        string Name { get; set; }
        int MaxHp { get; set; }
         int Hp { get; set; }
        int MaxMp { get; set; }
        int Mp { get; set; }
    }
}
