namespace OdoriRails.Helpers.Objects
{
    public enum SectorStatus
    {
        Open,
        Locked,
        Occupied
    }

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
        public Sector(int number, int trackNumber, SectorStatus status, int? tramId, string latitude, string longitude)
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

        public int Number { get; }
        public SectorStatus Status { get; protected set; }
        public Tram OccupyingTram { get; set; }
        public int TrackNumber { get; }
        public int? TramId { get; }
        public string Latitude { get; }
        public string Longitude { get; }

        public void SetTram(Tram tram)
        {
            OccupyingTram = tram;
        }
    }
}