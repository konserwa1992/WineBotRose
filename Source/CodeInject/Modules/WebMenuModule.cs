using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Modules
{
    [Serializable]
    public class WebMenuModule : IModule
    {
        WebServer ws;

        public string Name { get; set; } = "WebMenu";

        public WebMenuModule()
        {
            ws= new WebServer();
            ws.SetupWebSocketServer();
        }

        public void update()
        {
            ws.SendPlayerInformation();
            ws.SendNPCsInformation();
        }
    }
}
