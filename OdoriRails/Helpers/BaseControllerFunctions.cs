using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OdoriRails.Helpers.Objects;
using OdoriRails.Models;

namespace OdoriRails.Helpers
{
    public abstract class BaseControllerFunctions : Controller
    {
        protected object GetLoggedInUser(IEnumerable<Role> roles)
        {
            var user = (User) Session["User"];
            if (user == null) return RedirectToLogin("U bent niet ingelogd.");
            if (!roles.Contains(user.Role)) return RedirectToLogin("U bent hier niet voor gemachtigd.");
            return user;
        }

        private ActionResult RedirectToLogin(string text)
        {
            TempData["SigninModel"] = new LoginModel {Error = text};
            return RedirectToAction("Index", "Login");
        }
    }
}