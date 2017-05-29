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
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            var remise = (LogistiekBeheerModel)Session["Remise"];

            if (Session["Remise"] == null)
            {
                LogistiekLogic logic = new LogistiekLogic();

                remise.Tracks = logic.AllTracks;
                remise.Trams = logic.AllTrams;

                Session["Remise"] = remise;
            }


            return View(remise);
        }
    }
}