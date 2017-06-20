using System.Collections.Generic;
using OdoriRails.Helpers.LogistiekBeheersysteem.ObjectClasses;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.LogistiekBeheersysteem
{
    public class BeheerTrack : Track
    {
        public List<BeheerSector> BeheerSectors = new List<BeheerSector>();

        public BeheerTrack(int number, int? line, TrackType type, List<Sector> sectors) : base(number, line, type,
            sectors)
        {
        }

        public static BeheerTrack ToBeheerTrack(Track track)
        {
            return new BeheerTrack(track.Number, track.Line, track.Type, track.Sectors);
        }

        /// <summary>
        ///     Zet elke sectoren's status op 'Locked'
        /// </summary>
        public void LockTrack()
        {
            for (var i = 0; i < Sectors.Count; i++)
            {
                var beheerSector = Sectors[i] == null ? null : BeheerSector.ToBeheerSector(Sectors[i]);
                beheerSector.Lock();
                Sectors[i] = beheerSector;
            }
        }

        /// <summary>
        ///     Zet alle sectoren's status op 'Open'.
        /// </summary>
        public void UnlockTrack()
        {
            for (var i = 0; i < Sectors.Count; i++)
            {
                var beheerSector = Sectors[i] == null ? null : BeheerSector.ToBeheerSector(Sectors[i]);
                beheerSector.UnLock();
                Sectors[i] = beheerSector;
            }
        }
    }
}