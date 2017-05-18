using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OdoriRails.Helpers;
using OdoriRails.Models;

namespace OdoriRails.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            var user = new User{FirstName = "Jan"};
            var model = new LoginModel{User =  user};

            return View(model);
        }
    }
}