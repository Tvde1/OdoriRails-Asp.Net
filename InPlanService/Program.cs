﻿using System;
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
        static Timer CheckForChanges;

        static void Main(string[] args)
        {
            Console.Title = "OdoriRails Scheduler Server";
            Console.WriteLine("© 2017 - OdoriRails BV");
            Console.WriteLine("Press escape to exit.");
            Console.WriteLine();

            server = new LogistiekInPlanServer();
            CheckForChanges = new Timer(5000);
            CheckForChanges.Elapsed += new ElapsedEventHandler(CheckForChanges_Tick);
            CheckForChanges.Enabled = true;

            while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }
            Console.Clear();
            Console.WriteLine("Shutting down...");
        }

        private static void CheckForChanges_Tick(object sender, ElapsedEventArgs e)
        {
            server.FetchMovingTrams();
        }
    }
}
