using System.Runtime.Serialization;

namespace OdoriRails.Helpers.Objects
{
    public enum SectorStatus
    {
        Open,
        Locked,
        Occupied
    }

    [DataContract]
    public class Sector
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="number"></param>
        /// <param name="trackNumber"></param>
        /// <param name="status"></param>
        /// <param name="tramId"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public Sector(int number, int trackNumber, SectorStatus status, int? tramId, decimal? latitude,
            decimal? longitude)
        {
            Number = number;
            Status = status;
            TrackNumber = trackNumber;
            TramId = tramId;
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        ///     Minimale sector constuctor
        /// </summary>
        /// <param name="number"></param>
        public Sector(int number)
        {
            Number = number;
        }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public SectorStatus Status { get; set; }

        [DataMember]
        public Tram OccupyingTram { get; set; }

        public int TrackNumber { get; set; }
        public int? TramId { get; set; }

        [DataMember]
        public decimal? Latitude { get; set; }

        [DataMember]
        public decimal? Longitude { get; set; }

        public void SetTram(Tram tram)
        {
            OccupyingTram = tram;
        }
    }
}