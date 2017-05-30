using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdoriRails.Models
{
    public class FormResultModel
    {
        public string TramNumber { get; set; }
        public string TramModel { get; set; }
        public string TrackNumber { get; set; }
        public string TrackType { get; set; }
        public string SectorNumber { get; set; }
        public string SectorAmount { get; set; }
        public string DefaultLine { get; set; }
        public int RadioButton { get; set; }
    }
}