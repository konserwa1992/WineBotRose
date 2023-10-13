using System.Collections.Generic;

namespace CodeInject.WebServ.Models.PickUpFilter
{
    public  interface IPickupFilterModel
    {
        string Name { get; set; }
        List<ItemType> Filter { get; set; }
    }
}
