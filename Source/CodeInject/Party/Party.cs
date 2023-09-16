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
            PartyMemberDataAddres = (long*)*(long*)MemoryTools.MemoryTools.GetInt64(GameFunctionsAndObjects.DataFetch.BaseAddres + 0x0121A130, new short[] { 0x0, 0x10, 0x08 });

            int partyMemberCount = *(int*)(GameFunctionsAndObjects.DataFetch.BaseAddres + 0x121A170);

            PartyMemberList = new List<PartyMember>();

            for (int i=0;i<partyMemberCount;i++)
            {
                long* currentMember = (long*)*PartyMemberDataAddres; //selecting member

                PartyMember member = new PartyMember()
                {
                    MemberAddres = (long)currentMember,
                    MemberName = Marshal.PtrToStringAnsi(new IntPtr((long)currentMember + 0x10))
                };

                PartyMemberList.Add(member);
                PartyMemberDataAddres++; //move to next member
            }
        }

        public Party() {

        }

    }
}
