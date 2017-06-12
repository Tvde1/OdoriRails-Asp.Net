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
            List<Tram> trams = _logRepo.GetAllTrams();
            List<User> emptyUserList = new List<User>();

            for (var d = DateTime.Today; d <= DateTime.Today.AddDays(daysToEndDate); d = d.AddDays(1)) // iterate through days until enddate
            {
                foreach (var tram in trams)
                {
                    if (!_logRepo.HadBigMaintenance(tram)) // check for big service in next 7 days
                    {
                        // plan service and leave loop
                        Repair rep = new Repair(d, null, RepairType.Maintenance, "Big Planned Maintenance", "", emptyUserList, tram.Number);
                        _SRRepo.AddRepair(rep);
                        break;
                    }
                }

                for (int i = 0; i <= 3;) // checks three times for small services 
                {
                    foreach (var tram in trams)
                    {
                        if (!_logRepo.HadSmallMaintenance(tram)) // check for small service in 3 months
                        {
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
