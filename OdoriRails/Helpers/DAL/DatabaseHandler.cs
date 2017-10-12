using System;
using System.Data;
using System.Data.SqlClient;

namespace OdoriRails.Helpers.DAL
{
    public static class DatabaseHandler
    {
        private const string ConnectionString =
                @"Data Source=192.168.20.189;Initial Catalog=OdoriRails;User ID=sa;Password=OdoriRails123;Connect Timeout=10;";

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
                throw new DatabaseException();
                //var newCommand = command;
                //return null;
            }
        }
    }
}
