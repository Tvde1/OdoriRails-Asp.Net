using System.Collections.Generic;
using System.Linq;
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
        public LogistiekBeheerModel()
        {
            var logic = new LogistiekLogic();

            Tracks = logic.AllTracks.Values.ToList();
            Trams = logic.AllTrams.Values.ToList();
        }

        public LogistiekState State { get; set; }
        public List<BeheerTrack> Tracks { get; set; }
        public List<BeheerTram> Trams { get; set; }
        public FormResultModel Form { get; set; } = new FormResultModel();
    }
}