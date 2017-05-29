using System.Data;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.DAL.ContextInterfaces
{
    public interface IUserContext
    {
        DataRow GetUser(string userName);

        DataRow GetUser(int userId);

        DataRow GetUserId(string userName);

        int? GetUserIdByName(string name);

        User AddUser(User user);

        DataTable GetAllUsers();

        void RemoveUser(int userId);

        void UpdateUser(User user);

        DataTable GetAllUsersWithFunction(Role role);
    }
}