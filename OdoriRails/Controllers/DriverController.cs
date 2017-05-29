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
            var result = GetLoggedInUser(new[] { Role.Driver });
            if (result is ActionResult) return result as ActionResult;
            var user = result as User;

            var model = new DriverModel(user);

            return View(model);
        }
    }
}