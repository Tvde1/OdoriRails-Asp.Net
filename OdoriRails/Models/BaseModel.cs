using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using OdoriRails.Helpers;

namespace OdoriRails.Models
{
    public class BaseModel
    {
        //Elk model heeft deze

        public string ErrorMessage { get; set; }
        public User User { get; set; }
    }
}