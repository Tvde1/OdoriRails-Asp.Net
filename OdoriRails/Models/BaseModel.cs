using OdoriRails.Helpers;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models
{
    public abstract class BaseModel
    {
        //Elk model heeft deze properties
        public User User { get; set; }
        public string ControllerNameForHomeButton { get; set; }

        public BaseModel(User user, string controllerNameForHomeButton)
        {
            User = user;
            ControllerNameForHomeButton = controllerNameForHomeButton;
        }

        public BaseModel()
        {
            
        }

        public string Error { get; set; }
        public string Sucess { get; set; }
        public string Warning { get; set; }
    }
}