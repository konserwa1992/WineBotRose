using CodeInject.Actors;
using CodeInject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeInject.Hunt
{
    [Serializable]
    public abstract class EmptyHuntSetting : ModuleConteiner,IHuntSetting
    {
        public List<Skills> BotSkills { get; set; } = new List<Skills>();
        public List<MobInfo> ListOfMonstersToAttack { get; set; } = new List<MobInfo> { new MobInfo() };
        public List<Skills> BotBuffs { get; set; } = new List<Skills>();
        public IObject Target { get; set; }
        public bool NormalAttack { get; set; } = true;

        public virtual void Update()
        {
            base.ModuleExecute();
        }
        public int GetSkillIndex(int SkillID)
        {
            return PlayerCharacter.GetPlayerSkills.FindIndex(x => x.skillInfo.ID == SkillID);
        }
        public virtual void AddSkill(Skills skill, SkillTypes type)
        {
            if (!BotSkills.Any(x => x.skillInfo.ID == skill.skillInfo.ID))
            {
                skill.SkillType = type;
                BotSkills.Add(skill);
            }
        }
        public virtual void RemoveSkill(Skills skill)
        {
            Skills skillsToDelete = BotSkills.FirstOrDefault(x => x.skillInfo.ID == skill.skillInfo.ID);
            if (skillsToDelete != null) {
                BotSkills.Remove(skillsToDelete);
            }
        }
        public void AddBuffsSkill(Skills skill)
        {
            if (!BotBuffs.Any(x => x.skillInfo.ID == skill.skillInfo.ID))
            {
                BotBuffs.Add(skill);
            }
        }
        public void RemoveBuffsSkill(Skills skill)
        {
            Skills skillsToDelete = BotBuffs.FirstOrDefault(x => x.skillInfo.ID == skill.skillInfo.ID);
            if (skillsToDelete != null)
            {
                BotBuffs.Remove(skillsToDelete);
            }
        }
    }
}
