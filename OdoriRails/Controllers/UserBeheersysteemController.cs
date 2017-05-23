using OdoriRails.Helpers.UserBeheersysteem;
using OdoriRails.Models;
using System.Web.Mvc;
using OdoriRails.Helpers;

namespace OdoriRails.Controllers
{
    public class UserBeheersysteemController : BaseControllerFunctions
    {
        //Naar ronald: Deze kun je beter in ieder ActionResult zelf aanmaken.
        UserBeheerLogic logic = new UserBeheerLogic();
        UserBeheerSysteemModel model = new UserBeheerSysteemModel();

        // GET: UserBeheersysteem
        public ActionResult Index()//Show all users
        {
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            //model.users = logic.GetAllUsersFromDatabase();
            return View();
        }

      //  public ActionResult Index(int index)//Show all users of a certain type
      //  {
         //   model.users = logic.GetSelectUsersFromDatabase(index);
         //   return View(model);
      //  }
    }
}