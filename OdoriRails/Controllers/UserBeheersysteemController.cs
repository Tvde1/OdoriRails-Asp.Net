using OdoriRails.Helpers.UserBeheersysteem;
using OdoriRails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OdoriRails.Controllers
{
    public class UserBeheersysteemController : Controller
    {
        UserBeheerLogic logic = new UserBeheerLogic();
        UserBeheerSysteemModel model = new UserBeheerSysteemModel();

        // GET: UserBeheersysteem
        public ActionResult Index()//Show all users
        {
            model.users = logic.GetAllUsersFromDatabase();
            return View(model);
        }

        public ActionResult Index(int index)//Show all users of a certain type
        {
            model.users = logic.GetSelectUsersFromDatabase(index);
            return View(model);
        }
    }
}