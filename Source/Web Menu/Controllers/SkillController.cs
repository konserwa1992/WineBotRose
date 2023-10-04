using CodeInject.WebServ.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bot_Menu.Controllers
{
    public class SkillController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SkillList([FromBody] string data)
        {
            PlayerSkillModel skillListModel = JsonConvert.DeserializeObject<PlayerSkillModel>(data);

            return PartialView(skillListModel);
        }

        [HttpGet]
        public IActionResult SkillList()
        {
            return View();
        }
    }
}
