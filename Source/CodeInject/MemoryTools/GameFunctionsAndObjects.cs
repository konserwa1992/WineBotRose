

namespace CodeInject.MemoryTools
{
  

    internal class GameFunctionsAndObjects
    {
        public static DataReader DataFetch { get; private set; } = new DataReader();
        public static GameActions Actions { get; private set; } = new GameActions();
        private GameFunctionsAndObjects() { }
    }
}
