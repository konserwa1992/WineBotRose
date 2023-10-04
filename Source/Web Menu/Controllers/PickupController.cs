using CodeInject.WebServ.Models.PickUpFilter;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace Bot_Menu.Controllers
{
    public class PickupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SimpleFilter([FromBody]string filter)
        {
            SimpleFilterModel model = JsonConvert.DeserializeObject<SimpleFilterModel>(filter);
            return PartialView(JsonConvert.DeserializeObject<SimpleFilterModel>(filter));
        }
    }
}
