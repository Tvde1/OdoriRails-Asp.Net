using System.Web.Mvc;
using OdoriRails.Helpers.Objects;
using OdoriRails.Models;

namespace OdoriRails.Helpers
{
    public abstract class BaseControllerFunctions : Controller
    {
        protected User GetLoggedInUser()
        {
            return (User) Session["User"];
        }

        protected ActionResult NotLoggedIn()
        {
            TempData["SigninModel"] = new LoginModel { Error = "U bent niet ingelogd." };
            return RedirectToAction("Index", "Login");
        }
    }
}