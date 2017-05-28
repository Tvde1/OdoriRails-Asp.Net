using OdoriRails.Helpers.UserBeheersysteem;
using OdoriRails.Models;
using System.Web.Mvc;
using OdoriRails.Helpers;
using OdoriRails.Helpers.DAL.Repository;

namespace OdoriRails.Controllers
{
    public class UserBeheersysteemController : BaseControllerFunctions
    {
        // GET: UserBeheersysteem
        public ActionResult Index()//Show all users
        {
            var user = GetLoggedInUser();
            if (user == null) return NotLoggedIn();

            var model = new UserBeheerModel();

            return View(model);
        }

        //Show all users of a certain type
        public ActionResult Index(UserBeheerModel model)
        {
            model.UpdateUserList();
            return View(model);
        }

        //Edit User
        public ActionResult Edit(EditUserModel model)
        {
            var repo = new UserBeheerRepository();

            var tramIdResult = -1;
            if (string.IsNullOrEmpty(model.EditUser.Username))
            {
                model.Error = "De username mag niet leeg zijn.";
                return View(model);
            }
            if (repo.DoesUserExist(model.EditUser.Username) && repo.GetUserId(model.User.Username) != model.EditUser.Id)
            {
                model.Error = "Deze username is al in gebruik.";
                return View(model);
            }
            //if (tramId != "" && int.TryParse(tramId, out tramIdResult) && !repo.DoesTramExist(tramIdResult))
            //{
            //    //MessageBox.Show("Deze tram bestaat niet.");
            //    return false;
            //}
            if (string.IsNullOrEmpty(model.EditUser.Password))
            {
                model.Error = "Het wachtwoord kan niet leeg zijn.";
                return View(model);
            }

            if (model.OldUser == null)
                repo.AddUser(model.EditUser);
            else
                repo.UpdateUser(model.EditUser);
            
            var newModel = new UserBeheerModel { Sucess = "De gebruiker is aangemaakt/veranderd." };

            return RedirectToAction("Index", model);

        }
    }
}