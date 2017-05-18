﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OdoriRails.BaseClasses;
using OdoriRails.Helpers.DAL;
using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.DAL.Contexts;

namespace OdoriRails.Helpers
{
    public class ObjectCreator
    {
        private readonly IUserContext _userContext;
        private readonly ITramContext _tramContext;
        private readonly IServiceContext _serviceContext;

        public ObjectCreator(DatabaseHandler databaseHandler)
        {
            _userContext = new UserContext(databaseHandler);
            _tramContext = new TramContext(databaseHandler);
            _serviceContext = new ServiceContext(databaseHandler);
        }

        public User CreateUser(DataRow row)
        {
            var array = row.ItemArray;
            //name gebr wachtw email rol 
            var parentUserString = array[6] == DBNull.Value ? "" : CreateUser(_userContext.GetUser((int)array[6])).Username;
            var tramList = GenerateListWithFunction(_tramContext.GetTramIdByDriverId((int)array[0]), dataRow => dataRow.ItemArray[0]);

            var tram = tramList.Count > 0 ? (int?)tramList[0] : null;

            return new User((int)array[0], (string)array[1], (string)array[2], (string)array[4], (string)array[3], (Role)(int)array[5], parentUserString, tram);
        }

        public static Track CreateTrack(DataRow row)
        {
            var array = row.ItemArray;
            return new Track((int)array[0], (int)array[1], (TrackType)array[2]);
        }

        public Sector CreateSector(DataRow row)
        {
            var array = row.ItemArray;
            Tram tram = null;
            if (array[3] != DBNull.Value) tram = CreateTram(_tramContext.GetTram((int)array[3]));

            return new Sector((int)array[0], (int)array[2], (SectorStatus)array[1], tram);
        }

        public Tram CreateTram(DataRow row)
        {
            //Pk, Line, Status, Driver, Model, Remise, Location, Depart
            var array = row.ItemArray;
            var id = (int)array[0];
            var line = (int)array[1];
            var status = (TramStatus)array[2];
            var driver = array[3] == DBNull.Value ? null : CreateUser(_userContext.GetUser((int)array[3]));
            var model = (TramModel)array[4];
            var location = (TramLocation)array[6];
            DateTime? depart = null;
            if (array[7] != DBNull.Value) depart = (DateTime)array[7];

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

            var type = (CleaningSize)array[1];
            var comments = (string)array[2];
            var users = GenerateListWithFunction(_serviceContext.GetUsersInServiceById((int)service[0]), CreateUser);

            return new Cleaning(id, startDate, endDate, type, comments, users, tramId);
        }

        public Repair CreateRepair(DataRow row)
        {
            var array = row.ItemArray;
            var service = _serviceContext.GetServiceById((int)array[0]);

            var id = (int)service[0];
            var startDate = (DateTime)service[1];
            var endDate = service[2] == DBNull.Value ? (DateTime?)null : (DateTime)service[2];
            var tramId = (int)service[3];

            var solution = (string)array[1];
            var defect = (string)array[2];
            var type = (RepairType)array[3];
            var users = GenerateListWithFunction(_serviceContext.GetUsersInServiceById((int)service[0]), CreateUser);

            return new Repair(id, startDate, endDate, type, defect, solution, users, tramId);
        }

        public static List<T> GenerateListWithFunction<T>(DataTable data, Func<DataRow, T> func)
        {
            return (from DataRow row in data.Rows select func(row)).ToList();
        }
    }
}