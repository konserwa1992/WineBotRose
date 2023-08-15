using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.WebServ.Models
{
    public class PlayerSkillModel
    {
        public List<SkillInfo> UnUsedSkillList { get; set; }   = new List<SkillInfo>();
        public List<SkillInfo> SkillInUseList { get; set; } = new List<SkillInfo>();

    }
}
