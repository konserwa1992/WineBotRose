
using Bot_Menu.Models;
using Newtonsoft.Json;

namespace Bot_Menu.Mods
{
    public class PlayerInfo : IMod
    {
        public WebSocketSharp.WebSocket webSocket = new WebSocketSharp.WebSocket("ws://localhost:8080/CharacterInfo");
        public WebSocketSharp.WebSocket webSocket2 = new WebSocketSharp.WebSocket("ws://localhost:8080/AutoPotion");
        public  WebSocketSharp.WebSocket webSocket3 = new WebSocketSharp.WebSocket("ws://localhost:8080/NpcList");
        public WebSocketSharp.WebSocket webSkillSocket = new WebSocketSharp.WebSocket("ws://localhost:8080/SkillList");

        public WebSocketSharp.WebSocket webPickUpSocket = new WebSocketSharp.WebSocket("ws://localhost:8080/Filter");

        public NpcViewModel AttackedNpc {  get; set; }

        public PlayerInfoViewModel CharacterInfoJson { get; set; }
        public AutoPotionSettings AutoPotionSettings { get; set; } = new AutoPotionSettings();
        public SkillsListsModel Skills { get; set; } = new SkillsListsModel();
        public string NPCList { get; set; } = "";

        public PlayerInfo() {
           
            webSocket.OnMessage += (sender, e) =>
            {
                this.CharacterInfoJson = JsonConvert.DeserializeObject<PlayerInfoViewModel>(e.Data);
            };

            webSocket.Connect();


            webSocket2.OnMessage += (sender, e) =>
            {
                this.AutoPotionSettings = JsonConvert.DeserializeObject<AutoPotionSettings>(e.Data);
            };

            webSocket2.Connect();


            webSocket3.OnMessage += (sender, e) =>
            {

                if(e.Data.Contains("AttackedNPC"))
                {
                    dynamic obj = JsonConvert.DeserializeObject<dynamic>(e.Data);
                    AttackedNpc = new NpcViewModel()
                    { 
                        Hp = obj.AttackedNPC.Hp, 
                        X = obj.AttackedNPC.X,
                        Y = obj.AttackedNPC.Y,
                        Z = obj.AttackedNPC.Z,
                        MaxHp = obj.AttackedNPC.MaxHp,
                        Name = obj.AttackedNPC.Name
                    };
 
                }
                else
                this.NPCList = e.Data;
            };

            webSocket3.Connect();

            webSkillSocket.OnMessage += (sender, e) =>
            {
                this.Skills = JsonConvert.DeserializeObject<SkillsListsModel>(e.Data);
            };


            webSkillSocket.Connect();



            webPickUpSocket.OnMessage += (sender, e) =>
            {
                Console.WriteLine(e.Data);
            };
            webPickUpSocket.Connect();
        }



        public void Update()
        {
            webSocket.Send("Connected");
            webSocket2.Send("GetAutoPotionSettings");
            webSocket3.Send("GetItems");
            webSkillSocket.Send("GetItems");
            webPickUpSocket.Send("GetFilter");
        }
    }
}
