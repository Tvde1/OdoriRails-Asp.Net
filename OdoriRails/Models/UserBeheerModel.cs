using System;
using OdoriRails.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

        private List<User> _allUsers;
        private readonly UserBeheerRepository _repository = new UserBeheerRepository();

        public List<User> Users { get; set; }
        public SortMethods SortMethod { get; set; }


        public UserBeheerModel()
        {
            UpdateUserList();
        }

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
        private UserBeheerRepository _repository = new UserBeheerRepository();

        public User OldUser { get; set; }
        public User EditUser { get; set; }

        public EditUserModel()
        {
            EditUser = new User(OldUser);
        }

        public IReadOnlyCollection<User> AllUsers => _repository.GetAllUsers();

        public int GetIndex(string username)
        {
            return _repository.GetUserId(username);
        }
        
        private void SaveNewUser()
        {
            throw new NotImplementedException();
        }

        private void UpdateUser(User user)
        {
            //TODO: Fix this.

            int tramIdResult;
            //if (tramId == null) _repository.SetUserToTram(user, null);
            //if (int.TryParse(tramId, out tramIdResult)) _repository.SetUserToTram(user, tramIdResult);
            _repository.UpdateUser(user);
        }

        public void AddUserAndAssignTram(User user, string tramId)
        {
            var newUser = _repository.AddUser(user);

            int tramIdResult;
            if (tramId == null) _repository.SetUserToTram(newUser, null);
            if (int.TryParse(tramId, out tramIdResult)) _repository.SetUserToTram(newUser, tramIdResult);
        }
    }
}