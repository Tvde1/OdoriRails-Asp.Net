using System.Collections.Generic;
using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.DAL.Contexts;

namespace OdoriRails.Helpers.DAL.Repository
{
    public class UserBeheerRepository : BaseRepository
    {
        private readonly IUserContext _userContext = new UserContext();
        private readonly ITramContext _tramContext = new TramContext();
        private readonly ILoginContext _loginContext = new LoginContext();
        private readonly ObjectCreator _objectCreator = new ObjectCreator();

        /// <summary>
        /// Voegt een User toe aan de database.
        /// </summary>
        /// <param name="user"></param>
        public User AddUser(User user)
        {
            return _userContext.AddUser(user);
        }

        /// <summary>
        /// Haalt alle users op.
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            return ObjectCreator.GenerateListWithFunction(_userContext.GetAllUsers(), _objectCreator.CreateUser);
        }

        /// <summary>
        /// Verwijdert een User uit de database.
        /// </summary>
        /// <param name="user"></param>
        public void RemoveUser(User user)
        {
            _userContext.RemoveUser(user);
        }

        /// <summary>
        /// Haal een User op aan de hand van de userid.
        /// </summary>
        /// <param name="id"></param>
        public User GetUser(int id)
        {
            return _objectCreator.CreateUser(_userContext.GetUser(id));
        }

        /// <summary>
        /// Get de user ID via de username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetUserId(string username)
        {
            return _userContext.GetUserId(username);
        }

        /// <summary>
        /// Slaat de bestaande user op in de database.
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            _userContext.UpdateUser(user);
        }

        /// <summary>
        /// Haal een User op aan de hand van de username.
        /// </summary>
        /// <param name="userName"></param>
        public User GetUser(string userName)
        {
            return _objectCreator.CreateUser(_userContext.GetUser(userName));
        }

        /// <summary>
        /// Haalt alle users op die deze rol hebben.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public List<User> GetAllUsersWithFunction(Role role)
        {
            return ObjectCreator.GenerateListWithFunction(_userContext.GetAllUsersWithFunction(role), _objectCreator.CreateUser);
        }

        public bool DoesTramExist(int id)
        {
            return _tramContext.DoesTramExist(id);
        }

        public List<int> GetTramIdByDriverId(int id)
        {
            return ObjectCreator.GenerateListWithFunction(_tramContext.GetTramIdByDriverId(id), row => (int)row.ItemArray[0]);
        }

        public void SetUserToTram(User user, int? tramId)
        {
            if (tramId != null && DoesTramExist((int)tramId)) _tramContext.SetUserToTram(_objectCreator.CreateTram(_tramContext.GetTram((int)tramId)), user);
            else _tramContext.SetUserToTram(null, user);
        }

        /// <summary>
        /// Returns true if there already is an user with such name.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool DoesUserExist(string userName)
        {
            return _loginContext.ValidateUsername(userName);
        }
    }
}
