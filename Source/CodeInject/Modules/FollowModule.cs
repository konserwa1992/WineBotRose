using CodeInject.Actors;
using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeInject.Modules
{
    unsafe class FollowModule : IModule
    {
        public string Name { get; set; } = "FOLLOW";

        public string FollowPlayerName {  get; set; }

        public FollowModule(string folowPlayerName) {
            FollowPlayerName = folowPlayerName;
        }

        public void update()
        {
            IPlayer fPlayer = (IPlayer)GameFunctionsAndObjects.DataFetch.GetNPCs().FirstOrDefault(x => x.GetType() == typeof(OtherPlayer) && ((IPlayer)x).Name == FollowPlayerName);

            if(fPlayer != null)
            {
                GameFunctionsAndObjects.Actions.MoveToPoint(new System.Numerics.Vector2(*fPlayer.X/100, *fPlayer.Y/100));
            }
        }
    }
}
