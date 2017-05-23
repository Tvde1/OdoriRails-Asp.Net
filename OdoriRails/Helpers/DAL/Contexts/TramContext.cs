using System;
using System.Data;
using System.Data.SqlClient;
using OdoriRails.BaseClasses;
using OdoriRails.Helpers.DAL.ContextInterfaces;

namespace OdoriRails.Helpers.DAL.Contexts
{
    public class TramContext : ITramContext
    {
        private static readonly UserContext _userContext = new UserContext();
        private static readonly TrackSectorContext _trackSectorContext;

        public DataRow GetTram(int tramId)
        {
            var data = DatabaseHandler.GetData(new SqlCommand($"SELECT * FROM Tram WHERE TramPk = {tramId}"));
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

            DatabaseHandler.GetData(query);
        }

        public void RemoveTram(Tram tram)
        {
            DatabaseHandler.GetData(new SqlCommand($"DELETE FROM Tram WHERE TramPk = {tram.Number}"));
        }

        public DataTable GetAllTrams()
        {
            return DatabaseHandler.GetData(new SqlCommand("SELECT * FROM Tram"));
        }

        public DataTable GetTramsByDriver(User driver)
        {
            return DatabaseHandler.GetData(new SqlCommand($"SELECT * FROM Tram WHERE DriverFk = {driver.Id}"));
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
            DatabaseHandler.GetData(query);
        }

        public DataTable GetAllTramsWithStatus(TramStatus status)
        {
            return DatabaseHandler.GetData(new SqlCommand($"SELECT * FROM Tram WHERE Status = {(int)status}"));
        }

        public DataTable GetAllTramsWithLocation(TramLocation location)
        {
            return DatabaseHandler.GetData(new SqlCommand($"SELECT * FROM Tram WHERE Location = {(int)location}"));
        }

        public DataRow GetAssignedSector(Tram tram)
        {
            var data = DatabaseHandler.GetData(new SqlCommand($"SELECT * FROM Sector WHERE TramFk = {tram.Number}"));
            return data.Rows.Count == 0 ? null : data.Rows[0];
        }

        public void WipeDepartureTimes()
        {
            DatabaseHandler.GetData(new SqlCommand("UPDATE Tram SET DepartureTime = null"));
        }

        public DataRow FetchTram(Tram tram)
        {
            var data = DatabaseHandler.GetData(new SqlCommand($"SELECT * FROM Tram WHERE TramPk = {tram.Number}"));
            return data.Rows.Count == 0 ? null : data.Rows[0];
        }

        public bool DoesTramExist(int id)
        {
            return DatabaseHandler.GetData(new SqlCommand($"SELECT * FROM Tram WHERE TramPk = {id}")).Rows.Count > 0;
        }

        public void SetUserToTram(Tram tram, User user)
        {
            if (tram == null) return;
            DatabaseHandler.GetData(new SqlCommand($"UPDATE Tram SET DriverFk = {user?.Id.ToString() ?? "null"} WHERE TramPk = {tram.Number}"));
        }

        public DataTable GetTramIdByDriverId(int driverId)
        {
            return DatabaseHandler.GetData(new SqlCommand($"SELECT TramPk FROM Tram WHERE DriverFk = {driverId}"));
        }

        public void SetStatusToIdle(int tramId)
        {
            DatabaseHandler.GetData(new SqlCommand($"UPDATE Tram SET Status = 0 WHERE TramPk = {tramId}"));
        }
    }
}
