using Bot_Menu.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using CodeInject.WebServ.Models;
using Microsoft.AspNetCore.SignalR;
using WebSocketSharp;

namespace Bot_Menu.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult IndexAsync()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UserInformation(string characterDataJson)
        {
            return PartialView(JsonConvert.DeserializeObject<PlayerInfoModel>(characterDataJson));
        }

        [HttpGet]
        public IActionResult AutoPotion()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAutoPotionSettings(string json)
        {
            AutoPotionSettingsModel potSettings= JsonConvert.DeserializeObject<AutoPotionSettingsModel>(json);
            return PartialView(potSettings);
        }

        [HttpPost]
        public IActionResult SetAutoPotionSettings(int procHelath, int hpItemIndex,int hpItemDurr, int procMana,int mpItemIndex, int mpItemDurr)
        {
            WebSocketSharp.WebSocket setPotions = new WebSocketSharp.WebSocket("ws://localhost:8080/AutoPotion");

            setPotions.Send(JsonConvert.SerializeObject(new
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
        public IActionResult NpcList(string npcDataJson)
        {
            return PartialView(JsonConvert.DeserializeObject<List<CodeInject.WebServ.Models.NPCModel>>(npcDataJson));
        }

        public class SelectedValuesModel
        {
            public List<string> SelectedValues { get; set; }
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