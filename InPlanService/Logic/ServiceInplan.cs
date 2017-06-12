using OdoriRails.Helpers.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlanService.Logic
{
    class ServiceInplan
    {
        public void Update()
        {
            ServiceSortingAlgoritm algoritm = new ServiceSortingAlgoritm();
            algoritm.PlanServices(15);
        }
    }
}
