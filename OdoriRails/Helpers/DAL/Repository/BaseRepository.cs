using System;
using System.Data.SqlClient;

namespace OdoriRails.Helpers.DAL.Repository
{
    public abstract class BaseRepository
    {
        private const string ConnectionString =
            "Data Source=192.168.20.189;Initial Catalog=OdoriRails;User ID=sa;Password=OdoriRails123;Connect Timeout=1";

        public static Exception TestConnection()
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open(); // throws if connection is invalid
                    conn.Close();
                    return null;
                }
            }
            catch
            {
                return new DatabaseException();
            }
        }
    }
}