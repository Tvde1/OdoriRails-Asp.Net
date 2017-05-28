using System;
using System.Web.Mvc;
using OdoriRails.Helpers;
using OdoriRails.Models;

namespace OdoriRails.Controllers
{
    public class UserBeheersysteemController : BaseControllerFunctions
    {
        [HttpGet]
        public ActionResult Index() //Show all users
        {
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            var model = new UserBeheerModel();
            model.User = user;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserBeheerModel model)
        {
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            if (model.User == null) throw new Exception("wat xd");

            model.UpdateUserList();
            return View(model);
        }

        public ActionResult Edit(EditUserModel model)
        {
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            if (model.User == null) throw new Exception("lmao xd");

            var result = model.Save();
            return result ?? RedirectToAction("Index",
                       new UserBeheerModel {Sucess = "De gebruiker is aangemaakt/veranderd."});
        }
    }
}