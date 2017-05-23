using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using OdoriRails.Helpers;

namespace OdoriRails.Models
{
    public abstract class BaseModel
    {
        //Elk model heeft deze
        
        public string Error { get; set; }
        public string Sucess { get; set; }
        public string Warning { get; set; }
    }
}