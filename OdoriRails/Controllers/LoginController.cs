using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using OdoriRails.Helpers;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;
using OdoriRails.Models;

namespace OdoriRails.Controllers
{
    public class LoginController : BaseControllerFunctions
    {
        private readonly LoginRepository _repository = new LoginRepository();

        public ActionResult Index()
        {
            var user = Session["User"];
            if (user is User)
                return LogIn((user as User).Username, (user as User).Password, false, true);
            var conn = BaseRepository.TestConnection();
            if (conn != null)
            {
                var errorModel = new LoginModel {Error = "Kan niet met de database verbinden."};
                return View(errorModel);
            }
            var result = TrySignInWithCookies();
            if (result != null) return result;
            var model = TempData["SigninModel"] as LoginModel ?? new LoginModel();
            return View(model);
        }

        public ActionResult LogOut()
        {
            var cookie = Response.Cookies["UserSettings"];
            if (cookie != null)
                cookie.Expires = DateTime.Now.AddDays(-1);

            Session["User"] = null;

            var model = new LoginModel { Sucess = "U bent sucessvol uitgelogd." };
            TempData["SigninModel"] = model;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (model.Username == null && model.Password == null) return View(model);

            var conn = BaseRepository.TestConnection();
            if (conn != null)
            {
                var errorModel = new LoginModel {Error = "Kan niet met de database verbinden."};
                return View(errorModel);
            }

            if (model != null)
                return AttemptLogin(model.Username, model.Password, model.RememberMe);

            model.Warning = "Something weird happened wtf.";
            model = TempData["SigninModel"] as LoginModel ?? new LoginModel();

            return View(model);
        }

        private ActionResult TrySignInWithCookies()
        {
            var cookie = Request.Cookies["UserSettings"];

            if (cookie == null) return null;

            var password = cookie["Password"];
            var username = cookie["Username"];

            var result = _repository.ValidateUser(username, password);

            return result == 1 ? LogIn(username, password, true, true) : null;
        }

        private ActionResult AttemptLogin(string username, string password, bool rememberMe)
        {
            var result = _repository.ValidateUser(username, password);

            switch (result)
            {
                case 1:
                {
                    return LogIn(username, password, rememberMe);
                }
                default:
                {
                    var model = new LoginModel();

                    switch (result)
                    {
                        case -2:
                            model.Error = "This account doesn't exist.";
                            break;
                        case -1:
                            model.Error = "This username and password don't match.";
                            break;
                        default:
                            model.Error = "An uncaught login error has occured.";
                            break;
                    }
                    return Index(model);
                }
            }
        }
        
        private ActionResult LogIn(string username, string password, bool rememberme, bool isAutomatic = false)
        {
            if (!isAutomatic)
                if (rememberme)
                {
                    Response.Cookies["UserSettings"]["Username"] = username;
                    Response.Cookies["UserSettings"]["Password"] = password;

                    Response.Cookies["UserSettings"].Expires = DateTime.Now.AddDays(30);
                }
                else
                {
                    Response.Cookies["UserSettings"].Expires = DateTime.Now.AddDays(-1);
                }

            Session["User"] = _repository.FetchUser(username);

            //Adh van de user.role de juiste view teruggeven.

            switch (((User) Session["User"]).Role)
            {
                case Role.Administrator:
                    return RedirectToAction("Index", "UserBeheer");
                case Role.Logistic:
                    return RedirectToAction("Index", "LogistiekBeheersysteem");
                case Role.Driver:
                    return RedirectToAction("Index", "Driver");
                case Role.Cleaner:
                    return RedirectToAction("Index", "SR");
                case Role.Engineer:
                    return RedirectToAction("Index", "SR");
                case Role.HeadEngineer:
                    return RedirectToAction("Index", "SR");
                case Role.HeadCleaner:
                    return RedirectToAction("Index", "SR");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}