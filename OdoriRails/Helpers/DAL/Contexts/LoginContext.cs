using System.Data.SqlClient;
using OdoriRails.Helpers.DAL.ContextInterfaces;

namespace OdoriRails.Helpers.DAL.Contexts
{
    public class LoginContext : ILoginContext
    {
        public bool ValidateUsername(string username)
        {
            var query = new SqlCommand("SELECT UserPk FROM [User] WHERE Username = @usrname");
            query.Parameters.AddWithValue("@usrname", username);
            return DatabaseHandler.GetData(query).Rows.Count != 0;
        }

        public bool MatchUsernameAndPassword(string username, string password)
        {
            var query = new SqlCommand("SELECT Password FROM [User] WHERE Username = @usrname");
            query.Parameters.AddWithValue("@usrname", username);

            var data = DatabaseHandler.GetData(query);

            return data.Rows.Count > 0 && (string) data.Rows[0][0] == password;
        }
    }
}