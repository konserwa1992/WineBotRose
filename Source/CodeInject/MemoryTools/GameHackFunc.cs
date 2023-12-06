

using TEST;

namespace CodeInject.MemoryTools
{
    public class GameHackFunc
    {
        public static DataFetcher ClientData { get; private set; } = new DataFetcher();
        public static GameActions Actions { get; private set; } = new GameActions();
        private GameHackFunc() { }
    }
}
