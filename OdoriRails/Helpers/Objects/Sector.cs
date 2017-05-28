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
        public int Number { get; private set; }
        public SectorStatus Status { get; protected set; }
        public Tram OccupyingTram { get; set; }
        public int TrackNumber { get; private set; }
        public int? TramId { get; }

        /// <summary>
        /// Constructor
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
        /// Minimale sector constuctor
        /// </summary>
        /// <param name="number"></param>
        public Sector(int number)
        {
            Number = number;
        }

        public void SetTram(Tram tram)
        {
            OccupyingTram = tram;
        }
    }
}
