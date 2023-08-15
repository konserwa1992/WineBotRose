using CodeInject.WebServ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject
{
    public class Skills
    {
        public SkillInfo skillInfo;

        private Skills()
        {
            this.skillInfo = skillInfo;
        }

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
            return new Skills()
            {
                skillInfo = DataBase.GameDataBase.SkillDatabase.FirstOrDefault(s => s.ID == skillId)
            };
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
