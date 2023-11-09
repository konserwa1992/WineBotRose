using CodeInject.Actors;
using System.Collections.Generic;
using System.Linq;

namespace CodeInject.PickupFilters
{
    internal class AdvancedFilter : IFilter
    {
        public List<IBasicInfo> PickWeapon { get; set; } = new List<IBasicInfo>();

        public unsafe bool CanPickup(IObject item)
        {
            Item item2Check = item as Item;

           switch((ItemType)(*item2Check.ItemType))
            {
                case ItemType.Weapon:
                    return PickWeapon.Any(x => x.ID == *item2Check.ItemData && x.GetType() == typeof(WeaponItemsInfo));
                case ItemType.UsableItem:
                    return PickWeapon.Any(x => x.ID == *item2Check.ItemData && x.GetType() == typeof(UsableItemsInfo));
                case ItemType.Hat:
                    return PickWeapon.Any(x => x.ID == *item2Check.ItemData && x.GetType() == typeof(HeadItemsInfo));
                case ItemType.Material:
                    return PickWeapon.Any(x => x.ID == *item2Check.ItemData && x.GetType() == typeof(MaterialItemsInfo));
                case ItemType.Shield:
                    return PickWeapon.Any(x => x.ID == *item2Check.ItemData && x.GetType() == typeof(ShieldItemsInfo));
                case ItemType.Shoes:
                    return PickWeapon.Any(x => x.ID == *item2Check.ItemData && x.GetType() == typeof(FootItemsInfo));
                case ItemType.Gloves:
                    return PickWeapon.Any(x => x.ID == *item2Check.ItemData && x.GetType() == typeof(ArmItemsInfo));
                case ItemType.ChestArmor:
                    return PickWeapon.Any(x => x.ID == *item2Check.ItemData && x.GetType() == typeof(BodyItemsInfo));
       
                default: 
                    return false;
            }
        }
    }
}
