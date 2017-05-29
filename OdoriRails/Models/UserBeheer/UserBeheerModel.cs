using System;
using System.Collections.Generic;
using System.Linq;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models.UserBeheer
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
            UpdateUserList();
        }
    }
}