using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models.SRManagement
{
    public class MarkAsDoneViewModel : BaseModel
    {
        public User sessionuser { get; set; }
        public Cleaning CleaningMarkAsDone { get; set; }
        public Repair RepairMarkAsDone { get; set; }

        public string Comment { get; set; }
        public string Solution { get; set; }
        public int Serviceid { get; set; }
        public int TramIdtoCarryOver { get; set; } // used to carry over from view to model
    }
}