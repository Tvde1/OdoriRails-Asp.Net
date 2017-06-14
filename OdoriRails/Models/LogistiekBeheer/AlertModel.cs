using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdoriRails.Models.LogistiekBeheer
{
    public class AlertModel : BaseModel
    {
        public string Message { get; set; }
        public int TramId { get; set; }
        public int TrackId { get; set; }
        public int SectorId { get; set; }
    }
}