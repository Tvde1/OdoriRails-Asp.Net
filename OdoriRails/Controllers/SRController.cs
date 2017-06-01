using System.Collections.Generic;
using System.Web.Mvc;
using OdoriRails.Helpers;
using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;
using OdoriRails.Models.SRManagement;

namespace OdoriRails.Controllers
{
    public class SRController : BaseControllerFunctions
    {
        public SchoonmaakReparatieRepository _Repo = new SchoonmaakReparatieRepository();
        // GET: SchoonmaakReparatie
        public ActionResult Index()
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = (User) result;


            var model = TempData["SRModel"] as SRModel ?? new SRModel { User = user};

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
            var user = (User)result;

            var model = new SRModel(Role.Engineer) { User = user };
            var viewmodel = new EditRepairViewModel();

            if (user.Role != Role.HeadEngineer)
            {
                model.Error = "You do not have permission to do this!";
                TempData["SRModel"] = model;

                return RedirectToAction("Index", "SR");
            }

            viewmodel.RepairToChange = model.GetRepairToEdit(id);
            viewmodel.Id = viewmodel.RepairToChange.Id;
            viewmodel.AssignedWorkers = model.AssignedWorkers;

            return View(viewmodel);
        }
        public ActionResult EditCleaning(int id)
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = (User) result;

            var model = new SRModel(Role.Cleaner) { User = user };
            var viewmodel = new EditCleaningViewModel();

            if (user.Role != Role.HeadCleaner)
            {
                model.Error = "You do not have permission to do this!";
                TempData["SRModel"] = model;

                return RedirectToAction("Index", "SR");
            }

            viewmodel.CleaningToChange = model.GetCleaningToEdit(id);
            viewmodel.Id = viewmodel.CleaningToChange.Id;
            viewmodel.AssignedWorkers = model.AssignedWorkers;

            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult EditCleaning(EditCleaningViewModel viewmodel)
        {
            SRModel model = new SRModel();
            List<User> listusers = new List<User>();
            
            foreach (var user in viewmodel.AssignedWorkers)
            {
                if (user.Value)
                {
                    User usertoinsert = _Repo.GetUserFromName(user.Key);
                    listusers.Add(usertoinsert);
                }
            }

            Cleaning changedCleaning = new Cleaning(viewmodel.Id, viewmodel.StartDate, viewmodel.EndDate, viewmodel.Size, viewmodel.Comment, listusers, viewmodel.TramID);
            _Repo.EditService(changedCleaning);
            model.Error = "Cleaning posted succesfully!";
            //TempData["SRModel"] = model;
            return RedirectToAction("Index", "SR");
        }

    }
}