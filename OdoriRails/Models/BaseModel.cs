using OdoriRails.Helpers;

namespace OdoriRails.Models
{
    public abstract class BaseModel
    {
        //Elk model heeft deze
        public User User { get; set; }
        public string Error { get; set; }
        public string Sucess { get; set; }
        public string Warning { get; set; }
    }
}