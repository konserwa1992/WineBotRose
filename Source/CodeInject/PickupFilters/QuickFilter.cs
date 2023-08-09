using CodeInject.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.PickupFilters
{
    public class QuickFilter : IFilter
    {
        private List<ItemType> pickTypeList = new List<ItemType>();

        public QuickFilter() { }

        public bool AddToPick(ItemType type)
        {
            if (!pickTypeList.Contains(type))
            {
                pickTypeList.Add(type);
                return true;
            }

            return false;
        }

        public bool RemoveFromPick(ItemType type)
        {
            if (pickTypeList.Contains(type))
            {
                pickTypeList.Remove(type);
                return true;
            }

            return false;
        }

        public unsafe bool CanPickup(IObject item)
        {
            return pickTypeList.Contains((ItemType)(*(((Item)item).ItemType)));
        }
    }
}
