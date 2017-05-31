using System;
using System.Collections.Generic;
using System.Data;
using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.DAL.Contexts;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.DAL.Repository
{
    public class SchoonmaakReparatieRepository : BaseRepository
    {
        private readonly ObjectCreator _objectCreator = new ObjectCreator();
        private readonly IServiceContext _serviceContext = new ServiceContext();
        private readonly ITramContext _tramContext = new TramContext();
        private readonly IUserContext _userContext = new UserContext();

        /// <summary>
        ///     Haal alle reparaties op die deze user heeft.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Repair> GetAllRepairsFromUser(User user)
        {
            return ObjectCreator.GenerateListWithFunction(_serviceContext.GetAllRepairsFromUser(user),
                _objectCreator.CreateRepair);
        }

        /// <summary>
        ///     Haal alle schoonmaaks op die deze user heeft.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Cleaning> GetAllCleansFromUser(User user)
        {
            return ObjectCreator.GenerateListWithFunction(_serviceContext.GetAllCleansFromUser(user),
                _objectCreator.CreateCleaning);
        }

        /// <summary>
        ///     Haalt een lijst op van repairs zonder users.
        /// </summary>
        /// <returns></returns>
        public List<Repair> GetAllRepairsWithoutUsers()
        {
            return ObjectCreator.GenerateListWithFunction(_serviceContext.GetAllRepairsWithoutUsers(),
                _objectCreator.CreateRepair);
        }

        /// <summary>
        ///     Haalt een lijst op van cleanings zonder users.
        /// </summary>
        /// <returns></returns>
        public List<Cleaning> GetAllCleansWithoutUsers()
        {
            return ObjectCreator.GenerateListWithFunction(_serviceContext.GetAllCleansWithoutUsers(),
                _objectCreator.CreateCleaning);
        }

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
        ///     Haalt Repair/Cleaning op basis van ID
        /// </summary>
        /// <param name="repair"></param>
        /// <returns></returns>
        public List<Repair> GetRepairFromId(int id)
        {
            return ObjectCreator.GenerateListWithFunction(_serviceContext.GetAllRepairsfromId(id),_objectCreator.CreateRepair);
        }
        public List<Cleaning> GetCleanFromId(int id)
        {
            return ObjectCreator.GenerateListWithFunction(_serviceContext.GetAllCleansfromId(id), _objectCreator.CreateCleaning);
        }

        /// <summary>
        ///     Past de service aan in de database.
        /// </summary>
        /// <param name="service"></param>
        public void EditService(Service service)
        {
            _serviceContext.EditService(service);
        }

        /// <summary>
        ///     Verweider een service uit de database.
        /// </summary>
        /// <param name="service"></param>
        public void DeleteService(Service service)
        {
            _serviceContext.DeleteService(service);
        }

        /// <summary>
        ///     Haal een User op aan de hand van de username.
        /// </summary>
        /// <param name="userName"></param>
        public User GetUser(string userName)
        {
            return _objectCreator.CreateUser(_userContext.GetUser(userName));
        }

        /// <summary>
        ///     Haalt alle users op die deze rol hebben.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public List<User> GetAllUsersWithFunction(Role role)
        {
            return ObjectCreator.GenerateListWithFunction(_userContext.GetAllUsersWithFunction(role),
                _objectCreator.CreateUser);
        }

        public DataTable GetAllRepairsFromTram(int tramId)
        {
            return _serviceContext.GetAllRepairsFromTram(tramId);
        }

        public List<Cleaning> GetAllCleaningsFromTram(int tramId)
        {
            return ObjectCreator.GenerateListWithFunction(_serviceContext.GetAllCleaningsFromTram(tramId),
                _objectCreator.CreateCleaning);
        }

        /// <summary>
        ///     Returnt een int[] met Repairs,Queries
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public int[] GetAllRepairsForDay(DateTime day)
        {
            return _serviceContext.RepairsForDate(day);
        }

        /// <summary>
        ///     Returnt een int[] met bigclean, smallclean
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public int[] GetAllCleansFromDay(DateTime day)
        {
            return _serviceContext.CleansForDate(day);
        }

        public bool DoesTramExist(int id)
        {
            return _tramContext.DoesTramExist(id);
        }

        /// <summary>
        ///     Sets the tram's status to idle (when service is finished).
        /// </summary>
        /// <param name="tramId"></param>
        public void SetTramStatusToIdle(int tramId)
        {
            _tramContext.SetStatusToIdle(tramId);
        }

        public void GetRepairsFromTram(int tramId)
        {
            _serviceContext.GetAllRepairsFromTram(tramId);
        }

        public void GetCleansFromTram(int tramId)
        {
            _serviceContext.GetAllRepairsFromTram(tramId);
        }

        public void AddSolution(Repair repair, string solution)
        {
            var newRepair = new Repair(repair.Id, repair.StartDate, repair.EndDate, repair.Type, repair.Defect,
                solution, repair.AssignedUsers, repair.TramId);
            _serviceContext.EditService(newRepair);
        }

        public List<Tram> GetAllTrams()
        {
            return ObjectCreator.GenerateListWithFunction(_tramContext.GetAllTrams(), _objectCreator.CreateTram);
        }
    }
}