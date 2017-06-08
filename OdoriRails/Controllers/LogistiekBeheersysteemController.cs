using OdoriRails.Helpers;
using OdoriRails.Helpers.LogistiekBeheersysteem;
using System.Web.Mvc;
using OdoriRails.Helpers.Objects;
using OdoriRails.Models.LogistiekBeheer;

namespace OdoriRails.Controllers
{
    public class LogistiekBeheersysteemController : BaseControllerFunctions
    {
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

        #region tab actions

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

        #endregion

        //Main Actions
        public ActionResult LockTrack(LogistiekBeheerModel result)
        {
            //Testen
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");

            if (string.IsNullOrEmpty(result.Form.TrackNumbers))
            {
                remise.Error = "U moet wel spoornummers opgeven.";
                return RedirectToAction("Index");
            }


            if (result.Form.RadioButton == 1)
            {
                var lockStatus = remise.Logic.Lock(result.Form.TrackNumbers, result.Form.SectorNumbers);

                if (lockStatus != null)
                {
                    remise.Error = lockStatus;
                    return RedirectToAction("Index");
                }

                remise.Sucess = "Input is incorrect.";
                return RedirectToAction("Index");
            }
            if (result.Form.RadioButton == 0)
            {
                var unlock = remise.Logic.Unlock(result.Form.TrackNumbers, result.Form.SectorNumbers);
                remise.Error = unlock;
                if (unlock == null)
                    remise.Sucess = "Track verplaatst!";
                Session["Remise"] = null;
            }
            Session["Remise"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult MoveTram(LogistiekBeheerModel result)
        {
            //Testen
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");

            var moveTramResult = remise.Logic.MoveTram(result.Form.TramNumber, result.Form.TrackNumber, result.Form.SectorNumber);

            remise.Error = moveTramResult;
            if (moveTramResult == null)
                remise.Sucess = "De tram is verplaatst.";

            return RedirectToAction("Index");
        }

        public ActionResult ToggleDisabled(LogistiekBeheerModel result)
        {
            //Testen
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            remise.Logic.ToggleDisabled(result.Form.TramNumbers);
            Session["Remise"] = null;
            return RedirectToAction("Index");
        }

        //Main Actions
        public ActionResult AddTram(LogistiekBeheerModel result)
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            remise.Logic.AddTram(result.Form.TramNumber, result.Form.DefaultLine, result.Form.TramModel);
            Session["Remise"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult DeleteTram(LogistiekBeheerModel result)
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            var deleteResult = remise.Logic.DeleteTram(result.Form.TramNumber);

            remise.Error = deleteResult;
            if (deleteResult == null)
                remise.Sucess = "De tram is verwijderd.";
            return RedirectToAction("Index");
        }
        public ActionResult AddTrack(LogistiekBeheerModel result)
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            remise.Logic.AddTrack(result.Form.TrackNumber, result.Form.SectorAmount, result.Form.TrackType, result.Form.DefaultLine);

            Session["Remise"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult DeleteTrack(LogistiekBeheerModel result)
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            var deleteResult = remise.Logic.DeleteTrack(result.Form.TrackNumber);

            remise.Error = deleteResult;
            if (deleteResult == null)
                remise.Sucess = "Het spoor is verwijderd.";

            return RedirectToAction("Index");
        }
        public ActionResult AddSector(LogistiekBeheerModel result)
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            var addResult = remise.Logic.AddSector(result.Form.TrackNumber, result.Form.Latitude, result.Form.Longitude);

            remise.Error = addResult;

            if (addResult == null)
                result.Sucess = "De sector is toegevoegd.";

            return RedirectToAction("Index");
        }
        public ActionResult DeleteSector(LogistiekBeheerModel result)
        {
            var remise = (LogistiekBeheerModel)Session["Remise"];
            if (remise == null) return RedirectToAction("Index");
            var sectorResult = remise.Logic.DeleteSector(result.Form.TrackNumber);
            remise.Error = sectorResult;
            if (sectorResult == null)
                remise.Sucess = "De sector is sucessvol verwijderd.";
            return RedirectToAction("Index");
        }
    }
}