using nillauncher.src.server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nillauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            Runtime.loadConfig();
            console_output.actions.Add((ev) =>
            {
                Console.WriteLine(">> "+ev.Data);
            });
            ProcessHelper.start_bds();
            Console.ReadKey();
            Runtime.bds.StandardInput.WriteLine("list");
            Console.ReadKey();
            Runtime.bds.StandardInput.WriteLine("stop");
        }
    }
}
