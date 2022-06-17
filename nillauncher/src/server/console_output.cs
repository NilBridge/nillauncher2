using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nillauncher.src.server
{
    public class console_output
    {
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
    }
}
