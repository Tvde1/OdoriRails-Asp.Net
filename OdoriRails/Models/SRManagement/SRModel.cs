using System;
using System.Collections.Generic;
using System.Linq;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;


namespace OdoriRails.Models.SRManagement
{
    public class SRModel : BaseModel
    {
        private readonly SchoonmaakReparatieRepository _repository = new SchoonmaakReparatieRepository();
        public List<Cleaning> Cleans { get; set; }
        public List<Repair> Repairs { get; set; }

        public Repair RepairToEdit { get; set; }
        public Cleaning CleaningToEdit { get; set; }

        public Dictionary<string, bool> AssignedWorkers { get; set; }
        public List<User> Cleaners { get; set; }
        public List<User> Engineers { get; set; }

        public SRModel()
        {
            ControllerNameForHomeButton = "SRController";
            //paraterless consdrugdor
        }
        public SRModel(Role role) : this()
        {
            
            if (role == Role.Cleaner)
            {
                AssignedWorkers = GetAllCleaners().ToDictionary(x => x.Name, x => false);
            }
            if (role == Role.Engineer)
            {
                AssignedWorkers = GetAllEngineers().ToDictionary(x => x.Name, x => false);
            }

        }
        
        public Repair GetRepairToEdit(int id)
        {
            var rlist = _repository.GetRepairFromId(id);
            return rlist.ElementAt(0);
        }
        public Cleaning GetCleaningToEdit(int id)
        {
            var clist = _repository.GetCleanFromId(id);
            return clist.ElementAt(0);
        }
        public List<User> GetAllCleaners()
        {     
            var clist =_repository.GetAllUsersWithFunction(Role.Cleaner);
            return clist;
        }
        public List<User> GetAllEngineers()
        {
            var rlist = _repository.GetAllUsersWithFunction(Role.Engineer);
            return rlist;
        }

        public List<Repair> RepairListFromUser()
        {
            List<Repair> replist = new List<Repair>();
            replist = _repository.GetAllRepairsFromUser(User);
            return replist;
        }

        public List<Cleaning> CleaningListFromUser()
        {
            List<Cleaning> cleanlist = new List<Cleaning>();
            cleanlist = _repository.GetAllCleansFromUser(User);
            return cleanlist;
        }

        public void EditCleaningInDb(Cleaning cleaning)
        {
            _repository.EditService(cleaning);
        }
        public void EditRepairInDb(Repair repair)
        {
            _repository.EditService(repair);
        }

    }
}