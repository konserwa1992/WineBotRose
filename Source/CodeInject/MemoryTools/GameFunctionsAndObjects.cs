using CodeInject.Actors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeInject.MemoryTools
{
  

    internal class GameFunctionsAndObjects
    {
        public static GameActions Actions { get; private set; } = new GameActions();
        public static DataReader DataFetch { get; private set; } = new DataReader();
        private GameFunctionsAndObjects() { }
    }
}
