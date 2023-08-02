using CodeInject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISpace
{
    public unsafe class IClass
    {

        public static int IMain(string args)
        {
            cBot cBot = new cBot();
            cBot.ShowDialog();
  
            return 0;
        }

        [DllImport("kernel32")]
        static extern bool AllocConsole();
    }
}
