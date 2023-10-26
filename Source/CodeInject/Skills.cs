using System.Linq;


namespace CodeInject
{
    public enum SkillTypes
    {
        HealTarget,Buff,AttackSkill,Unknow
    }

    public class Skills
    {
        public SkillInfo skillInfo;
        public SkillTypes SkillType;


        public Skills(SkillInfo skillInfo,SkillTypes type)
        {
            this.skillInfo = skillInfo;
            this.SkillType = type;
        }

        public SkillInfo ToWSObject()
        {
            return new SkillInfo()
            {
                ID = skillInfo.ID,
                Name = skillInfo.Name
            };
        }


        public static Skills GetSkillByID(int skillId)
        {
            return new Skills(DataBase.GameDataBase.SkillDatabase.FirstOrDefault(s => s.ID == skillId),SkillTypes.Unknow);
        }

        public override string ToString()
        {

            if (skillInfo != null)
            {
                return skillInfo.ID+ " "+ skillInfo.Name;
            }
            else
            {
               return skillInfo.ID + " "+"Unknow";
            }
        }
    }
}
