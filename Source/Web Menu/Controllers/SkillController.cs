using CodeInject.WebServ.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bot_Menu.Controllers
{
    public class SkillController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SkillConfirm([FromBody] List<int> selectedValues)
        {
       
            SetSkillsModel SkillsToSet = new SetSkillsModel()
            {
                setSkills = selectedValues
            };

            WebSocketSharp.WebSocket setSkillSocket = new WebSocketSharp.WebSocket("ws://localhost:2458/SkillList");
           
            setSkillSocket.OnOpen += (sender, e) =>
            {
                string json = JsonConvert.SerializeObject(SkillsToSet);
                setSkillSocket.Send(json);
            };

            setSkillSocket.Connect();

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
