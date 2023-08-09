using CodeInject.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.PickupFilters
{
    public interface IFilter
    {
        bool CanPickup(IObject item);
    }
}
