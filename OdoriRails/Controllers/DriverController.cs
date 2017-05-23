using OdoriRails.Models;
using System.Web.Mvc;
using OdoriRails.Helpers;

namespace OdoriRails.Controllers
{
    public class DriverController : BaseControllerFunctions
    {
        // GET: Driver
        public ActionResult Index()
        {
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            var model = new DriverModel { User = user };

            return View(model);
        }
    }
}