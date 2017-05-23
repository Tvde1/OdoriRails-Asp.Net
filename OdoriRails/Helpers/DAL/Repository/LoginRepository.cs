using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.DAL.Contexts;

namespace OdoriRails.Helpers.DAL.Repository
{
    public class LoginRepository : BaseRepository
    {
        private ILoginContext _loginContext;

        public LoginRepository()
        {
            _loginContext = new LoginContext(DatabaseHandler);
        }


    }
}