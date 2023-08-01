using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Actors
{
    public unsafe interface IObject
    {
        long* ObjectPointer { get; set; }
        int* ID { get; set; }
        float* X { get; set; }
         float* Y { get; set; }
         float* Z { get; set; }

         double CalcDistance(IObject actor);
    }
}
