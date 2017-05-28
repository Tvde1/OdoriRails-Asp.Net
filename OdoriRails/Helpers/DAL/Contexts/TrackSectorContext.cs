using System;
using System.Data;
using System.Data.SqlClient;
using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.DAL.Contexts
{
    public class TrackSectorContext : ITrackSectorContext
    {
        private const int RemiseNumber = 1;

        public DataTable GetAllTracks()
        {
            var trackQuery = new SqlCommand($"SELECT * FROM Track WHERE RemiseFk = {RemiseNumber}");
            return DatabaseHandler.GetData(trackQuery);
        }

        public DataTable GetAllSectors()
        {
            var sectorQuery = new SqlCommand($"SELECT * FROM Sector WHERE RemiseFk = {RemiseNumber}");
            return DatabaseHandler.GetData(sectorQuery);
        }

        public void AddTrack(Track track)
        {
            var query = new SqlCommand(
                "INSERT INTO Track (TrackPk, Line, [Type], RemiseFK) VALUES (@id, @line, @type, @remise)");
            query.Parameters.AddWithValue("@id", track.Number);
            if (track.Line == null) query.Parameters.AddWithValue("@line", DBNull.Value);
            else query.Parameters.AddWithValue("@line", track.Line);
            query.Parameters.AddWithValue("@type", (int) track.Type);
            query.Parameters.AddWithValue("@remise", 1);
            DatabaseHandler.GetData(query);

            foreach (var sector in track.Sectors)
                AddSector(sector, track);
        }

        public void EditTrack(Track track)
        {
            var query = new SqlCommand(
                "UPDATE Track SET Line = @line, Type = @type, RemiseFk = @remise WHERE TrackPk = @id");
            query.Parameters.AddWithValue("@line", track.Line);
            query.Parameters.AddWithValue("@type", (int) track.Type);
            query.Parameters.AddWithValue("@remise", RemiseNumber);
            query.Parameters.AddWithValue("@id", track.Number);
            DatabaseHandler.GetData(query);

            foreach (var sector in track.Sectors)
                EditSector(sector);
        }

        public void DeleteTrack(Track track)
        {
            var query = new SqlCommand("DELETE FROM Track WHERE TrackPK = @track");
            query.Parameters.AddWithValue("@track", track.Number);
            DatabaseHandler.GetData(query);
        }

        public void AddSector(Sector sector, Track track)
        {
            var query = new SqlCommand(
                "INSERT INTO Sector (SectorPk, TrackFk, RemiseFK) VALUES (@id, @track, @remise)");
            query.Parameters.AddWithValue("@id", sector.Number);
            query.Parameters.AddWithValue("@track", track.Number);
            query.Parameters.AddWithValue("@remise", 1);
            DatabaseHandler.GetData(query);
        }

        public void EditSector(Sector sector)
        {
            var query = new SqlCommand(
                "UPDATE Sector SET Status = @stat, TramFk = @tram, RemiseFk = @remis WHERE SectorPk = @id AND TrackFk = @track");
            query.Parameters.AddWithValue("@stat", (int) sector.Status);
            query.Parameters.AddWithValue("@track", sector.TrackNumber);
            if (sector.OccupyingTram != null) query.Parameters.AddWithValue("@tram", sector.OccupyingTram.Number);
            else query.Parameters.AddWithValue("@tram", DBNull.Value);
            query.Parameters.AddWithValue("@remis", RemiseNumber);
            query.Parameters.AddWithValue("@id", sector.Number);
            DatabaseHandler.GetData(query);
        }

        public void DeleteSectorFromTrack(Track track, Sector sector)
        {
            var query = new SqlCommand("DELETE FROM Sector WHERE TrackFk = @track AND SectorPK = @sector");
            query.Parameters.AddWithValue("@track", track.Number);
            query.Parameters.AddWithValue("@sector", sector.Number + 1);
            DatabaseHandler.GetData(query);
        }

        public void WipeTramFromSectorByTramId(int id)
        {
            var query = new SqlCommand("UPDATE Sector SET Status = 0 WHERE TramFk = @id");
            query.Parameters.AddWithValue("@id", id);
            DatabaseHandler.GetData(query);

            query = new SqlCommand("UPDATE Sector SET TramFk = null WHERE TramFk = @id");
            query.Parameters.AddWithValue("@id", id);
            DatabaseHandler.GetData(query);
        }

        public void WipeTramsFromSectors()
        {
            DatabaseHandler.GetData(new SqlCommand("UPDATE Sector SET TramFk = null"));
            DatabaseHandler.GetData(new SqlCommand("UPDATE Sector SET Status = 0 WHERE Status = 2"));
        }
    }
}