using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

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
        ///     Toevoegen User, minimale hoeveelheid benodigde data.
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
        ///     Toevoegen User, alle benodigde data.
        /// </summary>
        public User(int id, string name, string username, string email, string password, Role role,
            string managedByUsername, int? tramId = null)
        {
            Id = id;
            Name = name;
            Email = email;
            Role = role;
            Username = username;
            Password = password;
            ManagerUsername = managedByUsername;
            TramId = tramId;
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
            TramId = oldUser.TramId;
        }

        /// <summary>
        ///     Database ID van de User.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Ophalen naam van User
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Ophalen emailadres van User
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     Ophalen rol van User
        /// </summary>
        [Required]
        public Role Role { get; set; }

        /// <summary>
        ///     Ophalen username van User
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        ///     Ophalen password van User
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        ///     Ophalen manager van User
        /// </summary>
        public string ManagerUsername { get; set; }

        [IntegerValidator]
        public int? TramId { get; set; }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}