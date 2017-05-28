﻿using System.Collections.Generic;

namespace OdoriRails.Helpers.Objects
{
    public enum Role
    {
        Administrator,
        Logistic,
        Driver,
        Cleaner,
        Engineer,
        HeadEngineer,
        HeadCleaner
    }

    public class User
    {
        /// <summary>
        /// Database ID van de User.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Ophalen naam van User
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Ophalen emailadres van User
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Ophalen rol van User
        /// </summary>
        public Role Role { get; }

        /// <summary>
        /// Ophalen username van User
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Ophalen password van User
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Ophalen manager van User
        /// </summary>
        public string ManagerUsername { get; }

        public List<int> TramIds { get; protected set; }

        /// <summary>
        /// Toevoegen User, minimale hoeveelheid benodigde data.
        /// </summary>
        public User(string name, string email, Role role)
        {
            Name = name;
            Email = email;
            Role = role;
        }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Toevoegen User, alle benodigde data.
        /// </summary>
        public User(int id, string name, string username, string email, string password, Role role, string managedByUsername, List<int> tramIds = null)
        {
            Id = id;
            Name = name;
            Email = email;
            Role = role;
            Username = username;
            Password = password;
            ManagerUsername = managedByUsername;
            TramIds = tramIds ?? new List<int>();
        }

        public User(User oldUser)
        {
            if (oldUser == null) return;
            Id = oldUser.Id;
            Name = oldUser.Name;
            Email = oldUser.Email;
            Role = oldUser.Role;
            Username = oldUser.Username;
            Password = oldUser.Password;
            ManagerUsername = oldUser.ManagerUsername;
            TramIds = oldUser.TramIds;
        }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}
