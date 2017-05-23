using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.DAL.Contexts;

namespace OdoriRails.Helpers.DAL.Repository
{
    public class LoginRepository : BaseRepository
    {
        private readonly ILoginContext _loginContext = new LoginContext();
        private readonly IUserContext _userContext = new UserContext();
        private readonly ObjectCreator _objectCreator = new ObjectCreator();

        public int ValidateUser(string username, string password)
        {
            if (!_loginContext.ValidateUsername(username)) return -2;
            if (!_loginContext.MatchUsernameAndPassword(username, password)) return -1;
            return 1;
        }


        public User FetchUser(string username)
        {
            return _objectCreator.CreateUser(_userContext.GetUser(username));
        }
    }
}