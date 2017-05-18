using System;

namespace OdoriRails.Helpers.DAL
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message)
        {
        }
    }
}