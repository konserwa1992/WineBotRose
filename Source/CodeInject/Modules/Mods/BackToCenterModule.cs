using CodeInject.Actors;
using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Modules
{
    unsafe class BackToCenterModule : IModule
    {
        public string Name { get; set; } = "BACKTOCENTER";

        private Vector3 CenterPosition {  get; set; }
        private float Radius { get; set; }
        private List<MobInfo> MonstersToAttackList { get; set; }

        public BackToCenterModule(List<MobInfo> monstersToAttackList ,Vector3 position,float radius)
        {
            CenterPosition = position;  
            MonstersToAttackList = monstersToAttackList;
            Radius = radius;
        }

        public  void Update()
        { 
            if (!GameFunctionsAndObjects.DataFetch.GetNPCs().Where(x => x.GetType() == typeof(NPC))
                .Where(x => MonstersToAttackList.Cast<MobInfo>().Any(y => ((NPC) x).Info != null && y.ID == ((NPC)x).Info.ID))
                .Where(x => ((NPC) x).CalcDistance(CenterPosition.X, CenterPosition.Y, CenterPosition.Z) < Radius).Any(x => *(((NPC) x).Hp) > 0))
            {
                GoToHuntingAreaCenter();
            }
        }

        private void GoToHuntingAreaCenter()
        {
            if (((int)*GameFunctionsAndObjects.DataFetch.GetPlayer().X) != (int)CenterPosition.X &&
                 ((int)*GameFunctionsAndObjects.DataFetch.GetPlayer().Y) != (int)CenterPosition.Y)
            {
                GameFunctionsAndObjects.Actions.MoveToPoint(new Vector2(CenterPosition.X / 100, CenterPosition.Y / 100));
            }
        }


        public override string ToString()
        {
            return "Back To Center";
        }
    }
}
