using System.Linq;


namespace CodeInject
{
    public class Skills
    {
        public SkillInfo skillInfo;



        public Skills(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
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
            return new Skills(DataBase.GameDataBase.SkillDatabase.FirstOrDefault(s => s.ID == skillId));
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
