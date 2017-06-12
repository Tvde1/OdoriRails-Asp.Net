using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InPlanService.Logic
{
    public class ServiceSortingAlgoritm
    {
        private readonly SchoonmaakReparatieRepository _SRRepo = new SchoonmaakReparatieRepository();
        private readonly LogisticRepository _logRepo = new LogisticRepository();

        public void PlanServices(int daysToEndDate)
        {
            Console.WriteLine("Service Start");
            List<Tram> trams = _logRepo.GetAllTrams();
            List<User> emptyUserList = new List<User>();

            for (var d = DateTime.Today; d <= DateTime.Today.AddDays(daysToEndDate); d = d.AddDays(1)) // iterate through days until enddate
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Day: " + d);
                foreach (var tram in trams)
                {
                    Console.Write(tram.Number + "-");
                    if (!_logRepo.HadBigMaintenance(tram)) // check for big service in next 7 days
                    {
                        Console.WriteLine();
                        Console.WriteLine("Big Maintenance Ingepland");
                        // plan service and leave loop
                        Repair rep = new Repair(d, null, RepairType.Maintenance, "Big Planned Maintenance", "", emptyUserList, tram.Number);
                        _SRRepo.AddRepair(rep);
                        break;
                    }
                }

                for (int i = 0; i <= 3;) // checks three times for small services 
                {
                    Console.WriteLine();
                    Console.WriteLine("Check Small Service: " + i);
                    foreach (var tram in trams)
                    {
                        Console.Write(tram.Number + "-");
                        if (!_logRepo.HadSmallMaintenance(tram)) // check for small service in 3 months
                        {
                            Console.WriteLine();
                            Console.WriteLine("Small Maintenance Ingepland");
                            Repair rep = new Repair(d, null, RepairType.Maintenance, "Small Planned Maintenance", "", emptyUserList, tram.Number);
                            _SRRepo.AddRepair(rep);
                            i++;
                            break;
                        }
                    }
                }
            }
        }
    }
}
