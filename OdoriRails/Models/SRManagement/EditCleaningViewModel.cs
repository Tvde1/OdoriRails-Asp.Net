using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models.SRManagement
{
    public class EditCleaningViewModel : BaseModel
    {
        public Dictionary<string, bool> AssignedWorkers { get; set; }
        public Cleaning CleaningToChange { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CleaningSize Size { get; set; }
        public string Comment { get; set; }
        public int TramID { get; set; }
        public int Id { get; set; }
    }
}