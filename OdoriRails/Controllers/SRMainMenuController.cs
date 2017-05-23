using System.Web.Mvc;
using OdoriRails.Helpers;

namespace OdoriRails.Controllers
{
    public class SRMainMenuController : BaseControllerFunctions
    {
        // GET: SchoonmaakReparatie
        public ActionResult Index()
        {
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();


            return View();
        }
    }
}