using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using OdoriRails.Helpers.Objects;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Driver;

namespace OdoriRails.Models
{
    public class DriverModel : BaseModel
    {
        public InUitRitTram Tram { get; set; }
        private readonly InUitrijRepository _inUitrijRepository = new InUitrijRepository();
        
        public bool NeedsCleaning { get; set; }
        public bool NeedsRepair { get; set; }
        public string Comments { get; set; }

        public DriverModel()
        {
            
        }

        public DriverModel(User user)
        {
            User = user;
            var tempTram = _inUitrijRepository.GetTramByDriver(User);
            Tram = tempTram == null ? null : InUitRitTram.ToInUitRitTram(tempTram);
        }

        public string GetAssignedTramLocation()
        {
            string text;
            switch (Tram.Location)
            {
                case TramLocation.In:
                    var sector = _inUitrijRepository.GetAssignedSector(Tram);
                    text = sector != null ? $"Track: {sector.TrackNumber}, Sector: {sector.Number + 1}" : null;
                    break;
                case TramLocation.ComingIn:
                    text = "Wachten op locatie.";
                    break;
                case TramLocation.Out:
                    text = "Buiten de remise.";
                    break;
                case TramLocation.GoingOut:
                    text = "Aan het vertrekken.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return text;
        }

        public void AddRepair()
        {
            var repair = new Repair(Tram.Number, Comments);
            _inUitrijRepository.AddRepair(repair);
        }

        public void AddCleaning()
        {
            var cleaning = new Cleaning(Tram.Number, Comments);
            _inUitrijRepository.AddCleaning(cleaning);
        }

        public void UpdateTram()
        {
            _inUitrijRepository.EditTram(Tram);
        }

        public void FetchTramUpdates()
        {
            var tempTram = _inUitrijRepository.GetTramByDriver(User);
            Tram = tempTram == null ? null : InUitRitTram.ToInUitRitTram(tempTram);
        }

        public void WaitForLocationUpdate()
        {
            while (true)
            {
                Thread.Sleep(1000);

                FetchTramUpdates();
                if (Tram.Location == TramLocation.In) return;
            }
        }

        public void WaitForStatusOut()
        {
            while (true)
            {
                Thread.Sleep(500);

                var loc = GetTramLocation();
                if (loc == TramLocation.Out) return;
            }
        }

        private TramLocation? GetTramLocation()
        {
            return _inUitrijRepository.GetLocation(Tram);
        }
    }
}