﻿using OdoriRails.Helpers.Objects;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Driver;

namespace OdoriRails.Models
{
    public class DriverModel : BaseModel
    {
        public InUitRitTram Tram { get; private set; }
        private InUitrijRepository _inUitrijRepository = new InUitrijRepository();

        public DriverModel(User user)
        {
            User = user;
            var tempTram = _inUitrijRepository.GetTramByDriver(User);
            Tram = tempTram == null ? null : InUitRitTram.ToInUitRitTram(tempTram[0]);
        }

        public string GetAssingedTramLocation()
        {
            Sector sector = _inUitrijRepository.GetAssignedSector(Tram);
            if (sector != null)
            {
                return string.Format("Track: {0}, Sector: {1}", sector.TrackNumber, sector.Number + 1);
            }
            else
            {
                return null;
            }
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