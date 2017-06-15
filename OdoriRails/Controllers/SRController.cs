using System;
using System.Collections.Generic;
using System.Linq;
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

        private SchoonmaakReparatieRepository _repo = new SchoonmaakReparatieRepository();
        // GET: SchoonmaakReparatie
        public ActionResult Index()
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = (User)result;


            var model = TempData["SRLogic"] as SRLogic ?? new SRLogic { User = user };

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

        public ActionResult AddCleaning()
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = (User)result;
            SRLogic _logic = new SRLogic(Role.Cleaner);
            AddCleaningModel model = new AddCleaningModel();

            if (user.Role != Role.HeadCleaner)
            {
                _logic.Error = "You do not have permission to do this!";
                TempData["SRLogic"] = _logic;

                return RedirectToAction("Index", "SR");
            }
            model.AssignedWorkers = _logic.AssignedWorkers;
            return View(model);
        }

        public ActionResult AddRepair()
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = (User)result;
            SRLogic _logic = new SRLogic(Role.Engineer);
            AddRepairModel model = new AddRepairModel();

            if (user.Role != Role.HeadEngineer)
            {
                _logic.Error = "You do not have permission to do this!";

                return RedirectToAction("Index", "SR");
            }
            model.AssignedWorkers = _logic.AssignedWorkers;
            return View(model);
        }

        public ActionResult AssignUsers()
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = (User)result;


            var model = TempData["SRLogic"] as SRLogic ?? new SRLogic { User = user };

            if (user.Role == Role.Cleaner || user.Role == Role.HeadCleaner)
            {
                model.Cleans = _repo.GetAllCleansWithoutUsers();
            }
            if (user.Role == Role.Engineer || user.Role == Role.HeadEngineer)
            {
                model.Repairs = _repo.GetAllRepairsWithoutUsers();
            }

            return View(model);
        }
        public ActionResult MarkAsDone(int id)
        {
            var logic = new SRLogic();
            var markasdonemodel = new MarkAsDoneViewModel();
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });

            if (result is ActionResult) return result as ActionResult;
            var user = (User)result;

            
            if (user.Role == Role.HeadEngineer || user.Role == Role.Engineer)
            {
                Repair servicetomarkasdone = logic.GetRepairToEdit(id);
                markasdonemodel.RepairMarkAsDone = servicetomarkasdone;
                markasdonemodel.Serviceid = servicetomarkasdone.Id;

            }
            if (user.Role == Role.HeadCleaner || user.Role == Role.Cleaner)
            {
                Cleaning servicetomarkasdone = logic.GetCleaningToEdit(id);
                markasdonemodel.CleaningMarkAsDone = servicetomarkasdone;
                markasdonemodel.Serviceid = servicetomarkasdone.Id;
            }
            markasdonemodel.User = user;
            return View(markasdonemodel);
        }
        public ActionResult EditRepair(int id)
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = (User)result;
            var _logic = new SRLogic(Role.Engineer) { User = user };
            var viewmodel = new EditRepairViewModel();

            if (user.Role != Role.HeadEngineer)
            {
                _logic.Error = "You do not have permission to do this!";
                TempData["SRLogic"] = _logic;

                return RedirectToAction("Index", "SR");
            }

            viewmodel.RepairToChange = _logic.GetRepairToEdit(id);
            viewmodel.Id = viewmodel.RepairToChange.Id;
            viewmodel.AssignedWorkers = _logic.AssignedWorkers;

            return View(viewmodel);
        }
        public ActionResult EditCleaning(int id)
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = (User)result;

            var _logic = new SRLogic(Role.Cleaner) { User = user };
            var viewmodel = new EditCleaningViewModel();

            if (user.Role != Role.HeadCleaner)
            {
                _logic.Error = "You do not have permission to do this!";
                TempData["SRLogic"] = _logic;

                return RedirectToAction("Index", "SR");
            }
            if (viewmodel.EndDate < viewmodel.StartDate)
            {
                viewmodel.EndDate = null;
            }
            viewmodel.CleaningToChange = _logic.GetCleaningToEdit(id);
            viewmodel.Id = viewmodel.CleaningToChange.Id;
            viewmodel.AssignedWorkers = _logic.AssignedWorkers;

            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult EditCleaning(EditCleaningViewModel viewmodel)
        {
            SRLogic logic = new SRLogic();
            List<User> listusers = new List<User>();

            foreach (var user in viewmodel.AssignedWorkers)
            {
                if (user.Value)
                {
                    User usertoinsert = _repo.GetUserFromName(user.Key);
                    listusers.Add(usertoinsert);
                }
            }
            if (viewmodel.EndDate < viewmodel.StartDate)
            {
                viewmodel.EndDate = null;
            }
            Cleaning changedCleaning = new Cleaning(viewmodel.Id, viewmodel.StartDate, viewmodel.EndDate, viewmodel.Size, viewmodel.Comment, listusers, viewmodel.TramID);
            
            try
            {
                _repo.EditService(changedCleaning);
            }
            catch
            {
                viewmodel.Error = "Something went wrong with posting the ervice. Check if the date field is filled and if the tram number is valid!";
                return View(viewmodel);
            }
            logic.Error = "Cleaning posted succesfully!";
            //TempData["SRLogic"] = logic; 
            return RedirectToAction("Index", "SR");
        }

        [HttpPost]
        public ActionResult EditRepair(EditRepairViewModel viewmodel)
        {
            SRLogic logic = new SRLogic();
            List<User> listusers = new List<User>();

            foreach (var user in viewmodel.AssignedWorkers)
            {
                if (user.Value)
                {
                    User usertoinsert = _repo.GetUserFromName(user.Key);
                    listusers.Add(usertoinsert);
                }
            }
            if (viewmodel.EndDate < viewmodel.StartDate)
            {
                viewmodel.EndDate = null;
            }
            Repair changedRepair = new Repair(viewmodel.Id, viewmodel.StartDate, viewmodel.EndDate, viewmodel.Size, viewmodel.Defect, viewmodel.Solution, listusers, viewmodel.TramID);
            try
            {
                _repo.EditService(changedRepair);
            }
            catch
            {
                viewmodel.Error = "Something went wrong with posting the ervice. Check if the date field is filled and if the tram number is valid!";
                return View(viewmodel);
            }
            
            logic.Error = "Repair posted succesfully!";
            //TempData["SRLogic"] = logic;
            return RedirectToAction("Index", "SR");
        }

        [HttpPost]
        public ActionResult MarkAsDone(MarkAsDoneViewModel viewmodel)
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = (User)result;

            if (user.Role == Role.HeadEngineer || user.Role == Role.Engineer)
            {
                var rlist = _repo.GetRepairFromId(viewmodel.Serviceid);
                var repairtofinish = rlist.ElementAt(0);
                _repo.SetTramStatusToIdle(viewmodel.TramIdtoCarryOver);
                repairtofinish.EndDate = DateTime.Now;
                repairtofinish.Solution = viewmodel.Solution;
                if (string.IsNullOrEmpty(viewmodel.Solution))
                {
                    viewmodel.Error = "Fill the text field and try again!";
                    viewmodel.User = user;
                    viewmodel.RepairMarkAsDone = repairtofinish;
                    return View(viewmodel);
                }
                _repo.EditService(repairtofinish);

            }
            if (user.Role == Role.HeadCleaner || user.Role == Role.Cleaner)
            {
                var clist = _repo.GetCleanFromId(viewmodel.Serviceid);
                var cleantofinish = clist.ElementAt(0);
                _repo.SetTramStatusToIdle(viewmodel.TramIdtoCarryOver);
                cleantofinish.EndDate = DateTime.Now;
                cleantofinish.Comments = viewmodel.Comment;
                if (string.IsNullOrEmpty(viewmodel.Comment))
                {
                    viewmodel.Error = "Fill the text field and try again!";
                    viewmodel.User = user;
                    viewmodel.CleaningMarkAsDone = cleantofinish;
                    return View(viewmodel);
                }
                _repo.EditService(cleantofinish);
            }

     

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddCleaning(AddCleaningModel model)
        {
            List<User> listusers = new List<User>();
            foreach (var user in model.AssignedWorkers)
            {
                if (user.Value)
                {
                    User usertoinsert = _repo.GetUserFromName(user.Key);
                    listusers.Add(usertoinsert);
                }
            }
            Cleaning cleaningtopost = new Cleaning(model.StartDate, null, model.Size, model.Comment, listusers, model.TramID);
            try
            {
                _repo.AddCleaning(cleaningtopost);
            }
            catch
            {
                model.Error = "Something went wrong with posting the ervice. Check if the date field is filled and if the tram number is valid!";
                return View(model);
            }
            
            return RedirectToAction("Index");

        }
        [HttpPost]
        public ActionResult AddRepair(AddRepairModel model)
        {
            List<User> listusers = new List<User>();

            foreach (var user in model.AssignedWorkers)
            {
                if (user.Value)
                {
                    User usertoinsert = _repo.GetUserFromName(user.Key);
                    listusers.Add(usertoinsert);
                }
            }
            Repair repairtopost = new Repair(model.StartDate, null, model.Type, model.Defect, "", listusers, model.TramID);

            _repo.AddRepair(repairtopost);
            try
            {
                _repo.AddRepair(repairtopost);
            }
            catch
            {
                model.Error = "Something went wrong with posting the ervice. Check if the date field is filled and if the tram number is valid!";
                return View(model);
            }


            return RedirectToAction("Index");
        }

        public ActionResult TramHistory(string id)
        {
            var newId = 0;
            if (!int.TryParse(id ?? "0", out newId)) return RedirectToAction("Index");

            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = (User)result;

            var model = new TramHistoryModel { User = user, TramId = newId };
            model.GetServices();

            return View(model);
        }

        public ActionResult PlanMaintenance()
        {
            _repo.PlanMaintenance(7); //Hardcoded voor een week momenteel 
            return RedirectToAction("Index");
        }
    }
}