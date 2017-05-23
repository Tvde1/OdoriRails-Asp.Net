using OdoriRails.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdoriRails.Models
{
    public class UserBeheerSysteemModel : BaseModel
    {
        List<User> users { get; set; }
    }
}