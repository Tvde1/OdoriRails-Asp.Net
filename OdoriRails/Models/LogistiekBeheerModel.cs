using OdoriRails.Helpers.LogistiekBeheersysteem;
using OdoriRails.Helpers.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdoriRails.Models
{
    public enum LogistiekState
    {
        Main,
        Edit
    }


    public class LogistiekBeheerModel : BaseModel
    {
        public LogistiekState State { get; set; }
        public List<BeheerTrack> Tracks { get; set; }
        public List<BeheerTram> Trams { get; set; }
    }
}