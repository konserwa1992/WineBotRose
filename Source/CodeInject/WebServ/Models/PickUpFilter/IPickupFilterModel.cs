﻿using CodeInject.PickupFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.WebServ.Models.PickUpFilter
{
    public  interface IPickupFilterModel
    {
        string Name { get; set; }
        List<ItemType> Filter { get; set; }
    }
}
