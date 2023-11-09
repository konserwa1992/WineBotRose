using CodeInject.Actors;
using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Hunt
{
    [Serializable]
    internal unsafe class HealerHunt : DefaultHunt
    {

        public int SkillIndex = 0;
        public IObject Target;
        public Vector3 HuntingAreaCenter { get; set; }
        public int Radius { get; set; } = 50;
        private cBot WinFormMenu;
        public List<string> Players2HealList = new List<string>();
        public int ProcHeal = 0;

        public HealerHunt()
        {
        }

        public HealerHunt(List<MobInfo> monstersToAttackList, Vector3 huntingAreaCenter, int radius, List<Skills> skillList, List<IObject> players2Heal, int healProc, cBot WinForm):base(monstersToAttackList, huntingAreaCenter, radius, skillList, WinForm)
        {
            HuntingAreaCenter = huntingAreaCenter;
            Radius = radius;
            ListOfMonstersToAttack = monstersToAttackList;
            WinFormMenu = WinForm;
            Target = null;
            Players2HealList = new List<string>();
            ProcHeal = healProc;

            foreach (IObject player in players2Heal)
            {
                IPlayer curPlayer = (IPlayer)player;
                Players2HealList.Add(curPlayer.Name);
            }
        }



        public override void Update()
        {
            if (Players2HealList.Count > 0)
            {
                foreach (string nick in Players2HealList)
                {
                    IPlayer currentPlayerObj2Heal = (IPlayer)GameFunctionsAndObjects.DataFetch.GetNPCs().Where(x => (typeof(Player) == x.GetType() || typeof(OtherPlayer) == x.GetType()) && ((IPlayer)x).Name == nick)
                        .OrderByDescending(x => (*((IPlayer)x).Hp / *((IPlayer)x).MaxHp) * 100.0f)
                        .FirstOrDefault();

                    if (currentPlayerObj2Heal != null) 
                    {
                        float currhp = (float)*currentPlayerObj2Heal.Hp;
                        float maxhp = (float)*currentPlayerObj2Heal.MaxHp;
                        if (((currhp / maxhp) * 100.0f) < ProcHeal)
                        {
                            GameFunctionsAndObjects.Actions.CastSpell(*currentPlayerObj2Heal.ID, GetSkillIndex(BotSkills.FirstOrDefault(x => x.SkillType == SkillTypes.HealTarget).skillInfo.ID));
                        }
                    }
                }
            }

            base.Update();
        }

        private void GoToHuntingAreaCenter()
        {
            if (((int)*GameFunctionsAndObjects.DataFetch.GetPlayer().X) != (int)HuntingAreaCenter.X &&
                 ((int)*GameFunctionsAndObjects.DataFetch.GetPlayer().Y) != (int)HuntingAreaCenter.Y)
            {
                GameFunctionsAndObjects.Actions.MoveToPoint(new Vector2(HuntingAreaCenter.X / 100, HuntingAreaCenter.Y / 100));
            }
        }
    }
}
