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
            PartyMemberList = GameHackFunc.Game.ClientData.GetPartyMembersList();
        }

        public Party() {

        }

    }
}
