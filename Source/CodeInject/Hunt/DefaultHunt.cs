using CodeInject.Actors;
using CodeInject.MemoryTools;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CodeInject.Hunt
{
    public unsafe class DefaultHunt : HuntSetting
    {
        public int SkillIndex = 0;
        public IObject Target;
        public Vector3 HuntingAreaCenter { get; set; }
        public int Radius {  get; set; }
        private cBot WinFormMenu;

        public DefaultHunt(List<MobInfo> monstersToAttackList,Vector3 huntingAreaCenter,int radius,List<Skills> skillList,cBot WinForm)
        {
            HuntingAreaCenter = huntingAreaCenter;
            Radius = radius;
            ListOfMonstersToAttack= monstersToAttackList;
            WinFormMenu = WinForm;
            BotSkills = skillList;
        }

        public override void AddSkill(Skills skill)
        {
            base.AddSkill(skill);
            WinFormMenu.SkillListUpdate();
        }

        public override void RemoveSkill(Skills skill)
        {
            base.RemoveSkill(skill);
            WinFormMenu.SkillListUpdate();
        }

        public override void Update()
        {
            if (this.SkillIndex < this.BotSkills.Count - 1)
            {
                this.SkillIndex++;
            }
            else
            {
                this.SkillIndex = 0;
            }

            if (Target == null || *((NPC)Target).Hp <= 0)
            {
                this.Target = GameFunctionsAndObjects.DataFetch.GetNPCs()
                .Where(x => ListOfMonstersToAttack.Cast<MobInfo>().Any(y => ((NPC)x).Info != null && y.ID == ((NPC)x).Info.ID))
                .Where(x => ((NPC)x).CalcDistance(HuntingAreaCenter.X, HuntingAreaCenter.Y, HuntingAreaCenter.Z) < Radius).FirstOrDefault(x => *(((NPC)x).Hp) > 0);
            }

            if (this.Target != null)
            {
                if (this.BotSkills.Count > 0)
                {
                    Skills Skill2Cast = PlayerCharacter.GetPlayerSkills.FirstOrDefault(x => x.skillInfo.ID == this.BotSkills[this.SkillIndex].skillInfo.ID);
                    GameFunctionsAndObjects.Actions.CastSpell(*this.Target.ID, GetSkillIndex(Skill2Cast.skillInfo.ID));
                }
                GameFunctionsAndObjects.Actions.Attack(*this.Target.ID);
            }
            else
            {
                GoToHuntingAreaCenter();
            }

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
