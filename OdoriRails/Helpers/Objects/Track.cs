using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OdoriRails.Helpers.Objects
{
    public enum TrackType
    {
        Normal,
        Service,
        Exit
    }

    [DataContract]
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

        [DataMember]
        public List<Sector> Sectors { get; set; } = new List<Sector>();

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public int? Line { get; set; }

        [DataMember]
        public TrackType Type { get; set; }

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