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

        public ActionResult EditService(Repair repair)
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = result as User;
            var model = new SRModel { User = user };

            if (user.Role == Role.HeadEngineer)
            {
                model.RepairToEdit = repair;
            }

            return View(model);
        }
        public ActionResult EditService(Cleaning cleaning)
        {
            var result = GetLoggedInUser(new[] { Role.Cleaner, Role.Engineer, Role.HeadCleaner, Role.HeadEngineer });
            if (result is ActionResult) return result as ActionResult;
            var user = result as User;
            var model = new SRModel { User = user };

            if (user.Role == Role.HeadCleaner)
            {
                model.CleaningToEdit = cleaning;
            }

            return View(model);
        }
    }
}