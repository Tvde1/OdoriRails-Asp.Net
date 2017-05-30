using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OdoriRails.Controllers;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;


namespace OdoriRails.Models.SRManagement
{
    public class MainMenuModel
    {
        private readonly SchoonmaakReparatieRepository _repository = new SchoonmaakReparatieRepository();
        private User LoggedInUser; // test purposes
        public List<Repair> RepairList()
        {
            List<Repair> replist = new List<Repair>();
            replist = _repository.GetAllRepairsFromUser(LoggedInUser);
            return replist;
        }
        
        public List<Cleaning> CleaningList { get; set; }
    }
}