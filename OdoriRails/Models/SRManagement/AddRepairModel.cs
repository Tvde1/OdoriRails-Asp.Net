using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models.SRManagement
{
    public class AddRepairModel : BaseModel
    {
        public Dictionary<string, bool> AssignedWorkers { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RepairType Size { get; set; }
        public string Defect { get; set; }
        public string Solution { get; set; }
        public int TramID { get; set; }
    }
}