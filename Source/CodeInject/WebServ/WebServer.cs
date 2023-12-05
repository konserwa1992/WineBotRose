using CodeInject.Actors;
using CodeInject.BotStates;
using CodeInject.MemoryTools;
using CodeInject.WebServ.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
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
            string characterJson = GameHackFunc.ClientData.GetPlayer().ToWSObject();
            foreach (var session in server.WebSocketServices["/CharacterInfo"].Sessions.Sessions)
            {
                server.WebSocketServices["/CharacterInfo"].Sessions.SendTo(characterJson, session.ID);
            }
        }

        public void SendNPCsInformation()
        {
            List<IObject> list = GameHackFunc.ClientData.GetNPCs();

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

                /*if (WineBot.WineBot.Instance.BotContext.GetState<HuntState>("HUNT").HuntInstance.Target != null)
                {
                    server.WebSocketServices["/NpcList"].Sessions.SendTo(JsonConvert.SerializeObject(new TargetInfoModel()
                    {
                        AttackedNPC = ((NPC)WineBot.WineBot.Instance.BotContext.GetState<HuntState>("HUNT").HuntInstance.Target).ToWSObject()
                    }), session.ID);
                }*/
            }
        }

    }
}
