using System.Web.Mvc;
using OdoriRails.Helpers;
using OdoriRails.Helpers.Objects;
using OdoriRails.Models;

namespace OdoriRails.Controllers
{
    public class DriverController : BaseControllerFunctions
    {
        // GET: Driver
        public ActionResult Index()
        {
            var user = GetLoggedInUser(new[] { Role.Driver });
            if (user == null) return NotLoggedIn();

            var model = new DriverModel(user);

            return View(model);
        }
    }
}