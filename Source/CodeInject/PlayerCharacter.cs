using CodeInject.Actors;
using CodeInject.MemoryTools;
using System.Collections.Generic;


namespace CodeInject
{
    class PlayerCharacter
    {
        public static IObject PlayerInfo {
            get
            {
                return GameHackFunc.Game.ClientData.GetPlayer();
            }
        }

        public static List<Skills> GetPlayerSkills
        {
            get
            {
                return GameHackFunc.Game.ClientData.GetPlayerSkills();
            }
        }
    }
}
