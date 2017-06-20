using System.Collections.Generic;
using System.Web.Http;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;

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