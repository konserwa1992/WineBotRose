using System;
using System.Collections.Generic;

namespace CodeInject.BotStates
{
    public interface IBotState
    {
        void Work(BotContext context);
    }
}
