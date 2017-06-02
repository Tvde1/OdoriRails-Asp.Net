using OdoriRails.Helpers;
using OdoriRails.Helpers.LogistiekBeheersysteem;
using System.Web.Mvc;
using OdoriRails.Helpers.Objects;
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
            var result = GetLoggedInUser(new[] { Role.Logistic, Role.Administrator });
            if (result is ActionResult) return result as ActionResult;
            var user = result as User;

            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null)
            {
                remise = new LogistiekBeheerModel();
                Session["Remise"] = remise;
            }
            remise.User = user;

            return View(remise);
        }

        //Tab Actions
        public ActionResult SetStateMain()
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            remise.State = LogistiekState.Main;
            return RedirectToAction("Index");
        }
        public ActionResult SetStateEdit()
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            remise.State = LogistiekState.Edit;
            return RedirectToAction("Index");
        }
        public ActionResult SetStateDelete()
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            remise.State = LogistiekState.Delete;
            return RedirectToAction("Index");
        }

        //Main Actions
        public ActionResult LockTrack(FormResultModel result)
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            //var tracks = result.TrackNumbers.Split(',');
            if (result.RadioButton == 1)
            {
                if (string.IsNullOrEmpty(result.TrackNumbers) || !_logic.Lock(result.TrackNumbers, result.SectorNumbers))
                {
                    remise.Error = "Input is incorrect.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(result.TrackNumbers) ||  !_logic.Unlock(result.TrackNumbers, result.SectorNumbers))
                {
                    remise.Error = "Input is incorrect.";
                    return RedirectToAction("Index");
                }
            }
            remise = null;
            return RedirectToAction("Index");
        }
        public ActionResult MoveTram(FormResultModel result)
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");

            if (_logic.MoveTram(result.TramNumber, result.TrackNumber, result.SectorNumber) == false)
            {
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
            var remise = (LogistiekBeheerModel) Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            _logic.ToggleDisabled(result.TramNumbers);
            Session["Remise"] = null;
            return RedirectToAction("Index");
        }

        //Main Actions
        public ActionResult AddTram(FormResultModel result)
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            _logic.AddTram(result.TramNumber, result.DefaultLine, result.TramModel);
            Session["Remise"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult DeleteTram(FormResultModel result)
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            if (_logic.DeleteTram(result.TramNumber) == false)
            {
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
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            _logic.AddTrack(result.TrackNumber, result.SectorAmount, result.TrackType, result.DefaultLine);
            Session["Remise"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult DeleteTrack(FormResultModel result)
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            if (_logic.DeleteTrack(result.TrackNumber) == false)
            {
                remise.Error = "Failed to delete the track";
            }
            else
            {
                Session["Remise"] = null; //Reload Display after update
            }
            return RedirectToAction("Index");
        }
        public ActionResult AddSector(FormResultModel result)
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            if (_logic.AddSector(result.TrackNumber) == false)
            {
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
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            _logic.DeleteSector(result.TrackNumber);
            remise.Error = "Failed to add the sector to the track";
            return RedirectToAction("Index");
        }
    }
}