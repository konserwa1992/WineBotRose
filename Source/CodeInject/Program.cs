using CodeInject;
using CodeInject.MemoryTools;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TEST;

namespace ISpace
{

    public class IClass
    {
        public unsafe static int IMain(string args)
        {
             cBot cBot = new cBot();
              cBot.ShowDialog();


        /*   AllocConsole();
            Console.WriteLine("Injected");
            Console.WriteLine("Injected1");

            MessageBox.Show(GameHackFunc.ClientData.GetPlayer().Name);*/

            return 0;
        }

        [DllImport("kernel32")]
        static extern bool AllocConsole();
    }
}
