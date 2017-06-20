using System.Runtime.Serialization;

namespace OdoriRails.Helpers.Objects
{
    [DataContract]
    public class ApiObject
    {
        internal ApiObject(Track track, Sector sector, Tram tram)
        {
            TrackType = (int) track.Type;
            TrackNumber = track.Number;
            SectorNumber = sector.Number;
            Latitude = sector.Latitude;
            Longitude = sector.Longitude;

            if (tram != null)
                Tram = new ApiTram(tram.Number, tram.Line);
        }

        [DataMember]
        internal int TrackType { get; }

        [DataMember]
        internal int TrackNumber { get; }

        [DataMember]
        internal int SectorNumber { get; }

        [DataMember]
        internal decimal? Latitude { get; }

        [DataMember]
        internal decimal? Longitude { get; }

        [DataMember]
        internal ApiTram Tram { get; }

        [DataContract]
        internal class ApiTram
        {
            public ApiTram(int tramNumber, int line)
            {
                TramNumber = tramNumber;
                Line = line;
            }

            [DataMember]
            internal int TramNumber { get; }

            [DataMember]
            internal int Line { get; }
        }
    }
}