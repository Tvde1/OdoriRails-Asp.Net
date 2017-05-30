using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using OdoriRails.Helpers;
using OdoriRails.Helpers.Objects;
using OdoriRails.Models;

namespace OdoriRails.Controllers
{
    public class DriverController : BaseControllerFunctions
    {
        [HttpGet]
        public ActionResult Index()
        {
            var result = GetLoggedInUser(new[] { Role.Driver });
            if (result is ActionResult) return result as ActionResult;
            var user = result as User;

            var model = new DriverModel(user);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DriverModel model)
        {
            var result = GetLoggedInUser(new[] { Role.Driver });
            if (result is ActionResult) return result as ActionResult;
            model.User = result as User;

            model.FetchTramUpdates();

            //var isRemise = Request.Form["remise"];
            //var isLeave = Request.Form["leave"];

            //if (isRemise == null && isLeave == null)
            //    throw new Exception("wat xd");

            if (model.Tram.Location == TramLocation.Out)
            {
                model.Tram.EditTramLocation(TramLocation.ComingIn);

                if (model.NeedsCleaning)
                {
                    model.Tram.EditTramStatus(TramStatus.Cleaning);
                    model.AddCleaning();
                }

                if (model.NeedsRepair)
                {
                    model.Tram.EditTramStatus(TramStatus.Defect);
                    model.AddRepair();
                }

                if (model.NeedsRepair && model.NeedsCleaning)
                    model.Tram.EditTramStatus(TramStatus.CleaningMaintenance);

                model.UpdateTram();

                model.WaitForLocationUpdate();
            }
            else
            {
                model.Tram.EditTramLocation(TramLocation.GoingOut);
                model.UpdateTram();
                model.WaitForStatusOut();
            }

            model.FetchTramUpdates();

            model.NeedsRepair = false;
            model.NeedsCleaning = false;
            model.Comments = null;

            return View(model);
        }
    }
}