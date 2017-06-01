using System.Runtime.Serialization;

namespace OdoriRails.Helpers.Objects
{
    [DataContract]
    public class ApiTram
    {
<<<<<<< HEAD
        public int TramNumber { get; }
        public int? TrackNumber { get; }
        public int SectorNumber { get; }
        public decimal Latitude { get; }
        public decimal Longitude { get; }
        public TrackType? TrackType { get; }
        public int Line { get; }

        public ApiTram(int tramNumber, int? trackNumber, int sectorNumber, decimal latitude, decimal longitude, TrackType? trackType, int line)
=======
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
>>>>>>> 8764bb2e5edb78063cb92552b0017221a72f5099
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