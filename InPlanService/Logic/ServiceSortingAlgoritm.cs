using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;
using OdoriRails.Helpers.LogistiekBeheersysteem;
using OdoriRails.Helpers.LogistiekBeheersysteem.ObjectClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlanService.Logic
{
    public class ServiceSortingAlgoritm
    {
        private readonly SchoonmaakReparatieRepository _SRRepo;
        private LogisticRepository _logRepo = new LogisticRepository();

        public void PlanServices(int daysToEndDate)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(daysToEndDate);

            List<Tram> trams = _logRepo.GetAllTrams();
            List<User> emptyUserList = new List<User>();

            for (var d = startDate; d <= endDate; d = d.AddDays(1)) // iterate through the next 15 days
            {
                foreach (var tram in trams)
                {
                    if (!_logRepo.HadBigMaintenance(tram) && tram.Number != 1) // check for big service in next 7 days
                    {
                        // no : plan service and leave loop
                        Repair rep = new Repair(d, null, RepairType.Maintenance, "Big Planned Maintenance", "", emptyUserList, tram.Number);
                        _SRRepo.AddRepair(rep);
                        break;
                    }
                }

                for (int i = 0; i <= 3;) // checks three times for small services, 
                {
                    foreach (var tram in trams)
                    {
                        if (!_logRepo.HadSmallMaintenance(tram) && tram.Number != 1) // check for small service in 3 months
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
