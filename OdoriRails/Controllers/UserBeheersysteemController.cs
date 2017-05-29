using System;
using System.Web.Mvc;
using OdoriRails.Helpers;
using OdoriRails.Models;

namespace OdoriRails.Controllers
{
    public class UserBeheersysteemController : BaseControllerFunctions
    {
        [HttpGet]
        public ActionResult Index(int? id)
        {
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            var model = new UserBeheerModel { User = user };
            if (id != null) model.DeleteUser(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserBeheerModel model)
        {
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            model.User = user;

            model.UpdateUserList();
            return View(model);
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("Index", "UserBeheersysteem");
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            var model = new EditUserModel(id.Value) { User = user };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditUserModel model)
        {
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            model.User = user;

            var result = model.Save();
            return result ?? RedirectToAction("Index",
                       new UserBeheerModel { Sucess = "De gebruiker is aangemaakt/veranderd." });
        }
    }
}