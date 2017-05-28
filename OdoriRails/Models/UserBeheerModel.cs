using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
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
            if (_allUsers == null)
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
            _repository.RemoveUser(Users[delIndex]);
        }
    }

    public class EditUserModel : BaseModel
    {
        private readonly UserBeheerRepository _repository = new UserBeheerRepository();

        public EditUserModel() : this(null, null)
        {
        }

        public EditUserModel(UserBeheerModel model, User user)
        {
            CopyBaseModel(model);

            OldUser = user;
            EditUser = new User(OldUser);
        }

        public User OldUser { get; set; }
        public User EditUser { get; set; }

        public IReadOnlyCollection<User> AllUsers => _repository.GetAllUsers();

        public int GetIndex(string username)
        {
            return _repository.GetUserId(username);
        }

        public ActionResult Save()
        {
            if (string.IsNullOrEmpty(EditUser.Username))
            {
                Error = "De username mag niet leeg zijn.";
                return new RedirectToRouteResult("Edit", new RouteValueDictionary(this));
            }
            if (_repository.DoesUserExist(EditUser.Username) && _repository.GetUserId(User.Username) != EditUser.Id)
            {
                Error = "Deze username is al in gebruik.";
                return new RedirectToRouteResult("Edit", new RouteValueDictionary(this));
            }
            if (EditUser.TramIds.Any(x => !_repository.DoesTramExist(x)))
            {
                Error = "Een van de gekozen tramnummers bestaat niet.";
                return new RedirectToRouteResult("Edit", new RouteValueDictionary(this));
            }
            if (string.IsNullOrEmpty(EditUser.Password))
            {
                Error = "Het wachtwoord kan niet leeg zijn.";
                return new RedirectToRouteResult("Edit", new RouteValueDictionary(this));
            }

            if (OldUser == null)
                _repository.AddUser(EditUser);
            else
                _repository.UpdateUser(EditUser);
            return null;
        }
    }
}