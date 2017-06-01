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
            Timer CheckForChanges = new Timer(5000);
            CheckForChanges.Elapsed += new ElapsedEventHandler(CheckForChanges_Tick);
            CheckForChanges.Enabled = true;

            Console.Title = "OdoriRails Scheduler Server";
            Console.WriteLine("© 2017 - OdoriRails BV");
            Console.WriteLine("Press escape to exit.");
            Console.WriteLine();

            while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }
            Console.Clear();
            Console.WriteLine("Closing...");
        }

        private static void CheckForChanges_Tick(object sender, ElapsedEventArgs e)
        {
            string tramsComingIn = server.FetchTramsComingIn();
            string tramsGoingOut = server.FetchTramsGoingOut();
            if (!String.IsNullOrEmpty(tramsComingIn))
                Console.WriteLine(tramsComingIn);
            if (!String.IsNullOrEmpty(tramsGoingOut))
                Console.WriteLine(tramsGoingOut);
        }
    }
}
