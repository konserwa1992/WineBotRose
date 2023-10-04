using CodeInject;

using System.Runtime.InteropServices;


namespace ISpace
{

    public class IClass
    {
        public static int IMain(string args)
        {

                 cBot cBot = new cBot();
                 cBot.ShowDialog();
           // AllocConsole();
            return 0;
        }

        [DllImport("kernel32")]
        static extern bool AllocConsole();
    }
}
