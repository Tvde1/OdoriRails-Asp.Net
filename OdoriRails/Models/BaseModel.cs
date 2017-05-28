using OdoriRails.Helpers.Objects;

namespace OdoriRails.Models
{
    public abstract class BaseModel
    {
        public BaseModel(User user, string controllerNameForHomeButton)
        {
            User = user;
            ControllerNameForHomeButton = controllerNameForHomeButton;
        }

        public BaseModel()
        {
        }

        //Elk model heeft deze properties
        public User User { get; set; }

        public string ControllerNameForHomeButton { get; set; }

        public string Error { get; set; }
        public string Sucess { get; set; }
        public string Warning { get; set; }

        protected void CopyBaseModel(BaseModel model)
        {
            User = model.User;
            ControllerNameForHomeButton = model.ControllerNameForHomeButton;
            Error = model.Error;
            Sucess = model.Sucess;
            Warning = model.Warning;
        }
    }
}