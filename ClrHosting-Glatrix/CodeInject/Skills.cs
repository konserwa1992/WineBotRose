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

        public Skills(SkillInfo skillInfo)
        {
            this.skillInfo = skillInfo;
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
