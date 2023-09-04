using CodeInject.PickupFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.WebServ.Models.PickUpFilter
{
    public class SimpleFilterModel : IPickupFilterModel
    {
        public string Name { get; set; } = "Simple";
        public object Filter { get; set; }
    }
}
