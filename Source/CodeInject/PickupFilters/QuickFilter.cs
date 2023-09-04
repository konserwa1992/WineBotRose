using CodeInject.Actors;
using System.Collections.Generic;


namespace CodeInject.PickupFilters
{
    public class QuickFilter : IFilter
    {
        public List<ItemType> pickTypeList { get; private set; } = new List<ItemType>();

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
