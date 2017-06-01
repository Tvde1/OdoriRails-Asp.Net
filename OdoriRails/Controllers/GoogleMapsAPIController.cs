using OdoriRails.Helpers.Objects;
using OdoriRails.Models;
using OdoriRails.Models.LogistiekBeheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OdoriRails.Helpers.DAL.Repository;

namespace OdoriRails.Controllers
{
    public class GoogleMapsAPIController : ApiController 
    {
        public List<ApiTram> Get()
        {
            var repository = new ApiRepository();

            var trams = repository.GetAllTrams();

            var list = new List<ApiTram>();
            foreach (var tram in trams)
            {
                var tramData = repository.GetTrackFromTram(tram);
                if (tramData.Key == null) continue;
                list.Add(new ApiTram(tram.Number, tramData.Key?.Number, tramData.Value?.Number, tramData.Value?.Latitude, tramData.Value?.Longitude, (int?)tramData.Key?.Type, tram.Line));
            }
            return list;
        }
    }
}
