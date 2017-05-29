﻿using System.Web;
using System.Web.Mvc;
using OdoriRails.Helpers;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Models;

namespace OdoriRails.Controllers
{
    public class UserBeheerController : BaseControllerFunctions
    {
        [HttpGet]
        public ActionResult Index(int? id)
        {
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            var model = TempData["BeheerModel"] as UserBeheerModel ?? new UserBeheerModel { User = user };
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
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            var editUser = id == null ? null : new UserBeheerRepository().GetUser(id.Value);

            var model = (EditUserModel)TempData["EditModel"] ?? new EditUserModel(editUser) { User = user };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditUserModel model)
        {
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            model.User = user;

            var result = model.Save(this);

            if (result != null) return result;

            var newModel = new UserBeheerModel { Sucess = "De gebruiker is aangemaakt/aangepast."};
            TempData["UserBeheerModel"] = newModel;
            return RedirectToAction("Index");
        }
    }
}