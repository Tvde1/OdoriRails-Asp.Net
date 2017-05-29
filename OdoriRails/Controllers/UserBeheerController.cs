using System.Web.Mvc;
using OdoriRails.Helpers;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;
using OdoriRails.Models.UserBeheer;

namespace OdoriRails.Controllers
{
    public class UserBeheerController : BaseControllerFunctions
    {
        [HttpGet]
        public ActionResult Index(int? id)
        {
            var result = GetLoggedInUser(new[] { Role.Administrator });
            if (result.GetType() == typeof(ActionResult)) return result as ActionResult;
            var user = result as User;

            var model = TempData["BeheerModel"] as UserBeheerModel ?? new UserBeheerModel { User = user };
            if (id != null) model.DeleteUser(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserBeheerModel model)
        {
            var result = GetLoggedInUser(new[] { Role.Administrator });
            if (result.GetType() == typeof(ActionResult)) return result as ActionResult;
            var user = result as User;

            model.User = user;

            model.UpdateUserList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var result = GetLoggedInUser(new[] { Role.Administrator });
            if (result.GetType() == typeof(ActionResult)) return result as ActionResult;
            var user = result as User;

            var editUser = id == null ? null : new UserBeheerRepository().GetUser(id.Value);

            var model = (EditUserModel)TempData["EditModel"] ?? new EditUserModel(editUser) { User = user };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditUserModel model)
        {
            var userResult = GetLoggedInUser(new[] { Role.Administrator });
            if (userResult.GetType() == typeof(ActionResult)) return userResult as ActionResult;
            var user = userResult as User;

            model.User = user;

            var result = model.Save(this);

            if (result != null) return result;

            var newModel = new UserBeheerModel { Sucess = "De gebruiker is aangemaakt/aangepast." };
            TempData["UserBeheerModel"] = newModel;
            return RedirectToAction("Index");
        }
    }
}