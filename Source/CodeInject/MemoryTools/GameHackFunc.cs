

using TEST;

namespace CodeInject.MemoryTools
{
    public class GameHackFunc
    {
        public static GameHackFunc Game { get;private set; } = new GameHackFunc();
        public  DataFetcher ClientData { get;  set; } = new DataFetcher();
        public  GameActions Actions { get;  set; } = new GameActions();
        private GameHackFunc() { }
    }
}
