using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdoriRails.Helpers.Objects
{
    public class ApiTram
    {
        public int TramNumber { get; }
        public int? TrackNumber { get; }
        public int SectorNumber { get; }
        public string Latitude { get; }
        public string Longitude { get; }
        public TrackType? TrackType { get; }
        public int Line { get; }

        public ApiTram(int tramNumber, int? trackNumber, int sectorNumber, string latitude, string longitude, TrackType? trackType, int line)
        {
            TramNumber = tramNumber;
            TrackNumber = trackNumber;;
            Latitude = latitude;
            Longitude = longitude;
            TrackType = trackType;
            Line = line;
            SectorNumber = sectorNumber;
        }
    }
}