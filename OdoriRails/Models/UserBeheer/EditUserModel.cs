using System.Collections.Generic;
using System.Web.Mvc;
using OdoriRails.Controllers;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models.UserBeheer
{
    public class EditUserModel : BaseModel
    {
        private readonly UserBeheerRepository _repository = new UserBeheerRepository();
        public bool IsNewUser { get; set; } = true;

        public EditUserModel()
        {
            
        }

        public EditUserModel(User user)
        {
            IsNewUser = user == null;
            EditUser = new User(user);
        }

        public User EditUser { get; set; } = new User(null);

        public IEnumerable<User> AllUsers => _repository.GetAllUsers();

        public int? GetUserId(string username)
        {
            return _repository.GetUserIdByFullName(username);
        }

        public ActionResult Save(UserBeheerController controller)
        {
            var existingUser = _repository.GetUserId(EditUser.Username);

            if (string.IsNullOrEmpty(EditUser.Username))
            {
                Error = "De username mag niet leeg zijn.";
                controller.TempData["EditModel"] = this;
                return new RedirectResult("Edit");
            }
            if (_repository.DoesUserExist(EditUser.Username) && existingUser != EditUser.Id)
            {
                Error = "Deze username is al in gebruik.";
                controller.TempData["EditModel"] = this;
                return new RedirectResult("Edit");
            }
            if (EditUser.TramId != null && !_repository.DoesTramExist(EditUser.TramId.Value))
            {
                Error = "Deze tram bestaat niet.";
                controller.TempData["EditModel"] = this;
                return new RedirectResult("Edit");
            }
            if (string.IsNullOrEmpty(EditUser.Password))
            {
                Error = "Het wachtwoord kan niet leeg zijn.";
                controller.TempData["EditModel"] = this;
                return new RedirectResult("Edit");
            }

            if (IsNewUser)
                _repository.AddUser(EditUser);
            else
                _repository.UpdateUser(EditUser);
            return null;
        }
    }
}