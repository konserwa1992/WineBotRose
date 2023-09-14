

namespace CodeInject.MemoryTools
{
  

    internal class GameFunctionsAndObjects
    {
        public static dataReadr DataFetch { get; private set; } = new dataReadr();
        public static GameActions Actions { get; private set; } = new GameActions();
        private GameFunctionsAndObjects() { }
    }
}
