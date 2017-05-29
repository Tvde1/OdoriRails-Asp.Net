using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OdoriRails.Helpers.Objects;
using OdoriRails.Models;

namespace OdoriRails.Helpers
{
    public abstract class BaseControllerFunctions : Controller
    {
        protected User GetLoggedInUser(IEnumerable<Role> roles)
        {
            var user  = (User) Session["User"];
            return user != null && roles.Contains(user.Role) ? user : null;
        }

        protected ActionResult NotLoggedIn()
        {
            TempData["SigninModel"] = new LoginModel {Error = "U bent niet ingelogd."};
            return RedirectToAction("Index", "Login");
        }
    }
}