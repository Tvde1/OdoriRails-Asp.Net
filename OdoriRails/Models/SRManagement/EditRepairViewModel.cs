using System;
using System.Collections.Generic;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models.SRManagement
{
    public class EditRepairViewModel : BaseModel
    {
        public Dictionary<string, bool> AssignedWorkers { get; set; }
        public Repair RepairToChange { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public RepairType Size { get; set; }
        public string Defect { get; set; }
        public string Solution { get; set; }
        public int TramID { get; set; }
        public int Id { get; set; }
    }
}