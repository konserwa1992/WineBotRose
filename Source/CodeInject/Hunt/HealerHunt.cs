using CodeInject.Actors;
using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public HealerHunt(List<MobInfo> monstersToAttackList, Vector3 huntingAreaCenter, int radius, List<Skills> skillList, List<IObject> players2Heal, int healProc,bool normalAttack, cBot WinForm):base(monstersToAttackList, huntingAreaCenter, radius, skillList, normalAttack, WinForm)
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
               IPlayer currentPlayerObj2Heal = (IPlayer)GameHackFunc.Game.ClientData.GetNPCs().Where(x => (typeof(Player) == x.GetType() || typeof(OtherPlayer) == x.GetType()) && Players2HealList.Contains(((IPlayer)x).Name))
                       .OrderBy(x => (((float)((IPlayer)x).Hp / (float)((IPlayer)x).MaxHp) * 100.0f))
                       .FirstOrDefault();



                    if (currentPlayerObj2Heal != null)
                    {

                      Skills reviveSkill = BotSkills.FirstOrDefault(x => x.SkillType == SkillTypes.Revive);
                        if (reviveSkill !=null && currentPlayerObj2Heal.Hp < 0)
                        {
                            GameHackFunc.Game.Actions.CastSpell((IObject)currentPlayerObj2Heal, reviveSkill.SkillIndex);
                        }
                        else
                        {

                            float currhp = (float)currentPlayerObj2Heal.Hp;
                            float maxhp = (float)currentPlayerObj2Heal.MaxHp;

                            if (((currhp / maxhp) * 100.0f) < ProcHeal)
                            {
                                GameHackFunc.Game.Actions.CastSpell((IObject)currentPlayerObj2Heal, BotSkills.FirstOrDefault(x => x.SkillType == SkillTypes.HealTarget).SkillIndex);
                            }
                        }
                    }
            }

            base.Update();
        }

        private void GoToHuntingAreaCenter()
        {
            if (((int)GameHackFunc.Game.ClientData.GetPlayer().X) != (int)HuntingAreaCenter.X &&
                 ((int)GameHackFunc.Game.ClientData.GetPlayer().Y) != (int)HuntingAreaCenter.Y)
            {
                GameHackFunc.Game.Actions.MoveToPoint(new Vector2(HuntingAreaCenter.X / 100, HuntingAreaCenter.Y / 100));
            }
        }
    }
}
