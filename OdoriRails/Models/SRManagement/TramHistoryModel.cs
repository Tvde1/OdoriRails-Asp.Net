using System.Collections.Generic;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models.SRManagement
{
    public class TramHistoryModel : BaseModel
    {
        private readonly SchoonmaakReparatieRepository _repository = new SchoonmaakReparatieRepository();

        public int TramId { get; set; } = -1;
        public List<Cleaning> Cleans { get; set; }
        public List<Repair> Repairs { get; set; }

        public void GetServices()
        {
            //if (TramId == -1) throw new Exception("Tram is null?!");
            Cleans = _repository.GetAllCleaningsFromTram(TramId);
            Repairs = _repository.GetAllRepairsFromTram(TramId);
        }
    }
}