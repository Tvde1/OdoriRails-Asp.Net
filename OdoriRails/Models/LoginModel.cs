using System.ComponentModel.DataAnnotations;

namespace OdoriRails.Models
{
    public class LoginModel : BaseModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}