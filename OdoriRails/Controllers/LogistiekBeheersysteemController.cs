using OdoriRails.Helpers;
using OdoriRails.Helpers.LogistiekBeheersysteem;
using OdoriRails.Helpers.Objects;
using OdoriRails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OdoriRails.Models.LogistiekBeheer;

namespace OdoriRails.Controllers
{
    public class LogistiekBeheersysteemController : BaseControllerFunctions
    {
        private readonly LogistiekLogic _logic = new LogistiekLogic();

        [HttpGet]
        public ActionResult Index()
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
            return RedirectToAction("Index");
        }
        public ActionResult SetStateEdit()
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (Session["Remise"] == null) return RedirectToAction("Index");
            remise.State = LogistiekState.Edit;
            return RedirectToAction("Index");
        }
        public ActionResult SetStateDelete()
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (Session["Remise"] == null) return RedirectToAction("Index");
            remise.State = LogistiekState.Delete;
            return RedirectToAction("Index");
        }

        //Main Actions
        public ActionResult LockTrack(FormResultModel result)
        {
            //var tracks = result.TrackNumbers.Split(',');
            if (result.RadioButton == 1)
            {
                if (!_logic.Lock(result.TrackNumbers, result.SectorNumbers))
                {
                    var remise = (LogistiekBeheerModel)Session["Remise"];
                    remise.Error = "Input is incorrect.";
                }
            }
            else
            {
                if (!_logic.Unlock(result.TrackNumbers, result.SectorNumbers))
                {
                    var remise = (LogistiekBeheerModel)Session["Remise"];
                    remise.Error = "Input is incorrect.";
                }
            }
            Session["Remise"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult MoveTram(FormResultModel result)
        {
            if (_logic.MoveTram(result.TramNumber, result.TrackNumber, result.SectorNumber) == false)
            {
                var remise = (LogistiekBeheerModel)Session["Remise"];
                remise.Error = "Failed to move the tram";
            }
            else
            {
                Session["Remise"] = null; //Reload Display after update
            }

            return RedirectToAction("Index");
        }
        public ActionResult ToggleDisabled(FormResultModel result)
        {
            _logic.ToggleDisabled(result.TramNumbers);
            Session["Remise"] = null;
            return RedirectToAction("Index");
        }

        //Main Actions
        public ActionResult AddTram(FormResultModel result)
        {
            _logic.AddTram(result.TramNumber, result.DefaultLine, result.TramModel);
            Session["Remise"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult DeleteTram(FormResultModel result)
        {
            if (_logic.DeleteTram(result.TramNumber) == false)
            {
                var remise = (LogistiekBeheerModel)Session["Remise"];
                remise.Error = "Failed to delete the tram";
            }
            else
            {
                Session["Remise"] = null; //Reload Display after update
            }
            return RedirectToAction("Index");
        }
        public ActionResult AddTrack(FormResultModel result)
        {
            _logic.AddTrack(result.TrackNumber, result.SectorAmount, result.TrackType, result.DefaultLine);
            Session["Remise"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult DeleteTrack(FormResultModel result)
        {
            if (_logic.DeleteTrack(result.TrackNumber) == false)
            {
                var remise = (LogistiekBeheerModel)Session["Remise"];
                remise.Error = "Failed to delete the track";
            }
            else
            {
                Session["Remise"] = null; //Reload Display after update
            }
            return RedirectToAction("Index");
        }
        public ActionResult AddDeleteSector(FormResultModel result)
        {
            if (_logic.AddSector(result.TrackNumber) == false)
            {
                var remise = (LogistiekBeheerModel)Session["Remise"];
                remise.Error = "Failed to delete the sector from the track";
            }
            else
            {
                Session["Remise"] = null; //Reload Display after update
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteSector(FormResultModel result)
        {
            _logic.DeleteSector(result.TrackNumber);
            var remise = (LogistiekBeheerModel)Session["Remise"];
            remise.Error = "Failed to add the sector to the track";
            return RedirectToAction("Index");
        }
    }
}