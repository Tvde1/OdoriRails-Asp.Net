using System.Collections.Generic;
using System.Linq;
using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.DAL.Contexts;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.DAL.Repository
{
    public class LogisticRepository : BaseRepository
    {
        private readonly ObjectCreator _objectCreator = new ObjectCreator();
        private readonly IServiceContext _serviceContext = new ServiceContext();
        private readonly ITrackSectorContext _trackSectorContext = new TrackSectorContext();
        private readonly ITramContext _tramContext = new TramContext();
        private readonly IUserContext _userContext = new UserContext();

        /// <summary>
        ///     Voegt een nieuwe tram toe aan de database.
        /// </summary>
        /// <param name="tram"></param>
        public void AddTram(Tram tram)
        {
            _tramContext.AddTram(tram);
        }

        /// <summary>
        ///     Verwijdert een Tram uit de database.
        /// </summary>
        /// <param name="tram"></param>
        public void RemoveTram(Tram tram)
        {
            _tramContext.RemoveTram(tram);
        }

        /// <summary>
        ///     Haal een Tram op aan de hand van de tramid.
        /// </summary>
        /// <param name="id"></param>
        public Tram GetTram(int id)
        {
            return _objectCreator.CreateTram(_tramContext.GetTram(id));
        }

        /// <summary>
        ///     Haal alle trams op.
        /// </summary>
        public List<Tram> GetAllTrams()
        {
            return ObjectCreator.GenerateListWithFunction(_tramContext.GetAllTrams(), _objectCreator.CreateTram);
        }

        /// <summary>
        ///     Haal de tram op waar deze meneer in rijdt.
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public int? GetTramByDriver(User driver)
        {
            var data = _tramContext.GetTramByDriver(driver);
            return (int?)data?["TramPk"];
        }

        /// <summary>
        ///     Gets alle trams met een bepaalde status.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<Tram> GetAllTramsWithStatus(TramStatus status)
        {
            return ObjectCreator.GenerateListWithFunction(_tramContext.GetAllTramsWithStatus(status),
                _objectCreator.CreateTram);
        }

        /// <summary>
        ///     Haal trams op met locatie.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public List<Tram> GetAllTramsWithLocation(TramLocation location)
        {
            return ObjectCreator.GenerateListWithFunction(_tramContext.GetAllTramsWithLocation(location),
                _objectCreator.CreateTram);
        }

        /// <summary>
        ///     Haalt alle tracks, sectoren en trams op sectoren op.
        /// </summary>
        /// <returns></returns>
        public List<Track> GetTracksAndSectors()
        {
            var tracks = ObjectCreator
                .GenerateListWithFunction(_trackSectorContext.GetAllTracks(), ObjectCreator.CreateTrack)
                .ToDictionary(x => x.Number, x => x);
            var sectors =
                ObjectCreator.GenerateListWithFunction(_trackSectorContext.GetAllSectors(),
                    _objectCreator.CreateSector);
            var trams = ObjectCreator.GenerateListWithFunction(_tramContext.GetAllTrams(), _objectCreator.CreateTram)
                .ToDictionary(x => x.Number, x => x);


            foreach (var sector in sectors)
            {
                if (sector.TramId == null) continue;
                sector.OccupyingTram = trams[(int)sector.TramId];
                tracks[sector.TrackNumber].AddSector(sector);
            }

            return tracks.Select(x => x.Value).ToList();
        }

        /// <summary>
        ///     Haal een User op aan de hand van de username.
        /// </summary>
        /// <param name="userName"></param>
        public User GetUser(string userName)
        {
            return _objectCreator.CreateUser(_userContext.GetUser(userName));
        }

        public void WipeAllDepartureTimes()
        {
            _tramContext.WipeDepartureTimes();
        }

        public void WipeAllTramsFromSectors()
        {
            _trackSectorContext.WipeTramsFromSectors();
        }

        public void WipeSectorByTramId(int id)
        {
            _trackSectorContext.WipeTramFromSectorByTramId(id);
        }

        public void EditTram(Tram tram)
        {
            _tramContext.EditTram(tram);
        }

        public void AddTrack(Track track)
        {
            _trackSectorContext.AddTrack(track);
        }

        public void AddSector(Sector sector, Track track)
        {
            _trackSectorContext.AddSector(sector, track);
        }

        public void EditSector(Sector sector)
        {
            _trackSectorContext.EditSector(sector);
        }

        public void DeleteSectorFromTrack(Track track, Sector sector)
        {
            _trackSectorContext.DeleteSectorFromTrack(track, sector);
        }

        public void EditTrack(Track track)
        {
            _trackSectorContext.EditTrack(track);
        }

        public void DeleteTrack(Track track)
        {
            _trackSectorContext.DeleteTrack(track);
        }

        public bool HadBigMaintenance(Tram tram)
        {
            return _serviceContext.HadBigMaintenance(tram);
        }

        public bool HadSmallMaintenance(Tram tram)
        {
            return _serviceContext.HadSmallMaintenance(tram);
        }

        public Repair AddRepair(Repair repair)
        {
            return _serviceContext.AddRepair(repair);
        }

        public Cleaning AddCleaning(Cleaning cleaning)
        {
            return _serviceContext.AddCleaning(cleaning);
        }

        public void DeleteService(Service service)
        {
            _serviceContext.DeleteService(service);
        }

        public List<Repair> GetAllRepairsFromUser(User user)
        {
            return ObjectCreator.GenerateListWithFunction(_serviceContext.GetAllRepairsFromUser(user),
                _objectCreator.CreateRepair);
        }
    }
}