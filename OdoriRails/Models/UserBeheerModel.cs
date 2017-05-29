using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OdoriRails.Controllers;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models
{
    public class UserBeheerModel : BaseModel
    {
        public enum SortMethods
        {
            All,
            Cleaners,
            Engineers,
            HeadCleaners,
            HeadEngineers,
            Drivers,
            Logistic,
            Administrators
        }

        private readonly UserBeheerRepository _repository = new UserBeheerRepository();

        private List<User> _allUsers;


        public UserBeheerModel()
        {
            UpdateUserList();
        }

        public List<User> Users { get; set; }
        public SortMethods SortMethod { get; set; }

        public void UpdateUserList()
        {
            _allUsers = _repository.GetAllUsers();

            switch (SortMethod)
            {
                case SortMethods.All:
                    Users = _allUsers;
                    break;
                case SortMethods.Cleaners:
                    Users = _allUsers.Where(x => x.Role == Role.Cleaner).ToList();
                    break;
                case SortMethods.Engineers:
                    Users = _allUsers.Where(x => x.Role == Role.Engineer).ToList();
                    break;
                case SortMethods.HeadCleaners:
                    Users = _allUsers.Where(x => x.Role == Role.HeadCleaner).ToList();
                    break;
                case SortMethods.HeadEngineers:
                    Users = _allUsers.Where(x => x.Role == Role.HeadEngineer).ToList();
                    break;
                case SortMethods.Drivers:
                    Users = _allUsers.Where(x => x.Role == Role.Driver).ToList();
                    break;
                case SortMethods.Logistic:
                    Users = _allUsers.Where(x => x.Role == Role.Logistic).ToList();
                    break;
                case SortMethods.Administrators:
                    Users = _allUsers.Where(x => x.Role == Role.Administrator).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void DeleteUser(int delIndex)
        {
            _repository.RemoveUser(delIndex);
        }
    }









    public class EditUserModel : BaseModel
    {
        private readonly UserBeheerRepository _repository = new UserBeheerRepository();

        public EditUserModel()
        {
            _oldUser = null;
            EditUser = new User(null);
        }

        public EditUserModel(User user)
        {
            _oldUser = user;
            EditUser = new User(_oldUser);
        }

        private readonly User _oldUser;
        public User EditUser { get; set; }

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

            if (_oldUser == null)
                _repository.AddUser(EditUser);
            else
                _repository.UpdateUser(EditUser);
            return null;
        }
    }
}