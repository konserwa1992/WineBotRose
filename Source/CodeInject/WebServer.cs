using CodeInject.Actors;
using CodeInject.MemoryTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using static CodeInject.WebSocketServices;

namespace CodeInject
{
    internal class WebServer
    {
        WebSocketServer server;

        public void SetupWebSocketServer(int port = 2458)
        {
            server = new WebSocketServer($"ws://localhost:{port}");

            server.AddWebSocketService<MyWebSocketService>("/CharacterInfo");
            server.AddWebSocketService<AutoPotionService>("/AutoPotion");
            server.AddWebSocketService<NPCService>("/NpcList");
            server.AddWebSocketService<SkillService>("/SkillList");
            server.AddWebSocketService<PickUpFilterService>("/Filter");

            server.Start();
        }

        public void SendPlayerInformation()
        {
            string characterJson = GameFunctionsAndObjects.DataFetch.GetPlayer().ToString();
            foreach (var session in server.WebSocketServices["/CharacterInfo"].Sessions.Sessions)
            {
                server.WebSocketServices["/CharacterInfo"].Sessions.SendTo(characterJson, session.ID);
            }
        }

        public void SendNPCsInformation()
        {
            List<IObject> list = GameFunctionsAndObjects.DataFetch.GetNPCs();

            List<object> toSerialzie = new List<object>();
            NPC last = null;
            foreach (IObject npc in list)
            {
                last = (NPC)npc;
                toSerialzie.Add(((NPC)npc).ToWSObject());
            }
            string npcListJson = JsonConvert.SerializeObject(toSerialzie);


            foreach (var session in server.WebSocketServices["/NpcList"].Sessions.Sessions)
            {
                server.WebSocketServices["/NpcList"].Sessions.SendTo(npcListJson, session.ID);

               /* server.WebSocketServices["/NpcList"].Sessions.SendTo(JsonConvert.SerializeObject(new
                {
                    AttackedNPC = ((NPC)WineBot.WineBot.Instance.Target).ToWSObject()
                }),session.ID);*/

            }
        }

    }
}
