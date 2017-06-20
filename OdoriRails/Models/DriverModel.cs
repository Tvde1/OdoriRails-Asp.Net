using System;
using System.Threading;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Driver;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models
{
    public class DriverModel : BaseModel
    {
        private readonly InUitrijRepository _inUitrijRepository = new InUitrijRepository();

        public DriverModel()
        {
        }

        public DriverModel(User user)
        {
            User = user;
            var tempTram = _inUitrijRepository.GetTramByDriver(User);
            Tram = tempTram == null ? null : InUitRitTram.ToInUitRitTram(tempTram);
        }

        public InUitRitTram Tram { get; set; }

        public bool NeedsCleaning { get; set; }
        public bool NeedsRepair { get; set; }
        public string Comments { get; set; }

        public string GetAssignedTramLocation()
        {
            string text;
            if (Tram != null)
                switch (Tram.Location)
                {
                    case TramLocation.In:
                        var sector = _inUitrijRepository.GetAssignedSector(Tram);
                        text = sector != null ? $"Track: {sector.TrackNumber}, Sector: {sector.Number + 1}" : null;
                        break;
                    case TramLocation.ComingIn:
                        text = "Waiting for location.";
                        break;
                    case TramLocation.Out:
                        text = "Out of the Remise.";
                        break;
                    case TramLocation.GoingOut:
                        text = "Leaving...";
                        break;
                    case TramLocation.NotAssigned:
                        text = "Not assigned.";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            else
                text = "Nog geen tram toegewezen.";

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