using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Party
{
    public class PartyMember
    {
        public long MemberAddres { get; set; }
        public string MemberName { get; set;}

        public override string ToString()
        {
            return MemberName;
        }
    }
}
