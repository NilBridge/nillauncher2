using nillauncher.src.server;
using nillauncher.src.ws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nillauncher
{
    class Program
    {
        public static server Server;
        static void Main(string[] args)
        {
            Runtime.loadConfig();
            console_output.setup();
            Server = new server($"ws://0.0.0.0:{Runtime.config.ws.port}", Runtime.config.ws.endpoint);
            ProcessHelper.start_bds();
            Console.ReadKey();
            Runtime.bds.StandardInput.WriteLine("list");
            Console.ReadKey();
            Runtime.bds.StandardInput.WriteLine("stop");
        }
    }
}
