using System.Web.Mvc;
using OdoriRails.Helpers;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Controllers
{
    public class SRMainMenuController : BaseControllerFunctions
    {
        // GET: SchoonmaakReparatie
        public ActionResult Index()
        {
            var user = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (user == null) return NotLoggedIn();


            return View();
        }
    }
}