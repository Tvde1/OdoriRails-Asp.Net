using System;
using System.Linq;
using System.Web.Mvc;
using OdoriRails.Helpers;
using OdoriRails.Helpers.Objects;
using OdoriRails.Models.SRManagement;

namespace OdoriRails.Controllers
{
    public class SRController : BaseControllerFunctions
    {
        // GET: SchoonmaakReparatie
        public ActionResult Index()
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = result as User;
            var model = new SRModel {User = user};

            if (user.Role == Role.Cleaner || user.Role == Role.HeadCleaner)
            {
                model.Cleans = model.CleaningListFromUser();
            }
            if (user.Role == Role.Engineer || user.Role == Role.HeadEngineer)
            {
                model.Repairs = model.RepairListFromUser(); 
            }

            return View(model);
        }

        public ActionResult EditRepair(int id)
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = result as User;
            var model = new SRModel { User = user };   

            if (user.Role != Role.HeadEngineer)
            {
                TempData["alertMessage"] = "You do not have permission to do this!";

                return RedirectToAction("Index", "SR");
            }
            model.Engineers = model.GetAllEngineers();
            model.RepairToEdit = model.GetRepairToEdit(id);
            return View(model);
        }
        public ActionResult EditCleaning(int id)
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = result as User;
            var model = new SRModel { User = user };
            

            if (user.Role != Role.HeadCleaner)
            {
                
                TempData["alertMessage"] = "You do not have permission to do this!";
                
                return RedirectToAction("Index", "SR"); ;
            }
            model.Cleaners = model.GetAllCleaners();
            model.CleaningToEdit = model.GetCleaningToEdit(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditCleaning(SRModel model)
        {
            
            model.EditCleaningInDb(model.CleaningToEdit);
            TempData["alertMessage"] = "Cleaning posted succesfully!";
            return RedirectToAction("Index", "SR");
        }

    }
}