using System.Collections.Generic;
using OdoriRails.BaseClasses;
using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.DAL.Contexts;

namespace OdoriRails.Helpers.DAL.Repository
{
    public class InUitrijRepository
    {
        public InUitrijRepository(DatabaseHandler databaseHandler)
        {
            _userContext = new UserContext(databaseHandler);
            _serviceContext = new ServiceContext(databaseHandler);
            _tramContext = new TramContext(databaseHandler);
            _objectCreator = new ObjectCreator(databaseHandler);
        }

        private readonly IUserContext _userContext;
        private readonly IServiceContext _serviceContext;
        private readonly ITramContext _tramContext;
        private readonly ObjectCreator _objectCreator;

        /// <summary>
        /// Voegt een Schoonmaak toe en geeft de schoonmaak met ID terug.
        /// </summary>
        /// <param name="cleaning"></param>
        /// <returns></returns>
        public Cleaning AddCleaning(Cleaning cleaning)
        {
            return _serviceContext.AddCleaning(cleaning);
        }

        /// <summary>
        /// Voegt een Repair toe en geeft de repair met ID terug.
        /// </summary>
        /// <param name="repair"></param>
        /// <returns></returns>
        public Repair AddRepair(Repair repair)
        {
            return _serviceContext.AddRepair(repair);
        }

        /// <summary>
        /// Get de user ID via de username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetUser(string username)
        {
            return _objectCreator.CreateUser(_userContext.GetUser(username));
        }

        /// <summary>
        /// Haal de tram op waar deze meneer in rijdt.
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public List<Tram> GetTramByDriver(User driver)
        {
            return ObjectCreator.GenerateListWithFunction(_tramContext.GetTramsByDriver(driver), _objectCreator.CreateTram);
        }

        /// <summary>
        /// Haal de sector op waar deze tram op staat.
        /// </summary>
        /// <param name="tram"></param>
        /// <returns></returns>
        public Sector GetAssignedSector(Tram tram)
        {
            return _objectCreator.CreateSector(_tramContext.GetAssignedSector(tram));
        }

        /// <summary>
        /// Edit tram.
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
