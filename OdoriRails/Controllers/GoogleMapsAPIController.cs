using OdoriRails.Helpers.Objects;
using OdoriRails.Models;
using OdoriRails.Models.LogistiekBeheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OdoriRails.Controllers
{
    public class GoogleMapsAPIController : ApiController
    {
        public List<Sector> Get()
        {
            List<Sector> sectorList = new List<Sector>();
            //Vul list met sectoren
            return sectorList;
        }
    }
}
