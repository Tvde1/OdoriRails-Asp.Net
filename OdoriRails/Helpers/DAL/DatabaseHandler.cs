using System;
using System.Data;
using System.Data.SqlClient;

namespace OdoriRails.Helpers.DAL
{
    public class DatabaseHandler
    {
        private const string ConnectionString =
            @"Data Source=192.168.20.189;Initial Catalog=OdoriRails;User ID=sa;Password=OdoriRails123;";

        public static DataTable GetData(SqlCommand command)
        {
            try
            {
                var dataTable = new DataTable();
                using (var conn = new SqlConnection(ConnectionString))
                {
                    command.Connection = conn;
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Lost connection to the database.");
                //return null;
                throw new DatabaseException("Something went wrong while communicating with the database.");
            }
        }
    }
}