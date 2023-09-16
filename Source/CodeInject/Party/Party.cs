using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
