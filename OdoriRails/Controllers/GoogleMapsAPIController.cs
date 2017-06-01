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
            var _repository = new ApiRepository();

            var trams = _repository.GetAllTrams();

            //Vul list met sectoren
            var list = new List<ApiTram>();
            foreach (var tram in trams)
            {
                var tramData = _repository.GetTrackFromTram(tram);
                list.Add(new ApiTram(tram.Number, tramData.Key?.Number, tramData.Value.Number, tramData.Value.Latitude, tramData.Value.Longitude, tramData.Key?.Type, tram.Line));
            }
            return list;
        }
    }
}
