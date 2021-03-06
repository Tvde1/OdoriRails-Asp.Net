﻿using System.Collections.Generic;
using System.Linq;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models.SRManagement
{
    public class SRLogic : BaseModel
    {
        //needs massive cleanup
        private readonly SchoonmaakReparatieRepository _repository = new SchoonmaakReparatieRepository();

        public SRLogic()
        {
            ControllerNameForHomeButton = "SRController";
            //paraterless consdrugdor
        }

        public SRLogic(Role role) : this()
        {
            if (role == Role.Cleaner)
                AssignedWorkers = GetAllCleaners().ToDictionary(x => x.Name, x => false);
            if (role == Role.Engineer)
                AssignedWorkers = GetAllEngineers().ToDictionary(x => x.Name, x => false);
        }

        public List<Cleaning> Cleans { get; set; }
        public List<Repair> Repairs { get; set; }

        public Dictionary<string, bool> AssignedWorkers { get; set; }

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
            var clist = _repository.GetAllUsersWithFunction(Role.Cleaner);
            clist.AddRange(_repository.GetAllUsersWithFunction(Role.HeadCleaner));
            return clist;
        }

        public List<User> GetAllEngineers()
        {
            var rlist = _repository.GetAllUsersWithFunction(Role.Engineer);
            rlist.AddRange(_repository.GetAllUsersWithFunction(Role.HeadEngineer));
            return rlist;
        }

        public List<Repair> RepairListFromUser()
        {
            var replist = new List<Repair>();
            replist = _repository.GetAllRepairsFromUser(User);
            return replist;
        }

        public List<Cleaning> CleaningListFromUser()
        {
            var cleanlist = new List<Cleaning>();
            cleanlist = _repository.GetAllCleansFromUser(User);
            return cleanlist;
        }
    }
}