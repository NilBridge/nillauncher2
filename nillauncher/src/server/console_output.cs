using System;
using System.Collections.Generic;
using System.Diagnostics;
using WebSocketSharp;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebSocketSharp.Server;

namespace nillauncher.src.server
{
    public class console_output
    {
        public static string id = "";
        private static int lines = 0;
        private static string result = "";
        private static bool is_running = false;
        private static string ws_id = "";
        public static void runcmd(string cmd,string id,string ws_id, int line = 1)
        {
            is_running = true;
            Console.WriteLine("开始执行");
            console_output.id = id;
            lines = line;
            console_output.ws_id = ws_id;
            Runtime.bds.StandardInput.WriteLine(cmd);
        }
        public static List<Action<DataReceivedEventArgs>> actions = new List<Action<DataReceivedEventArgs>>();
        public static void on_out_put(object tis,DataReceivedEventArgs arg)
        {
            foreach(var i in actions)
            {
                try
                {
                    i.Invoke(arg);
                }
                catch(Exception err)
                {
                    Console.WriteLine(err);
                }
            }
        }
        public static void setup()
        {
            int num = 0;
            actions.Add((ev) =>
            {
                if (is_running)
                {
                    num += 1;
                    result += ev.Data;
                    if (num == lines)
                    {
                        Program.Server.send(ws_id, JsonConvert.SerializeObject(new
                        {
                            type = "runcmdfeedback",
                            @params = new packs.@params
                            {
                                id = id,
                                result = result
                            }
                        }));
                        is_running = false;
                        lines = 1;
                        result = "";
                        num = 0;
                    }
                }
                else
                {
                    Console.WriteLine(ev.Data);
                }
            });
        }
    }
}
