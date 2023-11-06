using CodeInject.Actors;
using System;
using System.Collections.Generic;

namespace CodeInject.BotStates
{
    public class PickUpState : IBotState
    {
        public PickUpState()
        {
        }

        public void Work(BotContext context)
        {
            List<IObject> itemsAround = context.GetItemsNearby();
            if (itemsAround.Count > 0)
                ((Item)itemsAround[0]).Pickup();
            else
                context.SetState("HUNT");
        }
    }
}
