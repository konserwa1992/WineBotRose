using Bot_Menu.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Bot_Menu.Mods;
using Newtonsoft.Json;

namespace Bot_Menu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static PlayerInfo pl = new PlayerInfo();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult IndexAsync()
        {
            pl.Update();

            return View();
        }

        [HttpGet]
        public IActionResult UserInformation()
        {
            pl.Update();
            return PartialView(pl.CharacterInfoJson);
        }

        [HttpGet]
        public IActionResult AutoPotion()
        {
            pl.Update();
            return View(pl.AutoPotionSettings);
        }

        [HttpPost]
        public IActionResult AutoPotionSettings(int procHelath, int hpItemIndex,int hpItemDurr, int procMana,int mpItemIndex, int mpItemDurr)
        {
            pl.webSocket2.Send(JsonConvert.SerializeObject(new
            {
                OpCode = "SetPotions",
                procHelath = procHelath,
                hpItemIndex = hpItemIndex,
                hpItemDurr = hpItemDurr,
                procMana = procMana,
                mpItemIndex = mpItemIndex,
                mpItemDurr = mpItemDurr
            }));


            return null;
        }

        [HttpGet]
        public IActionResult NpcList()
        {
            pl.Update();
            if (pl.NPCList != "")
                return PartialView(JsonConvert.DeserializeObject<List<NpcViewModel>>(pl.NPCList));
            else
                return PartialView(new List<NpcViewModel>());
        }

        public class SelectedValuesModel
        {
            public List<string> SelectedValues { get; set; }
        }

        [HttpPost()]
        public IActionResult UseSkillConfirm([FromBody] List<int> selectedValues)
        {
     
            pl.webSkillSocket.Send(JsonConvert.SerializeObject(new SetSkills()
            {
                setSkills = selectedValues
            }));
            pl.Update();
            return null;
        }


        [HttpGet]
        public IActionResult SkillList()
        {
            pl.Update();

            return View(pl.Skills);
        }

        [HttpGet]
        public IActionResult TargetInfo()
        {
            pl.Update();
            if (pl.AttackedNpc!=null)
                return PartialView(pl.AttackedNpc);
            else
                return PartialView(new NpcViewModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult SimpleFilter()
        {
            return View();
        }

        public IActionResult PickupFilter()
        {

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}