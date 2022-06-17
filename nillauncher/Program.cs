using nillauncher.src.server;
using nillauncher.src.ws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using System.Threading;

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
            new Thread(() =>
            {
                while (true)
                {
                    string cmd = Console.ReadLine();
                    if (Runtime.bds.HasExited)
                    {
                        switch (cmd)
                        {
                            case "start":
                                ProcessHelper.start_bds();
                                break;
                        }
                    }
                    else
                    {
                        try
                        {
                            Runtime.bds.StandardInput.WriteLine(cmd);
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine(err);
                        }
                    }
                }
            }).Start();
        }
    }
}
