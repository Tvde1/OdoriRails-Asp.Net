using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace InPlanService
{
    class Program
    {
        static LogistiekInPlanServer server;

        static void Main(string[] args)
        {
            server = new LogistiekInPlanServer();
            Timer CheckForChanges = new Timer(2500);
            CheckForChanges.Elapsed += new ElapsedEventHandler(CheckForChanges_Tick);
            CheckForChanges.Enabled = true;
            Console.ReadLine();
        }

        private static void CheckForChanges_Tick(object sender, ElapsedEventArgs e)
        {
            server.FetchUpdates();
        }
    }
}
