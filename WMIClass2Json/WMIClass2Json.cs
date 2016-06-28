using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMIClass2Json
{
    class WMIClass2Json
    {
        static void Main(string[] args)
        {
            if (args.Count() > 0)
            {
                try
                {
                    WMIClass cs = new WMIClass(Environment.MachineName, args[0]);
                    Console.WriteLine(cs.Serialize());
                    Environment.Exit(0);
                }

                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }

            }
            else
            {
                Console.WriteLine("USE: WMIClass2Json.exe WMIClassName");
                Environment.Exit(1);
            }
        }
    }
}
