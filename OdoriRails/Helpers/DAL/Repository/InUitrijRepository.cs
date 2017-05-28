using System.Collections.Generic;
using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.DAL.Contexts;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.DAL.Repository
{
    public class InUitrijRepository : BaseRepository
    {
        private readonly ObjectCreator _objectCreator = new ObjectCreator();
        private readonly IServiceContext _serviceContext = new ServiceContext();
        private readonly ITramContext _tramContext = new TramContext();
        private readonly IUserContext _userContext = new UserContext();

        /// <summary>
        ///     Voegt een Schoonmaak toe en geeft de schoonmaak met ID terug.
        /// </summary>
        /// <param name="cleaning"></param>
        /// <returns></returns>
        public Cleaning AddCleaning(Cleaning cleaning)
        {
            return _serviceContext.AddCleaning(cleaning);
        }

        /// <summary>
        ///     Voegt een Repair toe en geeft de repair met ID terug.
        /// </summary>
        /// <param name="repair"></param>
        /// <returns></returns>
        public Repair AddRepair(Repair repair)
        {
            return _serviceContext.AddRepair(repair);
        }

        /// <summary>
        ///     Get de user ID via de username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetUser(string username)
        {
            return _objectCreator.CreateUser(_userContext.GetUser(username));
        }

        /// <summary>
        ///     Haal de tram op waar deze meneer in rijdt.
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public List<Tram> GetTramByDriver(User driver)
        {
            return ObjectCreator.GenerateListWithFunction(_tramContext.GetTramsByDriver(driver),
                _objectCreator.CreateTram);
        }

        /// <summary>
        ///     Haal de sector op waar deze tram op staat.
        /// </summary>
        /// <param name="tram"></param>
        /// <returns></returns>
        public Sector GetAssignedSector(Tram tram)
        {
            var sector = _objectCreator.CreateSector(_tramContext.GetAssignedSector(tram));
            sector.SetTram(tram);
            return sector;
        }

        /// <summary>
        ///     Edit tram.
        /// </summary>
        /// <param name="tram"></param>
        public void EditTram(Tram tram)
        {
            _tramContext.EditTram(tram);
        }

        public Tram FetchTram(Tram tram)
        {
            return _objectCreator.CreateTram(_tramContext.FetchTram(tram));
        }
    }
}