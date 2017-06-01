using System.Collections.Generic;
using OdoriRails.Helpers.LogistiekBeheersysteem;

namespace OdoriRails.Models.LogistiekBeheer
{
    public enum LogistiekState
    {
        Main,
        Edit,
        Delete
    }


    public class LogistiekBeheerModel : BaseModel
    {
        public LogistiekState State { get; set; }
        public List<BeheerTrack> Tracks { get; set; }
        public List<BeheerTram> Trams { get; set; }
        public FormResultModel Form { get; set; } = new FormResultModel();
    }
}