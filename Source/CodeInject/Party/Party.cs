using CodeInject.MemoryTools;
using System.Collections.Generic;

namespace CodeInject.Party
{
    public unsafe class Party
    {
        public List<PartyMember> PartyMemberList { get; set; }
        private long* PartyMemberDataAddres;

        public void Update()
        {
            PartyMemberList = GameFunctionsAndObjects.DataFetch.GetPartyMembersList();
        }

        public Party() {

        }

    }
}
