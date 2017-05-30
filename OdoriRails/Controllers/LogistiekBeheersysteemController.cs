using OdoriRails.Helpers;
using OdoriRails.Helpers.LogistiekBeheersysteem;
using OdoriRails.Helpers.Objects;
using OdoriRails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OdoriRails.Controllers
{
    public class LogistiekBeheersysteemController : BaseControllerFunctions
    {
        [HttpGet]
        public ActionResult Index(int? id)
        {
            //Check if logged-in
            //var user = GetLoggedInUser();
            //if (user == null) return RedirectToLogin();

            var remise = (LogistiekBeheerModel)Session["Remise"];

            if (Session["Remise"] == null)
            {
                remise = new LogistiekBeheerModel();
                LogistiekLogic logic = new LogistiekLogic();

                remise.Tracks = logic.AllTracks;
                remise.Trams = logic.AllTrams;

                Session["Remise"] = remise;
            }
            return View(remise);
        }

        public ActionResult SetStateMain()
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (Session["Remise"] == null) return RedirectToAction("Index");
            remise.State = LogistiekState.Main;
            Session["Remise"] = remise;
            return RedirectToAction("Index");
        }

        public ActionResult SetStateEdit()
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (Session["Remise"] == null) return RedirectToAction("Index");
            remise.State = LogistiekState.Edit;
            Session["Remise"] = remise;
            return RedirectToAction("Index");
        }


    }
}