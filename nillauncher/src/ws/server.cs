using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace nillauncher.src.ws
{
    public class serve : WebSocketBehavior
    {

    }
    public class server
    {
        private WebSocketServer ws;
        server(string url, string endpoint)
        {
            ws = new WebSocketServer(url);
            ws.Start();
            ws.AddWebSocketService<serve>(endpoint);
        }
    }
}
