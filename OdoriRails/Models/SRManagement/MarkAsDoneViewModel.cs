using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models.SRManagement
{
    public class MarkAsDoneViewModel : BaseModel
    {
        public Cleaning CleaningMarkAsDone { get; set; }
        public Repair RepairMarkAsDone { get; set; }

        public string Comment { get; set; }
        public string Solution { get; set; }
    }
}