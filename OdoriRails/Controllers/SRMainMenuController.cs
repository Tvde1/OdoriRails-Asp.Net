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
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result.GetType() == typeof(ActionResult)) return result as ActionResult;
            var user = result as User;


            return View();
        }
    }
}