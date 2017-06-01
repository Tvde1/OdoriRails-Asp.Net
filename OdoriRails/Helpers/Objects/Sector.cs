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
        /// <param name="tram"></param>
        public Sector(int number, int trackNumber, SectorStatus status, int? tramId)
        {
            Number = number;
            Status = status;
            TrackNumber = trackNumber;
            TramId = tramId;
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
        public string latitude { get; protected set; }
        public string longitude { get; protected set; }

        public void SetTram(Tram tram)
        {
            OccupyingTram = tram;
        }
    }
}