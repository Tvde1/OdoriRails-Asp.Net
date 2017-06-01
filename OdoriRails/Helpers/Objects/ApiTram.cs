using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace OdoriRails.Helpers.Objects
{
    [DataContract]
    public class ApiTram
    {
        [DataMember]
        internal int TramNumber { get; set; }
        [DataMember]
        internal int? TrackNumber { get; set; }
        [DataMember]
        internal int? SectorNumber { get; set; }
        [DataMember]
        internal string Latitude { get; set; }
        [DataMember]
        internal string Longitude { get; set; }
        [DataMember]
        internal int? TrackType { get; set; }
        [DataMember]
        internal int Line { get; set; }

        internal ApiTram(int tramNumber, int? trackNumber, int? sectorNumber, string latitude, string longitude, int? trackType, int line)
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