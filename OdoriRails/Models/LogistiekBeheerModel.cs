using OdoriRails.Helpers.LogistiekBeheersysteem;
using OdoriRails.Helpers.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdoriRails.Models
{
    public class LogistiekBeheerModel : BaseModel
    {
        public List<BeheerTrack> Tracks { get; set; }
        public List<BeheerTram> Trams { get; set; }
    }
}