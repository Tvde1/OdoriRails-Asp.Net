using System.Data;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.DAL.ContextInterfaces
{
    public interface IUserContext
    {
        DataRow GetUser(string userName);

        DataRow GetUser(int userId);

        int GetUserId(string userName);

        User AddUser(User user);

        DataTable GetAllUsers();

        void RemoveUser(User user);

        void UpdateUser(User user);

        DataTable GetAllUsersWithFunction(Role role);
    }
}