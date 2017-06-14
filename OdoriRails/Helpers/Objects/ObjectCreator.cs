using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.DAL.Contexts;

namespace OdoriRails.Helpers.Objects
{
    public class ObjectCreator
    {
        private readonly IServiceContext _serviceContext;
        private readonly ITramContext _tramContext;
        private readonly IUserContext _userContext;

        public ObjectCreator()
        {
            _userContext = new UserContext();
            _tramContext = new TramContext();
            _serviceContext = new ServiceContext();
        }

        public User CreateUser(DataRow row)
        {
            if (row == null) return null;
            var array = row.ItemArray;
            //name gebr wachtw email rol 
            var parentUserString = array[6] == DBNull.Value
                ? ""
                : CreateUser(_userContext.GetUser((int)array[6])).Username;

            var tramRow = _tramContext.GetTramIdByDriverId((int)array[0]);
            var tramId = (int?)tramRow?["TramPk"];

            return new User((int)row["UserPk"], (string)row["Name"], (string)row["Username"], (string)row["Email"], (string)array[3],
                (Role)(int)array[5], parentUserString, tramId);
        }

        public static Track CreateTrack(DataRow row)
        {
            return row == null ? null : new Track((int)row["TrackPk"], (int)row["Line"], (TrackType)row["Type"]);
        }

        public Sector CreateSector(DataRow row)
        {
            if (row == null) return null;
            var array = row.ItemArray;
            var occupyingTramNumber = row["TramFk"] == DBNull.Value ? null : (int?)row["TramFk"];
            var latitude = (decimal)row["Lat"];
            var longitude = (decimal)row["Long"];
            return new Sector((int)array[0], (int)array[2], (SectorStatus)array[1], occupyingTramNumber, latitude, longitude);
        }

        public Tram CreateTram(DataRow row)
        {
            if (row == null) return null;
            var id = (int)row["TramPk"];
            var line = (int) row["Line"];
            var status = (TramStatus)row["Status"];
            var driver = row["DriverFk"] == DBNull.Value ? null : CreateUser(_userContext.GetUser((int)row["DriverFk"]));
            var model = (TramModel)row["ModelFk"];
            var location = (TramLocation)row["Location"];
            DateTime? depart = null;
            if (row["DepartureTime"] != DBNull.Value) depart = (DateTime)row["DepartureTime"];

            return new Tram(id, status, line, driver, model, location, depart);
        }

        public Cleaning CreateCleaning(DataRow row)
        {
            var array = row.ItemArray;
            var service = _serviceContext.GetServiceById((int)array[0]);

            var id = (int)service[0];
            var startDate = (DateTime)service[1];
            var endDate = service[2] == DBNull.Value ? (DateTime?)null : (DateTime)service[2];
            var tramId = (int)service[3];

            var type = (CleaningSize)row["Size"];
            var comments = (string)row["Remarks"];
            var users = GenerateListWithFunction(_serviceContext.GetUsersInServiceById((int)service[0]), CreateUser);

            return new Cleaning(id, startDate, endDate, type, comments, users, tramId);
        }

        public Repair CreateRepair(DataRow row)
        {
            var id = (int)row["ServiceFk"];
            var startDate = (DateTime)row["StartDate"];
            var endDate = row["EndDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["EndDate"];
            var tramId = (int)row["TramFk"];
            var solution = row["Solution"] == DBNull.Value ? "" : (string)row["Solution"];
            var defect = (string)row["Defect"];
            var type = (RepairType)row["Type"];
            var users = GenerateListWithFunction(_serviceContext.GetUsersInServiceById((int)row["ServiceFk"]), CreateUser);

            return new Repair(id, startDate, endDate, type, defect, solution, users, tramId);
        }

        public static List<T> GenerateListWithFunction<T>(DataTable data, Func<DataRow, T> func)
        {
            return (from DataRow row in data.Rows select func(row)).ToList();
        }
    }
}