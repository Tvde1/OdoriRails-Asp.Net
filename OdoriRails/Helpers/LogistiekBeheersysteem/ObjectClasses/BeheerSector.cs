using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.LogistiekBeheersysteem.ObjectClasses
{
    public class BeheerSector : Sector
    {
        private BeheerSector(int number, int trackNumber, SectorStatus status, Tram tram, string latitude, string longitude) : base(number, trackNumber, status, tram?.Number, latitude, longitude)
        {
            SetTram(tram);
        }

        public static BeheerSector ToBeheerSector(Sector sector)
        {
            return new BeheerSector(sector.Number, sector.TrackNumber, sector.Status, sector.OccupyingTram, sector.Latitude, sector.Longitude);
        }

        /// <summary>
        /// Zet de status van de tram naar locked.
        /// </summary>
        public void Lock()
        {
            Status = SectorStatus.Locked;
        }

        /// <summary>
        /// Zet de status van de tram naar open.
        /// </summary>
        public void UnLock()
        {
            if (OccupyingTram == null)
            {
                Status = SectorStatus.Open;
            }
            else
            {
                Status = SectorStatus.Occupied;
            }
        }

        /// <summary>
        /// Zet de occupying tram. Gebruik `null` om de tram leeg te maken.
        /// </summary>
        /// <param name="tram"></param>
        public bool SetOccupyingTram(Tram tram)
        {
            if (Status == SectorStatus.Locked || Status == SectorStatus.Occupied) return false;
            else
            {
                Status = SectorStatus.Occupied;
                OccupyingTram = tram;
                return true;
            }

        }
    }
}