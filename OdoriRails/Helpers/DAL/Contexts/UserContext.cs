﻿using System;
using System.Data;
using System.Data.SqlClient;
using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.DAL.Contexts
{
    public class UserContext : IUserContext
    {
        private static TramContext _tramContext = new TramContext();


        public User AddUser(User user)
        {
            var query = new SqlCommand(
                "INSERT INTO [User] (Username,Password,Email,Name,Role,ManagedBy) VALUES (@username,@pass,@email,@name,@role,@managedBy); SELECT SCOPE_IDENTITY();");
            query.Parameters.AddWithValue("@name", user.Name);
            query.Parameters.AddWithValue("@username", user.Username);
            query.Parameters.AddWithValue("@pass", user.Password);
            query.Parameters.AddWithValue("@email", user.Email);
            query.Parameters.AddWithValue("@role", (int) user.Role);

            if (string.IsNullOrEmpty(user.ManagerUsername)) query.Parameters.AddWithValue("@managedBy", DBNull.Value);
            else query.Parameters.AddWithValue("@managedBy", GetUserId(user.ManagerUsername));

            user.SetId(Convert.ToInt32((decimal) DatabaseHandler.GetData(query).Rows[0][0]));
            return user;
        }

        public DataTable GetAllUsers()
        {
            return DatabaseHandler.GetData(new SqlCommand("SELECT * FROM [User]"));
        }

        public void RemoveUser(User user)
        {
            DatabaseHandler.GetData(new SqlCommand($"UPDATE [User] SET ManagedBy = null WHERE ManagedBy = {user.Id}"));
            DatabaseHandler.GetData(new SqlCommand($"DELETE FROM [User] WHERE UserPk = {user.Id}"));
        }

        public void UpdateUser(User user)
        {
            var query = new SqlCommand(
                "UPDATE [User] SET Name = @name, Username = @username, Password = @password, Email = @email, Role = @role, ManagedBy = @managedby WHERE UserPk = @id");
            query.Parameters.AddWithValue("@username", user.Username);
            query.Parameters.AddWithValue("@name", user.Name);
            query.Parameters.AddWithValue("@password", user.Password);
            query.Parameters.AddWithValue("@email", user.Email);
            query.Parameters.AddWithValue("@role", (int) user.Role);
            if (string.IsNullOrEmpty(user.ManagerUsername)) query.Parameters.AddWithValue("@managedby", DBNull.Value);
            else query.Parameters.AddWithValue("@managedby", GetUserId(user.ManagerUsername));
            query.Parameters.AddWithValue("@id", user.Id);
            DatabaseHandler.GetData(query);
        }

        public DataTable GetAllUsersWithFunction(Role role)
        {
            return DatabaseHandler.GetData(new SqlCommand($"SELECT * FROM [User] WHERE Role = {(int) role}"));
        }

        public DataRow GetUser(int id)
        {
            var data = DatabaseHandler.GetData(new SqlCommand($"SELECT * FROM [User] WHERE UserPk = {id}"));
            return data.Rows.Count == 0 ? null : data.Rows[0];
        }

        public DataRow GetUser(string userName)
        {
            var command = new SqlCommand("SELECT * FROM [User] WHERE UserPk = @id");
            command.Parameters.AddWithValue("@id", GetUserId(userName));
            var data = DatabaseHandler.GetData(command);
            return data.Rows.Count == 0 ? null : data.Rows[0];
        }

        public int GetUserId(string username)
        {
            var query = new SqlCommand("SELECT UserPk FROM [User] WHERE Username = @username");
            query.Parameters.AddWithValue("@username", username);
            return (int) DatabaseHandler.GetData(query).Rows[0].ItemArray[0];
        }
    }
}