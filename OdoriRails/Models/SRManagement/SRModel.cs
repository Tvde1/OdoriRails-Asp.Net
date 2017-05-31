﻿using System.Collections.Generic;
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

        public List<User> Cleaners { get; set; }
        public List<User> Engineers { get; set; }

        public IEnumerable<int> AssignedWorkers { get; set; }
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

        
    }
}