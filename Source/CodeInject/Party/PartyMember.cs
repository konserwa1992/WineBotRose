using CodeInject.Actors;
using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Party
{
    public unsafe class PartyMember
    {
        public long MemberAddres { get; set; }

        /// <summary>
        /// Everything what got NPC definition
        /// </summary>
        public IObject Details { get; set; }
        public string MemberName { get; set;}

 

        public override string ToString()
        {
            NPC npce = (NPC)GameFunctionsAndObjects.DataFetch.GetPartyMemberDetails(this);
            return *(((NPC)Details).Hp) +" "+ MemberName;
        }
    }
}
