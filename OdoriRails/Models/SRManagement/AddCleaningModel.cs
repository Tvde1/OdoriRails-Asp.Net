using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models.SRManagement
{
    public class AddCleaningModel : BaseModel
    {
        public Dictionary<string, bool> AssignedWorkers { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CleaningSize Size { get; set; }
        public string Comment { get; set; }
        public int TramID { get; set; }
    }
}