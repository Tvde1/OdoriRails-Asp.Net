using System;
using OdoriRails.Helpers.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using OdoriRails.Helpers.DAL.Repository;

namespace OdoriRails.Controllers
{
    public class GoogleMapsAPIController : ApiController 
    {
        public List<Track> Get()
        {
            var repository = new ApiRepository();
            
            return repository.GetAllTracks();
        }
    }
}
