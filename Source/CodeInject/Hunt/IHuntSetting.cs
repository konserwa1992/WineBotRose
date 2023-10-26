using CodeInject.Actors;
using System.Collections.Generic;

namespace CodeInject.Hunt
{
    public interface IHuntSetting
    {
        List<Skills> BotSkills { get; set; }
        List<Skills> BotBuffs { get; set; }

        List<MobInfo> ListOfMonstersToAttack { get; set; }
        IObject Target { get; set; }

        void Update();
        int GetSkillIndex(int SkillID);
        void AddSkill(Skills skill,SkillTypes type);
        void RemoveSkill(Skills skill);
        void AddBuffsSkill(Skills skill);
        void RemoveBuffsSkill(Skills skill);
 
    }
}
