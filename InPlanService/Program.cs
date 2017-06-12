using InPlanService.Logic;
using System;
using System.Timers;

namespace InPlanService
{
    class Program
    {
        static LogistiekInPlan logServer;
        static ServiceInplan serServer;
        static Timer CheckForChanges;

        static void Main(string[] args)
        {
            Console.Title = "OdoriRails Scheduler Server";
            Console.WriteLine("© 2017 - OdoriRails BV");
            Console.WriteLine("Press escape to exit.");
            Console.WriteLine();

            logServer = new LogistiekInPlan();
            serServer = new ServiceInplan();
            CheckForChanges = new Timer(5000);
            CheckForChanges.Elapsed += new ElapsedEventHandler(CheckForChanges_Tick);
            CheckForChanges.Enabled = true;

            serServer.Update();

            while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }

            Console.Clear();
            Console.WriteLine("Shutting down...");
        }

        private static void CheckForChanges_Tick(object sender, ElapsedEventArgs e)
        {
            logServer.FetchMovingTrams();
        }
    }
}
