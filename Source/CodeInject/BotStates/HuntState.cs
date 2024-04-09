using CodeInject.Actors;
using CodeInject.Hunt;
using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace CodeInject.BotStates
{
    public class HuntState : IBotState
    {
        public IHuntSetting HuntInstance;
  
        public HuntState(IHuntSetting huntInstance)
        {
            HuntInstance = huntInstance;
        }


        public unsafe void Work(BotContext context)
        {
            if(GameHackFunc.Game.ClientData.GetPlayer().Hp <=0)
            {
                context.SetState("STANDBY");
            }


            if(context.GetItemsNearby().Count > 0)
            {
                context.SetState("PICK");
                return;
            }
            HuntInstance.Update();
        }
    }
}
