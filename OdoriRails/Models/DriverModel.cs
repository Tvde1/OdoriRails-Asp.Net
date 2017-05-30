using System.ComponentModel.DataAnnotations;
using OdoriRails.Helpers.Objects;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Driver;

namespace OdoriRails.Models
{
    public class DriverModel : BaseModel
    {
        public InUitRitTram Tram { get; private set; }
        private readonly InUitrijRepository _inUitrijRepository = new InUitrijRepository();
        
        public bool NeedsCleaning;
        public bool NeedsRepair;
        public string Comments;

        public DriverModel(User user)
        {
            User = user;
            var tempTram = _inUitrijRepository.GetTramByDriver(User);
            Tram = tempTram == null ? null : InUitRitTram.ToInUitRitTram(tempTram);
        }

        public string GetAssignedTramLocation()
        {
            var sector = _inUitrijRepository.GetAssignedSector(Tram);
            return sector != null ? $"Track: {sector.TrackNumber}, Sector: {sector.Number + 1}" : null;
        }

        public void AddRepair(string defect)
        {
            var repair = new Repair(Tram.Number, defect);
            _inUitrijRepository.AddRepair(repair);
        }

        public void AddCleaning(string comment)
        {
            var cleaning = new Cleaning(Tram.Number, comment);
            _inUitrijRepository.AddCleaning(cleaning);
        }

        public void UpdateTram()
        {
            _inUitrijRepository.EditTram(Tram);
        }

        public void FetchTramUpdates()
        {
            Tram = InUitRitTram.ToInUitRitTram(_inUitrijRepository.FetchTram(Tram));
        }
    }
}