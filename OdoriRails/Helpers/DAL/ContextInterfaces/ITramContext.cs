using System.Collections.Generic;
using System.Data;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.DAL.ContextInterfaces
{
    public interface ITramContext
    {
        /// <summary>
        /// Get een tram via het Id.
        /// </summary>
        /// <param name="tramId"></param>
        /// <returns></returns>
        DataRow GetTram(int tramId);

        /// <summary>
        /// Voegt een nieuwe tram toe aan de database.
        /// </summary>
        /// <param name="tram">Het tram object.</param>
        void AddTram(Tram tram);

        /// <summary>
        /// Verwijder een tram uit de database.
        /// </summary>
        /// <param name="tram">Het tram object.</param>
        void RemoveTram(Tram tram);

        /// <summary>
        /// Een lijst van alle trams.
        /// </summary>
        /// <returns></returns>
        DataTable GetAllTrams();

        /// <summary>
        /// Haalt de tram op van de user die het bestuurt (kan null zijn).
        /// </summary>
        /// <param name="user">De bestuurder.</param>
        /// <returns></returns>
        DataTable GetTramsByDriver(User user);

        /// <summary>
        /// Edit deze tram in de database.
        /// </summary>
        /// <param name="tram"></param>
        void EditTram(Tram tram);

        /// <summary>
        /// Haalt alle trams op met status:
        /// </summary>
        /// <param name="tramStatus">De status</param>
        /// <returns></returns>
        DataTable GetAllTramsWithStatus(TramStatus tramStatus);

        /// <summary>
        /// Haalt alle trams op met locatie:
        /// </summary>
        /// <param name="tramLocation">De locatie.</param>
        /// <returns></returns>
        DataTable GetAllTramsWithLocation(TramLocation tramLocation);

        /// <summary>
        /// Haalt de sector op waar de tram op staat.
        /// </summary>
        /// <param name="tram"></param>
        /// <returns></returns>
        DataRow GetAssignedSector(Tram tram);

        void WipeDepartureTimes();

        DataRow FetchTram(Tram tram);

        bool DoesTramExist(int id);

        void SetUserToTram(int tramId, int? userId);

        DataTable GetTramIdsByDriverId(int driverId);

        void SetStatusToIdle(int tramId);
    }
}
