using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace OdoriRails.Helpers.DAL
{
    public class DatabaseHandler
    {
        //private const string ConnectionString = @"Data Source=192.168.20.189;Initial Catalog=OdoriRails;User ID=sa;Password=OdoriRails123;";

        private string _connectionString;

        public DatabaseHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public  DataTable GetData(SqlCommand command)
        {
            try
            {
                var dataTable = new DataTable();
                using (var conn = new SqlConnection(_connectionString))
                {
                    command.Connection = conn;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
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