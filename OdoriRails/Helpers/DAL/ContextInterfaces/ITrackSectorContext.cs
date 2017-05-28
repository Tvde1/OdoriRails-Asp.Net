using System.Data;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.DAL.ContextInterfaces
{
    public interface ITrackSectorContext
    {
        DataTable GetAllTracks();

        DataTable GetAllSectors();

        void AddTrack(Track track);

        void AddSector(Sector sector, Track track);

        /// <summary>
        ///     Updated de track in de database met de nieuwe informatie.
        /// </summary>
        void EditTrack(Track track);

        /// <summary>
        ///     Updated de sector in de database met de nieuwe informatie.
        /// </summary>
        void EditSector(Sector sector);

        void DeleteSectorFromTrack(Track track, Sector sector);

        void DeleteTrack(Track track);

        void WipeTramsFromSectors();

        void WipeTramFromSectorByTramId(int id);
    }
}