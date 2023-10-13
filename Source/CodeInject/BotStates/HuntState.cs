using CodeInject.Hunt;
using System.Collections.Generic;
using System.Net;

namespace CodeInject.BotStates
{
    public class HuntState : IBotState
    {
        public IHuntSetting HuntInstance;
  
        public HuntState(IHuntSetting huntInstance)
        {
            HuntInstance = huntInstance;
        }


        public void Work(BotContext context)
        {
            if(context.GetItemsNearby().Count > 0)
            {
                context.SetState("PICK");
                return;
            }
            HuntInstance.Update();
        }
    }
}
