using System;
using System.Timers;
using InPlanService.Logic;

namespace InPlanService
{
    internal class Program
    {
        private static LogistiekInPlan logServer;
        private static Timer CheckForChanges;

        private static void Main(string[] args)
        {
            Console.Title = "OdoriRails Scheduler Server";
            Console.WriteLine("© 2017 - OdoriRails BV");
            Console.WriteLine("Press escape to exit.");
            Console.WriteLine();

            logServer = new LogistiekInPlan();
            CheckForChanges = new Timer(5000);
            CheckForChanges.Elapsed += CheckForChanges_Tick;
            CheckForChanges.Enabled = true;

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
            }

            Console.Clear();
            Console.WriteLine("Shutting down...");
        }

        private static void CheckForChanges_Tick(object sender, ElapsedEventArgs e)
        {
            logServer.FetchMovingTrams();
        }
    }
}