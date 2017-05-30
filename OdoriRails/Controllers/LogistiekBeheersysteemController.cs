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
        LogistiekLogic logic = new LogistiekLogic();

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

        //Tab Actions
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

        //Main Actions
        public ActionResult LockTrack(FormResultModel result)
        {
            if (result.RadioButton == 1)
            {
                logic.Lock(result.TrackNumber, result.SectorNumber);
            }
            else
            {
                logic.Unlock(result.TrackNumber, result.SectorNumber);
            }
            return RedirectToAction("Index");
        }

        public ActionResult MoveTram(FormResultModel result)
        {
            logic.MoveTram(result.TramNumber, result.TrackNumber, result.SectorNumber);
            return RedirectToAction("Index");
        }

        public ActionResult ToggleDisabled(FormResultModel result)
        {
            logic.ToggleDisabled(result.TramNumber);
            return RedirectToAction("Index");
        }

        //Main Actions
        public ActionResult AddTram(FormResultModel result)
        {
            logic.AddTram(result.TramNumber, result.DefaultLine, result.TramModel);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteTram(FormResultModel result)
        {
            logic.DeleteTram(result.TramNumber);
            return RedirectToAction("Index");
        }

        public ActionResult AddTrack(FormResultModel result)
        {
            logic.AddTrack(result.TrackNumber, result.SectorAmount, result.TrackType, result.DefaultLine);
            return RedirectToAction("Index");
        }
        public ActionResult DeleteTrack(FormResultModel result)
        {
            logic.DeleteTram(result.TrackNumber);
            return RedirectToAction("Index");
        }

        public ActionResult AddDeleteSector(FormResultModel result)
        {
            if(result.RadioButton == 1)
            {
                logic.DeleteSector(result.TrackNumber);
            }
            else
            {
                logic.AddSector(result.TrackNumber);
            }
            return RedirectToAction("Index");
        }
    }
}