using System.Collections.Generic;

namespace OdoriRails.Helpers.Objects
{
    public enum TrackType
    {
        Normal,
        Service,
        Exit
    }

    public class Track
    {
        /// <summary>
        ///     Voledige constructor.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="line"></param>
        /// <param name="type"></param>
        public Track(int number, int? line, TrackType type, List<Sector> sectors)
        {
            Number = number;
            Line = line;
            Type = type;
            Sectors = sectors;
        }

        public Track(int number, int? line, TrackType type)
        {
            Number = number;
            Line = line;
            Type = type;
        }

        public List<Sector> Sectors { get; } = new List<Sector>();

        public int Number { get; }
        public int? Line { get; }

        public TrackType Type { get; }

        /// <summary>
        ///     Voegt een nieuwe sector toe aan het track.
        /// </summary>
        /// <param name="sector"></param>
        public void AddSector(Sector sector)
        {
            Sectors.Add(sector);
        }

        public void DeleteSector()
        {
            Sectors.RemoveAt(Sectors.Count - 1);
        }
    }
}