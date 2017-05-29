using System.Collections.Generic;
using System.Linq;
using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.DAL.Contexts;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.DAL.Repository
{
    public class UserBeheerRepository : BaseRepository
    {
        private readonly ILoginContext _loginContext = new LoginContext();
        private readonly ObjectCreator _objectCreator = new ObjectCreator();
        private readonly ITramContext _tramContext = new TramContext();
        private readonly IUserContext _userContext = new UserContext();

        /// <summary>
        ///     Voegt een User toe aan de database.
        /// </summary>
        /// <param name="user"></param>
        public User AddUser(User user)
        {
            return _userContext.AddUser(user);
        }

        /// <summary>
        ///     Haalt alle users op.
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            return ObjectCreator.GenerateListWithFunction(_userContext.GetAllUsers(), _objectCreator.CreateUser);
        }

        /// <summary>
        ///     Verwijdert een User uit de database.
        /// </summary>
        /// <param name="user"></param>
        public void RemoveUser(int userId)
        {
            _userContext.RemoveUser(userId);
        }

        /// <summary>
        ///     Haal een User op aan de hand van de userid.
        /// </summary>
        /// <param name="id"></param>
        public User GetUser(int id)
        {
            return _objectCreator.CreateUser(_userContext.GetUser(id));
        }

        /// <summary>
        ///     Get de user ID via de username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int? GetUserId(string username)
        {
            var data = _userContext.GetUserId(username);
            return (int?) data?["UserPk"];
        }

        public int? GetUserIdByFullName(string name)
        {
            return _userContext.GetUserIdByName(name);
        }

        /// <summary>
        ///     Slaat de bestaande user op in de database.
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            _userContext.UpdateUser(user);
            SetUserToTrams(user);
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

        public bool DoesTramExist(int id)
        {
            return _tramContext.DoesTramExist(id);
        }

        public List<int> GetTramIdByDriverId(int id)
        {
            return ObjectCreator.GenerateListWithFunction(_tramContext.GetTramIdsByDriverId(id),
                row => (int) row.ItemArray[0]);
        }

        public void SetUserToTrams(User user)
        {
            var tramsWithUser =
                ObjectCreator.GenerateListWithFunction(_tramContext.GetTramsByDriver(user), row => (int) row["TramPk"]);
            foreach (var tram in tramsWithUser.Where(x => !user.TramIds.Contains(x)))
                _tramContext.SetUserToTram(tram, null);

            foreach (var userTramId in user.TramIds)
                _tramContext.SetUserToTram(userTramId, user.Id);
        }

        /// <summary>
        ///     Returns true if there already is an user with such name.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool DoesUserExist(string userName)
        {
            return _loginContext.ValidateUsername(userName);
        }
    }
}