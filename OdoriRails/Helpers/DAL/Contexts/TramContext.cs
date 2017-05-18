using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using OdoriRails.BaseClasses;
using OdoriRails.Helpers.DAL.ContextInterfaces;

namespace OdoriRails.Helpers.DAL.Contexts
{
    public class _tramContext : ITramContext
    {
        private readonly DatabaseHandler _databaseHandler;
        private readonly _userContext _userContext;
        private readonly TrackSectorContext _trackSectorContext;
        public _tramContext(DatabaseHandler databaseHandler)
        {
            _databaseHandler = databaseHandler;
            _userContext = new _userContext(_databaseHandler);
            _trackSectorContext = new TrackSectorContext(_databaseHandler);
        }


        public DataRow GetTram(int tramId)
        {
            var data =  _databaseHandler.GetData(new SqlCommand($"SELECT * FROM Tram WHERE TramPk = {tramId}"));
            return data.Rows.Count == 0 ? null : data.Rows[0];
        }

        public void AddTram(Tram tram)
        {
            var query = new SqlCommand("INSERT INTO Tram (TramPk,Line,Status,DriverFk,ModelFk,RemiseFk,Location,DepartureTime) VALUES(@id,@line,@status,@driver,@model,@remise,@location,@dep); SELECT SCOPE_IDENTITY();");
            query.Parameters.AddWithValue("@id", tram.Number);
            query.Parameters.AddWithValue("@line", tram.Line);
            query.Parameters.AddWithValue("@status", (int)tram.Status);
            query.Parameters.AddWithValue("@model", (int)tram.Model);
            query.Parameters.AddWithValue("@location", (int)tram.Location);
            if (tram.DepartureTime == null) query.Parameters.AddWithValue("@dep", DBNull.Value);
            else query.Parameters.AddWithValue("@dep", tram.DepartureTime);
            if (tram.Driver != null) query.Parameters.AddWithValue("@driver", _userContext.GetUserId(tram.Driver.Username));
            else query.Parameters.AddWithValue("@driver", DBNull.Value);
            query.Parameters.AddWithValue("@remise", 1);

            _databaseHandler.GetData(query);
        }

        public void RemoveTram(Tram tram)
        {
            _databaseHandler.GetData(new SqlCommand($"DELETE FROM Tram WHERE TramPk = {tram.Number}"));
        }

        public DataTable GetAllTrams()
        {
            return _databaseHandler.GetData(new SqlCommand("SELECT * FROM Tram"));
        }

        public DataTable GetTramsByDriver(User driver)
        {
            return _databaseHandler.GetData(new SqlCommand($"SELECT * FROM Tram WHERE DriverFk = {driver.Id}"));
        }

        public void EditTram(Tram tram)
        {
            //line status driver model remise location departure 
            var query = new SqlCommand("UPDATE Tram SET Line = @line, Status = @stat, DriverFk = @driver, ModelFk = @model, RemiseFk = @remis, Location = @loc, DepartureTime = @dep WHERE TramPk = @id");
            query.Parameters.AddWithValue("@line", tram.Line);
            query.Parameters.AddWithValue("@stat", (int)tram.Status);
            if (tram.Driver != null) query.Parameters.AddWithValue("@driver", _userContext.GetUserId(tram.Driver.Username));
            else query.Parameters.AddWithValue("@driver", DBNull.Value);
            query.Parameters.AddWithValue("@model", (int)tram.Model);
            query.Parameters.AddWithValue("@remis", 1); //TODO: Correct updaten.
            if (tram.DepartureTime == null) query.Parameters.AddWithValue("@dep", DBNull.Value);
            else query.Parameters.AddWithValue("@dep", tram.DepartureTime);
            query.Parameters.AddWithValue("@loc", (int)tram.Location);
            query.Parameters.AddWithValue("@id", tram.Number);
            _databaseHandler.GetData(query);
        }

        public DataTable GetAllTramsWithStatus(TramStatus status)
        {
            return _databaseHandler.GetData(new SqlCommand($"SELECT * FROM Tram WHERE Status = {(int)status}"));
        }

        public DataTable GetAllTramsWithLocation(TramLocation location)
        {
            return _databaseHandler.GetData(new SqlCommand($"SELECT * FROM Tram WHERE Location = {(int)location}"));
        }

        public DataRow GetAssignedSector(Tram tram)
        {
            var data = _databaseHandler.GetData(new SqlCommand($"SELECT * FROM Sector WHERE TramFk = {tram.Number}"));
            return data.Rows.Count == 0 ? null : data.Rows[0];
        }

        public void WipeDepartureTimes()
        {
            _databaseHandler.GetData(new SqlCommand("UPDATE Tram SET DepartureTime = null"));
        }

        public DataRow FetchTram(Tram tram)
        {
            var data = _databaseHandler.GetData(new SqlCommand($"SELECT * FROM Tram WHERE TramPk = {tram.Number}"));
            return data.Rows.Count == 0 ? null : data.Rows[0];
        }

        public bool DoesTramExist(int id)
        {
            return _databaseHandler.GetData(new SqlCommand($"SELECT * FROM Tram WHERE TramPk = {id}")).Rows.Count > 0;
        }

        public void SetUserToTram(Tram tram, User user)
        {
            if (tram == null) return;
            _databaseHandler.GetData(new SqlCommand($"UPDATE Tram SET DriverFk = {user?.Id.ToString() ?? "null"} WHERE TramPk = {tram.Number}"));
        }

        public DataTable GetTramIdByDriverId(int driverId)
        {
            return _databaseHandler.GetData(new SqlCommand($"SELECT TramPk FROM Tram WHERE DriverFk = {driverId}"));
        }

        public void SetStatusToIdle(int tramId)
        {
            _databaseHandler.GetData(new SqlCommand($"UPDATE Tram SET Status = 0 WHERE TramPk = {tramId}"));
        }
    }
}
