using System.Runtime.Serialization;

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
        internal decimal? Latitude { get; set; }
        [DataMember]
        internal decimal? Longitude { get; set; }
        [DataMember]
        internal int? TrackType { get; set; }
        [DataMember]
        internal int Line { get; set; }

        internal ApiTram(int tramNumber, int? trackNumber, int? sectorNumber, decimal? latitude, decimal? longitude, int? trackType, int line)
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