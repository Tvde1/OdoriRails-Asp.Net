using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OdoriRails.Helpers.DAL.Repository
{
    public abstract class BaseRepository
    {
        private static readonly string ConnectionString = "xd";
        public DatabaseHandler DatabaseHandler { get; }

        public BaseRepository()
        {
            DatabaseHandler = new DatabaseHandler(ConnectionString);
        }

        public Exception TestConnection()
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open(); // throws if connection string is invalid
                    conn.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}