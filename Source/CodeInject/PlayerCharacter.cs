using CodeInject.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject
{
    class PlayerCharacter
    {
        public static IObject PlayerInfo {
            get
            {
                return GameFunctionsAndObjects.DataFetch.GetPlayer();
            }
        }

        public static List<Skills> GetPlayerSkills
        {
            get
            {
                return GameFunctionsAndObjects.DataFetch.GetPlayerSkills();
            }
        }
    }
}
